using System;


namespace NRawTherapee.Pp3Source
{
    public class ApplicationDefaultPp3Source
        : IPp3Source
    {
        public string ToArgument()
        {
            return "-d ";
        }
    }
}
