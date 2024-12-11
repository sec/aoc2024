namespace aoc2024.Code;

internal class Day11 : BaseDay
{
    readonly static Dictionary<(long, int), long> _cache = [];

    static long CountBlink(long stone, int times)
    {
        if (_cache.TryGetValue((stone, times), out var cached))
        {
            return cached;
        }

        while (times-- > 0)
        {
            if (stone == 0)
            {
                stone = 1;
                continue;
            }

            if (((int) Math.Log10(stone) + 1) % 2 == 0)
            {
                var str = stone.ToString();
                var left = long.Parse(str[..(str.Length / 2)]);
                var right = long.Parse(str[(str.Length / 2)..]);

                var result1 = CountBlink(left, times);
                var result2 = CountBlink(right, times);

                _cache[(left, times)] = result1;
                _cache[(right, times)] = result2;

                return result1 + result2;
            }

            stone *= (2 << 10) - 24;
        }

        return 1;
    }

    static long Blink(List<long> stones, int times) => stones.Select(s => CountBlink(s, times)).Sum();

    protected override object Part1() => Blink(ReadAllTextSplit(" ").Select(long.Parse).ToList(), 25);

    protected override object Part2() => Blink(ReadAllTextSplit(" ").Select(long.Parse).ToList(), 75);
}