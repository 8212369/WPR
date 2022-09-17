using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

#if __ANDROID__
using Com.Arthenica.Ffmpegkit;
#else
using FFMpegCore;
#endif

namespace WPR
{
    public static class AudioCompabilityConverter
    {
#if __ANDROID__
        private class FFMPEGConvertSession : Java.Lang.Object, IFFmpegSessionCompleteCallback
        {
            public TaskCompletionSource<FFmpegSession?> CompletionSource;

            public FFMPEGConvertSession()
            {
                CompletionSource = new TaskCompletionSource<FFmpegSession?>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public void Apply(FFmpegSession? session)
            {
                CompletionSource.SetResult(session);
            }

            public Task<FFmpegSession?> Convert(string source, string dest)
            {
                var session = FFmpegKit.ExecuteAsync($"-i \"{source}\" \"{dest}\"", this);

                if (session == null)
                {
                    CompletionSource.SetResult(null);
                    return CompletionSource.Task;
                }

                return CompletionSource.Task;
            }
        }
#endif

        public static async Task ScanWmaAndConvert(string rootFolder, Action<int> progressReport, CancellationToken cancelToken)
        {
#if __MOBILE__
            // I love this kit. Ignore so that the exception stack of Mono is not corrupted
            FFmpegKitConfig.IgnoreSignal(Signal.Sigxcpu);
#endif

            var fileEnum = Directory.EnumerateFiles(rootFolder, "*.wma", SearchOption.AllDirectories).ToList();

            int countSoFar = 0;
            int totalCount = fileEnum.Count();

            foreach (var filename in fileEnum)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

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

#if __ANDROID__
                    var session = await new FFMPEGConvertSession().Convert(filename, newFilename);

                    if (session == null)
                    {
                        continue;
                    }

                    if (!ReturnCode.IsSuccess(session.ReturnCode)) 
                    {
                        continue;
                    }
#else
                    bool ok = await FFMpegArguments
                        .FromFileInput(filename)
                        .OutputToFile(newFilename, true, null)
                        .ProcessAsynchronously();

                    if (!ok)
                    {
                        Common.Log.Warn(Common.LogCategory.AppAudioConverter, $"Fail to convert audio file {filename} to ogg!");
                        continue;
                    }
#endif

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
