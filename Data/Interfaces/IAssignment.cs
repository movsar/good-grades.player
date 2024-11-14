namespace Data.Interfaces
{
    public interface IAssignment : IEntityBase
    {
        public string Title { get; set; }
        bool IsContentSet { get; }

    }
}
