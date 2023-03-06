using System;
using System.Collections.Generic;

namespace NRawTherapee.OutputFormat;

public class PngOutputFormat
    : IOutputFormat
{
    readonly short? _bits;

    public string FileExtension => ".png";

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

        _bits = bits;
    }

    public string[] ToArguments()
    {
        var args = new List<string>();

        if(_bits != null)
        {
            args.Add($"-b{_bits}");
        }

        args.Add("-n");

        return args.ToArray();
    }
}
