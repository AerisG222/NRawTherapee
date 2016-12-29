using System;


namespace NRawTherapee.Pp3Source
{
    public class PerInputSkipIfMissingPp3Source
        : IPp3Source
    {
        public string ToArgument()
        {
            return "-S ";
        }
    }
}
