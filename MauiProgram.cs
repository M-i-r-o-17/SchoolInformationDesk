using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using System.Xml.Linq;

#if WINDOWS
using System.Diagnostics;
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

namespace SchoolInformationDesk
{
    public static class MauiProgram
    {
        // ============== Settings App ==============
        public static string[] allowWebSite =
        {
            "blgsosh16.obramur.ru",
        };

        public static string defaultRedirect = "blgsosh16.obramur.ru";

        public static string sheduleUrl = "192.168.10.1";

        public static bool offExploer = false;

        public static bool powerOff = false;

        public static string powerOffHour = "16";
        public static string powerOffMinut = "00";

        // ============== Servise ==============
        public static string curretUrl = "";

        public static string[] users =
        {
            "56789215", //root
            "98425647", //user
            "92749823", //power off
        };
#if WINDOWS
        public static bool exploer => Process.GetProcessesByName("explorer") != null;
        
        
        public static void clouseExploer()
        {
            if (!offExploer || !exploer) return;
        }

        public static void clouseEdge()
        {
            KillProcess("msedgewebview2.exe");
        }

        private async static void KillProcess(string name)
        {
            do
            {
                Process[] processInfo = Process.GetProcessesByName(name);
                if (processInfo != null)
                {
                    try
                    {
                        foreach (Process process in processInfo)
                            process.Kill();
                    }
                    catch (Exception) { }
                }

                await Task.Delay(1000);
            }
            while (true);
        }
#endif

        public static bool allowVisit(string url)
        {
            return allowWebSite.Contains(url) || url == sheduleUrl;
        }

        public static int userAcces(string password)
        {
            for (int i = 0; i < users.Length; i++)
                if (users[i] == password) return i;

            return -1;
        }
        public static void ShootDown()
        {
#if WINDOWS
                    Process.Start($"shutdown", "/s /t 5");
#endif
            Application.Current?.Quit();
        }

        private async static void PowerOff()
        {
            if (powerOff == false) return;

            while (powerOff == true)
            {
                if (DateTime.Now.ToString("HH:mm") == powerOffHour + ":" + powerOffMinut)
                {
                    powerOff = true;
                    ShootDown();
                }

                await Task.Delay(1000);
            }
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); });
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
            events.AddWindows(w =>
            {
                w.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = true; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                    var _appWindow = AppWindow.GetFromWindowId(myWndId);
                    _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                });
            });
#endif
            });

            Task.Run(PowerOff);
            return builder.Build();
        }
    }
}
