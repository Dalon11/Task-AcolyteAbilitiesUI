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
