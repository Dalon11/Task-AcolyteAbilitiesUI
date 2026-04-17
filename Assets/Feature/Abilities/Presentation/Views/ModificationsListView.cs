using System;
using System.Collections.Generic;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Bind-слой списка модификаторов.
    /// </summary>
    public sealed class ModificationsListView : MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private ModificationItemView _itemPrefab;

        private readonly List<ModificationItemView> _itemViews = new List<ModificationItemView>();

        private IModificationsListViewModel _viewModel;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;

        public void Bind(
            IModificationsListViewModel viewModel,
            Action<string> onHoverEnter,
            Action<string> onHoverExit)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_contentRoot == null)
                throw new InvalidOperationException("Не назначен ContentRoot для ModificationsListView.");

            if (_itemPrefab == null)
                throw new InvalidOperationException("Не назначен ItemPrefab для ModificationsListView.");

            Unbind();

            _viewModel = viewModel;
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
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

            for (int i = 0; i < _itemViews.Count; i++)
            {
                ModificationItemView itemView = _itemViews[i];
                if (itemView == null)
                    continue;

                itemView.Unbind();
                Destroy(itemView.gameObject);
            }

            _itemViews.Clear();
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            IReadOnlyList<IModificationItemViewModel> items = _viewModel.Items;
            EnsureItemViewCount(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                ModificationItemView itemView = _itemViews[i];
                IModificationItemViewModel itemViewModel = items[i];
                itemView.Bind(itemViewModel);
                itemView.SetInputHandlers(_onHoverEnter, _onHoverExit);
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
            while (_itemViews.Count < targetCount)
            {
                ModificationItemView itemView = Instantiate(_itemPrefab, _contentRoot);
                _itemViews.Add(itemView);
            }

            while (_itemViews.Count > targetCount)
            {
                int lastIndex = _itemViews.Count - 1;
                ModificationItemView itemView = _itemViews[lastIndex];
                _itemViews.RemoveAt(lastIndex);

                if (itemView == null)
                    continue;

                itemView.Unbind();
                Destroy(itemView.gameObject);
            }
        }
    }
}
