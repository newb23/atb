using ATB.Models;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATB.Utilities.Extensions;
using TreeSharp;
using static ATB.Utilities.Constants;

namespace ATB.Utilities
{
    public static class TargetingManager
    {
        private static readonly Composite TargetingManagerComposite;
        private static DateTime _pulseLimiter;

        static TargetingManager()
        {
            TargetingManagerComposite = new Decorator(r => true, new ActionRunCoroutine(ctx => TargetingManagerTask()));
        }

        public static Composite Execute()
        {
            return TargetingManagerComposite;
        }

        private static async Task<bool> TargetingManagerTask()
        {
            if (!MainSettingsModel.Instance.UseAutoTargeting || MainSettingsModel.Instance.AutoTargetSelection == AutoTargetSelection.None) return false;

            switch (MainSettingsModel.Instance.AutoTargetSelection)
            {
                case AutoTargetSelection.NearestEnemy:
                    if (!Core.Player.HasTarget || PulseCheck())
                    {
                        var target = GetClosestEnemy();
                        if (target != null && target != Me.CurrentTarget)
                        {
                            Logger.ATBLog("Nearest Enemy Target Change!");
                            target.Target();
                        }
                    }

                    break;

                case AutoTargetSelection.LowestCurrentHpTanked:
                    if (Me.IsTank())
                    {
                        Logger.ATBLog("Yer a tank, Harry! Can't assist yerself!");
                        MainSettingsModel.Instance.AutoTargetSelection = AutoTargetSelection.None;
                        break;
                    }

                    if (PartyManager.IsInParty && VisiblePartyMembers.Any(IsTank) && (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck()))
                    {
                        {
                            var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat
                            && ((Character)o).CurrentTargetId == PartyTank.ObjectId);
                            if (objs != null && objs.Any())
                            {
                                var newTarget = objs.OrderBy(o => o.CurrentHealth).First();
                                if (newTarget != Me.CurrentTarget)
                                {
                                    Logger.ATBLog("Lowest Current HP Tanked Target Change!");
                                    newTarget.Target();
                                }
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.LowestCurrentHp:
                    if (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck())
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderBy(o => o.CurrentHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Lowest Current HP Target Change!");
                                newTarget.Target();
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.LowestTotalHpTanked:
                    if (Me.IsTank())
                    {
                        Logger.ATBLog("Yer a tank, Harry! Can't assist yerself!");
                        MainSettingsModel.Instance.AutoTargetSelection = AutoTargetSelection.None;
                        break;
                    }

                    if (PartyManager.IsInParty && VisiblePartyMembers.Any(IsTank) && (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck()))
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o)
                        && ((Character)o).InCombat
                        && ((Character)o).CurrentTargetId == PartyTank.ObjectId);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderBy(o => o.MaxHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Lowest Total HP Tanked Target Change!");
                                newTarget.Target();
                            }
                        }
                    }

                    break;

                case AutoTargetSelection.LowestTotalHp:
                    if (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck())
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderBy(o => o.MaxHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Lowest Total HP Target Change!");
                                newTarget.Target();
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.HighestCurrentHpTanked:
                    if (Me.IsTank())
                    {
                        Logger.ATBLog("Yer a tank, Harry! Can't assist yerself!");
                        MainSettingsModel.Instance.AutoTargetSelection = AutoTargetSelection.None;
                        break;
                    }

                    if (PartyManager.IsInParty && VisiblePartyMembers.Any(IsTank) && (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck()))
                    {
                        {
                            var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat
                                                                                && ((Character)o).CurrentTargetId == PartyTank.ObjectId);
                            if (objs != null && objs.Any())
                            {
                                var newTarget = objs.OrderByDescending(o => o.CurrentHealth).First();
                                if (newTarget != Me.CurrentTarget)
                                {
                                    Logger.ATBLog("Highest Current HP Tanked Target Change!");
                                    newTarget.Target();
                                }
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.HighestCurrentHp:
                    if (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck())
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderByDescending(o => o.CurrentHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Highest Current HP Target Change!");
                                newTarget.Target();
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.HighestTotalHpTanked:
                    if (Me.IsTank())
                    {
                        Logger.ATBLog("Yer a tank, Harry! Can't assist yerself!");
                        MainSettingsModel.Instance.AutoTargetSelection = AutoTargetSelection.None;
                        break;
                    }

                    if (PartyManager.IsInParty && VisiblePartyMembers.Any(IsTank) && (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck()))
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o)
                                                                            && ((Character)o).InCombat
                                                                            && ((Character)o).CurrentTargetId == PartyTank.ObjectId);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderByDescending(o => o.MaxHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Highest Total HP Tanked Target Change!");
                                newTarget.Target();
                            }
                        }
                    }

                    break;

                case AutoTargetSelection.HighestTotalHp:
                    if (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || PulseCheck())
                    {
                        var objs = GameObjectManager.GameObjects.Where(o => IsValidEnemy(o) && ((Character)o).InCombat);
                        if (objs != null && objs.Any())
                        {
                            var newTarget = objs.OrderByDescending(o => o.MaxHealth).First();
                            if (newTarget != Me.CurrentTarget)
                            {
                                Logger.ATBLog("Highest Total HP Target Change!");
                                newTarget.Target();
                            }
                        }
                    }
                    break;

                case AutoTargetSelection.TankAssist:
                    if (Me.IsTank())
                    {
                        Logger.ATBLog("Yer a tank, Harry! Can't assist yerself!");
                        MainSettingsModel.Instance.AutoTargetSelection = AutoTargetSelection.None;
                        break;
                    }

                    if (PartyManager.IsInParty && VisiblePartyMembers.Any(IsTank) && (!Core.Player.HasTarget || !Core.Player.CurrentTarget.CanAttack || Core.Player.CurrentTarget != PartyTank.TargetCharacter))
                    {
                        Assist(VisiblePartyMembers.First(x => !x.IsMe && x.IsTank()));
                    }
                    break;

                default:
                    return false;
            }
            return false;
        }

