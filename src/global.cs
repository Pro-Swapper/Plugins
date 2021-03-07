using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProSwapperPluginsOld
{
    public class global
    {
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(2, 5);
        //Settings Writer

        public static string ProSwapperFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Pro_Swapper\";
        public static string settingspath()
        {
            string path = ProSwapperFolder + @"Config\" + version + "_config.txt";
            CreateDir(ProSwapperFolder + @"Config\");
            if (!File.Exists(path))
            {
                using (StreamWriter a = new StreamWriter(path))
                {
                    foreach (Setting foo in Enum.GetValues(typeof(Setting)))
                    {
                        a.WriteLine(foo + "=");
                    }
                }
                WriteSetting("0,33,113;64,85,170;65,105,255;255,255,255", Setting.theme);
            }
            return path;
        }

        public static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        public static void WriteSetting(string newText, Setting value)
        {
            string line;
            int counter = 1;
            string text = File.ReadAllText(settingspath());
            using (StringReader reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(value + "="))
                    {
                        lineChanger(value + "=" + newText, settingspath(), counter);
                        break;
                    }
                    counter++;
                }
            }
        }


        private static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        public static string ReadSetting(Setting value)
        {
            string line;
            using (StreamReader file = new StreamReader(settingspath()))
            {
                for (int counter = 0; (line = file.ReadLine()) != null; counter++)
                {
                    if (line.StartsWith(value + "="))
                        return line.Replace(value + "=", "");
                }
                return null;
            }
        }
        public enum Setting
        {
            Paks,
            theme,
            lastopened,
            swaplogs
        }

        public static void CloseFN()
        {
            try
            {
                foreach (Process a in Process.GetProcesses())
                {
                    string b = a.ProcessName.ToLower();
                    if (b.Contains("easyanticheat") | b.StartsWith("fortniteclient") | b.StartsWith("epicgameslauncher") | a.ProcessName.Contains("UnrealCEFSubProcess") | a.ProcessName.Equals("umodel") | a.ProcessName.Equals("FModel"))
                    {
                        if (a.ProcessName == "FortniteClient-Win64-Shipping")
                        {
                            MessageBox.Show("Closed Fortnite (Fortnite needs to be closed to use Pro Swapper)!", "Pro Swapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        a.Kill();
                    }
                }
            }
            catch
            { }
        }

        //Researcher
        public static long FindPosition(Stream stream, int searchPosition, long startIndex, byte[] searchPattern)
        {
            long searchResults = 0;
            stream.Position = startIndex;
            while (true)
            {
                if (stream.Position == stream.Length)
                    return searchResults;

                var latestbyte = stream.ReadByte();
                if (latestbyte == -1)
                    break;

                if (latestbyte == searchPattern[searchPosition])
                {
                    searchPosition++;
                    if (searchPosition == searchPattern.Length)
                        return stream.Position - searchPattern.Length;
                }
                else
                    searchPosition = 0;
            }

            return searchResults;
        }
    }
}
