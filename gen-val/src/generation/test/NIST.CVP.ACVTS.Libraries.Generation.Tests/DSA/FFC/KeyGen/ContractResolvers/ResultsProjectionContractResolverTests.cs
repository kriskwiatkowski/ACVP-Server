﻿using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen.ContractResolvers
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
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeGroupProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            if (isSample)
            {
                Assert.AreEqual(tg.P.ToPositiveBigInteger(), newTg.P.ToPositiveBigInteger(), nameof(newTg.P));
                Assert.AreEqual(tg.Q.ToPositiveBigInteger(), newTg.Q.ToPositiveBigInteger(), nameof(newTg.Q));
                Assert.AreEqual(tg.G.ToPositiveBigInteger(), newTg.G.ToPositiveBigInteger(), nameof(newTg.G));
            }
            else
            {
                Assert.IsNull(newTg.P, nameof(newTg.P));
                Assert.IsNull(newTg.Q, nameof(newTg.Q));
                Assert.IsNull(newTg.G, nameof(newTg.G));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCaseProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.AreEqual(tc.X.ToPositiveBigInteger(), newTc.X.ToPositiveBigInteger(), nameof(newTc.X));
                Assert.AreEqual(tc.Y.ToPositiveBigInteger(), newTc.Y.ToPositiveBigInteger(), nameof(newTc.Y));
            }
            else
            {
                Assert.IsNull(newTc.X, nameof(newTc.X));
                Assert.IsNull(newTc.Y, nameof(newTc.Y));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
