using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ryu_s;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var reverse = new Reverse(4, 4);
            var current = Reverse.Stone.Black;
            do
            {
                Show(reverse.Board);
                Console.WriteLine($"current={current}");

                int x;
                do
                {
                    Console.WriteLine("x:");
                    var xStr = Console.ReadLine();
                    if (int.TryParse(xStr, out x))
                        break;
                } while (true);
                int y;
                do
                {
                    Console.WriteLine("y:");
                    var yStr = Console.ReadLine();
                    if (int.TryParse(yStr, out y))
                        break;
                } while (true);
                var putPos = new Reverse.Pos { X = x, Y = y };
                if (!Reverse.Canput(reverse.Board, putPos, current))
                {
                    Console.WriteLine("置けない！");
                    continue;
                }
                reverse.ReverseStone(putPos, current);
                if (Reverse.IsFinished(reverse.Board))
                    break;
                current = Reverse.ReverseStone(current);
            } while (true);
            return;
        }
        static void Show(Reverse.Stone[,] board)
        {
            var height = board.GetLength(0);
            var width = board.Length / height;
            for (int i = 0; i < height; i++)
            {
                Console.Write('|');
                for (int j = 0; j < width; j++)
                {
                    var stone = board[i, j];
                    char c;
                    if (stone == Reverse.Stone.Black)
                        c = '●';
                    else if (stone == Reverse.Stone.White)
                        c = '○';
                    else
                        c = ' ';
                    Console.Write(c);
                }
                Console.WriteLine('|');
            }
        }
    }
}
