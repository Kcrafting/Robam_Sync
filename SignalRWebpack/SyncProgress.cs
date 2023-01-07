
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.SignalRWebpack
{
    public class SyncProgress
    {
        //private readonly static Lazy<SyncProgress> _instance = new Lazy<SyncProgress>(() => new SyncProgress(GlobalHost.ConnectionManager.GetHubContext<SyncProgressHub>().Clients));
        
        //private readonly ConcurrentDictionary<string, SyncProgress> _SyncProgresses = new ConcurrentDictionary<string, SyncProgress>();
        
        //private readonly object _updateStockPricesLock = new object();

        //private SyncProgress(IHubConnectionContext<dynamic> clients)
        //{
        //    Clients = clients;

        //    _SyncProgresses.Clear();
        //    var stocks = new List<SyncProgress>
        //    {
        //        //new Stock { Symbol = "MSFT", Price = 30.31m },
        //        //new Stock { Symbol = "APPL", Price = 578.18m },
        //        //new Stock { Symbol = "GOOG", Price = 570.30m }
        //    };
        //    //stocks.ForEach(stock => _SyncProgresss.TryAdd(stock.Symbol, stock));

        //    //_timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

        //}

        //public static SyncProgress Instance
        //{
        //    get
        //    {
        //        return _instance.Value;
        //    }
        //}

        //private IHubConnectionContext<dynamic> Clients
        //{
        //    get;
        //    set;
        //}
        //public IEnumerable<SyncProgress> GetAllSyncProgress()
        //{
        //    return _SyncProgresses.Values;
        //}
        //private void UpdateStockPrices(object state)
        //{
        //    lock (_updateStockPricesLock)
        //    {
        //        //if (!_updatingStockPrices)
        //        //{
        //        //    _updatingStockPrices = true;

        //        //    foreach (var stock in _stocks.Values)
        //        //    {
        //        //        if (TryUpdateStockPrice(stock))
        //        //        {
        //        //            BroadcastStockPrice(stock);
        //        //        }
        //        //    }

        //        //    _updatingStockPrices = false;
        //        //}
        //    }
        //}

    }
}
