using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.SharePoint.Client;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BOSCC_GPIO
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        EnvironmentManager manager = new EnvironmentManager();
        //ClientContext sharepoint = new ClientContext("https://bluemetal-my.sharepoint.com/personal/jimw_bluemetal_com/IoT/");
        ClientContext sharepoint = new ClientContext("https://insightonline-my.sharepoint.com/personal/jim_wilcox_insight_com/IoT/");


        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = manager;
            sharepoint.Credentials = new SharePointOnlineCredentials(Credentials.Username, Credentials.Password);
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            manager.RefreshTemp();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            Web web = sharepoint.Web;
            List log = web.Lists.GetByTitle("IoTLog");
            ListItemCreationInformation newLogEntryTemplate = new ListItemCreationInformation();
            ListItem newLogEntry = log.AddItem(newLogEntryTemplate);
            newLogEntry["Title"] = "Device #NHSPUG";
            newLogEntry["MeasuredTemperature"] = manager.MeasuredTemperature;
            newLogEntry["HeaterPowerOn"] = manager.HeaterPowerOn;
            newLogEntry["ACPowerOn"] = manager.ACPowerOn;
            newLogEntry.Update();
            await sharepoint.ExecuteQueryAsync();
        }

        private void chkHeater_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
