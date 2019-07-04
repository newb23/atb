using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ATB.Utilities.Extensions
{
    internal static class GameObjectExtensions
    {
        private static GameObject Target => Me.CurrentTarget;
        private static LocalPlayer Me => Core.Player;

        #region SafeNames

        public static bool ShowPlayerNames = false;

        public static string SafeName(this GameObject obj)
        {
            if (obj.IsMe)
            {
                return "Me";
            }

            string name;
            var character = obj as BattleCharacter;
            if (character != null)
            {
                name = character.CanAttack ? "Enemy -> " : "Ally -> ";
                if (ShowPlayerNames) name += character.Name;
                //else name += character.CurrentJob.ToString();
            }
            else
            {
                name = obj.Name;
            }

            return name + obj.Name;
        }

        #endregion SafeNames

        internal static bool HasAura(this GameObject unit, uint spell, bool isMyAura = false, double msLeft = 0)
        {
            var unitasc = unit as Character;
            if (unit == null || unitasc == null || !unitasc.IsValid)
            {
                return false;
            }
            var auras = isMyAura
                ? unitasc.CharacterAuras.Where(r => r.CasterId == Me.ObjectId && r.Id == spell)
                : unitasc.CharacterAuras.Where(r => r.Id == spell);

            return auras.Any(aura => aura.TimespanLeft.TotalMilliseconds >= msLeft);
        }

        internal static bool HasAnyAura(this GameObject unit, List<uint> auras)
        {
            var unitasc = unit as Character;

            if (unit == null || unitasc == null || !unitasc.IsValid)
            {
                return false;
            }

            return unitasc.CharacterAuras.Any(r => auras.Contains(r.Id));
        }

        public static bool IsTank(this GameObject tar)
        {
            var gameObject = tar as Character;
            return gameObject != null && Tanks.Contains(gameObject.CurrentJob);
        }

        public static bool IsHealer(this GameObject tar)
        {
            var gameObject = tar as Character;
            return gameObject != null && Healers.Contains(gameObject.CurrentJob);
        }

        public static bool IsDps(this GameObject tar)
        {
            var gameObject = tar as Character;
            return gameObject != null && Dps.Contains(gameObject.CurrentJob);
        }

        public static bool HealthCheck(this GameObject tar, int healthInt, float healthPercent)
        {
            if (tar == null)
                return false;

            // If our target has more health than our setting and more health percent than our health percent setting, return true, else, return false
            return tar.CurrentHealth > healthInt && tar.CurrentHealthPercent > healthPercent;

            //// If our target has more hp percent than our hp percent setting but has less health than our health setting, return false
            //if (tar.CurrentHealthPercent > healthPercent && tar.CurrentHealth < healthInt)
            //    return false;

            //// if our target has more health than our setting but less health percent than our hp percent setting, return false
            //if (tar.CurrentHealth > healthInt && tar.CurrentHealthPercent < healthPercent)
            //    return false;

            //// if our target has less health than our setting and less health than our percent setting, return false
            //return tar.CurrentHealth >= healthInt || !(tar.CurrentHealthPercent < healthPercent);
        }

        #region Helpers

        private static readonly List<ClassJobType> Tanks = new List<ClassJobType>()
        {
            ClassJobType.Gladiator,
            ClassJobType.Marauder,
            ClassJobType.Paladin,
            ClassJobType.Warrior,
            ClassJobType.DarkKnight,
        };

        private static readonly List<ClassJobType> Healers = new List<ClassJobType>()
        {
            ClassJobType.Arcanist,
            ClassJobType.Conjurer,
            ClassJobType.Scholar,
            ClassJobType.WhiteMage,
            ClassJobType.Astrologian
        };

        private static readonly List<ClassJobType> Dps = new List<ClassJobType>()
        {
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.Thaumaturge,
            ClassJobType.BlackMage,
            ClassJobType.Lancer,
            ClassJobType.Dragoon,
            ClassJobType.Pugilist,
            ClassJobType.Monk,
            ClassJobType.Ninja,
            ClassJobType.Machinist,
            ClassJobType.Rogue
        };

        public static IEnumerable<BattleCharacter> PartyMembers
        {
            get
            {
                return
                    PartyManager.VisibleMembers
                    .Select(pm => pm.GameObject as BattleCharacter)
                    .Where(pm => pm.IsTargetable);
            }
        }

        public static IEnumerable<BattleCharacter> HealManager
        {
            get
            {
                return
                    GameObjectManager.GetObjectsOfType<BattleCharacter>(true, true)
                    .Where(hm => hm.IsAlive && (PartyMembers.Contains(hm) || hm == Core.Player))
                    .OrderBy(HpScore);
            }
        }

        private static float HpScore(BattleCharacter c)
        {
            var score = c.CurrentHealthPercent;

            if (c.IsTank())
            {
                score -= 5f;
            }
            if (c.IsHealer())
            {
                score -= 3f;
            }
            return score;
        }

        #endregion Helpers
    }
}