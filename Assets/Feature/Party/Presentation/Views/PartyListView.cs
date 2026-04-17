using System;
using System.Collections.Generic;
using Feature.Party.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Party.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Party List.
    /// </summary>
    public sealed class PartyListView : MonoBehaviour
    {
        [SerializeField] private PartyCharacterItemView[] _itemViews = new PartyCharacterItemView[0];

        private IPartyViewModel _viewModel;
        private Action<string> _onCharacterClick;

        public void Bind(IPartyViewModel viewModel, Action<string> onCharacterClick)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_itemViews == null)
                throw new InvalidOperationException("Не задан массив элементов PartyCharacterItemView для PartyListView.");

            Unbind();

            _viewModel = viewModel;
            _onCharacterClick = onCharacterClick;
            _viewModel.onStateChanged += OnViewModelStateChanged;

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel != null)
                _viewModel.onStateChanged -= OnViewModelStateChanged;

            _viewModel = null;
            _onCharacterClick = null;

            for (int i = 0; i < _itemViews.Length; i++)
            {
                PartyCharacterItemView itemView = _itemViews[i];
                if (itemView == null)
                    continue;

                itemView.Unbind();
                itemView.gameObject.SetActive(false);
            }
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            IReadOnlyList<IPartyCharacterItemViewModel> items = _viewModel.Items;
            int renderCount = Mathf.Min(items.Count, _itemViews.Length);

            for (int i = 0; i < renderCount; i++)
            {
                PartyCharacterItemView itemView = _itemViews[i];
                if (itemView == null)
                    continue;

                IPartyCharacterItemViewModel itemViewModel = items[i];
                itemView.Bind(itemViewModel);
                itemView.SetInputHandlers(_onCharacterClick);
                itemView.gameObject.SetActive(true);
            }

            for (int i = renderCount; i < _itemViews.Length; i++)
            {
                PartyCharacterItemView itemView = _itemViews[i];
                if (itemView == null)
                    continue;

                itemView.Unbind();
                itemView.gameObject.SetActive(false);
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
    }
}
