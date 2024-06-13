using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public partial class FormMenu : Form
    {
        SoundPlayer simpleSound;
        public FormMenu()
        {
            InitializeComponent();
            simpleSound = new SoundPlayer(Properties.Resources.menu);
            simpleSound.Play();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            FormGame fGame  = new FormGame();
            this.Hide();
            fGame.Show();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMenu_Activated(object sender, EventArgs e)
        {
            simpleSound.Play();
        }

        private void btnTutorial_Click(object sender, EventArgs e)
        {
            FormTutorial fTutorial= new FormTutorial();
            this.Hide();
            fTutorial.Show();
        }
    }
}
