﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.SafePrimes
{
    public interface IObserverSafePrimesKeyVerGrain : IGrainWithGuidKey, IGrainObservable<SafePrimesKeyVerResult>
    {
        Task<bool> BeginWorkAsync(SafePrimesKeyVerParameters param);
    }
}