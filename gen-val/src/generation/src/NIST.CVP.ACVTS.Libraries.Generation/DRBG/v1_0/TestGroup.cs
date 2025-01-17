﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        /// <summary>
        /// Setting this property also updates the "base" equivalent properties of the class.
        /// </summary>
        [JsonIgnore]
        public DrbgParameters DrbgParameters
        {
            get => _drbgParameters;
            set
            {
                _drbgParameters = value;

                Mode = _drbgParameters.Mode;

                DerFunc = _drbgParameters.DerFuncEnabled;
                PredResistance = _drbgParameters.PredResistanceEnabled;
                ReSeed = _drbgParameters.ReseedImplemented;

                EntropyInputLen = _drbgParameters.EntropyInputLen;
                NonceLen = _drbgParameters.NonceLen;
                PersoStringLen = _drbgParameters.PersoStringLen;
                AdditionalInputLen = _drbgParameters.AdditionalInputLen;

                ReturnedBitsLen = _drbgParameters.ReturnedBitsLen;
            }
        }

        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "derFunc")]
        public bool DerFunc { get; set; }

        [JsonProperty(PropertyName = "reSeed")]
        public bool ReSeed { get; set; }

        [JsonProperty(PropertyName = "predResistance")]
        public bool PredResistance { get; set; }

        [JsonProperty(PropertyName = "entropyInputLen")]
        public int EntropyInputLen { get; set; }

        [JsonProperty(PropertyName = "nonceLen")]
        public int NonceLen { get; set; }

        [JsonProperty(PropertyName = "persoStringLen")]
        public int PersoStringLen { get; set; }

        [JsonProperty(PropertyName = "additionalInputLen")]
        public int AdditionalInputLen { get; set; }

        [JsonProperty(PropertyName = "returnedBitsLen")]
        public int ReturnedBitsLen { get; set; }

        [JsonProperty(PropertyName = "mode")]
        public DrbgMode Mode { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        private DrbgParameters _drbgParameters = new DrbgParameters();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            name = name.ToLower();

            if (bool.TryParse(value, out var boolVal))
            {
                switch (name)
                {
                    case "predictionresistance":
                        PredResistance = boolVal;
                        DrbgParameters.PredResistanceEnabled = boolVal;
                        return true;

                    default:
                        return false;
                }
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name)
            {
                case "entropyinputlen":
                    EntropyInputLen = intVal;
                    DrbgParameters.EntropyInputLen = intVal;
                    return true;

                case "noncelen":
                    NonceLen = intVal;
                    DrbgParameters.NonceLen = intVal;
                    return true;

                case "persostringlen":
                    PersoStringLen = intVal;
                    DrbgParameters.PersoStringLen = intVal;
                    return true;

                case "additionalinputlen":
                    AdditionalInputLen = intVal;
                    DrbgParameters.AdditionalInputLen = intVal;
                    return true;

                case "returnedbitslen":
                    ReturnedBitsLen = intVal;
                    DrbgParameters.ReturnedBitsLen = intVal;
                    return true;
            }

            return false;
        }
    }
}
