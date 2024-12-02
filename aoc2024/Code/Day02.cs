namespace aoc2024.Code;

internal class Day02 : BaseDay
{
    static bool IsSafe(List<int> items)
    {
        var sign = Math.Sign(items[0] - items[1]);

        for (int i = 0; i < items.Count - 1; i++)
        {
            var diff = items[i] - items[i + 1];

            if (Math.Sign(diff) != sign)
            {
                return false;
            }

            if (Math.Abs(diff) is < 1 or > 3)
            {
                return false;
            }
        }
        return true;
    }

    protected override object Part1() => ReadAllLinesSplit(" ", true).Count(x => IsSafe(x.Select(int.Parse).ToList()));

    protected override object Part2()
    {
        var total = 0;

        foreach (var line in ReadAllLinesSplit(" ", true))
        {
            var items = line.Select(int.Parse).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                var level = items.ToList();
                level.RemoveAt(i);

                if (IsSafe(level))
                {
                    total++;
                    break;
                }
            }
        }

        return total;
    }
}