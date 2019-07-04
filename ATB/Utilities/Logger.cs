using ff14bot.Helpers;
using System.Windows.Media;

namespace ATB.Utilities
{
    internal class Logger
    {
        internal static void ATBLog(string text, params object[] args)
        {
            Logging.Write(Colors.LawnGreen, $@"[ATB] {text}", args);
        }
    }
}