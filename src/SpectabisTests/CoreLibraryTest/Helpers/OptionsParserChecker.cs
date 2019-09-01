using System;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using Xunit;

namespace CoreLibraryTest
{
    public class OptionsParserChecker
    {
        [Fact]
        public void CheckIfOptionsAreParsedCorrectAsArguments()
        {
            EmulatorLaunchOptions options = EmulatorLaunchOptions.Fullboot | EmulatorLaunchOptions.Nohacks;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.True(arguments.Contains("--fullboot"));
            Assert.True(arguments.Contains("--nohacks"));
        }

        [Fact]
        public void CheckIfArgumentOptionsAreSplitWithSpaces()
        {
            EmulatorLaunchOptions options = EmulatorLaunchOptions.Fullboot | EmulatorLaunchOptions.Nohacks;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.True(arguments.Contains("--nohacks --fullboot"));
        }

        [Fact]
        public void CheckIfReturnEmptyWithNoOptions()
        {
            EmulatorLaunchOptions options = 0;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.True(String.IsNullOrEmpty(arguments));
        }
    }
}
