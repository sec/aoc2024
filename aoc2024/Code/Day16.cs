namespace aoc2024.Code;

internal class Day16 : BaseDay
{
    enum Dir { N, S, W, E };

    record XY(int X, int Y);
    record Path(HashSet<XY> Visited, XY LastPos, Dir LastDir);

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
        var starter = new Path(new HashSet<XY>([start]), start, Dir.E);
        fringe.Enqueue(starter, 0);

        while (fringe.TryDequeue(out var path, out var cost))
        {
            if (minCost != -1 && cost > minCost)
            {
                continue;
            }

            if (path.LastPos == end)
            {
                minCost = cost;

                if (returnCost)
                {
                    return cost;
                }

                foreach (var v in path.Visited)
                {
                    totalCount.Add(v);
                }
                continue;
            }

            var moves = new List<Dir>() { path.LastDir };
            if (path.LastDir is Dir.N or Dir.S)
            {
                moves.Add(Dir.W);
                moves.Add(Dir.E);
            }
            else
            {
                moves.Add(Dir.N);
                moves.Add(Dir.S);
            }

            foreach (var move in moves)
            {
                var vector = Vector(move);
                var newPos = new XY(path.LastPos.X + vector.X, path.LastPos.Y + vector.Y);
                if (map[newPos.Y, newPos.X] != '.')
                {
                    continue;
                }
                if (path.Visited.Contains(newPos))
                {
                    continue;
                }

                var newCost = cost + (move == path.LastDir ? 1 : 1001);

                var dontAdd = false;
                foreach (var (Element, Priority) in fringe.UnorderedItems)
                {
                    if (Priority < newCost && Element.Visited.Contains(newPos))
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
                newPath.Add(newPos);

                fringe.Enqueue(new Path(newPath, newPos, move), newCost);
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