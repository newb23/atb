using ATB.Models.Hotkeys;

namespace ATB.Utilities
{
    internal class HotkeyManager
    {
        public static void RegisterHotkeys()
        {
            ATBHotkeysModel.Instance.RegisterAll();
        }

        public static void UnregisterAllHotkeys()
        {
            ATBHotkeysModel.UnregisterAll();
        }
    }
}