﻿using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public string[] Function { get; set; }
        public string[] Curve { get; set; }
    }
}
