namespace Feature.CharacterSelection.Presentation.Contracts
{
    public interface ICharacterSelectionTooltipCoordinator
    {
        void OnCharacterHoverEnter();

        void OnAbilityHoverEnter(string abilityId);

        void OnModificationHoverEnter(string modificationId);

        void OnHoverExit();

        void HideTooltip();
    }
}
