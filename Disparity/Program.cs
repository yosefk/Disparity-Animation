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
                
                BeierNeelyMorph.test_morph1("E:\\sw\\Disparity\\test\\f.bmp",
                    "E:\\sw\\Disparity\\test\\f-dst.bmp",
                    "E:\\sw\\Disparity\\test\\f-src.json",
                    "E:\\sw\\Disparity\\test\\f-dst.json");
                BeierNeelyMorph.test_morph1("E:\\sw\\Disparity\\test\\donald.jpg",
    "E:\\sw\\Disparity\\test\\donald-dst.bmp",
    "E:\\sw\\Disparity\\test\\d-src.json",
    "E:\\sw\\Disparity\\test\\d-dst.json");

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
