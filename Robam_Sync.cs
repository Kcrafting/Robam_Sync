using Microsoft.AspNetCore.Mvc;
//using Controllers;
//using DataSyncServerFromRobamToKingdee;//.Utils;
using System;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using static Utils.RobamApi;
using Utils;
using Microsoft.Extensions.Configuration;
using Robam_Sync.Paras;
using Robam_Sync.Models;
using Robam_Sync.SignalRWebpack;
using Microsoft.AspNetCore.SignalR;

namespace Robam_Sync
{
    public class Robam_Sync : ControllerBase
    {
        volatile static bool sg_sync_syncinstockbill_Running = false;
        volatile static bool sg_sync_syncoutstockbilllist_Running = false;
        volatile static bool sg_sync_syncrealoutstockbill_Running = false;
        volatile static bool sg_sync_syncpartsoutstockbill_Running = false;
        volatile static bool sg_sync_syncpartsinstockbill_Running = false;

        volatile static bool sg_sync_syncallitem_Running = false;
        volatile static bool sg_sync_syncallitemparts_Running = false;
        volatile static bool sg_sync_syncallishops_Running = false;
        volatile static bool sg_sync_syncreturnback_Running = false;
        static bool K3cloud_Init = false;

        IHubContext<ChatHub, ITypedHubClient> _chatHubContext;
        public Robam_Sync(IHubContext<ChatHub, ITypedHubClient> chatHubContext)
        {
            _chatHubContext = chatHubContext;
            if (!K3cloud_Init)
            {
                K3cloud_Init = true;
                string txt = System.Text.Encoding.UTF8.GetString(Resource.appsettings);
                var obj = JObject.Parse(txt);
                new Thread(() =>
                {
                    KingdeeApi.InitAccount(
                    obj.SelectToken("K3CloudAccount.['Host']").Value<string>(),
                    obj.SelectToken("K3CloudAccount.['AcctId']").Value<string>(),
                    obj.SelectToken("K3CloudAccount.['UserName']").Value<string>(),
                    Utils.Utils.Decode(obj.SelectToken("K3CloudAccount.['Pwd']").Value<string>())
                    );
                })
                { IsBackground = true }.Start();
            }
            
            
        }

