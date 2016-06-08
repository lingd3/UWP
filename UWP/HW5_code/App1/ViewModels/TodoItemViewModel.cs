using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App1.ViewModels
{
    class TodoItemViewModel : ObservableCollection<Models.TodoItem>
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            //ImageSource image_source = new BitmapImage(new Uri("Assets / background.jpg", UriKind.Relative));
            //this.allItems.Add(new Models.TodoItem(image_source, "123", "123", DateTime.Now, true));
        }

        public void AddTodoItem(ImageSource image, string title, string details, DateTime d, bool c)
        {
            this.allItems.Add(new Models.TodoItem(image, title, details,d, c));
        }

        public void RemoveTodoItem()
        {
            allItems.Remove(selectedItem);
            this.selectedItem = null; 
        }


        public void UpdateTodoItem(ImageSource i, string t, string de, DateTime d, bool c)
        {
            this.selectedItem.image = i;
            this.selectedItem.title = t;
            this.selectedItem.details = de;
            this.selectedItem.setdate = d;
            selectedItem = null;
        }

    }
}