using System.IO;

namespace ElfParser
{
    public class ElfParser
    {

        public void ParseFromFile(string filePath)
        {
            ElfFile e = new ElfFile(File.ReadAllBytes(filePath));
        }
    }
}