        /// <summary>
        /// 采购订单同步
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public delegate void WriteLog(string txt, bool iserror = false);
        public static WriteLog wl_instock { get; set; } = Utils.Utils.RecordStepNew<Sqlite_Models_Instock>;
        public static WriteLog wl_outstock { get; set; } = Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>;
        public static WriteLog wl_qtxxtb { get; set; } = Utils.Utils.RecordStepNew<Sqlite_Models_QTXXTB>;
        public static WriteLog wl_jczldr { get; set; } = Utils.Utils.RecordStepNew<Sqlite_Models_JCZLTB>;
        [HttpPost]
        [Route("api/syncinstockbill")]
        public Sqlite_Models_Result_TableMessage sync_StockList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncinstockbill_Running)
            {
                sg_sync_syncinstockbill_Running = true;
                try
                {
                    Sqlite_Helper_Static.droptable<Sqlite_Models_Instock>();

                    //IActionResult result = new BadRequestObjectResult(new { }) ;


                    KingdeeApi.MatchAccountDo(
                        i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_CP,
                       (acct) =>
                       {
                           wl_instock("开始进行导入,账号" + acct.FAccount);
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                           DateTime _StartDate, _EndDate;
                           if (!DateTime.TryParse(startDate, out _StartDate))
                           {
                               wl_instock("传入的开始日期参数不能转换为日期！", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                               return;
                           }
                           if (!DateTime.TryParse(endDate, out _EndDate))
                           {
                               wl_instock("传入的结束日期参数不能转换为日期", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                               return;
                           }
                           if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                           {
                               wl_instock("日期间隔时间太短！", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                               return;
                           }
                           if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                           {
                               wl_instock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                               return;
                           }
                           var k3cloud = new KingdeeApi();
                           var ins = new RobamApi.Robam_CRM(acct);
                           //var billtypelist = ins.GetInBillType();
                           //if(billtypelist == null)
                           //{

                           //}
                           //确定不导入的单据类型
                           var unsyncbilltype = k3cloud.UnsyncBillType("STK_InStock");
                           if (unsyncbilltype == null)
                           {
                               wl_instock("单据列表获取失败!", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "单据列表获取失败"));
                               return;
                           }
                           wl_instock("开始获取单据列表，对比未同步信息");
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "开始获取单据列表，对比未同步信息"));
                           var instockbill = ins.GetOutstockbill(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (instockbill == null && instockbill.crminvexportheaderss == null)
                           {
                               wl_instock("老板分销系统获取单据失败", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "老板分销系统获取单据失败"));
                               return;
                           }
                           wl_instock("老板分销系统获取单据共计" + (instockbill?.crminvexportheaderss?.Count.ToString() ?? "0") + "条");
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                           var k3billlist = k3cloud.GetSyncedBillNoList(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (k3billlist == null)
                           {
                               wl_instock("获取K3单据列表失败", true);
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "获取K3单据列表失败"));
                               return;
                           }
                           wl_instock("获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条");
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条"));
                           //过滤单据类型导入
                           //if (billtypelist == null)
                           //{
                           //    Utils.Utils.RecordStepNew("获取Robam单据类型失败！", true);
                           //    return;
                           //}
                           //过滤掉不导入的单据
                           var billlists = instockbill.crminvexportheaderss.Where(
                               //i => billtypelist.dataobjs/*.Where(i => i.orderSourceType == "调拨单据")*/.Select(i => i.orderTypeCode).ToList<string>().Contains(i.orderTypeCode)
                               i => !unsyncbilltype.Contains(i.orderTypeCode)
                               ).Select(i => i.orderNo).ToList();
                           wl_instock("筛选需要同步的信息");
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "筛选需要同步的信息"));
                           var unsynced = billlists.Except(k3billlist);
                           wl_instock("未同步单据共计" + (unsynced?.Count().ToString() ?? "0") + "条");
                           wl_instock("需要同步的信息共计" + unsynced.Count().ToString() + "条");
                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                           int _index = 0;
                           int maxCount = 0;
                           maxCount = unsynced.Count();
                           if (unsynced.Count() > 0)
                           {
                               foreach (var bill in unsynced)
                               {
                                   _index++;
                                   wl_instock("第" + _index.ToString() + "条信息，\n");
                                   var exOrderHeadersId = instockbill.crminvexportheaderss.Where(i => i.orderNo == bill).FirstOrDefault().exOrderHeadersId.ToString();
                                   wl_instock("第" + _index.ToString() + "条信息，获取单据详情\n");
                                   var detailbill = ins.GetOutstockbillDetail(exOrderHeadersId);
                                   //分解基础资料
                                   if (detailbill != null)
                                   {
                                       wl_instock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                       if (Utils.Utils.SyncAllItemsFromInStockBill(detailbill, k3cloud, ins, "STK_InStock"))
                                       {
                                           wl_instock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                           //同步单据
                                           //if (k3cloud.SyncPurchaseOrderBill(detailbill, ins, acct.FCompany) == KingdeeApi.SyncResult.AllSuccess)
                                           if (k3cloud.SyncInstockBill(detailbill, ins, acct.FCompany) == KingdeeApi.SyncResult.AllSuccess)
                                           {
                                               wl_instock("第" + _index.ToString() + "条信息，同步单据完成\n");
                                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK",Utils.Utils.GetExecutePercent(_index,maxCount)));
                                           }
                                           else
                                           {
                                               wl_instock("第" + _index.ToString() + "条信息，同步单据失败\n", true);
                                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", Utils.Utils.GetExecutePercent(_index, maxCount)));
                                           }
                                       }
                                       else
                                       {
                                           wl_instock("同步信息时发生错误！\n", true);
                                           _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                                       }
                                   }
                                   else
                                   {
                                       wl_instock("第" + _index.ToString() + "条信息，获取单据详情失败!\n", true);
                                       _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                                   }
                               }
                           }
                           else
                           {
                               wl_instock("选择期间没有未同步单据");
                               _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                               return;
                           }
                       });
                    //return Ok(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Instock>()) });
                      _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK","同步完成!",true));
                    return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i=>i.Format()).ToList() };
                }
                catch (Exception exp)
                {
                    Utils.RobamApi.RecordError(exp.Message);
                    JObject jobj = new JObject();
                    jobj["Error"] = exp.Message;
                    jobj["Message"] = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Instock>());
                    wl_instock("Exception:" + exp.Message, true);
                    return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
                }
                finally
                {
                    sg_sync_syncinstockbill_Running = false;
                }
            }
            else
            {
                _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK", "同步失败!",true));
                return new Sqlite_Models_Result_TableMessage() { rowData = new List<Result_TableMessage_RowData>() { new Result_TableMessage_RowData() { index = 1,isError = true,description = "上次同步未完成!",errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } } };
                //return BadRequest(new { Message = JsonConvert.SerializeObject(new Sqlite_Models_Instock() { index = 1, isError = true, errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), description = "上次同步未完成!" }) });
               
            }

        }
        /// <summary>
        /// 销售订单同步
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/syncoutstockbilllist")]
        public IActionResult sync_OutStockBillList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncoutstockbilllist_Running)
            {
                sg_sync_syncoutstockbilllist_Running = true;

                try
                {
                    /*
                     2022-12-3 同步单据项次
                     */
                    Sqlite_Helper_Static.droptable<Sqlite_Models_Outstock_importSteps>();
                    KingdeeApi.MatchAccountDo(
                      i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_CP,
                     (acct) =>
                     {
                         DateTime _StartDate, _EndDate;
                         if (!DateTime.TryParse(startDate, out _StartDate))
                         {
                             wl_outstock("传入的开始日期参数不能转换为日期！", true);
                             return;
                         }
                         if (!DateTime.TryParse(endDate, out _EndDate))
                         {
                             wl_outstock("传入的结束日期参数不能转换为日期", true);
                             return;
                         }
                         if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                         {
                             wl_outstock("日期间隔时间太短！", true);
                             return;
                         }
                         if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                         {
                             wl_outstock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                             return;
                         }
                         KingdeeApi k3cloud = new KingdeeApi();

                         var unsyncbilltype = k3cloud.UnsyncBillType("PLYE_SaleOrder");
                         if (unsyncbilltype == null)
                         {
                             wl_outstock("单据列表获取失败!", true);
                             return;
                         }

                         var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution)?.FirstOrDefault();
                         if (acc == null)
                         {
                             wl_outstock("获取老板分销系统账户失败！");
                         }
                         var crm = new RobamApi.Robam_CRM(new RobamApi.UserAccount() { FAccount = acct.FAccount, FPWD = acct.FPWD });
                         var robam = new RobamApi.Robam_Distribution(new RobamApi.UserAccount() { FAccount = acc.FAccount, FPWD = acc.FPWD });

                         var list = robam.GetSaleReports(startDate, endDate);
                         //string d1 = JsonConvert.SerializeObject(list);
                         //string js = Utils.Utils.ReadFile(@"F:\Kingdee_Projects\Kingdee_K3_Cloud\开发区老板电器\DataSyncServerFromRobamToKingdee\SaleOrderList.json");
                         //var list = JsonConvert.DeserializeObject<List<DIS_SaleReport>>(js);
                         if (list == null)
                         {
                             wl_outstock("获取销售订单详情失败!", true);
                             return;
                         }
                         if (list.Count <= 0)
                         {
                             wl_outstock("获取销售订单的数量为零，不需要同步!", true);
                             return;
                         }
                         //var list = Sqlite_Helper_Static.read<DIS_SaleReport>();
                         //foreach(var item in list)
                         //{
                         //    Sqlite_Helper_Static.write(item);
                         //}
                         if (Utils.Utils.SyncAllItemsFromOutStockBill(list, k3cloud, crm, robam))
                         {
                             wl_outstock("同步基础资料失败!");
                         }
                         else
                         {
                             wl_outstock("同步基础资料成功!");
                         }
                         wl_outstock("开始组合json");
                         var bills = robam.GetSaleOrder(list, k3cloud);
                         if (bills == null)
                         {
                             wl_outstock("销售订单列表转换JSON失败!", true);
                             return;
                         }
                         if (bills.Count <= 0)
                         {
                             wl_outstock("销售订单列表转换JSON后数量为零，不需要同步!", true);
                             return;
                         }
                         var existsbilllist = k3cloud.GetSyncedBillNoList(startDate, endDate, "PLYE_SaleOrder");
                         var needimport = bills.Where(i => !existsbilllist.Contains(i.Model.FBillNo) && !unsyncbilltype.Contains(i.Model.FBillTypeID.FNumber)).ToList();
                         if (needimport == null)
                         {
                             wl_outstock("销售订单除重后为null!", true);
                             return;
                         }
                         if (needimport.Count <= 0)
                         {
                             wl_outstock("销售订单已经全部导入不需要同步!", true);
                             return;
                         }
                         foreach (var item in needimport)
                         {
                             if (k3cloud.SyncSaleOrderBill(item) != KingdeeApi.SyncResult.AllSuccess)
                             {
                                 wl_outstock(item.Model.FBillNo + "同步单据失败！", true);
                             }
                             else
                             {
                                 wl_outstock(item.Model.FBillNo + "同步单据成功！");
                             }
                         }

                     });

                }
                catch (Exception exp)
                {
                    RobamApi.RecordError(exp.Message);
                    Console.WriteLine(exp.Message);

                    wl_outstock("Exception:" + exp.Message, true);
                }
                finally
                {
                    sg_sync_syncoutstockbilllist_Running = false;
                }
                return Ok(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Outstock_importSteps>()) });
            }
            else
            {
                return BadRequest(new { Message = JsonConvert.SerializeObject(new Sqlite_Models_Outstock_importSteps() { FID = 1, IsError = true, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message = "上次同步未完成!" }) });
            }

        }
        /// <summary>
        /// 出库单同步
        /// </summary>
        /// <param name="startDate">开始导入日期</param>
        /// <param name="endDate">结束导入日期</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/getsaleordernowithinstockno")]
        public IActionResult sync_getSaleOrderNoWithStockNo([FromForm] string stockbillno)
        {
            try
            {
                if (String.IsNullOrEmpty(stockbillno))
                {
                    return Ok(new
                    {
                        IsSuccess = false,
                        SaleOrderNo = "",
                        Message = "错误,出库单单据编号参数为空"
                    });
                }
                var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == BusinessType.Robam_CP)?.FirstOrDefault();
                var crm = new RobamApi.Robam_CRM(new RobamApi.UserAccount() { FAccount = acc.FAccount, FPWD = acc.FPWD });
                var disacc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution && i.BusinessType == BusinessType.Robam_CP)?.FirstOrDefault();
                var dis = new RobamApi.Robam_Distribution(new RobamApi.UserAccount() { FAccount = disacc.FAccount, FPWD = disacc.FPWD });
                if (stockbillno.Length > 50)
                {
                    return Ok(new
                    {
                        IsSuccess = false,
                        SaleOrderNo = "",
                        Message = "错误,单据编号不符合规则"
                    });
                }
                var ins = crm.GetRealStockBill(stockbillno);
                if (ins?.crminvexportheaderss != null && ins?.crminvexportheaderss.Count > 0 && ins?.crminvexportheaderss[0]?.fxOrderNo != null)
                {
                    //查询现在系统里面有没有该单子 并确定审核状态
                    var k3cloud = new KingdeeApi();
                    var k3billlist = k3cloud.GetSyncedBillNoList2(ins.crminvexportheaderss[0].fxOrderNo, "PLYE_SaleOrder");

                    if (k3billlist != null && k3billlist.Count <= 0)
                    {

                        //单据未找到需要同步
                        if (!k3cloud.GetSalerOrderBillNo(ins.crminvexportheaderss[0].fxOrderNo))
                        {
                            //var b2 = dis.GetSaleorderDetail(ins.crminvexportheaderss[0].fxOrderNo);
                            //var b2 = dis.GetSaleOrder();
                            var list = dis.GetSaleReports(ins.crminvexportheaderss[0].fxOrderNo);
                            if (list != null && list.Count > 0)
                            {
                                var bills = dis.GetSaleOrder(list, k3cloud);
                                //var existsbilllist = k3cloud.GetSyncedBillNoList(startDate, endDate, "PLYE_SaleOrder");
                                var needimport = bills;//bills.Where(i => !existsbilllist.Contains(i.Model.FBillNo) && !unsyncbilltype.Contains(i.Model.FBillTypeID.FNumber)).ToList();
                                if (needimport != null)
                                {

                                    if (needimport.Count > 0)
                                    {
                                        foreach (var item in needimport)
                                        {
                                            if (k3cloud.SyncSaleOrderBill(item) != KingdeeApi.SyncResult.AllSuccess)
                                            {
                                                Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "同步单据失败！", true);
                                                return Ok(new
                                                {
                                                    IsSuccess = false,
                                                    SaleOrderNo = "",
                                                    Message = "错误,尝试同步单据失败"
                                                });
                                            }
                                            else
                                            {
                                                Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "同步单据成功！");
                                                //提交审核单据
                                                if (k3cloud.SubmitBill("PLYE_SaleOrder", item.Model.FBillNo))
                                                {
                                                    if (k3cloud.AuditBill("PLYE_SaleOrder", item.Model.FBillNo))
                                                    {
                                                        return Ok(new
                                                        {
                                                            IsSuccess = true,
                                                            SaleOrderNo = ins?.crminvexportheaderss[0].fxOrderNo,
                                                            Message = "查询成功"
                                                        });
                                                    }
                                                    else
                                                    {
                                                        Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "审核单据失败！", true);
                                                        return Ok(new
                                                        {
                                                            IsSuccess = false,
                                                            SaleOrderNo = "",
                                                            Message = "错误,尝试审核同步的单据失败"
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "提交单据失败！", true);
                                                    return Ok(new
                                                    {
                                                        IsSuccess = false,
                                                        SaleOrderNo = "",
                                                        Message = "错误,尝试提交同步的单据失败"
                                                    });
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Utils.Utils.RecordStepForOutStock("销售订单已经全部导入不需要同步!", true);
                                        return Ok(new
                                        {
                                            IsSuccess = false,
                                            SaleOrderNo = "",
                                            Message = "错误,尝试同步销售订单发现已存在"
                                        });
                                    }
                                }
                                else
                                {
                                    Utils.Utils.RecordStepForOutStock("销售订单除重后为null!", true);
                                    return Ok(new
                                    {
                                        IsSuccess = false,
                                        SaleOrderNo = "",
                                        Message = "错误,尝试同步销售订单发现单据有问题"
                                    });
                                }
                            }
                            else
                            {
                                Utils.Utils.RecordStepForRealoutStock("销售单据查询无结果\n", true);
                                return Ok(new
                                {
                                    IsSuccess = false,
                                    SaleOrderNo = "",
                                    Message = "错误,尝试同步销售订单发现查询无结果!"
                                });
                            }
                        }
                        else
                        {

                            Utils.Utils.RecordStepForRealoutStock("销售订单存在！无需同步！\n");
                            return Ok(new
                            {
                                IsSuccess = false,
                                SaleOrderNo = "",
                                Message = "错误,销售订单存在无需同步"
                            });
                        }
                    }
                    else if (k3billlist == null)
                    {
                        Utils.Utils.RecordStepForRealoutStock("获取K3单据列表失败", true);
                        return Ok(new
                        {
                            IsSuccess = false,
                            SaleOrderNo = "",
                            Message = "错误,获取金蝶云星空销售订单列表失败1"
                        });
                    }
                    if (k3billlist[0] == null || k3billlist[0][1] == null)
                    {
                        return Ok(new
                        {
                            IsSuccess = false,
                            SaleOrderNo = "",
                            Message = "错误,获取金蝶云星空销售订单列表失败2"
                        });
                    }
                    if (k3billlist[0][1] != "C")
                    {
                        //单子没有审核，尝试审核
                        if (k3cloud.SubmitBill("PLYE_SaleOrder", ins.crminvexportheaderss[0].fxOrderNo))
                        {
                            if (k3cloud.AuditBill("PLYE_SaleOrder", ins.crminvexportheaderss[0].fxOrderNo))
                            {
                                return Ok(new
                                {
                                    IsSuccess = true,
                                    SaleOrderNo = ins?.crminvexportheaderss[0].fxOrderNo,
                                    Message = "查询成功"
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    IsSuccess = false,
                                    SaleOrderNo = "",
                                    Message = "错误,尝试审核现有销售订单失败"
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                IsSuccess = false,
                                SaleOrderNo = "",
                                Message = "错误,尝试提交现有销售订失败!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            IsSuccess = true,
                            SaleOrderNo = ins?.crminvexportheaderss[0].fxOrderNo,
                            Message = "查询成功"
                        });
                    }

                }
                else
                {
                    return Ok(new
                    {
                        IsSuccess = false,
                        SaleOrderNo = "",
                        Message = "错误,出库单号没有查询到对应销售订单"
                    });
                }
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
                return Ok(new
                {
                    IsSuccess = false,
                    SaleOrderNo = "",
                    Message = "错误，" + exp.Message
                });
            }
            finally
            {

            }
            return Ok(new
            {
                IsSuccess = false,
                SaleOrderNo = "",
                Message = "错误，未知错误!"
            });
        }
        [HttpPost]
        [Route("api/syncrealoutstockbill")]
        public IActionResult sync_RealOutstockList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncrealoutstockbill_Running)
            {
                sg_sync_syncrealoutstockbill_Running = true;
                try
                {
                    Sqlite_Helper_Static.droptable<Sqlite_Models_Realoutstock_importStes>();

                    //IActionResult result = new BadRequestObjectResult(new { }) ;

                    KingdeeApi.MatchAccountDo(
                        i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_CP,
                       (acct) =>
                       {

                           Utils.Utils.RecordStepForRealoutStock("开始进行导入,账号" + acct.FAccount);
                           DateTime _StartDate, _EndDate;
                           if (!DateTime.TryParse(startDate, out _StartDate))
                           {
                               Utils.Utils.RecordStepForRealoutStock("传入的开始日期参数不能转换为日期！", true);
                               return;
                           }
                           if (!DateTime.TryParse(endDate, out _EndDate))
                           {
                               Utils.Utils.RecordStepForRealoutStock("传入的结束日期参数不能转换为日期", true);
                               return;
                           }
                           if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                           {
                               Utils.Utils.RecordStepForRealoutStock("日期间隔时间太短！", true);
                               return;
                           }
                           if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                           {
                               Utils.Utils.RecordStepForRealoutStock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                               return;
                           }
                           var k3cloud = new KingdeeApi();
                           var ins = new RobamApi.Robam_CRM(acct);
                           var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution)?.FirstOrDefault();
                           var dis = new RobamApi.Robam_Distribution(new RobamApi.UserAccount() { FAccount = acc.FAccount, FPWD = acc.FPWD });
                           //var billtypelist = ins.GetInBillType();
                           //确定不导入的单据类型
                           var unsyncbilltype = k3cloud.UnsyncBillType("SAL_OUTSTOCK");
                           Utils.Utils.RecordStepForRealoutStock("开始获取单据列表，对比未同步信息");
                           var instockbill = ins.GetRealStockBill(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (instockbill == null)
                           {
                               Utils.Utils.RecordStepForRealoutStock("老板分销系统获取单据失败", true);
                               return;
                           }
                           Utils.Utils.RecordStepForRealoutStock("老板分销系统获取单据共计" + (instockbill?.crminvexportheaderss?.Count.ToString() ?? "0") + "条");

                           var k3billlist = k3cloud.GetSyncedBillNoList(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"), "SAL_OUTSTOCK");
                           if (k3billlist == null)
                           {
                               Utils.Utils.RecordStepForRealoutStock("获取K3单据列表失败", true);
                               return;
                           }
                           Utils.Utils.RecordStepForRealoutStock("获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条");
                           //过滤单据类型导入
                           //if (billtypelist == null)
                           //{
                           //    Utils.Utils.RecordStepForRealoutStock("获取Robam单据类型失败！", true);
                           //    return;
                           //}
                           //过滤掉不导入的单据
                           var billlists = instockbill.crminvexportheaderss.Where(
                               //i => billtypelist.dataobjs/*.Where(i => i.orderSourceType == "调拨单据")*/.Select(i => i.orderTypeCode).ToList<string>().Contains(i.orderTypeCode)
                               i => !unsyncbilltype.Contains(i.orderTypeCode)
                               ).Select(i => i.orderNo);
                           Utils.Utils.RecordStepForRealoutStock("筛选需要同步的信息");

                           var unsynced = billlists.Except(k3billlist);
                           Utils.Utils.RecordStepForRealoutStock("未同步单据共计" + (unsynced?.Count().ToString() ?? "0") + "条");
                           Utils.Utils.RecordStepForRealoutStock("需要同步的信息共计" + unsynced.Count().ToString() + "条");
                           int _index = 0;
                           if (unsynced.Count() > 0)
                           {
                               foreach (var bill in unsynced)
                               {
                                   _index++;
                                   Utils.Utils.RecordStepForRealoutStock("第" + _index.ToString() + "条信息，\n");
                                   var exOrderHeadersId = instockbill.crminvexportheaderss.Where(i => i.orderNo == bill).FirstOrDefault().exOrderHeadersId.ToString();
                                   Utils.Utils.RecordStepForRealoutStock("第" + _index.ToString() + "条信息，获取单据详情\n");
                                   //获取到出库单
                                   var detailbill = ins.GetRealStockBillDetail(exOrderHeadersId);
                                   //分解基础资料
                                   if (detailbill != null)
                                   {
                                       Utils.Utils.RecordStepForRealoutStock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                       if (Utils.Utils.SyncAllItemsFromInStockBill(detailbill, k3cloud, ins, "SAL_OUTSTOCK"))
                                       {
                                           Utils.Utils.RecordStepForRealoutStock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                           //判断销售订单是否存在
                                           bool salebillExists = false;
                                           if (!k3cloud.GetSalerOrderBillNo(detailbill.crminvexportheaders.fxOrderNo))
                                           {
                                               //var b2 = dis.GetSaleorderDetail(detailbill.crminvexportheaders.fxOrderNo);
                                               //var b2 = dis.GetSaleOrder();
                                               var list = dis.GetSaleReports(detailbill.crminvexportheaders.fxOrderNo);
                                               if (list != null && list.Count > 0)
                                               {
                                                   var bills = dis.GetSaleOrder(list, k3cloud);
                                                   var existsbilllist = k3cloud.GetSyncedBillNoList(startDate, endDate, "PLYE_SaleOrder");
                                                   var needimport = bills.Where(i => !existsbilllist.Contains(i.Model.FBillNo) && !unsyncbilltype.Contains(i.Model.FBillTypeID.FNumber)).ToList();
                                                   if (needimport != null)
                                                   {
                                                       if (needimport.Count > 0)
                                                       {
                                                           foreach (var item in needimport)
                                                           {
                                                               if (k3cloud.SyncSaleOrderBill(item) != KingdeeApi.SyncResult.AllSuccess)
                                                               {
                                                                   Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "同步单据失败！", true);
                                                               }
                                                               else
                                                               {
                                                                   Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "同步单据成功！");
                                                                   //提交审核单据
                                                                   if (k3cloud.SubmitBill("PLYE_SaleOrder", item.Model.FBillNo))
                                                                   {
                                                                       if (k3cloud.AuditBill("PLYE_SaleOrder", item.Model.FBillNo))
                                                                       {
                                                                           salebillExists = true;
                                                                           //同步单据 改 ： 下推单据
                                                                       }
                                                                       else
                                                                       {
                                                                           Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "审核单据失败！", true);
                                                                       }
                                                                   }
                                                                   else
                                                                   {
                                                                       Utils.Utils.RecordStepForOutStock(item.Model.FBillNo + "提交单据失败！", true);
                                                                   }
                                                               }
                                                           }
                                                       }
                                                       else
                                                       {
                                                           Utils.Utils.RecordStepForOutStock("销售订单已经全部导入不需要同步!", true);
                                                       }
                                                   }
                                                   else
                                                   {
                                                       Utils.Utils.RecordStepForOutStock("销售订单除重后为null!", true);
                                                   }
                                               }
                                               else
                                               {
                                                   Utils.Utils.RecordStepForRealoutStock("销售单据查询无结果\n", true);
                                               }
                                           }
                                           else
                                           {
                                               salebillExists = true;
                                               Utils.Utils.RecordStepForRealoutStock("销售订单存在！无需同步！\n");
                                           }
                                           //销售单据检测完毕
                                           if (salebillExists)
                                           {
                                               var osbill = k3cloud.TranslateCRMOutStockToKingdeeOutStockBill(detailbill, acct.FCompany);
                                               if (osbill == null || k3cloud.SyncRealOutStockBill(osbill) != KingdeeApi.SyncResult.AllSuccess)
                                               {
                                                   Utils.Utils.RecordStepForRealoutStock("出库单创建时发生错误！\n");
                                               }
                                           }
                                       }
                                       else
                                       {
                                           Utils.Utils.RecordStepForRealoutStock("同步信息时发生错误！\n", true);
                                       }
                                   }
                                   else
                                   {
                                       Utils.Utils.RecordStepForRealoutStock("第" + _index.ToString() + "条信息，获取单据详情失败!\n", true);
                                   }
                               }
                           }
                           else
                           {
                               Utils.Utils.RecordStepForRealoutStock("选择期间没有未同步单据");
                           }
                       });
                    return Ok(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Realoutstock_importStes>()) });
                }
                catch (Exception exp)
                {
                    RobamApi.RecordError(exp.Message);
                    JObject jobj = new JObject();
                    jobj["Error"] = exp.Message;
                    jobj["Message"] = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Realoutstock_importStes>());
                    Utils.Utils.RecordStepForRealoutStock("Exception:" + exp.Message, true);
                    return BadRequest(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Realoutstock_importStes>()) });
                }
                finally
                {
                    sg_sync_syncrealoutstockbill_Running = false;
                }
            }
            else
            {
                return BadRequest(new { Message = JsonConvert.SerializeObject(new Sqlite_Models_Realoutstock_importStes() { FID = 1, IsError = true, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message = "上次同步未完成!" }) });
            }
        }
        /// <summary>
        /// 配件入库同步
        /// </summary>
        /// <param name="startDate">开始导入日期</param>
        /// <param name="endDate">结束导入日期</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/syncpartsoutstockbill")]
        public IActionResult sync_PartsOutstockList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncpartsoutstockbill_Running)
            {
                sg_sync_syncpartsoutstockbill_Running = true;
                try
                {
                    Sqlite_Helper_Static.droptable<Sqlite_Models_PartsOutstock_importStep>();


                    KingdeeApi.MatchAccountDo(
                        i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ,
                       (acct) =>
                       {
                           wl_outstock("开始进行导入");
                           DateTime _StartDate, _EndDate;
                           if (!DateTime.TryParse(startDate, out _StartDate))
                           {
                               wl_outstock("传入的开始日期参数不能转换为日期！", true);
                               return;
                           }
                           if (!DateTime.TryParse(endDate, out _EndDate))
                           {
                               wl_outstock("传入的结束日期参数不能转换为日期", true);
                               return;
                           }
                           if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                           {
                               wl_outstock("日期间隔时间太短！", true);
                               return;
                           }
                           if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                           {
                               wl_outstock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                               return;
                           }

                           var k3cloud = new KingdeeApi();
                           var ins = new RobamApi.Robam_CRM(acct);
                           //var billtypelist = ins.GetInBillType();
                           //确定不导入的单据类型
                           var unsyncbilltype = k3cloud.UnsyncBillType("PLYE_PartsOutstock");
                           wl_outstock("开始获取单据列表，对比未同步信息");
                           if (unsyncbilltype == null)
                           {
                               wl_outstock("获取已同步单据类型错误!", true);
                               return;
                           }

                           var buy_instockbill = ins.GetPartsOutstockBill(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (buy_instockbill == null)
                           {
                               wl_outstock("老板分销系统获取单据失败", true);
                               return;
                           }
                           else
                           {
                               if (buy_instockbill?.crminvexportheaderss == null || buy_instockbill?.crminvexportheaderss.Count == 0)
                               {
                                   wl_outstock("获取老板系统单据数量为0！", true);
                                   return;
                               }
                           }
                           wl_outstock("老板分销系统获取单据共计" + (buy_instockbill?.crminvexportheaderss?.Count.ToString() ?? "0") + "条");

                           var k3billlist = k3cloud.GetSyncedBillNoList(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"), "PLYE_PartsOutstock");

                           if (k3billlist == null)
                           {
                               wl_outstock("获取K3单据列表失败", true);
                               return;
                           }
                           wl_outstock("获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条");
                           //过滤单据类型导入
                           //if (billtypelist == null)
                           //{
                           //    wl_outstock("获取Robam单据类型失败！", true);
                           //    return;
                           //}
                           //过滤掉不导入的单据
                           var billlists = buy_instockbill.crminvexportheaderss.Where(
                               //i => billtypelist.dataobjs/*.Where(i => i.orderSourceType == "调拨单据")*/.Select(i => i.orderTypeCode).ToList<string>().Contains(i.orderTypeCode)
                               i => !unsyncbilltype.Contains(i.orderTypeCode)
                               ).Select(i => i.orderNo);
                           wl_outstock("筛选需要同步的信息");

                           var unsynced = billlists.Except(k3billlist);
                           wl_outstock("未同步单据共计" + (unsynced?.Count().ToString() ?? "0") + "条");
                           wl_outstock("需要同步的信息共计" + unsynced.Count().ToString() + "条");
                           int _index = 0;
                           if (unsynced.Count() > 0)
                           {
                               foreach (var bill in unsynced)
                               {
                                   _index++;
                                   wl_outstock("第" + _index.ToString() + "条信息，\n");
                                   var exOrderHeadersId = buy_instockbill.crminvexportheaderss.Where(i => i.orderNo == bill).FirstOrDefault().exOrderHeadersId.ToString();
                                   wl_outstock("第" + _index.ToString() + "条信息，获取单据详情\n");
                                   var detailbill = ins.GetOutstockbillDetail(exOrderHeadersId);
                                   //分解基础资料
                                   if (detailbill != null)
                                   {
                                       wl_outstock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                       if (Utils.Utils.SyncAllItemsFromInStockBill(detailbill, k3cloud, ins, "SAL_OUTSTOCK"))
                                       {
                                           wl_outstock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                           //同步单据
                                           if (k3cloud.SyncPartsOutstockBill(detailbill, ins, acct.FCompany) == KingdeeApi.SyncResult.AllSuccess)
                                           {
                                               wl_outstock("第" + _index.ToString() + "条信息，同步单据完成\n");
                                           }
                                       }
                                       else
                                       {
                                           wl_outstock("同步信息时发生错误！\n", true);
                                       }
                                   }
                                   else
                                   {
                                       wl_outstock("第" + _index.ToString() + "条信息，获取单据详情失败!\n", true);
                                   }
                               }
                           }
                           else
                           {
                               wl_outstock("选择期间没有未同步单据");
                           }
                       });
                    return Ok(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsOutstock_importStep>()) });
                }
                catch (Exception exp)
                {
                    RobamApi.RecordError(exp.Message);
                    JObject jobj = new JObject();
                    jobj["Error"] = exp.Message;
                    jobj["Message"] = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsOutstock_importStep>());
                    wl_outstock("Exception:" + exp.Message, true);
                    return BadRequest(new { Message = jobj.ToString() });
                }
                finally
                {
                    sg_sync_syncpartsoutstockbill_Running = false;
                }
            }
            else
            {
                return BadRequest(new { Message = JsonConvert.SerializeObject(new Sqlite_Models_PartsOutstock_importStep() { FID = 1, IsError = true, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message = "上次同步未完成!" }) });
            }
        }
        /// <summary>
        /// 配件出库同步
        /// </summary>
        /// <param name="startDate">开始导入日期</param>
        /// <param name="endDate">结束导入日期</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/syncpartsinstockbill")]
        public IActionResult sync_PartsInstockList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncpartsinstockbill_Running)
            {
                sg_sync_syncpartsinstockbill_Running = true;
                try
                {
                    Sqlite_Helper_Static.droptable<Sqlite_Models_PartsInstock_importStep>();

                    //IActionResult result = new BadRequestObjectResult(new { }) ;

                    KingdeeApi.MatchAccountDo(
                        i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ,
                       (acct) =>
                       {
                           wl_instock("开始进行导入");
                           DateTime _StartDate, _EndDate;
                           if (!DateTime.TryParse(startDate, out _StartDate))
                           {
                               wl_instock("传入的开始日期参数不能转换为日期！", true);
                               return;
                           }
                           if (!DateTime.TryParse(endDate, out _EndDate))
                           {
                               wl_instock("传入的结束日期参数不能转换为日期", true);
                               return;
                           }
                           if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                           {
                               wl_instock("日期间隔时间太短！", true);
                               return;
                           }
                           if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                           {
                               Utils.Utils.RecordStepForPartsOutstock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                               return;
                           }
                           var k3cloud = new KingdeeApi();
                           var ins = new RobamApi.Robam_CRM(acct);
                           //var billtypelist = ins.GetInBillType();
                           //确定不导入的单据类型
                           var unsyncbilltype = k3cloud.UnsyncBillType("PLYE_PartsInstock");
                           if (unsyncbilltype == null)
                           {
                               Utils.Utils.RecordStepForPartsOutstock("获取已同步单据类型错误!", true);
                               return;
                           }
                           wl_instock("开始获取单据列表，对比未同步信息");
                           var instockbill = ins.GetPartsInstockBill(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (instockbill == null)
                           {
                               wl_instock("老板分销系统获取单据失败", true);
                               return;
                           }
                           wl_instock("老板分销系统获取单据共计" + (instockbill?.crminvexportheaderss?.Count.ToString() ?? "0") + "条");
                           //获取 配件入库订单单号
                           var k3billlist = k3cloud.GetSyncedBillNoList(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"), "STK_InStock");
                           if (k3billlist == null)
                           {
                               wl_instock("获取K3单据列表失败", true);
                               return;
                           }
                           wl_instock("获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条");
                           //过滤单据类型导入
                           //if (billtypelist == null)
                           //{
                           //    wl_instock("获取Robam单据类型失败！", true);
                           //    return;
                           //}
                           //过滤掉不导入的单据类型
                           var billlists = instockbill.crminvexportheaderss.Where(
                               //i => billtypelist.dataobjs/*.Where(i => i.orderSourceType == "调拨单据")*/.Select(i => i.orderTypeCode).ToList<string>().Contains(i.orderTypeCode)
                               i => !unsyncbilltype.Contains(i.orderTypeCode)
                               ).Select(i => i.orderNo);
                           wl_instock("筛选需要同步的信息");

                           var unsynced = billlists.Except(k3billlist);
                           wl_instock("未同步单据共计" + (unsynced?.Count().ToString() ?? "0") + "条");
                           wl_instock("需要同步的信息共计" + unsynced.Count().ToString() + "条");
                           int _index = 0;
                           if (unsynced.Count() > 0)
                           {
                               foreach (var bill in unsynced)
                               {
                                   _index++;
                                   wl_instock("第" + _index.ToString() + "条信息，\n");
                                   var exOrderHeadersId = instockbill.crminvexportheaderss.Where(i => i.orderNo == bill).FirstOrDefault().exOrderHeadersId.ToString();
                                   wl_instock("第" + _index.ToString() + "条信息，获取单据详情\n");
                                   var detailbill = ins.GetOutstockbillDetail(exOrderHeadersId);
                                   //分解基础资料
                                   if (detailbill != null)
                                   {
                                       wl_instock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                       if (Utils.Utils.SyncAllItemsFromInStockBill(detailbill, k3cloud, ins, "STK_InStock"))
                                       {
                                           wl_instock("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                           //同步单据
                                           if (k3cloud.SyncPartsInstockBill(detailbill, ins, acct.FCompany) == KingdeeApi.SyncResult.AllSuccess)
                                           {
                                               wl_instock("第" + _index.ToString() + "条信息，同步单据完成\n");
                                           }
                                       }
                                       else
                                       {
                                           wl_instock("同步信息时发生错误！\n", true);
                                       }
                                   }
                                   else
                                   {
                                       wl_instock("第" + _index.ToString() + "条信息，获取单据详情失败!\n", true);
                                   }
                               }
                           }
                           else
                           {
                               wl_instock("选择期间没有未同步单据");
                           }
                       });
                    var s = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsInstock_importStep>());
                    return Ok(new { Message = s });


                }
                catch (Exception exp)
                {
                    RobamApi.RecordError(exp.Message);
                    JObject jobj = new JObject();
                    jobj["Error"] = exp.Message;
                    jobj["Message"] = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsInstock_importStep>());
                    wl_instock("Exception:" + exp.Message, true);
                    return BadRequest(new { Message = jobj.ToString() });
                }
                finally
                {
                    sg_sync_syncpartsinstockbill_Running = false;
                }
            }
            else
            {
                return BadRequest(new { Message = JsonConvert.SerializeObject(new Sqlite_Models_PartsInstock_importStep() { FID = 1, IsError = true, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message = "上次同步未完成!" }) });
            }
        }
        [HttpPost]
        [Route("api/syncpartsretuenbackbill")]
        public IActionResult sync_PartsreturnbackList([FromForm] string startDate, string endDate)
        {
            if (!sg_sync_syncreturnback_Running)
            {
                sg_sync_syncreturnback_Running = true;
                try
                {
                    Sqlite_Helper_Static.droptable<Sqlite_Models_PartsInstock_importStep>();

                    //IActionResult result = new BadRequestObjectResult(new { }) ;

                    KingdeeApi.MatchAccountDo(
                        i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ,
                       (acct) =>
                       {
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("开始进行导入");
                           DateTime _StartDate, _EndDate;
                           if (!DateTime.TryParse(startDate, out _StartDate))
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("传入的开始日期参数不能转换为日期！", true);
                               return;
                           }
                           if (!DateTime.TryParse(endDate, out _EndDate))
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("传入的结束日期参数不能转换为日期", true);
                               return;
                           }
                           if (!(_EndDate - _StartDate > TimeSpan.FromHours(6)))
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("日期间隔时间太短！", true);
                               return;
                           }
                           if (_EndDate - _StartDate > TimeSpan.FromDays(2))
                           {
                               Utils.Utils.RecordStepForPartsOutstock("日期间隔时间不能超过2天，否则将导致导入时间过长!", true);
                               return;
                           }
                           var k3cloud = new KingdeeApi();
                           var ins = new RobamApi.Robam_CRM(acct);
                           //var billtypelist = ins.GetInBillType();
                           //确定不导入的单据类型
                           var unsyncbilltype = k3cloud.UnsyncBillType("PLYE_PartsInstock");
                           if (unsyncbilltype == null)
                           {
                               Utils.Utils.RecordStepForPartsOutstock("获取已同步单据类型错误!", true);
                               return;
                           }
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("开始获取单据列表，对比未同步信息");
                           var instockbill = ins.GetPartsReturnBack(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                           if (instockbill == null)
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("老板分销系统获取单据失败", true);
                               return;
                           }
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("老板分销系统获取单据共计" + (instockbill?.crminvexportheaderss?.Count.ToString() ?? "0") + "条");
                           //获取 配件入库订单单号
                           var k3billlist = k3cloud.GetSyncedBillNoList(_StartDate.ToString("yyyy-MM-dd HH:mm:ss"), _EndDate.ToString("yyyy-MM-dd HH:mm:ss"), "STK_InStock");
                           if (k3billlist == null)
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("获取K3单据列表失败", true);
                               return;
                           }
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("获取K3单据列表共计" + (k3billlist?.Count.ToString() ?? "0") + "条");
                           //过滤单据类型导入
                           //if (billtypelist == null)
                           //{
                           //    Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("获取Robam单据类型失败！", true);
                           //    return;
                           //}
                           //过滤掉不导入的单据类型
                           var billlists = instockbill.crminvexportheaderss.Where(
                               //i => billtypelist.dataobjs/*.Where(i => i.orderSourceType == "调拨单据")*/.Select(i => i.orderTypeCode).ToList<string>().Contains(i.orderTypeCode)
                               i => !unsyncbilltype.Contains(i.orderTypeCode)
                               ).Select(i => i.orderNo);
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("筛选需要同步的信息");

                           var unsynced = billlists.Except(k3billlist);
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("未同步单据共计" + (unsynced?.Count().ToString() ?? "0") + "条");
                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("需要同步的信息共计" + unsynced.Count().ToString() + "条");
                           int _index = 0;
                           if (unsynced.Count() > 0)
                           {
                               foreach (var bill in unsynced)
                               {
                                   _index++;
                                   Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，\n");
                                   var exOrderHeadersId = instockbill.crminvexportheaderss.Where(i => i.orderNo == bill).FirstOrDefault().exOrderHeadersId.ToString();
                                   Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，获取单据详情\n");
                                   var detailbill = ins.GetOutstockbillDetail(exOrderHeadersId);
                                   //分解基础资料
                                   if (detailbill != null)
                                   {
                                       Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                       if (Utils.Utils.SyncAllItemsFromInStockBill(detailbill, k3cloud, ins, "STK_InStock"))
                                       {
                                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，同步单据基础资料\n");
                                           //同步单据
                                           if (k3cloud.SyncPartsRerurnBack(detailbill, ins, acct.FCompany) == KingdeeApi.SyncResult.AllSuccess)
                                           {
                                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，同步单据完成\n");
                                           }
                                       }
                                       else
                                       {
                                           Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("同步信息时发生错误！\n", true);
                                       }
                                   }
                                   else
                                   {
                                       Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("第" + _index.ToString() + "条信息，获取单据详情失败!\n", true);
                                   }
                               }
                           }
                           else
                           {
                               Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("选择期间没有未同步单据");
                           }
                       });
                    var s = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsInstock_importStep>());
                    return Ok(new { Message = s });


                }
                catch (Exception exp)
                {
                    RobamApi.RecordError(exp.Message);
                    JObject jobj = new JObject();
                    jobj["Error"] = exp.Message;
                    jobj["Message"] = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsInstock_importStep>());
                    Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("Exception:" + exp.Message, true);
                    return BadRequest(new { Message = jobj.ToString() });
                }
                finally
                {
                    sg_sync_syncreturnback_Running = false;
                }
            }
            
            return Ok(new { Message = "success"});
        }
        /// <summary>
        /// 同步配件价格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/syncpartsprice")]
        public IActionResult syncPartsPrices()
        {
            try
            {
                var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == BusinessType.Robam_PJ)?.FirstOrDefault();
                var crm = new RobamApi.Robam_CRM(acc);
                var ins = crm.GetPartsPrice();
                if (ins == null)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = ""
                    });
                }
                if (crm.UpdatePartsPriceWithDb(ins))
                {
                    return Ok(new
                    {
                        isSuccess = true,
                        message = ""
                    });
                }
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
            return Ok(new
            {
                isSuccess = false,
                message = ""
            });
        }

        [HttpPost]
        [Route("api/test")]
        public IActionResult test()
        {
            string txt = "";
            //string str = @"{""crminvexportheaderss"":[{""exOrderHeadersId"":12653319,""orderNo"":""ST20220802000287"",""orderTypeId"":1000383,""orderTypeCode"":""P_HAND_OUT_DB_CONFIRM"",""orderTypeName"":""分公司内部调拨出货"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 11:10:03.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""陈晓燕"",""receiveDate"":""2022-08-04"",""printDate"":""2022-08-02 11:18:35.0"",""printCount"":2,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerCode"":""ORG-sjzb001"",""customerName"":""邢台市桥东三诚电器商行"",""customerShortName"":""石家庄"",""createdByName"":""李全双"",""lastUpdatedByName"":""陈晓燕"",""creationDate"":""2022-08-02 08:33:00.0"",""lastUpdateDate"":""2022-08-04 15:50:16.0"",""inventoryCode"":""sjzb003001"",""inventoryDesc"":""沧州仓库"",""receiveInventoryCode"":""sjzb001001"",""receiveInventoryDesc"":""邢台仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省邢台市桥西区河北省邢台市桥西区车站南路北斗化轻仓库"",""contactTel"":""0319-3195578"",""contactName"":""王岱巍"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130101I26-2208010005"",""sourceNo"":""DBCK202208010088"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""桥西区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12656585,""orderNo"":""ST20220802001324"",""orderTypeId"":1000385,""orderTypeCode"":""P_HAND_OUT_PLLS_CONFIRM"",""orderTypeName"":""客户零售出库单"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 15:29:08.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 14:16:41.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerName"":""王昆"",""createdByName"":""李全双"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 14:16:19.0"",""lastUpdateDate"":""2022-08-02 15:31:29.0"",""inventoryCode"":""sjzb003001"",""inventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省廊坊市三河市杨庄镇三河市南杨庄三河市南杨庄定制南杨庄"",""contactTel"":""18833651111"",""contactName"":""王昆"",""storeName"":""廊坊三河重信建材城零售云店"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130104S99-2208020002"",""sourceNo"":""1-6014472765"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""singlePerson"":""肖佳林"",""singleDate"":""2022-08-02 00:00:00.0"",""reservationDeliveryDate"":""2022-08-02 00:00:00.0"",""channelName"":""廊坊高胜"",""channelId"":""51111"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""三河市"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12657415,""orderNo"":""CGR20220802000031"",""orderTypeId"":31,""orderTypeCode"":""P_HAND_IN_HDCG_CONFIRM"",""orderTypeName"":""活动品采购入库单"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 15:13:08.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 15:13:30.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""remark"":""配件库转成品库"",""customerCode"":""ORG-sjzb003"",""customerName"":""沧州市世纪厨具销售有限公司"",""customerShortName"":""石家庄"",""createdByName"":""李全双"",""creationDate"":""2022-08-02 15:13:08.0"",""lastUpdateDate"":""2022-08-02 15:15:04.0"",""receiveInventoryCode"":""sjzb003001"",""receiveInventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省沧州市运河区河北省沧州市运河区清池北大道沧州炼油厂西门对过老板电器仓库    李玉胜   0317-3060078     13303170239"",""contactTel"":""0317-3060078"",""contactName"":""张三"",""deliveryCustomerName"":""张三"",""fxOrderNo"":""130103R06-2208020001"",""sourceNo"":""PL20220802000379"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""运河区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12657226,""orderNo"":""PLTH202208020141"",""orderTypeId"":505,""orderTypeCode"":""P_HAND_RETAIL_PLLS_CONFIRM"",""orderTypeName"":""零售退货入库"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-03"",""printDate"":""2022-08-02 15:07:20.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerCode"":""ORG-sjzb003"",""customerName"":""沧州市世纪厨具销售有限公司"",""customerShortName"":""石家庄"",""createdByName"":""李全双"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 15:06:53.0"",""lastUpdateDate"":""2022-08-03 17:15:50.0"",""receiveInventoryCode"":""sjzb003001"",""receiveInventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省廊坊市市辖区廊坊"",""contactTel"":""17733796992"",""contactName"":""李女士"",""storeName"":""沧州世纪厨具"",""deliveryCustomerName"":""李女士"",""fxOrderNo"":""130103S99-2208020022"",""sourceNo"":""1-6014810771"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""singleDate"":""2022-08-02 00:00:00.0"",""reservationDeliveryDate"":""2022-08-02 00:00:00.0"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""市辖区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12657871,""orderNo"":""YJRK202208020125"",""orderTypeId"":906,""orderTypeCode"":""P_HAND_IN_YJ_CONFIRM"",""orderTypeName"":""样机入库申请（下样）"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 15:40:50.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""remark"":""居然撤9b52样机销宋瑞波13831758679 \/B9B52T0017501995"",""customerCode"":""ORG-sjzb003"",""customerName"":""沧州市世纪厨具销售有限公司"",""customerShortName"":""石家庄"",""createdByName"":""李全双"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 15:40:25.0"",""lastUpdateDate"":""2022-08-02 15:44:27.0"",""inventoryCode"":""1301030034"",""inventoryDesc"":""沧州新华居然之家专卖店"",""receiveInventoryCode"":""sjzb003001"",""receiveInventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省沧州市新华区河北省沧州市新华区小赵庄乡黄河东路82号"",""contactTel"":""15530787778"",""contactName"":""刘均荣"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130103I04-2208020001"",""sourceNo"":""YJOU202208020477"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""reservationDeliveryDate"":""2022-08-02 00:00:00.0"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""新华区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12655165,""orderNo"":""ST20220802000904"",""orderTypeId"":1000383,""orderTypeCode"":""P_HAND_OUT_DB_CONFIRM"",""orderTypeName"":""分公司内部调拨出货"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 10:57:31.0"",""exportConfirmByName"":""刘洋"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 10:55:13.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""remark"":""燕郊国美国道撤样机8336S 5701 9B00 廊坊调沧州 三河海南国美撤样机9G53  W735"",""customerCode"":""ORG-sjzb003"",""customerName"":""沧州市世纪厨具销售有限公司"",""customerShortName"":""石家庄"",""createdByName"":""刘洋"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 10:55:03.0"",""lastUpdateDate"":""2022-08-02 11:02:07.0"",""inventoryCode"":""sjzb004001"",""inventoryDesc"":""廊坊仓库"",""receiveInventoryCode"":""sjzb003001"",""receiveInventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省沧州市运河区河北省沧州市运河区清池北大道沧州炼油厂西门对过老板电器仓库    李玉胜   0317-3060078     13303170239"",""contactTel"":""0317-3060078"",""contactName"":""李玉胜"",""deliveryCustomerCode"":""ORG-sjzb004"",""deliveryCustomerName"":""廊坊市大智贸易有限公司"",""fxOrderNo"":""130101I26-2208020002"",""sourceNo"":""DBCK202208020031"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""运河区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12653573,""orderNo"":""ST20220802000333"",""orderTypeId"":1000383,""orderTypeCode"":""P_HAND_OUT_DB_CONFIRM"",""orderTypeName"":""分公司内部调拨出货"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 08:51:45.0"",""exportConfirmByName"":""刘洋"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 08:45:29.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""remark"":""福成5201 南巷口5701 燕福达8351 三河海南国美 5701 WB721 825  817  CQ926  廊坊调沧州"",""customerCode"":""ORG-sjzb003"",""customerName"":""沧州市世纪厨具销售有限公司"",""customerShortName"":""石家庄"",""createdByName"":""刘洋"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 08:45:21.0"",""lastUpdateDate"":""2022-08-02 08:52:45.0"",""inventoryCode"":""sjzb004001"",""inventoryDesc"":""廊坊仓库"",""receiveInventoryCode"":""sjzb003001"",""receiveInventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省沧州市运河区河北省沧州市运河区清池北大道沧州炼油厂西门对过老板电器仓库    李玉胜   0317-3060078     13303170239"",""contactTel"":""0317-3060078"",""contactName"":""李玉胜"",""deliveryCustomerCode"":""ORG-sjzb004"",""deliveryCustomerName"":""廊坊市大智贸易有限公司"",""fxOrderNo"":""130101I26-2208010011"",""sourceNo"":""DBCK202208010098"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""运河区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12656421,""orderNo"":""ST20220802001235"",""orderTypeId"":1000383,""orderTypeCode"":""P_HAND_OUT_DB_CONFIRM"",""orderTypeName"":""分公司内部调拨出货"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-03 14:37:26.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""孙志平"",""receiveDate"":""2022-08-06"",""printDate"":""2022-08-06 08:49:31.0"",""printCount"":2,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerCode"":""ORG-sjzb006"",""customerName"":""邯郸市丛台区鑫升电器经销服务部"",""customerShortName"":""石家庄"",""createdByName"":""李全双"",""lastUpdatedByName"":""孙志平"",""creationDate"":""2022-08-02 14:02:48.0"",""lastUpdateDate"":""2022-08-06 11:41:06.0"",""inventoryCode"":""sjzb003001"",""inventoryDesc"":""沧州仓库"",""receiveInventoryCode"":""sjzb006001"",""receiveInventoryDesc"":""邯郸仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省邯郸市丛台区苏曹乡河北省邯郸市丛台区苏曹河西村顺发小区1-6-102（刘君生 13932045691）"",""contactTel"":""0310-7038985"",""contactName"":""刘君生"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130101I26-2208020004"",""sourceNo"":""DBCK202208020050"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""丛台区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12656447,""orderNo"":""ST20220802001242"",""orderTypeId"":1000385,""orderTypeCode"":""P_HAND_OUT_PLLS_CONFIRM"",""orderTypeName"":""客户零售出库单"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 15:29:26.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 14:06:14.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerName"":""吕玉芬"",""createdByName"":""李全双"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 14:03:05.0"",""lastUpdateDate"":""2022-08-02 15:31:49.0"",""inventoryCode"":""sjzb003001"",""inventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省廊坊市广阳区新开路街道管道局九区康乐花园14-502"",""contactTel"":""13731614880"",""contactName"":""吕玉芬"",""storeName"":""廊坊大中电器银河店"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130104S99-2207190008"",""sourceNo"":""1-6012325548"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""singlePerson"":""武丽新"",""singleDate"":""2022-07-19 00:00:00.0"",""reservationDeliveryDate"":""2022-07-19 00:00:00.0"",""channelName"":""廊坊大中"",""channelId"":""13731614880"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""广阳区"",""dealInInfo"":""处理成功""},{""exOrderHeadersId"":12656446,""orderNo"":""ST20220802001241"",""orderTypeId"":1000385,""orderTypeCode"":""P_HAND_OUT_PLLS_CONFIRM"",""orderTypeName"":""客户零售出库单"",""orderDate"":""2022-08-02"",""status"":""D2"",""statusName"":""已接收"",""exportFlag"":""Y"",""exportConfirmDate"":""2022-08-02 15:29:44.0"",""exportConfirmByName"":""李全双"",""receiveFlag"":""Y"",""receiveByName"":""李全双"",""receiveDate"":""2022-08-02"",""printDate"":""2022-08-02 14:05:04.0"",""printCount"":1,""printLock"":""N"",""orgName"":""老板电器库存组织"",""customerName"":""徐书发"",""createdByName"":""李全双"",""lastUpdatedByName"":""李全双"",""creationDate"":""2022-08-02 14:03:05.0"",""lastUpdateDate"":""2022-08-02 15:32:07.0"",""inventoryCode"":""sjzb003001"",""inventoryDesc"":""沧州仓库"",""customerReceiveFlag"":""Y"",""InceptAddress"":""河北省廊坊市永清县永清县工业园区东壮村东壮村0"",""contactTel"":""13803163733"",""contactName"":""徐书发"",""storeName"":""廊坊永清齐家"",""deliveryCustomerCode"":""ORG-sjzb003"",""deliveryCustomerName"":""沧州市世纪厨具销售有限公司"",""fxOrderNo"":""130104S99-2208020001"",""sourceNo"":""1-6012899118"",""transportTypeName"":""自提"",""partorgood"":""GOOD"",""singlePerson"":""刘洋"",""singleDate"":""2022-08-02 00:00:00.0"",""reservationDeliveryDate"":""2022-08-02 00:00:00.0"",""channelName"":""廊坊高胜"",""channelId"":""202282"",""receiptFlag"":""N"",""timeSlot"":""-"",""reconciliationFlag"":""N"",""inceptAreaName"":""永清县"",""dealInInfo"":""处理成功""}],""page"":{""begin"":0,""length"":10,""count"":79,""totalPage"":8,""currentPage"":1,""isCount"":true,""isFirst"":true,""isLast"":false,""size"":10}}";
            try
            {
                //Sqlite_Helper_Static.droptable<Sqlite_Models_ImportSteps>();
                //KingdeeApi k3cloud = new KingdeeApi();
                //var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ)?.FirstOrDefault();

                //if (acc == null)
                //{
                //    Utils.Utils.RecordStep<Sqlite_Models_ImportSteps>("获取账户失败！");
                //    return BadRequest(new { Message = "获取账户失败" });
                //}
                //var crm = new RobamApi.Robam_CRM(acc);
                //crm.GetItemList();
                //if (crm.SignIn())
                //{
                //    //string d = crm.GetoperatorId();
                //    //crm.GetRoleList();

                //}
                //var bill = crm.GetRealStockBill("2022-9-1", "2022-9-2");
                //if (bill == null)
                //{
                //    return BadRequest(new { Message = "获取单据失败" });
                //}
                //else
                //{
                //    //对比单据

                //}
                //var robam = new RobamApi.Robam_Distribution(new RobamApi.UserAccount() { FAccount = "1301031002", FPWD = "0nEYU2BCEdkJgN/SZD/oMZlly5370TX3XDjSpYI+jU4=" });
                ////var list = robam.GetSaleReport("2022-9-1","2022-9-2");
                //var list = Sqlite_Helper_Static.read<DIS_SaleReport>();
                ////foreach(var item in list)
                ////{
                ////    Sqlite_Helper_Static.write(item);
                ////}
                //Utils.Utils.SyncAllItemsFromOutStockBill(list, k3cloud, crm);
                //var bills = robam.GetSaleOrder(list);
                //foreach(var item in bills)
                //{
                //    if(k3cloud.SyncSaleOrderBill(item) == KingdeeApi.SyncResult.AllSuccess)
                //    {
                //        Utils.Utils.RecordStep("同步单据失败！");
                //    }
                //}
                //KingdeeApi k3cloud = new KingdeeApi();
                //string failreason = "";
                //string pushedbillno = "";

                ////k3cloud.PushXSDDToXSCK("130103S99-2211110004", out failreason,out pushedbillno);
                //var d = k3cloud.GetXSCK("XSCKD000005");
                //List<FEntityItem> e = new List<FEntityItem>();

                new Thread(() =>
                {
                    int i = 10;
                    while (i >= 1)
                    {
                        i--;
                        Thread.Sleep(1000);
                        if (_chatHubContext is null)
                        {
                            txt += "_chatHubContext is null\r\n";
                        }
                        else
                        {
                            _chatHubContext.Clients.All.messageReceived(Utils.Utils.StaticMessage("INSTOCK"));
                            txt += "_chatHubContext is not null\r\n";
                        }

                    }
                }).Start();

            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
                Console.WriteLine(exp.Message);
                txt += exp.Message + "\r\n";
            }
            //return Ok(new { Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_ImportSteps>()) });
            return Ok(new { Message = "" });
        }

        [HttpGet]
        [Route("api/geterrors_syncinstockbill")]
        public IActionResult GetErrorList_syncinstockbill()
        {
            try
            {
                string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_ImportSteps>());
                return Ok(new { Message = logs });

            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new { Message = "" });
        }
        [HttpGet]
        [Route("api/geterrors_syncoutstockbilllist")]
        public IActionResult GetErrorList_syncoutstockbilllist()
        {
            try
            {
                string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Outstock_importSteps>());
                return Ok(new { Message = logs });
            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new { Message = "" });
        }
        [HttpGet]
        [Route("api/geterrors_syncrealoutstockbill")]
        public IActionResult GetErrorList_syncrealoutstockbill()
        {
            try
            {
                string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_Realoutstock_importStes>());
                return Ok(new { Message = logs });
            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new { Message = "" });
        }
        [HttpGet]
        [Route("api/geterrors_syncpartsoutstockbill")]
        public IActionResult GetErrorList_syncpartsoutstockbill()
        {
            try
            {
                string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsOutstock_importStep>());
                return Ok(new { Message = logs });
            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new { Message = "" });
        }
        [HttpGet]
        [Route("api/geterrors_syncpartsinstockbill")]
        public IActionResult GetErrorList_syncpartsinstockbill()
        {
            try
            {
                string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_PartsInstock_importStep>());
                return Ok(new { Message = logs });
            }
            catch (Exception exp)
            {
                RobamApi.RecordError(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new { Message = "" });
        }

        [HttpGet]
        [Route("api/testunitcreate")]
        public IActionResult TestUnitCreate()
        {
            try
            {
                KingdeeApi ka = new KingdeeApi();
                //ka.CreateUnit("测试", "测试");
                //return Ok(new { 
                //    Message="创建成功！"
                //});
                //ka.CheckUnitExist("测试");
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new
            {
                Message = "创建失败！"
            });
        }

        [HttpGet]
        [Route("api/testitemcreate")]
        public IActionResult TestItemCreate()
        {
            try
            {
                KingdeeApi ka = new KingdeeApi();
                //ka.CreateUnit("测试", "测试");
                //return Ok(new { 
                //    Message="创建成功！"
                //});
                //ka.CreateItem("测试","");
                //ka.CreateSupplierGroup("ceshi");
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            finally
            {

            }
            return BadRequest(new
            {
                Message = "创建失败！"
            });
        }

        [HttpPost]
        [Route("api/getlogs")]
        public IActionResult SyncedLogs([FromForm] string startDate, string endDate)
        {
            string logs = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_ImportSteps>());
            return Ok(new { Message = logs });
        }
        [HttpGet]
        [Route("api/syncallitem")]
        public IActionResult SyncAllItem()
        {
            if (!sg_sync_syncallitem_Running)
            {
                sg_sync_syncallitem_Running = true;
                try
                {
                    KingdeeApi k3cloud = new KingdeeApi();
                    var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ)?.FirstOrDefault();

                    if (acc == null)
                    {
                        Utils.Utils.RecordStep<Sqlite_Models_MaterialSync>("获取账户失败！");
                        return BadRequest(new { Message = "获取账户失败" });
                    }
                    var crm = new RobamApi.Robam_CRM(acc);
                    var ret = crm.GetItemList();
                    if(ret == null)
                    {
                        wl_jczldr("获取基础资料失败!",true);
                        return Ok(new { Message = "failed"});
                    }
                    wl_jczldr("获取基础资料成功，共计" + ret.materials.Count + "条");



                    k3cloud.CreateItem(ret);
                    return Ok(new
                    {
                        Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_MaterialSync>().ToList())
                    });
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                finally
                {
                    sg_sync_syncallitem_Running = false;
                }
            }
            else
            {
                return Ok(new
                {
                    Message = JsonConvert.SerializeObject(new Sqlite_Models_MaterialSync() { FID = 1, IsError = false, Message = "上次同步仍未完成!", Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") })
                });
            }
            //Sqlite_Helper_Static.droptable<Sqlite_Models_MaterialSync>();
            return Ok(new
            {
                Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_MaterialSync>().ToList())
            });
        }
        [HttpGet]
        [Route("api/syncallitemparts")]
        public IActionResult SyncAllItemParts()
        {
            if (!sg_sync_syncallitemparts_Running)
            {
                sg_sync_syncallitemparts_Running = true;
                try
                {
                    KingdeeApi k3cloud = new KingdeeApi();
                    var acc = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Crm && i.BusinessType == RobamApi.BusinessType.Robam_PJ)?.FirstOrDefault();

                    if (acc == null)
                    {
                        //Utils.Utils.RecordStep<Sqlite_Models_MaterialSync>("获取账户失败！");
                        wl_jczldr("获取账户失败！", true);
                        return BadRequest(new { Message = "获取账户失败" });
                    }
                    var crm = new RobamApi.Robam_CRM(acc);
                    //获取配件
                    //var ret = crm.GetItemPartList();
                    var ret = crm.GetItemListFromText();
                    k3cloud.CreateItem(ret);
                    return Ok(new
                    {
                        Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_MaterialSync>().ToList())
                    });
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                finally
                {
                    sg_sync_syncallitemparts_Running = false;
                }
            }
            else
            {
                return Ok(new
                {
                    Message = JsonConvert.SerializeObject(new Sqlite_Models_MaterialSync() { FID = 1, IsError = false, Message = "上次同步仍未完成!", Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") })
                });
            }
            return Ok(new
            {
                Message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_MaterialSync>().ToList())
            });

        }
        [HttpGet]
        [Route("api/syncallitemlog")]
        public IActionResult SyncAllItemlog()
        {
            //Sqlite_Helper_Static.droptable<Sqlite_Models_MaterialSync>();
            string message = JsonConvert.SerializeObject(Sqlite_Helper_Static.read<Sqlite_Models_MaterialSync>().ToList());
            return Ok(new
            {
                Message = message
            });
        }

        [HttpGet]
        [Route("api/syncallitemparts")]
        public IActionResult SyncAllShops()
        {
            if (!sg_sync_syncallishops_Running)
            {
                sg_sync_syncallishops_Running = true;
                try
                {
                    KingdeeApi k3cloud = new KingdeeApi();
                    var accs = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution && i.BusinessType == RobamApi.BusinessType.Robam_CP)?.ToList();
                    foreach(var acc in accs)
                    {
                        if (acc == null)
                        {
                            Utils.Utils.RecordStep<Sqlite_Models_MaterialSync>("获取账户失败！");
                            return BadRequest(new { Message = "获取账户失败" });
                        }
                        var dis = new RobamApi.Robam_Distribution(acc);
                        var shops = dis.GetShops();
                        if(shops != null)
                        {
                            foreach (var item in shops.response.data.datas)
                            {
                                if (k3cloud.CreateShop(item.organization, item.organization_name, "001", "", "", "", "") != KingdeeApi.CreateResult.AllSuccess)
                                {
                                    wl_jczldr("门店" + item.organization + " " + item.organization_name + "创建失败!", true);
                                }
                                else
                                {
                                    wl_jczldr("门店" + item.organization + " " + item.organization_name + "创建成功!");
                                }
                            }
                        }
                    }
                    
                    //同步地区
                    //同步二级渠道
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                finally
                {
                    sg_sync_syncallishops_Running = false;
                }
            }
            else
            {
                return Ok(new
                {
                    Message = JsonConvert.SerializeObject(new Sqlite_Models_MaterialSync() { FID = 1, IsError = false, Message = "上次同步仍未完成!", Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") })
                });
            }
            return Ok(new { });
        }
        static bool _SyncAllSalers { get; set; } = false;
        [HttpGet]
        [Route("api/syncallitemparts")]
        public IActionResult SyncAllSalers()
        {
            if (!_SyncAllSalers)
            {
                _SyncAllSalers = true;
                try
                {
                    KingdeeApi k3cloud = new KingdeeApi();
                    var accs = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution && i.BusinessType == RobamApi.BusinessType.Robam_CP)?.ToList();
                    if(accs == null)
                    {
                        Robam_Sync.wl_jczldr("获取账户失败！", true);
                        return Ok(new { Message = "上次导入未执行完毕" });
                    }
                    foreach(var acc in accs)
                    {
                        if (acc == null)
                        {
                            Robam_Sync.wl_jczldr("获取账户失败！", true);
                            continue;
                        }
                        var dis = new RobamApi.Robam_Distribution(acc);
                        var salers = dis.GetSalers();
                        if(salers == null)
                        {
                            wl_jczldr("获取导购员失败!");
                            continue;
                        }
                        foreach (var item in salers.response.data.datas)
                        {
                            if(k3cloud.CheckSalerExists(item.employee_no) != KingdeeApi.CheckResult.ItemExists)
                            {
                                if (k3cloud.CreateSalesman(item.employee_no, item.employee_name, "001") == KingdeeApi.CreateResult.AllSuccess)
                                {
                                    wl_jczldr("创建导购员" + item?.employee_no ?? "" + " " + item?.employee_name ?? "" + "导入成功!");
                                }
                                else
                                {
                                    wl_jczldr("创建导购员" + item?.employee_no ?? "" + " " + item?.employee_name ?? "" + "导入成功!", true);
                                }
                            }
                            
                        }
                    }
                    
                }
                catch(Exception exp)
                {
                    wl_jczldr("创建导购员发生错误"  + exp.Message, true);
                }
                finally
                {
                    _SyncAllSalers = false;
                }
                return Ok(new { Message = "success"});
            }
            else
            {
                return Ok(new {Message="上次导入未执行完毕" });
            }
            
            
           
        }

        [HttpGet]
        [Route("api/syncallitemparts")]
        public IActionResult SyncAllStocks()
        {
            KingdeeApi k3cloud = new KingdeeApi();
            var accs = Utils.KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution && i.BusinessType == RobamApi.BusinessType.Robam_CP /*&& i.Area == Area.LF*/)?.ToList();

            foreach(var acc in accs)
            {
                if (acc == null)
                {
                    Robam_Sync.wl_jczldr("获取账户失败！",true);
                    continue;
                }
                var dis = new RobamApi.Robam_Distribution(acc);
                var stocks = dis.GetStocks();
                if(stocks != null)
                {
                    foreach (var item in stocks.response.data.datas)
                    {
                        if (k3cloud.CreateStock(item.warehouse_no, item.warehouse_name, "001") == KingdeeApi.CreateResult.AllSuccess)
                        {
                            wl_jczldr("创建仓库" + item?.warehouse_no ?? "" + " " + item?.warehouse_name ?? "" + "导入成功!");
                        }
                        else
                        {
                            wl_jczldr("创建仓库" + item?.warehouse_no ?? "" + " " + item?.warehouse_name ?? "" + "导入成功!", true);
                        }
                    }
                }
                else
                {
                    Robam_Sync.wl_jczldr("获取仓库失败！", true);
                    continue;
                }

            }
           

            return Ok(new { });

        }
        [HttpPost]
        [Route("api/syncinstock")]
        public Sqlite_Models_Result_TableMessage syncinstock([FromBody] Models.Paras paras)
        {
            //产品入库 
            //配件入库
            //配件退回
            if(paras == null)
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("参数传递错误!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
            }
            Sqlite_Helper_Static.deleteall<Sqlite_Models_Instock>();
            //Sqlite_Helper_Static.droptable<Sqlite_Models_Instock>();
            var startDate = DateTime.Parse(paras.FStartDate);
            var endDate = DateTime.Parse(paras.FEndDate);
            if(endDate < startDate)
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("导入结束日期不能小于开始日期!",true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
            }
            if(endDate - startDate < TimeSpan.FromHours(6))
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("导入间隔期间应该大于6小时!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
            }
            if(endDate - startDate > TimeSpan.FromDays(2))
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("导入间隔期间不能大于2天!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
            }
            if (paras.FBillTypes.Contains("pjrkd"))
            {
                //判断配件入库
                sync_PartsInstockList(paras.FStartDate, paras.FEndDate);
                //sync_PartsreturnbackList(paras.FStartDate, paras.FEndDate);
            }
            if (paras.FBillTypes.Contains("cprkd"))
            {
                //判断产品入库
                sync_StockList(paras.FStartDate,paras.FEndDate);
            }
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
        }

        [HttpPost]
        [Route("api/syncoutstock")]
        public Sqlite_Models_Result_TableMessage syncoutstock([FromBody] Models.Paras paras)
        {
            //产品出库
            //配件出库
            //
            if (paras == null)
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>("参数传递错误!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
            }
            Sqlite_Helper_Static.deleteall<Sqlite_Models_Outstock>();
            //Sqlite_Helper_Static.droptable<Sqlite_Models_Outstock>();
            var startDate = DateTime.Parse(paras.FStartDate);
            var endDate = DateTime.Parse(paras.FEndDate);
            if (endDate < startDate)
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>("导入结束日期不能小于开始日期!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
            }
            if (endDate - startDate < TimeSpan.FromHours(6))
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>("导入间隔期间应该大于6小时!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
            }
            if (endDate - startDate > TimeSpan.FromDays(2))
            {
                Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>("导入间隔期间不能大于2天!", true);
                return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
            }
            if (paras.FBillTypes.Contains("cpckd"))
            {
                //判断产品出库
                sync_OutStockBillList(paras.FStartDate, paras.FEndDate);
            }
            if (paras.FBillTypes.Contains("pjckd"))
            {
                //判断配件出库
                sync_PartsOutstockList(paras.FStartDate, paras.FEndDate);
            }

            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
        }

        [HttpPost]
        [Route("api/syncqtxxtb")]
        public Sqlite_Models_Result_TableMessage syncqtxxtb([FromBody] Models.Paras paras)
        {
            Sqlite_Helper_Static.deleteall<Sqlite_Models_QTXXTB>();
            if(paras == null)
            {
                return new Sqlite_Models_Result_TableMessage() { rowData = new List<Result_TableMessage_RowData> { new Result_TableMessage_RowData() { index = 1 ,description = "参数传递错误！接收的参数为null",isError = true} } };
            }
            if (paras.FBillTypes.Contains("pjjg"))
            {
                syncPartsPrices();
            }
            if (paras.FBillTypes.Contains("cpjg"))
            {

            }

            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_QTXXTB>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/synczczldr")]
        public Sqlite_Models_Result_TableMessage synczczldr([FromBody] Models.Paras paras)
        {
            Sqlite_Helper_Static.deleteall<Sqlite_Models_JCZLTB>();
            if (paras == null)
            {
                return new Sqlite_Models_Result_TableMessage() { rowData = new List<Result_TableMessage_RowData> { new Result_TableMessage_RowData() { index = 1, description = "参数传递错误！接收的参数为null", isError = true } } };
            }
            if (paras.FBillTypes.Contains("cp"))
            {
                SyncAllItem();
            }
            if (paras.FBillTypes.Contains("dgy"))
            {
                SyncAllSalers();
            }
            if (paras.FBillTypes.Contains("md"))
            {
                SyncAllShops();
            }
            if (paras.FBillTypes.Contains("pj"))
            {
                SyncAllItemParts();
            }
            if (paras.FBillTypes.Contains("ck"))
            {
                SyncAllStocks();
            }
            
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_JCZLTB>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncinstock")]
        public Sqlite_Models_Result_TableMessage record_syncinstock()
        {
            //产品出库
            //配件出库
            //
            //Utils.Utils.RecordStepNew<Sqlite_Models_Instock>("API响应" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), true);
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncoutstock")]
        public Sqlite_Models_Result_TableMessage record_syncoutstock()
        {
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncqtxxtb")]
        public Sqlite_Models_Result_TableMessage record_syncqtxxtb()
        {
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_QTXXTB>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncjczldr")]
        public Sqlite_Models_Result_TableMessage record_syncjczldr()
        {
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_JCZLTB>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncautoinstock")]
        public Sqlite_Models_Result_TableMessage record_syncautoinstock()
        {
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_AutoInstock>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/recordsyncautooutstock")]
        public Sqlite_Models_Result_TableMessage record_syncautooutstock()
        {
            return new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_AutoOutstock>().Select(i => i.Format()).ToList() };
        }
        [HttpPost]
        [Route("api/execute_autoinstock")]
        public IActionResult AutoInstock([FromBody] Models.Paras paras)
        {
            wl_instock = Utils.Utils.RecordStepNew<Sqlite_Models_AutoInstock>;
            syncinstock(paras);
            wl_instock = Utils.Utils.RecordStepNew<Sqlite_Models_Instock>;
            return Ok(new { Message = "导入成功!" });
        }
        [HttpPost]
        [Route("api/execute_autooutstock")]
        public IActionResult AutoOutstock([FromBody] Models.Paras paras)
        {
            wl_instock = Utils.Utils.RecordStepNew<Sqlite_Models_AutoOutstock>;
            syncoutstock(paras);
            wl_instock = Utils.Utils.RecordStepNew<Sqlite_Models_Outstock>;
            return Ok(new { Message = "导入成功!" });
        }
        [HttpPost]
        [Route("api/execute_clearlog")]
        public IActionResult ClearRKZDDRLog([FromBody] Models.Paras paras)
        {
            Sqlite_Helper_Static.droptable<Sqlite_Models_AutoInstock>();
            return Ok(new { Message = "导入成功!" });
        }

        [HttpPost]
        [Route("api/execute_clearlog")]
        public IActionResult ClearCKZDDRLog([FromBody] Models.Paras paras)
        {
            Sqlite_Helper_Static.droptable<Sqlite_Models_AutoOutstock>();
            return Ok(new { Message = "导入成功!" });
        }
    }
}