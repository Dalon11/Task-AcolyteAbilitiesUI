using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Presentation.Contracts;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Tooltip.Presentation.Binding.Contracts;

namespace Feature.CharacterSelection.Presentation.ViewModels
{

    /// <summary>
    /// Координирует взаимодействие компонентов в сценарии Character Selection Tooltip.
    /// </summary>
    public sealed class CharacterSelectionTooltipCoordinator : ICharacterSelectionTooltipCoordinator
    {
        private readonly ICharacterPaperViewModel _characterPaper;
        private readonly IAbilitiesListViewModel _abilities;
        private readonly IModificationsListViewModel _modifications;
        private readonly ITooltipViewModel _tooltip;

        public CharacterSelectionTooltipCoordinator(
            ICharacterPaperViewModel characterPaper,
            IAbilitiesListViewModel abilities,
            IModificationsListViewModel modifications,
            ITooltipViewModel tooltip)
        {
            if (characterPaper == null)
                throw new ArgumentNullException(nameof(characterPaper));

            if (abilities == null)
                throw new ArgumentNullException(nameof(abilities));

            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            if (tooltip == null)
                throw new ArgumentNullException(nameof(tooltip));

            _characterPaper = characterPaper;
            _abilities = abilities;
            _modifications = modifications;
            _tooltip = tooltip;
        }

        public void OnCharacterHoverEnter()
        {
            string header;
            string description;
            if (!_characterPaper.TryGetTooltipContent(out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnAbilityHoverEnter(string abilityId)
        {
            string header;
            string description;
            if (!_abilities.TryGetTooltipContent(abilityId, out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnModificationHoverEnter(string modificationId)
        {
            string header;
            string description;
            if (!_modifications.TryGetTooltipContent(modificationId, out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnHoverExit()
        {
            HideTooltip();
        }

        public void HideTooltip()
        {
            _tooltip.Hide();
        }

        private void ShowTooltip(string header, string description)
        {
            if (string.IsNullOrWhiteSpace(header) || description == null)
            {
                HideTooltip();
                return;
            }

            TooltipModel tooltip = new TooltipModel(header, description);
            _tooltip.Show(tooltip);
        }
    }
}
