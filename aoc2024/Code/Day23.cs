namespace aoc2024.Code;

internal class Day23 : BaseDay
{
    Dictionary<string, HashSet<string>> GetLan()
    {
        var hub = new Dictionary<string, HashSet<string>>();
        foreach (var pair in ReadAllLinesSplit("-", true))
        {
            var one = pair[0];
            var two = pair[1];

            if (!hub.TryGetValue(one, out var lan))
            {
                lan = hub[one] = [];
            }
            lan.Add(two);

            if (!hub.TryGetValue(two, out lan))
            {
                lan = hub[two] = [];
            }
            lan.Add(one);
        }
        return hub;
    }

    List<List<string>> FindParty(int partySize)
    {
        var hub = GetLan();

        var fringe = new Queue<List<string>>();
        var party = new List<List<string>>();
        var visited = new HashSet<string>();

        if (partySize == int.MaxValue)
        {
            var first = hub.Keys.First();
            partySize = hub.Where(kv => kv.Value.Contains(first)).Count();
        }

        foreach (var k in hub.Keys)
        {
            fringe.Enqueue([k]);
        }

        while (fringe.TryDequeue(out var link))
        {
            if (link.Count == partySize)
            {
                party.Add(link);
                continue;
            }

            var found = false;
            IEnumerable<string> connected = hub.Where(kv => link.All(kv.Value.Contains)).Select(kv => kv.Key);

            foreach (var c in connected)
            {
                found = true;
                var nl = new List<string>(link) { c };
                nl.Sort();

                var hash = string.Join(",", nl);
                if (visited.Add(hash))
                {
                    fringe.Enqueue(nl);
                }
            }
            if (!found && link.Count == partySize)
            {
                party.Add(link);
            }
        }

        return party;
    }

    protected override object Part1() => FindParty(3).Where(x => x.Any(s => s[0] == 't')).Count();

    protected override object Part2() => string.Join(",", FindParty(int.MaxValue).OrderByDescending(s => s.Count).First());
}