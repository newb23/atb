using ATB.Models;
using ATB.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using System.Threading.Tasks;
using ff14bot.Objects;
using TreeSharp;
using static ATB.Utilities.Constants;

namespace ATB.Logic
{
    public static class Tank
    {
        private static readonly Composite TankComposite;
        private static bool TargetConverted => TargetConverted(Target);

        static Tank()
        {
            TankComposite = new Decorator(r => PartyDescriptors.IsTank(Me.CurrentJob), new ActionRunCoroutine(ctx => TankTask()));
        }

        public static Composite Execute()
        {
            return TankComposite;
        }

        private static async Task<bool> TankTask()
        {
            if (Me.IsDead || Me.IsMounted)
            {
                return false;
            }

            if (Me.InCombat || Target != null && TargetConverted && ConvertedTarget().TaggerType != 0)
                return await BrainBehavior.CombatLogic.ExecuteCoroutine();

            if (!Me.InCombat)
            {
                if (RoutineManager.Current.RestBehavior != null)
                    await RoutineManager.Current.RestBehavior.ExecuteCoroutine();

                if (RoutineManager.Current.PreCombatBuffBehavior != null)
                    await RoutineManager.Current.PreCombatBuffBehavior.ExecuteCoroutine();

                if (RoutineManager.Current.HealBehavior != null)
                    await RoutineManager.Current.HealBehavior.ExecuteCoroutine();

                if (RoutineManager.Current.PullBuffBehavior != null && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget))
                    await RoutineManager.Current.PullBuffBehavior.ExecuteCoroutine();

                if (Me.CurrentTarget != null)
                {
                    if (RoutineManager.Current.PullBehavior != null && MainSettingsModel.Instance.UsePull && TargetingManager.IsValidEnemy(Core.Player.CurrentTarget) && Core.Player.CurrentTarget.Location.Distance3D(Core.Player.Location) <= RoutineManager.Current.PullRange + Core.Player.CurrentTarget.CombatReach)
                        return await RoutineManager.Current.PullBehavior.ExecuteCoroutine();
                }
            }
            return false;
        }
    }
}