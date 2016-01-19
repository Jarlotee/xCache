//using System;
//using System.Diagnostics;
//using System.Threading.Tasks;

//namespace xCache.Durable
//{
//    public class InProcessDurableCacheQueue : IDurableCacheQueue
//    {
//        private readonly IDurableCacheRefreshHandler _handler;

//        public InProcessDurableCacheQueue(IDurableCacheRefreshHandler handler)
//        {
//            _handler = handler;
//        }

//        public void ScheduleRefresh(DurableCacheRefreshEvent refreshEvent)
//        {
//            Task.Delay(refreshEvent.RefreshTime)
//                .ContinueWith((t) => _handler.Handle(refreshEvent))
//                .ContinueWith((t) =>
//                {
//                    if (refreshEvent.UtcLifetime > DateTime.UtcNow.Add(refreshEvent.RefreshTime))
//                    {
//                        ScheduleRefresh(refreshEvent);
//                    }
//                    else
//                    {
//                        Trace.TraceInformation("Done refreshing for key {0}", refreshEvent.Key);
//                    }
//                });
//        }
//    }
//}