namespace Sample
{
    public interface IIdentified<TId>
    {
        TId Id { get; set; }
    }
}