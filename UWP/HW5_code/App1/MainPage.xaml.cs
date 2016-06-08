using SQLitePCL;
using System;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace App1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }

            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";
            }
            else
            {
                createButton.Content = "Update";
                _image.Source = ViewModel.SelectedItem.image;
                _title.Text = ViewModel.SelectedItem.title;
                _details.Text = ViewModel.SelectedItem.details;
                time.Date = ViewModel.SelectedItem.setdate;
            }
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                if (ViewModel.SelectedItem == null)
                {
                    createButton.Content = "Create";
                }
                else
                {
                    createButton.Content = "Update";
                    cancelButton.Content = "Delete";
                    _image.Source = ViewModel.SelectedItem.image;
                    _title.Text = ViewModel.SelectedItem.title;
                    _details.Text = ViewModel.SelectedItem.details;
                    time.Date = ViewModel.SelectedItem.setdate;
                }
            }
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
        }

        int count = 0;
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 4) as Line;
            if (count == 0)
            {
                line.Visibility = Visibility.Collapsed;
                count = 1;
            }
            else
            {
                line.Visibility = Visibility.Visible;
                count = 0;
            }
        }

        int count1 = 0;
        private void checkBox_Checked1(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 2) as Line;
            if (count1 == 0)
            {
                line.Visibility = Visibility.Collapsed;
                count1 = 1;
            }
            else
            {
                line.Visibility = Visibility.Visible;
                count1 = 0;
            }
        }

        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.UpdateTodoItem(_image.Source, _title.Text, _details.Text, time.Date.Date, true);
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (createButton.Content.ToString() == "Create")
            {
                if (ViewModel.SelectedItem == null)
                {
                    if (_title.Text == "")
                    {
                        var i = new MessageDialog("Title 不能为空！").ShowAsync();
                        return;
                    }
                    else if (_details.Text == "")
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
                    ViewModel.AddTodoItem(_image.Source, _title.Text, _details.Text, time.Date.Date, false);

                    var db = App.conn;
                    //using (var todo = db.Prepare("INSERT INTO Todo(Title, Context, Date) VALUES (?, ?, ?)"))
                    //{
                    //    string a = time.Date.DateTime.ToString();
                    //    todo.Bind(1, _title.Text);
                    //    todo.Bind(2, _details.Text);
                    //    todo.Bind(3, a);
                    //    todo.Step();
                    //}

                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                else
                {
                    if (_title.Text == "")
                    {
                        var i = new MessageDialog("Title 不能为空！").ShowAsync();
                        return;
                    }
                    else if (_details.Text == "")
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
                ViewModel.UpdateTodoItem(_image.Source, _title.Text, _details.Text, time.Date.Date, true);
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
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
                this._image.Source = bmp;
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (cancelButton.Content.ToString() == "Delete")
            {
                ViewModel.RemoveTodoItem();
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
            System.DateTime nowtime = System.DateTime.Now;
            RandomAccessStreamReference img = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/background.jpg"));
            IRandomAccessStream stream = await img.OpenReadAsync();
            Windows.UI.Xaml.Media.Imaging.BitmapImage bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            bmp.SetSource(stream);
            _image.Source = bmp;
            _title.Text = "";
            _details.Text = "";
            time.Date = nowtime;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText("tiles.xml"));

            XmlNodeList textElements = doc.GetElementsByTagName("text");
            if (ViewModel.AllItems.Count == 0) return;

            ((XmlElement)textElements[0]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].title;
            ((XmlElement)textElements[1]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].details;
            ((XmlElement)textElements[2]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].title;
            ((XmlElement)textElements[3]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].details;
            ((XmlElement)textElements[4]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].title;
            ((XmlElement)textElements[5]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].details;
            ((XmlElement)textElements[6]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].title;
            ((XmlElement)textElements[7]).InnerText = ViewModel.AllItems[ViewModel.AllItems.Count - 1].details;

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            var notification = new TileNotification(doc);
            updater.Update(notification);

        }

        dynamic item;
        private void share_click(object sender, RoutedEventArgs e)
        {
            item = ((MenuFlyoutItem)sender).DataContext;
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

        void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            DataRequestDeferral getFiles = request.GetDeferral();
            request.Data.Properties.Title = item.title;
            request.Data.Properties.Description = item.details;
            request.Data.SetBitmap(Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/background.jpg")));
            request.Data.SetText(item.title);
            getFiles.Complete();        
        }

        private void BtnGetAll_Click(object sender, RoutedEventArgs e)
        {
            var db = App.conn;
            using (var statement = db.Prepare("SELECT * FROM Todo WHERE Title LIKE ? OR Context LIKE ? OR Date LIKE ?"))
            {
                string a = "%" + Query.Text + "%";
                statement.Bind(1, a);
                statement.Bind(2, a);
                statement.Bind(3, a);
                string b = "";
                int b_ = 0;
                while (SQLiteResult.ROW == statement.Step())
                {
                    b = b + "Title: " + (string)statement[0] + ";  Context: " + (string)statement[1] + ";  Date: " + (string)statement[2] + "\n";
                    b_ = 1;
                }
                if (Query.Text != "" && b_ != 0)
                {
                    var i = new MessageDialog(b).ShowAsync();
                }
            }




        }


    }
}
