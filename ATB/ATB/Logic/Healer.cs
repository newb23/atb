using ATB.Models;
using ATB.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;
using TreeSharp;
using static ATB.Utilities.Constants;

namespace ATB.Logic
{
    public static class Healer
    {
        private static readonly Composite HealerComposite;
        private static bool TargetConverted => TargetConverted(Target);

        static Healer()
        {
            HealerComposite = new Decorator(r => PartyDescriptors.IsHealer(Me.CurrentJob), new ActionRunCoroutine(ctx => HealerTask()));
        }

        public static Composite Execute()
        {
            return HealerComposite;
        }

        private static async Task<bool> HealerTask()
        {
            if (Me.IsDead || Me.IsMounted)
            {
                return false;
            }

            if (Me.InCombat || Target != null && TargetConverted && ConvertedTarget().TaggerType != 0)
                return await BrainBehavior.CombatLogic.ExecuteCoroutine();

            if (Me.InCombat) return false;

            if (RoutineManager.Current.RestBehavior != null)
                await RoutineManager.Current.RestBehavior.ExecuteCoroutine();

            if (RoutineManager.Current.PreCombatBuffBehavior != null)
                await RoutineManager.Current.PreCombatBuffBehavior.ExecuteCoroutine();

            if (RoutineManager.Current.HealBehavior != null)
                await RoutineManager.Current.HealBehavior.ExecuteCoroutine();

            if (PartyManager.IsInParty && MainSettingsModel.Instance.UseSmartPull)
            {
                if (Me.CurrentTarget != null)
                {
                    if (Target != null && TargetConverted && ConvertedTarget().Tapped && ConvertedTarget().TaggerType == 2)
                    {
                        if (RoutineManager.Current.PullBuffBehavior != null && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget))
                            await RoutineManager.Current.PullBuffBehavior.ExecuteCoroutine();

                        if (RoutineManager.Current.PullBehavior != null && MainSettingsModel.Instance.UsePull && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget) && Core.Player.CurrentTarget.Location.Distance3D(Core.Player.Location) <= RoutineManager.Current.PullRange + Core.Player.CurrentTarget.CombatReach)
                            return await RoutineManager.Current.PullBehavior.ExecuteCoroutine();
                    }
                }

                var tankCheck = PartyManager.VisibleMembers;
                if (tankCheck.Any(x => PartyDescriptors.IsTank(x.Class)))
                {
                    return false;
                }
            }

            if (RoutineManager.Current.PullBuffBehavior != null && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget))
                await RoutineManager.Current.PullBuffBehavior.ExecuteCoroutine();

            if (Me.CurrentTarget == null) return false;

            if (RoutineManager.Current.PullBehavior != null && MainSettingsModel.Instance.UsePull && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget) && Core.Player.CurrentTarget.Location.Distance3D(Core.Player.Location) <= RoutineManager.Current.PullRange + Core.Player.CurrentTarget.CombatReach)
                return await RoutineManager.Current.PullBehavior.ExecuteCoroutine();

            return false;
        }
    }
}