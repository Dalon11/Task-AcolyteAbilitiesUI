using System.Collections.Generic;
using Feature.Abilities.Presentation.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Loadout.Presentation.Views
{

    /// <summary>
    /// Определяет целевой элемент для сценария Ability Drop Target.
    /// </summary>
    public sealed class AbilityDropTargetResolver
    {
        private readonly List<RaycastResult> _raycastResults;

        public AbilityDropTargetResolver()
        {
            _raycastResults = new List<RaycastResult>();
        }

        public string ResolveAbilityIdUnderPointer(PointerEventData eventData)
        {
            if (eventData == null)
                return string.Empty;

            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return string.Empty;

            _raycastResults.Clear();
            eventSystem.RaycastAll(eventData, _raycastResults);

            for (int i = 0; i < _raycastResults.Count; i++)
            {
                RaycastResult raycastResult = _raycastResults[i];
                if (raycastResult.gameObject == null)
                    continue;

                AbilityItemView abilityItemView = raycastResult.gameObject.GetComponentInParent<AbilityItemView>();
                if (abilityItemView == null)
                    continue;

                string abilityId = abilityItemView.AbilityId;
                if (string.IsNullOrWhiteSpace(abilityId))
                    continue;

                return abilityId;
            }

            return string.Empty;
        }
    }
}
