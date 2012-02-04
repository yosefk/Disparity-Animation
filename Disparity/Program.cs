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
                
                //BeierNeelyMorph.test_morph1();
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
