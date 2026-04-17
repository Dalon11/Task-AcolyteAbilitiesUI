using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// Корневая ViewModel экрана выбора персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenViewModel : ICharacterSelectionScreenViewModel, IDisposable
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly ICharacterSelectionService _characterSelectionService;
        private readonly CharacterSelectionScreenStateCoordinator _screenStateCoordinator;
        private readonly CharacterSelectionTooltipCoordinator _tooltipCoordinator;
        private readonly PartyViewModel _partyViewModel;

        public CharacterSelectionScreenViewModel(
            ICharacterRepository characterRepository,
            ICharacterSelectionService characterSelectionService)
        {
            if (characterRepository == null)
                throw new ArgumentNullException(nameof(characterRepository));

            if (characterSelectionService == null)
                throw new ArgumentNullException(nameof(characterSelectionService));

            _characterRepository = characterRepository;
            _characterSelectionService = characterSelectionService;

            _partyViewModel = new PartyViewModel();
            CharacterPaperViewModel characterPaperViewModel = new CharacterPaperViewModel();
            AbilitiesListViewModel abilitiesListViewModel = new AbilitiesListViewModel();
            ModificationsListViewModel modificationsListViewModel = new ModificationsListViewModel();
            TooltipViewModel tooltipViewModel = new TooltipViewModel();

            Party = _partyViewModel;
            CharacterPaper = characterPaperViewModel;
            Abilities = abilitiesListViewModel;
            Modifications = modificationsListViewModel;
            Tooltip = tooltipViewModel;

            _screenStateCoordinator = new CharacterSelectionScreenStateCoordinator(
                _partyViewModel,
                characterPaperViewModel,
                abilitiesListViewModel,
                modificationsListViewModel);
            _tooltipCoordinator = new CharacterSelectionTooltipCoordinator(
                characterPaperViewModel,
                abilitiesListViewModel,
                modificationsListViewModel,
                tooltipViewModel);

            _partyViewModel.onCharacterSelectionRequested += OnPartyCharacterSelectionRequested;
            _characterSelectionService.onCharacterChanged += OnCharacterChanged;
        }

        public IPartyViewModel Party { get; }

        public ICharacterPaperViewModel CharacterPaper { get; }

        public IAbilitiesListViewModel Abilities { get; }

        public IModificationsListViewModel Modifications { get; }

        public ITooltipViewModel Tooltip { get; }

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

        public void OnModificationHoverExit(string modificationId)
        {
            _ = modificationId;
            OnHoverExit();
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
            ApplyCharacter(character);
        }

        private void ApplyCharacter(CharacterModel character)
        {
            if (character == null)
            {
                ClearScreen();
                return;
            }

            _screenStateCoordinator.ApplyCharacter(character);
            _tooltipCoordinator.HideTooltip();
        }

        private void ClearScreen()
        {
            _screenStateCoordinator.ClearScreen();
            _tooltipCoordinator.HideTooltip();
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
