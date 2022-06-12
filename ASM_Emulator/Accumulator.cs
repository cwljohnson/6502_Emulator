using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class Accumulator : Register
    {
        public Accumulator(StatusRegister sr) : base(sr)
        {
        }

        public void Add(byte value)
        {
            int result = (int)this.Value + (int)value + (Sr.Carry ? 1 : 0);

            if (result > byte.MaxValue)
            {
                Sr.Carry = true;
            } else
            {
                Sr.Carry = false;
            }

            Value = (byte)(Value + value);
        }
    }
}
