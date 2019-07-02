using System;
using System.Runtime.InteropServices;

namespace ATB.Utilities
{
    static class NativeMethods
    {
        // ReadProcessMemory
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);
    }
}