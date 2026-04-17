namespace Feature.CharacterSelection.Presentation.Contracts
{
    public interface ICharacterSelectionTooltipCoordinator
    {
        public void OnCharacterHoverEnter();

        public void OnAbilityHoverEnter(string abilityId);

        public void OnModificationHoverEnter(string modificationId);

        public void OnHoverExit();

        public void HideTooltip();
    }
}
