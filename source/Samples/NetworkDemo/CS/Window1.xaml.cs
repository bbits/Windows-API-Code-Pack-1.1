//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text;
using Microsoft.WindowsAPICodePack.Net;
using System.ComponentModel;

namespace Microsoft.WindowsAPICodePack.Samples.NetworkDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private TextBlock EventLog = new TextBlock();

        public Window1()
        {
            InitializeComponent();

            LoadNetworkConnections();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // release all COM event handlers
            NetworkListManager.ConnectivityChanged -= NetworkListManager_ConnectivityChanged;

            NetworkListManager.NetworkAdded -= NetworkListManager_NetworkAdded;
            NetworkListManager.NetworkConnectivityChanged -= NetworkListManager_NetworkConnectivityChanged;
            NetworkListManager.NetworkDeleted -= NetworkListManager_NetworkDeleted;
            NetworkListManager.NetworkPropertyChanged -= NetworkListManager_NetworkPropertyChanged;

            NetworkListManager.NetworkConnectionConnectivityChanged -= NetworkListManager_NetworkConnectionConnectivityChanged;
            NetworkListManager.NetworkConnectionPropertyChanged -= NetworkListManager_NetworkConnectionPropertyChanged;
        }

        private void LoadNetworkConnections()
        {
            NetworkCollection networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.All);

            foreach (Network n in networks)
            {
                // Create a tab
                TabItem tabItem = new TabItem();
                tabItem.Header = string.Format("Network {0} ({1})", tabControl1.Items.Count, n.Name);
                tabControl1.Items.Add(tabItem);

                //
                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Orientation = Orientation.Vertical;

                // List all the properties
                AddProperty("Name: ", n.Name, stackPanel2);
                AddProperty("Description: ", n.Description, stackPanel2);
                AddProperty("Domain type: ", n.DomainType.ToString(), stackPanel2);
                AddProperty("Is connected: ", n.IsConnected.ToString(), stackPanel2);
                AddProperty("Is connected to the internet: ", n.IsConnectedToInternet.ToString(), stackPanel2);
                AddProperty("Network ID: ", n.NetworkId.ToString(), stackPanel2);
                AddProperty("Category: ", n.Category.ToString(), stackPanel2);
                AddProperty("Created time: ", n.CreatedTime.ToString(), stackPanel2);
                AddProperty("Connected time: ", n.ConnectedTime.ToString(), stackPanel2);
                AddProperty("Connectivity: ", n.Connectivity.ToString(), stackPanel2);

                //
                StringBuilder s = new StringBuilder();
                s.AppendLine("Network Connections:");
                NetworkConnectionCollection connections = n.Connections;
                foreach (NetworkConnection nc in connections)
                {
                    s.AppendFormat("\n\tConnection ID: {0}\n\tDomain: {1}\n\tIs connected: {2}\n\tIs connected to internet: {3}\n",
                        nc.ConnectionId, nc.DomainType, nc.IsConnected, nc.IsConnectedToInternet);
                    s.AppendFormat("\tAdapter ID: {0}\n\tConnectivity: {1}\n",
                        nc.AdapterId, nc.Connectivity);
                }
                s.AppendLine();

                Label label = new Label();
                label.Content = s.ToString();

                stackPanel2.Children.Add(label);
                tabItem.Content = stackPanel2;
            }

            // add events logging tab
            ScrollViewer scroll = new ScrollViewer();
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.CanContentScroll = true;
            scroll.Content = EventLog;

            TabItem tabItem2 = new TabItem();
            tabItem2.Header = "Events";
            tabControl1.Items.Add(tabItem2);

            tabItem2.Content = scroll;

            // add COM event handlers
            NetworkListManager.ConnectivityChanged += NetworkListManager_ConnectivityChanged;

            NetworkListManager.NetworkAdded += NetworkListManager_NetworkAdded;
            NetworkListManager.NetworkConnectivityChanged += NetworkListManager_NetworkConnectivityChanged;
            NetworkListManager.NetworkDeleted += NetworkListManager_NetworkDeleted;
            NetworkListManager.NetworkPropertyChanged += NetworkListManager_NetworkPropertyChanged;

            NetworkListManager.NetworkConnectionConnectivityChanged += NetworkListManager_NetworkConnectionConnectivityChanged;
            NetworkListManager.NetworkConnectionPropertyChanged += NetworkListManager_NetworkConnectionPropertyChanged;
        }

        private void NetworkListManager_ConnectivityChanged(ConnectivityStates NewConnectivity)
        {
            UpdateLog(string.Format("INetworkListManagerEvents.ConnectivityChange: {0}\n", NewConnectivity));
        }

        private void NetworkListManager_NetworkAdded(Guid NetworkId)
        {
            var Network = NetworkListManager.GetNetwork(NetworkId);
            var NetworkName = (Network?.Name ?? NetworkId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkEvents.NetworkAdded: {0}\n", NetworkName));
        }

        private void NetworkListManager_NetworkConnectivityChanged(Guid NetworkId, ConnectivityStates NewConnectivity)
        {
            var Network = NetworkListManager.GetNetwork(NetworkId);
            var NetworkName = (Network?.Name ?? NetworkId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkEvents.NetworkConnectivityChanged: {0}, {1}\n", NetworkName, NewConnectivity));
        }

        private void NetworkListManager_NetworkDeleted(Guid NetworkId)
        {
            var Network = NetworkListManager.GetNetwork(NetworkId);
            var NetworkName = (Network?.Name ?? NetworkId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkEvents.NetworkDeleted: {0}\n", NetworkName));
        }

        private void NetworkListManager_NetworkPropertyChanged(Guid NetworkId, NetworkPropertyChange Flags)
        {
            var Network = NetworkListManager.GetNetwork(NetworkId);
            var NetworkName = (Network?.Name ?? NetworkId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkEvents.NetworkPropertyChanged: {0}, {1}\n", NetworkName, Flags));
        }

        private void NetworkListManager_NetworkConnectionConnectivityChanged(Guid NetworkConnectionId, ConnectivityStates NewConnectivity)
        {
            var NetworkConnection = NetworkListManager.GetNetworkConnection(NetworkConnectionId);
            var NetworkConnectionName = (NetworkConnection?.ConnectionId.ToString() ?? NetworkConnectionId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkConnectionEvents.NetworkConnectionConnectivityChanged: {0}, {1}\n", NetworkConnectionName, NewConnectivity));
        }

        private void NetworkListManager_NetworkConnectionPropertyChanged(Guid NetworkConnectionId, NetworkConnectionPropertyChange Flags)
        {
            var NetworkConnection = NetworkListManager.GetNetworkConnection(NetworkConnectionId);
            var NetworkConnectionName = (NetworkConnection?.ConnectionId.ToString() ?? NetworkConnectionId.ToString() + " (vanished)");
            UpdateLog(string.Format("INetworkConnectionEvents.NetworkConnectionPropertyChanged: {0}, {1}\n", NetworkConnectionName, Flags));
        }

        private void UpdateLog(string Value)
        {
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
              new Action(() => EventLog.Text = EventLog.Text + Value)); ;
        }

        private void AddProperty(string propertyName, string propertyValue, StackPanel parent)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            Label propertyNameLabel = new Label();
            propertyNameLabel.Content = propertyName;
            panel.Children.Add(propertyNameLabel); 

            Label propertyValueLabel = new Label();
            propertyValueLabel.Content = propertyValue;
            panel.Children.Add(propertyValueLabel);

            parent.Children.Add(panel);
        }
    }
}
