namespace aoc2024.Code;

internal class Day25 : BaseDay
{
    record FivePin
    {
        readonly BitArray _bits;

        public FivePin(string[] map) => _bits = new BitArray(map.SelectMany(s => s.Select(c => c == '#')).ToArray());

        public bool Match(FivePin other) => !new BitArray(_bits).And(other._bits).HasAnySet();
    }

    protected override object Part1()
    {
        var locks = new List<FivePin>();
        var keys = new List<FivePin>();

        foreach (var map in ReadAllLines(true).Chunk(7))
        {
            (map[0] == "#####" ? locks : keys).Add(new FivePin(map));
        }

        return keys.Select(k => locks.Where(l => l.Match(k)).Count()).Sum();
    }

    protected override object Part2() => 2024;
}