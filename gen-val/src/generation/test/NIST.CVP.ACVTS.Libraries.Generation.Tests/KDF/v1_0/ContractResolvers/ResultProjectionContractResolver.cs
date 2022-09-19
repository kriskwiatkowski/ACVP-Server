﻿using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.v1_0.ContractResolvers
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class ResultsProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
        private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
        private readonly Projection _projection = Projection.Result;

        private VectorSetSerializer<TestVectorSet, TestGroup, TestCase> _serializer;
        private VectorSetDeserializer<TestVectorSet, TestGroup, TestCase> _deserializer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _serializer =
                new VectorSetSerializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider,
                    _contractResolverFactory
                );
            _deserializer =
                new VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider
                );
        }

        [Test]
        [TestCase(KdfModes.Counter, CounterLocations.None)]
        [TestCase(KdfModes.Feedback, CounterLocations.None)]
        [TestCase(KdfModes.Counter, CounterLocations.AfterFixedData)]
        [TestCase(KdfModes.Feedback, CounterLocations.AfterFixedData)]
        public void ShouldSerializeGroupProperties(KdfModes kdfMode, CounterLocations counterLocation)
        {
            var tvs = TestDataMother.GetTestGroups(1, kdfMode, counterLocation);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
        }

        [Test]
        [TestCase(KdfModes.Counter, CounterLocations.None)]
        [TestCase(KdfModes.Feedback, CounterLocations.None)]
        [TestCase(KdfModes.Counter, CounterLocations.AfterFixedData)]
        [TestCase(KdfModes.Feedback, CounterLocations.AfterFixedData)]
        public void ShouldSerializeEncryptCaseProperties(KdfModes kdfMode, CounterLocations counterLocation)
        {
            var tvs = TestDataMother.GetTestGroups(1, kdfMode, counterLocation);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.KeyOut, newTc.KeyOut, nameof(newTc.KeyOut));
            Assert.AreEqual(tc.FixedData, newTc.FixedData, nameof(newTc.FixedData));

            if (tg.KdfMode == KdfModes.Counter
                && tg.CounterLocation == CounterLocations.MiddleFixedData)
            {
                Assert.AreEqual(tc.BreakLocation, newTc.BreakLocation, nameof(newTc.BreakLocation));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regexTestPassed = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regexTestPassed.Matches(json).Count == 0);
        }
    }
}