using ff14bot.Enums;

namespace ATB.Utilities
{
    public static class PartyDescriptors
    {
        public static bool IsTank(ClassJobType c)
        {
            switch (c)
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

        public static bool IsDps(ClassJobType c)
        {
            switch (c)
            {
                case ClassJobType.Adventurer:
                case ClassJobType.Archer:
                case ClassJobType.Arcanist:
                case ClassJobType.Bard:
                case ClassJobType.BlackMage:
                case ClassJobType.Dragoon:
                case ClassJobType.Lancer:
                case ClassJobType.Machinist:
                case ClassJobType.Monk:
                case ClassJobType.Ninja:
                case ClassJobType.Pugilist:
                case ClassJobType.Rogue:
                case ClassJobType.Summoner:
                case ClassJobType.Thaumaturge:
                case ClassJobType.Samurai:
                case ClassJobType.RedMage:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsHealer(ClassJobType c)
        {
            return !IsDps(c) && !IsTank(c);
        }
    }
}