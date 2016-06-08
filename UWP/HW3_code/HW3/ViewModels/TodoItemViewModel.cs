using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3.ViewModels {
  public class TodoItemViewModel {
    private ObservableCollection<Models.TodoItem> Items = new ObservableCollection<Models.TodoItem>();
    public ObservableCollection<Models.TodoItem> AllItems { get { return this.Items; }}

    private Models.TodoItem selectedItem = default(Models.TodoItem);
    public Models.TodoItem SelectedItem {
      get { return selectedItem; }
      set { this.selectedItem = value; }
    }

    public TodoItemViewModel() {}

    public void AddTodoItem(Models.TodoItem todo) {
      this.Items.Add(todo);
    }

    public void DeleteTodoItem(string id) {
      this.Items.Remove(this.Items.SingleOrDefault(i => i.Id == id));
      this.selectedItem = null;
    }

    public void UpdateTodoItem(Models.TodoItem OriginTodo, Models.TodoItem UpdateInfo) {
      int index = this.Items.IndexOf(OriginTodo);
      if (index >= 0 && index < this.Items.Count) {
        this.Items[index] = UpdateInfo;
      }
      this.selectedItem = null;
    }
  }
}
