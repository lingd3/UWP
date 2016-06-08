using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Animal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate string AnimalSaying(object sender, myEventArgs e);//声明一个委托
        private event AnimalSaying Say;//委托声明一个事件
        private int times = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        interface Animal
        {
            string saying(object sender, myEventArgs e);
        }

        class cat : Animal
        {
            TextBlock word;

            public cat(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "cat: I am a Cat............\n";
                return "";
            }
        }

        class dog : Animal
        {
            TextBlock word;

            public dog(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "dog: I am a Dog............\n";
                return "";
            }
        }

        class pig : Animal
        {
            TextBlock word;

            public pig(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "pig: I am a Pig............\n";
                return "";
            }
        }

        private cat c;
        private dog d;
        private pig p;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Input.Text == "cat")
            {
                words.Text = "";
                c = new cat(words);
                //注册事件
                Say += new AnimalSaying(c.saying);
                Input.Text = "";
            }
            else if (Input.Text == "dog")
            {
                words.Text = "";
                d = new dog(words);
                Say += new AnimalSaying(d.saying);
                Input.Text = "";
            }
            else if (Input.Text == "pig")
            {
                words.Text = "";
                p = new pig(words);
                Say += new AnimalSaying(p.saying);
                Input.Text = "";
            }
            else {
                Input.Text = "";
                return;
            }

            //执行事件
            Say(this, new myEventArgs(times));  //事件中传递参数times
        }

        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            Random ran = new Random();
            int RandKey = ran.Next(0, 4);
            if (RandKey == 1)
            {
                words.Text = "";
                c = new cat(words);
                //注册事件
                Say += new AnimalSaying(c.saying);
            }
            else if (RandKey == 2)
            {
                words.Text = "";
                d = new dog(words);
                //注册事件
                Say += new AnimalSaying(d.saying);
            }
            else
            {
                words.Text = "";
                p = new pig(words);
                //注册事件
                Say += new AnimalSaying(p.saying);
            }

            //执行事件
            Say(this, new myEventArgs(times));  //事件中传递参数times
        }

        //自定义一个Eventargs传递事件参数
        class myEventArgs : EventArgs
        {
            public int t = 0;
            public myEventArgs(int tt)
            {
                this.t = tt;
            }
        }

    }
}


