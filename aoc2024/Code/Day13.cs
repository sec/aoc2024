namespace aoc2024.Code;

internal partial class Day13 : BaseDay
{
    [GeneratedRegex(@"(\d+).+?(\d+)")]
    private static partial Regex TwoNumbers();

    record struct Button(long X, long Y);

    record struct Arcade(Button A, Button B, Button Prize);

    static long Play(Arcade arcade, long add)
    {
        var resultX = arcade.Prize.X + add;
        var resultY = arcade.Prize.Y + add;

        var w = arcade.A.X * arcade.B.Y - arcade.B.X * arcade.A.Y;
        var wx = resultX * arcade.B.Y - resultY * arcade.B.X;
        var wy = arcade.A.X * resultY - resultX * arcade.A.Y;

        var pressA = wx / w;
        var pressB = wy / w;

        if (arcade.A.X * pressA + arcade.B.X * pressB == resultX && arcade.A.Y * pressA + arcade.B.Y * pressB == resultY)
        {
            return pressA * 3 + pressB;
        }
        return 0;
    }

    long PlayArcade(long add)
    {
        var arcades = new List<Arcade>();
        var r = TwoNumbers();
        var data = ReadAllLines(true);
        for (int i = 0; i < data.Length; i += 3)
        {
            var a = r.Match(data[i]);
            var b = r.Match(data[i + 1]);
            var p = r.Match(data[i + 2]);

            arcades.Add(new(
                new(long.Parse(a.Groups[1].Value), long.Parse(a.Groups[2].Value)),
                new(long.Parse(b.Groups[1].Value), long.Parse(b.Groups[2].Value)),
                new(long.Parse(p.Groups[1].Value), long.Parse(p.Groups[2].Value))
            ));
        }

        return arcades.Sum(s => Play(s, add));
    }

    protected override object Part1() => PlayArcade(0);

    protected override object Part2() => PlayArcade(10_000_000_000_000);
}