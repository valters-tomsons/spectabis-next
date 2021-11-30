using System.IO;
using System.Threading.Tasks;
using FileIntrinsics.Enums;

namespace RomParsing.Parsers
{
    public class FakeParser : IParser
    {
        public GameFileType FileType => GameFileType.Fake;

        public async Task<string?> ReadSerial(string filePath)
        {
            var content = await File.ReadAllLinesAsync(filePath, System.Text.Encoding.UTF8).ConfigureAwait(false);
            return content[1];
        }
    }
}