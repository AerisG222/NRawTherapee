using System;
using System.Collections.Generic;

namespace NRawTherapee.OutputFormat;

public class TiffOutputFormat
    : IOutputFormat
{
    readonly short? _bits;
    readonly bool _compression;

    public string FileExtension => ".tif";

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

        _bits = bits;
        _compression = compression;
    }

    public string[] ToArguments()
    {
        var args = new List<string>();

        if(_bits != null)
        {
            args.Add($"-b{_bits}");
        }

        if(_compression)
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
