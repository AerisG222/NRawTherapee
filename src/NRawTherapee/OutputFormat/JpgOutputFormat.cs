using System;
using System.Collections.Generic;

namespace NRawTherapee.OutputFormat;

public class JpgOutputFormat
    : IOutputFormat
{
    const byte DEFAULT_JPG_QUALITY = 92;

    readonly JpgSubsampling? _subsampling;
    readonly byte? _compression;

    public string FileExtension => ".jpg";

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

        _subsampling = subsampling;
        _compression = compression;
    }

    public string[] ToArguments()
    {
        var args = new List<string>();

        if(_compression == null)
        {
            // rawtherapee-cli indicates 92 is the default, but when just passing -j now, quality is very low (1)
            args.Add($"-j{DEFAULT_JPG_QUALITY}");
        }
        else
        {
            args.Add($"-j{_compression}");
        }

        if(_subsampling != null)
        {
            switch(_subsampling)
            {
                case JpgSubsampling.BestCompression:
                    args.Add("-js1");
                    break;
                case JpgSubsampling.NormalCompression:
                    args.Add("-js2");
                    break;
                case JpgSubsampling.BestQuality:
                    args.Add("-js3");
                    break;
            }
        }

        return args.ToArray();
    }
}
