﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Intersect.Logging;

namespace Intersect.Network
{
    public sealed class NetworkThread
    {
        private readonly PacketDispatcher mDispatcher;
        private bool mStarted;

        public NetworkThread(PacketDispatcher dispatcher, string name = null)
        {
            Name = name ?? "Network Worker Thread";
            CurrentThread = new Thread(Loop);
            Queue = new PacketQueue();
            mDispatcher = dispatcher;
            Connections = new List<IConnection>();
        }

        public string Name { get; }

        public Thread CurrentThread { get; }
        public PacketQueue Queue { get; }
        public IList<IConnection> Connections { get; }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            lock (this)
            {
                if (mStarted) return;
                mStarted = true;
                IsRunning = true;
            }

            CurrentThread.Start();
        }

        public void Stop()
        {
            lock (this)
            {
                IsRunning = false;
            }

            Queue?.Interrupt();
        }

        private void Loop()
        {
            var sw = new Stopwatch();
#if DIAGNOSTIC
            var last = 0L;
#endif
            sw.Start();
            while (IsRunning)
            {
                // ReSharper disable once PossibleNullReferenceException
                if (!Queue.TryNext(out IPacket packet)) continue;

                //Log.Debug($"Dispatching packet '{packet.GetType().Name}' (size={(packet as BinaryPacket)?.Buffer?.Length() ?? -1}).");
                if (!(mDispatcher?.Dispatch(packet) ?? false))
                {
                    Log.Warn($"Failed to dispatch packet '{packet}'.");
                }

#if DIAGNOSTIC
                if (last + (1 * TimeSpan.TicksPerSecond) < sw.ElapsedTicks)
                {
                    last = sw.ElapsedTicks;
                    Console.Title = $"Queue size: {Queue.Size}";
                }
#endif

                packet.Dispose();

                Thread.Yield();
            }
            sw.Stop();

            Log.Debug($"Exiting network thread ({Name}).");
        }
    }
}