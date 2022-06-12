using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class Register
    {
        protected StatusRegister Sr
        {
            get;
            private set;
        }

        public Register(StatusRegister sr)
        {
            Sr = sr;
        }

        public byte Value
        {
            get;
            protected set;
        }
        public void Load(byte value)
        {
            this.Value = value;
        }

        public void Increment()
        {
            this.Value++;
        }

        public void Decrement()
        {
            this.Value--;
        }

        public void Compare(byte value)
        {
            Sr.Zero = (this.Value == value);
        }
    }
}
