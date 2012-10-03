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
    //a matching form has a sister matching form.
    //matching is done by ID.
    //at each side, features without a match are highlighted.
    //in theory, we could match in the following ways:
    //1. mark something and then say, "here's its match at the other side"
    //2. select something so that the next thing marked at the other side is matched to it
    //3. select something, then select the match at the other side (marking doesn't involve matching)
    //[2] seems the easiest in terms of ID management: you know what ID to give the newly marked feature.
    //[2] also makes it easy to prevent matching already matched stuff - don't let the user select it
    //    in the first place.
    //so [2] seems easy to program.
    //[3] is an annoying workflow: matching independently marked stuff is tricky (like buttoning a shirt -
    //    if you start going wrong, you get a long string of errors to undo).
    //[1] may leave you with a marked feature you forgot about ([2] can only leave you with a
    //    /selection/ you forgot about, which is not an editing operation.)
    //[2] seems the nicest; the trouble with [2] is that you can't ever match two features
    //marked independently in the past - if that ever matters (which it shouldn't since independent
    //marking isn't the brightest idea since not everything has a natural match), we can easily add
    //[3] (a match operation that allows to match two features which are both unmatched).
    //
    //so, [2], then possibly [3].
    //
    //Q: what to do with IDs/accidental matches by ID? just "always go forward"? is that sufficient?
    //GUIDs??
    public class MatchingForm : MarkingForm
    {
        public MatchingForm sister;

        bool matched(IntLineFeature feature)
        {
            IntLineFeature lf;
            return sister.features.find(feature.id, out lf);
        }

        protected override void DrawFeatures(PaintEventArgs e)
        {
            Rectangle ROI = pictureBox.ROI;
            int zoom = pictureBox.zmLevel;
            foreach (var lf in features.intlines)
            {
                System.Drawing.Pen pen = matched(lf) ? System.Drawing.Pens.GreenYellow : System.Drawing.Pens.Red;
                Features.DrawLine(lf.line, e.Graphics, pen, -ROI.Left * zoom, -ROI.Top * zoom, zoom);
                
                //g.DrawLine(pen, left + lf.line.p.X * zoom, top + lf.line.p.Y * zoom, left + lf.line.q.X * zoom, top + lf.line.q.Y * zoom);
            }
        }
    }
}
