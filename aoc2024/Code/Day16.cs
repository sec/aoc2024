namespace aoc2024.Code;

internal class Day16 : BaseDay
{
    enum Dir { N, S, W, E };

    record XY(int X, int Y);
    record State(XY Pos, Dir Where);
    record Path(HashSet<State> Visited, State Last);

    int Walk(bool returnCost)
    {
        var data = ReadAllLines(true);
        var width = data[0].Length;
        var height = data.Length;
        var map = new char[height, width];
        var start = new XY(0, 0);
        var end = new XY(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var c = data[y][x];
                if (c == 'S')
                {
                    start = new(x, y);
                    c = '.';
                }
                else if (c == 'E')
                {
                    end = new(x, y);
                    c = '.';
                }
                map[y, x] = c;
            }
        }


        var minCost = -1;
        var totalCount = new HashSet<XY>();

        var fringe = new PriorityQueue<Path, int>();
        var startState = new State(start, Dir.E);
        var starter = new Path(new HashSet<State>([startState]), startState);

        fringe.Enqueue(starter, 0);

        while (fringe.TryDequeue(out var path, out var cost))
        {
            if (minCost != -1 && cost > minCost)
            {
                continue;
            }

            var last = path.Last;
            if (last.Pos == end)
            {
                minCost = cost;

                if (returnCost)
                {
                    return cost;
                }

                foreach (var v in path.Visited)
                {
                    totalCount.Add(v.Pos);
                }
                continue;
            }

            var forward = Vector(last.Where);
            if (map[forward.Y + last.Pos.Y, forward.X + last.Pos.X] == '.')
            {
                var newState = new State(new(forward.X + last.Pos.X, forward.Y + last.Pos.Y), last.Where);
                if (!path.Visited.Contains(newState))
                {
                    var newCost = cost + 1;

                    var dontAdd = false;
                    foreach (var (Element, Priority) in fringe.UnorderedItems)
                    {
                        if (Element.Visited.Contains(newState) && Priority < newCost)
                        {
                            dontAdd = true;
                            break;
                        }
                    }

                    if (!dontAdd)
                    {
                        var newPath = path.Visited.ToHashSet();
                        newPath.Add(newState);

                        fringe.Enqueue(new Path(newPath, newState), newCost);
                    }
                }
            }

            var moves = new List<Dir>();
            if (last.Where is Dir.N or Dir.S)
            {
                moves.Add(Dir.W);
                moves.Add(Dir.E);
            }
            else if (last.Where is Dir.W or Dir.E)
            {
                moves.Add(Dir.N);
                moves.Add(Dir.S);
            }

            foreach (var move in moves)
            {
                var newState = new State(last.Pos, move);
                if (path.Visited.Contains(newState))
                {
                    continue;
                }

                var newCost = cost + 1000;

                var dontAdd = false;
                foreach (var (Element, Priority) in fringe.UnorderedItems)
                {
                    if (Element.Visited.Contains(newState) && Priority < newCost)
                    {
                        dontAdd = true;
                        break;
                    }
                }

                if (dontAdd)
                {
                    continue;
                }

                var newPath = path.Visited.ToHashSet();
                newPath.Add(newState);

                fringe.Enqueue(new Path(newPath, newState), newCost);
            }
        }

        return totalCount.Count;
    }

    static XY Vector(Dir dir)
    {
        return dir switch
        {
            Dir.W => new(-1, 0),
            Dir.E => new(1, 0),
            Dir.N => new(0, -1),
            Dir.S => new(0, 1),
            _ => throw new NotImplementedException()
        };
    }

    protected override object Part1() => Walk(true);

    protected override object Part2() => Walk(false);
}