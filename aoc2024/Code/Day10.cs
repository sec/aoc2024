namespace aoc2024.Code;

internal class Day10 : BaseDay
{
    record Node(int X, int Y, int H);

    record Grid(int Width, int Height, Node[,] Map, List<Node> All);

    protected override object Part1() => Hike(false);

    protected override object Part2() => Hike(true);

    int Hike(bool countDistinct)
    {
        var map = GetMap();

        return map.All
            .Where(i => i.H == 0)
            .Select(i => Walk(i, map, countDistinct))
            .Sum();
    }

    Grid GetMap()
    {
        var data = ReadAllLines(true);
        var width = data[0].Length;
        var height = data.Length;

        var map = new Node[height, width];
        var all = new List<Node>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var node = new Node(x, y, data[y][x] - '0');
                map[y, x] = node;
                all.Add(node);
            }
        }
        return new Grid(width, height, map, all);
    }

    static int Walk(Node i, Grid map, bool countDistinct)
    {
        var fringe = new Queue<Node>();
        var visited = new HashSet<(int, int)>();
        var score = 0;

        fringe.Enqueue(i);
        visited.Add((i.X, i.Y));

        while (fringe.Count > 0)
        {
            var node = fringe.Dequeue();

            if (node.H == 9)
            {
                score++;
                continue;
            }

            foreach (var (X, Y) in new (int X, int Y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var nx = node.X + X;
                var ny = node.Y + Y;

                if (nx < 0 || nx >= map.Width || ny < 0 || ny >= map.Height)
                {
                    continue;
                }

                if (!countDistinct && visited.Contains((nx, ny)))
                {
                    continue;
                }

                var nextNode = map.Map[ny, nx];
                if (node.H + 1 != nextNode.H)
                {
                    continue;
                }

                fringe.Enqueue(nextNode);
                visited.Add((nx, ny));
            }
        }

        return score;
    }
}