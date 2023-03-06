using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Medallion.Shell;

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
            var cmd = Command.Run(Options.RawTherapeePath, Options.GetArguments(fileName));

            await cmd.Task.ConfigureAwait(false);

            return new Result {
                ExitCode = cmd.Result.ExitCode,
                StandardOutput = await cmd.StandardOutput.ReadToEndAsync().ConfigureAwait(false),
                StandardError = await cmd.StandardError.ReadToEndAsync().ConfigureAwait(false),
                OutputFilename = Options.GetTargetOutputFilePath(fileName)
            };
        }
        catch (Win32Exception ex)
        {
            throw new Exception("Error when trying to start the rawtherapee process.  Please make sure rawtherapee is installed, and its path is properly specified in the options.", ex);
        }
    }
}
