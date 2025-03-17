
namespace SchoolInformationDesk;

public partial class SettingsRoot : ContentPage
{
	private string[][][] keyboards =
	{
		new string[][]													//Русская клавиатура
		{
			new string[] { "ESC","1","2","3","4","5","6","7","8","9","0","BACK", },
            new string[] { "TAB","й","ц","у","к","е","н","г","ш","щ","з","\\", },
            new string[] { "CAP","ф","ы","в","а","п","р","о","л","д","","ENTER", },
            new string[] { "SHI","я","ч","с","м","и","т","ь","б","ю","ж","э", },
            new string[] { "RU","CTRL","-","+","ALT","SPACE","SPACE","SPACE", "SPACE", "х","ъ",".", },
        },

        new string[][]													//Символьная клавиатура русская
		{
            new string[] { "ESC","!","\"","№",";","%",":","?","*","(",")","BACK", },
            new string[] { "TAB","Й","Ц","У","К","Е","Н","Г","Ш","Щ","З","/", },
            new string[] { "CAP","Ф","Ы","В","А","П","Р","О","Л","Д","","ENTER", },
            new string[] { "SHI","Я","Ч","С","М","И","Т","Ь","Б","Ю","Ж","Э", },
            new string[] { "RU","CTRL","_","=","ALT","SPACE","SPACE","SPACE","SPACE", "Х","Ъ","," },
        },

        new string[][]													//Английская клавиатура
		{
            new string[] { "ESC", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "BACK", },
            new string[] { "TAB", "q","w","e","r","t","y","u","i","o","p","\\", },
            new string[] { "CAPS", "a","s","d","f","g","h","j","k","l","","ENTER", },
            new string[] { "SHI", "z","x","c","v","b","n","m",",",".",";","'", },
            new string[] { "EN","CTRL","-","+","ALT","SPACE","SPACE","SPACE", "SPACE", "[","]","/", },
        },

        new string[][]													//Символьная клавиатура английская
		{
            new string[] { "ESC", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "BACK", },
            new string[] { "TAB", "Q","W","E","R","T","Y","U","I","O","P","|", },
            new string[] { "CAPS", "A","S","D","F","G","H","J","K","L","","ENTER", },
            new string[] { "SHI", "Z","X","C","V","B","N","M","<",">",":","\"", },
            new string[] { "EN","CTRL","_","=","ALT","SPACE","SPACE","SPACE", "SPACE", "{","}","?", },
        }
    };

    private int language = 0;
    private int shiftMode = 0;
    private bool capsMode = false;

    private int changeItemIndex = -1;

    private Entry focused = null;

    private Button[][] keyboardKeys = 
    {
        new Button[] { new Button(),  new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() },
        new Button[] { new Button(),  new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() },
        new Button[] { new Button(),  new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() },
        new Button[] { new Button(),  new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() },
        new Button[] { new Button(),  new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button(), new Button() },
    };
    private SettingsModel settings;
    private void changeText()
    {
        keyboardGrid.Clear();

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 12; col++)
            {
                keyboardKeys[row][col] = new Button
                {
                    Text = $"{keyboards[language + shiftMode][row][col]}",
                };
                keyboardKeys[row][col].Clicked += ButtonEvent;
                keyboardGrid.Add(keyboardKeys[row][col], col + 1, row + 1);
            }
        }
    }

	public SettingsRoot()
	{

        InitializeComponent();

        settings = MauiProgram.LoadSettings();

        allSuccesefull.ItemsSource = MauiProgram.allowWebSite;

        changeText();
    }

    private void EntryClick(object sender, FocusEventArgs e)
    {
        focused = (Entry)sender;
    }
    private void ButtonEvent(object sender, EventArgs e)
    {
        bool changeText = true;

        if(((Button)sender).Text == "" || focused == null) changeText = false;

        switch(((Button)sender).Text)
        {
            case "SHI":
                shiftMode = shiftMode == 0 ? 1 : 0;
                this.changeText();
                return;

            case "RU":
            case "EN":
                language = language == 0 ? 2 : 0;
                this.changeText();
                return;

            case "BACK":
                if (!changeText) return;
                focused.Text = focused.Text.Length > 0 ? focused.Text.Remove(focused.Text.Length - 1) : "";
                return;

            default:

                if (!changeText) return;

                focused.Text += ((Button)sender).Text == "SPACE" ? " " : ((Button)sender).Text == "TAB" ? "    " : ((Button)sender).Text;

                shiftMode = capsMode? 1 : 0;
                if(!capsMode) this.changeText();

                focused.Focus();
                return;
        }
    }

    private void ListViewSelected(object sender, SelectedItemChangedEventArgs e)
    {
        for (int i = 0; i < MauiProgram.allowWebSite.Count; i++)
        {
            if(MauiProgram.allowWebSite[i] == ((ListView)sender).SelectedItem) changeItemIndex = i;
        }
    }

    private void ButtonChangeSite(object sender, EventArgs e)
    {
        if (changeItemIndex == -1 || changeItemIndex == 0) return;

        addSiteSuccesefull.Text = MauiProgram.allowWebSite[changeItemIndex];
    }
    private void ButtonDeleteItem(object sender, EventArgs e)
    {
        if (changeItemIndex == -1 || changeItemIndex == 0) return;
        MauiProgram.allowWebSite.Remove(MauiProgram.allowWebSite[changeItemIndex]);
        changeItemIndex = -1;

    }
    private void ButtonAddSite(object sender, EventArgs e)
    {
        if (addSiteSuccesefull.Text.Length > 0) MauiProgram.allowWebSite.Add(addChangeSite.Text);
    }

    private void ButtonExploerOff(object sender, CheckedChangedEventArgs e)
    {
        settings.killExploer = ((CheckBox)sender).IsChecked;
    }
    private void ButtonPowerOff(object sender, CheckedChangedEventArgs e)
    {
        settings.autoPowerOff = ((CheckBox)sender).IsChecked;
        powerOffHour.IsEnabled = settings.autoPowerOff;
        powerOffMinute.IsEnabled = settings.autoPowerOff;
        powerOffSave.IsEnabled = settings.autoPowerOff;
    }
    private void ButtonChangeShedule(object sender, EventArgs e)
    {
        settings.sheduleURL = sheduleEntry.Text;
    }


}