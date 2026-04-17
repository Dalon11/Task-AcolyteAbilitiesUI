using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Abilities.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Ability Item.
    /// </summary>
    public sealed class AbilityItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _frameColorTarget;
        [SerializeField] private Image[] _dropFieldColorTargets = new Image[0];
        [SerializeField] private Color _dropFieldEnabledColor = Color.white;
        [SerializeField] private Image _slotIconImage;
        [SerializeField] private Image _slotColorTarget;
        [SerializeField] private UiPointerInputView _hoverPointerInputView;
        [SerializeField] private UiPointerInputView _slotPointerInputView;

        private IAbilityItemViewModel _viewModel;
        private AbilityItemInputRouter _inputRouter;
        private AbilityItemVisualStateApplier _visualStateApplier;

        public string AbilityId
        {
            get
            {
                if (_viewModel == null)
                    return string.Empty;

                return _viewModel.Id;
            }
        }

        private void Awake()
        {
            if (_hoverPointerInputView == null)
                _hoverPointerInputView = GetComponent<UiPointerInputView>();

            if (_slotPointerInputView == null)
                _slotPointerInputView = _hoverPointerInputView;

            _inputRouter = new AbilityItemInputRouter(_hoverPointerInputView, _slotPointerInputView);
            _visualStateApplier = new AbilityItemVisualStateApplier(
                _frameColorTarget,
                _dropFieldColorTargets,
                _dropFieldEnabledColor,
                _slotIconImage,
                _slotColorTarget);
        }

        private void OnDestroy()
        {
            if (_inputRouter != null)
            {
                _inputRouter.Dispose();
                _inputRouter = null;
            }
        }

        public void Bind(IAbilityItemViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _viewModel = viewModel;
            _inputRouter.SetAbilityId(viewModel.Id);
            Refresh();
        }

        public void SetInputHandlers(
            Action<string> onHoverEnter,
            Action<string> onHoverExit,
            Action<string, PointerEventData> onPointerDown,
            Action<string, PointerEventData> onPointerUp)
        {
            _inputRouter.SetHandlers(onHoverEnter, onHoverExit, onPointerDown, onPointerUp);
        }

        public void HandleHoverEnter()
        {
            if (_viewModel == null)
                return;

            _inputRouter.HandleHoverEnterForCurrentAbility();
        }

        public void HandleHoverExit()
        {
            if (_viewModel == null)
                return;

            _inputRouter.HandleHoverExitForCurrentAbility();
        }

        public void HandlePointerDown(PointerEventData eventData)
        {
            if (_viewModel == null)
                return;

            _inputRouter.HandlePointerDownForCurrentAbility(eventData);
        }

        public void HandlePointerUp(PointerEventData eventData)
        {
            if (_viewModel == null)
                return;

            _inputRouter.HandlePointerUpForCurrentAbility(eventData);
        }

        public void Unbind()
        {
            _viewModel = null;
            _inputRouter.ClearBinding();
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_iconImage != null)
                _iconImage.sprite = _viewModel.Icon;

            if (_nameText != null)
                _nameText.text = _viewModel.Name;

            _visualStateApplier.Apply(_viewModel);
        }
    }
}
