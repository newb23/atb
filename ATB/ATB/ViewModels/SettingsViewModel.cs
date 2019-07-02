using System.Windows.Input;
using ATB.Commands;
using ATB.Models;
using ATB.Utilities;

namespace ATB.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public static MainSettingsModel Settings => MainSettingsModel.Instance;
        public static ICommand OverlayViewUpdate => new DelegateCommand(FormManager.OverlayToggle);
    }
}