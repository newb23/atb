using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ATB.Models;
using ATB.Utilities;
using Buddy.Overlay;
using Buddy.Overlay.Controls;
using ff14bot;

namespace ATB.Views
{
    public partial class Overlay
    {
        public Overlay()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            OverlayLogic.Stop();
        }

        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                var currentDelta = e.Delta;
                if (currentDelta == 120)
                    MainSettingsModel.Instance.OverlayFontSize = MainSettingsModel.Instance.OverlayFontSize + 5;
                else
                {
                    MainSettingsModel.Instance.OverlayFontSize = MainSettingsModel.Instance.OverlayFontSize - 5;
                }
            }

            if (Control.ModifierKeys == Keys.Shift)
            {
                var currentDeltaOpacity = e.Delta;
                if (currentDeltaOpacity == 120 && MainSettingsModel.Instance.OverlayOpacity < 1)
                    MainSettingsModel.Instance.OverlayOpacity = MainSettingsModel.Instance.OverlayOpacity + 0.05;
                else
                {
                    MainSettingsModel.Instance.OverlayOpacity = MainSettingsModel.Instance.OverlayOpacity - 0.05;
                }

                if (MainSettingsModel.Instance.OverlayOpacity <= 0)
                {
                    MainSettingsModel.Instance.OverlayOpacity = .1;
                }

                if (MainSettingsModel.Instance.OverlayOpacity >= 1)
                {
                    MainSettingsModel.Instance.OverlayOpacity = 1;
                }
            }
        }
    }

    public static class OverlayLogic
    {
        public static bool ATBEnemyOverlayIsVisible;

        private static readonly ATBEnemyOverlayUiComponent ATBOverlayComponent = new ATBEnemyOverlayUiComponent(true);

        public static void Start()
        {
            if (!Core.OverlayManager.IsActive)
            {
                Core.OverlayManager.Activate();
            }
            ATBEnemyOverlayIsVisible = true;
            Core.OverlayManager.AddUIComponent(ATBOverlayComponent);
        }

        public static void Stop()
        {
            if (!Core.OverlayManager.IsActive)
                return;

            Core.OverlayManager.RemoveUIComponent(ATBOverlayComponent);
            FormManager.SaveFormInstances();
            ATBEnemyOverlayIsVisible = false;
        }
    }

    internal class ATBEnemyOverlayUiComponent : OverlayUIComponent
    {
        public ATBEnemyOverlayUiComponent(bool isHitTestable) : base(true)
        {
        }

        private OverlayControl _control;

        public override OverlayControl Control
        {
            get
            {
                if (_control != null)
                    return _control;

                var overlayUc = new Overlay();

                _control = new OverlayControl
                {
                    Name = "ATBEnemyOverlay",
                    Content = overlayUc,
                    X = MainSettingsModel.Instance.OverlayX,
                    Y = MainSettingsModel.Instance.OverlayY,
                    AllowMoving = true
                };

                _control.MouseLeave += (sender, args) =>
                {
                    MainSettingsModel.Instance.OverlayX = _control.X;
                    MainSettingsModel.Instance.OverlayY = _control.Y;
                    MainSettingsModel.Instance.Save();
                };

                return _control;
            }
        }
    }
}