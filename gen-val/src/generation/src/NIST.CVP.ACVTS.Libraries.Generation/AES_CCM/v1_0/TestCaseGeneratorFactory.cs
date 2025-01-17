﻿using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.InternalTestType.ToLower())
            {
                case "ecma-aft":
                    return new TestCaseGeneratorEcma(_oracle);
                case "ecma-vadt":
                    return new TestCaseGeneratorEcmaVadt(_oracle);
                case "802.11":
                    return new TestCaseGenerator80211();
                default:
                    return new TestCaseGenerator(_oracle);
            }
        }
    }
}
