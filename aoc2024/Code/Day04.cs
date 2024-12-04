
namespace aoc2024.Code;

internal class Day04 : BaseDay
{
    protected override object Part1()
    {
        var map = ReadAllLines(true).Select(x => x.ToCharArray()).ToArray();
        var h = map.GetLength(0);
        var w = map[0].Length;

        var sum = 0;
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                sum += Count(map, i, j, w, h);
            }
        }

        return sum;
    }

    static int Count(char[][] map, int i, int j, int w, int h)
    {
        var cnt = 0;
        for (var wayY = -1; wayY < 2; wayY++)
        {
            for (var wayX = -1; wayX < 2; wayX++)
            {
                if (wayX == 0 && wayY == 0)
                {
                    continue;
                }

                var word = GetWord(map, i, j, w, h, wayX, wayY);

                if (word == "XMAS")
                {
                    cnt++;
                }
            }
        }
        return cnt;
    }

    static string GetWord(char[][] map, int i, int j, int w, int h, int wayX, int wayY)
    {
        var sb = new StringBuilder();
        for (int x = 0; x < 4; x++)
        {
            sb.Append(map[j][i]);
            j += wayY;
            i += wayX;

            if (j >= h || i >= w || j < 0 || i < 0)
            {
                break;
            }

        }
        return sb.ToString();
    }

    protected override object Part2()
    {
        var map = ReadAllLines(true).Select(x => x.ToCharArray()).ToArray();
        var h = map.GetLength(0);
        var w = map[0].Length;

        var sum = 0;
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                var ok = Get3x3(map, i, j, w, h, out var xmas);
                if (!ok)
                {
                    continue;
                }

                var f1 = (xmas[0, 0] == 'M' && xmas[2, 2] == 'S') || (xmas[0, 0] == 'S' && xmas[2, 2] == 'M');
                var f2 = (xmas[0, 2] == 'S' && xmas[2, 0] == 'M') || (xmas[0, 2] == 'M' && xmas[2, 0] == 'S');
                var f3 = xmas[1, 1] == 'A';

                if (f1 && f2 && f3)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    static bool Get3x3(char[][] map, int i, int j, int w, int h, out char[,] matrix)
    {
        matrix = new char[3, 3];

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (j + y >= h || i + x >= w)
                {
                    return false;
                }

                matrix[y, x] = map[j + y][i + x];
            }
        }
        return true;
    }
}