using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElfParser
{
    public class ElfFile
    {
        private bool bit64;
        private bool bigEndian;
        private uint entryPoint;
        private uint programHeaderTable;
        private uint sectionHeaderTable;
        private uint headerSize;
        private uint programHeaderSize;
        private uint numberOfEntriesProgramHeader;
        private uint sectionHeaderSize;
        private uint numberOfEntriesSectionHeader;
        private uint indexOfSectionHeaderForSectionNames;



        public ElfFile(byte[] bytes)
        {
            if (bytes[1] != 0x45 && bytes[2] != 0x4C && bytes[3] != 0x46) throw new Exception();
            bit64 = bytes[4] == 0x02;
            bigEndian = bytes[5] == 0x02;
            byte[] entryPointArray = bytes.Skip(0x18).Take(bit64 ? 8 : 4).ToArray();
            byte[] programHeaderTableArray = bytes.Skip(0x18 + (bit64 ? 8 : 4)).Take(bit64 ? 8 : 4).ToArray();
            byte[] sectionHeaderTableArray = bytes.Skip(0x18 + (bit64 ? 8 : 4) * 2).Take(bit64 ? 8 : 4).ToArray();
            byte[] headerSizeArray = bytes.Skip(bit64 ? 0x34 : 0x28).Take(2).ToArray();
            byte[] programHeaderSizeArray = bytes.Skip(bit64 ? 0x36 : 0x2A).Take(2).ToArray();
            byte[] numberOfEntriesProgramHeaderArray = bytes.Skip(bit64 ? 0x38 : 0x2C).Take(2).ToArray();
            byte[] sectionHeaderSizeArray = bytes.Skip(bit64 ? 0x3A : 0x2E).Take(2).ToArray();
            byte[] numberOfEntriesSectionHeaderArray = bytes.Skip(bit64 ? 0x3C : 0x30).Take(2).ToArray();
            byte[] indexOfSectionHeaderForSectionNamesArray = bytes.Skip(bit64 ? 0x3E : 0x32).Take(2).ToArray();

            if (bigEndian)
            {
                Array.Reverse(entryPointArray);
                Array.Reverse(programHeaderTableArray);
                Array.Reverse(sectionHeaderTableArray);
                Array.Reverse(headerSizeArray);
                Array.Reverse(programHeaderSizeArray);
                Array.Reverse(numberOfEntriesProgramHeaderArray);
                Array.Reverse(sectionHeaderSizeArray);
                Array.Reverse(numberOfEntriesSectionHeaderArray);
                Array.Reverse(indexOfSectionHeaderForSectionNamesArray);
            }

            entryPoint = BitConverter.ToUInt32(entryPointArray, 0);
            programHeaderTable = BitConverter.ToUInt32(programHeaderTableArray, 0);
            sectionHeaderTable = BitConverter.ToUInt32(sectionHeaderTableArray, 0);
            headerSize = BitConverter.ToUInt16(headerSizeArray, 0);
            programHeaderSize = BitConverter.ToUInt16(programHeaderSizeArray, 0);
            numberOfEntriesProgramHeader = BitConverter.ToUInt16(numberOfEntriesProgramHeaderArray, 0);
            sectionHeaderSize = BitConverter.ToUInt16(sectionHeaderSizeArray, 0);
            numberOfEntriesSectionHeader = BitConverter.ToUInt16(numberOfEntriesSectionHeaderArray, 0);
            indexOfSectionHeaderForSectionNames = BitConverter.ToUInt16(indexOfSectionHeaderForSectionNamesArray, 0);

            List<SectionX32> sectionHeaders = new List<SectionX32>();
            for (int i = 0; i < numberOfEntriesSectionHeader; i++)
            {
                sectionHeaders.Add(new SectionX32(bytes.Skip((int)sectionHeaderTable + (int)sectionHeaderSize * i).Take((int)sectionHeaderSize).ToArray(), bigEndian, bytes));
            }
            byte[] sectionHeaderNames = sectionHeaders[(int)indexOfSectionHeaderForSectionNames].MyData;
            foreach (SectionX32 s in sectionHeaders)
            {
                s.Name = System.Text.Encoding.ASCII.GetString(sectionHeaderNames.Skip((int)s.sh_name).TakeWhile(x => x != 0x00).ToArray());
            }
            List<SectionX32> variableSection = new List<SectionX32>();
            foreach(SectionX32 s in sectionHeaders)
            {
                if(Encoding.ASCII.GetString(s.MyData).Contains("I2C_HandleTypeDef") && Encoding.ASCII.GetString(s.MyData).Contains("hi2c1"))
                {
                    variableSection.Add(s);
                    string skld = Encoding.ASCII.GetString(s.MyData);
                }
            }

            List<ProgramX32> programHeaders = new List<ProgramX32>();
            for(int i = 0; i < numberOfEntriesProgramHeader; i++)
            {
                programHeaders.Add(new ProgramX32(bytes.Skip((int)programHeaderTable + (int)sectionHeaderSize * i).Take((int)programHeaderSize).ToArray(), bigEndian, bytes));
            }
        }
    }
}
