namespace Feature.Abilities.Presentation.Binding.Contracts
{
    /// <summary>
    /// Контракт корневой ViewModel экрана выбора персонажа для thin binding layer.
    /// </summary>
    public interface ICharacterSelectionScreenViewModel
    {
        IPartyViewModel Party { get; }

        ICharacterPaperViewModel CharacterPaper { get; }

        IAbilitiesListViewModel Abilities { get; }

        IModificationsListViewModel Modifications { get; }

        ITooltipViewModel Tooltip { get; }

        IModificationDragSlotViewModel DragSlot { get; }

        void Initialize();

        bool OnPartyCharacterClick(string characterId);

        void OnCharacterHoverEnter();

        void OnCharacterHoverExit();

        void OnAbilityHoverEnter(string abilityId);

        void OnAbilityHoverExit(string abilityId);

        bool OnAbilityPointerDown(string abilityId);

        void OnAbilityPointerUp(string abilityId);

        void OnModificationHoverEnter(string modificationId);

        void OnModificationHoverExit(string modificationId);

        bool OnModificationPointerDown(string modificationId);

        void OnModificationPointerUp(string abilityId);
    }
}
