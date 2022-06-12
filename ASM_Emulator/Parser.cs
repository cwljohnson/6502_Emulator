using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ASM_Emulator
{
    internal class Parser
    {
        public const int ProgramStart = 0x600;

        public Parser()
        {

        }

        public List<Instruction> LoadFile(string filename)
        {
            Console.WriteLine("Reading file {0}:", filename);
            List<Instruction> instructions = new List<Instruction>();

            List<Line> lines = new List<Line>();
            Dictionary<string, ushort> variables = new Dictionary<string, ushort>();

            Regex regexSplit = new Regex("[\t ]+");
            Regex labelRegex = new Regex("([A-z][A-z0-9]*?):");
            Regex variableRegex = new Regex("([A-z][A-z0-9]*?) = \\$([0-9]{4})");

            // read all lines from file
            using (StreamReader sr = new StreamReader(filename))
            {
                string? fileLine = sr.ReadLine();
                Line line = new Line();

                while (fileLine != null)
                {
                    fileLine = fileLine.Trim();

                    if (fileLine.Length != 0)
                    {
                        // preprocess for variables
                        Match m = variableRegex.Match(fileLine);

                        if (m.Success)
                        {
                            // this line is a variable defintion
                            string vName = m.Groups[1].Value;
                            string addrStr = m.Groups[2].Value;

                            ushort addr = UInt16.Parse(m.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
                            variables.Add(vName, addr);
                        } else
                        {
                            // is this line a label
                            m = labelRegex.Match(fileLine);

                            if (m.Success)
                            {
                                // this line is label
                                line.Label = m.Groups[1].Value;
                            } else
                            {
                                // this line is not label
                                line.Value = fileLine;
                                lines.Add(line);
                                line = new Line();
                            }
                        }                        
                    }
                    fileLine = sr.ReadLine();
                }
            }

            ushort addrCounter = ProgramStart;

            // translate each line into an instruction
            foreach (Line line in lines)
            {
                // if line has label, add to variables list
                if (line.Label != null)
                {
                    variables.Add(line.Label, addrCounter);
                }

                Instruction instruction = new Instruction();
                instruction.Label = line.Label;

                string[] cmds = regexSplit.Split(line.Value);
                string opCodeStr = cmds[0];
                

                bool hasAddress = false;

                // increment for op code
                addrCounter += 1;
                instruction.Length = 1;

                if (cmds.Length > 1)
                {
                    string argStr = cmds[1];

                    // read next argument
                    if (argStr[0] == '#')
                    {
                        // literal
                        byte arg;

                        if (argStr[1] == '$')
                        {
                            // hex number
                            arg = byte.Parse(argStr.Substring(2), System.Globalization.NumberStyles.HexNumber);

                        }
                        else
                        {
                            // non hex
                            arg = byte.Parse(argStr.Substring(1), System.Globalization.NumberStyles.Integer);
                        }

                        instruction.Data = arg;
                        addrCounter += 1;
                        instruction.Length += 1;
                    }
                    else if (argStr[0] == '$')
                    {
                        // hex address
                        ushort arg = ushort.Parse(argStr.Substring(1), System.Globalization.NumberStyles.HexNumber);
                        addrCounter += 2;
                        instruction.Length += 2;

                        instruction.Data = arg;
                        hasAddress = true;
                    }
                    else
                    {
                        // this is a variable
                        instruction.DataStr = argStr;
                        // + 2 to addrCounter because variable resolves to 16 bit address
                        addrCounter += 2;
                        instruction.Length += 2;

                        hasAddress = true;
                    }
                }
                OpCodes opCode = OpCodeHelper.GetOpCodeByAttr(opCodeStr, hasAddress);
                instruction.OpCode = opCode;

                instructions.Add(instruction);
            }

            // all variables and labels have been identified
            // replace as needed

            foreach (Instruction instruction in instructions)
            {
                if (instruction.DataStr != null)
                {
                    instruction.Data = variables[instruction.DataStr];
                }
            }

            for (int i = 0; i < instructions.Count; i++)
            {
                Console.WriteLine(instructions[i]);
            }

            return instructions;
        }

        public void WriteByteCode(string filename, List<Instruction> instructions)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    foreach (Instruction instruction in instructions)
                    {
                        bw.Write((byte)instruction.OpCode);

                        if (instruction.Length == 2)
                        {
                            bw.Write((byte)instruction.Data);
                        } else if (instruction.Length == 3)
                        {
                            bw.Write((byte)(instruction.Data >> 8));
                            bw.Write((byte)(instruction.Data));
                        }
                    }
                }
            }
        }

        private Instruction ParseLine(string line)
        {
            //Console.WriteLine(line);

            Instruction instruction = new Instruction();

            Regex regex = new Regex("[\t ]+");

            string[] cmds = regex.Split(line.Trim());
            string opCodeStr = cmds[0];
            string argStr = cmds[1];

            OpCodes opCode = Enum.Parse<OpCodes>(opCodeStr);
            instruction.OpCode = opCode;

            // read next argument
            if (argStr[0] == '#')
            {
                // literal
                byte arg = byte.Parse(argStr.Substring(1), System.Globalization.NumberStyles.HexNumber);
                instruction.Data = arg;
            }
            else if (argStr[0] == '$')
            {
                // address
                UInt16 arg = UInt16.Parse(argStr.Substring(1), System.Globalization.NumberStyles.HexNumber);
                instruction.Data = arg;
            }

            return instruction;
        }
    }
}
