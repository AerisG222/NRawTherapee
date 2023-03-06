using System;
using System.IO;


namespace NRawTherapee.Pp3Source
{
    public class UserSpecifiedPp3Source
        : IPp3Source
    {
        string Source { get; set; }


        public UserSpecifiedPp3Source(string source)
        {
            if(string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(!File.Exists(source))
            {
                throw new FileNotFoundException($"Specified .pp3 file was not found: {source}", source);
            }

            Source = source;
        }


        public string[] ToArguments()
        {
            return new string[] { "-p", Source };
        }
    }
}
