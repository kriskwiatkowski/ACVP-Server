﻿using System.Numerics;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC.Tests
{
    [TestFixture, LongCryptoTest]
    public class EccDsaTests
    {
        [Test]
        #region KeyPairGen-P
        [TestCase(Curve.P192,
            "e5ce89a34adddf25ff3bf1ffe6803f57d0220de3118798ea",
            "8abf7b3ceb2b02438af19543d3e5b1d573fa9ac60085840f",
            "a87f80182dcd56a6a061f81f7da393e7cffd5e0738c6b245",
            TestName = "KeyGen p-192")]
        [TestCase(Curve.P224,
            "e7c92383846a4e6887a10498d8eaca2bd0487d985bd7d3f92ce3ab30",
            "0a3682d2aaa4dd931bee042d32e95755507ab164b12f84843f4b7b96",
            "a6313a938eff7a293222e0e3c7b4c6132489b33255a61c3fc1ce2256",
            TestName = "KeyGen p-224")]
        [TestCase(Curve.P256,
            "c9806898a0334916c860748880a541f093b579a9b1f32934d86c363c39800357",
            "d0720dc691aa80096ba32fed1cb97c2b620690d06de0317b8618d5ce65eb728f",
            "9681b517b1cda17d0d83d335d9c4a8a9a9b0b1b3c7106d8f3c72bc5093dc275f",
            TestName = "KeyGen p-256")]
        [TestCase(Curve.P384,
            "5394f7973ea868c52bf3ff8d8ceeb4db90a683653b12485d5f627c3ce5abd8978fc9673d14a71d925747931662493c37",
            "fd3c84e5689bed270e601b3d80f90d67a9ae451cce890f53e583229ad0e2ee645611fa9936dfa45306ec18066774aa24",
            "b83ca4126cfc4c4d1d18a4b6c21c7f699d5123dd9c24f66f833846eeb58296196b42ec06425db5b70a4b81b7fcf705a0",
            TestName = "KeyGen p-384")]
        [TestCase(Curve.P521,
            "0184258ea667ab99d09d4363b3f51384fc0acd2f3b66258ef31203ed30363fcda7661b6a817daaf831415a1f21cb1cda3a74cc1865f2ef40f683c14174ea72803cff",
            "019ee818048f86ada6db866b7e49a9b535750c3673cb61bbfe5585c2df263860fe4d8aa8f7486aed5ea2a4d733e346eaefa87ac515c78b9a986ee861584926ce4860",
            "01b6809c89c0aa7fb057a32acbb9ab4d7b06ba39dba8833b9b54424add2956e95fe48b7fbf60c3df5172bf386f2505f1e1bb2893da3b96d4f5ae78f2544881a238f7",
            TestName = "KeyGen p-521")]
        #endregion KeyPairGen-P
        #region KeyPairGen-B
        [TestCase(Curve.B163,
            "025d594310681b01fd63333cdd4315e54e18fe2623",
            "7e7162c48dcab690aa9ef76d2ed066cedae33364",
            "8cc32f4b5a88985c6e0c418e4abe988d5375371d",
            TestName = "KeyGen b-163")]
        [TestCase(Curve.B233,
            "1e0da3dca621aab89a54e9528937ca7567464e6e783357878c1ecef15c",
            "bf1e4d6ad911b7d4cfdfc990132b1e23bd279f4692bbac82e9e8b80dd4",
            "6c2a7599c395b8cc01b29b33ad6808361a7417d0dd7bd478a4a4783446",
            TestName = "KeyGen b-233")]
        [TestCase(Curve.B283,
            "10d57c6f40baac97852771cee44a04137fb0ae504df7d6bb4153e5f13678f511520d47",
            "05c555fecdea33c76bbc3498a2cf3f64eda57f3bedc9579439162a736953d25d16ffb6a3",
            "8808d8babe945f2f0040f70c9f10714b8852179314d17f8f1cef8164fe5d1705e33eff",
            TestName = "KeyGen b-283")]
        [TestCase(Curve.B409,
            "ebd71c6f6a42bb485480526d916977665df53c198dbd027e2a36ddd4e1178bed069ca6758d0069098301e9ef89dc545ce9c691",
            "01fc79d655eb2f07e8127fb0857de31fadb25afc04ea340fa448d669439e7519a3487c7601875d1f3431d3707a5a36de3532408d",
            "019a4dae9a205ead5fc6dac8b84f7b8846667b1853d02bfd696115f266b380b5be63eb684a46fb3536f9c44ac33cb5aa32000246",
            TestName = "KeyGen b-409")]
        [TestCase(Curve.B571,
            "01443e93c7ef6802655f641ecbe95e75f1f15b02d2e172f49a32e22047d5c00ebe1b3ff0456374461360667dbf07bc67f7d6135ee0d1d46a226a530fefe8ebf3b926e9fbad8d57a6",
            "053e3710d8e7d4138db0a369c97e5332c1be38a20a4a84c36f5e55ea9fd6f34545b864ea64f319e74b5ee9e4e1fa1b7c5b2db0e52467518f8c45b658824871d5d4025a6320ca06f8",
            "03a22cfd370c4a449b936ae97ab97aab11c57686cca99d14ef184f9417fad8bedae4df8357e3710bcda1833b30e297d4bf637938b995d231e557d13f062e81e830af5ab052208ead",
            TestName = "KeyGen b-571")]
        #endregion KeyPairGen-B
        #region KeyPairGen-K
        [TestCase(Curve.K163,
            "028a7447f95b43c072722ee52f2a68897518830272",
            "072dadf24b00f9a2a0ad6fbfb9d86181e939900174",
            "04bc1d4987dde0d2f633df16d686e2a78d6d3f49f3",
            TestName = "KeyGen k-163")]
        [TestCase(Curve.K233,
            "01da7422b50e3ff051f2aaaed10acea6cbf6110c517da2f4eaca8b5b87",
            "01c7475da9a161e4b3f7d6b086494063543a979e34b8d7ac44204d47bf9f",
            "0131cbd433f112871cc175943991b6a1350bf0cdd57ed8c831a2a7710c92",
            TestName = "KeyGen k-233")]
        [TestCase(Curve.K283,
            "01de6fc561ce8c3ec9a7c03a51e0c61204991f8caca8c7b073cd07945ffb22c48c30e5d4",
            "021e41033585949f5bf30a73d935c580946c3f15b942b42b54e3397fc4115ee96bbbcff0",
            "050789e0c1dacaebb72d7fe27081b2048a8fac3a58693e52807b8c346930b5c4deb549cb",
            TestName = "KeyGen k-283")]
        [TestCase(Curve.K409,
            "0190c5a00374cc3254fdd421c8e52b0cb0f00317bbfb4153195eb6195557989b8e78b27df35c8f47bb4b4ee4608ea04f2adb72",
            "415d296d3d421801dd4ef870cdd234220af52c896f2d8e70c368622167655d45ab7db524552f7aeb9c1159bcac10f24b9b1864",
            "0f824d69ec629e2dabd323cfc93992f253c901ada1427967e591ca0e0970ae7ed35e252159255a3bdbf21d09b0c7bfeb72626a",
            TestName = "KeyGen k-409")]
        [TestCase(Curve.K571,
            "4b7223994f77708dbefe1e76fedb6279710b8769933f87d12d4304bac646fc453055632beb70f87c6bcf6f28fcccba25088789d1f15013f25320ff09321e921eb3e66b0829e87c",
            "023691a3028fc2ea92f707f13c61953ebf411a247739f225f21878fa786e416c5aac32a5d73368bf3ca350f1e05022d17093dc318b42e5fa7234e32f959f20146da2165db36230c0",
            "fd2635485e32d637bfd8f53ff600b9b2bcc6d79884be54dc50103e25c460d41c8d502d7927bb19adfb2cd59a83ec92f4186ac5c75014d3946f4a2a725d3324f6dc206197d19d79",
            TestName = "KeyGen k-571")]
        #endregion KeyPairGen-K
        public void ShouldGenerateKeyPairsCorrectly(Curve curveEnum, string dHex, string qXHex, string qYHex)
        {
            var d = LoadValue(dHex);
            var qX = LoadValue(qXHex);
            var qY = LoadValue(qYHex);

            var Q = new EccPoint(qX, qY);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var domainParams = new EccDomainParameters(curve);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(d);

            var subject = new EccDsa(entropyProvider);

            var result = subject.GenerateKeyPair(domainParams);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.KeyPair.PrivateD, d, "d");
            Assert.AreEqual(result.KeyPair.PublicQ.X, Q.X, "Qx");
            Assert.AreEqual(result.KeyPair.PublicQ.Y, Q.Y, "Qy");
        }

        [Test]
        #region KeyPairVer-P
        [TestCase(Curve.P192,
            "a9ee43654ce0b400c40b8d4ad9a4a53cac9662c56f3c4bde",
            "f376f44a79c50ac3fdfd25ab684af439b5938bee11a63f21",
            true,
            TestName = "KeyVer p-192 Good Pair")]
        [TestCase(Curve.P192,
            "c07ce28e4c846d7327f0554119ddb7e865fa1dd448ba2b40",
            "33aefa3177b99901d9ab6c12eb0749197420296ccb9d4e4a",
            false,
            TestName = "KeyVer p-192 Bad Pair")]
        [TestCase(Curve.P224,
            "4946a9935f9e416fdaf8bebdbc2ac454db06c6bba64b9d18b2b4f758",
            "2a69cc394e21b913244e01a4c45ff00f1b6d0a63ec2a738955cd1714",
            true,
            TestName = "KeyVer p-224 Good Pair")]
        [TestCase(Curve.P224,
            "7a9369e2173bbf29589bf47e3ae0ccf47df6d2268c2292f906cc9261",
            "011afc53c7c1b085029f53b41fcd5a336bafb35b89d302f2bd04df44e6",
            false,
            TestName = "KeyVer p-224 Bad Pair")]
        [TestCase(Curve.P256,
            "e0f7449c5588f24492c338f2bc8f7865f755b958d48edb0f2d0056e50c3fd5b7",
            "86d7e9255d0f4b6f44fa2cd6f8ba3c0aa828321d6d8cc430ca6284ce1d5b43a0",
            true,
            TestName = "KeyVer p-256 Good Pair")]
        [TestCase(Curve.P256,
            "017875397ae87369365656d490e8ce956911bd97607f2aff41b56f6f3a61989826",
            "980a3c4f61b9692633fbba5ef04c9cb546dd05cdec9fa8428b8849670e2fba92",
            false,
            TestName = "KeyVer p-256 Bad Pair")]
        [TestCase(Curve.P384,
            "6e9c7e92ee23713fabb05d0b50e088eb534fd1e2b257c03304cfa33598f88a07c7e31a13e24707a7057ca2919323058e",
            "a218a485e22eae08c3618cfd73befcfcd13c3f196c08df99d7f79ebffe9f127b896aa0cb36cfdf2fc4818b8cd766f185",
            true,
            TestName = "KeyVer p-384 Good Pair")]
        [TestCase(Curve.P384,
            "e87cc868cdf196471d3fc78c324be2c4a0de8dbde182afea88baa51666f3cc9993eae5f1d60d4aec58894f0357273c48",
            "187219b0adc398c835791798053cc6a0bcc6e43228ac23101ee93dfce0e508be988a55fa495eb93b832064dc035e7720",
            false,
            TestName = "KeyVer p-384 Bad Pair")]
        [TestCase(Curve.P521,
            "00602d4e6955c52cacb2451b8a465d9345703a0a2a723e953156c07524d701f3f5f696c5be70c092210bb163e0d5df75151cf48f7ee0edc360f61cc8a94be560683d",
            "00eb13acba7b5bc7d32626d6499c5906ac73de240dd7766cef84d53525cb98c4dd852ed8dab8c1b440bacacccb8f2d4024c5e6a3de80840803a0bc11e5750b53c878",
            true,
            TestName = "KeyVer p-521 Good Pair")]
        [TestCase(Curve.P521,
            "00bec1326722dd943f750fda964cb485f31469095e5b4b5c4fa02041e2f443693ac36c09f371aea7d501881fc4e369f68e071bb14c5003b31dce504bd14338eb8104",
            "036cd502b0a4e91e1080a5b94886a819a823687693ce878408b59183730c69e5ab2d6e05ea5f3d32855cf8c7c20da59a28913025a1fa6835a3751ec6da69502f0547",
            false,
            TestName = "KeyVer p-521 Bad Pair")]
        #endregion KeyPairVer-P
        #region KeyPairVer-K
        [TestCase(Curve.K163,
            "00586a4f27fc0028d3d6704cc7d34ff6f1e8390653",
            "07e5b2d17f8b71a065ff6c44179aaa0b172870c5e9",
            true,
            TestName = "KeyVer k-163 Good Pair")]
        [TestCase(Curve.K163,
            "07a72843139eeae1bbdfeecff6405a98abb9902f49",
            "062c856f2da223dd9f485aa4d44f99e5acb4101439",
            false,
            TestName = "KeyVer k-163 Bad Pair")]
        [TestCase(Curve.K233,
            "01c8004a0e48ed3b1d5cc5269aa4ed8040b8ce65ed9ed85957415c727375",
            "006902b59db2c36fdd56b288237c8fe945a26f46b43c5fe6b72a63955949",
            true,
            TestName = "KeyVer k-233 Good Pair")]
        [TestCase(Curve.K233,
            "01746ae7228af86ab040c01f23e07c5d31cd2812282cad9eecde330517c2",
            "009a1bf94980ce173d5e69992e7d564fa9367de2421d49ce63c00752461d",
            false,
            TestName = "KeyVer k-233 Bad Pair")]
        [TestCase(Curve.K283,
            "00832e0997c96030015d66f9b303792ff06c7e80ccf7d8b17cfa241faaadb3f311d8d72e",
            "008350488707dff338247fbf02cfda762df81aa81771f9f5c37a6afb1f2bf41b8ae1e5da",
            true,
            TestName = "KeyVer k-283 Good Pair")]
        [TestCase(Curve.K283,
            "011540cc626089f66e0ce45ce978d8009db8430e4814c27b75efd0ea5b05b220118a5fd6",
            "0b38e248ad9c561844b717c0e4be9accda6251864c4b159dce275eecaa85d3b7fe1b27be",
            false,
            TestName = "KeyVer k-283 Bad Pair")]
        [TestCase(Curve.K409,
            "01ac8ea71b2e5cec9da5364c46ad98080196dc0f3da9c598b8f91fdc7ca77ec73dfb3a341b659e33e3da31a142db17dd73512e8a",
            "009a91a9a8f1cf782e22d8112b12d22bf26666bbc0dad61c07850c8f8580327b7892c85727cdf5fd8c5bd8ccafdc7a6ec4b8d861",
            true,
            TestName = "KeyVer k-409 Good Pair")]
        [TestCase(Curve.K409,
            "01dd599a9f2e5f8f8b897e65c3699f60239a454fbb415bd3a37ae1bcc3f5836acd75daf516c069fe1c1bf5cf7e5b90a9bae94937",
            "014f9a117846a7cd641450f43f06fc6279efe4d1d3b33c0f50d5ef92a5b459d4328d7d70492ec93988b444d8bc103cdbee074202",
            false,
            TestName = "KeyVer k-409 Bad Pair")]
        [TestCase(Curve.K571,
            "00691836b9d85ec371b6dbee7ffa4c941ee13fd3697c9648d900c313c900d526151fb8c5ba150c97c5a8d501733044be787cb242b62a22d15225e2466f9740c4f9f737bb813b19ce",
            "0743d2b68ba2b922f6dc231a9cb5dd96ff66abc9d6bb1ead15f1db8203f37b5decb7181f925fc8116fa995a9bd587e533c22a406eec60f8ce8c526bceddb6f9c2122a16b4e6e3124",
            true,
            TestName = "KeyVer k-571 Good Pair")]
        [TestCase(Curve.K571,
            "0459d8275f0943fbc433fe6d526ba37402304cdefac9cfd0260b2b985cf6aa4b1ee7f851239006c6f3d17bf6421b4b7e01a6da3c8806eacbd88f6cf5ebe214b61a21303ffacbabc1",
            "031644ba0527badb4db7c23585448cd3da35a042fb5ef02f3d6a4220e026f81a12b0922ae068d3801cccc55a8121386c67c0d7a50b3ceba57ccb56119eff2b221e246163657941ed",
            false,
            TestName = "KeyVer k-571 Bad Pair")]
        #endregion KeyPairVer-K
        #region KeyPairVer-B
        [TestCase(Curve.B163,
            "03a4caa1439ac6d09ef2db6cb7ee135e9f8ad00d06",
            "00011012158e38ece659a3d6f21e7c985030d1c3af",
            true,
            TestName = "KeyVer b-163 Good Pair")]
        [TestCase(Curve.B163,
            "03574c6d7d8c872ba9342758a92f0a8dc3a423449c",
            "046daa0fc26b0e75243acf0eb88f1fad3c634a5210",
            false,
            TestName = "KeyVer b-163 Bad Pair")]
        [TestCase(Curve.B233,
            "014de1fd44d12bb5915981916fb059083f78cdb7f8a91bc04f2df3b4789a",
            "01d20cd76dbc575e5644965c25f44e8ee9f76471e9650421107859c70b9c",
            true,
            TestName = "KeyVer b-233 Good Pair")]
        [TestCase(Curve.B233,
            "00adc181113825aa7e7eba3325897afd9ffccb158ab6bc0ed2ab7fc4c99b",
            "01ad23986e48e39ddf900286e9fd804d6b101c5039bd1eee81c8bc9e7ad7",
            false,
            TestName = "KeyVer b-233 Bad Pair")]
        [TestCase(Curve.B283,
            "043fd538968344d44457519d2efdc7fb55827b1d72338359bb00f652d1099eb5ff8d7636",
            "04d3bd0c8d2c3407a12220fcb0d46f252776f2d01c6a750310caa6353d25ce95a4eb71ce",
            true,
            TestName = "KeyVer b-283 Good Pair")]
        [TestCase(Curve.B283,
            "04cf25c4f75eb0e49a309521151ff857677f68cb7135be63d846eaebea46d6546daf9314",
            "05e8e693e01eedca8932a03ec6607dc59ff7551f7d3cb240bfff23c83814df5ab447bb1c",
            false,
            TestName = "KeyVer b-283 Bad Pair")]
        [TestCase(Curve.B409,
            "0038cea2e0e0853a4f80094ce69717d17581ffdb3f072c5bf0ade9693b2d41a2ee3b1b023eeb8af891d26bada800ea8c762aea91",
            "000ac7de557fff033cd04674b0883d65957a388833991a187ea5e53a9184d4e85cb58e94282d7e381cf26723a0647ff529f2b61d",
            true,
            TestName = "KeyVer b-409 Good Pair")]
        [TestCase(Curve.B409,
            "018ebf4f8eee41fa8554b68731715db8cfa00725052313ffc47a7fbffcda15cae845b98ec74ae2efe4b7fd27c499a720855db97e",
            "0165b6a9b00aaeeebd496f111447f4fc6e770cfff97acdb9973319528db39f0ca691c0b321219270bfcde77d4bcd10eb04134b71",
            false,
            TestName = "KeyVer b-409 Bad Pair")]
        [TestCase(Curve.B571,
            "03620b1ff0a19a5c414fe89ae0c708e0c27e98c70049e0c178515618ba609b44edeba39a678b87a6a3e465dcb332d743ee216398e56ab32805fc0f24a12e73b99c0bf87a6ece885f",
            "051164359081485a21abaf386aa1dbe1e23d98e666ea5e62b7aca8e9c7f73891bf863c51d7b7eab889a78fe4b912122d3926f88d635bc3374591f38b70796bd7fe0f173cd9f82e56",
            true,
            TestName = "KeyVer b-571 Good Pair")]
        [TestCase(Curve.B571,
            "01b7b2ad688b0d859e50064cd88a4fd74f4cec5a6b3c9256785e722775a8ef4930d8dd1f7a7b291b6b75e32eae615accd1e1769bfc4d8bb9a148157a84658057216733927437fc34",
            "0a625c2bfe9a7f3def70d24c1db89dd51120d954edb4bfdcaa1ae1e385ca26fb8181e95db992d8176e129edb2445c2e18fdb26257c4a69458523d605d2955b8b246111af5a805456",
            false,
            TestName = "KeyVer b-571 Bad Pair")]
        #endregion KeyPairVer-B
        public void ShouldValidateKeyPairsCorrectly(Curve curveEnum, string qXHex, string qYHex, bool expectedResult)
        {
            var qX = LoadValue(qXHex);
            var qY = LoadValue(qYHex);
            var Q = new EccPoint(qX, qY);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var domainParams = new EccDomainParameters(curve);
            var keyPair = new EccKeyPair(Q);

            var subject = new EccDsa();

            var result = subject.ValidateKeyPair(domainParams, keyPair);

            Assert.AreEqual(expectedResult, result.Success);
        }

        [Test]
        #region SigGen-P
        [TestCase(Curve.P224, "sha2-224",
            "16797b5c0c7ed5461e2ff1b88e6eafa03c0f46bf072000dfc830d615",
            "699325d6fc8fbbb4981a6ded3c3a54ad2e4e3db8a5669201912064c64e700c139248cdc19495df081c3fc60245b9f25fc9e301b845b3d703a694986e4641ae3c7e5a19e6d6edbf1d61e535f49a8fad5f4ac26397cfec682f161a5fcd32c5e780668b0181a91955157635536a22367308036e2070f544ad4fff3d5122c76fad5d",
            "d9a5a7328117f48b4b8dd8c17dae722e756b3ff64bd29a527137eec0",
            "2fc2cff8cdd4866b1d74e45b07d333af46b7af0888049d0fdbc7b0d6",
            "8d9cc4c8ea93e0fd9d6431b9a1fd99b88f281793396321b11dac41eb",
            TestName = "SigGen p-224 sha2-224")]
        [TestCase(Curve.P224, "sha2-512/224",
            "c1942527afc0f168b2889c131d0cbf713602d47fc2c84c17ac2bbc20",
            "431d15884d1818e35010678adc27323ebec6c766d12906b2171e15fe080955792a2c9406e22584e28ab86b87c3cf1dd10dbf286095cd4e3d4df138c042da68b12b3170ad3d2261cb47198780fac953c0887399667265fcd69ac1a6e1a07f3ce96bb51045061699e6aeb9213b291dbe535969834210f8f7c52614c629c902b768",
            "4fdf891ffa17b242222b5f7a44495d3caa35e40ae55e20fcdc8ce729",
            "1df644ecf2c7ba3ce8724d78adb4760a674d28a2434ad4e2838fe8ac",
            "b2758a7ec69fba63731b84728ad78b1c063af645bcf2177d67f50398",
            TestName = "SigGen p-224 sha2-512/224")]
        [TestCase(Curve.P256, "sha2-256",
            "519b423d715f8b581f4fa8ee59f4771a5b44c8130b4e3eacca54a56dda72b464",
            "5905238877c77421f73e43ee3da6f2d9e2ccad5fc942dcec0cbd25482935faaf416983fe165b1a045ee2bcd2e6dca3bdf46c4310a7461f9a37960ca672d3feb5473e253605fb1ddfd28065b53cb5858a8ad28175bf9bd386a5e471ea7a65c17cc934a9d791e91491eb3754d03799790fe2d308d16146d5c9b0d0debd97d79ce8",
            "94a1bbb14b906a61a280f245f9e93c7f3b4a6247824f5d33b9670787642a68de",
            "f3ac8061b514795b8843e3d6629527ed2afd6b1f6a555a7acabb5e6f79c8c2ac",
            "8bf77819ca05a6b2786c76262bf7371cef97b218e96f175a3ccdda2acc058903",
            TestName = "SigGen p-256 sha2-256")]
        [TestCase(Curve.P384, "sha2-384",
            "201b432d8df14324182d6261db3e4b3f46a8284482d52e370da41e6cbdf45ec2952f5db7ccbce3bc29449f4fb080ac97",
            "6b45d88037392e1371d9fd1cd174e9c1838d11c3d6133dc17e65fa0c485dcca9f52d41b60161246039e42ec784d49400bffdb51459f5de654091301a09378f93464d52118b48d44b30d781eb1dbed09da11fb4c818dbd442d161aba4b9edc79f05e4b7e401651395b53bd8b5bd3f2aaa6a00877fa9b45cadb8e648550b4c6cbe",
            "dcedabf85978e090f733c6e16646fa34df9ded6e5ce28c6676a00f58a25283db8885e16ce5bf97f917c81e1f25c9c771",
            "50835a9251bad008106177ef004b091a1e4235cd0da84fff54542b0ed755c1d6f251609d14ecf18f9e1ddfe69b946e32",
            "0475f3d30c6463b646e8d3bf2455830314611cbde404be518b14464fdb195fdcc92eb222e61f426a4a592c00a6a89721",
            TestName = "SigGen p-384 sha2-384")]
        [TestCase(Curve.P521, "sha2-512",
            "00f749d32704bc533ca82cef0acf103d8f4fba67f08d2678e515ed7db886267ffaf02fab0080dca2359b72f574ccc29a0f218c8655c0cccf9fee6c5e567aa14cb926",
            "9ecd500c60e701404922e58ab20cc002651fdee7cbc9336adda33e4c1088fab1964ecb7904dc6856865d6c8e15041ccf2d5ac302e99d346ff2f686531d25521678d4fd3f76bbf2c893d246cb4d7693792fe18172108146853103a51f824acc621cb7311d2463c3361ea707254f2b052bc22cb8012873dcbb95bf1a5cc53ab89f",
            "003af5ab6caa29a6de86a5bab9aa83c3b16a17ffcd52b5c60c769be3053cdddeac60812d12fecf46cfe1f3db9ac9dcf881fcec3f0aa733d4ecbb83c7593e864c6df1",
            "004de826ea704ad10bc0f7538af8a3843f284f55c8b946af9235af5af74f2b76e099e4bc72fd79d28a380f8d4b4c919ac290d248c37983ba05aea42e2dd79fdd33e8",
            "0087488c859a96fea266ea13bf6d114c429b163be97a57559086edb64aed4a18594b46fb9efc7fd25d8b2de8f09ca0587f54bd287299f47b2ff124aac566e8ee3b43",
            TestName = "SigGen p-521 sha2-512")]
        #endregion SigGen-P
        #region SigGen-K
        [TestCase(Curve.K163, "sha-1",
            "02fe8758c01034e100eca848cc422f08c6c6315dd4",
            "8cd800cf87bde9117b4c5898319e159194bff6b270d5ebb4c1c469b3da8303f1884ae416d0aff0ac0357584340b33e12ab9ccca48060a15991a22de26920941c0a55bc3f824a1d36d00dca737c84cdd85e65c37e6137475d1d68589071b6ba83df035c028c54893022d4b6defd67ac0a521011368c624132849656d7e71c04ce",
            "013228ae356f03104e574a9eb97423d54ad44c6d95",
            "028c15effd01c56184ae28c386bb3e4ea0889933ed",
            "03147feab3a8d38b53f771c4a8135c94a630a73694",
            TestName = "SigGen k-163 sha-1")]
        [TestCase(Curve.K163, "sha2-512/256",
            "0153c7b83d952a313e29b23201c43a120096a127e2",
            "4d277fb8e3201de69f429afe4537623df2515d355473ff1ac57e519fd38ae75c70f614cce00626da7c10b0f98b4c55ce382a50d596de4a266577a2b46e97bfe5918c55ee54856a1e7e6a49cd9a755619650845e7b4b3bcd8e10e23b0b65c4e3c8375fd493739ae4965c9cb27e760066488e5776fd12d4d22d335bf2465766b60",
            "001eabc458fd610daec7d0015bc2c3e8a3b093d81e",
            "031fe24009bc0ea66662b0f11165683c5d47e83d09",
            "030036a9890ccc330055ec5b3f2402e42df43882e9",
            TestName = "SigGen k-163 sha2-512/256")]
        [TestCase(Curve.K233, "sha2-224",
            "004c1d414696cc3657dd9df73ace56eda2636769ce7082e064c260be45a5",
            "f23f784fe136c9fc0d169503d361e9c6148b0f1fbdcae0a97fae1af7033ddef25cb7489c9963cfcb009a8cbfe44a8510a64a073eb1deae4c324ceb9302008c92c69b2dafcc9077fd3cc3c7c119edc3ced36d176ceaa55ac036bf7f07f6fa215e8bb8196e59a5e1c9af4f98b90ab4970885bd7015fa26a09e03c7cf6b4b23d929",
            "0058f8511089fcd59324469f6736b92693afe26bd4719e198f1f2287dc5f",
            "0016bafefb4933ffd00bd1db6d6c4fac8a06375603adc0aa2a5664083ff4",
            "003bcb84b8f1990cfc7b88f2b8cc817105cd8e150808e7c87b310cdc47e3",
            TestName = "SigGen k-233 sha2-224")]
        [TestCase(Curve.K283, "sha2-256",
            "00668de088c6913640fbefbe6d2c44ab26e481802dbf957044a4957c3c5d0a0fde331501",
            "f646e7334e191c2bf0056d3bfd23f03ef7f0777b923f962519a8399d311b8f68414c689ca34b96871fae99eb7ea534fcd83e788e56eeef817cbfe33677283c736b99bf6a626f9515291e842bf99f694e4e8aa7c9911c591a87d5f112b3d96b064594e2b368e6d1bf1a1cd343d54916a66da22c26355266aa2884120fffb8b94d",
            "00b24bf54795fa02eb9527f21ead5497a6db2bcc7849a16d206239f830df313dfb7a2716",
            "00852d8b6fe93b0b36af5d99530eed08669eb9a25972fbea59f32dafe88b722bada98ab5",
            "00e5b08d410f2252f724dfcecaedb37b92a6c09cde646ff6237007f4199068f945ebebe2",
            TestName = "SigGen k-283 sha2-256")]
        [TestCase(Curve.K409, "sha2-384",
            "006f2c6e9ea8109223d9a349fce14927618fc4fa95e05ecf9aba1546619eaeaca7b5815cc07e97ae8cd1e9973ac603f84d838393",
            "ec69f2937ec793aaa3486d59d0c960ee50f640a9ce98a3becffc12d6a6c1c6c2f255d37d29f9b4d068373a96beadac98fd5203a9f229bfc70bcd449640165ae5128e3f8d057769e28356e73e35d8e9af7876f608390090892c67391ddfcc1c332aa61efbf72d54bc615998b3be8ab0a9d372784bea48c9fab244482c75cb2de3",
            "0042325aded3f71fc3ff0c84106f80a10af08d76d5e710a35d462e880e015a36d063599573ce2044537b9f62b51ed4fd2ed8b860",
            "00667c74ee2d632aed13cad47e0b46a5176940652d7da613e4965876e7e22d89994bdeadd6b5d9361c516fd51a4fb6b60b537e9c",
            "0026a01220a1166a4d0172428753e98caf0aaac5b0a09c5a3f11b2645d243991d141f59d6cc502ac44b70e7c48d6b0d7b6ec4869",
            TestName = "SigGen k-409 sha2-384")]
        [TestCase(Curve.K571, "sha2-512",
            "015b7271d4319db5743119c8103a7d4c6d57e9c62f3eb93762156d2ebd159980aa57cea948e416717d715a2e458851f1b2e9ad4172bbcc53861db29c3ee0ba8e82617a5866170847",
            "97b79c76d9c637f51294369e0bb52c4189f2fd3bd0607f91834aa71b3555605a89ff68e84fb5bda603f502f620e14e8b0c7affefafa2f0b303009ee99653ae4550a05315e551dd12a4d8328279b8150d030b03c5650ed4f8d3ba7c3a5361f472f436b200b321e7863c771e20ddd7bdf739c51de3676f953a5501e4477aed1bd8",
            "00c585e425ae4a34f9b7b9205f095ea07599716f1eab1a8bbd934219ad760c4606ebbeb06cbfd3952e045a040b8ce20603aea4f965d1b6e87eac7a61672823fb2de7767e3466c730",
            "0129162cce6fb05e1fc8630ec6c3a16d108bcd251719d89631497177e6fe6d1373f114ad9dde6e04a4ee0b4747f91c78703012e5a058c132d54f2ccccfc0f9326b27d60322b497e4",
            "0140163edb5f3c4b49228e4614bfc6da9f73674eab82678ad9947b2a635f733dbce99ce3209f613e2a75e62ed84db4d7d13de6d789b7cfedc0cb6a028d8316db8831db66c91791c5",
            TestName = "SigGen k-571 sha2-512")]
        #endregion SigGen-K
        #region SigGen-B
        [TestCase(Curve.B163, "sha2-512/224",
            "00f140b92443ed7e98b93e3b73d60fda614e0c8add",
            "57484e2b01b44d4e6af194be98a7fbbae4936b54951987b9a644a2e74f582497d9caaa2a8bf4fe359b5903f119ca2a0793279de2559253e8792816523eebb9ff1a20501e0fed0c1993913b97c2347130a28adfa0bf3bcc85d9578e06df68f87a3ef75a7d6479dc06bf00f548bd64b5c9e8d0035562ce50785f487d754b59d5a3",
            "009fe3911e4b21dcd4b43c88ec2bdcbf05e55d3ebb",
            "0178180d1df5c8d0c7dcba4260a9b85aae6ab5d3c5",
            "0313ae085175c6d903a4801be37ddb45342c2a6455",
            TestName = "SigGen b-163 sha2-512/224")]
        [TestCase(Curve.B233, "sha2-224",
            "0056673197bfeea9bd7a8b820b4ae51a50411bf118a692bb9ed3d304da53",
            "f1b67fde01e60e4bb7904d906e9436a330c5cb5721fd4e0a3c75b83dade868736bb1d21cfb1b5c6407c373e386ee68ec2239b700e763728eb675a153b8ac44cf2a87be85fe8ed6683430cf4b7d718891cbf8d583d0a37cc952cc25fe803a7aa4fda80f05541a2f1f2601cdd0c095f7110f2a84f7d641b8531572269b21cbe77b",
            "00a6c9914a55ef763913273b062475fd0188eb2d5af9c8c1dd97cb3cefc3",
            "008601a42d7f7eb047e8ed9820ddce665c7277f8ef38c880b57109b7160d",
            "0026d6f50f0508953657df5d753c595ffb8e1c19f8d092f8ce8db54f76d0",
            TestName = "SigGen b-233 sha2-224")]
        [TestCase(Curve.B283, "sha2-256",
            "029639da33f48e4fb0d9efdf50bba550e739f0d2476385cba09d926e789191b6fb0a73ff",
            "f415d0adcd533dd8318b94560f86732c262ad2c6dff9dc83e2435543f429a2158cd2fbab0d96c027f71008c4895ecc644c2ceaefa80937f6cc6338d15d36e459a16bd9387a361a6d800acfd834ad5aecf442e30b70f5bfa164747cf9f89325b80976052a83a5e896c00c54f81472b14329cf23bec10a8e693005de2a506ba83d",
            "032a930fdb1ba2338554a252d1bf7f0169d18750a4ec4878d2968c5e735f98b9d0c25edb",
            "030cd65f1097d3fa0d05e1d6072675f1377a883b683c54b8a1f4960f90d68f3ee8c7bd98",
            "015c61ddf43386a2b8cf557760200ac06a480797e21c92e45e6a311e1a508b03c4d9632e",
            TestName = "SigGen b-283 sha2-256")]
        [TestCase(Curve.B409, "sha2-224",
            "006b6cfe9c82e3abb0451758c8ecedf8934044f4d45f87c32cfd4361fc9d906200bc4e9eb8be8ff5e3c60866e701eb80c83cb3f5",
            "75780bef161dc8f88578ffb8502ed29ec1b5ee710eb961dad0bfad23b6091a3e7bb95009153924d030d9b313dabaad8a80ecf46719d366dba1cff6dbc91b0224edf6e17d4601bb3c61e99d0bcf45ccec1fc25a7b097df873ecb00b747b910204b277e6d3103f324300eee6279897d7cb858049364680bfab99ab12763332c1e5",
            "00726838d4111f360fa833dfff819f0f8d2aa4e85cc77a984518df0396e084c6fd02386672d94986b8ba3052e377f4bcc1ffca30",
            "000e00c35cc4a6c47338ade3799144461af0edd96c05a2f0f790d9d3565921b7f69ef0823d027e6d5cb107a5bd0690ea062da76f",
            "005807929f80b7f4815c7414ba139a499ec82a2868ba889c6624c3d587b0b34f01bae3efc795b1f9b9c3f1a7fb27e848a28cb622",
            TestName = "SigGen b-409 sha2-224")]
        [TestCase(Curve.B571, "sha2-256",
            "0076ea4f6807a4def65ade2613f0b0c9908c6227cc50562d83b285853da3bc8a029745a709b3391d554c4c49969188d0dcaefd2add1689a906d953465d2cf0c070f7adae8018910d",
            "e6e35adb16da21c8d361987f9698b13c83a74194a7f75f4f7ea30c75b8291aab3c6646c06ec1a884ada64dd92036220cb33c13655c6375fca3a7a1cf610ae212c88892c6a34c7f8ddf2494bd4b33b5b39ce6ea6ab429c04c24daec47ef7d3224913285143fe5708c71c5b4539ef733da468b0ccb36af08e408223506e9ecd089",
            "01418fa97ec46f939b5fd89644fae01d25dd2c3a9bdde12eb01ec41347f72122f7091db00e8ebe661233f8f78ed7a5fa11b4c15d7f07d083536fc881bb0dd50f1805eea2234a5462",
            "02fc660a1b0b7267b52eb589bcffd854d4304b6bfec8b06a57e299caf1fa19b035d808697158a0238a227007c45836ad5de46977488fe193be429c3ad88e4807a66418de19b5d488",
            "00a7b50f80b173e12e85e4d301fe2403b2c4a9abb974d9efabc685ec7136c348d8d11d7717f41ec49133d4767fddc3aef9c3f4e63eff8a5a184362ccfe52b960fe79cee3f111a20f",
            TestName = "SigGen b-571 sha2-256")]
        #endregion SigGen-B
        public void ShouldGenerateSignaturesCorrectly(Curve curveEnum, string sha, string dHex, string msgHex, string kHex, string rHex, string sHex)
        {
            var d = LoadValue(dHex);
            var k = LoadValue(kHex);
            var msg = new BitString(msgHex);
            var expectedR = LoadValue(rHex);
            var expectedS = LoadValue(sHex);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var shaFactory = new NativeShaFactory();
            var (_, shaMode, shaDigestSize) = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(sha);
            var hashFunction = new HashFunction(shaMode, shaDigestSize);
            var hash = shaFactory.GetShaInstance(hashFunction);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(k);
            var nonceProvider = new RandomNonceProvider(entropyProvider);

            var domainParams = new EccDomainParameters(curve);
            var keyPair = new EccKeyPair(d);

            var subject = new EccDsa(hash, nonceProvider);
            //subject.AddEntropy(k);

            var result = subject.Sign(domainParams, keyPair, msg);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedR, result.Signature.R, "r");
            Assert.AreEqual(expectedS, result.Signature.S, "s");
        }

        [Test]
        #region SigGen-P-SHA3
        [TestCase(Curve.P224, "sha3-224",
   /* D */  "3F0C488E987C80BE0FEE521F8D90BE6034EC69AE11CA72AA777481E8",
   /* msg */"Example of ECDSA with P-224",
   /* K */  "A548803B79DF17C40CDE3FF0E36D025143BCBBA146EC32908EB84937",
   /* R */  "C3A3F5B82712532004C6F6D1DB672F55D931C3409EA1216D0BE77380",
   /* S */  "485732290B465E864A3345FF12673303FEAA4DB68AC29D784BF6DAE2",
            TestName = "SigGen P-224 SHA3-224")]
        [TestCase(Curve.P256, "sha3-256",
   /* D */  "C477F9F65C22CCE20657FAA5B2D1D8122336F851A508A1ED04E479C34985BF96",
   /* msg */"Example of ECDSA with P-256",
   /* K */  "7A1A7E52797FC8CAAA435D2A4DACE39158504BF204FBE19F14DBB427FAEE50AE",
   /* R */  "2B42F576D07F4165FF65D1F3B1500F81E44C316F1F0B3EF57325B69ACA46104F",
   /* S */  "0A861C2526900245C73BACB9ADAEC1A5ACB3BA1F7114A3C334FDCD5B7690DADD",
            TestName = "SigGen P-256 SHA3-256")]
        [TestCase(Curve.P384, "sha3-384",
   /* D */  "F92C02ED629E4B48C0584B1C6CE3A3E3B4FAAE4AFC6ACB0455E73DFC392E6A0AE393A8565E6B9714D1224B57D83F8A08",
   /* msg */"Example of ECDSA with P-384",
   /* K */  "2E44EF1F8C0BEA8394E3DDA81EC6A7842A459B534701749E2ED95F054F0137680878E0749FC43F85EDCAE06CC2F43FEF",
   /* R */  "30EA514FC0D38D8208756F068113C7CADA9F66A3B40EA3B313D040D9B57DD41A332795D02CC7D507FCEF9FAF01A27088",
   /* S */  "691B9D4969451A98036D53AA725458602125DE74881BBC333012CA4FA55BDE39D1BF16A6AAE3FE4992C567C6E7892337",
            TestName = "SigGen P-384 SHA3-384")]
        [TestCase(Curve.P521, "sha3-512",
   /* D */  "0100085F47B8E1B8B11B7EB33028C0B2888E304BFC98501955B45BBA1478DC184EEEDF09B86A5F7C21994406072787205E69A63709FE35AA93BA333514B24F961722",
   /* msg */"Example of ECDSA with P-521",
   /* K */  "C91E2349EF6CA22D2DE39DD51819B6AAD922D3AECDEAB452BA172F7D63E370CECD70575F597C09A174BA76BED05A48E562BE0625336D16B8703147A6A231D6BF",
   /* R */  "0140C8EDCA57108CE3F7E7A240DDD3AD74D81E2DE62451FC1D558FDC79269ADACD1C2526EEEEF32F8C0432A9D56E2B4A8A732891C37C9B96641A9254CCFE5DC3E2BA",
   /* S */  "B25188492D58E808EDEBD7BF440ED20DB771CA7C618595D5398E1B1C0098E300D8C803EC69EC5F46C84FC61967A302D366C627FCFA56F87F241EF921B6E627ADBF",
            TestName = "SigGen P-521 SHA3-512")]
        #endregion SigGen-P-SHA3
        #region SigGen-K-SHA3
        [TestCase(Curve.K233, "sha3-224",
   /* D */  "8434613F4B799B4C26E4D7AB8E9481B04B09E648C94AFFD14B611A20",
   /* msg */"Example of ECDSA with K-233",
   /* K */  "0190DA60FE3B179B96611DB7C7E5217C9AFF0AEE435782EBFB2DFFF27F",
   /* R */  "3EA7231662E6516F11E37D59D500D70F09E62D64FE6FF445C9B479C8B0",
   /* S */  "4CC71DA49B12F09C1DFABB3D7B5BC03CE4898B5E07B2AA3D4BF6569395",
            TestName = "SigGen K-233 SHA3-224")]
        [TestCase(Curve.K283, "sha3-256",
   /* D */  "069E6D19F7E454A83664FF49208F6038EAF842E164DF42D0F64948FF9C94B014988329",
   /* msg */"Example of ECDSA with K-283",
   /* K */  "E3084442D66FA9A02C42890163E57EE33CA1F4583C65BCBDE92781C7A3C83E89B773",
   /* R */  "01C973D58FD17A06AA8F39D5EC42E0A6B99339C1D57FF6F0EABD04ED57AE35F2E5C95E05",
   /* S */  "01E1A2E8C2CF4E4796B1B3F1C4D7B8F627D53888DF8CB1A7A2A499DC823E6E979AD7B285",
            TestName = "SigGen K-283 SHA3-256")]
        [TestCase(Curve.K409, "sha3-384",
   /* D */  "019F5789FE26E0E700C69E253E9F74D76EAFB4C979D0B1584D4FE98715D45B7BAAA851E02A1ECAED8B96602CF611D8A504BBD5",
   /* msg */"Example of ECDSA with K-409",
   /* K */  "01592048516CCD793C7B863B00985FDBA71C3D1EDF449F667AC0D05EF37D15A94AD3282F29F7E9FD9491872F931354A1CCFA39",
   /* R */  "6F421A230AA8B471939A77BF4F2C64FC0B4CAA39EDCD06337A92B62BD70A883DFC65C68A9AD33AA5EFB061884D8FEDECD2AC65",
   /* S */  "7116C6CF1FCDA7D13ACD5D3EB8C66D4769A642A6AAE1FA74A4C8F6B9C3CD73C76EA3B6ED489D540C88EA92795EBF32849F3260",
            TestName = "SigGen K-409 SHA3-384")]
        [TestCase(Curve.K571, "sha3-512",
   /* D */  "01042FDE4D66E76725E7957E208A85CF23BC0D5B8D001B36AEAFB34AD1104004CCF99AFDFABCA11585A4EB5263C87052CB05EF7FB39D9E5F6CF495E9DCE5840B83FBC5FF3AD8B2F3",
   /* msg */"Example of ECDSA with K-571",
   /* K */  "0104063C918DE62000A3FD87775D8D71398722BD153B8EA33060349C5FE6CF6CB4677957E6BA50D3C8A8B5182B9CF962954A6BBB5F7868B88E5778AA62A0CF8002BF19DA3049FF51",
   /* R */  "68758C313FBF1945F775D95B25866DBC8D001D9C7EF4FFA53774E8C68927A52707F034957247CB5717A5B6D84AE6F6C688DBE59859BD6D03B48237476742AFE37CEFE36D5B20EE",
   /* S */  "01186A2C9FC3FF4B334099C16B3F1A96B6B1CE94018909874E2B8379B3C1DEBB9C5FABFAFD0C977F423EABCB5B76B9ECF6BC76D22718CE03E95D7E49ED9A6C27929C693088178359",
            TestName = "SigGen K-571 SHA3-512")]
        #endregion SigGen-K-SHA3
        #region SigGen-B-SHA3
        [TestCase(Curve.B233, "sha3-224",
   /* D */  "8AF6D5A8E875977C7D4BA1F611CF7B6D70B26140BF84A1CC281F1B7B",
   /* msg */"Example of ECDSA with B-233",
   /* K */  "8A65482917BA18F1E8B266A3795B0A3A09C439FA6B611E37123BAF72",
   /* R */  "86806715D9620F0A3E62C1BA593D842E41F582B37F39F1E79D2292BD8C",
   /* S */  "D57867ACD82E45EDE4F8B92D0401257C7100F58D143DED7B5813388704",
            TestName = "SigGen B-233 SHA3-224")]
        [TestCase(Curve.B283, "sha3-256",
   /* D */  "010652D37B0A9DB64D4033AC6549CD1DF37E1EEDE2612C2363257C6AFF6C8CB5DCB63648",
   /* msg */"Example of ECDSA with B-283",
   /* K */  "0100EC321393E6DD6C4D47BE5AE189E5E35408579D0862178F94CCBBA3C4049A4D88E297",
   /* R */  "037CB284AC41E72EDA2A93EB8D6DFF58620F7CD99B927EEC7A060A8F6FB7D9265EAFA76F",
   /* S */  "027A943BE3894A44E3EAA2A90CD83883767DBA364A10643BDECBE65C104AE104589BED7A",
            TestName = "SigGen B-283 SHA3-256")]
        [TestCase(Curve.B409, "sha3-384",
   /* D */  "4AF896DB379ABDF70C8FADE9EBD28CD530F2ECB336B4DE84BD6E065EF56C8C548C532D00FA55CA8ACF3E98ADBCA9F78D241B",
   /* msg */"Example of ECDSA with B-409",
   /* K */  "6A0B81D9320B5C305D730B1C1E74B03FAFB88A7EC355990B75F9B70E8532433296A32492CBA06F8583D5B19C5B8C5D6D07EC",
   /* R */  "F3E4DA3101C64239D76831995C0EC1E56CE4690C42DDD53DBF3D147B0173CC15D87E749382CD5EBD9492FD568F43959763B768",
   /* S */  "58961D556202AFCC840235F736D27F1B4EEC64DFAD18F5450963C75DCB828E68E80425365031E36BFEBBD14320757C594DBBDD",
            TestName = "SigGen B-409 SHA3-384")]
        [TestCase(Curve.B571, "sha3-512",
   /* D */  "03CCE32BA00DC3A7EEFC9EB6F6CBFB9C5F0E57F532B7EE6826D4A75D0E756FD533900F2CEA8CCCC50EE22CE079398D371EC4A2EC45CC24B887606678E9C67453D0F5E768E9D752",
   /* msg */"Example of ECDSA with B-571",
   /* K */  "01062FF6D95C49AC610CB9AF9900D59C288669C3626306DB7EB7F119499BA1D54CB6BE888758CAADA69952675CC0CD4999176879BC302A7E2A5118DFC7D538DA114CCAC2BAF9AD08",
   /* R */  "E17447E422A2C0085190354AC149210C1137A92C10F9B14D225E6510DA76B19EF44D39390DD9D808C9DFBAE67D9CF0E7BE79A9E72FA8FA1DFE89F43FB6D093A6CEB30E136EABA3",
   /* S */  "0319B6441D2B79089C9797308A8F96EF92B9FA5708A4E7AC3896BF2B8FE9714B95E59EB6007A3FC2A9D706695893F984191B995FD2216AA8A44F9746954FA1F47176D2331F7D3679",
            TestName = "SigGen B-571 SHA3-512")]
        #endregion SigGen-B-SHA3
        public void ShouldGenerateSHA3SignaturesCorrectly(Curve curveEnum, string sha, string dHex, string msgAscii, string kHex, string rHex, string sHex)
        {
            var d = LoadValue(dHex);
            var k = LoadValue(kHex);
            var msg = LoadMessage(msgAscii);
            var expectedR = LoadValue(rHex);
            var expectedS = LoadValue(sHex);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var shaFactory = new NativeShaFactory();
            var (_, shaMode, shaDigestSize) = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(sha);
            var hashFunction = new HashFunction(shaMode, shaDigestSize);
            var hash = shaFactory.GetShaInstance(hashFunction);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(k);
            var nonceProvider = new RandomNonceProvider(entropyProvider);

            var domainParams = new EccDomainParameters(curve);
            var keyPair = new EccKeyPair(d);

            var subject = new EccDsa(hash, nonceProvider);
            //subject.AddEntropy(k);

            var result = subject.Sign(domainParams, keyPair, msg);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedR, result.Signature.R, "r");
            Assert.AreEqual(expectedS, result.Signature.S, "s");
        }

        [Test]
        #region SigVer-P
        [TestCase(Curve.P192, "sha-1",
            "72a83b1ee3f83034324db4377663c933b4799564b2335bea",
            "76b0c9874b94daff7e78881d22e5fcd53a3ea2afd0d118f4",
            "bf826447a665165a837ed32a13c49e3b57a9e9bce263d1492bcc418b0eefd4093032b62ecd27f1a2031af454077f7858f1e3970050e9b44b98b388b27f3487fdf27adcaae07dc7ab1913dd7983a9744063dd01e976cb818cc7c3a838b50bc55588d41240d97b714d2c2dab550814724250a5a478ad445e154bc8950f8f1aaa37",
            "161e7d162dbeeb5f8d3393df65fb6a136ad867ddd3b85ca0",
            "301cdf1284766043f9a0cc1eb2f2a21538dd8e618cc46ff3",
            true,
            TestName = "SigVer p-192 sha-1 Good Signature")]
        [TestCase(Curve.P192, "sha-1",
            "1de0280fbc9fecb9852b43e5ad9afe9e7913785c0dd26281",
            "df29c3aad5cc12e09c92dd90f32ee05f2b2522ded35bac18",
            "0f6be792b3525f87712a85e6ca8113641ca236b1e7b2446edfc081d08e9c28a7bce507d61caa6de3e66002a6ddc020e257353fee70773ee38381cd82e5014ea446777a25bc141da38bb74d922c61cde6c3d43116cff5d67564476e6de0366a99fbba4b811c66ff851f51b25f1db87a2b6a33da0d3e94335b00620754a20bf19f",
            "5094e6e7982856996dbdd8c2fbec21992426213852d2f772",
            "c44050cbc64b739f0c37851da5c47e3177054ea0b843fdba",
            false,
            TestName = "SigVer p-192 sha-1 Bad Signature")]
        [TestCase(Curve.P192, "sha2-512/224",
            "4e18ceeb46772b85c83d9b4ef3981da60aa92c6cbdca5ce4",
            "f12a138be13e4664a28ae5c2a15c4c317ef2f6f9d119096c",
            "55e84ee467d7edaeefe85a42514fb1ee15af9d2a3c3ef49aaed8ddd0d3b937a0bf993e076eb4b685c493bfe1f1ef76975e5703ddc63a6aaee7c22e15dd876728bb0a3fb8e056b39dec3462ea10c1e4ecbcc646a5c196452005fa6546780bc1555094302f3704cac45a23120a271de7ad7b590085b96281f69623ffbdb99d88b1",
            "b22db35976e340887de6ea169e5a3b7820c270df337199a5",
            "97b8e6890990b308124478050e7a921e996d38d1a490f365",
            true,
            TestName = "SigVer p-192 sha2-512/224 Good Signature")]
        [TestCase(Curve.P192, "sha2-512/256",
            "be8e233ed06229080985513d8b07a0bb8e6046b93cfb64bd",
            "5d6fc629989f3338d522b7c8ca9c15103d97afb3bca56807",
            "2d9eabb148b493e1ee4fcb8184b1fb28fa3c6e6c99e9bd5fa9c00e24c5f1db51ad81728b52dcb7ccca807c8e3c20c4acc67869b58d65ee072c8892f6a09ced894d474d8c6b53a2dd44ffb722ce033c7fcc264b382ae75ac759a5264e527e1f51905ffa4c002b60dd422c28212b8f01dade27c0d2c5324f6e3ac313413ce81a6f",
            "ea014b2c41a9e4688bb2ca602e5c42a1b9f3c9f8bbda36f7",
            "999791e0c6f41d8356950e6e20929c4b6ca4e5c0372d289b",
            true,
            TestName = "SigVer p-192 sha2-512/256 Good Signature")]
        [TestCase(Curve.P224, "sha2-224",
            "f1eb36b3e1c96a18d87878d5fa8b79d77afce9d2ce40d26199f33482",
            "ae819af474f3efbd62401a407036505c5a2d60449274593865de3374",
            "2dad0fdc03e9617e0de30b3108e0ef155e4e6c3169cec76622c16dc55fcac39a5fb002472072754e7885cac0e318b3ce0588559152a37e6e55effb6b8e19c45ac8aaa91fbd8cad41fd2a2d5af03841ba13f405b20a04585ac0e456502b9686e72e87e8ad7257d3d65781766c3752c6aa9a24d6f49052e753e2e31e155a35b7ec",
            "003122e976bac378c06ec95fd73290b067e7ff022d23493c40663ec9",
            "b99eb4220146a282c7a34f98a9a4fa38ed3f48ca2c7983cde2d3235f",
            true,
            TestName = "SigVer p-224 sha2-224 Good Signature")]
        [TestCase(Curve.P224, "sha2-224",
            "3bdcc7c6112cde3c0522f1a4863f1d7b6727c5bff67598ba2f1bafc1",
            "47acb6b254e0e8747e0039de471d0dda443cb09a592c678717d83200",
            "26b7a6da0a0099c0ed3b297e994765cee13a77fbb5ac13c5cf3cea4ea7bb66ddcc58f85e7b65787a40df26a475f9e47b1ef92db42afdb3ad37a52d773c90f2f0d6e0d2549a2ad5de26bcedcbe6b7629d727216b89928b873841d31c7ffcbda4bd3055eba8e66416c3601eab01e3ae8cffa20d9a9e79eb31cf1084354f0a25f25",
            "a5aab7768f549f8fe3c7e650154c865b71ea5089bd6303bfdfd19316",
            "ee4989c4b96bcc802464fe44b2adeb1b3506755a3f4fb3f9252bf21b",
            false,
            TestName = "SigVer p-224 sha2-224 Bad Signature")]
        [TestCase(Curve.P256, "sha2-256",
            "e424dc61d4bb3cb7ef4344a7f8957a0c5134e16f7a67c074f82e6e12f49abf3c",
            "970eed7aa2bc48651545949de1dddaf0127e5965ac85d1243d6f60e7dfaee927",
            "e1130af6a38ccb412a9c8d13e15dbfc9e69a16385af3c3f1e5da954fd5e7c45fd75e2b8c36699228e92840c0562fbf3772f07e17f1add56588dd45f7450e1217ad239922dd9c32695dc71ff2424ca0dec1321aa47064a044b7fe3c2b97d03ce470a592304c5ef21eed9f93da56bb232d1eeb0035f9bf0dfafdcc4606272b20a3",
            "bf96b99aa49c705c910be33142017c642ff540c76349b9dab72f981fd9347f4f",
            "17c55095819089c2e03b9cd415abdf12444e323075d98f31920b9e0f57ec871c",
            true,
            TestName = "SigVer p-256 sha2-256 Good Signature")]
        [TestCase(Curve.P256, "sha2-256",
            "87f8f2b218f49845f6f10eec3877136269f5c1a54736dbdf69f89940cad41555",
            "e15f369036f49842fac7a86c8a2b0557609776814448b8f5e84aa9f4395205e9",
            "e4796db5f785f207aa30d311693b3702821dff1168fd2e04c0836825aefd850d9aa60326d88cde1a23c7745351392ca2288d632c264f197d05cd424a30336c19fd09bb229654f0222fcb881a4b35c290a093ac159ce13409111ff0358411133c24f5b8e2090d6db6558afc36f06ca1f6ef779785adba68db27a409859fc4c4a0",
            "d19ff48b324915576416097d2544f7cbdf8768b1454ad20e0baac50e211f23b0",
            "a3e81e59311cdfff2d4784949f7a2cb50ba6c3a91fa54710568e61aca3e847c6",
            false,
            TestName = "SigVer p-256 sha2-256 Bad Signature")]
        [TestCase(Curve.P384, "sha2-384",
            "cb908b1fd516a57b8ee1e14383579b33cb154fece20c5035e2b3765195d1951d75bd78fb23e00fef37d7d064fd9af144",
            "cd99c46b5857401ddcff2cf7cf822121faf1cbad9a011bed8c551f6f59b2c360f79bfbe32adbcaa09583bdfdf7c374bb",
            "9dd789ea25c04745d57a381f22de01fb0abd3c72dbdefd44e43213c189583eef85ba662044da3de2dd8670e6325154480155bbeebb702c75781ac32e13941860cb576fe37a05b757da5b5b418f6dd7c30b042e40f4395a342ae4dce05634c33625e2bc524345481f7e253d9551266823771b251705b4a85166022a37ac28f1bd",
            "33f64fb65cd6a8918523f23aea0bbcf56bba1daca7aff817c8791dc92428d605ac629de2e847d43cee55ba9e4a0e83ba",
            "4428bb478a43ac73ecd6de51ddf7c28ff3c2441625a081714337dd44fea8011bae71959a10947b6ea33f77e128d3c6ae",
            true,
            TestName = "SigVer p-384 sha2-384 Good Signature")]
        [TestCase(Curve.P384, "sha2-384",
            "1f94eb6f439a3806f8054dd79124847d138d14d4f52bac93b042f2ee3cdb7dc9e09925c2a5fee70d4ce08c61e3b19160",
            "1c4fd111f6e33303069421deb31e873126be35eeb436fe2034856a3ed1e897f26c846ee3233cd16240989a7990c19d8c",
            "4132833a525aecc8a1a6dea9f4075f44feefce810c4668423b38580417f7bdca5b21061a45eaa3cbe2a7035ed189523af8002d65c2899e65735e4d93a16503c145059f365c32b3acc6270e29a09131299181c98b3c76769a18faf21f6b4a8f271e6bf908e238afe8002e27c63417bda758f846e1e3b8e62d7f05ebd98f1f9154",
            "3c15c3cedf2a6fbff2f906e661f5932f2542f0ce68e2a8182e5ed3858f33bd3c5666f17ac39e52cb004b80a0d4ba73cd",
            "9de879083cbb0a97973c94f1963d84f581e4c6541b7d000f9850deb25154b23a37dd72267bdd72665cc7027f88164fab",
            false,
            TestName = "SigVer p-384 sha2-384 Bad Signature")]
        [TestCase(Curve.P521, "sha2-512",
            "0153eb2be05438e5c1effb41b413efc2843b927cbf19f0bc9cc14b693eee26394a0d8880dc946a06656bcd09871544a5f15c7a1fa68e00cdc728c7cfb9c448034867",
            "0143ae8eecbce8fcf6b16e6159b2970a9ceb32c17c1d878c09317311b7519ed5ece3374e7929f338ddd0ec0522d81f2fa4fa47033ef0c0872dc049bb89233eef9bc1",
            "f69417bead3b1e208c4c99236bf84474a00de7f0b9dd23f991b6b60ef0fb3c62073a5a7abb1ef69dbbd8cf61e64200ca086dfd645b641e8d02397782da92d3542fbddf6349ac0b48b1b1d69fe462d1bb492f34dd40d137163843ac11bd099df719212c160cbebcb2ab6f3525e64846c887e1b52b52eced9447a3d31938593a87",
            "00dd633947446d0d51a96a0173c01125858abb2bece670af922a92dedcec067136c1fa92e5fa73d7116ac9c1a42b9cb642e4ac19310b049e48c53011ffc6e7461c36",
            "00efbdc6a414bb8d663bb5cdb7c586bccfe7589049076f98cee82cdb5d203fddb2e0ffb77954959dfa5ed0de850e42a86f5a63c5a6592e9b9b8bd1b40557b9cd0cc0",
            true,
            TestName = "SigVer p-521 sha2-512 Good Signature")]
        [TestCase(Curve.P521, "sha2-512",
            "01184b27a48e223891cbd1f4a0255747d078f82768157e5adcc8e78355a2ff17d8363dfa39bcdb48e2fae759ea3bd6a8909ce1b2e7c20653915b7cd7b94d8f110349",
            "003bd6e273ee4278743f1bb71ff7aefe1f2c52954d674c96f268f3985e69727f22adbe31e0dbe01da91e3e6d19baf8efa4dcb4d1cacd06a8efe1b617bd681839e6b9",
            "3607eaa1db2f696b93d573f67f0359422101cc6ceb526a5ec87b249e5b791ac4df488f4832eb00c6ec94bb52b7dd9d953a9c3ced3fb7171d28c42f81fd9998cd7d35c7030975381e54e071a37eb41d3e419fe93576d141e36a980089db54ebbf3a3ebf8a076daf8e57ce4484d7f7d234e1f6d658da5103a6e1d6ae9641ecac79",
            "004c1d88d03878f967133eb56714945d3c89c3200fad08bd2d3b930190246bf8d43e453643c94fdab9c646c5a11271c800d5df25c11927c000263e785251d62acd59",
            "012e31766af5c605a1a67834702052e7e56bbd9e2381163a9bf16b579912a98bebabb70587da58bec621c1e779a8a21c193dda0785018fd58034f9a6ac3e297e3790",
            false,
            TestName = "SigVer p-521 sha2-512 Bad Signature")]
        #endregion SigVer-P
        #region SigVer-K
        [TestCase(Curve.K163, "sha-1",
            "033ecd8f31b2a4528692e8d6a64da3b1c4a5bd03a0",
            "02b0357df509db56d5b58d9de7968e5b44a822e311",
            "afd1324e877bd73ddc2ea040fa6fe0e70f10837c4d41ffe67b2f4f3a7bc41d24dc90c159ecd28b401cca36e9b9c31ec0f2ce09471d8dab50273cd7a4cea721455ea4318131e4c55396a089f4280a2bef234005d775046929c6ff784caaedb5559dca9e6f1800ce61fc2399dfd0fe71f49c9668d71cfd942b85dd59ec94ab543e",
            "02fb6f4f62727870a1b2cfe3d7405aa2f4a1882718",
            "03d70d8106ca04c2c2a3ecb4f36df1756b7b685f39",
            true,
            TestName = "SigVer k-163 sha-1 Good Signature")]
        [TestCase(Curve.K163, "sha-1",
            "0069b337f2903942473650163a3469dc40171f9f26",
            "0785dc6495e81807d4ed785d5d75fc665a786200b5",
            "21d738272430edb754051e653a636c9594418f993b5dd9d2eca795c5b542b59e485f8791d1ed4a0d0c78209e7e4301a532141295a7ac4d496153fd7dd0ac89482392b44cd6e945f8ff8483e633c4ad08aa3b9b5d7b2c4b1214176e65c78483656bf6c86099d569e41cc05ce1951e5d6ebf5e1ab4873f2f6139a41183d3faecbf",
            "035c1803549f650a4e6f70b8842cb4bef2cb738869",
            "03f5de8ff3886e3fd55fa4b252f4f8635ae50f5f5d",
            false,
            TestName = "SigVer k-163 sha-1 Bad Signature")]
        [TestCase(Curve.K163, "sha2-512/224",
            "071936424e1b8653b813b38d0aace19118e19bafa9",
            "01504aa5d3c0a96dbb80af70b657732030bfaa8eec",
            "b60b2ee441bc67cfaa8c5fef62cae9e40c6db8433d73a36c7a1f6467fc3f48cd0537cb1aa3d9f5ab79300f1db8d416e61bda3c863778995c4e044bf83f36887d467e17ea909de6f0707ea74c4106a5ac7b64e759d65fd477856992c7e76e1fdb54fa73680b92749c54900484231b90ac3f1445fd5179f506fe10d61a45009041",
            "00b5e5e2874ba5f81477ed579432ca39852351c15f",
            "03d047a34cc3eb423656e9b74dbf5cf7cc9c8b4ee2",
            true,
            TestName = "SigVer k-163 sha2-512/224 Good Signature")]
        [TestCase(Curve.K233, "sha2-224",
            "015e948fb47fe493ab59b1b03c8f06ffe635e9d95e532a6b9736fee039ab",
            "008186b0907c69e2856ef55f575644ad185afa400380f631cb1bdaabaa7a",
            "b4a4659a5c20f3af085cf047d9bc50870bfc44f001ce6d0e0d44238f1f61af245bbfc013f15860369d619973f59a39baff6f211a4cd0d44061705524241bf1a815705d270ea43dd8299ca4b57199e88fc492a5315f940a1eaa6205b69d67c55148237fd2940bf9e7e9e1002487b10175116735e37fd2ae5ca5da670eb1865d37",
            "0003b6d091b7504d7bbe3156bd30b68cf89c69e3417004c12bf8fab9419f",
            "00042b8bd930b72c983e781780ad553113160b1e88c1420ae555031e0cda",
            true,
            TestName = "SigVer k-233 sha2-224 Good Signature")]
        [TestCase(Curve.K233, "sha2-224",
            "009519e08532fad399f3d69a57e248bbead22fd2e5dc784feb3bb61ae22d",
            "0069bd883bdd1dc1c3b04e6ab7ebb765097712741dc88bafc7ded26affb8",
            "fc4cb7b52cd8a0350bffa51eb28875b9bf4eeb12dc6a84c2c5f18f7e98b92b77a8043f89a7aacdaff26cd2a2ab1562fd197357bd098b74f1c08f347205c1dae9e30e4ba4864e6a309b180b3de0d57196725744b7d144cf265f56739359b51135f9c058b0a801fa44a4846772dd7112f3b822034be4150e6d769de70ceba6ad59",
            "0067b025272a74077e9675ebde9348b53904a1baa8df5e98f3161cccf5d3",
            "007737820d0037c7de9c577e4f9256384401071bb925a184913259232ed9",
            false,
            TestName = "SigVer k-233 sha2-224 Bad Signature")]
        [TestCase(Curve.K283, "sha2-256",
            "05176d69c610481c8d49b06e517475da3940c9e4270e2ed9821ea21f4d96297fb05f39ee",
            "079087740a5925602ae7773d1bca9cbb5f824fb1f5791f66ca37e7046ad39ee50787ecff",
            "ea2bf1062045fe799a33ecd899d0e63598325d00400004117baa99a90ee63f5eeeab19e5293fb2e5ab9c23f4a585430b2cbdd91c26cc4cba0ba4547dd27b6a730a5c0549cd5f1b5fac8aa0022b7a2b28454d143411dfae999a137b11ecd68d303abbe625c679acaa1d54488a336eb0cc9d1758884549fe10c37cde81c5b69fd7",
            "003bf4c5c61f95b0b5e298a91a3b2e2dca3ad66340696c608d6cf5cc22f67e1938bea8fe",
            "0049776b29390c2eb64357a920088001b47b74a514121f693971f657516acd5d884a1851",
            true,
            TestName = "SigVer k-283 sha2-256 Good Signature")]
        [TestCase(Curve.K283, "sha2-256",
            "00d7e249a93742faf10bc417a71d29ba5bf2c64856c1c1e82a033b8abbad471d1527e123",
            "0362eb55cd5b254667a165af27c6f1ad8ceff78a1d3eb24227ab16c02233aec7d055ad0d",
            "b884abe9ab8cc7f80022a1883e49c0a45a7d2c0e378ce72ac6dd37bfd05ffe69dc8b815c41d63484755827511223ab1d1302684bf81b1f2356263e06138f9a2e341a1da5b020a8a46a91673bd27139ca9fcb2a355da5ce2412c82fb57d9aad1742ebc09b4b3ea1509715fb7787f72dd6523a07f54c4c8a285b3789df5f882b7b",
            "018e914222a04e787c9fb971e0996460358071c90e00fb54df79a7ca4130ce87d9be5558",
            "016d7c46ab425f49f2270f31e2584fbe0c23cee2d8bf1f65112cc5838d8650a49d39b2ce",
            false,
            TestName = "SigVer k-283 sha2-256 Bad Signature")]
        [TestCase(Curve.K409, "sha2-384",
            "00c2b2a3de502427f2c48b9a078187cf511d117e6e1c0bb6b4a7d06dd9f28e2741257b5a6a8da6272d038961275f6c470b5bf33e",
            "001b22d4544e479a197f6543a98fe1b75d3b70dfe0b51fab7040593b1f0ee6464dc2665a7eac6354344f7dbcec9122e0ef0b8a0c",
            "2a79cd9b399e272b6f047b93f1d97c4d2895c6a26f43788e8696fe531d3b2a65661532357127396420e88e5b0099d0a08d466a02abced4b3f07831b242490e69bd1495f6d517ce6b335a5d82b1a64584cd468e4988a932458033b82001f05319cb71c4ea1e8c2973867e69383a659a8ce122c7e8016b2493da6352c99192730b",
            "0078f850c7fdf351f9258e0b8d140333fc180460368659c35421d5646491a2c870c82b455f1595d8c2fdf2b4180d149e7956e5a3",
            "00125e37e45a4b55bf40b5752b42f4f9d250873c06e569716e944431cb2245a84bd4f81570b556f796561357efd62a85d6088b63",
            true,
            TestName = "SigVer k-409 sha2-384 Good Signature")]
        [TestCase(Curve.K409, "sha2-384",
            "015b559a8de781d2037c28cfec048503ddd2db3c1b4fa3ba3a865d29af54e8d55b69d02105e6576f36e787530167dd6cf246df8a",
            "005936b463cbf13c0174a04419d858eeb446c10e78e178118fe6ec98c3c2e9f29163974a9f04a9fc47b4007aeec778fcc80423d5",
            "18cb0d5da751779b563c4748f913c568054ec029129766f775f5d8f6c114e5d9a1a64ea65a414ee3df8ac08cde4f2320b91fe039232e2b2f2c42bc2cd7c0c6fb3354e4bf9fbea4ee756ccddad981c348c21d471ba79520ff18e99561524350e9e455df01e1d23e7c2217384184f8f0478097f8dc836c4b97c3144efd00601883",
            "005af10cfd3ce0f76f3d745164ff974de778060bb1436c5f6842b64e76c07388f6db0f23eaf60cef30558c32e33b0143056a03be",
            "007c73c351cd11504291d0802300442a9fa578355e5790b5907ff1dd1a38fabfe3bc0fc67698fb0bd0d4724cbc824d04e6625b95",
            false,
            TestName = "SigVer k-409 sha2-384 Bad Signature")]
        [TestCase(Curve.K571, "sha2-512",
            "058a268f8f6acbf1e6eaf768b41fea5f3fe1de955d217b38aa0a0d177f38fb5555959dfda2c4a5a07017c630a63f1f12b932b49b3f4c63b88df940cc0057be9f751574d411a75911",
            "0685aa85a9acca6202ea39a4ac707e7c71a7d68654aa66877f59d201e027c05c798783a3d249ff7ce5e4ce702f62f83e5d0e211fd549f9e9547175d072a4c69f1f7e6f2fee79a03d",
            "7eede0cfe07fcb63ffe755e6850f51bcac8bb021b6cca4d0f74e2bb94f6e25db03dc9caf1000c12bc1c9a58cbfa6b3f3a2715682d9de53a0c2688deff0402a684cda5801281afb9519b002bdc26bfadb1a676ccad338d2e94a331c8bc61bf642b3f42e09adc3cf6c9fc8cee32c3bc9df5fa4efbc95d2aae043b5c532eed91993",
            "0156de73d8f47245ff8a48f8a6ea83304e103a95a04b685a9970e13b8e1da8aa4f014fae3779364f9d48509ecc084d8e92b4539296e2fb648519ef517a06fb44cef98aac27336b49",
            "0038c43641aa56f02d511ad64fdf64f18622380ef0d23cb2fcd58e80c8d29e3511764ecde6bcf837f4f51b55a261b5525420602dd00b12c313357b7f88a7d5e0adc3b63cc3543fba",
            true,
            TestName = "SigVer k-571 sha2-512 Good Signature")]
        [TestCase(Curve.K571, "sha2-512",
            "04a40715ac2607c7990ca2c5955068550fb6763672ac357bbf6607db4a9482228a00d9e5102fc16dffd1169182a95136db51738733032d645b88d484e9ca6d3943955e3108ab889c",
            "006dc468d96902bbd40343f97f9fa06b0fe522a5f02bf6aa209034e4526f3d6acf52c3adad16cf0c19244699c4d9a13ad9198167b51ab7e52803fc467be78ae9e33247125c61a791",
            "9d3cfc9bb655ade2053a01e18e4b09ea7efc70f4826c3aba47d89d10dd1f87624fd37d3dd72dd8f44b9aa590123bf5a8b0919b8db79e0a6ef29ec76d72b83804db87dcf371a15caa2687e5ec638602623735b575e0e2694625a4523f185c6ead2c9c2c6c9af91656239e11e9e3a67b54ce87062b62a14c4a42dd9eb38dc431ec",
            "00b63aa01eb8d319cab96807870e31bd6844137472b6fd37eddd22834f6dd50dd36f98e0db573560696e49d307758d1cb650af00434680dc03195dbfa037f6896789df391bb47651",
            "009f6be893285e402fb52d7ef861386f05f8756b03982e06ed55577f2f42fe412442c2ac50a2560fead9bf6af1d9a3c9a312f6c7cd339e4a1eb2348ef32e866d712ca8e402760133",
            false,
            TestName = "SigVer k-571 sha2-512 Bad Signature")]
        #endregion SigVer-K
        #region SigVer-B
        [TestCase(Curve.B163, "sha-1",
            "0524dbd975aa32058697369ee4bf7d4235d3119bca",
            "02ef5b1dc73c3b4fe92d9df5d1350c4f00b60a7f84",
            "cf595acef2afc0dcb73729a99485611e553625bb6f95111241df2e05a694365d35005ceb7a27e6ca58817c6ff94872b6c0c1cbce121d37d8a01a468787e6337d9402f935b68b70835df8a5d72de847d7c37e62a282095002d3bb8e03866b7fa81d00202c7a60e1604943b3947bd6b60c1a44c71897bed9bf07ac448d30469346",
            "02a97d1c5e39c96d027b62306c0b9ecdc6d0005b49",
            "01ac3d253190ad17f3981ecc34cf4dd04d9444f1c6",
            true,
            TestName = "SigVer b-163 sha-1 Good Signature")]
        [TestCase(Curve.B163, "sha-1",
            "04c17763b41b908d5996b2ca81948e8ad0ce1a9b07",
            "0623eb1eb594618fd2b8bdfc634c0d6a6178d42f95",
            "5f8a328003fb66a42280ccfd95e979c27cb8e273b9b88374786335f2193cc17657c8e91d17cf550ae412850568b24c6296bef7ea0bac4bd369b24a59d7b5db6621b95be1532fd2cf78962f77c1250128e44d38b773ac47ba354da2db258491c65bb3575ed6cadd62c6c0a23b3e94ea47fdf9dbabbdc225fb97b61a0b43916340",
            "019430563f1d4f9cb15a69a518982bac9a33615406",
            "0153b8d92893fdf3d5464fc5f809759be68a559efe",
            false,
            TestName = "SigVer b-163 sha-1 Bad Signature")]
        [TestCase(Curve.B233, "sha2-224",
            "01b57c565a6954dfa86a0b14787b156951fbf9e57d38decb6023b91b6609",
            "010e7f602ea0af12fee22cb3b49577c32ea9aa1607e95b0c33f17d2855f1",
            "7243e6a8e966e138fac8f4a757aba92b96baaaebc05e665ac3b2b391da505e80f1e957a1e17f8a1a39756eb5b666eeb6344457e90c60921f863795a791708b009562b72334520b533be64af343c344fd73ee8f59de16146ca3b417e6430980ddf2e47f04f1b169c1de5a5932ed576ab67c4e11202a3639fdac63b5046b976675",
            "000bd52ecfa95f6b4e3a338193aca06b6e26f5933e483f35d16954a42385",
            "00ca6ec1f6969358a6847d99bc417817ea4dcb6f769889959d155de6662e",
            true,
            TestName = "SigVer b-233 sha2-224 Good Signature")]
        [TestCase(Curve.B233, "sha2-224",
            "01733c844ae203484c9047eacba786cd82455334bc3bf996338aa366f419",
            "015c2e37144be4fff0a03f75c957b0f1a7058cc5f0986c9dbbebf3bf2184",
            "2c7f61cc29d24432c3b61a6a90ae84322d75e6c9f14dfd7d924a7dac6d430157648cd909372ac142e70ad371b3205881fa006f3f14983ccddd7f4e0bf57ef12c5aba582f055f793efab775c026091779d89a8294795a80768cd360433b39ecd7cae3f6bc093a21d20b3fb90a5eb379b6ffeb76b103560d777ea071431c457d3f",
            "00b227e523c3bdf8d6debdd41fd68e193173cb50bab173bde3887196b004",
            "00b9f964568362a1c2af5dc3326d8a42abbec0d0abc7cfe6d01ff13d99bc",
            false,
            TestName = "SigVer b-233 sha2-224 Bad Signature")]
        [TestCase(Curve.B283, "sha2-256",
            "06f2d1642bf9e9819f1a35960787d1bc3d83ecd7fff2123139fd03d8110a086c7878d444",
            "0578414d960307b719a76841147b645b4aac29bedc3702995084b6314fb80b9324f19bd4",
            "ee0cb76f8107836222bf2cab38f9e63189a76737f1a3c0714cabf18115f7548dc8db5e3029e321495774d472403c94584bd11a8036a4287e8096f3871b5c0b37426acd5cba6c65ce40e1e71d6c700acbf4d59e01af59f48f465cc7df46740131ce9b541e2576d08e29ce7e757a6c5b4844ababc46b0d48a1a3d4c618aadbebbf",
            "033299bc17a7855f2fe909304b5bf6424e66699f360ef823c3d9de213635e3e929fb5e65",
            "0379040cb6191352cf75006ccc87aaaa6f0ade2aa8d7bdbecd03f65832f8942f48b33a23",
            true,
            TestName = "SigVer b-283 sha2-256 Good Signature")]
        [TestCase(Curve.B283, "sha2-256",
            "0704854a3f8ebc1f83d13f1be9e5db37ebb53b090d5e36b1f54ac23fa45f68d5a4688c80",
            "00270e6166ad949697be018dc985368014d3bb73f3e67a614c7155c74e52aa07d208f750",
            "5f663647eba953d2c9b740684770117ad174ff2fae6b76c84b4d8a3c6c709b678548f82a42141b598bdad06946537336532d36e8130a503295ef94619398865566f93895d1f3391148e711799190acde9c6e34355dcd1dcee7fd0612a3fde42a8270fa4f09be83d06b8ddbb62f21fcbc8a43f388edc8e801d770c34cfd4fd3be",
            "0056042dd4b68831521cfd60760a7315cf13829a0945fd8fa427b5839ba8107489e60252",
            "00e00e434c83dc8e0187d85a9f9cd03b47c53006d523c581578caa498fc7d60577e69eee",
            false,
            TestName = "SigVer b-283 sha2-256 Bad Signature")]
        [TestCase(Curve.B409, "sha-1",
            "00e0e4fa090ab0cddaf24f6cc519b6d4dbfea0dbb697fa9b16bfbe2e3e3f160ac37ebc4fd886ecb0a0671f885227a796cb7fa00f",
            "015e44beb435ed5b625df0fc0a74376b185cad91ccf8d9c92981f7b0fde6e2a7f4fadf301f1ad3a5df62dd477e2871c4a9ea74f1",
            "3036da2fa56650429ee90c189888c37c5ae10ff42087e26454ef436a8717522980313db5c49d26ef9a18bca19c35f04b22e67f4d3c5d4a94a4846377f2e9e52ddc8659b03de9716366d36dd9107b607360a9a7bb3645b76bbecef229241828c254f855f19e4c47834caa54eb64db07717d80422ab050352dfbc0d8f9c1b2b0d2",
            "001a1ef5fff035efad063640c75eefc985cd29d2d42dcfb387ab461f70108d9b367091097dd6a36983abbbe39ffb11ab12c9c108",
            "0095fa5a326613fcdaf44555c62d1a08c5b937ac1c774f05992da7d10f254d00df109ef194b72b250ebc04c6819220411d9be44f",
            true,
            TestName = "SigVer b-409 sha-1 Good Signature")]
        [TestCase(Curve.B409, "sha-1",
            "002c3bdc458ef796b9755fa3393f4502b4fa95a8542699128ee76617248fa33acd2b86d4a777056537385e9f8dfde007ab6f7f8f",
            "0007d1062be51d255717d1142c5cbce59cf72f1c456515d666a1f3594f5d2454f908ba9b82354497368cbd4a6c95b6703e7cd0e3",
            "b4626b2954716ad518154f02a83163669f0f616ac9977ac7deffc504221abc8416b06c46c3bf1ad84d150661203a6d8416480b14b10893dc3858513746cb2ba28b96b4bcdcdc30c75cfbac3a9cf0c4f98f55e6cbc2d430ca64d68882b86adfa88d081533a1eb1fabfaae763bf81ac74f2ef1930c137c54521c083dc96b72b1ae",
            "0029b4d7a0e95456c21c6f6b59de9adb415e725114c6dd981d8473fe69852922e1a15909b4fad11b23257d1f4ca4fb32065100ab",
            "0019497b34be56fd4c240e3f4a8a602566d294a5ebdabd1db46fbf49c2ddc8a4dd6211f63538bf4b4caf29f1ac5320e792b591b9",
            false,
            TestName = "SigVer b-409 sha-1 Bad Signature")]
        [TestCase(Curve.B571, "sha-1",
            "0796e57aa6bca619785b91240aa10ff57ae0c73f807f156546639e045e4b6f54a293b133d8f3955e1e380cf60cfa622f897b74e8a06baa03fa54857f8bc4a4f50b5f1fa74ac6b64b",
            "078f9d3f66eb67579b6ad73fcb0f34402e462e05275fe584e709f1a2fb6f9c9f590adeb72dc9bcef306150d207c7344788312e73647491b3ec4d35334130c7fc42e01548e2eb654c",
            "188fab6788a134eae93b62c3e3556d6047123b7b273907512af4d61777f408dc50585a9eeb41c310f4e8d7f26a3c562cf65250c42bfb4c4e5c28170ee79566924f9af46936c635fbc5128c1d3cb46b3f2ee2f6c967a081e345f561bc9c534c77901ad238cc60355e1e04691bf0a66dc71a51dea507af6775408a30dcd1c7fff6",
            "02691c347cc3efabb7216fb96375f1b3130bde054089b3891a52393307acb5e9f405bea8ecf3a4edae775511e72075c65fb04a47b9f6518c2541fa6902a513ac69184f8297c9a72a",
            "00133375d15a1017a62b6912470c88bf74c4ebab65d4754bd1bf419e58e5f8610a92349c82d8a5811243081d4ae1762206096619f50327f608910d40f0868726eb3f9369bfd1d37c",
            true,
            TestName = "SigVer b-571 sha-1 Good Signature")]
        [TestCase(Curve.B571, "sha-1",
            "02d7bb1f66eaec470fe4c68a867e2d315328182d22230154b2d77226bd820b4a23e9d7b9c2df04cde63ea4d64fa9ac63d1af17bf4cc5f529a7ead164304bbb46d36ef4142bafcf29",
            "065039196f74ffbe3908377263cd0c6656080eb640013039b8f2b5c9ee7a5c3155518d1c5ce32c11e05a3b6b7b489997add843e81cbb9e8411e2fba372c160359cd9113709c6d235",
            "e7822cdd238ccf58ff343fbf61895fa50dcc6fe3170094368bbf7b1552a7c406ea54fed69a1b84ff582a78409f665da1560a79d9925d98dc16cda10083b6f707ab05bdad47aab84790f88550a02a56f07c2dc4d3884df8cfc4c019252c6122fb30f539268742b77e2b9d50575441834542727f2e983f7cdfb7327a0b0c3c7c73",
            "021920d9ce54bbe8fd6f4c6edf79cbd6f6eaed206f63ae63ba4eeb801052fa82dc0d12ccb0d47e5abbb02580c50969e7efcccf70d7f2875f9e5d9cac0dcc44f41e7ee0542d6c1b66",
            "010fac7c8bbcc4aeccf17e3365c53836d379f8d7bd21520f093c8bc79ae363803b9237b70430e644f8ee4f3f7f964fca5c3229a5f87f7d0942ce3cd8673d3f0c92360fd2fd84a0b2",
            false,
            TestName = "SigVer b-571 sha-1 Bad Signature")]
        #endregion SigVer-B
        public void ShouldValidateSignaturesCorrectly(Curve curveEnum, string sha, string qXHex, string qYHex, string msgHex, string rHex, string sHex, bool expectedResult)
        {
            var qX = LoadValue(qXHex);
            var qY = LoadValue(qYHex);
            var msg = new BitString(msgHex);
            var r = LoadValue(rHex);
            var s = LoadValue(sHex);

            var Q = new EccPoint(qX, qY);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var keyPair = new EccKeyPair(Q);
            var signature = new EccSignature(r, s);

            var shaFactory = new NativeShaFactory();
            var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(sha);
            var hashFunction = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
            var hash = shaFactory.GetShaInstance(hashFunction);

            var domainParams = new EccDomainParameters(curve);

            var subject = new EccDsa(hash);

            var result = subject.Verify(domainParams, keyPair, msg, signature);

            Assert.AreEqual(expectedResult, result.Success);
        }

        [Test]
        #region SigVer-P-SHA3
        [TestCase(Curve.P224, "sha3-224",
   /* Qx*/  "E84FB0B8E7000CB657D7973CF6B42ED78B301674276DF744AF130B3E",
   /* Qy*/  "4376675C6FC5612C21A0FF2D2A89D2987DF7A2BC52183B5982298555",
   /* msg */"Example of ECDSA with P-224",
   /* R */  "C3A3F5B82712532004C6F6D1DB672F55D931C3409EA1216D0BE77380",
   /* S */  "485732290B465E864A3345FF12673303FEAA4DB68AC29D784BF6DAE2",
            TestName = "SigVer P-224 SHA3-224")]
        [TestCase(Curve.P256, "sha3-256",
   /* Qx*/  "B7E08AFDFE94BAD3F1DC8C734798BA1C62B3A0AD1E9EA2A38201CD0889BC7A19",
   /* Qy*/  "3603F747959DBF7A4BB226E41928729063ADC7AE43529E61B563BBC606CC5E09",
   /* msg */"Example of ECDSA with P-256",
   /* R */  "2B42F576D07F4165FF65D1F3B1500F81E44C316F1F0B3EF57325B69ACA46104F",
   /* S */  "0A861C2526900245C73BACB9ADAEC1A5ACB3BA1F7114A3C334FDCD5B7690DADD",
            TestName = "SigVer P-256 SHA3-256")]
        [TestCase(Curve.P384, "sha3-384",
   /* Qx*/  "3BF701BC9E9D36B4D5F1455343F09126F2564390F2B487365071243C61E6471FB9D2AB74657B82F9086489D9EF0F5CB5",
   /* Qy*/  "D1A358EAFBF952E68D533855CCBDAA6FF75B137A5101443199325583552A6295FFE5382D00CFCDA30344A9B5B68DB855",
   /* msg */"Example of ECDSA with P-384",
   /* R */  "30EA514FC0D38D8208756F068113C7CADA9F66A3B40EA3B313D040D9B57DD41A332795D02CC7D507FCEF9FAF01A27088",
   /* S */  "691B9D4969451A98036D53AA725458602125DE74881BBC333012CA4FA55BDE39D1BF16A6AAE3FE4992C567C6E7892337",
            TestName = "SigVer P-384 SHA3-384")]
        [TestCase(Curve.P521, "sha3-512",
   /* Qx*/  "98E91EEF9A68452822309C52FAB453F5F117C1DA8ED796B255E9AB8F6410CCA16E59DF403A6BDC6CA467A37056B1E54B3005D8AC030DECFEB68DF18B171885D5C4",
   /* Qy*/  "0164350C321AECFC1CCA1BA4364C9B15656150B4B78D6A48D7D28E7F31985EF17BE8554376B72900712C4B83AD668327231526E313F5F092999A4632FD50D946BC2E",
   /* msg */"Example of ECDSA with P-521",
   /* R */  "0140C8EDCA57108CE3F7E7A240DDD3AD74D81E2DE62451FC1D558FDC79269ADACD1C2526EEEEF32F8C0432A9D56E2B4A8A732891C37C9B96641A9254CCFE5DC3E2BA",
   /* S */  "B25188492D58E808EDEBD7BF440ED20DB771CA7C618595D5398E1B1C0098E300D8C803EC69EC5F46C84FC61967A302D366C627FCFA56F87F241EF921B6E627ADBF",
            TestName = "SigVer P-521 SHA3-512")]
        #endregion SigVer-P-SHA3
        #region SigVer-K-SHA3
        [TestCase(Curve.K233, "sha3-224",
   /* Qx*/  "017C9DD766AEFBE4DE4B15F46DB0671DC4CA0767ED51ECEA94757D9C662E",
   /* Qy*/  "01CDD726084837AE73C11C27D605C6EB2D5E31482358780305C2522B151B",
   /* msg */"Example of ECDSA with K-233",
   /* R */  "3EA7231662E6516F11E37D59D500D70F09E62D64FE6FF445C9B479C8B0",
   /* S */  "4CC71DA49B12F09C1DFABB3D7B5BC03CE4898B5E07B2AA3D4BF6569395",
            TestName = "SigVer K-233 SHA3-224")]
        [TestCase(Curve.K283, "sha3-256",
   /* Qx*/  "01B64A60D4A365409635AAA27E1708D90B839AFA2D9820E12B79C3AF1094B6010AAEF5BE",
   /* Qy*/  "0334B5F30CA21756BDE6D47738F2458F56FBF6BDC76FCFB8F3E591455F041A952EE87A8E",
   /* msg */"Example of ECDSA with K-283",
   /* R */  "01C973D58FD17A06AA8F39D5EC42E0A6B99339C1D57FF6F0EABD04ED57AE35F2E5C95E05",
   /* S */  "01E1A2E8C2CF4E4796B1B3F1C4D7B8F627D53888DF8CB1A7A2A499DC823E6E979AD7B285",
            TestName = "SigVer K-283 SHA3-256")]
        [TestCase(Curve.K409, "sha3-384",
   /* Qx*/  "0127065590DF9265FDFBA4ED6EDF76A9BC8CE880B58B6F571A1AB62BA3401269441F3B95ECD0909465022240AE45C7B36A91DE58",
   /* Qy*/  "003C85268D9267302090425BBC14C3D9AE1C1CFC78E0BFCCCFC1FB5DCA5B195C6F8CFBE2D85E4071B71317AA2B0B65C391F82502",
   /* msg */"Example of ECDSA with K-409",
   /* R */  "6F421A230AA8B471939A77BF4F2C64FC0B4CAA39EDCD06337A92B62BD70A883DFC65C68A9AD33AA5EFB061884D8FEDECD2AC65",
   /* S */  "7116C6CF1FCDA7D13ACD5D3EB8C66D4769A642A6AAE1FA74A4C8F6B9C3CD73C76EA3B6ED489D540C88EA92795EBF32849F3260",
            TestName = "SigVer K-409 SHA3-384")]
        [TestCase(Curve.K571, "sha3-512",
   /* Qx*/  "04D9CFE0A7338FEA703E007F5D10BABD2DF3F319B47DF1E23C4F7E5ABF5014C1390B78F117E6AF8258A48F56ACB9FAAC788530B5CCDB1AB7E9390EC5DD7A39D5EEAF6C41BF50AC76",
   /* Qy*/  "064732C504F81DC5F9B0E882B6DA46E124E8241358F077896D25ECF028AD0E6011993C85E68741A07D7817C400CF94B1A3F524F48668B5B970972618616DB4362A769D16CAC34BF0",
   /* msg */"Example of ECDSA with K-571",
   /* R */  "68758C313FBF1945F775D95B25866DBC8D001D9C7EF4FFA53774E8C68927A52707F034957247CB5717A5B6D84AE6F6C688DBE59859BD6D03B48237476742AFE37CEFE36D5B20EE",
   /* S */  "01186A2C9FC3FF4B334099C16B3F1A96B6B1CE94018909874E2B8379B3C1DEBB9C5FABFAFD0C977F423EABCB5B76B9ECF6BC76D22718CE03E95D7E49ED9A6C27929C693088178359",
            TestName = "SigVer K-571 SHA3-512")]
        #endregion SigVer-K-SHA3
        #region SigVer-B-SHA3
        [TestCase(Curve.B233, "sha3-224",
   /* Qx*/  "01D3AD52D68F8383F582E2BA00F89CE1632211EDC24440C31798E0C8ED40",
   /* Qy*/  "6C3B96CC0E6BC59355A1294E22DBF1D4B9071C28DA1389B6DEBE0E7F43",
   /* msg */"Example of ECDSA with B-233",
   /* R */  "86806715D9620F0A3E62C1BA593D842E41F582B37F39F1E79D2292BD8C",
   /* S */  "D57867ACD82E45EDE4F8B92D0401257C7100F58D143DED7B5813388704",
            TestName = "SigVer B-233 SHA3-224")]
        [TestCase(Curve.B283, "sha3-256",
   /* Qx*/  "0390858E9327A714C74AF0C3ADEDF4E6C75CAFDCC46507A49E415B138A094B6F43E882AC",
   /* Qy*/  "D4A65D973CD150A5221BEDF872A4BA207FF4427DFFFD4827C5BF169E719162504D0631",
   /* msg */"Example of ECDSA with B-283",
   /* R */  "037CB284AC41E72EDA2A93EB8D6DFF58620F7CD99B927EEC7A060A8F6FB7D9265EAFA76F",
   /* S */  "027A943BE3894A44E3EAA2A90CD83883767DBA364A10643BDECBE65C104AE104589BED7A",
            TestName = "SigVer B-283 SHA3-256")]
        [TestCase(Curve.B409, "sha3-384",
   /* Qx*/  "01951C5E41607E9317F247D49A389D0E120F479D47737543098AE5E1BB62BD59DE70E1C584AE655C702D39DD4F7883E1876C4A9B",
   /* Qy*/  "016B16B98A3353D75BEB4D3576C64568BA381463CF77D4AEB85218D2D546E7A1EE3AB9316D8C7DF00D155B7891B2C0BF4B5E942E",
   /* msg */"Example of ECDSA with B-409",
   /* R */  "F3E4DA3101C64239D76831995C0EC1E56CE4690C42DDD53DBF3D147B0173CC15D87E749382CD5EBD9492FD568F43959763B768",
   /* S */  "58961D556202AFCC840235F736D27F1B4EEC64DFAD18F5450963C75DCB828E68E80425365031E36BFEBBD14320757C594DBBDD",
            TestName = "SigVer B-409 SHA3-384")]
        [TestCase(Curve.B571, "sha3-512",
   /* Qx*/  "310EAD2BEF3DDB84F9FC1777A7EE179FFCB77AAB497BDC00E290597A5FCE306FE419D2F1F208E54850516526DB8E03B0519BEF60E3A3CC8198FBCA8C469ACFE46AB70D5C31874F",
   /* Qy*/  "0373CE6EA68F55D1501D5203ACA03C5AB709A337A8E03B03838F47C06762065FBDD08A102A08C42FF1760145BE54D8606D326EA22A54DF034FAC30988049820BEBA2B0AF9F6404B3",
   /* msg */"Example of ECDSA with B-571",
   /* R */  "E17447E422A2C0085190354AC149210C1137A92C10F9B14D225E6510DA76B19EF44D39390DD9D808C9DFBAE67D9CF0E7BE79A9E72FA8FA1DFE89F43FB6D093A6CEB30E136EABA3",
   /* S */  "0319B6441D2B79089C9797308A8F96EF92B9FA5708A4E7AC3896BF2B8FE9714B95E59EB6007A3FC2A9D706695893F984191B995FD2216AA8A44F9746954FA1F47176D2331F7D3679",
            TestName = "SigVer B-571 SHA3-512")]
        #endregion SigVer-B-SHA3
        public void ShouldValidateSHA3SignaturesCorrectly(Curve curveEnum, string sha, string qXHex, string qYHex, string msgAscii, string rHex, string sHex)
        {
            var qX = LoadValue(qXHex);
            var qY = LoadValue(qYHex);
            var msg = LoadMessage(msgAscii);
            var r = LoadValue(rHex);
            var s = LoadValue(sHex);

            var Q = new EccPoint(qX, qY);

            var factory = new EccCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var keyPair = new EccKeyPair(Q);
            var signature = new EccSignature(r, s);

            var shaFactory = new NativeShaFactory();
            var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(sha);
            var hashFunction = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
            var hash = shaFactory.GetShaInstance(hashFunction);

            var domainParams = new EccDomainParameters(curve);

            var subject = new EccDsa(hash);

            var result = subject.Verify(domainParams, keyPair, msg, signature);

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(Curve.P192)]
        [TestCase(Curve.P224)]
        [TestCase(Curve.P256)]
        [TestCase(Curve.P384)]
        [TestCase(Curve.P521)]
        [TestCase(Curve.K163)]
        [TestCase(Curve.K233)]
        [TestCase(Curve.K283)]
        [TestCase(Curve.K409)]
        [TestCase(Curve.K571)]
        [TestCase(Curve.B163)]
        [TestCase(Curve.B233)]
        [TestCase(Curve.B283)]
        [TestCase(Curve.B409)]
        [TestCase(Curve.B571)]
        public void ShouldValidateExactBitLengthAgainstOrderN(Curve curve)
        {
            var curveFactory = new EccCurveFactory();

            var attributes = CurveAttributesHelper.GetCurveAttribute(curve);

            Assert.AreEqual(attributes.ExactBitLengthOrderN, curveFactory.GetCurve(curve).OrderN.ExactBitLength(), $"{curve}");
        }

        private BigInteger LoadValue(string hex)
        {
            return new BitString(hex).ToPositiveBigInteger();
        }

        private BitString LoadMessage(string message)
        {
            return new BitString(Encoding.ASCII.GetBytes(message));
        }
    }
}
