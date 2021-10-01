using System;
using System.Collections.Generic;
using System.IO;

namespace SubtitleSynchronizerLibrary
{
    public class SubtitleSynchronizer
    {
        private  List<SubtitleLine> subtitleLines = new List<SubtitleLine>();
        
        public FileReadOperationResult ReadSubTitleFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return FileReadOperationResult.FromFailure("Arquivo inexistente.");
            }

            if (!Path.GetExtension(filePath).ToUpper().Equals(".SRT"))
            {
                return FileReadOperationResult.FromFailure("Extensão inválida");
            }

            var subTitleLines = File.ReadAllText(filePath).Split(new string[] { "\r\n\r\n" },
                               StringSplitOptions.RemoveEmptyEntries);

            foreach(var line in subTitleLines)
            {
                if (!SubtitleLine.IsLineValid(line))
                {
                    return FileReadOperationResult.FromFailure("O arquivo contém linhas inválidas");
                }

                AddSubtitleLine(SubtitleLine.CreateSubtitleLineFromString(line));
            }

            return FileReadOperationResult.FromSuccess("Arquivo lido com sucesso");
        }

        public SubtitleOffSetAplicationOperationResult ApplyOffsetToTheSubtitleInSeconds(int seconds)
        {
            try
            {
                foreach(var line in subtitleLines)
                {
                    line.AddOffSetInSeconds(seconds);
                }

                return SubtitleOffSetAplicationOperationResult.FromSuccess("Offset aplicado com sucesso");
            }
            catch(Exception ex)
            {
                return SubtitleOffSetAplicationOperationResult.FromFailure("Falha ao adiocionar o intervalo a legenda.");
            }
        }

        public SubtitleOffSetAplicationOperationResult ApplyOffsetToTheSubtitleInMilliseconds(int millisecons)
        {
            try
            {
                foreach (var line in subtitleLines)
                {
                    line.AddOffSetInMilliseconds(millisecons);
                }

                return SubtitleOffSetAplicationOperationResult.FromSuccess("Offset aplicado com sucesso");
            }
            catch (Exception ex)
            {
                return SubtitleOffSetAplicationOperationResult.FromFailure("Falha ao adiocionar o intervalo a legenda.");
            }
        }

        public GenerateFileAndSaveFileOnDiskOperationResult GenerateSubtitleFileWithTheNewOffSet(string newFilePath)
        {
            var text = string.Empty;
            for(int i = 0; i< subtitleLines.Count; i++)
            {
                if (i == subtitleLines.Count -1)
                {
                    text += SubtitleLine.GetLineText(subtitleLines[i], true);
                }
                else
                {
                    text += SubtitleLine.GetLineText(subtitleLines[i], false);
                }
            }

            using (var stream = new StreamWriter(newFilePath))
            {
                stream.Write(text);
            }

            return GenerateFileAndSaveFileOnDiskOperationResult.FromSuccess("Arquivo gravado em disco com sucesso!");
        }

        private void AddSubtitleLine(SubtitleLine line)
        {
            this.subtitleLines.Add(line);
        }


    }
}
