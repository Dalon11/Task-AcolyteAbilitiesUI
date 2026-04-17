using System;
using Feature.Tooltip.Presentation.Views;
using UnityEngine;

namespace Feature.CharacterSelection.Presentation.Views
{

    /// <summary>
    /// Адаптирует входные события для сценария Character Selection Tooltip Hover Input.
    /// </summary>
    public sealed class CharacterSelectionTooltipHoverInputAdapter
    {
        private readonly TooltipHoverDelayCoordinator _hoverDelayCoordinator;

        public CharacterSelectionTooltipHoverInputAdapter(float hoverDelaySeconds, float mouseMovementThreshold)
        {
            _hoverDelayCoordinator = new TooltipHoverDelayCoordinator(hoverDelaySeconds, mouseMovementThreshold);
        }

        public void StartPending(TooltipHoverDelayCoordinator.HoverTargetType targetType, string targetId)
        {
            _hoverDelayCoordinator.StartPending(
                targetType,
                targetId,
                UnityEngine.Input.mousePosition,
                Time.unscaledTime);
        }

        public void Cancel()
        {
            _hoverDelayCoordinator.Cancel();
        }

        public void Process(Action<TooltipHoverDelayCoordinator.HoverTargetType, string> onHoverReady)
        {
            _hoverDelayCoordinator.Process(
                UnityEngine.Input.mousePosition,
                Time.unscaledTime,
                onHoverReady);
        }
    }
}
