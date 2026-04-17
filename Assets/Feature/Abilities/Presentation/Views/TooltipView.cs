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
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Vector2 _cursorOffset = Vector2.zero;
        [SerializeField] private float _screenPadding = 10f;

        private ITooltipViewModel _viewModel;
        private TooltipPositionController _positionController;

        private void Awake()
        {
            float resolvedScreenPadding = Mathf.Max(0f, _screenPadding);

            _positionController = new TooltipPositionController(_panelRect, _rootCanvas, _cursorOffset, resolvedScreenPadding);
        }

        private void Update()
        {
            if (_viewModel == null)
                return;

            if (!_viewModel.IsVisible)
                return;

            if (IsAnyMouseButtonDown())
                _viewModel.Hide();
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
                UpdateTooltipPosition();
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

        private bool IsAnyMouseButtonDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0) ||
                   UnityEngine.Input.GetMouseButtonDown(1) ||
                   UnityEngine.Input.GetMouseButtonDown(2);
        }
    }
}
