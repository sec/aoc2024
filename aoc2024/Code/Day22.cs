using System.Collections.Concurrent;

namespace aoc2024.Code;

internal class Day22 : BaseDay
{
    class MonkeySecret(long seed)
    {
        long _seed = seed;

        readonly int[] _bananas = new int[2000];
        readonly int[] _prices = new int[2000];

        int _index = 0;

        public long Next()
        {
            var startPrice = (int) _seed % 10;

            _seed = (_seed ^ (_seed * 64)) % 16777216;
            _seed = (_seed ^ (_seed / 32)) % 16777216;
            _seed = (_seed ^ (_seed * 2048)) % 16777216;

            var price = (int) _seed % 10;

            _bananas[_index] = price;
            _prices[_index++] = price - startPrice;

            return _seed;
        }

        public long Next(int n)
        {
            while (n-- > 0)
            {
                Next();
            }
            return _seed;
        }

        public IEnumerable<Memory<int>> Sequences()
        {
            var map = _prices.AsMemory();

            for (int i = 0; i < _prices.Length - 3; i++)
            {
                yield return map.Slice(i, 4);
            }
        }

        public long GetBananas(Memory<int> change)
        {
            for (int i = 0; i < _prices.Length - 3; i++)
            {
                if (_prices[i] == change.Span[0] && _prices[i + 1] == change.Span[1] && _prices[i + 2] == change.Span[2] && _prices[i + 3] == change.Span[3])
                {
                    return _bananas[i + 3];
                }
            }
            return 0;
        }
    }

    protected override object Part1()
    {
        return ReadAllLines(true)
            .Select(long.Parse)
            .Select(s => new MonkeySecret(s))
            .Sum(s => s.Next(2000));
    }

    protected override object Part2()
    {
        var data = ReadAllLines(true)
            .Select(long.Parse)
            .Select(s => new MonkeySecret(s))
            .ToList();

        data.ForEach(b => b.Next(2000));

        var max = long.MinValue;
        var bag = new ConcurrentBag<long>();

        foreach (var buyer in data.Take(5))
        {
            bag.Clear();

            Parallel.ForEach(buyer.Sequences(), () => long.MinValue, (seq, loop, tmp) => Math.Max(data.Sum(x => x.GetBananas(seq)), tmp), bag.Add);
            max = Math.Max(max, bag.Max());
        }

        return max;
    }
}