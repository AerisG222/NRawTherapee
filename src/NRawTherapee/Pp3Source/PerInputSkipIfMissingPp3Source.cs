namespace NRawTherapee.Pp3Source
{
    public class PerInputSkipIfMissingPp3Source
        : IPp3Source
    {
        public string[] ToArguments()
        {
            return new string[] { "-S" };
        }
    }
}
