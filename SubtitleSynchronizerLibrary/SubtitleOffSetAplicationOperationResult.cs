using System;
using System.Collections.Generic;
using System.Text;

namespace SubtitleSynchronizerLibrary
{
    public class SubtitleOffSetAplicationOperationResult: OperationResult
    {

        public static SubtitleOffSetAplicationOperationResult FromSuccess(string message)
        {
            return new SubtitleOffSetAplicationOperationResult(ResultStatus.SUCCESS, message);
        }

        public static SubtitleOffSetAplicationOperationResult FromFailure(string message)
        {
            return new SubtitleOffSetAplicationOperationResult(ResultStatus.FAILED, message);
        }

        public SubtitleOffSetAplicationOperationResult(ResultStatus result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}
