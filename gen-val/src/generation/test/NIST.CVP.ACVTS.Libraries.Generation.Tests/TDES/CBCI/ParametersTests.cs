﻿using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBCI
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "TDES-CBCI",
                Direction = new[] { "Encrypt" },
                IsSample = false
            };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "TDES-CBCI",
                Direction = new[] { "Encrypt" },
                IsSample = false
            };
            Assert.That(parameters != null);
            Assert.AreEqual("TDES-CBCI", parameters.Algorithm);
        }
    }
}