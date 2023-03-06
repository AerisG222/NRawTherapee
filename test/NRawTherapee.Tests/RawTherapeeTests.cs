using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using NRawTherapee.OutputFormat;


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

            opts.OutputFormat = new JpgOutputFormat();
            PrepareOutputDirectory(opts, "jpg");
            ExecuteTest(opts);

            opts.OutputFormat = new JpgOutputFormat(JpgSubsampling.BestCompression, null);
            PrepareOutputDirectory(opts, "jpg1");
            ExecuteTest(opts);

            opts.OutputFormat = new JpgOutputFormat(JpgSubsampling.NormalCompression, null);
            PrepareOutputDirectory(opts, "jpg2");
            ExecuteTest(opts);

            opts.OutputFormat = new JpgOutputFormat(JpgSubsampling.BestQuality, null);
            PrepareOutputDirectory(opts, "jpg3");
            ExecuteTest(opts);

            opts.OutputFormat = new JpgOutputFormat(null, 78);
            PrepareOutputDirectory(opts, "jpg4");
            ExecuteTest(opts);

            opts.OutputFormat = new PngOutputFormat();
            PrepareOutputDirectory(opts, "png");
            ExecuteTest(opts);

            opts.OutputFormat = new PngOutputFormat(16);
            PrepareOutputDirectory(opts, "png16");
            ExecuteTest(opts);

            opts.OutputFormat = new PngOutputFormat(8);
            PrepareOutputDirectory(opts, "png8");
            ExecuteTest(opts);

            opts.OutputFormat = new TiffOutputFormat();
            PrepareOutputDirectory(opts, "tif");
            ExecuteTest(opts);

            opts.OutputFormat = new TiffOutputFormat(16, false);
            PrepareOutputDirectory(opts, "tif16");
            ExecuteTest(opts);

            opts.OutputFormat = new TiffOutputFormat(16, true);
            PrepareOutputDirectory(opts, "tif16z");
            ExecuteTest(opts);

            opts.OutputFormat = new TiffOutputFormat(8, false);
            PrepareOutputDirectory(opts, "tif8");
            ExecuteTest(opts);

            opts.OutputFormat = new TiffOutputFormat(8, true);
            PrepareOutputDirectory(opts, "tif8z");
            ExecuteTest(opts);
        }


        Options GetPreferredDefaultOptions()
        {
            var opts = new Options();

            // default to a pre-specified profile
            opts.AddUserSpecifiedPp3Source("/usr/share/rawtherapee/profiles/Unclipped.pp3");

            // override the default with any customizations that exist for the input
            opts.AddPerInputPp3Source();

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
                    Console.WriteLine($"cmdline: {string.Join(' ', opts.GetArguments(sourceFile))}");
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
