using System;
using System.Text;


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


        public string ToArgument()
        {
            var sb = new StringBuilder();

            if(Bits != null)
            {
                sb.Append($"-b{Bits} ");
            }

            sb.Append("-n ");

            return sb.ToString();
        }
    }
}
