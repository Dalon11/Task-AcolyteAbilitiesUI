using System;
using Feature.CharacterSelection.Core.Enums;
using Feature.Abilities.Presentation.ViewModels;
using Feature.Loadout.Presentation.Contracts;
using Feature.Loadout.Presentation.Contracts.Models;
using Feature.Modifications.Presentation.ViewModels;
using UnityEngine;

namespace Feature.Loadout.Presentation.ViewModels
{

    /// <summary>
    /// Координирует взаимодействие компонентов в сценарии Modification Drag And Drop.
    /// </summary>
    public sealed class ModificationDragAndDropCoordinator : IModificationDragAndDropCoordinator
    {
        private readonly AbilitiesListViewModel _abilitiesListViewModel;
        private readonly ModificationsListViewModel _modificationsListViewModel;
        private readonly ModificationDragSlotViewModel _dragSlotViewModel;

        private DragSourceType _dragSourceType;
        private string _activeModificationId;
        private ModificationType _activeModificationType;
        private Sprite _activeIcon;
        private Color _activeColor;

        public ModificationDragAndDropCoordinator(
            AbilitiesListViewModel abilitiesListViewModel,
            ModificationsListViewModel modificationsListViewModel,
            ModificationDragSlotViewModel dragSlotViewModel)
        {
            if (abilitiesListViewModel == null)
                throw new ArgumentNullException(nameof(abilitiesListViewModel));

            if (modificationsListViewModel == null)
                throw new ArgumentNullException(nameof(modificationsListViewModel));

            if (dragSlotViewModel == null)
                throw new ArgumentNullException(nameof(dragSlotViewModel));

            _abilitiesListViewModel = abilitiesListViewModel;
            _modificationsListViewModel = modificationsListViewModel;
            _dragSlotViewModel = dragSlotViewModel;
            ResetDragState();
        }

        public bool TryStartDragFromModification(string modificationId)
        {
            if (IsDragActive())
                return false;

            ModificationItemViewModel modificationItemViewModel;
            if (!_modificationsListViewModel.TryLockById(modificationId, out modificationItemViewModel))
                return false;

            _dragSourceType = DragSourceType.ModificationList;
            _activeModificationId = modificationItemViewModel.Id;
            _activeModificationType = modificationItemViewModel.ModificationType;
            _activeIcon = ResolveIcon(modificationItemViewModel);
            _activeColor = modificationItemViewModel.TypeColor;

            _dragSlotViewModel.Show(_activeIcon, _activeColor);
            _abilitiesListViewModel.SetDragPreview(_activeModificationType, _activeColor);
            return true;
        }

        public bool TryStartDragFromAbility(string abilityId)
        {
            if (IsDragActive())
                return false;

            DraggedAbilityModificationData draggedData;
            if (!_abilitiesListViewModel.TryTakeAppliedModificationFromAbility(abilityId, out draggedData))
                return false;

            _dragSourceType = DragSourceType.AbilitySlot;
            _activeModificationId = draggedData.ModificationId;
            _activeModificationType = draggedData.ModificationType;
            _activeIcon = draggedData.Icon;
            _activeColor = draggedData.Color;

            _dragSlotViewModel.Show(_activeIcon, _activeColor);
            _abilitiesListViewModel.SetDragPreview(_activeModificationType, _activeColor);
            return true;
        }

        public void EndDrag(string abilityId)
        {
            if (!IsDragActive())
                return;

            bool isApplied = !string.IsNullOrWhiteSpace(abilityId)
                && _abilitiesListViewModel.TryApplyModificationToAbility(
                    abilityId,
                    _activeModificationId,
                    _activeModificationType,
                    _activeIcon,
                    _activeColor);

            if (!isApplied)
                RestoreSourceAfterFailedDrop();

            ClearDragVisualState();
            ResetDragState();
        }

        public void CancelDrag()
        {
            EndDrag(string.Empty);
        }

        private bool IsDragActive()
        {
            return _dragSourceType != DragSourceType.None;
        }

        private void RestoreSourceAfterFailedDrop()
        {
            if (_dragSourceType == DragSourceType.ModificationList)
            {
                _modificationsListViewModel.UnlockById(_activeModificationId);
                return;
            }

            if (_dragSourceType == DragSourceType.AbilitySlot)
            {
                _modificationsListViewModel.UnlockById(_activeModificationId);
            }
        }

        private void ClearDragVisualState()
        {
            _abilitiesListViewModel.ClearDragPreview();
            _dragSlotViewModel.Hide();
        }

        private void ResetDragState()
        {
            _dragSourceType = DragSourceType.None;
            _activeModificationId = string.Empty;
            _activeModificationType = ModificationType.Unknown;
            _activeIcon = null;
            _activeColor = Color.clear;
        }

        private Sprite ResolveIcon(ModificationItemViewModel modificationItemViewModel)
        {
            if (modificationItemViewModel == null)
                return null;

            if (modificationItemViewModel.Icon != null)
                return modificationItemViewModel.Icon;

            return modificationItemViewModel.TypeIcon;
        }

        private enum DragSourceType
        {
            None = 0,
            ModificationList = 1,
            AbilitySlot = 2
        }
    }
}
