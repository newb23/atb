using ATB.Models;

namespace ATB.ViewModels
{
    public class OverlayViewModel : BaseViewModel
    {
        private static OverlayViewModel _instance;
        public static OverlayViewModel Instance => _instance ?? (_instance = new OverlayViewModel());

        public MainSettingsModel Settings => MainSettingsModel.Instance;

        private volatile string _isPausedString;
        private int _overlaySize;

        public string IsPausedString
        { get { return _isPausedString; } set { _isPausedString = value; OnPropertyChanged(); } }

        public int OverlaySize
        { get { return _overlaySize; } set { _overlaySize = value; OnPropertyChanged(); } }
    }
}