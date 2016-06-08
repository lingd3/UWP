using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace HW3.Models {
  public class TodoItem {
    public string Id;

    public string Title { get; set; }

    public string Discription { get; set; }

    public bool? Completed { get; set; }

    public DateTime DueDate { get; set; }

    public ImageSource ImagePath { get; set; }

    public TodoItem(string title, string discription, DateTimeOffset duedate, ImageSource imagepath) {
      this.Id = Guid.NewGuid().ToString();
      this.Title = title;
      this.Discription = discription;
      this.Completed = false;
      this.DueDate = duedate.DateTime;
      this.ImagePath = imagepath;
    }

    // validata the Todo's information
    // notify if invalid
    public bool TodoInfoValidator() {
      string MessageToNotify = string.Empty;

      if (this.Title.Length == 0) {
        MessageToNotify += "Title is empty!\n";
      }

      if (this.Discription.Length == 0) {
        MessageToNotify += "Details are empty!\n";
      }

      if (this.DueDate.Subtract(DateTime.Now).Days < 0) {
        MessageToNotify += "Due date should be later than now!\n";
      }

      if (MessageToNotify.Length != 0) {
        var Notify = new MessageDialog(MessageToNotify).ShowAsync();
        return false;
      } else {
        return true;
      }
    }

    // update Todo's information expect Id
    public void Update(ref TodoItem UpdateInfo) {
      this.Title = UpdateInfo.Title;
      this.Discription = UpdateInfo.Discription;
      this.DueDate = UpdateInfo.DueDate;
      this.ImagePath = UpdateInfo.ImagePath;
    }
  }
}
