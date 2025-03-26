using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Captain.Common.Utilities
{
    public class NativeMethods
    {
        public static readonly uint GenericWrite = 0x40000000;
        public static readonly uint OpenExisting = 3; 

        // Need to declare the function from the kernel32 
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile(string lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr pSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);  

    }
}
