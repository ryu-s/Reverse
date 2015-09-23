using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ryu_s;
namespace WindowsFormsApplication1
{
    public partial class UserControl1 : UserControl
    {
        Reverse reverse;
        Button[,] myBoard;
        Dictionary<Button, Reverse.Pos> dict = new Dictionary<Button, Reverse.Pos>();
        public UserControl1()
        {
            InitializeComponent();
        }
        public void Init(int width, int height)
        {
            reverse = new Reverse(width, height);
            myBoard = new Button[height, width];

            var baseX = 0;
            var baseY = 100;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var button = new Button();
                    button.Width = 20;
                    button.Height = 20;
                    button.Location = new Point(baseX + button.Width * j, baseY + button.Height * i);
                    button.Click += Button_Click;
                    this.Controls.Add(button);
                    myBoard[i, j] = button;
                    dict.Add(button, new Reverse.Pos { X = j, Y = i });
                }
            }
            ApplyBoard();
        }
        Reverse.Stone current = Reverse.Stone.Black;
        private void Button_Click(object sender, EventArgs e)
        {
            var pos = dict[(Button)sender];
            if (Reverse.Canput(reverse.Board, pos, current))
            {
                reverse.ReverseStone(pos, current);
                current = Reverse.ReverseStone(current);
                ApplyBoard();
            }
            if (!Reverse.CanPut(reverse.Board, current))
            {
                //一つも置く場所が無い場合は相手のターンに移行
                current = Reverse.ReverseStone(current);
                ApplyBoard();
            }
        }
        private void ApplyBoard()
        {
            var height = reverse.Board.GetLength(0);
            var width = reverse.Board.Length / height;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var stone = reverse.Board[i, j];
                    var button = myBoard[i, j];
                    if (stone == Reverse.Stone.Black)
                        button.Text = "●";
                    else if (stone == Reverse.Stone.White)
                        button.Text = "○";
                    else
                        button.Text = "";
                }
            }
            label1.Text = current.ToString();
        }
    }
}
