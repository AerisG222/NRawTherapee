namespace NRawTherapee.OutputFormat
{
    public interface IOutputFormat
    {
        string FileExtension { get; }
        string[] ToArguments();
    }
}
