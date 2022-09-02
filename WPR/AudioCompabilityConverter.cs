using FFMpegCore;

using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace WPR
{
    public static class AudioCompabilityConverter
    {
        public static async Task ScanWmaAndConvert(string rootFolder, Action<int> progressReport, CancellationToken cancelToken)
        {
            var fileEnum = Directory.EnumerateFiles(rootFolder, "*.wma", SearchOption.AllDirectories).ToList();

            int countSoFar = 0;
            int totalCount = fileEnum.Count();

            foreach (var filename in fileEnum)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                string test = Path.ChangeExtension(filename, ".xnb");

                if (!File.Exists(filename + ".xnb") && !File.Exists(Path.ChangeExtension(filename, ".xnb"))) {
                    countSoFar++;
                    progressReport((int)(countSoFar * 100.0 / totalCount));

                    continue;
                }
                
                FileStream headerCheckFile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                
                byte[] Magic = new byte[16] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA,
                    0x00, 0x62, 0xCE, 0x6C };

                byte[] MagicCheck = new byte[16];

                headerCheckFile.Read(MagicCheck, 0, 16);

                if (MagicCheck.SequenceEqual(Magic))
                {
                    string newFilename = filename + ".new.ogg";

                    if (cancelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    bool ok = await FFMpegArguments
                        .FromFileInput(filename)
                        .OutputToFile(newFilename, true, null)
                        .ProcessAsynchronously();

                    if (!ok)
                    {
                        Common.Log.Warn(Common.LogCategory.AppAudioConverter, $"Fail to convert audio file {filename} to ogg!");
                        continue;
                    }

                    headerCheckFile.Dispose();

                    File.Move(filename, filename + ".original", true);
                    File.Move(newFilename, filename, true);

                    countSoFar++;
                    progressReport((int)(countSoFar * 100.0 / totalCount));
                }
            }
        } 
    }
}
