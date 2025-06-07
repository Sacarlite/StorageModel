namespace Warehouse.Domain.Interfaces
{
    public interface IStorageItem : IHasWeight
    {
        int Id { get; }
        double Width { get; }
        double Height { get; }
        double Depth { get; }
        double Volume { get; }
    }
}
