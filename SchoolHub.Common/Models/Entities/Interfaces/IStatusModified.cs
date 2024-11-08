namespace SchoolHub.Common.Models.Entities.Interfaces
{
    public interface IStatusModified
    {
        bool Deleted { get; set; }

        DateTime? DateDeleted { get; set; }

        DateTime DateRegistration { get; set; }

        DateTime? LastModificationDate { get; set; }
    }
}
