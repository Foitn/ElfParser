using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElfParser
{
    public class ProgramX32
    {
        public uint p_type;
        public uint p_offset;
        public uint p_vaddr;
        public uint p_paddr;
        public uint p_filesz;
        public uint p_memsz;
        public uint p_flags;
        public uint p_align;

        public byte[] MyData;

        public ProgramX32(byte[] sectionBytes, bool bigEndian, byte[] completeFile)
        {
            byte[] p_typeArray = sectionBytes.Take(4).ToArray();
            byte[] p_offsetArray = sectionBytes.Skip(0x04).Take(4).ToArray();
            byte[] p_vaddrArray = sectionBytes.Skip(0x08).Take(4).ToArray();
            byte[] p_paddrArray = sectionBytes.Skip(0x0C).Take(4).ToArray();
            byte[] p_fileszArray = sectionBytes.Skip(0x10).Take(4).ToArray();
            byte[] p_memszArray = sectionBytes.Skip(0x14).Take(4).ToArray();
            byte[] p_flagsArray = sectionBytes.Skip(0x18).Take(4).ToArray();
            byte[] p_alignArray = sectionBytes.Skip(0x1C).Take(4).ToArray();

            if (bigEndian)
            {
                Array.Reverse(p_typeArray);
                Array.Reverse(p_offsetArray);
                Array.Reverse(p_vaddrArray);
                Array.Reverse(p_paddrArray);
                Array.Reverse(p_fileszArray);
                Array.Reverse(p_memszArray);
                Array.Reverse(p_flagsArray);
                Array.Reverse(p_alignArray);
            }

            p_type = BitConverter.ToUInt32(p_typeArray, 0);
            p_offset = BitConverter.ToUInt32(p_offsetArray, 0);
            p_vaddr = BitConverter.ToUInt32(p_vaddrArray, 0);
            p_paddr = BitConverter.ToUInt32(p_paddrArray, 0);
            p_filesz = BitConverter.ToUInt32(p_fileszArray, 0);
            p_memsz = BitConverter.ToUInt32(p_memszArray, 0);
            p_flags = BitConverter.ToUInt32(p_flagsArray, 0);
            p_align = BitConverter.ToUInt32(p_alignArray, 0);

            MyData = completeFile.Skip((int)p_offset).Take((int)p_filesz).ToArray();
            string TheData = System.Text.Encoding.ASCII.GetString(MyData);
            if (TheData.Contains("APBPrescTable"))
            {
                return;
            }
        }
    }
}
