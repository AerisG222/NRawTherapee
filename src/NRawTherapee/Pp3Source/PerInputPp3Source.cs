namespace NRawTherapee.Pp3Source
{
    public class PerInputPp3Source
        : IPp3Source
    {
        public string[] ToArguments()
        {
            return new string[] { "-s" };
        }
    }
}
