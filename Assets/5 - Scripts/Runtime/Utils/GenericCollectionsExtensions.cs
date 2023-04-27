namespace System.Collections.Generic
{
    public static class GenericCollectionsExtensions
    {
        public static int FindIndex<T>(this IReadOnlyList<T> readonlyList, Predicate<T> match)
        {
            return ((List<T>)readonlyList).FindIndex(match);
        }

        public static T Find<T>(this IReadOnlyList<T> readonlyList, Predicate<T> match)
        {
            return ((List<T>)readonlyList).Find(match);
        }
    }
}
