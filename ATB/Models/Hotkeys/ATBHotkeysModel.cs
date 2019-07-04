using ATB.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ATB.Views;
using HotkeyManager = ff14bot.Managers.HotkeyManager;

namespace ATB.Models.Hotkeys
{
    public class ATBHotkeysModel : BaseModel
    {
        private static LocalPlayer Me => Core.Player;
        private static ATBHotkeysModel _instance;
        public static ATBHotkeysModel Instance => _instance ?? (_instance = new ATBHotkeysModel());

        private ATBHotkeysModel() : base(@"Settings/" + Me.Name + "/ATB/ATB_Hotkeys.json")
        {
        }

        private Keys _pause, _changeAutoTarget, _smartPull, _mechanicWarning, _autoTargeting, _autoFace;

        private ModifierKeys _pauseModifier, _changeAutoTargetModifier, _smartPullModifier, _mechanicWarningModifier, _autoTargetingModifier, _autoFaceModifier;

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys PauseKey
        { get { return _pause; } set { _pause = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys PauseModifier
        { get { return _pauseModifier; } set { _pauseModifier = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys ChangeAutoTargetKey
        { get { return _changeAutoTarget; } set { _changeAutoTarget = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys ChangeAutoTargetModifier
        { get { return _changeAutoTargetModifier; } set { _changeAutoTargetModifier = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys SmartPullKey
        { get { return _smartPull; } set { _smartPull = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys SmartPullModifier
        { get { return _smartPullModifier; } set { _smartPullModifier = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys MechanicWarningKey
        { get { return _mechanicWarning; } set { _mechanicWarning = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys MechanicWarningModifier
        { get { return _mechanicWarningModifier; } set { _mechanicWarningModifier = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys AutoTargetingKey
        { get { return _autoTargeting; } set { _autoTargeting = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys AutoTargetingModifier
        { get { return _autoTargetingModifier; } set { _autoTargetingModifier = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys AutoFaceKey
        { get { return _autoFace; } set { _autoFace = value; OnPropertyChanged(); } }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys AutoFaceModifier
        { get { return _autoFaceModifier; } set { _autoFaceModifier = value; OnPropertyChanged(); } }

        public void RegisterAll()
        {
            HotkeyManager.Register("ATB_Pause", PauseKey, PauseModifier, hk =>
            {
                MainSettingsModel.Instance.UsePause = !MainSettingsModel.Instance.UsePause;
                ToastManager.AddToast(MainSettingsModel.Instance.UsePause ? "ATB Paused!" : "ATB Resumed!", TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                if (MainSettingsModel.Instance.UseOverlay)
                {
                    if (MainSettingsModel.Instance.HideOverlayWhenRunning && !MainSettingsModel.Instance.UsePause)
                        OverlayLogic.Stop();
                    else
                        OverlayLogic.Start();
                }

                if (MainSettingsModel.Instance.UseOutputToEcho)
                    ChatManager.SendChat(MainSettingsModel.Instance.UsePause ? "/echo ATB Paused!" : "/echo ATB Resumed!");

                Logger.ATBLog(MainSettingsModel.Instance.UsePause ? "ATB Paused!" : "ATB Resumed!");
            });

            HotkeyManager.Register("ATB_ChangeAutoTarget", ChangeAutoTargetKey, ChangeAutoTargetModifier, hk =>
                {
                    MainSettingsModel.Instance.ChangeAutoTargetSelectionCommand.Execute(null);
                    ToastManager.AddToast(MainSettingsModel.Instance.AutoTargetSelection.ToString(), TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                    if (MainSettingsModel.Instance.UseOutputToEcho)
                        ChatManager.SendChat("/echo " + MainSettingsModel.Instance.AutoTargetSelection + " selected!");

                    Logger.ATBLog(MainSettingsModel.Instance.AutoTargetSelection + " selected!");
                });

            HotkeyManager.Register("ATB_SmartPull", SmartPullKey, SmartPullModifier, hk =>
                {
                    MainSettingsModel.Instance.UseSmartPull = !MainSettingsModel.Instance.UseSmartPull;
                    ToastManager.AddToast(MainSettingsModel.Instance.UseSmartPull ? "Smart Pull Enabled!" : "Smart Pull Disabled!", TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                    if (MainSettingsModel.Instance.UseOutputToEcho)
                        ChatManager.SendChat(MainSettingsModel.Instance.UseSmartPull ? "/echo Smart Pull Enabled!" : "/echo Smart Pull Disabled!");

                    Logger.ATBLog(MainSettingsModel.Instance.UseSmartPull ? "Smart Pull Enabled!" : "Smart Pull Disabled!");
                });

            HotkeyManager.Register("ATB_MechanicWarnings", MechanicWarningKey, MechanicWarningModifier, hk =>
                {
                    MainSettingsModel.Instance.UseExtremeCaution = !MainSettingsModel.Instance.UseExtremeCaution;
                    ToastManager.AddToast(MainSettingsModel.Instance.UseExtremeCaution ? "Mechanic Warnings Enabled!" : "Mechanic Warnings Disabled!", TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                    if (MainSettingsModel.Instance.UseOutputToEcho)
                        ChatManager.SendChat(MainSettingsModel.Instance.UseExtremeCaution ? "/echo Mechanic Warnings Enabled!" : "/echo Mechanic Warnings Disabled!");

                    Logger.ATBLog(MainSettingsModel.Instance.UseExtremeCaution ? "Mechanic Warnings Enabled!" : "Mechanic Warnings Disabled!");
                });

            HotkeyManager.Register("ATB_AutoTargeting", AutoTargetingKey, AutoTargetingModifier, hk =>
            {
                MainSettingsModel.Instance.UseAutoTargeting = !MainSettingsModel.Instance.UseAutoTargeting;
                ToastManager.AddToast(MainSettingsModel.Instance.UseAutoTargeting ? "Auto-Targeting Enabled!" : "Auto-Targeting Disabled!", TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                if (MainSettingsModel.Instance.UseOutputToEcho)
                    ChatManager.SendChat(MainSettingsModel.Instance.UseAutoTargeting ? "/echo Auto-Targeting Enabled!" : "/echo Auto-Targeting Disabled!");

                Logger.ATBLog(MainSettingsModel.Instance.UseAutoTargeting ? "Auto-Targeting Enabled!" : "Auto-Targeting Disabled!");
            });

            HotkeyManager.Register("ATB_AutoFace", AutoFaceKey, AutoFaceModifier, hk =>
            {
                MainSettingsModel.Instance.UseAutoFace = !MainSettingsModel.Instance.UseAutoFace;
                ToastManager.AddToast(MainSettingsModel.Instance.UseAutoFace ? "Auto-Face Enabled!" : "Auto-Face Disabled!", TimeSpan.FromMilliseconds(750), Color.FromRgb(110, 225, 214), Colors.White, new FontFamily("High Tower Text Italic"), new FontWeight(), 52);

                if (MainSettingsModel.Instance.UseOutputToEcho)
                    ChatManager.SendChat(MainSettingsModel.Instance.UseAutoFace ? "/echo Auto-Face Enabled!" : "/echo Auto-Face Disabled!");

                Logger.ATBLog(MainSettingsModel.Instance.UseAutoFace ? "Auto-Face Enabled!" : "Auto-Face Disabled!");
            });
        }

        public static void UnregisterAll()
        {
            HotkeyManager.Unregister("ATB_Pause");
            HotkeyManager.Unregister("ATB_ChangeAutoTarget");
            HotkeyManager.Unregister("ATB_SmartPull");
            HotkeyManager.Unregister("ATB_MechanicWarnings");
            HotkeyManager.Unregister("ATB_AutoTargeting");
            HotkeyManager.Unregister("ATB_AutoFace");
        }
    }
}