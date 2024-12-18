namespace aoc2024.Code;

internal class Day18 : BaseDay
{
    record XY(int X, int Y);
    record State(XY Pos, int Steps);

    int Simulate(int count, out XY lastByte)
    {
        var size = _testRun ? 7 : 71;
        var bytes = ReadAllLinesSplit(",", true)
            .Select(s => new XY(int.Parse(s[0]), int.Parse(s[1])))
            .ToList();

        var start = new XY(0, 0);
        var end = _testRun ? new XY(6, 6) : new XY(70, 70);

        lastByte = bytes[count - 1];
        var corrupted = bytes.Take(count).ToHashSet();
        var fringe = new Queue<State>();
        var visited = new HashSet<XY>();

        fringe.Enqueue(new(start, 0));

        while (fringe.TryDequeue(out var state))
        {
            if (state.Pos == end)
            {
                return state.Steps;
            }
            visited.Add(state.Pos);

            foreach (var (X, Y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var next = new XY(state.Pos.X + X, state.Pos.Y + Y);
                if (next.X < 0 || next.X >= size || next.Y < 0 || next.Y >= size)
                {
                    continue;
                }
                if (corrupted.Contains(next))
                {
                    continue;
                }
                if (visited.Add(next))
                {
                    fringe.Enqueue(new(next, state.Steps + 1));
                }
            }
        }

        return 0;
    }

    protected override object Part1() => Simulate(_testRun ? 12 : 1024, out _);

    protected override object Part2()
    {
        var start = _testRun ? 12 : 1024;
        XY result;

        while (Simulate(start++, out result) > 0)
        {
            // go
        }

        return $"{result.X},{result.Y}";
    }
}