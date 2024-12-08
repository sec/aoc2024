namespace aoc2024.Code;

internal class Day08 : BaseDay
{
    record struct Antenna(int X, int Y);

    static bool IsOnLine(Antenna a1, Antenna a2, Antenna a3) => (a2.Y - a1.Y) * (a3.X - a1.X) == (a2.X - a1.X) * (a3.Y - a1.Y);

    static IEnumerable<Antenna> Spawn(Antenna a, int dx, int dy)
    {
        yield return new Antenna(a.X + dx, a.Y + dy);
        yield return new Antenna(a.X - dx, a.Y - dy);
        yield return new Antenna(a.X + dx, a.Y - dy);
        yield return new Antenna(a.X - dx, a.Y + dy);
    }

    int Antinodes(Func<Antenna, Antenna, int, int, int, int, IEnumerable<(int X, int Y)>> func)
    {
        var map = ReadAllLines(true);
        var width = map[0].Length;
        var height = map.Length;

        var data = new Dictionary<char, List<Antenna>>();
        var hashset = new HashSet<(int, int)>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var c = map[y][x];
                if (c == '.')
                {
                    continue;
                }

                if (!data.TryGetValue(c, out var value))
                {
                    value = data[c] = [];
                }

                value.Add(new(x, y));
            }
        }

        foreach (var list in data.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var a = list[i];
                    var b = list[j];

                    var dx = a.X - b.X;
                    var dy = a.Y - b.Y;

                    var valid = func(a, b, dx, dy, width, height);
                    foreach (var (X, Y) in valid)
                    {
                        hashset.Add((X, Y));
                    }
                }
            }
        }

        return hashset.Count;
    }

    protected override object Part1() => Antinodes((a, b, dx, dy, width, height) =>
         Spawn(a, dx, dy).Union(Spawn(b, dx, dy))
                    .Where(i => IsOnLine(a, b, i))
                    .Where(i => i.X != a.X && i.Y != a.Y)
                    .Where(i => i.X != b.X && i.Y != b.Y)
                    .Where(i => i.X >= 0 && i.X < width && i.Y >= 0 && i.Y < height)
                    .Select(i => (i.X, i.Y)));

    protected override object Part2() => Antinodes((a, b, dx, dy, width, height) =>
    {
        var valid = new HashSet<(int, int)>();
        var checker = new HashSet<Antenna>();
        var fringe = new Queue<Antenna>();

        fringe.Enqueue(a);
        fringe.Enqueue(b);

        while (fringe.Count > 0)
        {
            var p = fringe.Dequeue();
            if (!checker.Add(p))
            {
                continue;
            }

            foreach (var s in Spawn(p, dx, dy).Where(i => IsOnLine(a, b, i)))
            {
                if (s.X >= 0 && s.X < width && s.Y >= 0 && s.Y < height)
                {
                    valid.Add((s.X, s.Y));
                    fringe.Enqueue(s);
                }
            }
        }

        return valid;
    });
}