using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using TascheAtWork.PocketAPI;
using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Test();
            DataContext = this;
        }
        
        private readonly List<Site>  Sites = new List<Site>();

        private void Test()
        {
            // "http://localhost/authorizationFinished"
            var client = new PocketAPIClient(callbackUri: "#");

            client.GetRequestCode();

            var urlForAccessConfirm = client.GenerateAuthenticationUri();

            Process.Start(urlForAccessConfirm.AbsoluteUri);
            
            var usr = client.GetUser();
            



            try
            {
                Task.Factory.StartNew(() => client.GetItems(count: 20))
                                 .ContinueWith(t1 =>
                                 {
                                     foreach (var pocketItem in t1.Result.Where(pocketItem => pocketItem.Uri != null))
                                         Sites.Add(new Site() {Content = pocketItem.Title, Url = pocketItem.Uri.AbsoluteUri});
                                 });
             


            }
            catch (PocketAPIException ex)
            {
                Debug.Write(ex.Message);
            }
        }

        
    }


    public class Site
    {
        public string Url { get; set; }
        public string Content { get; set; }
    }
}




