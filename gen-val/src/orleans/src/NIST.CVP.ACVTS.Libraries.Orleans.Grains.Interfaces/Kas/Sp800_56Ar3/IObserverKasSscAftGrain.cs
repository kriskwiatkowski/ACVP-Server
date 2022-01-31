﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3
{
    public interface IObserverKasSscAftGrain : IGrainWithGuidKey, IGrainObservable<KasSscAftResult>
    {
        Task<bool> BeginWorkAsync(KasSscAftParameters param);
    }
}