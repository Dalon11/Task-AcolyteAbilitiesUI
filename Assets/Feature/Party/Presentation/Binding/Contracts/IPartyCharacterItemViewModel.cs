using UnityEngine;

namespace Feature.Party.Presentation.Binding.Contracts
{
    public interface IPartyCharacterItemViewModel
    {
        public string Id { get; }

        public string DisplayName { get; }

        public Sprite Icon { get; }

        public bool IsSelected { get; }
    }
}
