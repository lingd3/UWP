using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace App1.Models
{
    class TodoItem
    {

        public string id;

        public ImageSource image { get; set; }

        public string title { get; set; }

        public string details { get; set; }

        public bool completed { get; set; }

        public DateTime setdate { get; set; }

        public TodoItem(ImageSource image_source, string title, string details, DateTime d, bool c)
        {
            this.id = Guid.NewGuid().ToString();
            this.image = image_source;
            this.title = title;
            this.details = details;
            this.setdate = d;
            this.completed = c;
        }
    }
}
