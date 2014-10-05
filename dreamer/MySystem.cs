using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace dreamer
{
    public class MySystem
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, // handle to destination window 
        uint Msg, // message 
        int wParam, // first message parameter 
        int lParam // second message parameter 
        );

        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        public static void installFont()
        {
            try
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\CHICSA.TTF"))
                    return;
                string WinFontDir = Environment.GetEnvironmentVariable("WINDIR") + "\\fonts";
                string FontFileName = "CHICSA.TTF";
                string FontName = "ChickenScratch AOE";
                int Ret;
                int Res;
                string FontPath;
                //const int WM_FONTCHANGE = 0x001D;
                //const int HWND_BROADCAST = 0xffff;
                FontPath = WinFontDir + "\\" + FontFileName;
                if (!File.Exists(FontPath))
                {
                    File.Copy(Application.StartupPath + "\\Content\\Font\\CHICSA.TTF", FontPath);
                    Ret = AddFontResource(FontPath);

                    //Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                    //WIN7下编译这句会出错，不知道是不是系统的问题，这里应该是发送一个系统消息关系不大不影响字体安装，所以我注释掉了
                    Ret = WriteProfileString("fonts", FontName + "(TrueType)", FontFileName);
                }
            }
            catch { }
        }
    }
}
