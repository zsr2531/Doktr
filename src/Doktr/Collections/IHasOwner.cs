namespace Doktr.Collections
{
    public interface IHasOwner<T>
        where T : IHasOwner<T>
    {
        T Owner
        {
            get;
            set;
        }
    }
}