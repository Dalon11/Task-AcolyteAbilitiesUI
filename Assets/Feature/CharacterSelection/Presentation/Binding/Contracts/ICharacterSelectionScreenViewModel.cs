using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.Loadout.Presentation.Binding.Contracts;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Party.Presentation.Binding.Contracts;
using Feature.Tooltip.Presentation.Binding.Contracts;

namespace Feature.CharacterSelection.Presentation.Binding.Contracts
{

    public interface ICharacterSelectionScreenViewModel
    {
        public IPartyViewModel Party { get; }

        public ICharacterPaperViewModel CharacterPaper { get; }

        public IAbilitiesListViewModel Abilities { get; }

        public IModificationsListViewModel Modifications { get; }

        public ITooltipViewModel Tooltip { get; }

        public IModificationDragSlotViewModel DragSlot { get; }

        public void Initialize();

        public bool OnPartyCharacterClick(string characterId);

        public void OnCharacterHoverEnter();

        public void OnCharacterHoverExit();

        public void OnAbilityHoverEnter(string abilityId);

        public void OnAbilityHoverExit(string abilityId);

        public bool OnAbilityPointerDown(string abilityId);

        public void OnAbilityPointerUp(string abilityId);

        public void OnModificationHoverEnter(string modificationId);

        public void OnModificationHoverExit(string modificationId);

        public bool OnModificationPointerDown(string modificationId);

        public void OnModificationPointerUp(string abilityId);
    }
}
