namespace aoc2024.Code;

internal class Day19 : BaseDay
{
    class TrieNode()
    {
        public TrieNode[] Nodes = new TrieNode['z' - 'a'];
        public bool End = false;
        public string Value = string.Empty;

        public void Insert(string key)
        {
            var current = this;
            for (int i = 0; i < key.Length; i++)
            {
                var index = key[i] - 'a';

                if (current.Nodes[index] is null)
                {
                    current.Nodes[index] = new();
                }
                current = current.Nodes[index];
            }
            current.End = true;
            current.Value = key;
        }

        public bool Search(string word)
        {
            var current = this;

            for (int i = 0; i < word.Length; i++)
            {
                var index = word[i] - 'a';

                if (current.Nodes[index] is not null)
                {
                    current = current.Nodes[index];

                    if (current.End && Search(word[(i + 1)..]))
                    {
                        return true;
                    }
                }
                else
                {
                    if (current.End == false)
                    {
                        return false;
                    }
                    current = this;
                    i--;
                }
            }

            return current.End;
        }
    }

    readonly static Dictionary<string, long> _cache = [];

    static long Check(string design, List<string> avail)
    {
        if (string.IsNullOrEmpty(design))
        {
            return 1;
        }

        if (_cache.TryGetValue(design, out var total))
        {
            return total;
        }

        total = _cache[design] = avail
            .Where(design.StartsWith)
            .Select(s => Check(design[s.Length..], avail))
            .Sum();

        return total;
    }

    protected override object Part1()
    {
        var avail = ReadAllLines()[0].Split(", ").Select(s => s.Trim()).ToList();
        var designs = ReadAllLines(true).Skip(1).Select(x => x.Trim()).ToList();

        var root = new TrieNode();
        avail.ForEach(root.Insert);

        return designs.Where(root.Search).Count();
    }

    protected override object Part2()
    {
        _cache.Clear();

        var avail = ReadAllLines()[0].Split(", ").Select(s => s.Trim()).ToList();
        var designs = ReadAllLines(true).Skip(1).Select(x => x.Trim()).ToList();

        return designs.Sum(s => Check(s, avail));
    }
}