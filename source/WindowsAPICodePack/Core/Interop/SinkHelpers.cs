using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MS.WindowsAPICodePack.Internal
{
    internal abstract class SinkBase 
    {
        private static object ThreadLock = new object();
        private static ArrayList EventSinkHelpers = new ArrayList();

        private IConnectionPoint ConnectionPoint;
        private int Cookie = 0;
        protected Delegate ForwardingDelegate = default(Delegate);


        internal static void AddSink<SinkType>(IConnectionPointContainer Container, Type EventInterface, Delegate @delegate) where SinkType : SinkBase, new()
        {
            lock (ThreadLock)
            {
                var Sink = new SinkType();
                Guid riid = EventInterface.GUID;
                Container.FindConnectionPoint(ref riid, out Sink.ConnectionPoint);

                int Cookie = 0;
                Sink.ConnectionPoint.Advise(Sink, out Cookie);
                Sink.Cookie = Cookie;
                Sink.ForwardingDelegate = @delegate;
                EventSinkHelpers.Add(Sink);
            }
        }


        internal static void RemoveSink<SinkType>(Delegate @delegate) where SinkType : SinkBase
        {
            lock (ThreadLock)
            {
                if (EventSinkHelpers != null)
                {
                    int Count = EventSinkHelpers.Count;
                    int Index = 0;
                    if (0 < Count)
                    {
                        do
                        {
                            SinkType Sink = EventSinkHelpers[Index] as SinkType;
                            if ((Sink != null) && (Sink.ForwardingDelegate != null) && (Sink.ForwardingDelegate.Equals(@delegate)))
                            {
                                EventSinkHelpers.RemoveAt(Index);
                                Sink.UnAdvise();
                                return;
                            }
                            Index++;
                        }
                        while (Index < Count);
                    }
                }
            }
        }


        private void UnAdvise()
        {
            ConnectionPoint.Unadvise(Cookie);
            Marshal.ReleaseComObject(ConnectionPoint);
            ConnectionPoint = null;
            Cookie = 0;
            ForwardingDelegate = default(Delegate);
        }
    }
}
