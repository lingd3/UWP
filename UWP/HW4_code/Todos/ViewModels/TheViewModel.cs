using Todos.Models;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.ViewModels
{
    class TheViewModel : ViewModelBase
    {
        private string title;
        public string Title { get { return title; } set { Set(ref title, value); } }

        private string details;
        public string Details { get { return details; } set { Set(ref details, value); } }

        private DateTime date;
        public DateTime Date { get { return date; } set { Set(ref date, value); } }


        #region Methods for handling the apps permanent data
        public void LoadData()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("TheData"))
            {
                MyDataItem data = JsonConvert.DeserializeObject<MyDataItem>(
                    (string)ApplicationData.Current.RoamingSettings.Values["TheData"]);
                Title = data.Title;
                Details = data.Details;
                Date = data.Date;
            }
            else
            {
                // New start, initialize the data
                Title = string.Empty;
                Details = string.Empty;
                Date = DateTime.Now;
            }
        }

        public void SaveData()
        {
            MyDataItem data = new MyDataItem { Title = this.Title, Details = this.Details };
            ApplicationData.Current.RoamingSettings.Values["TheData"] =
                JsonConvert.SerializeObject(data);
        }
        #endregion
    }
}
