﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1
{
    public abstract class KasAftTestGeneratorBase<
        TKasAftParameters, TKasAftResult,
        TKasDsaAlgoAttributes, TDomainParameters, TKeyPair, TScheme> :
        IKasAftTestGenerator<TKasAftParameters, TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase, new()
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
        where TScheme : struct, IComparable
    {

        protected readonly IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > KasBuilder;
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > SchemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;

        protected KasAftTestGeneratorBase(
            IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > kasBuilder,
            ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder
        )
        {
            KasBuilder = kasBuilder;
            SchemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
        }

        public TKasAftResult GetTest(TKasAftParameters param)
        {
            TKasAftResult testResult = new TKasAftResult();

            KeyAgreementRole serverRole = param.ServerKeyAgreementRole;
            KeyConfirmationRole serverKcRole = param.ServerKeyConfirmationRole;

            var serverKeyNonceRequirements = GetPartyNonceKeyGenRequirements(
                param,
                serverRole,
                serverKcRole
            );

            var entropyProvider = _entropyProviderFactory
                .GetEntropyProvider(EntropyProviderTypes.Testable);

            // If the server has a requirement of generating an DKM nonce, 
            // inject it into the entropy provider
            if (serverKeyNonceRequirements.GeneratesDkmNonce)
            {
                var dkmNonceLength = GetDkmLengthRequirement(param);

                testResult.DkmNonceServer = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(dkmNonceLength);
                entropyProvider.AddEntropy(testResult.DkmNonceServer.GetDeepCopy());
                SchemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // Set up entropy injection when server generates an ephemeral nonce
            if (serverKeyNonceRequirements.GeneratesEphemeralNonce)
            {
                var ephemeralNonceLength = GetEphemeralLengthRequirement(param);

                testResult.EphemeralNonceServer = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(ephemeralNonceLength);
                entropyProvider.AddEntropy(testResult.EphemeralNonceServer.GetDeepCopy());
                SchemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // a nonce is used for KdfNoKc, set up entropy injection
            if (param.KasMode == KasMode.KdfNoKc)
            {
                testResult.NonceNoKc = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(128);

                entropyProvider.AddEntropy(testResult.NonceNoKc.GetDeepCopy());
                SchemeBuilder.WithEntropyProvider(entropyProvider);
            }

            BitString aesCcmNonce = null;
            if ((serverRole == KeyAgreementRole.InitiatorPartyU || param.IsSample) && param.MacType == KeyAgreementMacType.AesCcm)
            {
                aesCcmNonce = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(param.AesCcmNonceLen);

                testResult.NonceAesCcm = aesCcmNonce;
            }

            MacParameters macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(param.MacType)
                .WithMacLength(param.MacLen)
                .WithNonce(aesCcmNonce)
                .Build();

            var serverKas = GetKasInstance(
                serverKeyNonceRequirements,
                serverRole,
                serverKcRole,
                macParameters,
                param,
                testResult,
                param.IdServer
            );

            TDomainParameters domainParameters = GetGroupDomainParameters(param);
            serverKas.SetDomainParameters(domainParameters);

            var serverPublicInfo = serverKas.ReturnPublicInfoThisParty();

            SetTestResultInformationFromKasResult(param, testResult, serverKas, null, null);

            // For sample, we need to generate everything up front so that something's available
            // in the answer files
            if (param.IsSample)
            {
                testResult.Deferred = false;

                SchemeBuilder.WithEntropyProvider(
                    _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                );

                var iutKeyNonceRequirements = GetPartyNonceKeyGenRequirements(
                    param,
                    param.IutKeyAgreementRole,
                    param.IutKeyConfirmationRole
                );

                var entropyProviderSample = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);

                if (iutKeyNonceRequirements.GeneratesDkmNonce)
                {
                    var dkmNonceLength = GetDkmLengthRequirement(param);

                    testResult.DkmNonceIut = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                        .GetEntropy(dkmNonceLength);

                    entropyProviderSample.AddEntropy(testResult.DkmNonceIut.GetDeepCopy());
                    SchemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (iutKeyNonceRequirements.GeneratesEphemeralNonce)
                {
                    var ephemeralNonceLength = GetEphemeralLengthRequirement(param);

                    testResult.EphemeralNonceIut = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                        .GetEntropy(ephemeralNonceLength);

                    entropyProviderSample.AddEntropy(testResult.EphemeralNonceIut.GetDeepCopy());
                    SchemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (param.KasMode == KasMode.KdfNoKc)
                {
                    entropyProviderSample.AddEntropy(testResult.NonceNoKc.GetDeepCopy());
                    SchemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (param.AesCcmNonceLen != 0)
                {
                    testResult.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
                }

                testResult.IdIut = new BitString("a1b2c3d4e5");

                var iutKas = GetKasInstance(
                    iutKeyNonceRequirements,
                    param.IutKeyAgreementRole,
                    param.IutKeyConfirmationRole,
                    macParameters,
                    param,
                    testResult,
                    testResult.IdIut
                );

                var result = iutKas.ComputeResult(serverPublicInfo);

                SetTestResultInformationFromKasResult(param, testResult, serverKas, iutKas, result);
            }

            return testResult;
        }

        /// <summary>
        /// Gets the nonce and key generation requirements for the party, as per the options provided.
        /// </summary>
        /// <param name="param">The test case parameter information.</param>
        /// <param name="partyKeyAgreementRole">The party's key agreement role.</param>
        /// <param name="partyKeyConfirmationRole">The party's key confirmation role.</param>
        /// <returns></returns>
        protected abstract SchemeKeyNonceGenRequirement GetPartyNonceKeyGenRequirements(
            TKasAftParameters param,
            KeyAgreementRole partyKeyAgreementRole,
            KeyConfirmationRole partyKeyConfirmationRole
        );

        /// <summary>
        /// Gets the length requirement for the dkm nonce.
        /// </summary>
        /// <param name="param">The test parameters</param>
        /// <returns></returns>
        protected abstract int GetDkmLengthRequirement(TKasAftParameters param);

        /// <summary>
        /// Gets the length requirement for the ephemeral key/nonce.
        /// </summary>
        /// <param name="param">The test parameters.</param>
        /// <returns></returns>
        protected abstract int GetEphemeralLengthRequirement(TKasAftParameters param);

        /// <summary>
        /// Gets the KAS instance based on the provided parameters.
        /// </summary>
        /// <param name="partyKeyNonceRequirements">The party's key/nonce gen requirements.</param>
        /// <param name="partyRole">The party's role.</param>
        /// <param name="partyKcRole">The party's key confirmation role.</param>
        /// <param name="macParameters">The MAC parameters.</param>
        /// <param name="param">The test parameters.</param>
        /// <param name="result">The resulting test.</param>
        /// <param name="partyId">The party's identifier.</param>
        /// <returns></returns>
        protected abstract IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
            > GetKasInstance(
                SchemeKeyNonceGenRequirement partyKeyNonceRequirements,
                KeyAgreementRole partyRole,
                KeyConfirmationRole partyKcRole,
                MacParameters macParameters,
                TKasAftParameters param,
                TKasAftResult result,
                BitString partyId
            );

        /// <summary>
        /// Gets the domain parameters from the test parameters.
        /// </summary>
        /// <param name="param">The test parameteres to retrieve the domain parameters from.</param>
        /// <returns></returns>
        protected abstract TDomainParameters GetGroupDomainParameters(TKasAftParameters param);

        /// <summary>
        /// Sets the KAS instance's generated information on the test result.
        /// </summary>
        /// <param name="param">The test parameters.</param>
        /// <param name="result">The test case result in which to have its information set.</param>
        /// <param name="serverKas">The server's instance of KAS.</param>
        /// <param name="iutKas">The IUT's instance of KAS.</param>
        /// <param name="iutResult">the IUT"s result of the key agreement.</param>
        protected abstract void SetTestResultInformationFromKasResult(
            TKasAftParameters param,
            TKasAftResult result,
            IKas<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > serverKas,
            IKas<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > iutKas,
            KasResult iutResult
        );
    }
}
