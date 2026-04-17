namespace Feature.Loadout.Presentation.ViewModels
{
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



