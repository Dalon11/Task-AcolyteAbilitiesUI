using System;
using UnityEngine;

namespace Feature.Tooltip.Presentation.Views
{

    /// <summary>
    /// Координирует взаимодействие компонентов в сценарии Tooltip Hover Delay.
    /// </summary>
    public sealed class TooltipHoverDelayCoordinator
    {
        public enum HoverTargetType
        {
            None = 0,
            Character = 1,
            Ability = 2,
            Modification = 3
        }

        private readonly float _hoverDelaySeconds;
        private readonly float _mouseMovementThreshold;

        private HoverTargetType _pendingHoverTargetType;
        private string _pendingHoverTargetId;
        private bool _isHoverPending;
        private bool _isHoverTooltipShown;
        private float _hoverStationaryStartTime;
        private Vector2 _lastMousePosition;

        public TooltipHoverDelayCoordinator(float hoverDelaySeconds, float mouseMovementThreshold)
        {
            _hoverDelaySeconds = Mathf.Max(0f, hoverDelaySeconds);
            _mouseMovementThreshold = Mathf.Max(0f, mouseMovementThreshold);
            Cancel();
        }

        public bool IsPending => _isHoverPending && !_isHoverTooltipShown;

        public void StartPending(
            HoverTargetType targetType,
            string targetId,
            Vector2 mousePosition,
            float unscaledTime)
        {
            _pendingHoverTargetType = targetType;
            _pendingHoverTargetId = targetId ?? string.Empty;
            _isHoverPending = true;
            _isHoverTooltipShown = false;
            _hoverStationaryStartTime = unscaledTime;
            _lastMousePosition = mousePosition;
        }

        public void Cancel()
        {
            _pendingHoverTargetType = HoverTargetType.None;
            _pendingHoverTargetId = string.Empty;
            _isHoverPending = false;
            _isHoverTooltipShown = false;
            _hoverStationaryStartTime = 0f;
            _lastMousePosition = Vector2.zero;
        }

        public void Process(Vector2 mousePosition, float unscaledTime, Action<HoverTargetType, string> onHoverReady)
        {
            if (!_isHoverPending || _isHoverTooltipShown)
                return;

            Vector2 mouseDelta = mousePosition - _lastMousePosition;
            float movementThresholdSqr = _mouseMovementThreshold * _mouseMovementThreshold;
            if (mouseDelta.sqrMagnitude > movementThresholdSqr)
            {
                _lastMousePosition = mousePosition;
                _hoverStationaryStartTime = unscaledTime;
                return;
            }

            if (unscaledTime - _hoverStationaryStartTime < _hoverDelaySeconds)
                return;

            _isHoverTooltipShown = true;

            Action<HoverTargetType, string> handler = onHoverReady;
            if (handler != null)
                handler.Invoke(_pendingHoverTargetType, _pendingHoverTargetId);
        }
    }
}
