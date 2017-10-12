using System;
using System.Collections.Generic;


namespace NRawTherapee.OutputFormat
{
    public class PngOutputFormat
        : IOutputFormat
    {
        short? Bits { get; set; }


        public string FileExtension 
        { 
            get
            {
                return ".png";
            }
        }


        public PngOutputFormat()
            : this(null)
        {

        }


        public PngOutputFormat(short? bits)
        {
            if(bits != null)
            {
                if(!(bits == 8 || bits == 16))
                {
                    throw new ArgumentOutOfRangeException(nameof(bits), "bits must be either 8 or 16");
                }
            }

            Bits = bits;
        }


        public string[] ToArguments()
        {
            var args = new List<string>();

            if(Bits != null)
            {
                args.Add($"-b{Bits}");
            }

            args.Add("-n");

            return args.ToArray();
        }
    }
}
