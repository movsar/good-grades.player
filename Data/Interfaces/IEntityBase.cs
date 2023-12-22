namespace Data.Interfaces
{
    public interface IEntityBase
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public abstract void SetFromModel(IModelBase model);
        public abstract IModelBase ToModel();
    }
}
