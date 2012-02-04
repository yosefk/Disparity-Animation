using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;

namespace Disparity
{
    public partial class MarkingForm : Form
    {
        public Image image;
        public Features features;

        //editing state
        System.Drawing.Point newlinestart;
        System.Drawing.Point newlineend;
        bool inline;

        public MarkingForm()
        {
            InitializeComponent();
        }

        private void MarkingForm_Load(object sender, EventArgs e)
        {
            pictureBox.Image = image;
            pictureBox.Size = new Size(pictureBox.Image.Width, pictureBox.Image.Height);
            pictureBox.SizeMode = PictureBoxSizeMode.Normal;
            if (features == null)
            {
                features = new Features();
            }
        }

        private void MarkingForm_Resize(object sender, EventArgs e)
        {
            pictureBox.Size = new Size(pictureBox.Image.Width, pictureBox.Image.Height);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            features.Draw(e.Graphics);
            if (inline)
            {
                e.Graphics.DrawLine(System.Drawing.Pens.HotPink, newlinestart, newlineend);
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!inline)
                {
                    newlinestart = new System.Drawing.Point(e.X, e.Y);
                    newlineend = newlinestart;
                    inline = true;
                }
                else
                {
                    var lf = new IntLineFeature();
                    lf.id = features.newId();
                    lf.line.p.X = newlinestart.X;
                    lf.line.p.Y = newlinestart.Y;
                    lf.line.q.X = newlineend.X;
                    lf.line.q.Y = newlineend.Y;
                    features.intlines.Add(lf);
                    inline = false;
                }
                pictureBox.Refresh();
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (inline)
            {
                newlineend = new System.Drawing.Point(e.X, e.Y);
                pictureBox.Refresh();
            }
        }
    }
}
