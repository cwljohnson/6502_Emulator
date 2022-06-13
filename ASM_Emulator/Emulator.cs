using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class Emulator
    {
        public const int MemorySize = UInt16.MaxValue;
        public const int ProgramStart = 0x600;
        public const int StackStart = 0x0100;


        Accumulator A;
        Register X;
        Register Y;
        StatusRegister SR;

        byte[] ram;

        public Emulator()
        {
            ram = new byte[MemorySize];
            SR = new StatusRegister();
            A = new Accumulator(SR);
            X = new Register(SR);
            Y = new Register(SR);
        }

        public UInt16 ProgramCounter
        {
            get;
            private set;
        }

        public UInt16 StackPointer
        {
            get;
            private set;
        }

        public void LoadRaw(string filename)
        {
            int currentAddr = ProgramStart;

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (fs.Position != fs.Length)
                    {
                        ram[currentAddr++] = br.ReadByte();
                    }
                }
            }
        }

        public void Execute()
        {
            ProgramCounter = ProgramStart;
            StackPointer = 0xFF;

            while (ram[ProgramCounter] != 0)
            {
                OpCodes opCode = (OpCodes)ram[ProgramCounter++];

                if (opCode == OpCodes.CLC)
                {
                    SR.Carry = false;
                }
                else if (opCode == OpCodes.JSR)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    UInt16 prevAddr = ProgramCounter;
                    ram[StackStart + StackPointer--] = (byte)(prevAddr >> 8);
                    ram[StackStart + StackPointer--] = (byte)(prevAddr);

                    ProgramCounter = addr;
                }
                else if (opCode == OpCodes.JMP)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    ProgramCounter = addr;
                }
                else if (opCode == OpCodes.RTS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[StackStart + ++StackPointer]) | (UInt16)(ram[StackStart + ++StackPointer] << 8));
                    ProgramCounter = addr;
                }
                else if (opCode == OpCodes.ADC_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    A.Add(value);
                }
                else if (opCode == OpCodes.ADC_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    byte value = ram[addr];
                    A.Add(value);
                }
                else if (opCode == OpCodes.DEX)
                {
                    Y.Decrement();
                }
                else if (opCode == OpCodes.STY_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    ram[addr] = Y.Value;
                }
                else if (opCode == OpCodes.STX_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    ram[addr] = X.Value;
                }
                else if (opCode == OpCodes.STA_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    ram[addr] = A.Value;
                }
                else if (opCode == OpCodes.LDY_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    Y.Load(value);
                }
                else if (opCode == OpCodes.LDX_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    X.Load(value);
                }
                else if (opCode == OpCodes.LDA_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    A.Load(value);
                }
                else if (opCode == OpCodes.LDY_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    Y.Load(ram[addr]);
                }
                else if (opCode == OpCodes.LDA_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    A.Load(ram[addr]);
                }
                else if (opCode == OpCodes.LDX_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    X.Load(ram[addr]);
                }
                else if (opCode == OpCodes.CPY_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    Y.Compare(value);
                }
                else if (opCode == OpCodes.CPY_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    Y.Compare(ram[addr]);
                }
                else if (opCode == OpCodes.DEX)
                {
                    X.Decrement();
                }
                else if (opCode == OpCodes.INY)
                {
                    Y.Increment();
                }
                else if (opCode == OpCodes.BNE)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    if (!SR.Zero)
                    {
                        ProgramCounter = addr;
                    }
                }
                else if (opCode == OpCodes.CPX_IMM)
                {
                    byte value = ram[ProgramCounter++];
                    X.Compare(value);
                }
                else if (opCode == OpCodes.INX)
                {
                    X.Increment();
                }
                else if (opCode == OpCodes.NOP)
                {
                    // do nothing
                }
                else if (opCode == OpCodes.CPX_ABS)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    X.Compare(ram[addr]);
                }
                else if (opCode == OpCodes.PRINT_ADDR)
                {
                    UInt16 addr = (UInt16)(((UInt16)ram[ProgramCounter++] << 8) | (UInt16)(ram[ProgramCounter++]));
                    Console.WriteLine("Current Memory from {0:X4}:", addr);
                    PrintRAM(addr,1);
                }
                else if (opCode == OpCodes.PRINT_MEM)
                {
                    Console.WriteLine("Current Memory from {0:X4}:", 0);
                    PrintRAM(0,1);
                }
            }
        }

        public void PrintRAM()
        {
            PrintRAM(0, 8);
        }

        public void PrintRAM(ushort offset,ushort numRows)
        {
            for (int i = 0; i < numRows; i++)
            {
                Console.Write("${0:X4}: ", offset + i * 8);
                for (int j = 0; j < 8; j++)
                {
                    Console.Write("{0:X2} ", ram[offset + i * 8 + j]);
                }
                Console.WriteLine();
            }
        }

        public void PrintInstructions()
        {
            PrintRAM(ProgramStart, 10);
        }
    }
}
