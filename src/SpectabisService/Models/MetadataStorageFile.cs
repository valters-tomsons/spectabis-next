using System.Collections.Generic;
using SpectabisLib.Models;

namespace SpectabisService.Models
{
    public class MetadataStorageFile
    {
        public MetadataStorageFile(IEnumerable<GameMetadata> data)
        {
            Metadata = data;
        }

        public MetadataStorageFile()
        {
        }

        public IEnumerable<GameMetadata> Metadata { get; set; }
    }
}