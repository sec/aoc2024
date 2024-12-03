namespace aoc2024.Code;

internal partial class Day03 : BaseDay
{
    const string DO = "do()";
    const string DONT = "don't()";

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();

    static long Scan(string line) => MulRegex()
        .Matches(line)
        .Sum(x => long.Parse(x.Groups[1].Value) * long.Parse(x.Groups[2].Value));

    protected override object Part1() => Scan(ReadAllText());

    protected override object Part2() => (DO + ReadAllText())
        .Replace("\n", "")
        .Replace("\r", "")
        .Replace(DO, $"{Environment.NewLine}{DO}")
        .Replace(DONT, $"{Environment.NewLine}{DONT}")
        .Split(Environment.NewLine)
        .Where(x => x.StartsWith(DO))
        .Sum(Scan);
}