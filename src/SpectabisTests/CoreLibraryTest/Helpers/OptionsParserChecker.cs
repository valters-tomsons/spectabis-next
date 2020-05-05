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
            const EmulatorLaunchOptions options = EmulatorLaunchOptions.Fullboot | EmulatorLaunchOptions.Nohacks;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.Contains("--fullboot", arguments);
            Assert.Contains("--nohacks", arguments);
        }

        [Fact]
        public void CheckIfArgumentOptionsAreSplitWithSpaces()
        {
            const EmulatorLaunchOptions options = EmulatorLaunchOptions.Fullboot | EmulatorLaunchOptions.Nohacks;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.Contains("--nohacks --fullboot", arguments);
        }

        [Fact]
        public void CheckIfReturnEmptyWithNoOptions()
        {
            const EmulatorLaunchOptions options = 0;

            var arguments = EmulatorOptionsParser.ConvertToLaunchArguments(options);

            Assert.True(string.IsNullOrEmpty(arguments));
        }
    }
}
