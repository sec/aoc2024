namespace aoc2024.Code;

internal class Day01 : BaseDay
{
    int Solver(Func<(List<int> One, List<int> Two), int> solver)
    {
        var one = new List<int>();
        var two = new List<int>();

        foreach (var item in ReadAllLinesSplit(" ", true))
        {
            one.Add(int.Parse(item[0]));
            two.Add(int.Parse(item[1]));
        }

        one.Sort();
        two.Sort();

        return solver((one, two));
    }

    protected override object Part1() => Solver(d => Enumerable.Zip(d.One, d.Two).Select(zip => Math.Abs(zip.First - zip.Second)).Sum());

    protected override object Part2() => Solver(d => d.One.Select(x => x * d.Two.Count(y => y == x)).Sum());
}