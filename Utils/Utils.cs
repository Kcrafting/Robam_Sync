using Models;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Utils.RobamApi;
using Robam_Sync;
using Robam_Sync.Models;
using Robam_Sync.SignalRWebpack;

namespace Utils
{
    public static class Utils
    {
        const string KEY_64 = "D1A8A42F";//注意了，是8个字符
        const string IV_64 = "73588578";
        public static DIS_SecondChannel SG_SecondChannel = null;
        public static string Encode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        public static long GetMillisecondsTimeStemp()
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));// Local. .ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差秒数
            return timeStamp;
        }
        public static long GetMillisecondsTimeStemp(DateTime dt)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));// Local. .ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(dt - startTime).TotalMilliseconds; // 相差秒数
            return timeStamp;
        }
        public static long GetSecondsTimeStemp()
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));// Local. .ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差秒数
            return timeStamp;
        }
        public static string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        public static void RecordStep<T>(string message, bool iserror = false) where T: Sqlite_Models_ImportSteps, new()
        {
            try
            {
                Sqlite_Helper_Static.write<T>(new T()
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    IsError = iserror,
                    Message = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static void RecordStepNew<T>(string message, bool iserror = false) where T : Sqlite_Models_Parent, new()
        {
            try
            {
                Sqlite_Helper_Static.write<T>(new T()
                {
                    errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    isError = iserror,
                    description = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static void RecordStepForOutStock(string message, bool iserror = false)
        {
            try
            {
                Sqlite_Helper_Static.write<Sqlite_Models_Outstock_importSteps>(new Sqlite_Models_Outstock_importSteps()
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    IsError = iserror,
                    Message = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static void RecordStepForRealoutStock(string message, bool iserror = false)
        {
            try
            {
                Sqlite_Helper_Static.write<Sqlite_Models_Realoutstock_importStes>(new Sqlite_Models_Realoutstock_importStes()
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    IsError = iserror,
                    Message = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static void RecordStepForPartsOutstock(string message, bool iserror = false)
        {
            try
            {
                Sqlite_Helper_Static.write<Sqlite_Models_PartsOutstock_importStep>(new Sqlite_Models_PartsOutstock_importStep()
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    IsError = iserror,
                    Message = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static void RecordStepForPartsInstock(string message, bool iserror = false)
        {
            try
            {
                Sqlite_Helper_Static.write<Sqlite_Models_PartsInstock_importStep>(new Sqlite_Models_PartsInstock_importStep()
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"),
                    IsError = iserror,
                    Message = message
                });
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
        }
        public static bool SyncAllItemsFromInStockBill(CRM_OutStockDetail billdetail, KingdeeApi kai, Robam_CRM robam,string billform)
        {
            try
            {
                int fid = 0;
                RecordStep<Sqlite_Models_ImportSteps>(JsonConvert.SerializeObject(billdetail));
                //KingdeeApi kai = new KingdeeApi();
                if (billdetail == null)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("单据详情为null 退出\n",true);
                    return false;
                }
                int outfid = 0;
                //创建组织
                RecordStep<Sqlite_Models_ImportSteps>("检测创建组织\n");

                if (billdetail.crminvexportheaders.orgId != null && kai.CheckComanyExist(billdetail.crminvexportheaders.orgId.ToString()) != KingdeeApi.CheckResult.ItemExists)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("创建组织不存在，开始创建\n");
                    if(!string.IsNullOrWhiteSpace(billdetail.crminvexportheaders?.orgId?.ToString()) /*&& billdetail.crminvexportheaders.orgName != null*/)
                    {
                        if (kai.CreateComany(billdetail.crminvexportheaders.orgId.ToString(), billdetail.crminvexportheaders.orgName.ToString() ?? billdetail.crminvexportheaders.orgId.ToString(), "") != KingdeeApi.CreateResult.AllSuccess)
                        {
                            RecordStep<Sqlite_Models_ImportSteps>("创建组织成功\n");
                        }
                        else
                        {
                            RecordStep<Sqlite_Models_ImportSteps>("创建组织失败" + kai.m_ErrorMessage, true);
                            //return false;
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建组织时有编码或者名称为null\n");
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("创建组织存在\n");
                }
                //创建单据类型

                if (!string.IsNullOrWhiteSpace(billdetail.crminvexportheaders.orderTypeName))
                {
                    RecordStep<Sqlite_Models_ImportSteps>("检测单据类型存在\n");
                    if (kai.CheckBillTypeExist(billdetail.crminvexportheaders.orderTypeName, billform) != KingdeeApi.CheckResult.ItemExists)
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("单据类型不存在\n");
                        if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.orderTypeName) /*&& !string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.orderTypeName)*/)
                        {
                            if (kai.CreateBillType(billdetail.crminvexportheaders.orderTypeName, billdetail.crminvexportheaders.orderTypeName,billform) != KingdeeApi.CreateResult.AllSuccess)
                            {
                                //创建失败
                                RecordStep<Sqlite_Models_ImportSteps>("检测单据类型创建失败" + kai.m_ErrorMessage, true);
                                //return false;
                            }
                            else
                            {
                                //创建成功
                                RecordStep<Sqlite_Models_ImportSteps>("单据类型创建成功\n");
                            }
                        }
                        else
                        {
                            RecordStep<Sqlite_Models_ImportSteps>("创建单据类型时有编码或者名称为null\n");
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("单据类型存在\n");
                    }
                }
                //创建公司单位
                RecordStep<Sqlite_Models_ImportSteps>("检测公司单位父分组存在\n");
                if (kai.CheckComanyGroupExist("001", out outfid) != KingdeeApi.CheckResult.ItemExists)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("公司单位父分组不存在\n");
                    if (kai.CreateComanyGroup("001", "老板电器") == KingdeeApi.CreateResult.AllSuccess || kai.CreateComanyGroup("001", "老板电器") == KingdeeApi.CreateResult.ItemAlreadyExists)
                    {
                        //创建成功
                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位父分组<" + "001-老板电器" + ">成功！");
                    }
                    else
                    {
                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位父分组<" + "001-老板电器" + ">失败！" + kai.m_ErrorMessage, true);
                        //return false;
                        //return false;
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("公司单位父分组存在\n");
                }
                RecordStep<Sqlite_Models_ImportSteps>("检测公司单位存在\n");
                if (kai.CheckComanyExist(billdetail.crminvexportheaders.deliveryCustomerCode) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.deliveryCustomerCode )/* && !string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.deliveryCustomerName )*/)
                    {
                        if (kai.CreateComany(billdetail.crminvexportheaders.deliveryCustomerCode, billdetail.crminvexportheaders.deliveryCustomerName, "001") == KingdeeApi.CreateResult.AllSuccess)
                        {
                            //创建成功
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位<" + billdetail.crminvexportheaders.deliveryCustomerCode + "-" + billdetail.crminvexportheaders.deliveryCustomerName + ">成功！");
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位<" + billdetail.crminvexportheaders.deliveryCustomerCode + "-" + billdetail.crminvexportheaders.deliveryCustomerName + ">失败！" + kai.m_ErrorMessage, true);
                            //return false;
                            //return false;
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建公司单位时有编码或者名称为null\n");
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("公司单位存在\n");
                }

                RecordStep<Sqlite_Models_ImportSteps>("检测公司单位存在\n");
                if (billdetail.crminvexportheaders.customerCode != null && kai.CheckComanyExist(billdetail.crminvexportheaders.customerCode) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.customerCode)/* !=null && billdetail.crminvexportheaders.customerName != null*/)
                    {
                        if (kai.CreateComany(billdetail.crminvexportheaders.customerCode, billdetail.crminvexportheaders.customerName, "001") == KingdeeApi.CreateResult.AllSuccess)
                        {
                            //创建成功
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位<" + billdetail.crminvexportheaders.customerCode + "-" + billdetail.crminvexportheaders.customerName + ">成功！");
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建公司单位<" + billdetail.crminvexportheaders.customerCode + "-" + billdetail.crminvexportheaders.customerName + ">失败！" + kai.m_ErrorMessage, true);
                            //return false;
                            //return false;
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建公司单位时有编码或者名称为null\n");
                    }
                 
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("公司单位存在\n");
                }

                //创建仓库
                RecordStep<Sqlite_Models_ImportSteps>("检测仓库父分组存在\n");
                int outstockid = 0;
                if (kai.CheckStockGroupExist("001", out outstockid) != KingdeeApi.CheckResult.ItemExists)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库父分组不存在\n");
                    if (kai.CreateStockGroup("001", "老板电器") == KingdeeApi.CreateResult.AllSuccess)
                    {
                        //创建成功
                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + "001-老板电器" + ">成功！");
                    }
                    else
                    {
                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + "001-老板电器" + ">失败！" + kai.m_ErrorMessage, true);
                        //return false;
                        //return false;
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库父分组存在\n");
                }

                RecordStep<Sqlite_Models_ImportSteps>("检测仓库存在\n");
                if (kai.CheckStockExist(billdetail.crminvexportheaders.inventoryCode) != KingdeeApi.CheckResult.ItemExists)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库不存在\n");
                    if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.inventoryCode)/* != null && billdetail.crminvexportheaders.inventoryDesc != null*/)
                    {
                        if (kai.CreateStock(billdetail.crminvexportheaders.inventoryCode, billdetail.crminvexportheaders.inventoryDesc, "001") == KingdeeApi.CreateResult.AllSuccess)
                        {
                            //创建成功
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + billdetail.crminvexportheaders.inventoryCode + "-" + billdetail.crminvexportheaders.inventoryDesc + ">成功！");
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + billdetail.crminvexportheaders.inventoryCode + "-" + billdetail.crminvexportheaders.inventoryDesc + ">失败！" + kai.m_ErrorMessage, true);
                            //return false;
                            //return false;
                        }

                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建仓库时有编码或者名称为null\n");
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库存在\n");
                }
                RecordStep<Sqlite_Models_ImportSteps>("检测仓库存在\n");
                if (kai.CheckStockExist(billdetail.crminvexportheaders.receiveInventoryCode) != KingdeeApi.CheckResult.ItemExists)
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库不存在\n");
                    if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.receiveInventoryCode) /*!= null && billdetail.crminvexportheaders.receiveInventoryDesc != null*/)
                    {
                        if (kai.CreateStock(billdetail.crminvexportheaders.receiveInventoryCode, billdetail.crminvexportheaders.receiveInventoryDesc, "001") == KingdeeApi.CreateResult.AllSuccess)
                        {
                            //创建成功
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + billdetail.crminvexportheaders.receiveInventoryCode + "-" + billdetail.crminvexportheaders.receiveInventoryDesc + ">成功！");
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建仓库<" + billdetail.crminvexportheaders.receiveInventoryCode + "-" + billdetail.crminvexportheaders.receiveInventoryDesc + ">失败！" + kai.m_ErrorMessage, true);
                            //return false;
                            //return false;
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建仓库时有编码或者名称为null\n");
                    }
                   
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("仓库存在\n");
                }
                //创建供应商父分组
                if (kai.CheckSupplierGroupExist("001",out fid) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateSupplierGroup("001","老板电器") != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建供应商父分组失败！", true);
                        //return false;
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_ImportSteps>("创建供应商父分组成功！");
                    }
                }
                else
                {
                    RecordStep<Sqlite_Models_ImportSteps>("创建供应商父分组存在！");
                }
                //创建发送供应商
                if(!string.IsNullOrWhiteSpace( billdetail.crminvexportheaders.customerCode)/* != null && billdetail.crminvexportheaders.customerName != null*/)
                {
                    if (kai.CheckSupplierExist(billdetail.crminvexportheaders.customerCode) != KingdeeApi.CheckResult.ItemExists)
                    {
                        if (kai.CreateSupplier(billdetail.crminvexportheaders.customerCode, billdetail.crminvexportheaders.customerName, "001") != KingdeeApi.CreateResult.AllSuccess)
                        {
                            RecordStep<Sqlite_Models_ImportSteps>("创建供应商失败！", true);
                            //return false;
                        }
                    }
                }

                //创建接收供应商
                if (!string.IsNullOrWhiteSpace(billdetail.crminvexportheaders.deliveryCustomerCode)/* != null && billdetail.crminvexportheaders.deliveryCustomerName != null*/)
                {
                    if (kai.CheckSupplierExist(billdetail.crminvexportheaders.deliveryCustomerCode) != KingdeeApi.CheckResult.ItemExists)
                    {
                        if (kai.CreateSupplier(billdetail.crminvexportheaders.deliveryCustomerCode, billdetail.crminvexportheaders.deliveryCustomerName, "001") != KingdeeApi.CreateResult.AllSuccess)
                        {
                            RecordStep<Sqlite_Models_ImportSteps>("创建供应商失败！", true);
                            //return false;
                        }
                    }
                }
                
                RecordStep<Sqlite_Models_ImportSteps>("遍历分录，检测物料单位存在\n");
                if (billdetail.crminvexportheaders.crmInvExOrderLinesVs.Count > 0)
                {
                    foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                    {

                        //单位是否存在 统一按照基本单位pcs
                        RecordStep<Sqlite_Models_ImportSteps>("检测单位存在\n");
                        if (!string.IsNullOrWhiteSpace(item.unitCode))
                        {
                            if (kai.CheckUnitExist(item.unitCode, out outfid) != KingdeeApi.CheckResult.ItemExists)
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("单位不存在\n");
                                if (kai.CheckUnitExist("PCS", out outfid) != KingdeeApi.CheckResult.ItemExists)
                                {
                                    if (item.unitCode != null && item.unitDesc != null)
                                    {
                                        if (kai.CreateUnit(item.unitCode, item.unitDesc) == KingdeeApi.CreateResult.AllSuccess)
                                        {
                                            //创建成功
                                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建单位<" + item.unitCode + "-" + item.unitDesc + ">成功！");
                                        }
                                        else
                                        {
                                            Utils.RecordStep<Sqlite_Models_ImportSteps>("创建单位<" + item.unitCode + "-" + item.unitDesc + ">失败！" + kai.m_ErrorMessage, true);
                                            //return false;
                                            //return false;
                                        }
                                    }
                                }
                                else
                                {
                                    RecordStep<Sqlite_Models_ImportSteps>("创建单位时有编码或者名称为null\n");
                                }
                            }
                            else
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("单位存在\n");
                            }
                        }
                        
                        //检测物料状态
                        RecordStep<Sqlite_Models_ImportSteps>("检测物料状态存在\n");
                        if (!string.IsNullOrWhiteSpace(item.deliveryGoodsStatusName))
                        {
                            if (kai.CheckProductstatusExists(item.deliveryGoodsStatusName) != KingdeeApi.CheckResult.ItemExists)
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("物料状态不存在\n");
                                if (kai.CreateProductstatus(item.deliveryGoodsStatusName, item.deliveryGoodsStatusName) != KingdeeApi.CreateResult.AllSuccess)
                                {
                                    RecordStep<Sqlite_Models_ImportSteps>("物料状态创建失败\n", true);

                                }
                                else
                                {
                                    RecordStep<Sqlite_Models_ImportSteps>("物料状态创建成功\n");
                                    //return false;
                                }
                            }
                            else
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("物料状态存在\n");
                            }
                        }


                        RecordStep<Sqlite_Models_ImportSteps>("检测物料存在\n");
                        if (!string.IsNullOrWhiteSpace(item.materialCode) && !string.IsNullOrWhiteSpace(item.unitCode) && !string.IsNullOrWhiteSpace(billdetail.crminvexportheaders.inventoryCode))
                        {
                            if (kai.CheckItemExist(item.materialCode) != KingdeeApi.CheckResult.ItemExists)
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("物料不存在\n");
                                int groupid = 0;
                                //var itemdetail = robam.GetItemDetail(item.materialCode);
                                if (item != null)
                                {
                                    if (item.productTypeCode != null)
                                    {
                                        RecordStep<Sqlite_Models_ImportSteps>("检测物料分组存在\n");

                                        if (kai.CheckItemGroupExist(item.productTypeCode, out groupid) != KingdeeApi.CheckResult.ItemExists)
                                        {
                                            RecordStep<Sqlite_Models_ImportSteps>("物料分组不存在\n");
                                            if (kai.CreateItemGroup(item.productTypeCode, item.productTypeName) != KingdeeApi.CreateResult.AllSuccess)
                                            {
                                                Utils.RecordStep<Sqlite_Models_ImportSteps>("创建物料分组<" + item.productTypeCode + "-" + item.productTypeName + ">失败！" + kai.m_ErrorMessage, true);
                                                //return false;
                                                //return false;
                                            }
                                            else
                                            {
                                                Utils.RecordStep<Sqlite_Models_ImportSteps>("创建物料分组<" + item.productTypeCode + "-" + item.productTypeCode + ">成功！");
                                            }
                                        }
                                        else
                                        {
                                            RecordStep<Sqlite_Models_ImportSteps>("物料分组存在\n");
                                        }
                                    }
                                    else
                                    {
                                        Utils.RecordStep<Sqlite_Models_ImportSteps>("物料没有设置分组信息");
                                        //return false;
                                    }
                                    RecordStep<Sqlite_Models_ImportSteps>("创建物料\n");
                                    if (kai.CreateItem(item.materialCode, item.materialDesc, item.specification, item.unitCode, billdetail.crminvexportheaders.inventoryCode, item.productTypeCode) != KingdeeApi.CreateResult.AllSuccess)
                                    {
                                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建物料<" + item.materialCode + "-" + item.materialDesc + ">失败！" + kai.m_ErrorMessage, true);
                                        //return false;
                                        //return false;
                                    }
                                    else
                                    {
                                        Utils.RecordStep<Sqlite_Models_ImportSteps>("创建物料<" + item.materialCode + "-" + item.materialDesc + ">成功！");
                                    }
                                }
                                else
                                {
                                    Utils.RecordStep<Sqlite_Models_ImportSteps>("查询物料详情发生错误<" + item.materialCode + "-" + item.materialCode + ">失败！" + kai.m_ErrorMessage, true);
                                    //return false;
                                    //return false;
                                }
                            }
                            else
                            {
                                RecordStep<Sqlite_Models_ImportSteps>("物料存在\n");
                            }
                        }
                    }
                }
                else
                {
                    Utils.RecordStep<Sqlite_Models_ImportSteps>("单据详情分录不存在，不须要同步分录！");
                }
                //if(billdetail.crminvexportheaders.)
            }
            catch (Exception exp)
            {
                Utils.RecordStep<Sqlite_Models_ImportSteps>("同步物料时发生错误" + exp.Message);
                Logger.DebugLog2(exp.Message);
                return false;
            }
            return true;
        }
        public static bool SyncPurchaseOrder(CRM_OutStockDetail billdetail)
        {
            try
            {
                var bill = new K3Cloud_PurchaseOrder()
                {
                    Model =
                    {

                    }
                };
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return false;
        }
        public static bool SyncAllItemsFromOutStockBill(List<DIS_SaleReport> billList, KingdeeApi kai, Robam_CRM robamcrm,Robam_Distribution robamdis)
        {
            //获取物料代码
            var allitems = kai.GetAllItemsNumber();
            foreach (var item in billList)
                {
                int fid = 0;
                //创建库存状态
                string status_ = (item.商品状态).Substring(item.商品状态.IndexOf('-') + 1, item.商品状态.Length - item.商品状态.IndexOf('-') - 1);
                if (kai.CheckProductstatusExists(status_) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateProductstatus(status_, status_) != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建物料状态失败!", true);
                    }
                }
                //同步单据类型
                if(kai.CheckOutBillTypeExist(item.单据类型) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateOutBillType(item.单据类型, item.单据类型) != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建单据类型失败!", true);
                    }
                }
                //检查渠道的父分组存在
                if (kai.CheckChannelGroupExist("001", out fid) != KingdeeApi.CheckResult.ItemExists)
                {
                    if (kai.CreateChannelGroup("001", "老板电器") != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建父分组失败!", true);
                    }
                }
                //创建渠道
                if (!String.IsNullOrEmpty(item.渠道商) && kai.CheckChannelExist(item.渠道商) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateChannel(item.渠道商, item.渠道商名称, "001") != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建渠道商失败!", true);
                    }
                }
                //创建导购员分组
                if(kai.CheckSalerGroupExists("001",out fid) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateSalerGroup("001", "老板电器") != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建业务员分组失败!", true);
                    }
                }
                //创建导购员
                if (!String.IsNullOrEmpty(item.导购员) && kai.CheckSalerExists(item.导购员) != KingdeeApi.CheckResult.ItemExists)
                {
                    if(kai.CreateSaler(item.导购员, item.导购员名称, "001") != KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建业务员失败!", true);
                    }
                }
                //创建门店分组
                if(kai.CheckShopGroupExists("001",out fid) != KingdeeApi.CheckResult.ItemExists)
                {
                    //如果门店不存在，同步一遍所有门店 2022-11-14
                    if(kai.CreateStockGroup("001", "老板电器") == KingdeeApi.CreateResult.AllSuccess)
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("创建门店分组失败!", true);
                    }
                } 
                //创建门店
                if(!String.IsNullOrEmpty(item.门店) && kai.CheckShopExists(item.门店) != KingdeeApi.CheckResult.ItemExists)
                {
                    //同步所有门店 否 开始
                    var d = robamdis.GetShopDetail(item.门店);
                    if(d!= null)
                    {
                        //创建二级渠道
                        if(SG_SecondChannel == null)
                        {
                            SG_SecondChannel = robamdis.GetSecondChannelType();
                            if(SG_SecondChannel?.response?.data == null)
                            {
                                RecordStep<Sqlite_Models_Outstock_importSteps>("没能获取二级渠道编码对应的名称!", true);

                            }
                            else
                            {
                                //
                                if(kai.CheckSecondChannelExists(d.response.data.market_level) != KingdeeApi.CheckResult.ItemExists)
                                {
                                    var i1 = SG_SecondChannel.response.data.Where(i => d.response.data.market_level.Contains(i.option_value) || i.option_value.Contains(d.response.data.market_level)).FirstOrDefault();
                                    if (i1 != null)
                                    {
                                        if(kai.CreateSecondChannel(i1.option_value, i1.option_name) != KingdeeApi.CreateResult.AllSuccess)
                                        {
                                            RecordStep<Sqlite_Models_Outstock_importSteps>("创建二级渠道失败!", true);
                                        }
                                    }
                                }
                            }
                        }
                        //创建省
                        if(!string.IsNullOrEmpty(d?.response?.data?.province))
                        {
                            if (kai.CheckProvinceExists(d.response.data.province) != KingdeeApi.CheckResult.ItemExists)
                            {
                                if(kai.CreateProvince(d.response.data.province, d.response.data.province_name) != KingdeeApi.CreateResult.AllSuccess){
                                    RecordStep<Sqlite_Models_Outstock_importSteps>("创建省失败!", true);
                                }
                            }
                        }
                        //创建市
                        if (!string.IsNullOrEmpty(d?.response?.data?.city))
                        {
                            if (kai.CheckCityExists(d.response.data.city) != KingdeeApi.CheckResult.ItemExists)
                            {
                                if (kai.CreateCity(d.response.data.city, d.response.data.city_name) != KingdeeApi.CreateResult.AllSuccess)
                                {
                                    RecordStep<Sqlite_Models_Outstock_importSteps>("创建市失败!", true);
                                }
                            }
                        }
                        //创建区域
                        if (!string.IsNullOrEmpty(d?.response?.data?.county_district))
                        {
                            if (kai.CheckAreaExists(d.response.data.county_district) != KingdeeApi.CheckResult.ItemExists)
                            {
                                if (kai.CreateArea(d.response.data.county_district, d.response.data.county_district_name) != KingdeeApi.CreateResult.AllSuccess)
                                {
                                    RecordStep<Sqlite_Models_Outstock_importSteps>("创建区域失败!", true);
                                }
                            }
                        }

                        if (kai.CreateShop(item.门店, item.门店名称,"",(d?.response?.data?.province??""),(d?.response?.data?.city??""),(d?.response?.data?.county_district??""),(d?.response?.data?.market_level??"")) != KingdeeApi.CreateResult.AllSuccess)
                        {
                            RecordStep<Sqlite_Models_Outstock_importSteps>("创建门店失败!", true);
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("获取门店详情时失败!", true);
                    }
                }
                //创建物料
                //判断物料存在
                if (!allitems.Contains(item.实际销售码))
                {
                    var itemdetail = robamcrm.GetItemDetail(item.实际销售码);

                    if (itemdetail.materials.Any())
                    {
                        var ins = itemdetail.materials.FirstOrDefault();
                        //创建单位
                        if (kai.CheckUnitExist(ins.unitCode, out fid) != KingdeeApi.CheckResult.ItemExists)
                        {
                            if (kai.CreateUnit(ins.unitCode, ins.unitCode) != KingdeeApi.CreateResult.AllSuccess)
                            {
                                //创建失败
                                RecordStep<Sqlite_Models_Outstock_importSteps>("创建单位失败！", true);
                            }
                        }
                        //创建物料分组
                        if (kai.CheckItemGroupExist(ins.materialType, out fid) != KingdeeApi.CheckResult.ItemExists)
                        {
                            //创建物料
                            if (kai.CreateItemGroup(ins.materialType, ins.materialTypeName) != KingdeeApi.CreateResult.AllSuccess)
                            {
                                //kai.CreateItem(item.actual_selling_goods,item.bill_model_name, itemdetail.materials[0].)
                                //创建失败
                                RecordStep<Sqlite_Models_Outstock_importSteps>("创建物料分组失败!", true);
                            }
                            else
                            {

                            }
                        }
                        //创建物料
                        if (kai.CheckItemExist(ins.materialCode) != KingdeeApi.CheckResult.ItemExists)
                        {
                            var stock = kai.GetAStock();
                            if (kai.CreateItem(ins.materialCode, ins.materialName, ins.specification, ins.unitCode, stock, ins.materialType) != KingdeeApi.CreateResult.AllSuccess)
                            {
                                //创建失败
                                RecordStep<Sqlite_Models_Outstock_importSteps>("创建物料失败!", true);
                            }
                        }
                    }
                    else
                    {
                        RecordStep<Sqlite_Models_Outstock_importSteps>("获取物料失败！", true);
                    }
                }
                
                //return false;
            }
            return true;
        }
        public static bool SyncSaleOrder(DIS_SaleorderDetail billdetail)
        {
            return false;
        }
        public static bool SyncAllItemsFromRealOutStockBill()
        {

            return false;
        }
        public static string DecryptPassword(string password)
        {
            try
            {
                using (var engine = new V8ScriptEngine())
                {
                    engine.DocumentSettings.AccessFlags = Microsoft.ClearScript.DocumentAccessFlags.EnableFileLoading;
                    engine.DefaultAccess = Microsoft.ClearScript.ScriptAccess.Full; // 这两行是为了允许加载js文件

                    string scriptContent = string.Empty;
                    string filePath = "";
                    //加载js文件 D:\Kingdee_Files\Sync_WebSite\publish
                    //if (File.Exists(@"./crypto-js.js"))
                    //{
                    //    filePath = @"./crypto-js.js";
                    //}
                    //else if(File.Exists("D:\\Kingdee_Files\\Sync_WebSite\\publish\\crypto-js.js"))
                    //{
                    //    filePath = "D:\\Kingdee_Files\\Sync_WebSite\\publish\\crypto-js.js";
                    //}
                    //else
                    //{
                    //    Logger.log("未获取到解析密码JS脚本");
                    //    return null;
                    //}
                    
                    //using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    //{
                    //    using (StreamReader sr = new StreamReader(fs))
                    //    {
                    //        scriptContent = sr.ReadToEnd().Replace("\r\n", "");
                    //    }
                    //}
                    scriptContent = Resource.crypto_js.Replace("\r\n", "");
                    //scriptContent = @"function jia(a,b) {return a+b;}";
                    engine.Execute(scriptContent);  // 取得脚本里的所有内容，Execute一下，然后，调用engine.Script.func(x,y)执行一下。

                    var result = engine.Script.DecryptPassword(password);
                    return result;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return null;
        }
        public static string ReadFile(string filePath)
        {
            string ret = "";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    ret = sr.ReadToEnd().Replace("\r\n", "");
                }
            }
            return ret;
        }
        public static void SyncAllShops()
        {
            var acc = KingdeeApi.GetRobamAccountListForList()?.Where(i => i.ServerType == RobamApi.ServerType.Robam_Distribution)?.FirstOrDefault();
            if(acc == null)
            {
                Logger.log("获取分销账户失败!");
                return;
            }
            var robam = new RobamApi.Robam_Distribution(new RobamApi.UserAccount() { FAccount = acc.FAccount, FPWD = acc.FPWD });
            var s = robam.GetShops();
            if(s?.response?.data?.datas != null)
            {
                for(int i = 0;i < s.response.data.datas.Count; i++)
                {

                }
            }
            else
            {
                Logger.log("门店没有获取到任何数据!");
                return;
            }
        }
        public static bool updateLocalDB(string sql)
        {
            //public int ExecuteQuery(string sql)
            
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=127.0.0.1;Initial Catalog=AIS20220920105125;Integrated Security=TRUE"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout= 0;
                    var reader = cmd.ExecuteNonQuery();
                    if(reader > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.log( "ExecuteQuery:Exception:" + exp.Message + "->" + sql );
            }
            return false;
            
        }
        public static K3Cloud_OutStockBill TranslateCRMToKingdeeCloud(CRM_OutStockDetail crmbill)
        {
            try
            {
                K3Cloud_OutStockBill ins = new K3Cloud_OutStockBill();

                ins.Model = new Model()
                {

                };
                return null;
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return null;
        }
        public static K3Cloud_Bill_Model_XSTHD TranslateCRMToKingdeeCloud(CRM_PartsReturn crmbill)
        {
            var k3bill = new K3Cloud_Bill_Model_XSTHD();
            var entryList = new K3Cloud_Bill_Model_XSTHD_Model_FEntity();
            foreach (var i in crmbill.crmsoorderheaders)
            {
                var ins = new K3Cloud_Bill_Model_XSTHD_Model_FEntity();
                //ins.FMaterialId = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = i. };
            }
            return k3bill;
        }
        public static Sqlite_Models_Result_TableMessage StaticMessage(string billType,string tips = "",bool finish = false)
        {
            
            var ret = billType.ToUpper() switch
            {
                "INSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Instock>().Select(i => i.Format()).ToList() ,syncMessage = new Result_TableMessage_syncMessage() {isDone = finish,tips = tips } },
                "OUTSTOCK" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_Outstock>().Select(i => i.Format()).ToList(), syncMessage = new Result_TableMessage_syncMessage() { isDone = finish, tips = tips } },
                "QTXXTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_QTXXTB>().Select(i => i.Format()).ToList(), syncMessage = new Result_TableMessage_syncMessage() { isDone = finish, tips = tips } },
                "JCZLTB" => new Sqlite_Models_Result_TableMessage() { rowData = Sqlite_Helper_Static.read<Sqlite_Models_JCZLTB>().Select(i => i.Format()).ToList(), syncMessage = new Result_TableMessage_syncMessage() { isDone = finish, tips = tips } },
            };
            return ret;
        }
        public static string GetExecutePercent(int currentIndex,int maxCount)
        {
            if (maxCount == 0) return "100%";
            if (currentIndex == 0) return "0%";
            var d = (((decimal)currentIndex / (decimal)maxCount) * 100).ToString();
            return d.Substring(0, 2).Replace(".","") + "%";
        }
    }
}
