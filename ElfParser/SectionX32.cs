using System;
using System.Linq;

namespace ElfParser
{
    public class SectionX32
    {
        public uint sh_name;
        public uint sh_type;
        public uint sh_flags;
        public uint sh_addr;
        public uint sh_offset;
        public uint sh_size;
        public uint sh_link;
        public uint sh_info;
        public uint sh_addralign;
        public uint sh_entsize;

        public byte[] MyData;

        public string Name;

        public SectionX32(byte[] sectionBytes, bool bigEndian, byte[] completeFile)
        {
            byte[] sh_nameArray = sectionBytes.Take(4).ToArray();
            byte[] sh_typeArray = sectionBytes.Skip(0x04).Take(4).ToArray();
            byte[] sh_flagsArray = sectionBytes.Skip(0x08).Take(4).ToArray();
            byte[] sh_addrArray = sectionBytes.Skip(0x0C).Take(4).ToArray();
            byte[] sh_offsetArray = sectionBytes.Skip(0x10).Take(4).ToArray();
            byte[] sh_sizeArray = sectionBytes.Skip(0x14).Take(4).ToArray();
            byte[] sh_linkArray = sectionBytes.Skip(0x18).Take(4).ToArray();
            byte[] sh_infoArray = sectionBytes.Skip(0x1C).Take(4).ToArray();
            byte[] sh_addralignArray = sectionBytes.Skip(0x20).Take(4).ToArray();
            byte[] sh_entsizeArray = sectionBytes.Skip(0x24).Take(4).ToArray();

            if (bigEndian)
            {
                Array.Reverse(sh_nameArray);
                Array.Reverse(sh_typeArray);
                Array.Reverse(sh_flagsArray);
                Array.Reverse(sh_addrArray);
                Array.Reverse(sh_offsetArray);
                Array.Reverse(sh_sizeArray);
                Array.Reverse(sh_linkArray);
                Array.Reverse(sh_infoArray);
                Array.Reverse(sh_addralignArray);
                Array.Reverse(sh_entsizeArray);
            }

            sh_name = BitConverter.ToUInt32(sh_nameArray, 0);
            sh_type = BitConverter.ToUInt32(sh_typeArray, 0);
            sh_flags = BitConverter.ToUInt32(sh_flagsArray, 0);
            sh_addr = BitConverter.ToUInt32(sh_addrArray, 0);
            sh_offset = BitConverter.ToUInt32(sh_offsetArray, 0);
            sh_size = BitConverter.ToUInt32(sh_sizeArray, 0);
            sh_link = BitConverter.ToUInt32(sh_linkArray, 0);
            sh_info = BitConverter.ToUInt32(sh_infoArray, 0);
            sh_addralign = BitConverter.ToUInt32(sh_addralignArray, 0);
            sh_entsize = BitConverter.ToUInt32(sh_entsizeArray, 0);

            MyData = completeFile.Skip((int)sh_offset).Take((int)sh_size).ToArray();
            string TheData = System.Text.Encoding.ASCII.GetString(MyData);
            if (TheData.Contains("APBPrescTable"))
            {
                return;
            }
        }
    }
}
