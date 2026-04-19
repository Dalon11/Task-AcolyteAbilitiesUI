namespace Feature.Loadout.Presentation.Contracts.Models
{
    /// <summary>
    /// Хранит состояние Ability Modification Placement.
    /// </summary>
    public sealed class AbilityModificationPlacementState
    {
        public AbilityModificationPlacementState(string abilityId, string modificationId)
        {
            AbilityId = abilityId;
            ModificationId = modificationId;
        }

        public string AbilityId { get; }

        public string ModificationId { get; }
    }
}
