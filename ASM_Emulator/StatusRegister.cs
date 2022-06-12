using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Emulator
{
    internal class StatusRegister
    {
        public byte Value
        {
            get;
            private set;
        }

        public bool Carry
        {
            get
            {
                return (Value & (1 << 0)) == (1 << 0);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 0)) | ((value ? 1 : 0) << 0));
            }
        }

        public bool Zero
        {
            get
            {
                return (Value & (1 << 1)) == (1 << 1);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 1)) | ((value ? 1 : 0) << 1));
            }
        }

        public bool Interrupt
        {
            get
            {
                return (Value & (1 << 2)) == (1 << 2);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 2)) | ((value ? 1 : 0) << 2));
            }
        }

        public bool Decimal
        {
            get
            {
                return (Value & (1 << 3)) == (1 << 3);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 3)) | ((value ? 1 : 0) << 3));
            }
        }

        public bool Break
        {
            get
            {
                return (Value & (1 << 4)) == (1 << 4);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 4)) | ((value ? 1 : 0) << 4));
            }
        }

        public bool Overflow
        {
            get
            {
                return (Value & (1 << 6)) == (1 << 6);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 6)) | ((value ? 1 : 0) << 6));
            }
        }

        public bool Negative
        {
            get
            {
                return (Value & (1 << 7)) == (1 << 7);
            }
            set
            {
                Value = (byte)((Value & ~(1 << 7)) | ((value ? 1 : 0) << 7));
            }
        }
    }
}
