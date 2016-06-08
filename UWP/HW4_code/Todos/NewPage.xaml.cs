using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Todos.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Todos
{
    public sealed partial class NewPage : Page
    {
        private StorageFile storagefile;

        public NewPage()
        {
            this.InitializeComponent();

            ViewModel = new TheViewModel();
            DataContext = ViewModel;

            ViewModel.LoadData();
        }

        TheViewModel ViewModel { get; set; }

        private void Create_Item(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((App)App.Current).BackRequested += Page2_BackRequested;

            if (e.NavigationMode == NavigationMode.New)
            {
                // If this is a new navigation, this is a fresh launch so we can
                // discard any saved state
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
            else
            {
                // Try to restore state if any, in case we were terminated
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
                    textBox1.Text = (string)composite["Title"];
                    textBox2.Text = (string)composite["Details"];
                    Date.Date = (DateTimeOffset)composite["Date"];
                    // We're done with it, so remove it
                    ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ((App)App.Current).BackRequested -= Page2_BackRequested;

            bool suspending = ((App)App.Current).IsSuspending;
            if (suspending)
            {
                // Save volatile state in case we get terminated later on, then
                // we can restore as if we'd never been gone :)
                var composite = new ApplicationDataCompositeValue();
                composite["Title"] = textBox1.Text;
                composite["Details"] = textBox2.Text;
                composite["Date"] = Date.Date;
                ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
            }
        }

        private void Page2_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // When leaving the page, save the app data
            ViewModel.SaveData();
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
                Image.Source = writeableBitmap;
            }
        }
    }
}
