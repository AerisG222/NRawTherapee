using System;
using System.Collections.Generic;
using System.IO;
using Xunit;


namespace NRawTherapee.Tests
{
    public class RawTherapeeTests
    {
        const bool KEEP_TEST_RESULTS = true;
        const bool SHOW_COMMAND_LINES = true;
        
        
        List<string> _files = new List<string>();
        
        
        public RawTherapeeTests()
        {
            _files.Add("DSC_0041.NEF");
            _files.Add("DSC_3982.NEF");
            _files.Add("DSC_9762.NEF");
            _files.Add("space test.NEF");
        }
        
        
        [Fact]
        public void NoCustomizationTest()
        {
            var opts = new Options();

            PrepareOutputDirectory(opts, "noargs");

            ExecuteTest(opts);
        }
        
        
        [Fact]
        public void FormatTests()
        {
            var opts = GetPreferredDefaultOptions();
            
            opts.OutputFormat = Format.JpgBestCompression;
            PrepareOutputDirectory(opts, "jpg1");
            ExecuteTest(opts);

            opts.OutputFormat = Format.JpgNormalCompression;
            PrepareOutputDirectory(opts, "jpg2");
            ExecuteTest(opts);

            opts.OutputFormat = Format.JpgBestQuality;
            PrepareOutputDirectory(opts, "jpg3");
            ExecuteTest(opts);

            opts.OutputFormat = Format.Png16Bit;
            PrepareOutputDirectory(opts, "png16");
            ExecuteTest(opts);

            opts.OutputFormat = Format.Png8Bit;
            PrepareOutputDirectory(opts, "png8");
            ExecuteTest(opts);
        }
        

        Options GetPreferredDefaultOptions()
        {
            var opts = new Options();
            
            opts.OutputFormat = Format.JpgBestQuality;
            opts.Pp3Source = Pp3Source.UserSpecified;
            opts.UserSpecifiedPp3Source = "/usr/share/rawtherapee/profiles/Generic/Natural 1.pp3";
            
            return opts;
        }
        

        void PrepareOutputDirectory(Options opts, string testName)
        {
            var di = new DirectoryInfo(testName);

            if(!di.Exists)
            {
                di.Create();
            }

            opts.OutputDirectory = di.FullName;
        }

        
        void ExecuteTest(Options opts)
        {
            foreach(var sourceFile in _files)
            {
                var rt = new RawTherapee(opts);
                
                if(SHOW_COMMAND_LINES)
                {
                    Console.WriteLine($"cmdline: {opts.GetStartInfo(sourceFile).Arguments}");
                }
                
                var result = rt.Convert(sourceFile);
                
                Assert.True(File.Exists(result.OutputFilename));
                
                if(!KEEP_TEST_RESULTS)
                {
                    File.Delete(result.OutputFilename);
                }
            }
        }
    }
}
