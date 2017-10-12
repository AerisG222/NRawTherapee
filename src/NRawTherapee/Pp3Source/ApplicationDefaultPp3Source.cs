namespace NRawTherapee.Pp3Source
{
    public class ApplicationDefaultPp3Source
        : IPp3Source
    {
        public string[] ToArguments()
        {
            return new string[] { "-d" };
        }
    }
}
