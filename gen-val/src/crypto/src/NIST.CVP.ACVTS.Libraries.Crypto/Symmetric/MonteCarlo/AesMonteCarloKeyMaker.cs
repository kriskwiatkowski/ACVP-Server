﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo
{
    public class AesMonteCarloKeyMaker : IMonteCarloKeyMakerAes
    {
        public BitString MixKeys(BitString currentKey, BitString lastOutput, BitString secondToLastOutput)
        {
            switch (currentKey.BitLength)
            {
                case 128:
                    return currentKey.XOR(lastOutput.GetMostSignificantBits(128));
                case 192:
                    var mostSignificant16KeyBitStringXor =
                        currentKey.GetMostSignificantBits(64).XOR( // XOR 64 most significant key bits w/
                            secondToLastOutput.Substring(0, 64) // the 64 least significant bits of the previous cipher text
                        );
                    var leastSignificant128KeyBitStringXor = currentKey.GetLeastSignificantBits(128).XOR(lastOutput.GetMostSignificantBits(128));

                    return mostSignificant16KeyBitStringXor.ConcatenateBits(leastSignificant128KeyBitStringXor);
                case 256:
                    var mostSignificantFirst16BitStringXor = currentKey.GetMostSignificantBits(128).XOR(secondToLastOutput.GetMostSignificantBits(128));
                    var leastSignificant16BitStringXor = currentKey.GetLeastSignificantBits(128).XOR(lastOutput.GetMostSignificantBits(128));

                    return mostSignificantFirst16BitStringXor.ConcatenateBits(leastSignificant16BitStringXor);
                default:
                    throw new ArgumentException(nameof(currentKey));
            }
        }
    }
}
