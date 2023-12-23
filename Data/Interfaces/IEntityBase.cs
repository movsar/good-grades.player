using Realms;

namespace Data.Interfaces
{
    public interface IEntityBase : IRealmObject {
        string Id { get; }
        DateTimeOffset CreatedAt { get; }
        DateTimeOffset ModifiedAt { get; set; }
    }
}
