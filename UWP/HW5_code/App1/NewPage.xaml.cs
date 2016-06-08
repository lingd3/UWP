using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using App1.Models;

namespace App1
{
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }

        public async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            System.DateTime nowtime = System.DateTime.Now;
            title.Text = "";
            details.Text = "";
            time.Date = nowtime;
            RandomAccessStreamReference img = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/background.jpg"));
            IRandomAccessStream stream = await img.OpenReadAsync();
            Windows.UI.Xaml.Media.Imaging.BitmapImage bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            bmp.SetSource(stream);
            image.Source = bmp;
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker newphoto = new FileOpenPicker();
            newphoto.FileTypeFilter.Add(".jpg");
            newphoto.FileTypeFilter.Add(".jpeg");
            newphoto.FileTypeFilter.Add(".png");
            newphoto.FileTypeFilter.Add(".bmp");
            newphoto.FileTypeFilter.Add(".gif");
            newphoto.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            StorageFile file = await newphoto.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                Windows.UI.Xaml.Media.Imaging.BitmapImage bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bmp.SetSource(stream);
                this.image.Source = bmp;
            }
        }

        private ViewModels.TodoItemViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = ((ViewModels.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";

            }
            else
            {
                createButton.Content = "Update";
                image.Source = ViewModel.SelectedItem.image;
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.details;
                time.Date = ViewModel.SelectedItem.setdate;
            }
        }


        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (createButton.Content.ToString() == "Create")
            {
                if (ViewModel.SelectedItem == null)
                {
                    if (title.Text == "")
                    {
                        var i = new MessageDialog("Title 不能为空！").ShowAsync();
                        return;
                    }
                    else if (details.Text == "")
                    {
                        var i = new MessageDialog("Details 不能为空！").ShowAsync();
                        return;
                    }
                    else if (DateTimeOffset.Now.Year > time.Date.Year ||
                       DateTimeOffset.Now.Year == time.Date.Year && DateTimeOffset.Now.Month > time.Date.Month ||
                       DateTimeOffset.Now.Year == time.Date.Year && DateTimeOffset.Now.Month == time.Date.Month && DateTimeOffset.Now.Day > time.Date.Day)
                    {
                        var i = new MessageDialog("请选择正确的日期！").ShowAsync();
                        return;
                    }
                    ViewModel.AddTodoItem(image.Source, title.Text, details.Text, time.Date.Date, false);

                    //var db = App.conn;
                    //using (var todo = db.Prepare("INSERT INTO Todo(Title, Context, Date) VALUES (?, ?, ?)"))
                    //{
                    //    string a = time.Date.DateTime.ToString();
                    //    todo.Bind(1, title.Text);
                    //    todo.Bind(2, details.Text);
                    //    todo.Bind(3, a);
                    //    todo.Step();
                    //}

                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                else
                {
                    if (title.Text == "")
                    {
                        var i = new MessageDialog("Title 不能为空！").ShowAsync();
                        return;
                    }
                    else if (details.Text == "")
                    {
                        var i = new MessageDialog("Details 不能为空！").ShowAsync();
                        return;
                    }
                    else if (DateTimeOffset.Now.Year > time.Date.Year ||
                       DateTimeOffset.Now.Year == time.Date.Year && DateTimeOffset.Now.Month > time.Date.Month ||
                       DateTimeOffset.Now.Year == time.Date.Year && DateTimeOffset.Now.Month == time.Date.Month && DateTimeOffset.Now.Day > time.Date.Day)
                    {
                        var i = new MessageDialog("请选择正确的日期！").ShowAsync();
                        return;
                    }
                    UpdateButton_Clicked(sender, e);
                }
            }
            else
            {
                ViewModel.UpdateTodoItem(image.Source, title.Text, details.Text, time.Date.Date, true);
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }


        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem();
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
            else
            {
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.UpdateTodoItem(image.Source, title.Text, details.Text, time.Date.Date, true);
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
    }
}