﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "LMS";
    public string Mode { get; set; } = "sigGen";
    public string Revision { get; set; } = "1.0";
    public bool IsSample { get; set; }

    public List<TestGroup> TestGroups { get; set; } = new();
}
