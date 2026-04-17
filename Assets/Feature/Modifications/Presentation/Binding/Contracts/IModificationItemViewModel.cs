using UnityEngine;

namespace Feature.Modifications.Presentation.Binding.Contracts
{
    public interface IModificationItemViewModel
    {
        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public string TypeDisplayName { get; }

        public Sprite TypeIcon { get; }

        public Color TypeColor { get; }

        public bool IsInteractable { get; }

        public bool IsDimmed { get; }
    }
}
