namespace aoc2024.Code;

internal partial class Day14 : BaseDay
{
    [GeneratedRegex(@"(-?\d+).+?(-?\d+).+?(-?\d+).+?(-?\d+)")]
    private static partial Regex FourNumbers();

    internal class Robot(int x, int y, int vx, int vy)
    {
        public int X = x, Y = y, VX = vx, VY = vy;

        public void Move(int seconds, (int maxX, int maxY) max)
        {
            X = (X + (VX * seconds)) % max.maxX;
            Y = (Y + (VY * seconds)) % max.maxY;

            if (X < 0)
            {
                X = max.maxX + X;
            }
            if (Y < 0)
            {
                Y = max.maxY + Y;
            }
        }

        public bool InRange(int minX, int maxX, int minY, int maxY) => X >= minX && X <= maxX && Y >= minY && Y <= maxY;

        public static Robot FromString(string src)
        {
            var s = FourNumbers().Match(src);

            return new Robot(int.Parse(s.Groups[1].Value), int.Parse(s.Groups[2].Value), int.Parse(s.Groups[3].Value), int.Parse(s.Groups[4].Value));
        }
    }

    protected override object Part1()
    {
        var r = FourNumbers();
        var max = _testRun ? (11, 7) : (101, 103);
        var robots = ReadAllLines(true).Select(Robot.FromString).ToList();

        robots.ForEach(x => x.Move(100, max));

        var leftX = max.Item1 / 2 - 1;
        var rightX = max.Item1 / 2 + 1;
        var topY = max.Item2 / 2 - 1;
        var bottomY = max.Item2 / 2 + 1;

        var q1 = robots.Where(r => r.InRange(0, leftX, 0, topY)).Count();
        var q2 = robots.Where(r => r.InRange(rightX, max.Item1, 0, topY)).Count();
        var q3 = robots.Where(r => r.InRange(0, leftX, bottomY, max.Item2)).Count();
        var q4 = robots.Where(r => r.InRange(rightX, max.Item1, bottomY, max.Item2)).Count();

        return q1 * q2 * q3 * q4;
    }

    protected override object Part2()
    {
        if (_testRun)
        {
            return 0;
        }

        var max = _testRun ? (11, 7) : (101, 103);
        var robots = ReadAllLines(true).Select(Robot.FromString).ToList();

        for (int i = 1; i < int.MaxValue; i++)
        {
            robots.ForEach(x => x.Move(1, max));
            var g = robots.GroupBy(r => r.Y);

            foreach (var line in g)
            {
                foreach (var r in line)
                {
                    var found = true;
                    for (int check = 0; check < 10; check++)
                    {
                        if (!line.Any(s => s.X + check == r.X && s.Y == r.Y))
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        Console.WriteLine(Output(robots, max));

                        return i;
                    }
                }
            }
        }

        return 0;
    }

    static string Output(List<Robot> robots, (int, int) max)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < max.Item2; y++)
        {
            for (int x = 0; x < max.Item1; x++)
            {
                if (robots.Any(r => r.X == x && r.Y == y))
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('.');
                }

            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}