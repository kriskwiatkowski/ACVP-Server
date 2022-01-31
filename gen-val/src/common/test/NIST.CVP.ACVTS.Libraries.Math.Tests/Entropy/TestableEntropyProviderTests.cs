﻿using System;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Entropy
{
    [TestFixture, UnitTest]
    public class TestableEntropyProviderTests
    {
        [SetUp]
        public void Setup()
        {
            _subject = new TestableEntropyProvider();
        }

        private TestableEntropyProvider _subject;

        [Test]
        public void ShouldThrowExceptionWhenNoEntropy()
        {
            Assert.Throws(typeof(Exception), () => _subject.GetEntropy(0));
        }

        [Test]
        public void ShouldThrowExceptionWhenExhaustedEntropy()
        {
            _subject.AddEntropy(new BitString(1));
            _subject.AddEntropy(new BitString(2));

            _subject.GetEntropy(1);
            _subject.GetEntropy(2);
            Assert.Throws(typeof(Exception), () => _subject.GetEntropy(0));
        }

        [Test]
        public void ShouldThrowExceptionWhenEntropyLengthNotLargeEnough()
        {
            _subject.AddEntropy(new BitString(10));
            Assert.Throws(typeof(ArgumentException), () => _subject.GetEntropy(15));
        }

        [Test]
        public void ShouldReturnEntropyFirstInFirstOut()
        {
            _subject.AddEntropy(new BitString(1));
            _subject.AddEntropy(new BitString(2));

            var first = _subject.GetEntropy(1);
            _subject.AddEntropy(new BitString(3));
            var second = _subject.GetEntropy(2);
            var third = _subject.GetEntropy(3);

            Assert.AreEqual(1, first.BitLength, nameof(first));
            Assert.AreEqual(2, second.BitLength, nameof(second));
            Assert.AreEqual(3, third.BitLength, nameof(third));
        }
    }
}