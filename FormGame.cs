using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public partial class FormGame : Form
    {

        Random rd = new Random();
        int[,] tabuleiro = new int[9, 9];

        public FormGame()
        {
            InitializeComponent();
        }

        private void sortNumbersInRow(int row)
        {
            for (int col = 0; col < 9; col++)
            {
                tabuleiro[row, col] = rd.Next(0, 8);
            }
        }
        
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            FormMenu fMenu = new FormMenu();
            this.Hide();
            fMenu.Show();
        }
    }
}
