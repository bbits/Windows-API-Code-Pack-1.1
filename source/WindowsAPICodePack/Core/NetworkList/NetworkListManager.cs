//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Net
{
    /// <summary>
    /// Provides access to objects that represent networks and network connections.
    /// </summary>
    public static class NetworkListManager
    {
        #region Private Fields

        static NetworkListManagerClass manager = new NetworkListManagerClass();

        #endregion // Private Fields

        #region Private Sink classes

        private sealed class NetworkListManagerEventsSink : SinkBase, INetworkListManagerEvents
        {
            public void ConnectivityChanged([In] ConnectivityStates NewConnectivity)
            {
                (ForwardingDelegate as ConnectivityChangedEvent)?.Invoke(NewConnectivity);
            }
        }

        private sealed class NetworkEventsSink : SinkBase, INetworkEvents
        {
            public void NetworkAdded([In] Guid NetworkId)
            {
                (ForwardingDelegate as NetworkAddedEvent)?.Invoke(NetworkId);
            }

            public void NetworkConnectivityChanged([In] Guid NetworkId, [In] ConnectivityStates NewConnectivity)
            {
                (ForwardingDelegate as NetworkConnectivityChangedEvent)?.Invoke(NetworkId, NewConnectivity);
            }

            public void NetworkDeleted([In] Guid NetworkId)
            {
                (ForwardingDelegate as NetworkDeletedEvent)?.Invoke(NetworkId);
            }

            public void NetworkPropertyChanged(Guid NetworkId, NetworkPropertyChange Flags)
            {
                (ForwardingDelegate as NetworkPropertyChangedEvent)?.Invoke(NetworkId, Flags);
            }
        }

        private sealed class NetworkConnectionEventsSink : SinkBase, INetworkConnectionEvents
        {
            public void NetworkConnectionConnectivityChanged([In] Guid NetworkConnectionId, [In] ConnectivityStates NewConnectivity)
            {
                (ForwardingDelegate as NetworkConnectionConnectivityChangedEvent)?.Invoke(NetworkConnectionId, NewConnectivity);
            }

            public void NetworkConnectionPropertyChanged([In] Guid NetworkConnectionId, [In] NetworkConnectionPropertyChange Flags)
            {
                (ForwardingDelegate as NetworkConnectionPropertyChangedEvent)?.Invoke(NetworkConnectionId, Flags);
            }
        }

        #endregion

        #region Event Delegates

        /// <summary>
        /// INetworkListManageEvents ConnectivityChanged delegate
        /// </summary>
        /// <param name="NewConnectivity">New connectivity state</param>
        public delegate void ConnectivityChangedEvent(ConnectivityStates NewConnectivity);

        /// <summary>
        /// INetworkEvents NetworkAdded delegate
        /// </summary>
        /// <param name="NetworkId">Added Network</param>
        public delegate void NetworkAddedEvent(Guid NetworkId);

        /// <summary>
        /// INetworkEvents NetworkConnectivityChanged delegate
        /// </summary>
        /// <param name="NetworkId">Affected Network - not guranteed to still exist</param>
        /// <param name="NewConnectivity">New connectivity state</param>
        public delegate void NetworkConnectivityChangedEvent(Guid NetworkId, ConnectivityStates NewConnectivity);

        /// <summary>
        /// INetworkEvents NetworkDeleted delegate
        /// </summary>
        /// <param name="NetworkId">Deleteed Network - not guranteed to still exist</param>
        public delegate void NetworkDeletedEvent(Guid NetworkId);

        /// <summary>
        /// INetworkEvents NetworkPropertyChanged delegate
        /// </summary>
        /// <param name="NetworkId">Affected Network</param>
        /// <param name="Flags">Properties changed</param>
        public delegate void NetworkPropertyChangedEvent(Guid NetworkId, NetworkPropertyChange Flags);

        /// <summary>
        /// INetworkConnectionEvents NetworkConnectionConnectivityChanged delegate
        /// </summary>
        /// <param name="NetworkConnectionId">Added NetworkConnection</param>
        /// <param name="NewConnectivity">New connectivity state</param>
        public delegate void NetworkConnectionConnectivityChangedEvent(Guid NetworkConnectionId, ConnectivityStates NewConnectivity);

        /// <summary>
        /// INetworkConnectionEvents NetworkConnectionPropertyChanged delegate
        /// </summary>
        /// <param name="NetworkConnectionId">Affected NetworkConnection</param>
        /// <param name="Flags">Properties changed</param>
        public delegate void NetworkConnectionPropertyChangedEvent(Guid NetworkConnectionId, NetworkConnectionPropertyChange Flags);

        #endregion

        #region Events

        /// <summary>
        /// INetworkListManageEvents ConnectivityChanged event
        /// </summary>
        public static event ConnectivityChangedEvent ConnectivityChanged
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkListManagerEventsSink>((IConnectionPointContainer)manager, typeof(INetworkListManagerEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkListManagerEventsSink>(value); }
        }

        /// <summary>
        /// INetworkEvents NetworkAdded event
        /// </summary>
        public static event NetworkAddedEvent NetworkAdded
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkEventsSink>((IConnectionPointContainer)manager, typeof(INetworkEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkEventsSink>(value); }
        }

        /// <summary>
        /// INetworkEvents NetworkConnectivityChanged event
        /// </summary>
        public static event NetworkConnectivityChangedEvent NetworkConnectivityChanged
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkEventsSink>((IConnectionPointContainer)manager, typeof(INetworkEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkEventsSink>(value); }
        }

        /// <summary>
        /// INetworkEvents NetworkDeleted event
        /// </summary>
        public static event NetworkDeletedEvent NetworkDeleted
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkEventsSink>((IConnectionPointContainer)manager, typeof(INetworkEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkEventsSink>(value); }
        }

        /// <summary>
        /// INetworkEvents NetworkPropertyChanged event
        /// </summary>
        public static event NetworkPropertyChangedEvent NetworkPropertyChanged
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkEventsSink>((IConnectionPointContainer)manager, typeof(INetworkEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkEventsSink>(value); }
        }


        /// <summary>
        /// INetworkConnectionEvents NetworkConnectionConnectivityChanged event
        /// </summary>
        public static event NetworkConnectionConnectivityChangedEvent NetworkConnectionConnectivityChanged
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkConnectionEventsSink>((IConnectionPointContainer)manager, typeof(INetworkConnectionEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkConnectionEventsSink>(value); }
        }


        /// <summary>
        /// INetworkConnectionEvents NetworkConnectionPropertyChanged event
        /// </summary>
        public static event NetworkConnectionPropertyChangedEvent NetworkConnectionPropertyChanged
        {
            add
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                SinkBase.AddSink<NetworkConnectionEventsSink>((IConnectionPointContainer)manager, typeof(INetworkConnectionEvents), value);
            }

            remove { SinkBase.RemoveSink<NetworkConnectionEventsSink>(value); }
        }

        #endregion

        /// <summary>
        /// Retrieves a collection of <see cref="Network"/> objects that represent the networks defined for this machine.
        /// </summary>
        /// <param name="level">
        /// The <see cref="NetworkConnectivityLevels"/> that specify the connectivity level of the returned <see cref="Network"/> objects.
        /// </param>
        /// <returns>
        /// A <see cref="NetworkCollection"/> of <see cref="Network"/> objects.
        /// </returns>
        public static NetworkCollection GetNetworks(NetworkConnectivityLevels level)
        {
            // Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista();

            return new NetworkCollection(manager.GetNetworks(level));
        }

        /// <summary>
        /// Retrieves the <see cref="Network"/> identified by the specified network identifier.
        /// </summary>
        /// <param name="networkId">
        /// A <see cref="System.Guid"/> that specifies the unique identifier for the network.
        /// </param>
        /// <returns>
        /// The <see cref="Network"/> that represents the network identified by the identifier.
        /// </returns>
        public static Network GetNetwork(Guid networkId)
        {
            // Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista();

            // Check for invalid or stale
            try
            {
                return new Network(manager.GetNetwork(networkId));
            }
            catch (COMException e) when ((uint)e.ErrorCode == 0x8000FFFF)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a collection of <see cref="NetworkConnection"/> objects that represent the connections for this machine.
        /// </summary>
        /// <returns>
        /// A <see cref="NetworkConnectionCollection"/> containing the network connections.
        /// </returns>
        public static NetworkConnectionCollection GetNetworkConnections()
        {
            // Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista();

            return new NetworkConnectionCollection(manager.GetNetworkConnections());
        }

        /// <summary>
        /// Retrieves the <see cref="NetworkConnection"/> identified by the specified connection identifier.
        /// </summary>
        /// <param name="networkConnectionId">
        /// A <see cref="System.Guid"/> that specifies the unique identifier for the network connection.
        /// </param>
        /// <returns>
        /// The <see cref="NetworkConnection"/> identified by the specified identifier.
        /// </returns>
        public static NetworkConnection GetNetworkConnection(Guid networkConnectionId)
        {
            // Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista();

            // Check for invalid or stale
            INetworkConnection managedConnection = null;
            try
            {
                managedConnection = manager.GetNetworkConnection(networkConnectionId);  
            }
            catch (COMException e) when ((uint)e.ErrorCode == 0x8000FFFF)
            {
                return null;
            }

            // it is possible for INetworkConnection.GetNetworkConnection to succeed and return nothing for a valid Guid (i.e see docs for S_FALSE usage)
            return (managedConnection != null ? new NetworkConnection(managedConnection) : null);
        }

        /// <summary>
        /// Gets a value that indicates whether this machine 
        /// has Internet connectivity.
        /// </summary>
        /// <value>A <see cref="System.Boolean"/> value.</value>
        public static bool IsConnectedToInternet
        {
            get
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                return manager.IsConnectedToInternet;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this machine 
        /// has network connectivity.
        /// </summary>
        /// <value>A <see cref="System.Boolean"/> value.</value>
        public static bool IsConnected
        {
            get
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                return manager.IsConnected;
            }
        }

        /// <summary>
        /// Gets the connectivity state of this machine.
        /// </summary>
        /// <value>A <see cref="Connectivity"/> value.</value>
        public static ConnectivityStates Connectivity
        {
            get
            {
                // Throw PlatformNotSupportedException if the user is not running Vista or beyond
                CoreHelpers.ThrowIfNotVista();

                return manager.GetConnectivity();
            }
        }
    }

}