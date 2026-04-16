using System;

namespace Feature.Abilities.Configs.Enums
{
    [Flags]
    public enum ModificationTypeFlags
    {
        None = 0,
        Psyker = 1 << 0,
        Dot = 1 << 1,
        Attack = 1 << 2,
        Buff = 1 << 3,
        Debuff = 1 << 4,
        All = Psyker | Dot | Attack | Buff | Debuff
    }
}
