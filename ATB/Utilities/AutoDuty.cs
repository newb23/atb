using ATB.Models;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using System;
using System.Media;

namespace ATB.Utilities
{
    internal class AutoDuty
    {
        public AutoDuty()
        {
            _joinTimeSet = false;
            _commenced = false;
        }

        private static DateTime _joinTime, _autoDutyPulseRejectTime;
        private static bool _dutyReady, _joinTimeSet, _commenced;

        public static bool AutoSprint()
        {
            if (MainSettingsModel.Instance.AutoSprint && ActionManager.IsSprintReady && MovementManager.IsMoving)
            {
                ActionManager.Sprint();
                return true;
            }

            return false;
        }

        public static void AutoDutyRoot()
        {
          

            if (DutyManager.InInstance || (!MainSettingsModel.Instance.AutoDutyNotify && !MainSettingsModel.Instance.AutoCommenceDuty))
            {
                if (DateTime.Now < _autoDutyPulseRejectTime) return;

                _autoDutyPulseRejectTime = DateTime.Now.Add(TimeSpan.FromSeconds(30));

                return;
            }
            if (DutyManager.DutyReady)
            {
                if (!_dutyReady)
                {
                    _dutyReady = true;
                    if (MainSettingsModel.Instance.AutoDutyNotify)
                        Play();
                    Logger.ATBLog(@"Duty is ready!");
                    Reset();
                }

                if (MainSettingsModel.Instance.AutoCommenceDuty)
                    Commence();
            }
            else
                _dutyReady = false;
        }

        public static void Commence()
        {
            if (!_joinTimeSet)
            {
                _joinTime = DateTime.Now.Add(TimeSpan.FromSeconds(MainSettingsModel.Instance.AutoCommenceDelay));
                _joinTimeSet = true;
            }

            if (!_commenced && DateTime.Now > _joinTime && ContentsFinderConfirm.IsOpen)
            {
                DutyManager.Commence();
                _commenced = true;
            }
        }

        public static void Reset()
        {
            _commenced = false;
            _joinTimeSet = false;
        }

        public static void Play()
        {
            SoundPlayer sndplayr = new SoundPlayer(Properties.Resources.DutyReady);
            sndplayr.Play();
        }
    }
}