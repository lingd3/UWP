using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace hw2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private int time = 0;
        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            time++;
            if (time%2 != 0)
            {
                line.Visibility = Visibility.Visible;
            } 
            else
            {
                line.Visibility = Visibility.Collapsed;
            }
        }

        private int time1 = 0;
        private void checkbox_Click1(object sender, RoutedEventArgs e)
        {
            time1++;
            if (time1 % 2 != 0)
            {
                line1.Visibility = Visibility.Visible;
            }
            else
            {
                line1.Visibility = Visibility.Collapsed;
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }

    }
}
