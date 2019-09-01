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
                return String.Empty;
            }

            var launchArgument = new StringBuilder();
            var optionEnums = GetSelectedOptionsList(launchOptions);

            foreach (var item in optionEnums)
            {
                if(launchArgument.Length > 0)
                {
                    launchArgument.Append(" ");
                }

                launchArgument.Append($"--{item}");
            }

            return launchArgument.ToString();
        }

        public static string RomPathToArgument(string romPath)
        {
            if(String.IsNullOrWhiteSpace(romPath))
            {
                return String.Empty;
            }

            return $"\"{romPath}\"";
        }

        public static string ConfigurationPathToArgument(string cfgPath)
        {
            if(String.IsNullOrWhiteSpace(cfgPath))
            {
                return String.Empty;
            }

            return $"--cfgpath{cfgPath}";
        }

        private static IEnumerable<string> GetSelectedOptionsList(EmulatorLaunchOptions launchOptions)
        {
            // var enumNames = typeof(EmulatorLaunchOptions).GetEnumNames();
            var enumNames = launchOptions.ToString().Replace(" ", String.Empty).ToLower().Split(',');
            var selectedOptions = new List<string>(enumNames.Length);

            foreach (var option in enumNames)
            {
                Enum.TryParse(option, out EmulatorLaunchOptions parsedEnum);
                if(launchOptions.HasFlag(parsedEnum))
                {
                    selectedOptions.Add(option);
                }
            }

            return selectedOptions;
        }
    }
}