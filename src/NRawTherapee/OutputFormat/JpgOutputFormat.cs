using System;
using System.Text;


namespace NRawTherapee.OutputFormat
{
    public class JpgOutputFormat
        : IOutputFormat
    {
        JpgSubsampling? Subsampling { get; set; }
        byte? Compression { get; set; }


        public string FileExtension 
        { 
            get
            {
                return ".jpg";
            }
        }


        public JpgOutputFormat()
            : this(null, null)
        {

        }


        public JpgOutputFormat(JpgSubsampling? subsampling, byte? compression)
        {
            if(compression != null) {
                if(compression <= 0 || compression > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(compression), "Compression must be a value between 1 and 100 (inclusive)");
                }
            }

            Subsampling = subsampling;
            Compression = compression;
        }


        public string ToArgument()
        {
            var sb = new StringBuilder();

            if(Compression == null)
            {
                sb.Append("-j ");
            }
            else
            {
                sb.Append($"-j{Compression} ");
            }

            if(Subsampling != null)
            {
                switch(Subsampling)
                {
                    case JpgSubsampling.BestCompression:
                        sb.Append("-js1 ");
                        break;
                    case JpgSubsampling.NormalCompression:
                        sb.Append("-js2 ");
                        break;
                    case JpgSubsampling.BestQuality:
                        sb.Append("-js3 ");
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
