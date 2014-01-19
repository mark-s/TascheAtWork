using System.Collections.Generic;
using System.Diagnostics;
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
        }
        
        private readonly List<Site>  Sites = new List<Site>();

        private void Test()
        {

            PocketAPIClient client = new PocketAPIClient(platformConsumerKey: "19717-4b69b3aeae8cf912108818c4", callbackUri: "http://localhost/authorizationFinished");

            client.GetRequestCode();

            var urlForAccessConfirm = client.GenerateAuthenticationUri();

            Process.Start(urlForAccessConfirm.AbsoluteUri);


            var usr = client.GetUser();


            try
            {
                List<PocketItem> items = client.GetItems(count: 20);


                foreach (var pocketItem in items)
                {
                    if (pocketItem.Uri != null)
                    {
                        Sites.Add(new Site() {Content = pocketItem.Title, Url = pocketItem.Uri.AbsoluteUri});
                    }

                }


            }
            catch (PocketException ex)
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




