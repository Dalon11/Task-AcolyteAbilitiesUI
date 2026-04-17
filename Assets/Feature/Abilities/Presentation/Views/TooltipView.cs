using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using TMPro;
using UnityEngine;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Тонкий bind-слой единого tooltip.
    /// </summary>
    public sealed class TooltipView : MonoBehaviour
    {
        [SerializeField] private RectTransform _panelRect;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private TMP_Text _descriptionText;

        private static readonly Vector2 CursorOffset = new Vector2(18f, -18f);
        private const float ScreenPadding = 10f;

        private ITooltipViewModel _viewModel;
        private TooltipLayoutController _layoutController;
        private TooltipPositionController _positionController;
        private TooltipRaycastConfigurator _raycastConfigurator;

        private void Awake()
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();

            _layoutController = new TooltipLayoutController(_panelRect, _headerText, _descriptionText);
            _positionController = new TooltipPositionController(_panelRect, rootCanvas, CursorOffset, ScreenPadding);
            _raycastConfigurator = new TooltipRaycastConfigurator(_panelRect);

            _layoutController.EnsureLayoutComponents();
            _raycastConfigurator.EnsureIgnoresRaycasts();
        }

        private void LateUpdate()
        {
            if (_viewModel == null)
                return;

            if (!_viewModel.IsVisible)
                return;

            UpdateTooltipPosition();
        }

        public void Bind(ITooltipViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();

            _viewModel = viewModel;
            _viewModel.onStateChanged += OnViewModelStateChanged;

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel == null)
                return;

            _viewModel.onStateChanged -= OnViewModelStateChanged;
            _viewModel = null;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_panelRect != null)
                _panelRect.gameObject.SetActive(_viewModel.IsVisible);

            ITooltipContentViewModel content = _viewModel.Content;

            if (_headerText != null)
                _headerText.text = content.Header;

            if (_descriptionText != null)
                _descriptionText.text = content.Description;

            if (_panelRect != null && _viewModel.IsVisible)
            {
                ApplyLayoutDrivenTooltipSize(content);
                UpdateTooltipPosition();
            }
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void OnViewModelStateChanged()
        {
            Refresh();
        }

        private void UpdateTooltipPosition()
        {
            if (_positionController == null)
                return;

            _positionController.UpdatePosition();
        }

        private void ApplyLayoutDrivenTooltipSize(ITooltipContentViewModel content)
        {
            if (_layoutController == null)
                return;

            _layoutController.ApplyLayoutDrivenTooltipSize(content);
        }
    }
}
