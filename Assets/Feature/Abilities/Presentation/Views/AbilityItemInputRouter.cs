using System;
using Feature.Common.Presentation.Input;
using UnityEngine.EventSystems;

namespace Feature.Abilities.Presentation.Views
{

    /// <summary>
    /// Маршрутизирует hover и pointer-события элемента способности в привязанные обработчики.
    /// </summary>
    public sealed class AbilityItemInputRouter : IDisposable
    {
        private readonly UiPointerInputView _hoverPointerInputView;
        private readonly UiPointerInputView _slotPointerInputView;

        private string _abilityId;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;
        private Action<string, PointerEventData> _onPointerDown;
        private Action<string, PointerEventData> _onPointerUp;

        public AbilityItemInputRouter(UiPointerInputView hoverPointerInputView, UiPointerInputView slotPointerInputView)
        {
            _hoverPointerInputView = hoverPointerInputView;
            _slotPointerInputView = slotPointerInputView;
            _abilityId = string.Empty;

            Subscribe();
        }

        public void SetAbilityId(string abilityId)
        {
            _abilityId = abilityId ?? string.Empty;
        }

        public void SetHandlers(
            Action<string> onHoverEnter,
            Action<string> onHoverExit,
            Action<string, PointerEventData> onPointerDown,
            Action<string, PointerEventData> onPointerUp)
        {
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
            _onPointerDown = onPointerDown;
            _onPointerUp = onPointerUp;
        }

        public void ClearBinding()
        {
            _abilityId = string.Empty;
            _onHoverEnter = null;
            _onHoverExit = null;
            _onPointerDown = null;
            _onPointerUp = null;
        }

        public void HandleHoverEnterForCurrentAbility()
        {
            DispatchHoverEnter();
        }

        public void HandleHoverExitForCurrentAbility()
        {
            DispatchHoverExit();
        }

        public void HandlePointerDownForCurrentAbility(PointerEventData eventData)
        {
            DispatchPointerDown(eventData);
        }

        public void HandlePointerUpForCurrentAbility(PointerEventData eventData)
        {
            DispatchPointerUp(eventData);
        }

        public void Dispose()
        {
            Unsubscribe();
            ClearBinding();
        }

        private void Subscribe()
        {
            if (_hoverPointerInputView != null)
            {
                _hoverPointerInputView.onPointerEnter += OnPointerEnter;
                _hoverPointerInputView.onPointerExit += OnPointerExit;
            }

            if (_slotPointerInputView != null)
            {
                _slotPointerInputView.onPointerDown += OnPointerDown;
                _slotPointerInputView.onPointerUp += OnPointerUp;
            }
        }

        private void Unsubscribe()
        {
            if (_hoverPointerInputView != null)
            {
                _hoverPointerInputView.onPointerEnter -= OnPointerEnter;
                _hoverPointerInputView.onPointerExit -= OnPointerExit;
            }

            if (_slotPointerInputView != null)
            {
                _slotPointerInputView.onPointerDown -= OnPointerDown;
                _slotPointerInputView.onPointerUp -= OnPointerUp;
            }
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            _ = eventData;
            DispatchHoverEnter();
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            _ = eventData;
            DispatchHoverExit();
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            DispatchPointerDown(eventData);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            DispatchPointerUp(eventData);
        }

        private void DispatchHoverEnter()
        {
            if (string.IsNullOrWhiteSpace(_abilityId))
                return;

            Action<string> handler = _onHoverEnter;
            if (handler != null)
                handler.Invoke(_abilityId);
        }

        private void DispatchHoverExit()
        {
            if (string.IsNullOrWhiteSpace(_abilityId))
                return;

            Action<string> handler = _onHoverExit;
            if (handler != null)
                handler.Invoke(_abilityId);
        }

        private void DispatchPointerDown(PointerEventData eventData)
        {
            if (string.IsNullOrWhiteSpace(_abilityId))
                return;

            Action<string, PointerEventData> handler = _onPointerDown;
            if (handler != null)
                handler.Invoke(_abilityId, eventData);
        }

        private void DispatchPointerUp(PointerEventData eventData)
        {
            if (string.IsNullOrWhiteSpace(_abilityId))
                return;

            Action<string, PointerEventData> handler = _onPointerUp;
            if (handler != null)
                handler.Invoke(_abilityId, eventData);
        }
    }
}
