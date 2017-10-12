using System;
using System.Collections.Generic;


namespace NRawTherapee.OutputFormat
{
    public class TiffOutputFormat
        : IOutputFormat
    {
        short? Bits { get; set; }
        bool Compression { get; set; }


        public string FileExtension 
        { 
            get
            {
                return ".tif";
            }
        }


        public TiffOutputFormat()
            : this(null, false)
        {

        }


        public TiffOutputFormat(short? bits, bool compression)
        {
            if(bits != null)
            {
                if(!(bits == 8 || bits == 16))
                {
                    throw new ArgumentOutOfRangeException(nameof(bits), "bits must be either 8 or 16");
                }
            }

            Bits = bits;
            Compression = compression;
        }


        public string[] ToArguments()
        {
            var args = new List<string>();

            if(Bits != null)
            {
                args.Add($"-b{Bits}");
            }

            if(Compression)
            {
                args.Add("-tz");
            }
            else
            {
                args.Add("-t");
            }

            return args.ToArray();
        }
    }
}
