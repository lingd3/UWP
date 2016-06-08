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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW3 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class AddTodoPage : Page {
    public AddTodoPage() {
      this.InitializeComponent();
    }

    private ViewModels.TodoItemViewModel ViewModel;

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
      if (ViewModel.SelectedItem == null) {
        UpdateButton.Visibility = Visibility.Collapsed;
      } else {
        CreateButton.Visibility = Visibility.Collapsed;
        DueDatePicker.Date = ViewModel.SelectedItem.DueDate;
      }
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Models.TodoItem TodoToCreate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source);
      if (TodoToCreate.TodoInfoValidator()) {
        ViewModel.AddTodoItem(TodoToCreate);
        Frame.Navigate(typeof(MainPage), ViewModel);
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      ViewModel.SelectedItem = null;
      Frame.Navigate(typeof(MainPage), ViewModel);
    }

    private void DeleteTodoButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        ViewModel.DeleteTodoItem(ViewModel.SelectedItem.Id);
        Frame.Navigate(typeof(MainPage), ViewModel);
      }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        Models.TodoItem TodoToUpdate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source);
        TodoToUpdate.Id = ViewModel.SelectedItem.Id;
        if (TodoToUpdate.TodoInfoValidator()) {
          ViewModel.UpdateTodoItem(ViewModel.SelectedItem, TodoToUpdate);
          Frame.Navigate(typeof(MainPage), ViewModel);
        }
      }
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
          TodoImage.Source = bitmapImage;
        }
      }
    }
  }
}
