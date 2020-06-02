using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evasor
{
    public class Module
    {
        public Module(string moduleName, IntPtr baseAddress, uint size)
        {
            this.ModuleName = moduleName;
            this.BaseAddress = baseAddress;
            this.Size = size;
        }

        public string ModuleName { get; set; }
        public IntPtr BaseAddress { get; set; }
        public uint Size { get; set; }
    }
}
