namespace aoc2024.Code;

internal class Day24 : BaseDay
{
    struct BitArray64(long bits)
    {
        public long Bits = bits;

        public bool this[int index]
        {
            readonly get
            {
                long mask = 1L << index;

                return (Bits & mask) == mask;
            }
            set
            {
                long mask = 1L << index;

                if (value)
                {
                    Bits |= mask;
                }
                else
                {
                    Bits &= ~mask;
                }
            }
        }
    }

    record Wire(string Left, string Op, string Right, string Output);

    class LogicSystem
    {
        private readonly Dictionary<string, bool> _gates;
        private readonly List<Wire> _wires;

        public LogicSystem(IEnumerable<string[]> input)
        {
            _gates = [];
            _wires = [];

            foreach (var line in input)
            {
                if (line.Length == 2)
                {
                    _gates[line[0]] = line[1] == "1";
                }
                else
                {
                    _wires.Add(new Wire(line[0], line[1], line[2], line[3]));
                }
            }
        }

        public bool Run()
        {
            while (_wires.Count > 0)
            {
                for (int i = 0; i < _wires.Count; i++)
                {
                    var wire = _wires[i];

                    if (!_gates.TryGetValue(wire.Left, out var left) || !_gates.TryGetValue(wire.Right, out var right))
                    {
                        continue;
                    }

                    var output = wire.Op switch
                    {
                        "XOR" => left ^ right,
                        "AND" => left & right,
                        "OR" => left | right,
                        _ => throw new NotImplementedException()
                    };

                    _gates[wire.Output] = output;

                    _wires.RemoveAt(i--);
                }
            }

            return true;
        }

        public long Get(char reg)
        {
            var ba = new BitArray64();
            var i = 0;
            foreach (var kv in _gates.Where(kv => kv.Key[0] == reg).OrderBy(kv => kv.Key))
            {
                ba[i++] = kv.Value;
            }

            return ba.Bits;
        }

        internal void Set(char reg, long bits)
        {
            var ba = new BitArray64(bits);
            for (int i = 0; i < 45; i++)
            {
                var r = $"{reg}{i:d2}";
                _gates[r] = ba[i];
            }
        }

        internal IEnumerable<string> FindWrongWires()
        {
            foreach (var wire in _wires)
            {
                if (wire.Output[0] == 'z' && wire.Op != "XOR" && wire.Output != "z45")
                {
                    yield return wire.Output;
                    continue;
                }

                if (wire.Output[0] != 'z' && wire.Left[0] is not 'x' and not 'y' && wire.Right[0] is not 'x' and not 'y' && wire.Op == "XOR")
                {
                    yield return wire.Output;
                    continue;
                }

                if (wire.Op == "XOR" && ((wire.Left[0] is 'x' && wire.Right[0] is 'y') || (wire.Left[0] is 'y' && wire.Right[0] is 'x')))
                {
                    if (wire.Left is "x00" or "y00" || wire.Right is "y00" or "x00")
                    {
                        continue;
                    }

                    var found = _wires.Where(w => w.Op == "XOR").Any(w => w != wire && (w.Left == wire.Output || w.Right == wire.Output));
                    if (!found)
                    {
                        yield return wire.Output;
                        continue;
                    }
                }

                if (wire.Op == "AND" && ((wire.Left[0] is 'x' && wire.Right[0] is 'y') || (wire.Left[0] is 'y' && wire.Right[0] is 'x')))
                {
                    if (wire.Left is "x00" or "y00" || wire.Right is "y00" or "x00")
                    {
                        continue;
                    }

                    var found = _wires.Where(w => w.Op == "OR").Any(w => w != wire && (w.Left == wire.Output || w.Right == wire.Output));
                    if (!found)
                    {
                        yield return wire.Output;
                        continue;
                    }
                }
            }
        }
    }

    protected override object Part1()
    {
        var system = new LogicSystem(ReadAllLinesSplit(": ->", true));
        system.Run();

        return system.Get('z');
    }

    protected override object Part2()
    {
        if (_testRun)
        {
            return 0;
        }

        var system = new LogicSystem(ReadAllLinesSplit(": ->", true));

        return string.Join(",", system.FindWrongWires().OrderBy(s => s));
    }
}