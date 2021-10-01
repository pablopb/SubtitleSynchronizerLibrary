using System;
using System.Collections.Generic;
using System.Text;

namespace SubtitleSynchronizerLibrary
{
    public class GenerateFileAndSaveFileOnDiskOperationResult: OperationResult
    {

        public static GenerateFileAndSaveFileOnDiskOperationResult FromSuccess(string message)
        {
            return new GenerateFileAndSaveFileOnDiskOperationResult(ResultStatus.SUCCESS, message);
        }

        public static GenerateFileAndSaveFileOnDiskOperationResult FromFailure(string message)
        {
            return new GenerateFileAndSaveFileOnDiskOperationResult(ResultStatus.FAILED, message);
        }

        public GenerateFileAndSaveFileOnDiskOperationResult(ResultStatus result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}
