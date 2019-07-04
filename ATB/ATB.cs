using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ATB.Logic;
using ATB.Models;
using ATB.Utilities;
using ATB.ViewModels;
using ATB.Views;
using ff14bot;
using ff14bot.Managers;
using TreeSharp;
using HotkeyManager = ATB.Utilities.HotkeyManager;

#pragma warning disable 4014
#pragma warning disable CS1998

namespace ATB
{
    public class ATB
    {
        private ATBWindow _form;
        private static int _rbVersion;
        private static DateTime _pulseLimiter;
        //private static readonly string VersionPath = Path.Combine(Environment.CurrentDirectory, @"BotBases\ATB\version.txt");

        public ATB()
        {
        }

        public static void OnButtonPress()
        {
            FormManager.OpenForms();
        }

        public void OnInitialize(int version)
        {
            //Logger.ATBLog($"Initializing Version: {File.ReadAllText(VersionPath)}");
            Logger.ATBLog($"Initializing Version: GitHub 1.0.0");

            _rbVersion = version;

            FormManager.SaveFormInstances();
        }

        public void OnShutdown()
        {
            FormManager.SaveFormInstances();
            HotkeyManager.UnregisterAllHotkeys();
        }

        public Composite GetRoot()
        {
            return Root;
        }

        private Composite _root;

        private Composite Root
        {
            get
            {
                return _root ?? (_root = new Decorator(r => TreeTick()
                                && !MainSettingsModel.Instance.UsePause
                                && Core.Player.IsAlive
                                && ExtremeCaution(),
                                new PrioritySelector(Helpers.Execute(), TargetingManager.Execute(), Tank.Execute(), Healer.Execute(), Dps.Execute())));
            }
        }

        public void Start()
        {
            Logger.ATBLog(@"Starting ATB!");
            if (MainSettingsModel.Instance.UseOverlay && !MainSettingsModel.Instance.HideOverlayWhenRunning)
                OverlayLogic.Start();
            HotkeyManager.RegisterHotkeys();
        }

        public void Stop()
        {
            OverlayLogic.Stop();
            FormManager.SaveFormInstances();
            HotkeyManager.UnregisterAllHotkeys();
        }

        private static bool ExtremeCaution()
        {
            return !MainSettingsModel.Instance.UseExtremeCaution || Utilities.ExtremeCaution.ExtremeCautionTask();
        }

        private static bool TreeTick()
        {
            if (!TreeRoot.IsRunning) return false;
            OverlayViewModel.Instance.IsPausedString = MainSettingsModel.Instance.UsePause ? "ATB Paused" : "ATB Running";

            if (DateTime.Now < _pulseLimiter) return true;
            _pulseLimiter = DateTime.Now.AddSeconds(3);

            AutoDuty.AutoSprint();
            AutoDuty.AutoDutyRoot();
            FormManager.SaveFormInstances();

            if (MainSettingsModel.Instance.UseAutoFace && GameSettingsManager.FaceTargetOnAction == false)
                GameSettingsManager.FaceTargetOnAction = true;

            if (!MainSettingsModel.Instance.UseAutoFace && GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = false;

            //if (!MainSettingsModel.Instance.UseAutoTpsAdjust)
            //{
            if (TreeRoot.TicksPerSecond != (byte)MainSettingsModel.Instance.TpsAdjust)
            {
                TreeRoot.TicksPerSecond = (byte)MainSettingsModel.Instance.TpsAdjust;
            }
            return true;
            //}

            //var currentTpsSetting = MainSettingsModel.Instance.TpsAdjust;

            //switch (_rbVersion)
            //{
            //    case 1:
            //        var currentFps32 = Core.Memory.Read<float>(Core.Memory.Read<IntPtr>(Core.Memory.ImageBase + 0x107e474) + 0x15d0);

            //        if (currentTpsSetting != 30 && currentFps32 >= 55)
            //            MainSettingsModel.Instance.TpsAdjust = 30;

            //        if (currentTpsSetting != 20 && IsWithin(currentFps32, 25, 35))
            //            MainSettingsModel.Instance.TpsAdjust = 20;

            //        if (currentTpsSetting != 10 && IsWithin(currentFps32, 10, 20))
            //            MainSettingsModel.Instance.TpsAdjust = 10;
            //        break;

            //    case 2:
            //        var currentFps64 = Core.Memory.Read<float>(Core.Memory.Read<IntPtr>(Core.Memory.ImageBase + 0x16b7508) + 0x186C);

            //        if (currentTpsSetting != 30 && currentFps64 >= 55)
            //            MainSettingsModel.Instance.TpsAdjust = 30;

            //        if (currentTpsSetting != 20 && IsWithin(currentFps64, 25, 35))
            //            MainSettingsModel.Instance.TpsAdjust = 20;

            //        if (currentTpsSetting != 10 && IsWithin(currentFps64, 10, 20))
            //            MainSettingsModel.Instance.TpsAdjust = 10;
            //        break;

            //    case 3:
            //        var currentFpscn = Core.Memory.Read<float>(Core.Memory.Read<IntPtr>(Core.Memory.ImageBase + 0x1039534) + 0x15c8);

            //        if (currentTpsSetting != 30 && currentFpscn >= 55)
            //            MainSettingsModel.Instance.TpsAdjust = 30;

            //        if (currentTpsSetting != 20 && IsWithin(currentFpscn, 25, 35))
            //            MainSettingsModel.Instance.TpsAdjust = 20;

            //        if (currentTpsSetting != 10 && IsWithin(currentFpscn, 10, 20))
            //            MainSettingsModel.Instance.TpsAdjust = 10;
            //        break;
            //}

            //if (TreeRoot.TicksPerSecond != (byte)MainSettingsModel.Instance.TpsAdjust)
            //{
            //    TreeRoot.TicksPerSecond = (byte)MainSettingsModel.Instance.TpsAdjust;
            //}

            //return true;
        }

        private static bool IsWithin(float num, int min, int max)
        {
            return num > min && num < max;
        }
    }
}