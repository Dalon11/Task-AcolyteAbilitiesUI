using System;
using System.Collections.Generic;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Bind-слой списка способностей.
    /// </summary>
    public sealed class AbilitiesListView : MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private AbilityItemView _itemPrefab;

        private readonly List<AbilityItemView> _itemViews = new List<AbilityItemView>();

        private IAbilitiesListViewModel _viewModel;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;

        public void Bind(
            IAbilitiesListViewModel viewModel,
            Action<string> onHoverEnter,
            Action<string> onHoverExit)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_contentRoot == null)
                throw new InvalidOperationException("Не назначен ContentRoot для AbilitiesListView.");

            if (_itemPrefab == null)
                throw new InvalidOperationException("Не назначен ItemPrefab для AbilitiesListView.");

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
                AbilityItemView itemView = _itemViews[i];
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

            IReadOnlyList<IAbilityItemViewModel> items = _viewModel.Items;
            EnsureItemViewCount(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                AbilityItemView itemView = _itemViews[i];
                IAbilityItemViewModel itemViewModel = items[i];
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
                AbilityItemView itemView = Instantiate(_itemPrefab, _contentRoot);
                _itemViews.Add(itemView);
            }

            while (_itemViews.Count > targetCount)
            {
                int lastIndex = _itemViews.Count - 1;
                AbilityItemView itemView = _itemViews[lastIndex];
                _itemViews.RemoveAt(lastIndex);

                if (itemView == null)
                    continue;

                itemView.Unbind();
                Destroy(itemView.gameObject);
            }
        }
    }
}
