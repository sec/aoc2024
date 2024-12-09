namespace aoc2024.Code;

internal class Day09 : BaseDay
{
    const int FREE_SPACE = -1;

    record struct BlockInfo(int Index, int Length, int Block);

    protected override object Part1() => Checksum(DefragSingle(Expand(ReadAllText())));

    protected override object Part2() => Checksum(DefragFull(Expand(ReadAllText())));

    static ulong Checksum(int[] disk)
    {
        var sum = 0UL;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] == FREE_SPACE)
            {
                continue;
            }
            sum += (ulong) (i * disk[i]);
        }
        return sum;
    }

    static IEnumerable<BlockInfo> FreeSpaces(int[] disk)
    {
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] == FREE_SPACE)
            {
                var j = i + 1;
                while (j < disk.Length && disk[j] == FREE_SPACE)
                {
                    j++;
                }
                yield return new BlockInfo(i, j - i, FREE_SPACE);
                i = j;
            }
        }
    }

    static IEnumerable<BlockInfo> UsedSpaces(int[] disk)
    {
        for (int i = disk.Length - 1; i >= 0; i--)
        {
            if (disk[i] != FREE_SPACE)
            {
                var j = i - 1;
                while (j >= 0 && disk[i] == disk[j])
                {
                    j--;
                }
                yield return new BlockInfo(j + 1, i - j, disk[i]);
                i = j + 1;
            }
        }
    }

    static int[] DefragFull(int[] disk)
    {
        foreach (var used in UsedSpaces(disk))
        {
            foreach (var free in FreeSpaces(disk))
            {
                if (free.Index >= used.Index)
                {
                    break;
                }
                if (free.Length >= used.Length)
                {
                    for (int cnt = 0; cnt < used.Length; cnt++)
                    {
                        disk[free.Index + cnt] = used.Block;
                        disk[used.Index + cnt] = FREE_SPACE;
                    }
                    break;
                }
            }
        }
        return disk;
    }

    static int[] DefragSingle(int[] disk)
    {
        var last_used = disk.Length - 1;

        while (true)
        {
            var free = Array.IndexOf(disk, FREE_SPACE);
            if (free < 0)
            {
                break;
            }

            var used = FREE_SPACE;
            for (int i = last_used; i >= 0; i--)
            {
                if (disk[i] != FREE_SPACE)
                {
                    used = i;
                    last_used = i;
                    break;
                }
            }
            if (used < 0 || used < free)
            {
                break;
            }

            disk[free] = disk[used];
            disk[used] = FREE_SPACE;
        }

        return disk;
    }

    static int[] Expand(string diskmap)
    {
        var total = diskmap.Select(c => (byte) c - 48).Sum();
        var map = new int[total];
        var index = 0;
        var id = 0;

        for (int i = 0; i < diskmap.Length; i++)
        {
            for (int j = 0; j < (byte) diskmap[i] - 48; j++)
            {
                map[index++] = i % 2 == 0 ? id : FREE_SPACE;
            }
            if (i % 2 == 0)
            {
                id++;
            }
        }

        return map;
    }
}