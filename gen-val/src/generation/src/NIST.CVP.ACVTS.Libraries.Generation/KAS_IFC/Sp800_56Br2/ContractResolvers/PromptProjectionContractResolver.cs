﻿using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includePropertiesAllScenarios = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.KeyConfirmationDirection),
                nameof(TestGroup.KeyConfirmationRole),
                nameof(TestGroup.KeyGenerationMethod),
                nameof(TestGroup.L),
                nameof(TestGroup.Modulo),
                nameof(TestGroup.Scheme),
                nameof(TestGroup.IutId),
                nameof(TestGroup.ServerId),
                nameof(TestGroup.KasRole),
                nameof(TestGroup.KdfConfiguration),
                nameof(TestGroup.KtsConfiguration),
                nameof(TestGroup.MacConfiguration),
                nameof(TestGroup.Tests),
            };

            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            var includeFixedPublicExponent = new[]
            {
                nameof(TestGroup.PublicExponent)
            };
            var fixedPubExpIncludeWithKeyGens = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_basic,
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor
            };
            if (includeFixedPublicExponent.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);
                        return fixedPubExpIncludeWithKeyGens.Contains(testGroup.KeyGenerationMethod);
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, Cs, nonce, salt from both parties
            // AFT tests - include, public keys, Cs, nonce, salt from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.IutE),
                nameof(TestCase.IutN),
                nameof(TestCase.ServerE),
                nameof(TestCase.ServerN),
                nameof(TestCase.ServerC),
                nameof(TestCase.ServerNonce),
                nameof(TestCase.KdfParameter),
                nameof(TestCase.IutD),
                nameof(TestCase.IutDmp1),
                nameof(TestCase.IutDmq1),
                nameof(TestCase.IutIqmp),
                nameof(TestCase.IutP),
                nameof(TestCase.IutQ),
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            var ktsParameterProperty = new[]
            {
                nameof(TestCase.KtsParameter)
            };
            if (ktsParameterProperty.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testCase.KtsParameter != null)
                            return testCase.KtsParameter.ShouldSerialize;

                        return false;
                    };
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                // nameof(TestCase.ServerD),
                // nameof(TestCase.ServerDmp1),
                // nameof(TestCase.ServerDmq1),
                // nameof(TestCase.ServerIqmp),
                nameof(TestCase.IutC),
                nameof(TestCase.IutZ),
                nameof(TestCase.IutNonce),
                nameof(TestCase.Tag),
                nameof(TestCase.Dkm)
            };

            if (includePropertiesValScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var includePropertiesValNonBasicKey = new[]
            {
                nameof(TestCase.ServerP),
                nameof(TestCase.ServerQ),
            };
            var keyGenMethodsToIncludePq = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor,
                IfcKeyGenerationMethod.RsaKpg2_crt,
                IfcKeyGenerationMethod.RsaKpg2_primeFactor,
            };

            if (includePropertiesValNonBasicKey.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase) && keyGenMethodsToIncludePq.Contains(testGroup.KeyGenerationMethod))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
            #endregion Conditional Test Case properties
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            #region KdfConfigurations
            if (typeof(OneStepConfiguration).IsAssignableFrom(type))
            {
                return OneStepConfigurationSerialization(jsonProperty);
            }

            if (typeof(OneStepNoCounterConfiguration).IsAssignableFrom(type))
            {
                return OneStepNoCounterConfigurationSerialization(jsonProperty);
            }

            if (typeof(TwoStepConfiguration).IsAssignableFrom(type))
            {
                return TwoStepConfigurationSerialization(jsonProperty);
            }

            if (typeof(IkeV1Configuration).IsAssignableFrom(type))
            {
                return IkeV1ConfigurationSerialization(jsonProperty);
            }

            if (typeof(IkeV2Configuration).IsAssignableFrom(type))
            {
                return IkeV2ConfigurationSerialization(jsonProperty);
            }

            if (typeof(Tls10_11Configuration).IsAssignableFrom(type))
            {
                return Tls10_11ConfigurationSerialization(jsonProperty);
            }

            if (typeof(Tls12Configuration).IsAssignableFrom(type))
            {
                return Tls12ConfigurationSerialization(jsonProperty);
            }
            #endregion KdfConfigurations

            #region KdfParameters
            if (typeof(KdfParameterOneStep).IsAssignableFrom(type))
            {
                return KdfParameterOneStepSerialization(jsonProperty);
            }

            if (typeof(KdfParameterOneStepNoCounter).IsAssignableFrom(type))
            {
                return KdfParameterOneStepNoCounterSerialization(jsonProperty);
            }

            if (typeof(KdfParameterTwoStep).IsAssignableFrom(type))
            {
                return KdfParameterTwoStepSerialization(jsonProperty);
            }

            if (typeof(KdfParameterIkeV1).IsAssignableFrom(type))
            {
                return KdfParameterIkeV1Serialization(jsonProperty);
            }

            if (typeof(KdfParameterIkeV2).IsAssignableFrom(type))
            {
                return KdfParameterIkeV2Serialization(jsonProperty);
            }

            if (typeof(KdfParameterTls10_11).IsAssignableFrom(type))
            {
                return KdfParameterTls10_11Serialization(jsonProperty);
            }

            if (typeof(KdfParameterTls12).IsAssignableFrom(type))
            {
                return KdfParameterTls12Serialization(jsonProperty);
            }
            #endregion KdfParameters

            return jsonProperty.ShouldSerialize;
        }

        #region KdfConfiguration
        private Predicate<object> OneStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(OneStepConfiguration.KdfType),
                nameof(OneStepConfiguration.AuxFunction),
                nameof(OneStepConfiguration.FixedInfoEncoding),
                nameof(OneStepConfiguration.FixedInfoPattern),
                nameof(OneStepConfiguration.SaltMethod),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> OneStepNoCounterConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(OneStepNoCounterConfiguration.KdfType),
                nameof(OneStepNoCounterConfiguration.AuxFunction),
                nameof(OneStepNoCounterConfiguration.FixedInfoEncoding),
                nameof(OneStepNoCounterConfiguration.FixedInfoPattern),
                nameof(OneStepNoCounterConfiguration.SaltMethod),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> TwoStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TwoStepConfiguration.KdfType),
                nameof(TwoStepConfiguration.KdfMode),
                nameof(TwoStepConfiguration.FixedInfoEncoding),
                nameof(TwoStepConfiguration.FixedInfoPattern),
                nameof(TwoStepConfiguration.SaltMethod),
                nameof(TwoStepConfiguration.CounterLen),
                nameof(TwoStepConfiguration.CounterLocation),
                nameof(TwoStepConfiguration.IvLen),
                nameof(TwoStepConfiguration.MacMode),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> IkeV1ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(IkeV1Configuration.KdfType),
                nameof(IkeV1Configuration.HashFunction),
                nameof(IkeV1Configuration.RequiresAdditionalNoncePair),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> IkeV2ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(IkeV2Configuration.KdfType),
                nameof(IkeV2Configuration.HashFunction),
                nameof(IkeV2Configuration.RequiresAdditionalNoncePair),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> Tls10_11ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(Tls10_11Configuration.KdfType),
                nameof(Tls10_11Configuration.RequiresAdditionalNoncePair),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> Tls12ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(Tls12Configuration.KdfType),
                nameof(Tls12Configuration.RequiresAdditionalNoncePair),
                nameof(Tls12Configuration.HashFunction),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        #endregion KdfConfiguration

        #region KdfParameter
        private Predicate<object> KdfParameterOneStepSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterOneStep.KdfType),
                nameof(KdfParameterOneStep.Context),
                nameof(KdfParameterOneStep.AlgorithmId),
                nameof(KdfParameterOneStep.Iv),
                nameof(KdfParameterOneStep.Label),
                nameof(KdfParameterOneStep.Salt),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterOneStepNoCounterSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterOneStepNoCounter.KdfType),
                nameof(KdfParameterOneStepNoCounter.Context),
                nameof(KdfParameterOneStepNoCounter.AlgorithmId),
                nameof(KdfParameterOneStepNoCounter.Iv),
                nameof(KdfParameterOneStepNoCounter.Label),
                nameof(KdfParameterOneStepNoCounter.Salt),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterTwoStepSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterTwoStep.KdfType),
                nameof(KdfParameterTwoStep.Context),
                nameof(KdfParameterTwoStep.AlgorithmId),
                nameof(KdfParameterTwoStep.Iv),
                nameof(KdfParameterTwoStep.Label),
                nameof(KdfParameterTwoStep.Salt),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterIkeV1Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterIkeV1.KdfType),
                nameof(KdfParameterIkeV1.AdditionalInitiatorNonce),
                nameof(KdfParameterIkeV1.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterIkeV2Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterIkeV2.KdfType),
                nameof(KdfParameterIkeV2.AdditionalInitiatorNonce),
                nameof(KdfParameterIkeV2.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterTls10_11Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterTls10_11.KdfType),
                nameof(KdfParameterTls10_11.AdditionalInitiatorNonce),
                nameof(KdfParameterTls10_11.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterTls12Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterTls10_11.KdfType),
                nameof(KdfParameterTls10_11.AdditionalInitiatorNonce),
                nameof(KdfParameterTls10_11.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        #endregion KdfParameter
    }
}