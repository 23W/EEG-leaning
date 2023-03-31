namespace EEGCore.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }

        public static IEnumerable<int> Range(int from, int to, int step)
        {
            for (int i = from; i < to; i += step)
            {
                yield return i;
            }
        }
    }
}
