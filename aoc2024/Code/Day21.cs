namespace aoc2024.Code;

internal class Day21 : BaseDay
{
    record XY(int X, int Y);

    static readonly char[,] _keypad = {
        { '7', '8', '9' },
        { '4', '5', '6' },
        { '1', '2', '3' },
        { ' ', '0', 'A' }
    };

    static readonly Dictionary<char, XY> _keypadMap = new()
    {
         { '7', new XY(0, 0) }, { '8', new XY(1, 0) }, { '9', new XY(2, 0) },
         { '4', new XY(0, 1) }, { '5', new XY(1, 1) }, { '6', new XY(2, 1) },
         { '1', new XY(0, 2) }, { '2', new XY(1, 2) }, { '3', new XY(2, 2) },
                                { '0', new XY(1, 3) }, { 'A', new XY(2, 3) }
    };

    static readonly char[,] _dirpad = {
        { ' ', '^', 'A' },
        { '<', 'v', '>' }
    };

    static readonly Dictionary<char, XY> _dirpadMap = new()
    {
                               { '^', new XY(1, 0) }, { 'A', new XY(2, 0) },
        { '<', new XY(0, 1) }, { 'v', new XY(1, 1) }, { '>', new XY(2, 1) }
    };

    static readonly Dictionary<(char, char, bool), List<List<char>>> _cache = [];

    static IEnumerable<List<char>> PathAll(char start, char end, char[,] pad, Dictionary<char, XY> map)
    {
        var width = pad.GetLength(1);
        var height = pad.GetLength(0);

        var dist = new Dictionary<XY, int>();
        var fringe = new Queue<XY>();
        fringe.Enqueue(map[start]);

        var parent = new Dictionary<XY, List<XY>>
        {
            [map[start]] = []
        };
        parent[map[start]].Clear();
        parent[map[start]].Add(new XY(-1, -1));

        foreach (var kv in map)
        {
            dist[kv.Value] = int.MaxValue;
        }

        dist[map[start]] = 0;

        while (fringe.TryDequeue(out var u))
        {
            foreach (var (x, y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var v = new XY(u.X + x, u.Y + y);
                if (v.X < 0 || v.X >= width || v.Y < 0 || v.Y >= height)
                {
                    continue;
                }
                if (pad[v.Y, v.X] == ' ')
                {
                    continue;
                }

                if (dist[v] > dist[u] + 1)
                {
                    dist[v] = dist[u] + 1;
                    fringe.Enqueue(v);

                    parent[v] = [];
                    parent[v].Add(u);
                }
                else if (dist[v] == dist[u] + 1)
                {
                    parent[v].Add(u);
                }
            }
        }

        var paths = new List<List<XY>>();
        FindPaths(paths, [], parent, map[end]);

        foreach (var path in paths)
        {
            path.Reverse();

            var moves = new List<char>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                var dx = path[i].X - path[i + 1].X;
                var dy = path[i].Y - path[i + 1].Y;
                moves.Add(new XY(dx, dy) switch
                {
                    (-1, 0) => '>',
                    (1, 0) => '<',
                    (0, -1) => 'v',
                    (0, 1) => '^',
                    _ => throw new NotImplementedException()
                });
            }
            moves.Add('A');

            yield return moves;
        }
    }

    static void FindPaths(List<List<XY>> paths, List<XY> path, Dictionary<XY, List<XY>> parent, XY u)
    {
        if (u == new XY(-1, -1))
        {
            paths.Add(new List<XY>(path));

            return;
        }

        foreach (var par in parent[u])
        {
            path.Add(u);

            FindPaths(paths, path, parent, par);

            path.RemoveAt(path.Count - 1);
        }
    }

    static IEnumerable<string> FindSequence(string code, bool useKeypad)
    {
        var pad = useKeypad ? _keypad : _dirpad;
        var map = useKeypad ? _keypadMap : _dirpadMap;

        var paths = new List<List<char>>();
        code = 'A' + code;
        for (int i = 0; i < code.Length - 1; i++)
        {
            if (!_cache.TryGetValue((code[i], code[i + 1], useKeypad), out var nextPaths))
            {
                nextPaths = PathAll(code[i], code[i + 1], pad, map).ToList();

                _cache[(code[i], code[i + 1], useKeypad)] = nextPaths;
            }

            if (paths.Count == 0)
            {
                paths = nextPaths;
            }
            else
            {
                paths = (from a in paths
                         from b in nextPaths
                         select a.Concat(b).ToList()).ToList();
            }
        }

        return paths.Select(s => string.Join("", s));
    }

    static int EnterCode(string code, int n)
    {
        var min = int.MaxValue;
        foreach (var s1 in FindSequence(code, true))
        {
            var findMin = FindMin(s1, n);

            min = Math.Min(min, findMin);
        }

        return min;
    }

    private static int FindMin(string s1, int v)
    {
        var min = int.MaxValue;

        foreach (var code in FindSequence(s1, false))
        {
            if (v <= 1)
            {
                min = Math.Min(min, code.Length);
            }
            else
            {
                min = Math.Min(min, FindMin(code, v - 1));
            }
        }

        return min;
    }

    protected override object Part1()
    {
        return ReadAllLines(true)
            .Select(code => EnterCode(code, 2) * int.Parse(code[..3]))
            .Sum();
    }

    protected override object Part2()
    {
        //TODO: d21p2
        return ReadAllLines(true)
            .Select(code => EnterCode(code, 0) * int.Parse(code[..3]))
            .Sum();
    }
}