using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{

    public partial class FormGame : Form
    {
        SoundPlayer simpleSound;

        int tabuleiroSideLeft = -1;
        int tabuleiroSideRight = -1;
        int vez;

        List<Domino> dominoes = new List<Domino>();
        List<Domino> tabuleiro = new List<Domino>();
        List<Domino> jogadas = new List<Domino>();
        List<Domino> playerMao1 = new List<Domino>();
        List<Domino> playerMao2 = new List<Domino>();

        Random rd = new Random();

        public FormGame()
        {
            InitializeComponent();
           
            try
            {
                simpleSound = new SoundPlayer(Properties.Resources.jogando);
                simpleSound.Play();

                // Cria todas as peças do dominó
                var countPecas = 0;
                for (int i = 0; i <= 6; i++)
                {
                    for (int j = i; j <= 6; j++)
                    {
                        dominoes.Add(new Domino(countPecas, i, j, (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_" + i.ToString() + "_" + j.ToString())));
                        tabuleiro.Add(new Domino(countPecas, i, j, (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_" + i.ToString() + "_" + j.ToString())));
                        countPecas++;
                    }
                }

                embaralhar();
                distribuir(playerMao1, maoPlayer1, true);
                distribuir(playerMao2, maoPlayer2, true);

                // Imprime as peças do dominó
                foreach (Domino domino in tabuleiro)
                {
                    flowLayoutPanel2.Controls.Add(domino.Image);
                }
                maisPontos();
            }
            finally
            {
                // Variavel utilizada para embaralhar peças.
                tabuleiro = null;
            }

        }

        private void embaralhar()
        {
            // Embaralha as peças do dominó            
            for (int i = 0; i < tabuleiro.Count; i++)
            {
                int j = rd.Next(i, tabuleiro.Count-1);
                Domino temp = tabuleiro[i];
                tabuleiro[i] = tabuleiro[j];
                tabuleiro[j] = temp;
            }
        }

        private void distribuir(List<Domino> mao, FlowLayoutPanel panelMao, bool showPecas)
        {
            for (int i = 0; i < 7; i++)
            {
                if (showPecas)
                {
                    tabuleiro[i].showPeca();
                    tabuleiro[i].Image.Click += new EventHandler(btnJogada);
                }
                mao.Add(tabuleiro[i]);
            }
            tabuleiro.RemoveRange(0, 7);

            foreach (Domino domino in mao)
            {
                panelMao.Controls.Add(domino.Image);
            }
        }

        private void maisPontos()
        {
            var pontosPlayerMao1 = 0;
            var pontosPlayerMao2 = 0;
            foreach (var domino in playerMao1)
            {
                pontosPlayerMao1 += domino.Side1 + domino.Side2;                
            }

            foreach (var domino in playerMao2)
            {
                pontosPlayerMao2 += domino.Side1 + domino.Side2;
            }

            if (pontosPlayerMao1 > pontosPlayerMao2)
            {
                vez = 1;
                label3.Text = "Sua vez";
            } else
            {
                vez = 2;
                label3.Text = "Robô jogando";
            }
            maoPlayer1.Enabled = (vez == 1);
            maoPlayer2.Enabled = (vez == 2);                         
        }

        private bool passou()
        {
            List<Domino> mao;
            if (vez == 1)
                mao = playerMao1;
            else
                mao = playerMao2;

            foreach (var peca in mao)
            {
                if (peca.Side1 == tabuleiroSideLeft || peca.Side1 == tabuleiroSideRight||
                    peca.Side2 == tabuleiroSideLeft || peca.Side2 == tabuleiroSideRight)
                {
                    label3.Text = "Ops, querendo passar com peça na mão?";
                    return false;
                }
            }
            
            //vez = vez == 1 ? 2 : 1;
            
            if (vez == 1)
            {
                vez = 2;
                label3.Text = "Robô jogando";
            }
            else
            {
                vez = 1;
                label3.Text = "Sua vez";
            }

            maoPlayer1.Enabled = vez == 1;
            maoPlayer2.Enabled = vez == 2;

            return true;
        }

        private void empate()
        {
            List<Domino> mao;
            mao = playerMao1;
            bool mao1TemPeca = false;
            bool mao2TemPeca = false;

            int pontos1 = 0;
            int pontos2 = 0;

            foreach (var peca in mao)
            {
                pontos1 += peca.Side1 + peca.Side2; 
                if (peca.Side1 == tabuleiroSideLeft || peca.Side1 == tabuleiroSideRight ||
                    peca.Side2 == tabuleiroSideLeft || peca.Side2 == tabuleiroSideRight)
                {
                    mao1TemPeca = true;
                }
            }

            mao = playerMao2;
            foreach (var peca in mao)
            {
                pontos2 += peca.Side1 + peca.Side2;
                if (peca.Side1 == tabuleiroSideLeft || peca.Side1 == tabuleiroSideRight ||
                    peca.Side2 == tabuleiroSideLeft || peca.Side2 == tabuleiroSideRight)
                {
                    mao2TemPeca = true;
                }
            }
            
            
            if (!mao1TemPeca && !mao2TemPeca)
            {
                if (pontos1 <= pontos2)
                {
                    MessageBox.Show("Você ganhou!!!");
                    pontos2 += Int16.Parse(pontosPlayer1.Text);
                    pontosPlayer1.Text = pontos2.ToString();
                }

                 else
                {
                    MessageBox.Show("O robô ganhou!!!");
                    pontos1 += Int16.Parse(pontosPlayer2.Text);
                    pontosPlayer2.Text = pontos1.ToString();
                }
            }
        }


        private void btnJogada(object sender, EventArgs e)
        {
            var peca = (PictureBox)sender;
            label3.Text = peca.Tag.ToString();
            var dom = dominoes[Int16.Parse(peca.Tag.ToString())];
            dom.showPeca();

            var status = validaJogada(dom);

            if (status != "inválido")
            {
                execJogada(dom);                
            }                            
        }

        private string validaJogada(Domino dom)
        {
            var status = "";
            if (dom.Side1 == tabuleiroSideRight)
                status = "right";                
            else if (dom.Side2 == tabuleiroSideRight)
                status = "right";                
            else if (dom.Side1 == tabuleiroSideLeft)
                status = "left";                
            else if (dom.Side2 == tabuleiroSideLeft)
                status = "left";                
            else if (tabuleiroSideLeft == -1 || tabuleiroSideRight == -1)
                status = "right";
            else
            {
                status = "inválido";
                label3.Text = status;
            }
            return status;
        }

        private void execJogada(Domino dom)
        {
            int pontos1 = 0;
            int pontos2 = 0;

            if (dom.Side1 == tabuleiroSideRight)
            {
                tabuleiroSideRight = dom.Side2;
                flowLayoutPanel1.Controls.Add(dom.Image);
                if (dom.Side1 != dom.Side2)
                    dom.giraPeca(90);
            }
            else if (dom.Side2 == tabuleiroSideRight)
            {
                tabuleiroSideRight = dom.Side1;
                flowLayoutPanel1.Controls.Add(dom.Image);
                if (dom.Side1 != dom.Side2)
                    dom.giraPeca(270);
            }
            else if (dom.Side1 == tabuleiroSideLeft)
            {
                if (dom.Side1 != dom.Side2)
                    dom.giraPeca(270);
                tabuleiroSideLeft = dom.Side2;
                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    flowLayoutPanel1.Controls.Add(dom.Image);
                    flowLayoutPanel1.Controls.SetChildIndex(dom.Image, 0);
                } else
                {
                    flowLayoutPanel1.Controls.Add(dom.Image);
                }
            }
            else if (dom.Side2 == tabuleiroSideLeft)
            {
                if (dom.Side1 != dom.Side2) 
                    dom.giraPeca(90);
                tabuleiroSideLeft = dom.Side1;
                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    flowLayoutPanel1.Controls.Add(dom.Image);
                    flowLayoutPanel1.Controls.SetChildIndex(dom.Image, 0);
                }
                else
                {
                    flowLayoutPanel1.Controls.Add(dom.Image);
                }
            }
            else
            {
                if (dom.Side1 != dom.Side2)
                    dom.giraPeca(90);
                tabuleiroSideLeft = dom.Side1;
                tabuleiroSideRight = dom.Side2;
                flowLayoutPanel1.Controls.Add(dom.Image);
            }
            jogadas.Add(dom);

            
            for (int i = 0; i < playerMao1.Count; i++)
            {
                pontos1 += playerMao1[i].Side1 + playerMao1[i].Side2;
                if (vez == 1)
                {
                    if (playerMao1[i].Peca == dom.Peca)
                    {
                        playerMao1.RemoveAt(i);
                        maoPlayer1.Controls.RemoveAt(i);
                        vez = 2;
                        label3.Text = "Robô jogando";
                        maoPlayer1.Enabled = false;
                        maoPlayer2.Enabled = true;
                    }
                }
                
            }                
            
            for (int i = 0; i < playerMao2.Count; i++)
            {
                pontos2 += playerMao2[i].Side1 + playerMao2[i].Side2;
                if (vez == 2)
                {
                    if (playerMao2[i].Peca == dom.Peca)
                    {
                        playerMao2.RemoveAt(i);
                        maoPlayer2.Controls.RemoveAt(i);
                        vez = 1;
                        label3.Text = "Sua vez";
                        maoPlayer1.Enabled = true;
                        maoPlayer2.Enabled = false;
                    }
                }                
            }

            if (playerMao1.Count == 0)
            {
                MessageBox.Show("Você ganhou!!!");
                pontos2 += Int16.Parse(pontosPlayer1.Text);
                pontosPlayer1.Text = pontos2.ToString();
            }

            if (playerMao2.Count == 0)
            {
                MessageBox.Show("O robô ganhou!!!");
                pontos1 += Int16.Parse(pontosPlayer2.Text);
                pontosPlayer2.Text = pontos1.ToString();
            }
        }        

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            FormMenu fMenu = new FormMenu();
            this.Hide();
            //simpleSound.Stop();
            fMenu.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            passou();
            empate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormTutorial fTutorial = new FormTutorial();
            this.Hide();
            fTutorial.Show();
        }
    }
}
