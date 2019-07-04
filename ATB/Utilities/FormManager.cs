using ATB.Models;
using ATB.Models.Hotkeys;
using ATB.Views;
using ff14bot;

namespace ATB.Utilities
{
    internal class FormManager
    {
        private static ATBWindow _form;

        public static void SaveFormInstances()
        {
            MainSettingsModel.Instance.Save();
            ATBHotkeysModel.Instance.Save();
        }

        private static ATBWindow Form
        {
            get
            {
                if (_form != null) return _form;
                _form = new ATBWindow();
                _form.Closed += (sender, args) => _form = null;
                return _form;
            }
        }

        public static void OpenForms()
        {
            if (Form.IsVisible)
            {
                Form.Activate();
                return;
            }

            Form.Show();
        }

        internal static void OverlayToggle()
        {
            if (!TreeRoot.IsRunning) return;

            if (!MainSettingsModel.Instance.UseOverlay && OverlayLogic.ATBEnemyOverlayIsVisible)
                OverlayLogic.Stop();

            if (MainSettingsModel.Instance.UseOverlay && !OverlayLogic.ATBEnemyOverlayIsVisible)
                OverlayLogic.Start();
        }
    }
}