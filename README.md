[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/AerisG222/NRawTherapee/blob/master/LICENSE.md)
[![NuGet](https://buildstats.info/nuget/NRawTherapee)](https://www.nuget.org/packages/NRawTherapee/)
[![Travis](https://img.shields.io/travis/AerisG222/NRawTherapee.svg)](https://travis-ci.org/AerisG222/NRawTherapee)
[![Coverity Scan](https://img.shields.io/coverity/scan/11286.svg)](https://scan.coverity.com/projects/aerisg222-nrawtherapee)

# NRawTherapee

A .Net library to wrap the functionality of Raw Therapee.

## Motivation
Create a simple wrapper around this excellent program to allow
.Net programs to easily convert RAW image files (like Nikon NEF).

## Using
- Install Raw Therapee
- Add a reference to NRawTherapee in your project.json
- Bring down the packages for your project via `dnu restore`

```csharp
using NRawTherapee;

namespace Test
{
    public class Example
    {
        public void Convert(string file)
        {
            var rt = new RawTherapee(new Options());
            var result = rt.Convert(file);
        }
    }
}
```

- View the tests for more examples
- You also might want to check out NMagickWand which can then help
  working with the generated file from NRawTherapee!

## Contributing
I'm happy to accept pull requests.  By submitting a pull request, you
must be the original author of code, and must not be breaking
any laws or contracts.

Otherwise, if you have comments, questions, or complaints, please file
issues to this project on the github repo.

## Todo
I hope to make many improvements to the library as time permits.
- Add tests
- Investigate options to bundle Raw Therapee
  
## License
NRawTherapee is licensed under the MIT license.  See LICENSE.md for more
information.

## Reference
- Raw Therapee: http://rawtherapee.com/
