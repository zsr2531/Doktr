namespace Doktr.Core.Models.Collections;

public static class CollectionExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> items)
    {
        return items switch
        {
            IList<T> list => list.Count == 0,
            IReadOnlyList<T> roList => roList.Count == 0,
            ICollection<T> coll => coll.Count == 0,
            IReadOnlyCollection<T> roColl => roColl.Count == 0,
            _ => !items.Any()
        };
    }
}