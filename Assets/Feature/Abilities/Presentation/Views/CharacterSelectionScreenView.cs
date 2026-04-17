using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Тонкий bind-слой экрана выбора персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenView : MonoBehaviour
    {
        [SerializeField] private PartyListView _partyListView;
        [SerializeField] private CharacterPaperView _characterPaperView;
        [SerializeField] private AbilitiesListView _abilitiesListView;
        [SerializeField] private ModificationsListView _modificationsListView;
        [SerializeField] private TooltipView _tooltipView;

        private ICharacterSelectionScreenViewModel _viewModel;

        public void Bind(ICharacterSelectionScreenViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();

            _viewModel = viewModel;

            if (_partyListView != null)
                _partyListView.Bind(_viewModel.Party, HandlePartyCharacterClick);

            if (_characterPaperView != null)
            {
                _characterPaperView.Bind(_viewModel.CharacterPaper);
                _characterPaperView.SetInputHandlers(HandleCharacterHoverEnter, HandleCharacterHoverExit);
            }

            if (_abilitiesListView != null)
                _abilitiesListView.Bind(_viewModel.Abilities, HandleAbilityHoverEnter, HandleAbilityHoverExit);

            if (_modificationsListView != null)
                _modificationsListView.Bind(_viewModel.Modifications, HandleModificationHoverEnter, HandleModificationHoverExit);

            if (_tooltipView != null)
                _tooltipView.Bind(_viewModel.Tooltip);

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel == null)
                return;

            if (_partyListView != null)
                _partyListView.Unbind();

            if (_characterPaperView != null)
                _characterPaperView.Unbind();

            if (_abilitiesListView != null)
                _abilitiesListView.Unbind();

            if (_modificationsListView != null)
                _modificationsListView.Unbind();

            if (_tooltipView != null)
                _tooltipView.Unbind();

            _viewModel = null;
        }

        public void HandlePartyCharacterClick(string characterId)
        {
            if (_viewModel == null)
                return;

            _viewModel.OnPartyCharacterClick(characterId);
        }

        public void HandleCharacterHoverEnter()
        {
            if (_viewModel == null)
                return;

            _viewModel.OnCharacterHoverEnter();
        }

        public void HandleCharacterHoverExit()
        {
            if (_viewModel == null)
                return;

            _viewModel.OnCharacterHoverExit();
        }

        public void HandleAbilityHoverEnter(string abilityId)
        {
            if (_viewModel == null)
                return;

            _viewModel.OnAbilityHoverEnter(abilityId);
        }

        public void HandleAbilityHoverExit(string abilityId)
        {
            if (_viewModel == null)
                return;

            _viewModel.OnAbilityHoverExit(abilityId);
        }

        public void HandleModificationHoverEnter(string modificationId)
        {
            if (_viewModel == null)
                return;

            _viewModel.OnModificationHoverEnter(modificationId);
        }

        public void HandleModificationHoverExit(string modificationId)
        {
            if (_viewModel == null)
                return;

            _viewModel.OnModificationHoverExit(modificationId);
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_partyListView != null)
                _partyListView.Refresh();

            if (_characterPaperView != null)
                _characterPaperView.Refresh();

            if (_abilitiesListView != null)
                _abilitiesListView.Refresh();

            if (_modificationsListView != null)
                _modificationsListView.Refresh();

            if (_tooltipView != null)
                _tooltipView.Refresh();
        }
    }
}

