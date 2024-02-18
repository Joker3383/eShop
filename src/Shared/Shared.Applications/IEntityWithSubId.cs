namespace Shared;

public interface IEntityWithSubId<T> : IEntity<T>
{

    T SubId { get; set; }
}