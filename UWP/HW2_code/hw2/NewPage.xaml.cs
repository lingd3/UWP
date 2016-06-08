using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace hw2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        private StorageFile storagefile;

        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            Uri uri = new Uri(BaseUri, "Assets/background.jpg");
            BitmapImage imgSource = new BitmapImage(uri);
            this.img.Source = imgSource;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            var i = new MessageDialog("Welcome!").ShowAsync();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (title.Text == "")
            {
                var i = new MessageDialog("Title 不能为空！").ShowAsync();
            }
            else if (details.Text == "")
            { 
                var i = new MessageDialog("Details 不能为空！").ShowAsync(); 
            }
            else if (DateTimeOffset.Now.Year > date.Date.Year ||
               DateTimeOffset.Now.Year == date.Date.Year && DateTimeOffset.Now.Month > date.Date.Month ||
               DateTimeOffset.Now.Year == date.Date.Year && DateTimeOffset.Now.Month == date.Date.Month && DateTimeOffset.Now.Day > date.Date.Day)
            {
                var i = new MessageDialog("请选择正确的日期！").ShowAsync();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            details.Text = "";
            date.Date = DateTimeOffset.Now;
            Uri uri = new Uri(BaseUri, "Assets/background.jpg");
            BitmapImage imgSource = new BitmapImage(uri);
            this.img.Source = imgSource;
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            storagefile = await picker.PickSingleFileAsync();
            if (storagefile != null)
            {
                WriteableBitmap writeableBitmap = new WriteableBitmap(500, 500);
                IRandomAccessStream stream = await storagefile?.OpenAsync(FileAccessMode.Read);
                await writeableBitmap.SetSourceAsync(stream);
                img.Source = writeableBitmap;
            }
        }
    }
}
