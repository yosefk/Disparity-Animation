﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Imaging.Formats;

namespace Disparity
{
    public partial class DisparityForm : Form
    {
        public DisparityForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImageInfo imageInfo = null;
                    pictureBox.Image = ImageDecoder.DecodeFromFile(openFileDialog1.FileName, out imageInfo);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Size = new Size(imageInfo.Width, imageInfo.Height);
                    ActiveControl = pictureBox;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed loading the image", ex.Message,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DisparityForm_Resize(object sender, EventArgs e)
        {
            pictureBox.Size = new Size(pictureBox.Image.Width, pictureBox.Image.Height);
        }

        private void markImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkingForm mf = new MarkingForm();
            mf.image = pictureBox.Image;
            mf.Show();
        }

        private void matchfeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                MatchingForm mf2 = new MatchingForm();
                try
                {
                    ImageInfo imageInfo = null;
                    mf2.image = ImageDecoder.DecodeFromFile(openFileDialog2.FileName, out imageInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed loading the image", ex.Message,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MatchingForm mf = new MatchingForm();
                mf.image = pictureBox.Image;

                mf.sister = mf2;
                mf2.sister = mf;

                mf.Show();
                mf2.Show();
            }

        }
    }
}
