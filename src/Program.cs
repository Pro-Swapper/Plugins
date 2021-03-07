using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProSwapperPluginsOld
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            MessageBox.Show("Select a Pro Swapper Plugin file (*.pro) and you will be able to swap an item from the file!", "Pro Swapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            using (OpenFileDialog plugin = new OpenFileDialog())
            {
                plugin.Filter = "Pro Swapper Plugins (*.pro)|*.pro";
                plugin.Title = "Select a Pro Swapper Plugin";
                plugin.ShowDialog();
                if (!plugin.FileName.EndsWith(".pro"))
                {
                    return;
                }

                new Plugins(plugin.FileName).ShowDialog();
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Plugins(plugin.FileName));
            }

            
        }
    }
}
