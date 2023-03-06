using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NRawTherapee;

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

    async Task<Result> RunProcessAsync(string fileName)
    {
        try
        {
            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();

            using var process = new Process();

            process.StartInfo.FileName = Options.RawTherapeePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.ErrorDataReceived += (sender, e) => stdErr.Append(e.Data);
            process.OutputDataReceived += (sender, e) => stdOut.Append(e.Data);

            foreach(var arg in Options.GetArguments(fileName))
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            await process.WaitForExitAsync().ConfigureAwait(false);

            return new Result {
                ExitCode = process.ExitCode,
                StandardOutput = stdOut.ToString(),
                StandardError = stdErr.ToString(),
                OutputFilename = Options.GetTargetOutputFilePath(fileName)
            };
        }
        catch (Win32Exception ex)
        {
            throw new Exception("Error when trying to start the rawtherapee process.  Please make sure rawtherapee is installed, and its path is properly specified in the options.", ex);
        }
    }
}
