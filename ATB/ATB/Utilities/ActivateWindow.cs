using ff14bot;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ATB.Utilities
{
    internal class ActivateWindow
    {
        public static int Ffixvpid = Core.OverlayManager.AttachedProcess.Id;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        internal static void ActivateFfxiv()
        {
            Process p = Process.GetProcessById(Ffixvpid);
            if (p != null)
            {
                SetForegroundWindow(p.MainWindowHandle);
            }
        }
    }
}