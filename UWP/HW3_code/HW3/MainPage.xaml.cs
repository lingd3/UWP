using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW3 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
      this.ViewModel = new ViewModels.TodoItemViewModel();
      this.SizeChanged += (s, e) => {
        SideGridShow = e.NewSize.Width > 800 ? true : false;
      };
     }

    public ViewModels.TodoItemViewModel ViewModel { get; set; }

    private bool SideGridShow { get; set; }  // whether the side grid show or not

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel)) {
        this.ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
        SideGrid_Set();
      }
    }

    // set info of the side grid in right
    private void SideGrid_Set() {
      if (ViewModel.SelectedItem == null) {
        // default style
        CreateButton.Visibility = Visibility.Visible;
        UpdateButton.Visibility = Visibility.Collapsed;
        TitleTextBox.Text = string.Empty;
        DetailTextBox.Text = string.Empty;
        DueDatePicker.Date = DateTime.Now;
      } else {
        CreateButton.Visibility = Visibility.Collapsed;
        UpdateButton.Visibility = Visibility.Visible;
        TitleTextBox.Text = ViewModel.SelectedItem.Title;
        DetailTextBox.Text = ViewModel.SelectedItem.Discription;
        DueDatePicker.Date = ViewModel.SelectedItem.DueDate;
        SlideImage.Source = ViewModel.SelectedItem.ImagePath;
      }
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e) {
      if (!SideGridShow)
        Frame.Navigate(typeof(AddTodoPage), ViewModel);
    }

    private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e) {
      ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
      if (!SideGridShow)
        Frame.Navigate(typeof(AddTodoPage), ViewModel);
      SideGrid_Set();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Models.TodoItem TodoToCreate = new Models.TodoItem(TitleTextBox.Text,
        DetailTextBox.Text, DueDatePicker.Date, SlideImage.Source);

      if (TodoToCreate.TodoInfoValidator()) {
        ViewModel.AddTodoItem(TodoToCreate);
      }
    }

    // update Todo's info and refresh page
    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        Models.TodoItem TodoToUpdate = new Models.TodoItem(TitleTextBox.Text,
          DetailTextBox.Text, DueDatePicker.Date, SlideImage.Source);
        TodoToUpdate.Id = ViewModel.SelectedItem.Id;

        if (TodoToUpdate.TodoInfoValidator()) {
          ViewModel.UpdateTodoItem(ViewModel.SelectedItem, TodoToUpdate);
          SideGrid_Set();
        }
      }
    }

    // unselect the TodoItem
    // let it enable to create one
    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      ViewModel.SelectedItem = null;
      SideGrid_Set();
    }

    private async void SelectPictureButton_Click(object sender, RoutedEventArgs e) {
      FileOpenPicker picker = new FileOpenPicker();
      // set format
      picker.ViewMode = PickerViewMode.Thumbnail;
      picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
      picker.FileTypeFilter.Add(".jpg");
      picker.FileTypeFilter.Add(".jpeg");
      picker.FileTypeFilter.Add(".png");

      // Open a stream for the selected file 
      StorageFile file = await picker.PickSingleFileAsync();
      // Ensure a file was selected 
      if (file != null) {
        using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read)) {
          // Set the image source to the selected bitmap 
          BitmapImage bitmapImage = new BitmapImage();
          bitmapImage.DecodePixelWidth = 350; //match the target Image.Width, not shown
          await bitmapImage.SetSourceAsync(fileStream);
          SlideImage.Source = bitmapImage;
        }
      }
    }
  }
}
