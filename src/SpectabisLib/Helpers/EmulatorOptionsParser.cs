using System;
using System.Collections.Generic;
using System.Text;
using SpectabisLib.Enums;

namespace SpectabisLib.Helpers
{
    public static class EmulatorOptionsParser
    {
        public static string ConvertToLaunchArguments(EmulatorLaunchOptions launchOptions)
        {
            if(launchOptions == 0)
            {
                return string.Empty;
            }

            var launchArgument = new StringBuilder();
            foreach (var item in GetSelectedOptionsList(launchOptions))
            {
                if(launchArgument.Length > 0)
                {
                    launchArgument.Append(" ");
                }

                launchArgument.Append("--").Append(item);
            }

            return launchArgument.ToString();
        }

        public static string RomPathToArgument(string romPath)
        {
            if(string.IsNullOrWhiteSpace(romPath))
            {
                return string.Empty;
            }

            return $"\"{romPath}\"";
        }

        public static string ConfigurationPathToArgument(Uri cfgContainerLocation)
        {
            return $"--cfgpath{cfgContainerLocation.LocalPath}";
        }

        private static IEnumerable<string> GetSelectedOptionsList(EmulatorLaunchOptions launchOptions)
        {
            var enumNames = launchOptions.ToString().Replace(" ", string.Empty).ToLower().Split(',');
            var selectedOptions = new List<string>(enumNames.Length);

            foreach (var option in enumNames)
            {
                Enum.TryParse(option, out EmulatorLaunchOptions parsedEnum);
                if((launchOptions & parsedEnum) != 0)
                {
                    selectedOptions.Add(option);
                }
            }

            return selectedOptions;
        }
    }
}