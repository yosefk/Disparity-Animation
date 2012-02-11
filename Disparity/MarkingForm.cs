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
        List<IntLineFeature> redostack; //not cleared by addition of new features on purpose
        //(feature addition is context-free so there's no real need to invalidate operations
        //once the editing "went down an alternate path" the way it happens with text editing)

        public MarkingForm()
        {
            InitializeComponent();
            redostack = new List<IntLineFeature>();
        }

        private void MarkingForm_Load(object sender, EventArgs e)
        {
            pictureBox.Image = image;
            pictureBox.Size = new Size(pictureBox.Image.Width, pictureBox.Image.Height);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ActiveControl = pictureBox;
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
            Rectangle ROI = pictureBox.ROI;
            int zoom = pictureBox.zmLevel;
            features.Draw(e.Graphics, System.Drawing.Pens.GreenYellow, -ROI.Left * zoom, -ROI.Top * zoom, zoom);
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
                    int zoom = pictureBox.zmLevel;
                    Rectangle ROI = pictureBox.ROI;
                    lf.id = features.newId();
                    lf.line.p.X = (newlinestart.X + ROI.Left * zoom) / zoom;
                    lf.line.p.Y = (newlinestart.Y + ROI.Top * zoom) / zoom;
                    lf.line.q.X = (newlineend.X + ROI.Left * zoom) / zoom;
                    lf.line.q.Y = (newlineend.Y + ROI.Top * zoom) / zoom;
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String json = features.ToString();
            System.IO.File.WriteAllText(openFileDialog1.FileName, json);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String json = features.ToString();
                System.IO.File.WriteAllText(saveFileDialog1.FileName, json);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String json = System.IO.File.ReadAllText(openFileDialog1.FileName);
                features = Features.FromString(json);
                pictureBox.Refresh();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (features.intlines.Count() > 0)
            {
                int last = features.intlines.Count() - 1;
                IntLineFeature undone = features.intlines[last];
                features.intlines.RemoveAt(last);

                redostack.Add(undone);
                pictureBox.Refresh();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redostack.Count() > 0)
            {
                int last = redostack.Count() - 1;
                IntLineFeature redone = redostack[last];
                redostack.RemoveAt(last);

                features.intlines.Add(redone);
                pictureBox.Refresh();
            }
        }
    }
}
