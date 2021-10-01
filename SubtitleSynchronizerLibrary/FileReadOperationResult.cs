using System;
using System.Collections.Generic;
using System.Text;

namespace SubtitleSynchronizerLibrary
{
    public class FileReadOperationResult: OperationResult
    {
        

        public static FileReadOperationResult FromSuccess(string message)
        {
            return new FileReadOperationResult(ResultStatus.SUCCESS, message);
        }

        public static FileReadOperationResult FromFailure(string message)
        {
            return new FileReadOperationResult(ResultStatus.FAILED, message);
        }

        public FileReadOperationResult(ResultStatus result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}
