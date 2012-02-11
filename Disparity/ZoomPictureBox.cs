//based on http://blogs.msdn.com/b/calvin_hsia/archive/2008/08/21/8885657.aspx
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Controls;

public class ZoomPictureBox : AForge.Controls.PictureBox
{
    public int zmLevel = 1;
    public Rectangle ROI;
    private Point zmPt;

    public ZoomPictureBox()
    {
        this.MouseWheel += new MouseEventHandler(ZoomPictureBox_MouseWheel);
        this.Paint += ZoomPictureBox_Paint;
    }

    void ZoomPictureBox_MouseWheel(object sender, MouseEventArgs e)
    {
        if (this.zmLevel == 1)
        {
            this.zmPt = new Point(e.X, e.Y);
        }

        if (e.Delta > 0)
        {
            if (zmLevel < 20)
            {
                zmLevel += 1;
            }
        }
        else
        {
            if (e.Delta < 1)
            {
                if (zmLevel > 1)
                {
                    zmLevel -= 1;
                }
            }
        }

        this.Invalidate();
    }

    new public Image Image // overrides
    {
        get
        {
            return base.Image;
        }
        set
        {
            zmLevel = 1;
            base.Image = value;
        }
    }

    //protected override void OnPaint(PaintEventArgs pe)
    private void ZoomPictureBox_Paint(object sender, PaintEventArgs pe)
    {
        //base.OnPaint(pe);
        if (this.Image != null)
        {
            Point loc;
            Size sz;
            if (zmLevel != 1)
            {
                sz = new Size(this.Image.Width / zmLevel, this.Image.Height / zmLevel);
                // center on zmPt. Casts are needed so integer divide doesn't occur (intermediate double result)
                loc = new Point((int)(this.Image.Width * (zmPt.X / (double)this.ClientRectangle.Width)) - sz.Width / 2,
                                 (int)(this.Image.Height * (zmPt.Y / (double)this.ClientRectangle.Height)) - sz.Height / 2);
            }
            else
            {
                loc = new Point(0, 0);
                sz = this.Image.Size;
            }

            Rectangle rectSrc = new Rectangle(loc, sz);
            ROI = rectSrc;
            // now draw the rect of the source picture in the entire client rect of MyPictureBox
            pe.Graphics.DrawImage(this.Image, this.ClientRectangle, rectSrc, GraphicsUnit.Pixel);
        }
        //Paint(this, pe);
    }
}

