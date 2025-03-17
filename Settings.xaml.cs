
namespace SchoolInformationDesk;

public partial class Settings : ContentPage
{
	private string password = "";
	public Settings()
	{
		InitializeComponent();
	}

    private async void ButtonEvent(object sender, EventArgs e)
	{
		Button btn = (Button) sender;
		switch(btn.Text)
		{
			case "Enter":
                MauiProgram.curretUser =  MauiProgram.userAcces(password);
				break;
			case "Delete":
				if(password.Length > 0)
				{
                    password = password.Remove(password.Length - 1);
					labelPassword.Text = labelPassword.Text.Remove(labelPassword.Text.Length - 1);
                }
				
                break;
			default:
				password += btn.Text;
				labelPassword.Text += "*";
				break;
		}

		if (MauiProgram.curretUser == -1) return;

		switch(MauiProgram.curretUser)
		{
			case 0:
					await Navigation.PushAsync(new SettingsRoot());
				break;
			default:
				return;
        }
	}
}