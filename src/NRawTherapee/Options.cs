using System;
using System.IO;
using System.Diagnostics;
using System.Text;


namespace NRawTherapee
{
    public class Options
    {
        public string RawTherapeePath { get; set; }
        public Format OutputFormat { get; set; }
        public int JpgCompression { get; set; }
        public string OutputFile { get; set; }
        public string OutputDirectory { get; set; }
        public bool Overwrite { get; set; }
        public bool DoOutputPp3 { get; set; }
        public Pp3Source Pp3Source { get; set; }
        public string UserSpecifiedPp3Source { get; set; }


        public Options()
        {
            RawTherapeePath = "rawtherapee";
            OutputFormat = Format.JpgBestQuality;
        }
        
        
        public ProcessStartInfo GetStartInfo(string rawFile)
        {
            Validate();

            var psi = new ProcessStartInfo();
            
            psi.FileName = RawTherapeePath;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            
            StringBuilder args = new StringBuilder();
            
            if(DoOutputPp3) 
            {
                args.Append($"-O {EscapeFilename(GetTargetOutputFilePath(rawFile))} ");
            }
            else
            {
                args.Append($"-o {EscapeFilename(GetTargetOutputFilePath(rawFile))} ");
            }

            switch(Pp3Source)
            {
                case Pp3Source.ApplicationDefault:
                    args.Append("-d ");
                    break;
                case Pp3Source.PerInput:
                    args.Append("-s ");
                    break;
                case Pp3Source.PerInputSkipIfNotExists:
                    args.Append("-S ");
                    break;
                case Pp3Source.UserSpecified:
                    args.Append($"-p {EscapeFilename(UserSpecifiedPp3Source)} ");
                    break;
            }
            
            var jpgCompression = JpgCompression <= 0 || JpgCompression > 100 ? string.Empty : JpgCompression.ToString();
            var jpgSwitch = $"-j{jpgCompression} ";

            switch(OutputFormat)
            {
                case Format.JpgBestCompression:
                    args.Append(jpgSwitch);
                    args.Append("-js1 ");
                    break;
                case Format.JpgNormalCompression:
                    args.Append(jpgSwitch);
                    args.Append("-js2 ");
                    break;
                case Format.JpgBestQuality:
                    args.Append(jpgSwitch);
                    args.Append("-js3 ");
                    break;
                case Format.Png8Bit:
                    args.Append("-b8 ");
                    args.Append("-n ");
                    break;
                case Format.Png16Bit:
                    args.Append("-n ");
                    break;
                case Format.Tiff8Bit:
                    args.Append("-b8 ");
                    args.Append("-t ");
                    break;
                case Format.Tiff8BitCompressed:
                    args.Append("-b8 ");
                    args.Append("-tz ");
                    break;
                case Format.Tiff16Bit:
                    args.Append("-t ");
                    break;
                case Format.Tiff16BitCompressed:
                    args.Append("-tz ");
                    break;
            }
            
            if(Overwrite)
            {
                args.Append("-Y ");
            }

            args.Append($"-c {EscapeFilename(rawFile)}");
            
            psi.Arguments = args.ToString();
            
            return psi;
        }
        

        internal string GetTargetOutputFilePath(string inputFile)
        {
            if(!string.IsNullOrWhiteSpace(OutputFile))
            {
                return OutputFile;
            }

            var filename = Path.ChangeExtension(Path.GetFileName(inputFile), GetOutputExtension());

            return Path.Combine(OutputDirectory, filename);
        }

        
        string EscapeFilename(string file)
        {
            return $"\"{file}\"";
        }


        string GetOutputExtension()
        {
            switch(OutputFormat)
            {
                case Format.JpgBestCompression:
                case Format.JpgNormalCompression:
                case Format.JpgBestQuality:
                    return ".jpg";
                case Format.Png8Bit:
                case Format.Png16Bit:
                    return ".png";
                case Format.Tiff8Bit:
                case Format.Tiff8BitCompressed:
                case Format.Tiff16Bit:
                case Format.Tiff16BitCompressed:
                    return ".tiff";
            }

            throw new InvalidOperationException("Format not handled!");
        }


        void Validate()
        {
            if(OutputFormat == Format.None) 
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

            if(Pp3Source == Pp3Source.UserSpecified)
            {
                if(string.IsNullOrWhiteSpace(UserSpecifiedPp3Source))
                {
                    throw new InvalidOperationException("You must specify the .pp3 file to use when requesting the UserSpecified option");
                }
                
                if(!File.Exists(UserSpecifiedPp3Source))
                {
                    throw new FileNotFoundException("The path to the specified .pp3 was not found", UserSpecifiedPp3Source);
                }
            }
        }
    }
}
