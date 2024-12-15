namespace aoc2024.Code;

internal partial class Day15 : BaseDay
{
    record XY(int X, int Y)
    {
        public int X = X;
        public int Y = Y;

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }
    }

    protected override object Part1()
    {
        var data = ReadAllLines(true);
        var width = data[0].Length;
        var height = 1 + Array.FindLastIndex(data, s => s.Contains("###"));
        var robot = new XY(0, 0);

        var map = new char[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (data[y][x])
                {
                    case '#':
                    case 'O':
                    case '.':
                        map[y, x] = data[y][x];
                        break;
                    case '@':
                        map[y, x] = '.';
                        robot = new(x, y);
                        break;
                }
            }
        }
        var moves = data.Skip(height).SelectMany(x => x.ToCharArray()).ToList();
        var dirs = new Dictionary<char, XY>
        {
            { '^', new(0, -1) },
            { '<', new(-1, 0) },
            { '>', new(1, 0) },
            { 'v', new(0, 1) }
        };

        foreach (var move in moves)
        {
            var next = dirs[move];

            if (CheckMap(robot.X + next.X, robot.Y + next.Y, '.', out _))
            {
                robot = new(robot.X + next.X, robot.Y + next.Y);
                continue;
            }

            if (CheckMap(robot.X + next.X, robot.Y + next.Y, 'O', out _))
            {
                var moved = false;
                var blocks = -1;
                for (int i = 2; i < width; i++)
                {
                    var check = new XY(next.X * i, next.Y * i);
                    CheckMap(robot.X + check.X, robot.Y + check.Y, 'O', out var c);
                    if (c == 'O')
                    {
                        continue;
                    }
                    if (c == '#')
                    {
                        blocks = -1;
                        break;
                    }
                    if (c == '.')
                    {
                        blocks = i;
                        moved = true;
                        break;
                    }
                }
                while (blocks > 0)
                {
                    map[robot.Y + next.Y * blocks, robot.X + next.X * blocks] = map[robot.Y + next.Y * (blocks - 1), robot.X + next.X * (blocks - 1)];
                    blocks--;
                }
                if (moved)
                {
                    map[robot.Y + next.Y, robot.X + next.X] = '.';
                    robot = new(robot.X + next.X, robot.Y + next.Y);
                }
            }
        }

        var sum = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[y, x] == 'O')
                {
                    sum += 100 * y + x;
                }
            }
        }
        return sum;

        bool CheckMap(int x, int y, char what, out char ret)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                ret = ' ';
                return false;
            }
            ret = map[y, x];
            return ret == what;
        }
    }

    protected override object Part2()
    {
        var data = ReadAllLines(true);

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i]
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.");
        }

        var width = data[0].Length;
        var height = 1 + Array.FindLastIndex(data, s => s.Contains("###"));

        var robot = new XY(0, 0);
        var walls = new List<XY>();
        var cargo = new List<XY>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (data[y][x])
                {
                    case '#':
                        walls.Add(new(x, y));
                        break;
                    case '[':
                        cargo.Add(new(x, y));
                        break;
                    case '@':
                        robot = new(x, y);
                        break;
                }
            }
        }
        var moves = data.Skip(height).SelectMany(x => x.ToCharArray()).ToList();
        var dirs = new Dictionary<char, XY>
        {
            { '^', new(0, -1) },
            { '<', new(-1, 0) },
            { '>', new(1, 0) },
            { 'v', new(0, 1) }
        };

        foreach (var move in moves)
        {
            var next = dirs[move];
            var nextRobot = new XY(robot.X + next.X, robot.Y + next.Y);

            if (IsCargoNext(nextRobot.X, nextRobot.Y))
            {
                var box = cargo.Single(c => (c.X == nextRobot.X && c.Y == nextRobot.Y) || (c.X + 1 == nextRobot.X && c.Y == nextRobot.Y));
                var fringe = new Queue<XY>();
                var visited = new HashSet<XY>();

                fringe.Enqueue(box);
                visited.Add(box);
                while (fringe.Count > 0)
                {
                    var current = fringe.Dequeue();

                    var nearby1 = cargo.Where(c => (c.X == (next.X + current.X) && c.Y == (next.Y + current.Y)) || (c.X + 1 == (next.X + current.X) && c.Y == (next.Y + current.Y)));
                    var nearby2 = cargo.Where(c => (c.X == (next.X + current.X + 1) && c.Y == (next.Y + current.Y)) || (c.X + 1 == (next.X + current.X + 1) && c.Y == (next.Y + current.Y)));
                    foreach (var n in nearby1.Union(nearby2))
                    {
                        if (!visited.Add(n))
                        {
                            continue;
                        }
                        fringe.Enqueue(n);
                    }
                }

                var canMove = true;
                foreach (var v in visited)
                {
                    var moved = new XY(v.X + next.X, v.Y + next.Y);
                    if (walls.Any(w => (w.X == moved.X && w.Y == moved.Y) || (w.X == (moved.X + 1) && w.Y == moved.Y)))
                    {
                        canMove = false;
                        break;
                    }
                }

                if (!canMove)
                {
                    continue;
                }

                foreach (var v in visited)
                {
                    v.Move(next.X, next.Y);
                }
                robot = nextRobot;

                continue;
            }

            if (IsWallNext(nextRobot.X, nextRobot.Y))
            {
                continue;
            }

            robot = nextRobot;
        }

        bool IsCargoNext(int x, int y) => cargo.Any(c => (c.X == x && c.Y == y) || (c.X + 1 == x && c.Y == y));

        bool IsWallNext(int x, int y) => walls.Any(w => w.X == x && w.Y == y);

        return cargo.Select(c => 100 * c.Y + c.X).Sum();
    }
}