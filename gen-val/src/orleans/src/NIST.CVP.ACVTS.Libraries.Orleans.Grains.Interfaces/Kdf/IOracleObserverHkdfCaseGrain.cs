﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverHkdfCaseGrain : IGrainWithGuidKey, IGrainObservable<HkdfResult>
    {
        Task<bool> BeginWorkAsync(HkdfParameters param);
    }
}