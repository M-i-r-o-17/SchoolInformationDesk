using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInformationDesk
{
    public class SettingsModel
    {
        public List<string> allowURL { get; set; }
        public string sheduleURL { get; set; }
        public bool autoPowerOff { get; set; }
        public bool killExploer { get; set; }
        public bool autoLoad {  get; set; }
        public string hour { get; set; }
        public string minute { get; set; }
        public List<string> users { get; set; }

        private void settingsDefault()
        {
            allowURL = new List<string>()
            {
                "blgsosh16.obramur.ru",
                "region.obramur.ru",
                "portal.obramur.ru",
                "minobrnauki.gov.ru",
                "edu.gov.ru",
                "uchi.ru"
            };

            sheduleURL = "192.168.10.1";

            autoPowerOff = false;
            killExploer = false;
            autoLoad = false;
            hour = "16";
            minute = "00";
            users = new List<string>()
            {
                "123456789"
            };

        }

        public SettingsModel(SettingsModel model = null)
        {
            if (model == null)
            {
                settingsDefault();
                return;
            }

            allowURL = model.allowURL != null ? model.allowURL : new List<string>() { "blgsosh16.obramur.ru", "region.obramur.ru", "portal.obramur.ru", "minobrnauki.gov.ru", "edu.gov.ru", "uchi.ru" };
            sheduleURL = model.sheduleURL != null ? model.sheduleURL : "192.168.10.1";
            autoPowerOff = model.autoPowerOff;
            killExploer = model.killExploer;
            autoLoad = model.autoLoad;
            hour = model.hour != null ? model.hour : "16";
            minute = model.minute != null ? model.minute : "00";
            users = model.users != null ? model.users : new List<string>() { "123456789" };

        }
    }
}
