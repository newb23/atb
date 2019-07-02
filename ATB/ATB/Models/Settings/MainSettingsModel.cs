using ATB.Commands;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using ATB.ViewModels;
using ATB.Utilities.Extensions;

namespace ATB.Models
{
    public class MainSettingsModel : BaseModel
    {
        private static LocalPlayer Me => Core.Player;
        private static MainSettingsModel _instance;
        public static MainSettingsModel Instance => _instance ?? (_instance = new MainSettingsModel());

        private MainSettingsModel() : base(@"Settings/" + Me.Name + "/ATB/Main_Settings.json")
        {
        }

        private bool _autoCommenceDuty, _autoDutyNotify, _usePull, _usePause, _useAutoFace, _useAutoTalk, _useAutoQuest, _useAutoCutscene, _useAutoTargeting,
            _useSmartPull, _useExtremeCaution, _useAutoTpsAdjust, _outputToEcho, _useOverlay, _useToastMessages, _hideOverlayWhenRunning;

        private int _autoCommenceDelay, _tpsAdjust, _overlayFontSize;

        private double _overlayWidth, _overlayHeight, _overlayX, _overlayY, _overlayOpacity;

        private float _maxTargetDistance;
        
        [Setting]
        [DefaultValue(false)]
        public bool AutoDutyNotify
        { get { return _autoDutyNotify; } set { _autoDutyNotify = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool AutoCommenceDuty
        { get { return _autoCommenceDuty; } set { _autoCommenceDuty = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(30)]
        public int AutoCommenceDelay
        { get { return _autoCommenceDelay; } set { _autoCommenceDelay = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UsePull
        { get { return _usePull; } set { _usePull = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(false)]
        public bool UsePause
        { get { return _usePause; } set { _usePause = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoFace
        { get { return _useAutoFace; } set { _useAutoFace = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(100)]
        public double OverlayWidth
        { get { return _overlayWidth; } set { _overlayWidth = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(50)]
        public double OverlayHeight
        { get { return _overlayHeight; } set { _overlayHeight = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(60)]
        public double OverlayX
        { get { return _overlayX; } set { _overlayX = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(60)]
        public double OverlayY
        { get { return _overlayY; } set { _overlayY = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(15)]
        public float MaxTargetDistance
        { get { return _maxTargetDistance; } set { _maxTargetDistance = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoTalk
        { get { return _useAutoTalk; } set { _useAutoTalk = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoQuest
        { get { return _useAutoQuest; } set { _useAutoQuest = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoCutscene
        { get { return _useAutoCutscene; } set { _useAutoCutscene = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(false)]
        public bool UseAutoTargeting
        { get { return _useAutoTargeting; } set { _useAutoTargeting = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseSmartPull
        { get { return _useSmartPull; } set { _useSmartPull = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseExtremeCaution
        { get { return _useExtremeCaution; } set { _useExtremeCaution = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoTpsAdjust
        { get { return _useAutoTpsAdjust; } set { _useAutoTpsAdjust = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(30)]
        public int TpsAdjust
        { get { return _tpsAdjust; } set { _tpsAdjust = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(false)]
        public bool UseOutputToEcho
        { get { return _outputToEcho; } set { _outputToEcho = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseOverlay
        { get { return _useOverlay; } set { _useOverlay = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(1)]
        public double OverlayOpacity
        { get { return _overlayOpacity; } set { _overlayOpacity = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(75)]
        public int OverlayFontSize
        { get { return _overlayFontSize; } set { _overlayFontSize = value; OverlayViewModel.Instance.OverlaySize = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(true)]
        public bool UseToastMessages
        { get { return _useToastMessages; } set { _useToastMessages = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(false)]
        public bool HideOverlayWhenRunning
        { get { return _hideOverlayWhenRunning; } set { _hideOverlayWhenRunning = value; OnPropertyChanged(); } }

        [JsonIgnore]
        private List<string> _combatRoutineList;

        [JsonIgnore]
        public List<string> CombatRoutineList => _combatRoutineList ?? (_combatRoutineList = GetCombatRoutines().ToList());

        private static IEnumerable<string> GetCombatRoutines()
        {
            var retval = new HashSet<string>();
            foreach (var routine in RoutineManager.AllRoutines.Select(x => x.Name).Where(x => x != "InvalidRoutineWrapper" && x != " "))
            {
                var index = routine.IndexOf('[');
                retval.Add(index > 0 ? routine.Substring(0, index) : routine);
            }

            retval.Add(string.Empty);

            return retval;
        }

        private volatile AutoTargetSelection _autoTargetSelection;

        [Setting]
        [DefaultValue(AutoTargetSelection.None)]
        public AutoTargetSelection AutoTargetSelection
        { get { return _autoTargetSelection; } set { _autoTargetSelection = value; OnPropertyChanged(); } }

        [JsonIgnore]
        public ICommand ChangeAutoTargetSelectionCommand => new DelegateCommand(ChangeAutoTargetSelection);

        private void ChangeAutoTargetSelection()
        {
            switch (AutoTargetSelection)
            {
                case AutoTargetSelection.None:
                    AutoTargetSelection = AutoTargetSelection.NearestEnemy;
                    return;

                case AutoTargetSelection.NearestEnemy:
                    AutoTargetSelection = AutoTargetSelection.LowestCurrentHpTanked;
                    if (Me.IsTank()) AutoTargetSelection = AutoTargetSelection.LowestCurrentHp;
                    return;

                case AutoTargetSelection.LowestCurrentHpTanked:
                    AutoTargetSelection = AutoTargetSelection.LowestCurrentHp;
                    return;

                case AutoTargetSelection.LowestCurrentHp:
                    AutoTargetSelection = AutoTargetSelection.LowestTotalHpTanked;
                    if (Me.IsTank()) AutoTargetSelection = AutoTargetSelection.LowestTotalHp;
                    return;

                case AutoTargetSelection.LowestTotalHpTanked:
                    AutoTargetSelection = AutoTargetSelection.LowestTotalHp;
                    return;

                case AutoTargetSelection.LowestTotalHp:
                    AutoTargetSelection = AutoTargetSelection.HighestCurrentHpTanked;
                    if (Me.IsTank()) AutoTargetSelection = AutoTargetSelection.HighestCurrentHp;
                    return;

                case AutoTargetSelection.HighestCurrentHpTanked:
                    AutoTargetSelection = AutoTargetSelection.HighestCurrentHp;
                    return;

                case AutoTargetSelection.HighestCurrentHp:
                    AutoTargetSelection = AutoTargetSelection.HighestTotalHpTanked;
                    if (Me.IsTank()) AutoTargetSelection = AutoTargetSelection.HighestTotalHp;
                    return;

                case AutoTargetSelection.HighestTotalHpTanked:
                    AutoTargetSelection = AutoTargetSelection.HighestTotalHp;
                    return;

                case AutoTargetSelection.HighestTotalHp:
                    AutoTargetSelection = AutoTargetSelection.TankAssist;
                    if (Me.IsTank()) AutoTargetSelection = AutoTargetSelection.None;
                    return;

                case AutoTargetSelection.TankAssist:
                    AutoTargetSelection = AutoTargetSelection.None;
                    break;
            }
        }

        private SelectedTheme _selectedTheme;

        [Setting]
        [DefaultValue(SelectedTheme.Pink)]
        public SelectedTheme Theme
        { get { return _selectedTheme; } set { _selectedTheme = value; OnPropertyChanged(); } }
    }

    public enum AutoTargetSelection
    {
        None,
        NearestEnemy,
        LowestCurrentHpTanked,
        LowestCurrentHp,
        LowestTotalHpTanked,
        LowestTotalHp,
        HighestCurrentHpTanked,
        HighestCurrentHp,
        HighestTotalHpTanked,
        HighestTotalHp,
        TankAssist
    }

    public enum SelectedTheme
    {
        Blue,
        Pink,
        Green,
        Red,
        Yellow
    }
}