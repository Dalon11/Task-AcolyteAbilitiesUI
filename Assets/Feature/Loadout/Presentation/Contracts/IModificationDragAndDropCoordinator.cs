namespace Feature.Loadout.Presentation.Contracts
{
    public interface IModificationDragAndDropCoordinator
    {
        public bool TryStartDragFromModification(string modificationId);

        public bool TryStartDragFromAbility(string abilityId);

        public void EndDrag(string abilityId);

        public void CancelDrag();
    }
}
