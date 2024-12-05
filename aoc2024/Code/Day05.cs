namespace aoc2024.Code;

internal class Day05 : BaseDay
{
    record struct Order(int A, int B);

    List<Order> Orders() => ReadAllLines(true)
            .Where(x => x.Contains('|'))
            .Select(x => x.Split('|'))
            .Select(x => new Order(int.Parse(x[0]), int.Parse(x[1])))
            .ToList();

    IEnumerable<List<int>> Pages() => ReadAllLines(true)
        .Where(x => x.Contains(','))
        .Select(x => x.Split(',').Select(int.Parse).ToList());

    static bool InCorrectOrder(List<int> update, List<Order> orders)
    {
        for (int i = 0; i < update.Count; i++)
        {
            foreach (var rule in orders.Where(x => x.A == update[i]))
            {
                var index = update.IndexOf(rule.B);
                if (index != -1 && index < i)
                {
                    return false;
                }
            }
        }
        return true;
    }

    static List<int> FixNotInCorrectOrder(List<int> update, List<Order> orders)
    {
        for (int i = 0; i < update.Count; i++)
        {
            var item = update[i];
            foreach (var rule in orders.Where(x => x.A == item))
            {
                var index = update.IndexOf(rule.B);
                if (index != -1 && index < i)
                {
                    update.RemoveAt(i);
                    update.Insert(index, item);
                    i = -1;

                    break;
                }
            }
        }
        return update;
    }

    protected override object Part1()
    {
        var orders = Orders();

        return Pages()
            .Where(x => InCorrectOrder(x, orders))
            .Select(x => x[x.Count / 2])
            .Sum();
    }

    protected override object Part2()
    {
        var orders = Orders();

        return Pages()
            .Where(x => !InCorrectOrder(x, orders))
            .Select(x => FixNotInCorrectOrder(x, orders))
            .Select(x => x[x.Count / 2])
            .Sum();
    }
}