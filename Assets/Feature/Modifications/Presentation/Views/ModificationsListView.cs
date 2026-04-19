using System;
using System.Collections.Generic;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Pooling.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Modifications.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Modifications List.
    /// </summary>
    public sealed class ModificationsListView : MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private ModificationItemView _itemPrefab;

        private readonly List<ModificationItemView> _activeItemViews = new List<ModificationItemView>();

        private IModificationsListViewModel _viewModel;
        private IComponentPoolService _componentPoolService;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;
        private Action<string, PointerEventData> _onPointerDown;
        private Action<string, PointerEventData> _onPointerUp;

        public void SetPoolService(IComponentPoolService componentPoolService)
        {
            if (componentPoolService == null)
                throw new ArgumentNullException(nameof(componentPoolService));

            _componentPoolService = componentPoolService;
        }

        public void Bind(
            IModificationsListViewModel viewModel,
            Action<string> onHoverEnter,
            Action<string> onHoverExit,
            Action<string, PointerEventData> onPointerDown,
            Action<string, PointerEventData> onPointerUp)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_contentRoot == null)
                throw new InvalidOperationException("Не задан ContentRoot для ModificationsListView.");

            if (_itemPrefab == null)
                throw new InvalidOperationException("Не задан ItemPrefab для ModificationsListView.");

            if (_componentPoolService == null)
                throw new InvalidOperationException("Не задан пул компонентов для ModificationsListView.");

            Unbind();

            _viewModel = viewModel;
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
            _onPointerDown = onPointerDown;
            _onPointerUp = onPointerUp;
            _viewModel.onStateChanged += OnViewModelStateChanged;

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel != null)
                _viewModel.onStateChanged -= OnViewModelStateChanged;

            _viewModel = null;
            _onHoverEnter = null;
            _onHoverExit = null;
            _onPointerDown = null;
            _onPointerUp = null;

            while (_activeItemViews.Count > 0)
            {
                int lastIndex = _activeItemViews.Count - 1;
                ReleaseItemViewAt(lastIndex);
            }
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            IReadOnlyList<IModificationItemViewModel> items = _viewModel.Items;
            EnsureItemViewCount(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                ModificationItemView itemView = _activeItemViews[i];
                IModificationItemViewModel itemViewModel = items[i];
                itemView.Bind(itemViewModel);
                itemView.SetInputHandlers(_onHoverEnter, _onHoverExit, _onPointerDown, _onPointerUp);
                itemView.transform.SetSiblingIndex(i);
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

        private void EnsureItemViewCount(int targetCount)
        {
            while (_activeItemViews.Count < targetCount)
            {
                ModificationItemView itemView = _componentPoolService.Acquire(_itemPrefab, _contentRoot);
                _activeItemViews.Add(itemView);
            }

            while (_activeItemViews.Count > targetCount)
            {
                int lastIndex = _activeItemViews.Count - 1;
                ReleaseItemViewAt(lastIndex);
            }
        }

        private void ReleaseItemViewAt(int index)
        {
            if (index < 0 || index >= _activeItemViews.Count)
                return;

            ModificationItemView itemView = _activeItemViews[index];
            _activeItemViews.RemoveAt(index);

            if (itemView == null)
                return;

            itemView.Unbind();
            _componentPoolService.Release(itemView);
        }
    }
}
