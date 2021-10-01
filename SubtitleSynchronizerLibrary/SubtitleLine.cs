using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleSynchronizerLibrary
{
    public class SubtitleLine
    {
        public int LineNumber { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string[] LineText { get; private set; }

        public static SubtitleLine CreateSubtitleLineFromString(string lineText)
        {
            if (!IsLineValid(lineText))
            {
                throw new ArgumentException("Linha em um formato inválido");
            }

            var fields = lineText.Split(new string[] { System.Environment.NewLine },
                              StringSplitOptions.None);

            
            var lineNumberValid = int.TryParse(fields[0], out var lineNumber);
            var subtitleInterval = fields[1];
            var intervalRegularExpression = @"[0-9]+:[0-9]+:[0-9]+,[0-9]+";
            Regex re = new Regex(intervalRegularExpression);
            var intervals = new List<string>();
            foreach (Match match in re.Matches(subtitleInterval)) {
                intervals.Add(match.Value);
            }

            var startInterval = GetIntervalFromString(intervals[0]);
            var endInterval = GetIntervalFromString(intervals[1]);
            var subTitleText = new List<string>();
            for(int i = 2; i < fields.Length; i++)
            {
                subTitleText.Add(fields[i]);
            }

            return new SubtitleLine(lineNumber, startInterval, endInterval, subTitleText.ToArray());
        }

        private static TimeSpan GetIntervalFromString(string interval)
        {
            var intervals = interval.Split(':');
            var seconds = intervals[2].Split(',')[0];
            var milliseconds = intervals[2].Split(',')[1];

            return new TimeSpan(0, intervals[0].Equals("00")? 0: Convert.ToInt32(intervals[0].TrimStart(new Char[] { '0' })),
                intervals[1].Equals("00") ? 0 : Convert.ToInt32(intervals[1].TrimStart(new Char[] { '0' })),
                seconds.Equals("00") ? 0: Convert.ToInt32(seconds),
                Convert.ToInt32(milliseconds.TrimStart(new Char[] { '0' }))
                );
        }

        internal void AddOffSetInMilliseconds(int milliseconds)
        {
            if (milliseconds == 0)
            {
                throw new ArgumentException("intervalo inválido");
            }
            if (milliseconds > 0)
            {
                StartTime = StartTime.Add(new TimeSpan(0, 0,0,0, milliseconds));
                EndTime = EndTime.Add(new TimeSpan(0, 0, 0, 0, milliseconds));
            }
            else
            {
                StartTime = StartTime.Subtract(new TimeSpan(0, 0, 0, 0, milliseconds * -1));
                EndTime = EndTime.Subtract(new TimeSpan(0, 0, 0, 0, milliseconds * -1));
            }
        }

        public SubtitleLine(int lineNumber, TimeSpan startTime, TimeSpan endTime, string[] lineText)
        {
            LineNumber = lineNumber;
            StartTime = startTime;
            EndTime = endTime;
            LineText = lineText;
        }

        public void AddOffSetInSeconds(int seconds)
        {
           if(seconds == 0)
            {
                throw new ArgumentException("intervalo inválido");
            }
           if(seconds > 0)
            {
                StartTime = StartTime.Add(new TimeSpan(0, 0, seconds));
                EndTime = EndTime.Add(new TimeSpan(0, 0, seconds));
            }
           else
            {
                StartTime = StartTime.Subtract(new TimeSpan(0, 0, seconds * -1));
                EndTime = EndTime.Subtract(new TimeSpan(0, 0, seconds * -1));
            }
        }

        public static bool IsLineValid(string lineText)
        {
            var fields = lineText.Split(new string[] { System.Environment.NewLine },
                               StringSplitOptions.None);

            if(fields.Length < 3)
            {
                return false;
            }
            var lineNumberValid = int.TryParse(fields[0], out var lineNumber);
            var subtitleInterval = fields[1];
            var intervalRegularExpression = @"[0-9]+:[0-9]+:[0-9]+,[0-9]+";
            Regex re = new Regex(intervalRegularExpression);
            var results = re.Matches(subtitleInterval);
            if(results.Count < 2)
            {
                return false;
            }
            return true;
        }

        public static string GetLineText(SubtitleLine line, bool finalLine)
        {
            var subtitleText = string.Empty;
            for(var i = 0; i < line.LineText.Length; i++)
            {
                if(i == line.LineText.Length - 1)
                {
                    subtitleText += line.LineText[i];
                }
                else
                {
                    subtitleText += line.LineText[i] + "\n"; 
                }
            }

            if (finalLine)
            {
                return $"{line.LineNumber}\n{GetFormattedInterval(line)}\n{subtitleText}";
            }

            return $"{line.LineNumber}\n{GetFormattedInterval(line)}\n{subtitleText}\n\n";
        }

        public static string GetFormattedInterval(SubtitleLine line)
        {
           return $@"{line.StartTime.Hours.ToString().PadLeft(2,'0')}:{line.StartTime.Minutes.ToString().PadLeft(2, '0')}:{line.StartTime.Seconds.ToString().PadLeft(2, '0')},{line.StartTime.Milliseconds.ToString().PadLeft(3, '0').Substring(0, 3)} --> {line.EndTime.Hours.ToString().PadLeft(2, '0')}:{line.EndTime.Minutes.ToString().PadLeft(2, '0')}:{line.EndTime.Seconds.ToString().PadLeft(2, '0')},{line.EndTime.Milliseconds.ToString().PadLeft(3, '0').Substring(0, 3)}";
        }
    }
}
