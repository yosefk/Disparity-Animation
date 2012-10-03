using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Disparity
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Features.test_json();

            try
            {
                /*
                BeierNeelyMorph.test_morph1("E:\\sw\\Disparity\\test\\f.bmp",
                    "E:\\sw\\Disparity\\test\\f-dst.bmp",
                    "E:\\sw\\Disparity\\test\\f-src.json",
                    "E:\\sw\\Disparity\\test\\f-dst.json");*/
                /*
                BeierNeelyMorph.test_morph1("E:\\sw\\Disparity\\test\\donald.jpg",
    "E:\\sw\\Disparity\\test\\donald-dst.bmp",
    "E:\\sw\\Disparity\\test\\dm-src.json",
    "E:\\sw\\Disparity\\test\\dm-dst.json");*/
                 
                /*
                BeierNeelyMorph.test_morph1("E:\\pprod\\cel-by\\cel-by8-r.jpg",
    "E:\\pprod\\cel-by\\cel-by8m-dst.bmp",
    "E:\\pprod\\cel-by\\8m-src.json",
    "E:\\pprod\\cel-by\\8m-dst.json");
                 */
                BeierNeelyMorph.test_morph1_movie("E:\\pprod\\cel-by\\cel-by8-r.jpg",
    "E:\\sw\\Disparity\\ffmpeg-test\\m{0:000}.png",
    "E:\\pprod\\cel-by\\8m-src.json",
    "E:\\pprod\\cel-by\\8m-dst.json",
    100);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DisparityForm());
        }
    }
}
