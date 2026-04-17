namespace Feature.Loadout.Presentation.Contracts
{
    public interface IModificationDragAndDropCoordinator
    {
        bool TryStartDragFromModification(string modificationId);

        bool TryStartDragFromAbility(string abilityId);

        void EndDrag(string abilityId);

        void CancelDrag();
    }
}
