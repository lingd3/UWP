using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Windows.UI.Popups;

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
        }

        private async void GetAir(string num)  //查询空气质量指数
        {
            try
            {
                air.Text = "";
                // 创建一个HTTP client实例对象
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                /*
                默认情况下，HttpClient对象不会将用户代理标头随 HTTP 请求一起发送到 Web 服务。
                某些 HTTP 服务器（包括某些 Microsoft Web 服务器）要求从客户端发送的 HTTP 请求附带用户代理标头。
                如果标头不存在，则 HTTP 服务器返回错误。
                在 Windows.Web.Http.Headers 命名空间中使用类时，需要添加用户代理标头。
                我们将该标头添加到 HttpClient.DefaultRequestHeaders 属性以避免这些错误。
                */
                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                string getAir = "http://apistore.baidu.com/microservice/aqi?city=" + place.Text; //获取完整的url

                //发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(getAir);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                //var i = new MessageDialog(response.ToString()).ShowAsync();

                // 因为返回的字节流中含有中文，传输过程中，所以需要编码后才可以正常显示
                // “\u5e7f\u5dde”表示“广州”，\u表示Unicode
                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // 可以用来测试返回的结果
                //string returnContent = await response.Content.ReadAsStringAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                JsonTextReader json = new JsonTextReader(new StringReader(result));
                string jsonVal = "", quality = "";

                // 直接读取json
                while (json.Read())
                {
                    jsonVal += json.Value;
                    if (jsonVal.Equals("aqi"))  // 读到“aqi”时，提取aqi的数据加到air.Text中
                    {
                        json.Read();
                        quality = "空气质量指数： ";
                        quality += json.Value;  // 该对象重载了“+=”,故可与字符串进行连接
                        air.Text = air.Text + quality;
                    }
                    if (jsonVal.Equals("level"))  // 读到“level”
                    {
                        json.Read();
                        quality = "空气质量： ";
                        quality += json.Value;  // 该对象重载了“+=”,故可与字符串进行连接
                        air.Text = air.Text + "     " + quality;
                    }
                    jsonVal = "";
                }
            }
            catch (HttpRequestException ex1)
            {
                infor.Text = ex1.ToString();
            }
            catch (Exception ex2)
            {
                infor.Text = ex2.ToString();
            }
        }

        private async void GetPhone(string num)
        {
            try
            {
                phone_place.Text = "";
                // 创建一个HTTP client实例对象
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                /*
                默认情况下，HttpClient对象不会将用户代理标头随 HTTP 请求一起发送到 Web 服务。
                某些 HTTP 服务器（包括某些 Microsoft Web 服务器）要求从客户端发送的 HTTP 请求附带用户代理标头。
                如果标头不存在，则 HTTP 服务器返回错误。
                在 Windows.Web.Http.Headers 命名空间中使用类时，需要添加用户代理标头。
                我们将该标头添加到 HttpClient.DefaultRequestHeaders 属性以避免这些错误。
                */
                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                string getWhere = "http://life.tenpay.com/cgi-bin/mobile/MobileQueryAttribution.cgi?chgmobile=" + phone.Text;  //获取完整的url
                //var i = new MessageDialog(getWhere).ShowAsync();

                //发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(getWhere);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                // 因为返回的字节流中含有中文，传输过程中，所以需要编码后才可以正常显示
                // “\u5e7f\u5dde”表示“广州”，\u表示Unicode
                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);  //先将获取到的信息写到string类型中，再用XmlDocument将string
                XmlDocument doc = new XmlDocument();                         //还原为xml
                //result = "<?xml version='1.0' encoding='gb2312' ?>" + "\n" + result;
                //var i = new MessageDialog(result).ShowAsync();
                doc.LoadXml(result);
                XmlElement root = null;
                root = doc.DocumentElement;
                phone_place.Text = root.GetElementsByTagName("city")[0].InnerText; //获取元素为city的内容
                //var i = new MessageDialog().ShowAsync();

            }
            catch (HttpRequestException ex1)
            {
                infor.Text = ex1.ToString();
            }
            catch (Exception ex2)
            {
                infor.Text = ex2.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetAir(place.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetPhone(phone.Text);
        }
    }
}
