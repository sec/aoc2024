namespace aoc2024.Code;

internal class Day07 : BaseDay
{
    record struct Equation(long Result, long[] Numbers);

    static bool IsValid(Equation e, string oper) => oper.GetCombinationsWithRepetition(e.Numbers.Length - 1).AsParallel().Any(x => CheckSign(e, x));

    static bool CheckSign(Equation e, List<char> sign)
    {
        var i = 1;
        var acc = e.Numbers[0];

        foreach (var s in sign)
        {
            var right = e.Numbers[i++];
            acc = s switch
            {
                '+' => acc + right,
                '*' => acc * right,
                '|' => long.Parse($"{acc}{right}"),

                _ => throw new NotImplementedException()
            };
        }
        return acc == e.Result;
    }

    IEnumerable<Equation> Data => ReadAllLinesSplit(":", true).Select(x => new Equation(long.Parse(x[0]), x[1].Trim().Split(' ').Select(long.Parse).ToArray()));

    long Run(string oper) => Data.Where(i => IsValid(i, oper)).Sum(x => x.Result);

    protected override object Part1() => Run("+*");

    protected override object Part2() => Run("+*|");
}