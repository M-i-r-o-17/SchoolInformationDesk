namespace SchoolInformationDesk
{
    public partial class MainPage : ContentPage
    {
    
        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            Task.Run(WebViewLoader);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            await browser.FadeTo(0, 500, Easing.Linear);
            browser.IsVisible = false;
            loadImage.IsVisible = true;
            UrlWebViewSource source = (UrlWebViewSource)e.Source;
            string uri = source?.Url == null ? "" : source.Url;

            if (uri == "") return;

            try
            {
                uri = uri.Split("/")[2];
                MauiProgram.curretUrl = source.Url;
                url.Text = "Загрузка...";

                if (MauiProgram.allowVisit(uri)) return;

                if (browser.CanGoBack) browser.GoBack();
                else browser.Source = "https://" + MauiProgram.defaultRedirect + "/";
            }
            catch { return; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            await browser.EvaluateJavaScriptAsync("let links = document.querySelectorAll(\"a\"); for(let index = 0; index < links.length; index++){console.log(index);links[index].setAttribute(\"target\",\"\")}");
            url.Text = MauiProgram.curretUrl;
            browser.IsVisible = true;
            loadImage.IsVisible = false;
            await browser.FadeTo(1, 500, Easing.Linear);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task WebViewLoader()
        {
            while (true)
            {
                if(loadImage.IsVisible)
                {
                    await loadImage.FadeTo(1, 500, Easing.Linear);
                    while (loadImage.IsVisible)
                    {
                        await loadImage.ScaleTo(0.5, 2000, Easing.Linear);
                        await loadImage.ScaleTo(1, 2000, Easing.Linear);
                    }
                    await loadImage.FadeTo(0, 500, Easing.Linear);
                }

                back.IsEnabled = browser.CanGoBack;
                forward.IsEnabled = browser.CanGoForward;
            }

        }
    
        private void ButtonBack(object sender, System.EventArgs e)
        {
            if(browser.CanGoBack) browser.GoBack();
        }

        private void ButtonReload(object sender, System.EventArgs e)
        {
            browser.Reload();
        }
        private void ButtonForward(object sender, System.EventArgs e)
        {
            if (browser.CanGoForward) browser.GoForward();
        }
        private void ButtonHome(object sender, System.EventArgs e)
        {
            browser.Source = "https://" + MauiProgram.defaultRedirect + "/";
        }
    }

}
