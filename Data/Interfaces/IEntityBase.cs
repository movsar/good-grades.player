namespace Data.Interfaces
{
    public interface IEntityBase {
        string Id { get; }
        DateTimeOffset CreatedAt { get; }
        DateTimeOffset ModifiedAt { get; set; }
    }
}
