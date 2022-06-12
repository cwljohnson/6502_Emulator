using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class Instruction
    {
        public OpCodes OpCode
        {
            get;
            set;
        }

        enum DataType
        {
            Value,
            Register,
            Label
        }

        public UInt16 Data
        {
            get;
            set;
        }
        public string? DataStr
        {
            get;
            set;
        }

        public string? Label
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }

        public override string ToString()
        {
            var name = Enum.GetName(typeof(OpCodes), OpCode);

            OpCodeAttribute attr = typeof(OpCodes).GetField(name).GetCustomAttributes(false).OfType<OpCodeAttribute>().SingleOrDefault();
            if (attr != null)
            {
                if (attr.Address)
                {
                    return string.Format("{0} ${1:X4}\t({2})\t{3}", attr.Name, Data, DataStr != null ? DataStr : "", Label == "" ? "" : string.Format("Label: {0}", Label));
                } else
                {
                    return string.Format("{0} #{1:X2}  \t({2})\t{3}", attr.Name, (byte)Data, DataStr != null ? DataStr : "", Label == "" ? "" : string.Format("Label: {0}", Label));
                }
                
            } else
            {
                return string.Format("{0} #{1:X2} ${2:X4} {3}", OpCode, (byte)Data, Data, Label == "" ? "" : string.Format("Label: {0}", Label));
            }           
        }
    }
}
