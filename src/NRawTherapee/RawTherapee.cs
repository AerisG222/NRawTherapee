using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;


namespace NRawTherapee
{
    public class RawTherapee
    {
        public Options Options { get; private set; }
        
        
        public RawTherapee(Options options)
        {
            Options = options;
        }
        
        
        public Result Convert(string srcPath)
        {
            return ConvertAsync(srcPath).Result;
        }
        
        
        public Task<Result> ConvertAsync(string srcPath)
        {
            if(!File.Exists(srcPath))
            {
                throw new FileNotFoundException("Please make sure the raw image exists.", srcPath);
            }
            
            return RunProcessAsync(srcPath);
        }
        
        
        // http://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
        Task<Result> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<Result>();
            var process = new Process
            {
                StartInfo = Options.GetStartInfo(fileName),
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                var result = new Result {
                    ExitCode = process.ExitCode,
                    StandardOutput = process.StandardOutput.ReadToEnd(),
                    StandardError = process.StandardError.ReadToEnd(),
                    OutputFilename = Options.GetTargetOutputFilePath(fileName)
                };
                
                tcs.SetResult(result);
                process.Dispose();
            };

            try
            {
                process.Start();
            }
            catch (Win32Exception ex)
            {
                throw new Exception("Error when trying to start the rawtherapee process.  Please make sure rawtherapee is installed, and its path is properly specified in the options.", ex);
            }

            return tcs.Task;
        }
    }
}
