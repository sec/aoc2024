namespace aoc2024.Code;

internal class Day12 : BaseDay
{
    record XY(int X, int Y);
    record Region(char Plant, List<XY> Plants)
    {
        public int Area => Plants.Count;

        public int Perimeter => Plants.Select(i =>
        {
            var cnt = 0;
            foreach (var (X, Y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                if (!Plants.Any(s => s.X == i.X + X && s.Y == i.Y + Y))
                {
                    cnt++;
                }
            }
            return cnt;
        }).Sum();

        public int Sides
        {
            get
            {
                var s = 0;
                foreach (var p in Plants)
                {
                    s += !Any(p, -1, 0) && !Any(p, 0, -1) ? 1 : 0;
                    s += !Any(p, -1, 0) && !Any(p, 0, +1) ? 1 : 0;
                    s += !Any(p, +1, 0) && !Any(p, 0, -1) ? 1 : 0;
                    s += !Any(p, +1, 0) && !Any(p, 0, +1) ? 1 : 0;

                    s += Any(p, -1, 0) && Any(p, 0, -1) && !Any(p, -1, -1) ? 1 : 0;
                    s += Any(p, +1, 0) && Any(p, 0, -1) && !Any(p, +1, -1) ? 1 : 0;
                    s += Any(p, -1, 0) && Any(p, 0, +1) && !Any(p, -1, +1) ? 1 : 0;
                    s += Any(p, +1, 0) && Any(p, 0, +1) && !Any(p, +1, +1) ? 1 : 0;
                }

                return s;
            }
        }

        private bool Any(XY p, int mx, int my) => Plants.Any(s => s.X == p.X + mx && s.Y == p.Y + my);
    }

    List<Region> GetRegions()
    {
        var garden = ReadAllLines(true)
            .Select(x => x.ToArray())
            .ToArray();

        var width = garden[0].Length;
        var height = garden.Length;

        var all = new List<(char C, XY P)>();
        var regions = new List<Region>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                all.Add((garden[y][x], new(x, y)));
            }
        }

        while (all.Count > 0)
        {
            var (C, P) = all.First();

            var visited = new HashSet<XY>
            {
                P
            };

            var fringe = new Queue<XY>();
            fringe.Enqueue(P);

            while (fringe.Count > 0)
            {
                var act = fringe.Dequeue();

                foreach (var (X, Y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                {
                    var next = all.SingleOrDefault(i => i.P.X == act.X + X && i.P.Y == act.Y + Y && i.C == C);

                    if (next.P is null || visited.Contains(next.P))
                    {
                        continue;
                    }

                    if (next.C == C)
                    {
                        fringe.Enqueue(next.P);
                        visited.Add(next.P);
                    }
                }
            }

            regions.Add(new Region(C, [.. visited]));

            all.RemoveAll(p => visited.Contains(p.P));
        }
        return regions;
    }

    protected override object Part1() => GetRegions().Sum(x => x.Area * x.Perimeter);

    protected override object Part2() => GetRegions().Sum(x => x.Area * x.Sides);
}