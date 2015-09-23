using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ryu_s
{
    public class Reverse
    {
        private Stone[,] board;
        public Stone[,] Board { get { return board; } }
        public Reverse(int width, int height)
        {
            board = new Stone[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Board[i, j] = Stone.None;
                }
            }
            Board[height / 2 - 1, width / 2 - 1] = Stone.White;
            Board[height / 2 - 1, width / 2] = Stone.Black;
            Board[height / 2, width / 2 - 1] = Stone.Black;
            Board[height / 2, width / 2] = Stone.White;

        }
        public enum Stone
        {
            None,
            Black,
            White,
        }
        public class Pos
        {
            public int X;
            public int Y;
        }

        public void ReverseStone(Pos putPos, Stone myStone)
        {
            Debug.WriteLine("Reverse(Pos, Stone)");
            //まず裏返す
            foreach (var pos in ReverseStone(Board, putPos, myStone))
            {
                Board[pos.Y, pos.X] = myStone;
            }
            //新しいのを置く  
            Board[putPos.Y, putPos.X] = myStone;
        }

        public static long count = 0;
        /// <summary>
        /// 置く場所が一つでもあるか
        /// </summary>
        /// <param name="board"></param>
        /// <param name="myStone"></param>
        /// <returns></returns>
        public static bool CanPut(Stone[,] board, Stone myStone)
        {
            //まだ何も置いていない場所に自分の石が置けるか調べる
            var noneTupples = GetStones(board);
            bool b = false;
            foreach (var noneTupple in noneTupples)
            {
                b |= Canput(board, noneTupple.Item2, myStone);
            }
            return b;
        }
        public static bool IsFinished(Stone[,] board)
        {
            return !CanPut(board, Stone.Black) && !CanPut(board, Stone.White);
        }
        /// <summary>
        /// ここに石が置けるか
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="putPos"></param>
        /// <returns></returns>
        public static bool Canput(Stone[,] board, Pos putPos, Stone myStone)
        {
            return ReverseStone(board, putPos, myStone).Count() > 0;
        }
        /// <summary>
        /// ここに石を置いた場合どれがひっくり返るか
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="putPos"></param>
        /// <returns></returns>
        public static IEnumerable<Pos> ReverseStone(Stone[,] board, Pos putPos, Stone myStone)
        {
            var list = new List<Pos>();
            if (board[putPos.Y, putPos.X] != Stone.None)
                return list;
            //上
            list.AddRange(ReverseLine(board, putPos, new Pos { X = 0, Y = -1 }, myStone));
            //右上
            list.AddRange(ReverseLine(board, putPos, new Pos { X = 1, Y = -1 }, myStone));
            //右
            list.AddRange(ReverseLine(board, putPos, new Pos { X = 1, Y = 0 }, myStone));
            //右下
            list.AddRange(ReverseLine(board, putPos, new Pos { X = 1, Y = 1 }, myStone));
            //下
            list.AddRange(ReverseLine(board, putPos, new Pos { X = 0, Y = 1 }, myStone));
            //左下
            list.AddRange(ReverseLine(board, putPos, new Pos { X = -1, Y = 1 }, myStone));
            //左
            list.AddRange(ReverseLine(board, putPos, new Pos { X = -1, Y = 0 }, myStone));
            //左上
            list.AddRange(ReverseLine(board, putPos, new Pos { X = -1, Y = -1 }, myStone));
            return list;
        }
        public static IEnumerable<Pos> ReverseLine(Stone[,] board, Pos putPos, Pos houkou, Stone myStone)
        {
            var line = GetLineStr(board, putPos, houkou);
            var list = new List<Pos>();
            if (CanPutLine(line, myStone))
            {
                foreach (var s in GetLine(board, putPos, houkou))
                {
                    if (s.Item1 == Stone.None || s.Item1 == myStone)
                        break;
                    list.Add(s.Item2);
                }
            }
            return list;
        }
        /// <summary>
        /// 石を置いた場所からのあるベクトルの状態を文字列で表す
        /// </summary>
        /// <param name="board"></param>
        /// <param name="putPos"></param>
        /// <param name="houkou">例えば右ならx=1,Y=0 左上ならx=-1, y=-1</param>
        /// <returns></returns>
        public static string GetLineStr(Stone[,] board, Pos putPos, Pos houkou)
        {
            return new string(GetLine(board, putPos, houkou).Select(tupple => CharOfStone(tupple.Item1)).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<Stone, Pos>> GetStones(Stone[,] board)
        {
            if (board.Length <= 0)
                yield break;
            var height = board.GetLength(0);
            var width = board.Length / height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!(0 <= x && x < width && 0 <= y && y < height))
                        yield break;
                    var stone = board[y, x];
                    yield return new Tuple<Stone, Pos>(stone, new Pos { X = x, Y = y });
                }
            }
        }
        public static IEnumerable<Tuple<Stone, Pos>> GetLine(Stone[,] board, Pos putPos, Pos houkou)
        {
            Debug.WriteLine("GetLine(Stone[,], Pos, Pos)");
            if (board.Length <= 0)
                yield break;
            var height = board.GetLength(0);
            var width = board.Length / height;

            int x = putPos.X;
            int y = putPos.Y;
            do
            {
                x += houkou.X;
                y += houkou.Y;
                if (!(0 <= x && x < width && 0 <= y && y < height))
                    break;
                var stone = board[y, x];
                yield return new Tuple<Stone, Pos>(stone, new Pos { X = x, Y = y });
            } while (true);
        }
        public static char CharOfStone(Stone stone)
        {
            Debug.WriteLine($"{count++} CharOfStone(Stone)");
            if (stone == Stone.Black)
                return 'b';
            else if (stone == Stone.White)
                return 'w';
            else
                return 'n';
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stone"></param>
        /// <returns></returns>
        public static Stone ReverseStone(Stone stone)
        {
            if (stone == Stone.None)
                return Stone.None;
            return stone == Stone.Black ? Stone.White : Stone.Black;
        }
        /// <summary>
        /// 石が置けるか
        /// </summary>
        /// <param name="line"></param>
        /// <param name="myStone"></param>
        /// <returns></returns>
        /// <remarks>例えば自分がbだったとしてlineが"wwwnbb"だったら置ける。"bw"だったら置けない。</remarks>
        public static bool CanPutLine(string line, Stone myStone)
        {
            Debug.WriteLine($"CanPutLine(string, Stone)");
            var my = CharOfStone(myStone);
            var you = CharOfStone(ReverseStone(myStone));
            var none = CharOfStone(Stone.None);
            //1個以上相手の石が続いてから自分の石が来れば置ける
            var pattern = "^" + you + "+" + my + ".*";
            return System.Text.RegularExpressions.Regex.IsMatch(line, pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}
