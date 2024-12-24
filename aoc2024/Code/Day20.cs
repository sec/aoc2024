namespace aoc2024.Code;

internal class Day20 : BaseDay
{
    record XY(int X, int Y)
    {
        public int ManhattanDistance(XY other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    static List<XY> Race(bool[,] map, XY start, XY end)
    {
        var fringe = new Queue<(XY Pos, int Step)>();
        var visited = new HashSet<XY>();
        var parent = new Dictionary<XY, XY>();

        fringe.Enqueue((start, 0));

        while (fringe.TryDequeue(out var state))
        {
            if (state.Pos == end)
            {
                var route = new List<XY>();
                var current = end;
                route.Add(current);

                while (parent.ContainsKey(current))
                {
                    route.Add(parent[current]);
                    current = parent[current];
                }

                return route;
            }
            visited.Add(state.Pos);

            Move(state.Pos, state.Step, -1, 0);
            Move(state.Pos, state.Step, 1, 0);
            Move(state.Pos, state.Step, 0, -1);
            Move(state.Pos, state.Step, 0, 1);
        }

        void Move(XY p, int step, int x, int y)
        {
            var npos = new XY(p.X + x, p.Y + y);

            if (map[npos.Y, npos.X])
            {
                return;
            }
            if (visited.Add(npos))
            {
                fringe.Enqueue((npos, step + 1));
                parent[npos] = p;
            }
        }

        throw new NotImplementedException();
    }

    protected int Race(int saveTime, int cheatLen)
    {
        var data = ReadAllLines(true);
        var width = data[0].Length;
        var height = data.Length;
        var map = new bool[height, width];

        var start = new XY(0, 0);
        var end = new XY(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var c = data[y][x];
                map[y, x] = c == '#';
                if (c == 'S')
                {
                    start = new(x, y);
                }
                else if (c == 'E')
                {
                    end = new(x, y);
                }
            }
        }

        var total = 0;
        var path = Race(map, start, end);

        for (int i = 0; i < path.Count; i++)
        {
            for (int j = 0; j < path.Count; j++)
            {
                var dist = path[i].ManhattanDistance(path[j]);
                var diff = i - (j + dist);
                if (dist <= cheatLen && diff >= saveTime)
                {
                    total++;
                }
            }
        }

        return total;
    }

    protected override object Part1() => Race(_testRun ? 12 : 100, 2);

    protected override object Part2() => Race(_testRun ? 72 : 100, 20);
}