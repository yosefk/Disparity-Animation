using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonFx.Json;
using AForge;

namespace Disparity
{
    struct IntLineFeature
    {
        public int id;
        public IntLine line;
    };

    class Features
    {
        string imagefile; //the file name of the marked image 
        public IntLineFeature[] intlines;
        int nextid;

        public override string ToString()
        {
            dynamic[] lines = new dynamic[intlines.Length];
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
                f.intlines = new IntLineFeature[contents.intlines.Length];
                int i = 0;
                foreach (dynamic l in contents.intlines)
                {
                    IntLineFeature lf = new IntLineFeature();
                    lf.id = l.id;
                    int[] line = l.line;
                    lf.line.p.X = l.line[0];
                    lf.line.p.Y = line[1];
                    lf.line.q.X = line[2];
                    lf.line.q.Y = line[3];
                    f.intlines[i] = lf;
                    i++;
                }
            }
            //catch { } //FIXME: catch the right thing
            return f;
        }

        public static void test_json()
        {
            Features f = new Features();
            f.imagefile = "somefile.png";
            f.intlines = new IntLineFeature[3];
            for (int i = 0; i < 3; ++i)
            {
                var l = new IntLineFeature();
                l.id = i;
                l.line.p.X = i;
                l.line.p.Y = i * 2;
                l.line.q.X = i + 10;
                l.line.q.Y = i * 2 + 10;
                f.intlines[i] = l;
            }
            string json = f.ToString();
            Console.WriteLine(json);

            Features g = Features.FromString(json);
            Console.WriteLine(string.Format("file: {0} line[1].p.X: {1} line[2].q.Y: {2}", g.imagefile, g.intlines[1].line.p.X, g.intlines[2].line.q.Y));

            bool same = json == g.ToString();
            Console.WriteLine(string.Format("same json: {0}", same));
        }
    }
}
