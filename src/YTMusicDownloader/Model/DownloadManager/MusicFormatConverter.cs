﻿using System;
using System.Diagnostics;
using System.IO;

namespace YTMusicDownloader.Model.DownloadManager
{
    class MusicFormatConverter
    {
        public static void M4AToMp3(string filePath, bool deleteOriginal = true)
        {
            if(string.IsNullOrEmpty(filePath) || Path.GetExtension(filePath) != ".m4a")
                throw new ArgumentException(nameof(filePath));

            var toolPath = Path.Combine("tools", "ffmpeg.exe");

            var convertedFilePath = filePath.Replace("m4a", "mp3");
            File.Delete(convertedFilePath);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = toolPath,
#if !DEBUG
                    WindowStyle = ProcessWindowStyle.Hidden,
#endif
                    Arguments = $"-i \"{filePath}\" -acodec libmp3lame -ab 192k \"{convertedFilePath}\""
                }
            };

            process.Start();
            process.WaitForExit();

            if(!File.Exists(convertedFilePath))
                throw new InvalidOperationException("File was not converted successfully!");

            if(deleteOriginal)
                File.Delete(filePath);
        }
    }
}