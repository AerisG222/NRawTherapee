using System;
using System.Collections.Generic;
using System.IO;
using NRawTherapee.Pp3Source;
using NRawTherapee.OutputFormat;

namespace NRawTherapee;

public class Options
{
    public string RawTherapeePath { get; set; } = "rawtherapee-cli";
    public IOutputFormat OutputFormat { get; set; } = new JpgOutputFormat();
    public string OutputFile { get; set; }
    public string OutputDirectory { get; set; }
    public bool Overwrite { get; set; }
    public bool DoOutputPp3 { get; set; }
    public List<IPp3Source> Pp3Sources { get; } = new List<IPp3Source>();

    public void AddPp3Source(IPp3Source source)
    {
        Pp3Sources.Add(source);
    }

    public void AddApplicationDefaultPp3Source()
    {
        Pp3Sources.Add(new ApplicationDefaultPp3Source());
    }

    public void AddPerInputPp3Source()
    {
        Pp3Sources.Add(new PerInputPp3Source());
    }

    public void AddPerInputSkipIfMissingPp3Source()
    {
        Pp3Sources.Add(new PerInputSkipIfMissingPp3Source());
    }

    public void AddUserSpecifiedPp3Source(string source)
    {
        Pp3Sources.Add(new UserSpecifiedPp3Source(source));
    }

    public string[] GetArguments(string rawFile)
    {
        Validate();

        var args = new List<string>();

        if(DoOutputPp3)
        {
            args.Add("-O");
            args.Add(GetTargetOutputFilePath(rawFile));
        }
        else
        {
            args.Add("-o");
            args.Add(GetTargetOutputFilePath(rawFile));
        }

        args.AddRange(OutputFormat.ToArguments());

        foreach(var source in Pp3Sources)
        {
            args.AddRange(source.ToArguments());
        }

        if(Overwrite)
        {
            args.Add("-Y");
        }

        args.Add("-c");
        args.Add(rawFile);

        return args.ToArray();
    }

    internal string GetTargetOutputFilePath(string inputFile)
    {
        if(!string.IsNullOrWhiteSpace(OutputFile))
        {
            return OutputFile;
        }

        var filename = Path.ChangeExtension(Path.GetFileName(inputFile), OutputFormat.FileExtension);

        return Path.Combine(OutputDirectory, filename);
    }

    void Validate()
    {
        if(OutputFormat == null)
        {
            throw new InvalidOperationException("You must specify an output format");
        }

        if(string.IsNullOrWhiteSpace(OutputFile) && string.IsNullOrWhiteSpace(OutputDirectory))
        {
            throw new InvalidOperationException("You must specify either an output file or directory");
        }

        if(!string.IsNullOrWhiteSpace(OutputFile) && File.Exists(OutputFile))
        {
            if(!Overwrite)
            {
                throw new InvalidOperationException("The output file already exists, and you did not specify that it can be overwritten");
            }
        }

        if(!string.IsNullOrWhiteSpace(OutputDirectory) && !Directory.Exists(OutputDirectory))
        {
            throw new InvalidOperationException("The output directory must exist before executing");
        }
    }
}
