using System.Collections.Generic;
using SpectabisLib.Enums;

namespace SpectabisLib.Helpers
{
    public static class ContainerConfigTypeParser
    {
        private static readonly Dictionary<ContainerConfigType, string> TypeDirectories = new Dictionary<ContainerConfigType, string>()
        {
            {ContainerConfigType.Root, string.Empty},
            {ContainerConfigType.Inis, "inis"}
        };

        public static string GetTypeDirectoryName(ContainerConfigType type)
        {
            return TypeDirectories[type];
        }
    }
}