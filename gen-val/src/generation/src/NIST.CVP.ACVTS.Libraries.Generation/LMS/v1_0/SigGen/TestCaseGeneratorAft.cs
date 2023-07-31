﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 20;

    public TestCaseGeneratorAft(IOracle oracle)
    {
        _oracle = oracle;
    }    
    
    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new LmsSignatureParameters
        {
            MessageLength = 1024
        };

        try
        {
            TestCase testCase = null;
            LmsSignatureResult result = null;
            if (isSample)
            {
                // Rely on tree generated by the group
                param.LmsKeyPair = group.KeyPair;
                result = await _oracle.GetLmsSignatureCaseAsync(param);
                testCase = new TestCase { Message = result.Message, Signature = result.Signature, Q = result.Q };
            }
            else
            {
                // Rely on tree generated by the client
                result = await _oracle.GetDeferredLmsSignatureCaseAsync(param);
                testCase = new TestCase { Message = result.Message };
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
        }
    }
    
    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}
