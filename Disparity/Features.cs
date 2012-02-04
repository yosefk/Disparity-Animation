using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonFx.Json;
using AForge;

namespace Disparity
{
    class Features
    {
        string imagefile; //the file name of the marked image 
        public IntLine[] intlines;

        public override string ToString()
        {
            int[][][] lines = new int[intlines.Length][][];
            int i = 0;
            foreach(IntLine line in intlines)
            {
                lines[i] = new int[][]{
                    new int[]{intlines[i].p.X, intlines[i].p.Y},
                    new int[]{intlines[i].q.X, intlines[i].q.Y}
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
            try
            {
                f.intlines = new IntLine[contents.intlines.Length];
                int i = 0;
                foreach(dynamic line in contents.intlines)
                {
                    IntLine l = new IntLine();
                    l.p = new IntPoint(line[0][0], line[0][1]);
                    l.q = new IntPoint(line[1][0], line[1][1]);
                    f.intlines[i] = l;
                    i++;
                }
            }
            catch { } //FIXME: catch the right thing
            return f;
        }

        public static void test_json()
        {
            Features f = new Features();
            f.imagefile = "somefile.png";
            f.intlines = new IntLine[3];
            for (int i = 0; i < 3; ++i)
            {
                var l = new IntLine();
                l.p.X = i;
                l.p.Y = i * 2;
                l.q.X = i + 10;
                l.q.Y = i * 2 + 10;
                f.intlines[i] = l;
            }
            string json = f.ToString();
            Console.WriteLine(json);

            Features g = Features.FromString(json);
            Console.WriteLine(string.Format("file: {0} line[1].p.X: {1} line[2].q.Y: {2}", g.imagefile, g.intlines[1].p.X, g.intlines[2].q.Y));

            bool same = json == g.ToString();
            Console.WriteLine(string.Format("same json: {0}", same));
        }
    }
}
