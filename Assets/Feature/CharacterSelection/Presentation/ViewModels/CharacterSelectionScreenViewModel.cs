using System;
using System.Collections.Generic;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Abilities.Presentation.ViewModels;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.CharacterPaper.Presentation.ViewModels;
using Feature.CharacterSelection.Core.Domain.Contracts;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Presentation.Binding.Contracts;
using Feature.Loadout.Presentation.Binding.Contracts;
using Feature.Loadout.Presentation.ViewModels;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Modifications.Presentation.ViewModels;
using Feature.Party.Presentation.Binding.Contracts;
using Feature.Party.Presentation.ViewModels;
using Feature.Tooltip.Presentation.Binding.Contracts;
using Feature.Tooltip.Presentation.ViewModels;

namespace Feature.CharacterSelection.Presentation.ViewModels
{
    /// <summary>
    /// �������� ViewModel ������ ������ ���������.
    /// </summary>
    public sealed class CharacterSelectionScreenViewModel : ICharacterSelectionScreenViewModel, IDisposable
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly ICharacterSelectionService _characterSelectionService;
        private readonly CharacterSelectionScreenStateCoordinator _screenStateCoordinator;
        private readonly CharacterSelectionTooltipCoordinator _tooltipCoordinator;
        private readonly ModificationDragAndDropCoordinator _modificationDragAndDropCoordinator;
        private readonly CharacterLoadoutStateService _characterLoadoutStateService;
        private readonly AbilitiesListViewModel _abilitiesListViewModel;
        private readonly PartyViewModel _partyViewModel;

        private string _currentCharacterId;

        public CharacterSelectionScreenViewModel(
            ICharacterRepository characterRepository,
            ICharacterSelectionService characterSelectionService,
            CharacterSelectionScreenDependencies dependencies)
        {
            if (characterRepository == null)
                throw new ArgumentNullException(nameof(characterRepository));

            if (characterSelectionService == null)
                throw new ArgumentNullException(nameof(characterSelectionService));

            if (dependencies == null)
                throw new ArgumentNullException(nameof(dependencies));

            _characterRepository = characterRepository;
            _characterSelectionService = characterSelectionService;
            _characterLoadoutStateService = dependencies.CharacterLoadoutStateService;
            _currentCharacterId = string.Empty;

            _partyViewModel = dependencies.PartyViewModel;
            _abilitiesListViewModel = dependencies.AbilitiesListViewModel;
            _screenStateCoordinator = dependencies.ScreenStateCoordinator;
            _tooltipCoordinator = dependencies.TooltipCoordinator;
            _modificationDragAndDropCoordinator = dependencies.ModificationDragAndDropCoordinator;

            Party = dependencies.PartyViewModel;
            CharacterPaper = dependencies.CharacterPaperViewModel;
            Abilities = dependencies.AbilitiesListViewModel;
            Modifications = dependencies.ModificationsListViewModel;
            Tooltip = dependencies.TooltipViewModel;
            DragSlot = dependencies.DragSlotViewModel;

            _partyViewModel.onCharacterSelectionRequested += OnPartyCharacterSelectionRequested;
            _characterSelectionService.onCharacterChanged += OnCharacterChanged;
        }

        public IPartyViewModel Party { get; }

        public ICharacterPaperViewModel CharacterPaper { get; }

        public IAbilitiesListViewModel Abilities { get; }

        public IModificationsListViewModel Modifications { get; }

        public ITooltipViewModel Tooltip { get; }

        public IModificationDragSlotViewModel DragSlot { get; }

        public void Initialize()
        {
            IReadOnlyList<CharacterModel> characters = _characterRepository.GetAllCharacters();
            string resolvedCharacterId = ResolveInitialCharacterId(characters);

            _screenStateCoordinator.BuildParty(characters, resolvedCharacterId);

            if (string.IsNullOrWhiteSpace(resolvedCharacterId))
            {
                ClearScreen();
                return;
            }

            _characterSelectionService.SelectCharacter(resolvedCharacterId);

            CharacterModel currentCharacter;
            if (_characterSelectionService.TryGetCurrentCharacter(out currentCharacter))
            {
                if (!string.Equals(CharacterPaper.CharacterId, currentCharacter.Id, StringComparison.Ordinal))
                    ApplyCharacter(currentCharacter);
            }
            else
                ClearScreen();
        }

