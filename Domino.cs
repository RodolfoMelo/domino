using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public class Domino
    {
        public int Peca { get; set; }
        public int Side1 { get; set; }
        public int Side2 { get; set; }
        public PictureBox Image { get; set; }

        protected System.Drawing.Image ImageEnabled { get; set; }
        protected System.Drawing.Image ImageDisabled = Properties.Resources.D_VIRADA;

        public Domino(int peca, int side1, int side2, System.Drawing.Image imageLocation)
        {
            Peca = peca;
            Side1 = side1;
            Side2 = side2;
            ImageEnabled = imageLocation;

            Image = new PictureBox();
            Image.Image = ImageDisabled;
            Image.SizeMode = PictureBoxSizeMode.StretchImage;
            
            Image.Height = 100;
            Image.Width = 54;
            Image.Tag = peca;            
        }

        public void showPeca()
        {
            Image.Image = ImageEnabled;
        }

        public void giraPeca(int grau)
        {
            Image flipImage = Image.Image;
            if (grau == 90)
                flipImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            else
                flipImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
            Image.Image = flipImage;            
            Image.Height = 54;
            Image.Width = 100;                            
        }

        public override string ToString()
        {
            return $"[{Side1}|{Side2}]";
        }
    }
}
