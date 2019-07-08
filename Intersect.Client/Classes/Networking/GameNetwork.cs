﻿using System;
using Intersect.Client.Framework.Network;
using Intersect.Client.General;
using Intersect.Client.UI.Menu;
using Intersect.Config;
using Intersect.Configuration;
using Intersect.Logging;
using Intersect.Network;
using Intersect.Network.Packets;

namespace Intersect.Client.Networking
{
    public static class GameNetwork
    {
        public static GameSocket Socket;

        private static bool sConnected;
        public static bool Connecting;

        private static int sPing;
        public static bool Connected => Socket?.IsConnected() ?? sConnected;

        public static int Ping
        {
            get { return Socket?.Ping() ?? sPing; }
            set { sPing = value; }
        }

        public static void InitNetwork()
        {
            if (Socket == null) return;
            Socket.Connected += MySocket_OnConnected;
            Socket.Disconnected += MySocket_OnDisconnected;
            Socket.DataReceived += MySocket_OnDataReceived;
            Socket.ConnectionFailed += MySocket_OnConnectionFailed;
            TryConnect();
        }

        private static void TryConnect()
        {
            sConnected = false;
            MainMenu.OnNetworkConnecting();
            Socket?.Connect(ClientConfiguration.Instance.Host, ClientConfiguration.Instance.Port);
        }

        private static void MySocket_OnConnectionFailed(bool denied)
        {
            sConnected = false;
            if (!denied) TryConnect();
        }

        private static void MySocket_OnDataReceived(IPacket packet)
        {
            PacketHandler.HandlePacket(packet);
        }

        private static void MySocket_OnDisconnected()
        {
            //Not sure how to handle this yet!
            sConnected = false;
            if (Globals.GameState == GameStates.InGame || Globals.GameState == GameStates.Loading)
            {
                Globals.ConnectionLost = true;
                Socket?.Disconnect("");
                TryConnect();
            }
            else
            {
                Socket?.Disconnect("");
                TryConnect();
            }

        }

        private static void MySocket_OnConnected()
        {
            //Not sure how to handle this yet!
            sConnected = true;
        }

        public static void Close(string reason)
        {
            try
            {
                sConnected = false;
                Connecting = false;
                Socket?.Disconnect(reason);
                Socket?.Dispose();
                Socket = null;
            }
            catch (Exception exception)
            {
                Log.Trace(exception);
            }
        }

        public static void SendPacket(CerasPacket packet)
        {
            Socket?.SendPacket(packet);
        }

        public static void Update()
        {
            Socket?.Update();
        }
    }
}