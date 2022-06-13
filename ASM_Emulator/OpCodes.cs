using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class OpCodeAttribute : Attribute
    {
        public string Name { get; private set; }
        public bool Address { get; private set; }

        public OpCodeAttribute(string name, bool address)
        {
            Name = name;
            Address = address;
        }
    }
    internal enum OpCodes : byte
    {
        [OpCode("CLC", false)]
        CLC = 0x18,
        [OpCode("JSR", true)]
        JSR = 0x20,
        [OpCode("JMP", true)]
        JMP = 0x4C,
        [OpCode("RTS", false)]
        RTS = 0x60,
        [OpCode("ADC", false)]
        ADC_IMM = 0x69,
        [OpCode("ADC", true)]
        ADC_ABS = 0x6D,
        [OpCode("DEY", false)]
        DEY = 0x88,
        [OpCode("STY", true)]
        STY_ABS = 0x8C,
        [OpCode("STX", true)]
        STX_ABS = 0x8E,
        [OpCode("STA", true)]
        STA_ABS = 0x8D,
        [OpCode("LDY", false)]
        LDY_IMM = 0xA0,
        [OpCode("LDX", false)]
        LDX_IMM = 0xA2,
        [OpCode("LDA", false)]
        LDA_IMM = 0xA9,
        [OpCode("LDY", true)]
        LDY_ABS = 0xAC,
        [OpCode("LDA", true)]
        LDA_ABS = 0xAD,
        [OpCode("LDX", true)]
        LDX_ABS = 0xAE,
        [OpCode("CPY", false)]
        CPY_IMM = 0xC0,
        [OpCode("CPY", true)]
        CPY_ABS = 0xCC,
        [OpCode("DEX", false)]
        DEX = 0xCA,
        [OpCode("INY", false)]
        INY = 0xC8,
        [OpCode("BNE", true)]
        BNE = 0xD0,
        [OpCode("CPX", false)]
        CPX_IMM = 0xE0,
        [OpCode("INX", false)]
        INX = 0xE8,
        [OpCode("NOP", false)]
        NOP = 0xEA,
        [OpCode("CPX", true)]
        CPX_ABS = 0xEC,
        [OpCode("PRINT", true)]
        PRINT_ADDR = 0xFE,
        [OpCode("PRINT", false)]
        PRINT_MEM = 0xFF
    }

    internal static class OpCodeHelper
    {
        public static OpCodes GetOpCodeByAttr(string name, bool addr)
        {
            foreach (var item in typeof(OpCodes).GetFields())
            {
                if (Attribute.GetCustomAttribute(item, typeof(OpCodeAttribute)) is OpCodeAttribute attribute)
                {
                    if (string.Equals(attribute.Name, name) && (attribute.Address == addr))
                    {
                        return (OpCodes)item.GetValue(null);
                    }
                }
            }

            throw new ArgumentException($"OpCode with name \"{name}\" and address: {addr} could not be found ");
        }
    }

}
