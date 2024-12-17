namespace aoc2024.Code;

internal class Day17 : BaseDay
{
    class BitComputer(long A, long B, long C)
    {
        public List<long> Output { get; } = [];

        long _ip = 0;

        public void Run(long[] code)
        {
            while (_ip < code.Length)
            {
                Run(code[_ip], code[_ip + 1]);
                _ip += 2;
            }
        }

        private void Run(long inst, long op)
        {
            switch (inst)
            {
                case 0:
                    A /= MathPow(GetCombo(op));
                    break;
                case 1:
                    B ^= op;
                    break;
                case 2:
                    B = GetCombo(op) % 8;
                    break;
                case 3:
                    if (A != 0)
                    {
                        _ip = op - 2;
                    }
                    break;
                case 4:
                    B ^= C;
                    break;
                case 5:
                    Output.Add(GetCombo(op) % 8);
                    break;
                case 6:
                    B = A / MathPow(GetCombo(op));

                    break;
                case 7:
                    C = A / MathPow(GetCombo(op));
                    break;
            }
        }

        static long MathPow(long op)
        {
            return op switch
            {
                0 => 1,
                1 => 2,
                2 => 4,
                3 => 8,
                4 => 16,
                5 => 32,
                6 => 64,
                7 => 128,
                _ => throw new NotImplementedException()
            };
        }

        long GetCombo(long op) => op switch
        {
            (>= 0) and (<= 3) => op,
            4 => A,
            5 => B,
            6 => C,
            7 => throw new InvalidProgramException(),
            _ => throw new NotImplementedException()
        };

        public void Reset(long a, long b, long c)
        {
            _ip = 0;
            A = a;
            B = b;
            C = c;
            Output.Clear();
        }
    }

    protected override object Part1()
    {
        var data = ReadAllLinesSplit(": ,", true).ToList();
        var comp = new BitComputer(long.Parse(data[0].Last()), long.Parse(data[2].Last()), long.Parse(data[1].Last()));

        comp.Run(data.Last().Skip(1).Select(long.Parse).ToArray());

        return string.Join(",", comp.Output);
    }

    protected override object Part2()
    {
        var data = ReadAllLinesSplit(": ,", true).ToList();
        var code = data.Last().Skip(1).Select(long.Parse).ToArray();
        var b = long.Parse(data[2].Last());
        var c = long.Parse(data[1].Last());

        var a = 0L;
        var comp = new BitComputer(0, b, c);

        for (var i = 15; i >= 0; i--, a *= 8)
        {
            while (true)
            {
                comp.Reset(a, b, c);
                comp.Run(code);

                if (comp.Output.SequenceEqual(code.TakeLast(comp.Output.Count)))
                {
                    break;
                }

                a++;
            }
        }

        return a / 8;
    }
}