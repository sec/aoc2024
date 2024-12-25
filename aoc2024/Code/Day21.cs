namespace aoc2024.Code;

internal class Day21 : BaseDay
{
    record XY(int X, int Y);

    static readonly Dictionary<(string, int), long> _codeCache = [];

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
        { ' ', new XY(0, 0) }, { '^', new XY(1, 0) }, { 'A', new XY(2, 0) },
        { '<', new XY(0, 1) }, { 'v', new XY(1, 1) }, { '>', new XY(2, 1) }
    };

    static IEnumerable<IEnumerable<char>> PathAll(char start, char end, char[,] pad, Dictionary<char, XY> map)
    {
        var width = pad.GetLength(1);
        var height = pad.GetLength(0);

        var fringe = new Queue<List<XY>>();
        fringe.Enqueue(new([map[start]]));

        while (fringe.TryDequeue(out var path))
        {
            if (path.Last() == map[end])
            {
                yield return path.Zip(path.Skip(1)).Select(zip => new XY(zip.First.X - zip.Second.X, zip.First.Y - zip.Second.Y) switch
                {
                    (-1, 0) => '>',
                    (1, 0) => '<',
                    (0, -1) => 'v',
                    (0, 1) => '^',
                    _ => throw new NotImplementedException()
                }).Concat(['A']);
            }

            foreach (var (x, y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var u = path.Last();
                var v = new XY(u.X + x, u.Y + y);
                if (v.X < 0 || v.X >= width || v.Y < 0 || v.Y >= height || pad[v.Y, v.X] == ' ' || path.Contains(v))
                {
                    continue;
                }
                fringe.Enqueue([.. path, v]);
            }
        }
    }

    static long EnterCode(string code, bool useKeypad, int level)
    {
        code = 'A' + code;

        if (!useKeypad && _codeCache.TryGetValue((code, level), out var total))
        {
            return total;
        }

        var pad = useKeypad ? _keypad : _dirpad;
        var map = useKeypad ? _keypadMap : _dirpadMap;

        total = 0L;

        foreach (var (from, to) in code.Zip(code.Skip(1)))
        {
            var paths = PathAll(from, to, pad, map);

            if (level < 1)
            {
                total += paths.Select(c => c.Count()).Min();
            }
            else
            {
                total += paths.Select(p => EnterCode(string.Join("", p), false, level - 1)).Min();
            }
        }

        _codeCache[(code, level)] = total;

        return total;
    }

    long EnterCode(int level) => ReadAllLines(true).Select(code => EnterCode(code, true, level) * int.Parse(code[..3])).Sum();

    protected override object Part1() => EnterCode(2);

    protected override object Part2() => EnterCode(25);
}