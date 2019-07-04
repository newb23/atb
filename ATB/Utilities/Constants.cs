using System;
using ff14bot;
using ff14bot.Objects;

namespace ATB.Utilities
{
    internal class Constants
    {
        internal static LocalPlayer Me => Core.Player;
        internal static GameObject Target => Core.Player.CurrentTarget;

        internal static bool TargetConverted(GameObject conversionTarget)
        {
            var myObject = conversionTarget as BattleCharacter;
            try
            {
                myObject = conversionTarget as BattleCharacter;
            }
            catch (Exception)
            {
                // ignored
            }

            return myObject != null;
        }

        internal static BattleCharacter ConvertedTarget()
        {
            return (BattleCharacter)Target;
        }
    }
}