namespace Core.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; }
        public DateTime? DeletionDate { get; }
        public void SoftDelete(bool cascade = false);
        public void Restore(bool cascade = false);
    }
}