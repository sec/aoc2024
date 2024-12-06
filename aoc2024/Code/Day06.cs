namespace aoc2024.Code;

internal class Day06 : BaseDay
{
    record XY(int X, int Y);

    enum Way { Up, Down, Right, Left };

    (List<char[]> Map, int Width, int Height, List<XY> PlacesToCheck, XY Start) Init()
    {
        var map = ReadAllLines(true).Select(x => x.ToArray()).ToList();
        var width = map[0].Length;
        var height = map.Count;
        var placesToCheck = new List<XY>();
        var start = new XY(0, 0);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                switch (map[i][j])
                {
                    case '^':
                        start = new(j, i);
                        break;
                    case '.':
                        placesToCheck.Add(new(j, i));
                        break;
                }
            }
        }

        return (map, width, height, placesToCheck, start);
    }

    static void WalkMap(XY Start, List<char[]> Map, int Width, int Height, out int distinctSteps, out bool wasInLoop, bool onlyCheckLoop)
    {
        distinctSteps = 0;
        wasInLoop = false;

        var way = Way.Up;
        var vector = new XY(0, -1);
        var pos = Start;

        var distinct = new HashSet<XY>();
        var visited = new HashSet<(XY, Way)>();

        while (true)
        {
            if (onlyCheckLoop && !visited.Add((pos, way)))
            {
                wasInLoop = true;
                break;
            }

            if (!onlyCheckLoop)
            {
                distinct.Add(pos);
            }

            var newPos = new XY(pos.X + vector.X, pos.Y + vector.Y);
            if (newPos.X < 0 || newPos.X >= Width || newPos.Y < 0 || newPos.Y >= Height)
            {
                break;
            }

            if (Map[newPos.Y][newPos.X] == '#')
            {
                way = way switch
                {
                    Way.Up => Way.Right,
                    Way.Right => Way.Down,
                    Way.Down => Way.Left,
                    Way.Left => Way.Up,

                    _ => throw new NotImplementedException()
                };
                vector = way switch
                {
                    Way.Up => new XY(0, -1),
                    Way.Right => new(1, 0),
                    Way.Down => new(0, 1),
                    Way.Left => new(-1, 0),

                    _ => throw new NotImplementedException()
                };
            }
            else
            {
                pos = newPos;
            }
        }

        distinctSteps = distinct.Count;
    }

    protected override object Part1()
    {
        var (Map, Width, Height, _, Start) = Init();

        WalkMap(Start, Map, Width, Height, out var answer, out _, false);

        return answer;
    }

    protected override object Part2()
    {
        var (Map, Width, Height, PlacesToCheck, Start) = Init();

        var r = 0;
        foreach (var place in PlacesToCheck)
        {
            Map[place.Y][place.X] = '#';

            WalkMap(Start, Map, Width, Height, out _, out var loop, true);

            Map[place.Y][place.X] = '.';

            if (!loop)
            {
                continue;
            }
            r++;
        }
        return r;
    }
}