namespace PeopleManagement.Domain.Entites
{
    public class BaseEntity
    {
        public virtual long Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime? DeletionAt { get; set; }
    }
}