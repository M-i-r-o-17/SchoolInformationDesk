using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Newtonsoft.Json;
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
        public static List<string> allowWebSite;

        public static string defaultRedirect;

        public static string sheduleUrl;

        public static bool offExploer;

        public static bool powerOff;

        public static string powerOffHour;
        public static string powerOffMinut;

        // ============== Servise ==============
        public static string curretUrl = "";
        public static int curretUser = -1;
        public static string[] users =
        {
            "56789215", //root
        };

        public static bool exploer => Process.GetProcessesByName("explorer") != null;
        
        public static void ShootDown()
        {
            Process.Start($"shutdown", "/s /t 5");
            Application.Current?.Quit();
        }

        public static void Hybernation()
        {
            Process.Start($"shutdown", "/h");
        }
        
        private static async void clouseExploer()
        {
            while(offExploer)
            {
                if (!exploer) return;
                KillProcess("explorer");
                await Task.Delay(1000);
            }
            
        }

        private static async void clouseEdge()
        {
            while(true)
            {
                KillProcess("msedgewebview2.exe");
                await Task.Delay(1000);
            }
            
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
                else break;

                await Task.Delay(1000);
            }
            while (true);

        }

        private async static void PowerOff()
        {
            
            while (powerOff)
            {

                if (DateTime.Now.ToString("HH:mm") == powerOffHour + ":" + powerOffMinut)
                {
                    powerOff = true;
                    Hybernation();
                }

                await Task.Delay(1000);
            }
        }

        public static void ReloadService()
        {
            Task.Run(clouseEdge);
            Task.Run(clouseExploer);
            Task.Run(PowerOff);
        }
        public static SettingsModel LoadSettings(string path = "./settings.json")
        {
            SettingsModel model = new SettingsModel();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<SettingsModel>(json);

                model = new SettingsModel(data);
            }

            return model;
        }
        public static void SaveSettings(SettingsModel settings, string path = "./settings.json")
        {
            var json = JsonConvert.SerializeObject(settings);

            File.WriteAllText(path, json);
        }
        public static void UpdateSettings(SettingsModel settings = null)
        {
            if ( settings == null)  settings = new SettingsModel();

            allowWebSite = settings.allowURL;
            sheduleUrl = settings.sheduleURL;

            powerOff = settings.autoPowerOff;
            offExploer = settings.killExploer;

            powerOffHour = settings.hour;
            powerOffMinut = settings.minute;

            users = settings.users.ToArray();

            defaultRedirect = allowWebSite[0];

            if (settings.autoLoad)
            {
                if(!File.Exists("%userprofile%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"))
                {
                    File.Copy("./dsadsa.asd", "%userprofile%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\");
                }
            }
            else
            {
                if (File.Exists("%userprofile%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"))
                {
                    File.Delete("%userprofile%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\");
                }
            }

        }
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
        public static MauiApp CreateMauiApp()
        {
            UpdateSettings(LoadSettings());

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
            ReloadService();
#endif
            });

            

            return builder.Build();
        }
    }
}
