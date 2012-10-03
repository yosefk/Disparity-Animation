using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using JsonFx.Json;
using AForge;

namespace Disparity
{
    public struct IntLineFeature
    {
        public int id;
        public IntLine line;
    };

    public class Features
    {
        public string imagefile; //the file name of the marked image 
        public List<IntLineFeature> intlines;

        public Features()
        {
            intlines = new List<IntLineFeature>();
        }

        public int newId()
        {
            int id = 0;
            foreach (var lf in intlines)
            {
                id = Math.Max(id, lf.id);
            }
            return id + 1;
        }

        public IntLine[] barelines()
        {
            IntLine[] bare = new IntLine[intlines.Count];
            for (int i = 0; i < intlines.Count; ++i)
            {
                bare[i] = intlines[i].line;
            }
            return bare;
        }

        public Line[] floatlines()
        {
            Line[] bare = new Line[intlines.Count];
            for (int i = 0; i < intlines.Count; ++i)
            {
                bare[i] = intlines[i].line.floatline();
            }
            return bare;
        }

        public bool find(int id, out IntLineFeature found)
        {
            foreach (var lf in intlines)
            {
                if (lf.id == id)
                {
                    found = lf;
                    return true;
                }
            }
            found = new IntLineFeature();
            return false;
        }

        public override string ToString()
        {
            dynamic[] lines = new dynamic[intlines.Count];
            int i = 0;
            foreach (var lf in intlines)
            {
                lines[i] = new {
                    id = lf.id, line = new int[]
                    {
                        lf.line.p.X, lf.line.p.Y,
                        lf.line.q.X, lf.line.q.Y
                    }
                };
                i++;
            }
            var settings = new JsonFx.Serialization.DataWriterSettings();
            settings.PrettyPrint = true;
            var writer = new JsonWriter(settings);
            var output = new { imagefile = imagefile, intlines = lines };
            return writer.Write(output);
        }

        public static Features FromString(string json)
        {
            var reader = new JsonReader();
            dynamic contents = reader.Read(json);
            Features f = new Features();
            f.imagefile = contents.imagefile;
            //try
            {
                f.intlines = new List<IntLineFeature>();
                foreach (dynamic l in contents.intlines)
                {
                    IntLineFeature lf = new IntLineFeature();
                    lf.id = l.id;
                    int[] line = l.line;
                    lf.line.p.X = l.line[0];
                    lf.line.p.Y = line[1];
                    lf.line.q.X = line[2];
                    lf.line.q.Y = line[3];
                    f.intlines.Add(lf);
                }
            }
            //catch { } //FIXME: catch the right thing
            return f;
        }

        public static void test_json()
        {
            Features f = new Features();
            f.imagefile = "somefile.png";
            f.intlines = new List<IntLineFeature>();
            for (int i = 0; i < 3; ++i)
            {
                var l = new IntLineFeature();
                l.id = i;
                l.line.p.X = i;
                l.line.p.Y = i * 2;
                l.line.q.X = i + 10;
                l.line.q.Y = i * 2 + 10;
                f.intlines.Add(l);
            }
            string json = f.ToString();
            Console.WriteLine(json);

            Features g = Features.FromString(json);
            Console.WriteLine(string.Format("file: {0} line[1].p.X: {1} line[2].q.Y: {2}", g.imagefile, g.intlines[1].line.p.X, g.intlines[2].line.q.Y));

            bool same = json == g.ToString();
            Console.WriteLine(string.Format("same json: {0}", same));
            System.IO.File.WriteAllText("test.features.json", json);

        }

        public static void DrawLine(IntLine line, Graphics g, System.Drawing.Pen pen, int left = 0, int top = 0, int zoom = 1)
        {
            g.DrawLine(pen, left + line.p.X * zoom, top + line.p.Y * zoom, left + line.q.X * zoom, top + line.q.Y * zoom);
        }

        public void Draw(Graphics g, System.Drawing.Pen pen, int left=0, int top=0, int zoom=1)
        {
            foreach (var lf in intlines)
            {
                DrawLine(lf.line, g, pen, left, top, zoom);
                //g.DrawLine(pen, left + lf.line.p.X * zoom, top + lf.line.p.Y * zoom, left + lf.line.q.X * zoom, top + lf.line.q.Y * zoom);
            }
        }
    }
}
