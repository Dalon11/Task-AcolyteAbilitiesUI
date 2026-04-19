using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.Modifications.Presentation.Binding.Contracts
{
    public interface IModificationsListViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IModificationItemViewModel> Items { get; }

        public void BeginStateChangeBatch();

        public void EndStateChangeBatch();

        public void Rebuild(IReadOnlyList<ModificationModel> modifications);

        public void ApplyAvailabilityByAbilities(IReadOnlyList<AbilityModel> abilities);

        public bool TryLockById(string modificationId, out IModificationItemViewModel item);

        public void UnlockById(string modificationId);

        public bool TryGetItemById(string modificationId, out IModificationItemViewModel item);

        public bool TryGetTooltipContent(string modificationId, out string header, out string description);
    }
}
