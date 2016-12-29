using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using NRawTherapee.Pp3Source;
using NRawTherapee.OutputFormat;


namespace NRawTherapee
{
    public class Options
    {
        public string RawTherapeePath { get; set; } = "rawtherapee";
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

        
        public ProcessStartInfo GetStartInfo(string rawFile)
        {
            Validate();

            var psi = new ProcessStartInfo();
            
            psi.FileName = RawTherapeePath;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            
            var args = new StringBuilder();
            
            if(DoOutputPp3) 
            {
                args.Append($"-O {PathUtils.QuoteFilename(GetTargetOutputFilePath(rawFile))} ");
            }
            else
            {
                args.Append($"-o {PathUtils.QuoteFilename(GetTargetOutputFilePath(rawFile))} ");
            }
            
            args.Append(OutputFormat.ToArgument());

            foreach(var source in Pp3Sources)
            {
                args.Append(source.ToArgument());
            }
            
            if(Overwrite)
            {
                args.Append("-Y ");
            }

            args.Append($"-c {PathUtils.QuoteFilename(rawFile)}");
            
            psi.Arguments = args.ToString();
            
            return psi;
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
}