        public static GameObject GetClosestEnemy()
        {
            return GameObjectManager.GameObjects.Where(u =>
                    IsValidEnemy(u)
                    && Core.Player.Location.Distance3D(u.Location) <= MainSettingsModel.Instance.MaxTargetDistance)
                .OrderBy(u => Core.Player.Location.Distance3D(u.Location)).FirstOrDefault();
        }

        public static bool IsTank(Character c)
        {
            switch (c.CurrentJob)
            {
                case ClassJobType.Marauder:
                case ClassJobType.Warrior:
                case ClassJobType.Paladin:
                case ClassJobType.Gladiator:
                case ClassJobType.DarkKnight:
                    return true;

                default:
                    return false;
            }
        }

        public static Character PartyTank
        {
            get
            {
                if (VisiblePartyMembers.Count <= 0) return null;
                try
                {
                    return VisiblePartyMembers.First(IsTank);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<Character> VisiblePartyMembers
        {
            get
            {
                var members = new List<Character>();
                if (!PartyManager.IsInParty)
                    members.Add(Core.Player);
                else
                    members.AddRange(from pm in PartyManager.AllMembers where pm.IsInObjectManager select (Character)GameObjectManager.GetObjectByObjectId(pm.ObjectId));
                return members;
            }
        }

        public static void Assist(Character c)
        {
            var target = GameObjectManager.GetObjectByObjectId(c.CurrentTargetId);
            if (target != null && target.IsTargetable && target.IsValid && target.CanAttack)
            {
                Logger.ATBLog(@"Assisting " + c.SafeName());
                target.Target();
            }
        }

        public static bool IsValidEnemy(GameObject obj)
        {
            if (!(obj is Character))
                return false;
            var c = (Character)obj;
            return !c.IsMe && !c.IsDead && c.IsValid && c.IsTargetable && c.IsVisible && c.CanAttack;
        }

        public static bool PulseCheck()
        {
            if (DateTime.Now < _pulseLimiter) return false;
            if (DateTime.Now > _pulseLimiter)
                _pulseLimiter = DateTime.Now.Add(TimeSpan.FromSeconds(1));
            return true;
        }
    }
}