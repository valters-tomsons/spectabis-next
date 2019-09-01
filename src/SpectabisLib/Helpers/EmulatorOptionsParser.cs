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