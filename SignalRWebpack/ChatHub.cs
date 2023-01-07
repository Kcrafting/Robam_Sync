using Microsoft.AspNetCore.SignalR;
using Robam_Sync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Robam_Sync.SignalRWebpack
{
    [SQLite.Table("SignlRLogs")]
    public class SignlR_Logs
    {
        [SQLite.AutoIncrement,SQLite.PrimaryKey,SQLite.Unique]
        public int ID { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff");
        public string Message { get; set; } = "";
    }
    public interface ITypedHubClient
    {
        //Task NewMessage(SignalR_Paras para);
        Task messageReceived(Sqlite_Models_Result_TableMessage result);
    }
    //public class ChatHub : Microsoft.AspNetCore.SignalR.Hub<ITypedHubClient>
    //{
    //    private readonly ChatHub _ChatHubService;
    //    public static ChatHub staticInstance { get; private set; }
    //    public static void UpdateClientMessage(string billType)
    //    {
    //        Logger.log("into UpdateClientMessage");
    //        if (staticInstance is not null)
    //        {
    //            Logger.log("staticInstance is not null");
    //            staticInstance.NewMessage(new SignalR_Paras() { Token = "", BillType = billType });
    //        }
    //        else
    //        {
    //            Logger.log("staticInstance is null");
    //        }
    //    }
    //    public ChatHub()
    //    {

    //        staticInstance = this;
    //    }
    //    public async Task NewMessage(SignalR_Paras para)
    //    {
    //        Logger.log("into NewMessage");
    //        var ret = para.BillType.ToUpper() switch
    //        {
    //            "INSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() },
    //            "OUTSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() },
    //            "QTXXTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_QTXXTB>().Select(i => i.Format()).ToList() },
    //            "JCZLTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_JCZLTB>().Select(i => i.Format()).ToList() },
    //        };
    //        await Clients.All.SendAsync("messageReceived", ret);
    //        Logger.log("into NewMessage send finish");
    //    }
    //}
    public class ChatHub : Hub<ITypedHubClient>
    {
        public async void NewMessage(SignalR_Paras para)
        {
            Sqlite_Helper_Static.write(new SignlR_Logs() { Message = "方法调用!当前线程" + Thread.CurrentThread.ManagedThreadId.ToString()});
            //Clients.All.NewMessage(para);
            var ret = para.BillType.ToUpper() switch
            {
                "INSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() },
                "OUTSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() },
                "QTXXTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_QTXXTB>().Select(i => i.Format()).ToList() },
                "JCZLTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_JCZLTB>().Select(i => i.Format()).ToList() },
            };
            await Clients.All.messageReceived(ret);
            Sqlite_Helper_Static.write(new SignlR_Logs() { Message = "方法调用完毕!当前线程" + Thread.CurrentThread.ManagedThreadId.ToString() });
        }
    }

}
