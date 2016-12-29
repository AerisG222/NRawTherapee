using System;
using System.Text;


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
                return ".tiff";
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


        public string ToArgument()
        {
            var sb = new StringBuilder();

            if(Bits != null)
            {
                sb.Append($"-b{Bits} ");
            }

            if(Compression)
            {
                sb.Append("-tz ");
            }
            else
            {
                sb.Append("-t ");
            }

            return sb.ToString();
        }
    }
}
