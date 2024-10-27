namespace Domain.Commons.BaseEntities
{
    public interface IEntityWitkKey<TKey>
    {
        TKey Id { get; set; }
    }
}
