namespace aoc2024.Infra;

internal static class Ext
{
    internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
    {
        if (length == 1)
        {
            return list.Select(t => new T[] { t });
        }

        return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    internal static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> items, int count)
    {
        int i = 0;
        foreach (var item in items)
        {
            if (count == 1)
            {
                yield return new T[] { item };
            }
            else
            {
                foreach (var result in GetCombinations(items.Skip(i + 1), count - 1))
                {
                    yield return new T[] { item }.Concat(result);
                }
            }
            ++i;
        }
    }

    internal static string Rev(this string s) => string.Join(string.Empty, s.Reverse());

    internal static ulong LCM(params ulong[] values)
    {
        var org = values.ToList();
        var seq = values.ToList();

        while (true)
        {
            var min = seq.Min();
            var minIndex = seq.IndexOf(min);

            seq[minIndex] += org[minIndex];

            if (seq.All(x => x == seq[0]))
            {
                return seq.First();
            }
        }
    }

    internal static List<List<T>> GetCombinationsWithRepetition<T>(this IEnumerable<T> items, int length)
    {
        var combinations = new List<List<T>>();
        GenerateCombinations(items, length, [], combinations);

        return combinations;
    }

    internal static void GenerateCombinations<T>(this IEnumerable<T> items, int length, List<T> current, List<List<T>> combinations)
    {
        if (current.Count == length)
        {
            combinations.Add(new List<T>(current));

            return;
        }

        foreach (T item in items)
        {
            current.Add(item);
            GenerateCombinations(items, length, current, combinations);
            current.RemoveAt(current.Count - 1);
        }
    }
}