        public bool OnPartyCharacterClick(string characterId)
        {
            return Party.RequestCharacterSelection(characterId);
        }

        public void OnCharacterHoverEnter()
        {
            _tooltipCoordinator.OnCharacterHoverEnter();
        }

        public void OnAbilityHoverEnter(string abilityId)
        {
            _tooltipCoordinator.OnAbilityHoverEnter(abilityId);
        }

        public void OnModificationHoverEnter(string modificationId)
        {
            _tooltipCoordinator.OnModificationHoverEnter(modificationId);
        }

        public void OnCharacterHoverExit()
        {
            OnHoverExit();
        }

        public void OnAbilityHoverExit(string abilityId)
        {
            _ = abilityId;
            OnHoverExit();
        }

        public bool OnAbilityPointerDown(string abilityId)
        {
            return _modificationDragAndDropCoordinator.TryStartDragFromAbility(abilityId);
        }

        public void OnAbilityPointerUp(string abilityId)
        {
            _modificationDragAndDropCoordinator.EndDrag(abilityId);
        }

        public void OnModificationHoverExit(string modificationId)
        {
            _ = modificationId;
            OnHoverExit();
        }

        public bool OnModificationPointerDown(string modificationId)
        {
            return _modificationDragAndDropCoordinator.TryStartDragFromModification(modificationId);
        }

        public void OnModificationPointerUp(string abilityId)
        {
            _modificationDragAndDropCoordinator.EndDrag(abilityId);
        }

        public void OnHoverExit()
        {
            _tooltipCoordinator.OnHoverExit();
        }

        public void Dispose()
        {
            _partyViewModel.onCharacterSelectionRequested -= OnPartyCharacterSelectionRequested;
            _characterSelectionService.onCharacterChanged -= OnCharacterChanged;
        }

        private void OnPartyCharacterSelectionRequested(string characterId)
        {
            _characterSelectionService.SelectCharacter(characterId);
        }

        private void OnCharacterChanged(CharacterModel character)
        {
            _modificationDragAndDropCoordinator.CancelDrag();
            SaveCurrentCharacterLoadout();
            ApplyCharacter(character);
        }

        private void ApplyCharacter(CharacterModel character)
        {
            if (character == null)
            {
                ClearScreen();
                return;
            }

            IReadOnlyList<AbilityModificationPlacementState> savedPlacements;
            if (!_characterLoadoutStateService.TryGet(character.Id, out savedPlacements))
                savedPlacements = null;

            _screenStateCoordinator.ApplyCharacter(character, savedPlacements);
            _tooltipCoordinator.HideTooltip();
            _currentCharacterId = character.Id;
        }

        private void ClearScreen()
        {
            _modificationDragAndDropCoordinator.CancelDrag();
            _screenStateCoordinator.ClearScreen();
            _tooltipCoordinator.HideTooltip();
            _currentCharacterId = string.Empty;
        }

        private void SaveCurrentCharacterLoadout()
        {
            if (string.IsNullOrWhiteSpace(_currentCharacterId))
                return;

            IReadOnlyList<AbilityModificationPlacementState> placements = _abilitiesListViewModel.GetCurrentPlacements();
            _characterLoadoutStateService.Save(_currentCharacterId, placements);
        }

        private string ResolveInitialCharacterId(IReadOnlyList<CharacterModel> characters)
        {
            if (characters == null || characters.Count == 0)
                return string.Empty;

            CharacterModel firstCharacter = characters[0];
            if (firstCharacter == null)
                return string.Empty;

            return firstCharacter.Id;
        }
    }
}



