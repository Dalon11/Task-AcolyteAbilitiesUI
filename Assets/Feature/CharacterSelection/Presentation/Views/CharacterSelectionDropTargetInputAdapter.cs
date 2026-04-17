using System;
using Feature.Loadout.Presentation.Views;
using UnityEngine.EventSystems;

namespace Feature.CharacterSelection.Presentation.Views
{

    /// <summary>
    /// Адаптирует входные события для сценария Character Selection Drop Target Input.
    /// </summary>
    public sealed class CharacterSelectionDropTargetInputAdapter
    {
        private readonly AbilityDropTargetResolver _abilityDropTargetResolver;

        public CharacterSelectionDropTargetInputAdapter()
        {
            _abilityDropTargetResolver = new AbilityDropTargetResolver();
        }

        public string ResolveAbilityIdUnderPointer(PointerEventData eventData)
        {
            if (eventData == null)
                return string.Empty;

            return _abilityDropTargetResolver.ResolveAbilityIdUnderPointer(eventData);
        }
    }
}
