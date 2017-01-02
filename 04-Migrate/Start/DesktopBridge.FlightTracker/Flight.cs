﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace DesktopBridge.FlightTracker
{
    public partial class Flight : Form
    {
        private RegistryKey _regKey;
        public Flight()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Code"] = codeTextbox.Text;
            ApplicationData.Current.LocalSettings.Values["Date"] = dateTextbox.Text;
            ApplicationData.Current.LocalSettings.Values["Departure"] = departureTextbox.Text;
            ApplicationData.Current.LocalSettings.Values["Arrival"] = arrivalTextbox.Text;

            operationStatusLabel.Text = "The flight has been saved";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Code"))
            {
                codeTextbox.Text = ApplicationData.Current.LocalSettings.Values["Code"].ToString();
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Date"))
            {
                dateTextbox.Text = ApplicationData.Current.LocalSettings.Values["Date"].ToString();
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Departure"))
            {
                departureTextbox.Text = ApplicationData.Current.LocalSettings.Values["Departure"].ToString();
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Arrival"))
            {
                arrivalTextbox.Text = ApplicationData.Current.LocalSettings.Values["Arrival"].ToString();
            }

            string triggerName = "FlightTimeZoneTrigger";

            // Check if the task is already registered
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == triggerName)
                {
                    // The task is already registered.
                    return;
                }
            }

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = triggerName;
            builder.TaskEntryPoint = "DesktopBridge.FlightTracker.Notification.ToastTask";
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            builder.Register();


            DesktopBridge.Helpers bridgeHelpers = new DesktopBridge.Helpers();
            if (bridgeHelpers.IsRunningAsUwp())
            {
                updateStripMenuItem.Visible = false;
            }
        }

        private async void exportButton_Click(object sender, EventArgs e)
        {
            operationStatusLabel.Text = "Exporting file...";
            progressBar.Visible = true;

            // Simulate a high load process
            await Task.Delay(5 * 1000);

            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fileName = $"{userPath}\\BoardingPass.txt";
            var builder = new StringBuilder();
            builder.AppendLine("Boarding pass generated by FlightTracker");
            builder.AppendLine("-----------------------------------------");
            builder.AppendLine($"Flight code: {codeTextbox.Text}");
            builder.AppendLine($"Flight date: {dateTextbox.Text}");
            builder.AppendLine($"Departure city: {departureTextbox.Text}");
            builder.AppendLine($"Arrival city: {arrivalTextbox.Text}");
            builder.AppendLine("-----------------------------------------");
            builder.AppendLine("Thank you for using FlightTracker");
            File.WriteAllText(fileName, builder.ToString());

            progressBar.Visible = false;
            operationStatusLabel.Visible = false;
            ShowNotification();
        }

        private void aboutFlightTrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        [Conditional("DesktopUWP")]
        private void ShowNotification()
        {
            string xml = $@"<toast>
                <visual>
                    <binding template='ToastGeneric'>
                        <text>Flight Tracker</text>
                        <text>The boarding pass for flight {codeTextbox.Text} from {departureTextbox.Text} to {arrivalTextbox.Text} has been exported on your desktop</text>
                    </binding>
                </visual>
            </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void updateStripMenuItem_Click(object sender, EventArgs e)
        {
            string title = "Flight Tracker";
            string message = "There are no updates available.";

            MessageBox.Show(message, title);
        }
    }
}
