using ZJF;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ZJF.ZJF_WEBAPI;
using static Utils.RobamApi;

namespace Utils
{
    /*
     基础资料生成：
        只要编码不为空，则生成对应基础资料
        如果没有名称，则名称和编码一致
     */
    public class KingdeeApi
    {
        static string host , acctid, username, pwd ;
        static List<UserAccount> acctList = new List<UserAccount>();
        static volatile bool acctListGet = false;
        static object m_lock = new object();
        #region A1
        public enum CreateResult
        {
            LoginFailed = 0,
            CreateFailed = 1,
            CommitFailed = 2,
            AuditFailed = 3,
            AllSuccess = 4,
            UnknownError = 5,
            ParameterIsNull = 6,
            ItemAlreadyExists = 7,
        }
        public enum CheckResult
        {
            LoginFailed = 0,
            UnknownError = 1,
            ItemExists = 2,
            ItemNotExists = 3,
            QueryError = 4,
            ParameterIsNull = 5
        }
        public enum SyncResult
        {
            AllSuccess = 0,
            ErrorDuringSync = 1,

        }
        #endregion A1
        public static void InitAccount(string _host, string _acctid, string _username, string _pwd)
        {
            host = _host;
            acctid = _acctid;
            username = _username;
            pwd = _pwd;
            TryGetAccount();
        }
        public static void TryGetAccount()
        {
            lock (m_lock)
            {
                if (!acctListGet)
                {
                    if (GetRobamAccountList())
                    {
                        acctListGet = true;
                    }
                }
            }
        }
        public static bool GetRobamAccountList()
        {
            try
            {
                if (ZJF_WEBAPI.init(host, acctid, username, pwd))
                {
                    acctList.Clear();
                    var filter = new K3Cloud_Current_Query()
                    {
                        FormId = "PLYE_AccountMsg",
                        FilterString = "",
                        FieldKeys = "FAccount,FPWD,FAccountType,FAccountArea,FAccountRight,FCompany.FNumber"
                    };
                    var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                    var jrr = JArray.Parse(queryResult);
                    if (jrr.Count == 0)
                    {
                        return false;
                    }
                    else if (jrr.Count > 0)
                    {
                        foreach (var item in jrr)
                        {
                            var acc = new UserAccount();
                            acc.FAccount = item[0].ToString();
                            acc.FPWD = item[1].ToString();
                            acc.FAccountType = item[2].ToString();
                            acc.FAccountArea = item[3].ToString();
                            acc.FAccountRight = item[4].ToString();
                            acc.FCompany = item[5].ToString();
                            acctList.Add(acc);
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return false;
        }
        public static List<UserAccount> GetRobamAccountListForList()
        {
            if (GetRobamAccountList())
            {
                return acctList;
            }
            else
            {
                return null;
            }
        }
        public static void MatchAccountDo(Func<UserAccount,bool> pre,Action<UserAccount> act)
        {
            var accl = acctList.Where(pre).ToList();
            foreach(var acci in accl)
            {
                act.Invoke(acci);
            }
        }
        public static List<UserAccount> GetMatchAccount(Func<UserAccount, bool> pre)
        {
            return acctList.Where(pre).ToList();
        }
        public string m_ErrorMessage { get; private set; }
        public bool m_InitSuccess { get; private set; } = false;
        public KingdeeApi()
        {
            if (ZJF_WEBAPI.init(host, acctid, username, pwd))
            {
                //if (!acctListGet)
                //{
                //    if (GetRobamAccountList())
                //    {
                //        acctListGet = true;
                //    }
                //}
                TryGetAccount();
                m_InitSuccess = true;
            }
        }
        public bool checkLogin()
        {
            if (m_InitSuccess)
            {
                return true;
            }
            else
            {
                if(ZJF_WEBAPI.init(host, acctid, username, pwd))
                {
                    m_InitSuccess = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public K3Cloud_Unit aDefaultUnit(string funitnumber, string funitname)
        {
            return  new K3Cloud_Unit() { 
                Model = new K3Cloud_Unit_Model() {
                    FNumber = funitnumber,
                    FName = funitname,
                    FUnitGroupId = new K3Cloud_Unit_Model_FUnitGroupId() { 
                        FNumber = "Quantity" } ,
                    FPrecision = 0,
                    SubHeadEntity = new K3Cloud_Unit_Model_SubHeadEntity() { 
                        FConvertType = "0",
                        FConvertDenominator = 1.0000000000m,
                        FConvertNumerator = 1.0000000000m
                    }
                }
            };
        }
        public K3Cloud_Item aDefaultItem(string fitemnumber, string fitemname, string fitemmodel, string funitnumber, string fstocknumber,string fgroup)
        {
            return new K3Cloud_Item()
            {
                Model =  {
                        FNumber = fitemnumber,
                        FName = fitemname,
                        FSpecification = fitemmodel,
                        FMaterialGroup = {FNumber = fgroup },
                        FDSMatchByLot = false,
                        FIsSalseByNet = false,
                        SubHeadEntity =
                        {
                            FBaseUnitId = { FNUMBER = funitnumber},
                            FCategoryID =  {FNUMBER = "CHLB05_SYS" },
                        },
                        SubHeadEntity1 =
                        {
                            FStoreUnitID =  {FNumber = funitnumber},
                            FCurrencyId = { FNumber = "PRE001" },
                            FStockId = { FNumber = fstocknumber },
                        },
                        SubHeadEntity2 =
                        {
                            FSalePriceUnitId = {FNumber = funitnumber},
                            FSaleUnitId = {FNumber = funitnumber},
                        },
                        SubHeadEntity3 =
                        {
                            FPurchaseUnitId = { FNumber = funitnumber },
                            FPurchasePriceUnitId = { FNumber = funitnumber },
                        },
                        SubHeadEntity4 =
                        {
                            FMfgPolicyId = {FNumber = "ZZCL001_SYS" },
                        },
                        SubHeadEntity5 =
                        {
                            FMinIssueUnitId = {FNumber = funitnumber},
                            FStandHourUnitId = "3600",
                        },
                    }
            };
        }
        public K3Cloud_Supplier aDefaultSuppiler(string fsuppliernumber, string fsuppliername, string groupnumber)
        {
            return new K3Cloud_Supplier() { 
                Model =
                {
                    FName = fsuppliername,
                    FNumber = fsuppliernumber,
                    FGroup = {FNumber = groupnumber },
                    FFinanceInfo =
                    {
                        FPayCurrencyId = {FNumber = "PRE001" }
                    },
                }
            };
        }
        public K3Cloud_Customer aDefaultCustomer(string fcustomernumber,string fcustomername,string fgroup)
        {
            return new K3Cloud_Customer() {
                Model =
                {
                    FNumber = fcustomernumber,
                    FName = fcustomername,
                    FGroup = {FNumber = fgroup},
                    FTRADINGCURRID = {FNumber = "PRE001" },

                }
            };
        }
        //单位 分组已经预设
        public CheckResult CheckUnitExist(string funitnumber,out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(funitnumber))
                {
                    return CheckResult.ParameterIsNull;
                }


                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FormId = "BD_UNIT",
                    FilterString = "FNumber = '" + funitnumber + "'",
                    FieldKeys = "FUnitID"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if(jrr.Count > 0)
                {
                    fid = jrr[0][0].Value<int>();
                    return CheckResult.ItemExists;
                }

            }
            catch(Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
                return CheckResult.UnknownError;
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateUnit(string funitnumber,string funitname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(funitnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(funitname))
                {
                    funitname = funitnumber;
                }
                if (!checkLogin())
                {
                    return CreateResult.LoginFailed;
                }
                var u = aDefaultUnit(funitnumber, funitname);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
                var ir = JObject.Parse(ret).ToObject<K3Cloud_Unit_Result>();
                if(ir.Result.Id != 0)
                {
                    var commitpara = new K3Cloud_Unit_Commit() { Ids = ir.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_UNIT", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var auditpara = new K3Cloud_Unit_Audit() { Ids = ir.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_UNIT", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return  CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                } 
            }
            catch(Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
                return CreateResult.CreateFailed;
            }
            return CreateResult.UnknownError;

        }
        //物料 分组
        public CheckResult CheckItemGroupExist(string fproductgroup, out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fproductgroup))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = "SAL_MATERIALGROUP",
                    FilterString = "FNumber = '" + fproductgroup + "'",
                    FieldKeys = "FID,FParentId,FNumber,FName"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    fid = Convert.ToInt32(jrr[0][0].ToString());
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateItemGroup(string fproductGroupnumber, string fproductGroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fproductGroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fproductGroupname))
                {
                    fproductGroupname = fproductGroupnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckItemGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckItemGroupExist(fproductGroupnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Item_Group()
                        {
                            FNumber = fproductGroupnumber,
                            FName = fproductGroupname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_MATERIAL", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_MATERIAL", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckItemGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckItemGroupExist(fproductGroupnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Item_Group()
                                {
                                    FNumber = fproductGroupnumber,
                                    FName = fproductGroupname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_MATERIAL", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckItemExist(string fitemnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fitemnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FormId = "BD_MATERIAL",
                    FilterString = "FNumber = '" + fitemnumber + "'",
                    FieldKeys = "FMaterialID"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch(Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateItem(string fitemnumber, string fitemname,string fitemmodel,string funitnumber,string fstocknumber,string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fitemnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fitemname))
                {
                    fitemname = fitemnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fitemmodel))
                {
                    fitemmodel = "";
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(funitnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fstocknumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    fgroup = "";
                    //return CreateResult.ParameterIsNull;
                }

                var item = aDefaultItem(fitemnumber,fitemname, fitemmodel??"", funitnumber, fstocknumber, fgroup);
                string para = JsonConvert.SerializeObject(item);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_MATERIAL", para });

                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null  && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_MATERIAL", JsonConvert.SerializeObject(commitpara) });
                    if(JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_MATERIAL", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CreateResult CreateItem(CRM_ItemDetail ci)
        {
            try
            {
                if (ci != null && ci.materials?.Count > 0)
                {
                    //除重
                    var filter = new K3Cloud_Current_Query()
                    {
                        FormId = "BD_MATERIAL",
                        FilterString = "",
                        FieldKeys = "FNumber"
                    };
                    var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                    var j = JArray.Parse(queryResult);
                    var j2 = j.ToObject<List<List<string>>>();
                    var jrr = j2.Select(i => i.Any() ? i[0] : "").ToList<string>();
                    var vallist = ci.materials.Where(i => !jrr.Contains(i.materialCode)).ToList();
                    int c = ci.materials.Count;
                    for (int i = 0; i < ci.materials.Count; i++)// var item in ci.materials)
                    {
                        var item = ci.materials[i];
                        int parentId = 0;
                        if (CheckItemGroupExist(item.materialType, out parentId) != CheckResult.ItemExists)
                        {
                            if (CreateItemGroup(item.materialType, item.materialTypeName) != CreateResult.AllSuccess)
                            {
                                Utils.RecordStep<Sqlite_Models_MaterialSync>("分组" + item.materialType + ":" + item.materialTypeName + " 创建失败!", true);
                                continue;
                            }
                            else
                            {
                                Utils.RecordStep<Sqlite_Models_MaterialSync>("分组" + item.materialType + ":" + item.materialTypeName + " 创建成功!");
                            }
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_MaterialSync>("分组" + item.materialType + ":" + item.materialTypeName + " 已经存在!");
                        }
                        if (CheckItemExist(item.materialCode) != CheckResult.ItemExists)
                        {
                            if (CreateItem(item.materialCode, item.materialName, item.specification, item.unitCode, "sjzb003001", item.materialType) != CreateResult.AllSuccess)
                            {
                                Utils.RecordStep<Sqlite_Models_MaterialSync>(c.ToString() + "/" + i.ToString() + " 物料" + item.materialCode + ":" + item.materialName + " 创建失败!", true);
                                continue;
                            }
                            else
                            {
                                Utils.RecordStep<Sqlite_Models_MaterialSync>(c.ToString() + "/" + i.ToString() + " 物料" + item.materialCode + ":" + item.materialName + " 创建成功!");
                            }
                        }
                        else
                        {
                            Utils.RecordStep<Sqlite_Models_MaterialSync>(c.ToString() + "/" + i.ToString() + " 物料" + item.materialCode + ":" + item.materialName + " 已经存在!");
                        }
                    }
                }
                else
                {
                    return CreateResult.UnknownError;
                }
                return CreateResult.AllSuccess;
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public List<string> GetAllItemsNumber()
        {
            
            try
            {
                List<string> list = new List<string>();
                if (!checkLogin())
                {
                    return null;
                }
                int addcount = 0;
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = "BD_MATERIAL",
                    FilterString = "",
                    FieldKeys = "FNumber",
                    Limit = 10000,
                    StartRow = 0,
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                var jrr = JArray.Parse(queryResult);
                foreach (var item in jrr)
                {
                    list.Add(item[0].ToString());
                }
                while (jrr.Count == 10000)
                {
                    queryResult = "";
                    addcount++;
                    filter[0].StartRow = addcount * 10000 + 1;
                    queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, JsonConvert.SerializeObject(filter));
                    jrr = JArray.Parse(queryResult);
                    foreach (var item in jrr)
                    {
                        list.Add(item[0].ToString());
                    }
                }
               
                return list;
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        //供应商 分组
        public CheckResult CheckSupplierGroupExist(string fsuppliergroup, out int fid)
        {
            fid = 0;
            try
            {
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    
                    FormId = "BD_Supplier",
                } ;
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });
                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if(jrr.Result.NeedReturnData.Where(i=>i.FNUMBER == fsuppliergroup).Any())
                    {
                        fid = jrr.Result.NeedReturnData.FirstOrDefault().FID;
                        return CheckResult.ItemExists;
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                    
                }
                else 
                {
                    return CheckResult.QueryError;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSupplierGroup(string fsuppliergroupnumber, string fsuppliergroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsuppliergroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsuppliergroupname))
                {
                    fsuppliergroupname = fsuppliergroupnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CreateResult.LoginFailed;
                }
                int fid = 0;
                if(CheckSupplierGroupExist("001",out fid) != CheckResult.ItemExists)
                {
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                        FParentId = 0
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Supplier", para });
                    if (CheckSupplierGroupExist("001", out fid) == CheckResult.ItemExists)
                    {
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            //return CreateResult.AllSuccess;
                            var currentparent = new K3Cloud_Item_Group()
                            {
                                FNumber = fsuppliergroupnumber,
                                FName = fsuppliergroupname,
                                FParentId = fid
                            };
                            string para2 = JsonConvert.SerializeObject(currentparent);
                            var currentret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Supplier", para2 });
                            if (JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                            {
                                return CreateResult.AllSuccess;
                            }
                            else
                            {
                                return CreateResult.CreateFailed;
                            }
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                }
                else
                {
                    var currentparent = new K3Cloud_Item_Group()
                    {
                        FNumber = fsuppliergroupnumber,
                        FName = fsuppliergroupname,
                        FParentId = fid
                    };
                    var para = JsonConvert.SerializeObject(currentparent);
                    var currentret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Supplier", para });
                    if (JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckSupplierExist(string fsuppliernumber)
        {
            try
            {
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FNumber",
                    FormId = "BD_Supplier",
                    FilterString = "FNumber = '" + fsuppliernumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] {  JsonConvert.SerializeObject(filter) });
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSupplier(string fsuppliergroupnumber, string fsuppliergroupname, string groupnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsuppliergroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsuppliergroupname))
                {
                    fsuppliergroupname = fsuppliergroupnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(groupnumber))
                {
                    groupnumber = "";
                    //return CreateResult.ParameterIsNull;
                }
                var suppiler = aDefaultSuppiler(fsuppliergroupnumber,fsuppliergroupname,groupnumber);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_Supplier", JsonConvert.SerializeObject(suppiler) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var commitpara = new K3Cloud_Item_Commit() { Ids = JObject.Parse(ret).SelectToken("Result.['Id']").Value<string>()};
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_Supplier", JsonConvert.SerializeObject(commitpara) });
                    var commitObj = JObject.Parse(commitret);
                    if (commitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && commitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var auditpara = new K3Cloud_Item_Audit() { Ids = JObject.Parse(ret).SelectToken("Result.['Id']").Value<string>()};
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_Supplier", JsonConvert.SerializeObject(auditpara) });
                        var aduitObj = JObject.Parse(auditret);
                        if (aduitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && aduitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //客户
        public CheckResult CheckCustomerGroupExist(string fcustomergroup, out int fid)
        {
            fid = 0;
            try
            {
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "BD_Customer",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para } );
                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if (jrr.Result.NeedReturnData.Where(i=>i.FNUMBER == fcustomergroup).Any())
                    {
                        fid = jrr.Result.NeedReturnData.FirstOrDefault().FID;
                        return CheckResult.ItemExists;
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateCustomerGroup(string fcustomergroupnumber,string fcustomergroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcustomergroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fcustomergroupname))
                {
                    fcustomergroupname = fcustomergroupnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CreateResult.LoginFailed;
                }
                int fid = 0;
                if (CheckCustomerGroupExist("001", out fid) != CheckResult.ItemExists)
                {
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                        FParentId = 0
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Customer", para });
                    if (CheckCustomerGroupExist("001", out fid) == CheckResult.ItemExists)
                    {
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            //return CreateResult.AllSuccess;
                            var currentparent = new K3Cloud_Item_Group()
                            {
                                FNumber = fcustomergroupnumber,
                                FName = fcustomergroupname,
                                FParentId = fid
                            };
                            string para2 = JsonConvert.SerializeObject(currentparent);
                            var currentret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Customer", para2 });
                            if (JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                            {
                                return CreateResult.AllSuccess;
                            }
                            else
                            {
                                return CreateResult.CreateFailed;
                            }
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                }
                else
                {
                    var currentparent = new K3Cloud_Item_Group()
                    {
                        FNumber = fcustomergroupnumber,
                        FName = fcustomergroupname,
                        FParentId = fid
                    };
                    var para = JsonConvert.SerializeObject(currentparent);
                    var currentret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Customer", para });
                    if (JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(currentret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckCustomerExist(string fcustomernumber)
        {
            try
            {
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FNumber",
                    FormId = "BD_Customer",
                    FilterString = "FNumber = '" + fcustomernumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] {  JsonConvert.SerializeObject(filter) });
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateCustomer(string fcustomernumber, string fcustomername,string groupnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcustomernumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fcustomername))
                {
                    fcustomername = fcustomernumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(groupnumber))
                {
                    groupnumber = "";
                    //return CreateResult.ParameterIsNull;
                }
                var customer = aDefaultCustomer(fcustomernumber, fcustomername, groupnumber);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_Customer", JsonConvert.SerializeObject(customer) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var commitpara = new K3Cloud_Item_Commit() { Ids = JObject.Parse(ret).SelectToken("Result.['Id']").Value<string>() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_Customer", JsonConvert.SerializeObject(commitpara) });
                    var commitObj = JObject.Parse(commitret);
                    if (commitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && commitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var auditpara = new K3Cloud_Item_Audit() { Ids = JObject.Parse(ret).SelectToken("Result.['Id']").Value<string>() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_Customer", JsonConvert.SerializeObject(auditpara) });
                        var aduitObj = JObject.Parse(auditret);
                        if (aduitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && aduitObj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //仓库
        public CheckResult CheckStockGroupExist(string fstocknumber, out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fstocknumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "BD_STOCK",
                } ;
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });
                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if(jrr.Result.NeedReturnData.Count > 0)
                    {
                        if (jrr.Result.NeedReturnData.Select(i => i.FNUMBER).ToList<string>().Contains(fstocknumber))
                        {
                            return CheckResult.ItemExists;
                        }
                        return CheckResult.ItemNotExists;
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateStockGroup(string fstocknumber, string fstockname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fstocknumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fstockname))
                {
                    fstockname = fstocknumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckStockGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckStockGroupExist(fstocknumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Item_Group()
                        {
                            FNumber = fstocknumber,
                            FName = fstockname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_STOCK", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_STOCK", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckItemGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckItemGroupExist(fstocknumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Item_Group()
                                {
                                    FNumber = fstocknumber,
                                    FName = fstockname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_STOCK", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckStockExist(string fcustomernumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcustomernumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FStockID",
                    FormId = "BD_STOCK",
                    FilterString = "FNumber = '" + fcustomernumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateStock(string fstocknumber, string fstockname,string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fstocknumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fstockname))
                {
                    fstockname = fstocknumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    fgroup = "";
                    //return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Stock() {
                    Model =
                    {
                        FNumber = fstocknumber,
                        FName = fstockname,
                        FStockStatusType = "0,1,2,3,4,5,6,7,8",
                        FSortingPriority = 1,
                        FGroup = {FNumber = fgroup }
                    }
                };
                string savepara = JsonConvert.SerializeObject(stock);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_STOCK", savepara });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_STOCK", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_STOCK", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //创建公司单位
        public CheckResult CheckComanyGroupExist(string fcomanynumber, out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fcomanynumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "PLYE_Company",
                } ;
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });
                
                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    //if (jrr.Count == 0)
                    //{
                    //    return CheckResult.ItemNotExists;
                    //}
                    //else if (jrr.Count > 0)
                    //{
                    //    fid = Convert.ToInt32(jrr[0][0].ToString());
                    //    return CheckResult.ItemExists;
                    //}
                    if(jrr.Result.NeedReturnData.Count > 0)
                    {
                        if (jrr.Result.NeedReturnData.Select(i => i.FNUMBER).ToList<string>().Contains(fcomanynumber))
                        {
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }
               
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateComanyGroup(string fcompanynumber, string fcompanyname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcompanynumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fcompanyname))
                {
                    fcompanyname = fcompanynumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckComanyGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckComanyGroupExist(fcompanynumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Item_Group()
                        {
                            FNumber = fcompanynumber,
                            FName = fcompanyname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Company", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Company", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckItemGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckItemGroupExist(fcompanynumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Item_Group()
                                {
                                    FNumber = fcompanynumber,
                                    FName = fcompanyname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Company", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckComanyExist(string fcompanynumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcompanynumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys="FID",
                    FormId = "PLYE_Company",
                    FilterString = "FNumber = '" + fcompanynumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                
                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateComany(string fcompanynumber, string fcompanyname, string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcompanynumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fcompanyname))
                {
                    fcompanyname = fcompanynumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Company()
                {
                    Model =
                    {
                        FNumber = fcompanynumber,
                        FName = fcompanyname,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup }
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Company", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Company", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Company", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //创建入库单据类型
        public CheckResult CheckBillTypeExist(string billtypenumber,string billform)
        {
            try
            {
                billform = billform.ToUpper();
                if (string.IsNullOrWhiteSpace(billtypenumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                var para = new K3Cloud_BillType_View() { 
                    Number = billform + "_" + billtypenumber
                };
                //var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "BOS_BillType", JsonConvert.SerializeObject(para) });
                var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "BOS_BillType", JsonConvert.SerializeObject(para) });
                var kbr = JObject.Parse(ret).ToObject<K3Cloud_BillType_QueryList>();
                if (!kbr.Result.ResponseStatus.IsSuccess)
                {
                    if (kbr.Result.ResponseStatus.Errors.Count > 0 && kbr.Result.ResponseStatus.Errors[0].Message == "传递的编码值不存在")
                    {
                        return CheckResult.ItemNotExists;
                    }
                    else
                    {
                        return CheckResult.UnknownError;
                    }
                    
                }
                else if(kbr.Result.ResponseStatus.IsSuccess)
                {
                    //判断formid值
                    if(kbr.Result.Result.BillFormID_Id.ToUpper() == billform.ToUpper())
                    {
                        return CheckResult.ItemExists;
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                    
                }
            }
            catch(Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateBillType(string billtypenumber,string billtypename,string billform)
        {
            try
            {
                billform = billform.ToUpper();
                if (string.IsNullOrWhiteSpace(billtypenumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(billtypename))
                {
                    billtypename = billtypenumber;
                    //return CreateResult.ParameterIsNull;
                }
                var para = new K3Cloud_BillType_Create() { 
                    Model =
                    {
                        FBillFormID = { FNumber = billform },
                        FNumber = billform + "_" + billtypenumber,
                        FName = billtypename,
                    }
                };
                var paraTxt = JsonConvert.SerializeObject(para);
                //var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BOS_BillType", paraTxt });
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BOS_BillType", paraTxt });
                var saveret = JObject.Parse(ret).ToObject<K3Cloud_BillType_Save>();
                if (saveret.Result.ResponseStatus.IsSuccess)
                {
                    var commitpara = new K3Cloud_BillType_Commit() { 
                        Numbers = { billform + "_" + billtypenumber },
                    };
                    //var commitret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(commitpara) })).ToObject<K3Cloud_BillType_Save>();
                    var commitret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(commitpara) })).ToObject<K3Cloud_BillType_Save>();
                    //var commitret2 = JObject.Parse(commitret).ToObject<k3c>
                    if (commitret.Result.ResponseStatus.IsSuccess)
                    {
                        var auditpara = new K3Cloud_BillType_Audit() { 
                            Numbers = { billform + "_" + billtypenumber }
                        };
                        //var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) });
                        //var auditret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) })).ToObject<K3Cloud_BillType_Save>();
                        var auditret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) })).ToObject<K3Cloud_BillType_Save>();
                        if (auditret.Result.ResponseStatus.IsSuccess)
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //物料状态
        public CheckResult CheckProductstatusExists(string statuscode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(statuscode))
                {
                    return CheckResult.ParameterIsNull;
                }
                var para = new K3Cloud_StockStatus_View()
                {
                    Number = statuscode
                };
                var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "BD_StockStatus", JsonConvert.SerializeObject(para) });
                var kbr = JObject.Parse(ret).ToObject<K3Cloud_BillType_Result>();
                if (!kbr.Result.ResponseStatus.IsSuccess)
                {
                    if (kbr.Result.ResponseStatus.Errors.Count > 0 && kbr.Result.ResponseStatus.Errors[0].Message == "传递的编码值不存在")
                    {
                        return CheckResult.ItemNotExists;
                    }
                    else
                    {
                        return CheckResult.UnknownError;
                    }

                }
                else if (kbr.Result.ResponseStatus.IsSuccess)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateProductstatus(string statuscode, string statusname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(statuscode))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (!string.IsNullOrWhiteSpace(statusname))
                {
                    statuscode = statusname;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(statusname))
                {
                    statusname = statuscode;
                    //return CreateResult.ParameterIsNull;
                }
                var para = new K3Cloud_StockStatus()
                {
                    Model =
                    {
                        FName = statusname,
                        FNumber = statuscode,
                    }
                };
                var paraTxt = JsonConvert.SerializeObject(para);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_StockStatus", paraTxt });
                var saveret = JObject.Parse(ret).ToObject<K3Cloud_BillType_Save>();
                if (saveret.Result.ResponseStatus.IsSuccess)
                {
                    var commitpara = new K3Cloud_StockStatus_Commit()
                    {
                        Numbers = { statuscode },
                    };
                    var commitret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_StockStatus", JsonConvert.SerializeObject(commitpara) })).ToObject<K3Cloud_BillType_Save>();
                    //var commitret2 = JObject.Parse(commitret).ToObject<k3c>
                    if (commitret.Result.ResponseStatus.IsSuccess)
                    {
                        var auditpara = new K3Cloud_StockStatus_Audit()
                        {
                            Numbers = { statuscode }
                        };
                        //var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) });
                        var auditret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_StockStatus", JsonConvert.SerializeObject(auditpara) })).ToObject<K3Cloud_BillType_Save>();
                        if (auditret.Result.ResponseStatus.IsSuccess)
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
            
        }
        public List<K3Cloud_FNumberAndName> GetProductstatusList()
        {
            List<K3Cloud_FNumberAndName> list = new List<K3Cloud_FNumberAndName>();
            try
            {
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FNumber,FName",
                    FormId = "BD_StockStatus",
                    FilterString = ""
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                for(int i = 1; i < jrr.Count; i++)
                {
                    list.Add(new K3Cloud_FNumberAndName() { FNumber = jrr[i][0].ToString(), FName = jrr[i][1].ToString() });
                }
                return list;
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return null;

        }
        //渠道商创建
        public CheckResult CheckChannelGroupExist(string fchannelnumber, out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fchannelnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "PLYE_Channel",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });

                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    //if (jrr.Count == 0)
                    //{
                    //    return CheckResult.ItemNotExists;
                    //}
                    //else if (jrr.Count > 0)
                    //{
                    //    fid = Convert.ToInt32(jrr[0][0].ToString());
                    //    return CheckResult.ItemExists;
                    //}
                    if (jrr.Result.NeedReturnData.Count > 0)
                    {
                        var ins = jrr.Result.NeedReturnData.Where(i => i.FNUMBER == fchannelnumber).FirstOrDefault();// Select(i => i.FNUMBER).ToList<string>().Contains(fchannelnumber));
                        if (ins!=null)
                        {
                            fid = ins.FID;
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateChannelGroup(string fchannelnumber, string fchannelname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fchannelnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fchannelname))
                {
                    fchannelname = fchannelnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckChannelGroupExist(fchannelnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Common.K3Cloud_Common_Group()
                        {
                            FNumber = fchannelnumber,
                            FName = fchannelname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Channel", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Channel", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckChannelGroupExist(fchannelnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Common.K3Cloud_Common_Group()
                                {
                                    FNumber = fchannelnumber,
                                    FName = fchannelname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Channel", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckChannelExist(string fchannelnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fchannelnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Channel",
                    FilterString = "FNumber = '" + fchannelnumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateChannel(string fchannelnumber, string fchannelname, string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fchannelnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fchannelname))
                {
                    fchannelname = fchannelnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Company()
                {
                    Model =
                    {
                        FNumber = fchannelnumber,
                        FName = fchannelname,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup }
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Channel", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Channel", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Channel", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //导购员创建
        public CheckResult CheckSalerGroupExists(string fsalergroupnumber,out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fsalergroupnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "PLYE_Saler",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });

                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if (jrr.Result.NeedReturnData.Count > 0)
                    {
                        var ins = jrr.Result.NeedReturnData.Where(i => i.FNUMBER == fsalergroupnumber).FirstOrDefault();// Select(i => i.FNUMBER).ToList<string>().Contains(fchannelnumber));
                        if (ins != null)
                        {
                            fid = ins.FID;
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSalerGroup(string fsalergroupnumber,string fsalergroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalergroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsalergroupname))
                {
                    fsalergroupname = fsalergroupnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckSalerGroupExists("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckSalerGroupExists(fsalergroupnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Common.K3Cloud_Common_Group()
                        {
                            FNumber = fsalergroupnumber,
                            FName = fsalergroupname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckChannelGroupExist(fsalergroupnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Common.K3Cloud_Common_Group()
                                {
                                    FNumber = fsalergroupnumber,
                                    FName = fsalergroupname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckSalerExists(string fsalernumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalernumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Saler",
                    FilterString = "FNumber = '" + fsalernumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSaler(string fsalernumber,string fsalername,string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalernumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsalername))
                {
                    fsalername = fsalernumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Company()
                {
                    Model =
                    {
                        FNumber = fsalernumber,
                        FName = fsalername,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup }
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //创建部门
        public CheckResult CheckDepartmentGroupExists(string fdepartmentgroupnumber,out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fdepartmentgroupnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "BD_Department",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });

                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if (jrr.Result.NeedReturnData.Count > 0)
                    {
                        var ins = jrr.Result.NeedReturnData.Where(i => i.FNUMBER == fdepartmentgroupnumber).FirstOrDefault();// Select(i => i.FNUMBER).ToList<string>().Contains(fchannelnumber));
                        if (ins != null)
                        {
                            fid = ins.FID;
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateDepartmentGroup(string fdepartmentgroupnumber,string fdepartmentgroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fdepartmentgroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fdepartmentgroupname))
                {
                    fdepartmentgroupname = fdepartmentgroupnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckChannelGroupExist(fdepartmentgroupnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Common.K3Cloud_Common_Group()
                        {
                            FNumber = fdepartmentgroupnumber,
                            FName = fdepartmentgroupname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Department", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Department", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckChannelGroupExist(fdepartmentgroupnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Common.K3Cloud_Common_Group()
                                {
                                    FNumber = fdepartmentgroupnumber,
                                    FName = fdepartmentgroupname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "BD_Department", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckDepartmentExists(string fdepartmentnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fdepartmentnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Saler",
                    FilterString = "FNumber = '" + fdepartmentnumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateDepartment(string fdepartmentnumber,string fdepartmentname, string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fdepartmentnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fdepartmentname))
                {
                    fdepartmentname = fdepartmentnumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Company()
                {
                    Model =
                    {
                        FNumber = fdepartmentnumber,
                        FName = fdepartmentname,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup }
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BD_Department", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BD_Department", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BD_Department", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //业务员创建
        public CheckResult CheckSalesmanGroupExists(string fsalesmangroupexists,out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fsalesmangroupexists))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "PLYE_Saler",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });

                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if (jrr.Result.NeedReturnData.Count > 0)
                    {
                        var ins = jrr.Result.NeedReturnData.Where(i => i.FNUMBER == fsalesmangroupexists).FirstOrDefault();// Select(i => i.FNUMBER).ToList<string>().Contains(fchannelnumber));
                        if (ins != null)
                        {
                            fid = ins.FID;
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSalesmanGroup(string fsalesmangroupnumber,string fsalesmangroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalesmangroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsalesmangroupname))
                {
                    fsalesmangroupname = fsalesmangroupnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckChannelGroupExist(fsalesmangroupnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Common.K3Cloud_Common_Group()
                        {
                            FNumber = fsalesmangroupnumber,
                            FName = fsalesmangroupname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckChannelGroupExist(fsalesmangroupnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Common.K3Cloud_Common_Group()
                                {
                                    FNumber = fsalesmangroupnumber,
                                    FName = fsalesmangroupname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Saler", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckSalesmanExists(string fsalesmannumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalesmannumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Saler",
                    FilterString = "FNumber = '" + fsalesmannumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSalesman(string fsalesmannumber, string fsalesmanname,string fgroup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsalesmannumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fsalesmanname))
                {
                    fsalesmanname = fsalesmannumber;
                    //return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fgroup))
                {
                    return CreateResult.ParameterIsNull;
                }
                var stock = new K3Cloud_Company()
                {
                    Model =
                    {
                        FNumber = fsalesmannumber,
                        FName = fsalesmanname,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup }
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Saler", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //创建门店
        public CheckResult CheckShopGroupExists(string fshopgroupnumber, out int fid)
        {
            fid = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(fshopgroupnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Company_GroupQuery()
                {
                    FormId = "PLYE_Shop",
                };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupquery, new object[] { para });

                var jrr = JObject.Parse(queryResult).ToObject<K3Cloud_Company_GroupQuery_Result>();
                if (jrr.Result.ResponseStatus.IsSuccess)
                {
                    if (jrr.Result.NeedReturnData.Count > 0)
                    {
                        var ins = jrr.Result.NeedReturnData.Where(i => i.FNUMBER == fshopgroupnumber).FirstOrDefault();// Select(i => i.FNUMBER).ToList<string>().Contains(fchannelnumber));
                        if (ins != null)
                        {
                            fid = ins.FID;
                            return CheckResult.ItemExists;
                        }
                    }
                    else
                    {
                        return CheckResult.ItemNotExists;
                    }
                }
                else
                {
                    return CheckResult.QueryError;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateShopGroup(string fshopgroupnumber, string fshopgroupname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fshopgroupnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fshopgroupname))
                {
                    fshopgroupname = fshopgroupnumber;
                    //return CreateResult.ParameterIsNull;
                }

                int pid = 0;
                //判断上级组织是否存在
                if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                {
                    int tpid = 0;
                    if (!(CheckChannelGroupExist(fshopgroupnumber, out tpid) == CheckResult.ItemExists))
                    {
                        var createparent = new K3Cloud_Common.K3Cloud_Common_Group()
                        {
                            FNumber = fshopgroupnumber,
                            FName = fshopgroupname,
                            FParentId = pid
                        };
                        string para = JsonConvert.SerializeObject(createparent);
                        var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Shop", para });
                        if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.CreateFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.ItemAlreadyExists;
                    }
                }
                else
                {
                    //创建分组
                    var createparent = new K3Cloud_Item_Group()
                    {
                        FNumber = "001",
                        FName = "老板电器",
                    };
                    string para = JsonConvert.SerializeObject(createparent);
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Shop", para });
                    if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        if (CheckChannelGroupExist("001", out pid) == CheckResult.ItemExists)
                        {
                            int tpid = 0;
                            //var ins = new K3Cloud_Item_Group() { FNumber="测试",FName="测试",FParentId};
                            if (!(CheckChannelGroupExist(fshopgroupnumber, out tpid) == CheckResult.ItemExists))
                            {
                                var createpara = new K3Cloud_Common.K3Cloud_Common_Group()
                                {
                                    FNumber = fshopgroupnumber,
                                    FName = fshopgroupname,
                                    FParentId = pid
                                };
                                string para2 = JsonConvert.SerializeObject(createpara);
                                var ret1 = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.groupsave, new object[] { "PLYE_Shop", para2 });
                                if (JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret1).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                                {
                                    return CreateResult.AllSuccess;
                                }
                                else
                                {
                                    return CreateResult.CreateFailed;
                                }
                            }
                            else
                            {
                                return CreateResult.ItemAlreadyExists;
                            }
                        }
                    }
                    else
                    {
                        return CreateResult.CreateFailed;
                    }
                }

                //上级组织创建分组
                //ZJF_WEBAPI.sendRepuest("save", new object[] { "BD_UNIT", JsonConvert.SerializeObject(u) });
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        public CheckResult CheckShopExists(string fshopnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fshopnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Shop",
                    FilterString = "FNumber = '" + fshopnumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateShop(string fshopnumber,string fshopname,string fgroup,string province,string city,string area,string secondchannel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fshopnumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(fshopname))
                {
                    fshopname = fshopnumber;
                    //return CreateResult.ParameterIsNull;
                }


                var stock = new K3Cloud_Shop()
                {
                    Model =
                    {
                        FNumber = fshopnumber,
                        FName = fshopname,
                        FCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        FGroup = {FNumber = fgroup },
                        FProvince = {FNumber = province },
                        FCity = {FNumber = city },
                        FArea = {FNumber = area},
                        FSecondChannel = {FNumber = secondchannel},
                    }
                };

                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Shop", JsonConvert.SerializeObject(stock) });
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                    //提交审批
                    var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                    var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Shop", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                        var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Shop", JsonConvert.SerializeObject(commitpara) });
                        if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }
                else
                {
                    return CreateResult.CommitFailed;
                }

            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }

        //创建省
        public CheckResult CheckProvinceExists(string fprovincenumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fprovincenumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Province",
                    FilterString = "FNumber = '" + fprovincenumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateProvince(string fprovincenumber, string fprovincename)
        {
            if (string.IsNullOrWhiteSpace(fprovincenumber))
            {
                return CreateResult.ParameterIsNull;
            }
            if (string.IsNullOrWhiteSpace(fprovincenumber))
            {
                return CreateResult.ParameterIsNull;
            }
            if (!checkLogin())
            {
                return CreateResult.LoginFailed;
            }
            var p = new K3Cloud_Province() { 
                Model =
                {
                    FNumber = fprovincenumber,
                    FName = fprovincename
                }
            };
            var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Province", JsonConvert.SerializeObject(p) });
            if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                //提交审批
                var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Province", JsonConvert.SerializeObject(commitpara) });
                if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                    var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Province", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.AuditFailed;
                    }
                }
                else
                {
                    return CreateResult.CommitFailed;
                }
            }
            return CreateResult.UnknownError;
        }
        //创建市
        public CheckResult CheckCityExists(string fcitynumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fcitynumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_City",
                    FilterString = "FNumber = '" + fcitynumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateCity(string fcitynumber, string fcityname)
        {
            if (string.IsNullOrWhiteSpace(fcitynumber))
            {
                return CreateResult.ParameterIsNull;
            }
            if (string.IsNullOrWhiteSpace(fcityname))
            {
                return CreateResult.ParameterIsNull;
            }
            if (!checkLogin())
            {
                return CreateResult.LoginFailed;
            }
            var p = new K3Cloud_Province()
            {
                Model =
                {
                    FNumber = fcitynumber,
                    FName = fcityname
                }
            };
            var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_City", JsonConvert.SerializeObject(p) });
            if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                //提交审批
                var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_City", JsonConvert.SerializeObject(commitpara) });
                if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                    var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_City", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.AuditFailed;
                    }
                }
                else
                {
                    return CreateResult.CommitFailed;
                }
            }
            return CreateResult.UnknownError;
        }
        //创建地区
        public CheckResult CheckAreaExists(string fareanumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fareanumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_Area",
                    FilterString = "FNumber = '" + fareanumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateArea(string fareanumber, string fareaname)
        {
            if (string.IsNullOrWhiteSpace(fareanumber))
            {
                return CreateResult.ParameterIsNull;
            }
            if (string.IsNullOrWhiteSpace(fareaname))
            {
                return CreateResult.ParameterIsNull;
            }
            if (!checkLogin())
            {
                return CreateResult.LoginFailed;
            }
            var p = new K3Cloud_Province()
            {
                Model =
                {
                    FNumber = fareanumber,
                    FName = fareaname
                }
            };
            var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Area", JsonConvert.SerializeObject(p) });
            if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                //提交审批
                var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_Area", JsonConvert.SerializeObject(commitpara) });
                if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                    var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_Area", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.AuditFailed;
                    }
                }
                else
                {
                    return CreateResult.CommitFailed;
                }
            }
            return CreateResult.UnknownError;
        }
        //创建二级渠道
        public CheckResult CheckSecondChannelExists(string fsecondchannelnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fsecondchannelnumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                if (!checkLogin())
                {
                    return CheckResult.LoginFailed;
                }
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FID",
                    FormId = "PLYE_SecondChannel",
                    FilterString = "FNumber = '" + fsecondchannelnumber + "'"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                if (jrr.Count == 0)
                {
                    return CheckResult.ItemNotExists;
                }
                else if (jrr.Count > 0)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateSecondChannel(string fsecondchannelnumber, string fsecondchannelname)
        {
            if (string.IsNullOrWhiteSpace(fsecondchannelnumber))
            {
                return CreateResult.ParameterIsNull;
            }
            if (string.IsNullOrWhiteSpace(fsecondchannelname))
            {
                return CreateResult.ParameterIsNull;
            }
            if (!checkLogin())
            {
                return CreateResult.LoginFailed;
            }
            var p = new K3Cloud_Province()
            {
                Model =
                {
                    FNumber = fsecondchannelnumber,
                    FName = fsecondchannelname
                }
            };
            var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_SecondChannel", JsonConvert.SerializeObject(p) });
            if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                var createret = JObject.Parse(ret).ToObject<K3Cloud_Item_Success>();
                //提交审批
                var commitpara = new K3Cloud_Item_Commit() { Ids = createret.Result.Id.ToString() };
                var commitret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "PLYE_SecondChannel", JsonConvert.SerializeObject(commitpara) });
                if (JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(commitret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    var com = new K3Cloud_Item_Audit() { Ids = createret.Result.Id.ToString() };
                    var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "PLYE_SecondChannel", JsonConvert.SerializeObject(commitpara) });
                    if (JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']") != null && JObject.Parse(auditret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        return CreateResult.AllSuccess;
                    }
                    else
                    {
                        return CreateResult.AuditFailed;
                    }
                }
                else
                {
                    return CreateResult.CommitFailed;
                }
            }
            return CreateResult.UnknownError;
        }
        //创建出库单据类型
        //创建入库单据类型
        public CheckResult CheckOutBillTypeExist(string billtypenumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(billtypenumber))
                {
                    return CheckResult.ParameterIsNull;
                }
                var para = new K3Cloud_BillType_View()
                {
                    Number = billtypenumber
                };
                var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "BOS_BillType", JsonConvert.SerializeObject(para) });
                var kbr = JObject.Parse(ret).ToObject<K3Cloud_BillType_Result>();
                if (!kbr.Result.ResponseStatus.IsSuccess)
                {
                    if (kbr.Result.ResponseStatus.Errors.Count > 0 && kbr.Result.ResponseStatus.Errors[0].Message == "传递的编码值不存在")
                    {
                        return CheckResult.ItemNotExists;
                    }
                    else
                    {
                        return CheckResult.UnknownError;
                    }

                }
                else if (kbr.Result.ResponseStatus.IsSuccess)
                {
                    return CheckResult.ItemExists;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CheckResult.UnknownError;
        }
        public CreateResult CreateOutBillType(string billtypenumber, string billtypename)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(billtypenumber))
                {
                    return CreateResult.ParameterIsNull;
                }
                if (string.IsNullOrWhiteSpace(billtypename))
                {
                    billtypename = billtypenumber;
                    //return CreateResult.ParameterIsNull;
                }
                var para = new K3Cloud_BillType_Create()
                {
                    Model =
                    {
                        FBillFormID = { FNumber = "PLYE_SaleOrder" },
                        FNumber = billtypenumber,
                        FName = billtypename,
                    }
                };
                var paraTxt = JsonConvert.SerializeObject(para);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "BOS_BillType", paraTxt });
                var saveret = JObject.Parse(ret).ToObject<K3Cloud_BillType_Save>();
                if (saveret.Result.ResponseStatus.IsSuccess)
                {
                    var commitpara = new K3Cloud_BillType_Commit()
                    {
                        Numbers = { billtypenumber },
                    };
                    var commitret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(commitpara) })).ToObject<K3Cloud_BillType_Save>();
                    //var commitret2 = JObject.Parse(commitret).ToObject<k3c>
                    if (commitret.Result.ResponseStatus.IsSuccess)
                    {
                        var auditpara = new K3Cloud_BillType_Audit()
                        {
                            Numbers = { billtypenumber }
                        };
                        //var auditret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) });
                        var auditret = JObject.Parse(ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "BOS_BillType", JsonConvert.SerializeObject(auditpara) })).ToObject<K3Cloud_BillType_Save>();
                        if (auditret.Result.ResponseStatus.IsSuccess)
                        {
                            return CreateResult.AllSuccess;
                        }
                        else
                        {
                            return CreateResult.AuditFailed;
                        }
                    }
                    else
                    {
                        return CreateResult.CommitFailed;
                    }
                }
                else
                {
                    return CreateResult.CreateFailed;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return CreateResult.UnknownError;
        }
        //同步采购订单
        public SyncResult SyncPurchaseOrderBill(CRM_OutStockDetail billdetail, Robam_CRM robam,string forgid)
        {
            try
            {
                var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach(var code in qrcodes.barcodeList)
                    {
                        if(code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode } );
                        }
                    }
                    
                    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity() {
                        FMaterialID = {FNUMBER = item.materialCode },
                        FUnitID = {FNUMBER = item.unitCode },
                        FQty_Fact = Convert.ToDecimal( item.actualQuantity),
                        FQty = Convert.ToDecimal(item.quantity),
                        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        FSourceBillNo = item.sourceOrderNo,
                        FInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        FRobam_PurchaseOrderCodeEntity = sublist,
                        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },
                        
                    });
                }
                var bill = new K3Cloud_PurchaseOrder()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNUMBER = ("PLYE_Purchaseorder").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeCode },
                        FDate = billdetail.crminvexportheaders.orderDate,
                        FBookDate = billdetail.crminvexportheaders.customerDate,
                        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        FContact = billdetail.crminvexportheaders.contactName,
                        FContactPhone = billdetail.crminvexportheaders.contactTel,
                        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FCustomer = billdetail.crminvexportheaders.contactName,
                        FRobam_PurchaseOrderEntity = list,
                        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        FOrgID = { FNumber = forgid },
                        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_Purchaseorder", JsonConvert.SerializeObject(bill) });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch(Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //同步入库单
        public SyncResult SyncInstockBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                var list = new List<K3Cloud_InstockBill_Model_FInStockEntry>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach (var code in qrcodes.barcodeList)
                    {
                        if (code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity() { FQrCodeText = code.barcode });
                        }
                    }

                    list.Add(new K3Cloud_InstockBill_Model_FInStockEntry()
                    {
                        FMaterialId = { FNumber = item.materialCode ?? "" },
                        FUnitID = { FNumber = item.unitCode ?? "" },
                        FRealQty = Convert.ToDecimal(item.actualQuantity ?? 0m),
                        FPriceBaseQty = Convert.ToDecimal(item.actualQuantity ?? 0m),
                        FRemainInStockQty = Convert.ToDecimal(item.actualQuantity ?? 0m),
                        FRemainInStockBaseQty = Convert.ToDecimal(item.actualQuantity ?? 0m),
                        FAPNotJoinQty = Convert.ToDecimal(item.actualQuantity ?? 0m),
                        FPriceUnitID = { FNumber = item.unitCode??"" },
                        FRemainInStockUnitId = { FNumber = item.unitCode??"" },
                        FStockId = { FNumber = billdetail.crminvexportheaders.inventoryCode??"" },
                        FStockStatusId = { FNumber = item.deliveryGoodsStatus??"" },
                        FRobam_SubEntity = sublist,
                        FItemNo = (item.itemNo??0)
                        //FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        //FSourceBillNo = item.sourceOrderNo,
                        //FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        //FRobam_PurchaseOrderCodeEntity = sublist,
                        //FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                var bill = new K3Cloud_InstockBill()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FRobamBillNo = billdetail.crminvexportheaders.orderNo,
                        //FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNumber = ("STK_InStock").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeName },
                        FRobamDate = billdetail.crminvexportheaders.orderDate,
                        FSupplierId = { FNumber = (billdetail.crminvexportheaders.customerCode?? "ORG-sjz") },
                        FInStockEntry = list,
                        FRobamCompany = { FNumber = forgid },
                        FJsonText = JsonConvert.SerializeObject(billdetail,Formatting.Indented)
                        //FBookDate = billdetail.crminvexportheaders.customerDate,
                        //FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        //FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        //FContact = billdetail.crminvexportheaders.contactName,
                        //FContactPhone = billdetail.crminvexportheaders.contactTel,
                        //FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        //FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        //FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        //FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        //FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        //FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FCustomer = billdetail.crminvexportheaders.contactName,
                        //FRobam_PurchaseOrderEntity = list,
                        //FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        //FOrgID = { FNumber = forgid },
                        //FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                string paras = JsonConvert.SerializeObject(bill);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "STK_InStock", paras });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }

        //同步销售订单
        public SyncResult SyncSaleOrderBill(K3Cloud_SaleOrder billdetai/*,out K3Cloud_FEntity_Link link*/)
        {
            //link = null;
            try
            {
                
                if (DateTime.Parse(billdetai.Model.FDate) < DateTime.Parse("2022-11-1 00:00:00"))
                {
                    billdetai.Model.FIsHistoryBill = true;
                }
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_SaleOrder", JsonConvert.SerializeObject(billdetai) });
                var ins = JObject.Parse(ret).ToObject<K3Cloud_BillType_Save>();
                if (ins.Result.ResponseStatus.IsSuccess)
                {
                    //link = new K3Cloud_FEntity_Link() {
                    //    FRuleId = "95244c21-7a23-430d-ba95-106387c37f49",
                    //    FSTableName = "Robam_SaleOrderEntity",
                    //    FSBillId = Convert.ToInt32( ins.Result.Id),

                    //}
                    return SyncResult.AllSuccess;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //同步出库单
        public SyncResult SyncOutStockBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid )
        {
            try
            {
                //string orgbill = billdetail.
                //if (true)
                //{
                //    //先下推出库单，
                //    //下推后根据源单修改
                //}
                //else
                //{
                    //如果没有源单
                    var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                    foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                    {
                        //获取原单号
                        string orgbillno = item.sourceOrderNo;
                    //判断金蝶中是否存在该原单号
                    if (!GetSalerOrderBillNo(orgbillno))
                    {
                        //源单存在下推单据

                    }
                    else
                    {
                        //源单不存在，不创建源单，直接取得源单的门店，导购员，仓库，产品状态信息

                        //robam.get
                    }
                    

                        var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                        var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                        foreach (var code in qrcodes.barcodeList)
                        {
                            if (code.materialCode == item.materialCode)
                            {
                                sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                            }
                        }

                        list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                        {
                            FMaterialID = { FNUMBER = item.materialCode },
                            FUnitID = { FNUMBER = item.unitCode },
                            FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                            FQty = Convert.ToDecimal(item.quantity),
                            FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                            FSourceBillNo = item.sourceOrderNo,
                            FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                            FRobam_PurchaseOrderCodeEntity = sublist,
                            FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                        });
                    }
                    var bill = new K3Cloud_PurchaseOrder()
                    {
                        Model =
                        {
                            FBillNo = billdetail.crminvexportheaders.orderNo,
                            FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                            FBillTypeID = {FNUMBER = ("PLYE_RealOutstockBill").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeCode },
                            FDate = billdetail.crminvexportheaders.orderDate,
                            FBookDate = billdetail.crminvexportheaders.customerDate,
                            FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                            FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                            FContact = billdetail.crminvexportheaders.contactName,
                            FContactPhone = billdetail.crminvexportheaders.contactTel,
                            FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                            FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                            FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                            FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                            FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                            FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                            FCustomer = billdetail.crminvexportheaders.contactName,
                            FRobam_PurchaseOrderEntity = list,
                            FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                            FOrgID = { FNumber = forgid },
                            FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                            FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                        }
                    };
                    var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_RealOutstockBill", JsonConvert.SerializeObject(bill) });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
                //}




                //直接创建
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //配件入库单
        public SyncResult SyncPartsInstockBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                //var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                //foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                //{
                //    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                //    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                //    foreach (var code in qrcodes.barcodeList)
                //    {
                //        if (code.materialCode == item.materialCode)
                //        {
                //            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                //        }
                //    }

                //    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                //    {
                //        FMaterialID = { FNUMBER = item.materialCode },
                //        FUnitID = { FNUMBER = item.unitCode },
                //        FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                //        FQty = Convert.ToDecimal(item.quantity),
                //        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                //        FSourceBillNo = item.sourceOrderNo,
                //        FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                //        FRobam_PurchaseOrderCodeEntity = sublist,
                //        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                //    });
                //}
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                //var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_PartsInstock", JsonConvert.SerializeObject(bill) });
                SyncPartsInstockOrderBill(billdetail, robam, forgid);
                var list = new List<K3Cloud_InstockBill_Model_FInStockEntry>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach (var code in qrcodes.barcodeList)
                    {
                        if (code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity() { FQrCodeText = code.barcode });
                        }
                    }

                    list.Add(new K3Cloud_InstockBill_Model_FInStockEntry()
                    {
                        FMaterialId = { FNumber = item.materialCode },
                        FUnitID = { FNumber = item.unitCode },
                        FRealQty = Convert.ToDecimal(item.actualQuantity),
                        FPriceBaseQty = Convert.ToDecimal(item.actualQuantity),
                        FRemainInStockQty = Convert.ToDecimal(item.actualQuantity),
                        FRemainInStockBaseQty = Convert.ToDecimal(item.actualQuantity),
                        FAPNotJoinQty = Convert.ToDecimal(item.actualQuantity),
                        FPriceUnitID = { FNumber = item.unitCode },
                        FRemainInStockUnitId = { FNumber = item.unitCode },
                        FStockId = { FNumber = billdetail.crminvexportheaders.inventoryCode },
                        FStockStatusId = { FNumber = item.deliveryGoodsStatus },
                        FRobam_SubEntity = sublist,
                        //FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        //FSourceBillNo = item.sourceOrderNo,
                        //FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        //FRobam_PurchaseOrderCodeEntity = sublist,
                        //FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                var bill = new K3Cloud_InstockBill()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FRobamBillNo = billdetail.crminvexportheaders.orderNo,
                        //FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNumber = ("STK_InStock").ToUpper() + "_" +  billdetail.crminvexportheaders.orderTypeName },
                        FRobamDate = billdetail.crminvexportheaders.orderDate,
                        FSupplierId = { FNumber = billdetail.crminvexportheaders.customerCode},
                        FInStockEntry = list,
                        FRobamCompany = {FNumber = forgid }
                        //FBookDate = billdetail.crminvexportheaders.customerDate,
                        //FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        //FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        //FContact = billdetail.crminvexportheaders.contactName,
                        //FContactPhone = billdetail.crminvexportheaders.contactTel,
                        //FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        //FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        //FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        //FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        //FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        //FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FCustomer = billdetail.crminvexportheaders.contactName,
                        //FRobam_PurchaseOrderEntity = list,
                        //FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        //FOrgID = { FNumber = forgid },
                        //FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                string paras = JsonConvert.SerializeObject(bill);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "STK_InStock", paras });

                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //配件红字单据
        /// <summary>
        /// 老板配件退回同步到系统的销售退货单
        /// </summary>
        /// <param name="billdetail"></param>
        /// <param name="robam"></param>
        /// <param name="forgid"></param>
        /// <returns></returns>
        public SyncResult SyncPartsRerurnBack(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                //var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                //foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                //{
                //    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                //    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                //    foreach (var code in qrcodes.barcodeList)
                //    {
                //        if (code.materialCode == item.materialCode)
                //        {
                //            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                //        }
                //    }

                //    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                //    {
                //        FMaterialID = { FNUMBER = item.materialCode },
                //        FUnitID = { FNUMBER = item.unitCode },
                //        FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                //        FQty = Convert.ToDecimal(item.quantity),
                //        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                //        FSourceBillNo = item.sourceOrderNo,
                //        FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                //        FRobam_PurchaseOrderCodeEntity = sublist,
                //        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                //    });
                //}
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                //var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_PartsInstock", JsonConvert.SerializeObject(bill) });
                SyncPartsInstockOrderBill(billdetail, robam, forgid);
                var list = new List<K3Cloud_InstockBill_Model_FInStockEntry>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach (var code in qrcodes.barcodeList)
                    {
                        if (code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity() { FQrCodeText = code.barcode });
                        }
                    }

                    list.Add(new K3Cloud_InstockBill_Model_FInStockEntry()
                    {
                        FMaterialId = { FNumber = item.materialCode },
                        FUnitID = { FNumber = item.unitCode },
                        FRealQty = 0 - Convert.ToDecimal(item.actualQuantity),
                        FPriceBaseQty = Convert.ToDecimal(item.actualQuantity),
                        FRemainInStockQty = Convert.ToDecimal(item.actualQuantity),
                        FRemainInStockBaseQty = Convert.ToDecimal(item.actualQuantity),
                        FAPNotJoinQty = Convert.ToDecimal(item.actualQuantity),
                        FPriceUnitID = { FNumber = item.unitCode },
                        FRemainInStockUnitId = { FNumber = item.unitCode },
                        FStockId = { FNumber = billdetail.crminvexportheaders.inventoryCode },
                        FStockStatusId = { FNumber = item.deliveryGoodsStatus },
                        FRobam_SubEntity = sublist,
                        //FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        //FSourceBillNo = item.sourceOrderNo,
                        //FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        //FRobam_PurchaseOrderCodeEntity = sublist,
                        //FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                var bill = new K3Cloud_InstockBill()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FRobamBillNo = billdetail.crminvexportheaders.orderNo,
                        //FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNumber = ("STK_InStock").ToUpper() + "_" +  billdetail.crminvexportheaders.orderTypeName },
                        FRobamDate = billdetail.crminvexportheaders.orderDate,
                        FSupplierId = { FNumber = billdetail.crminvexportheaders.customerCode},
                        FInStockEntry = list,
                        FRobamCompany = {FNumber = forgid }
                        //FBookDate = billdetail.crminvexportheaders.customerDate,
                        //FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        //FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        //FContact = billdetail.crminvexportheaders.contactName,
                        //FContactPhone = billdetail.crminvexportheaders.contactTel,
                        //FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        //FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        //FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        //FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        //FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        //FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FCustomer = billdetail.crminvexportheaders.contactName,
                        //FRobam_PurchaseOrderEntity = list,
                        //FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        //FOrgID = { FNumber = forgid },
                        //FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                string paras = JsonConvert.SerializeObject(bill);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "STK_InStock", paras });

                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //配件入库订单
        public SyncResult SyncPartsInstockOrderBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach (var code in qrcodes.barcodeList)
                    {
                        if (code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                        }
                    }

                    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                    {
                        FMaterialID = { FNUMBER = item.materialCode },
                        FUnitID = { FNUMBER = item.unitCode },
                        FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                        FQty = Convert.ToDecimal(item.quantity),
                        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        FSourceBillNo = item.sourceOrderNo,
                        FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        FRobam_PurchaseOrderCodeEntity = sublist,
                        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                var bill = new K3Cloud_PurchaseOrder()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNUMBER = ("STK_InStock").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeCode },
                        FDate = billdetail.crminvexportheaders.orderDate,
                        FBookDate = billdetail.crminvexportheaders.customerDate,
                        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        FContact = billdetail.crminvexportheaders.contactName,
                        FContactPhone = billdetail.crminvexportheaders.contactTel,
                        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FCustomer = billdetail.crminvexportheaders.contactName,
                        FRobam_PurchaseOrderEntity = list,
                        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        FOrgID = { FNumber = forgid },
                        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "STK_InStock", JsonConvert.SerializeObject(bill) });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //配件入库订单
        public SyncResult SyncPartsInstockOrderBillForSystemBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                K3Cloud_InstockBill kib = new K3Cloud_InstockBill();

                var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    foreach (var code in qrcodes.barcodeList)
                    {
                        if (code.materialCode == item.materialCode)
                        {
                            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                        }
                    }

                    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                    {
                        FMaterialID = { FNUMBER = item.materialCode },
                        FUnitID = { FNUMBER = item.unitCode },
                        FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                        FQty = Convert.ToDecimal(item.quantity),
                        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        FSourceBillNo = item.sourceOrderNo,
                        FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        FRobam_PurchaseOrderCodeEntity = sublist,
                        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                var bill = new K3Cloud_PurchaseOrder()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNUMBER = ("PLYE_PartsInstock").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeCode },
                        FDate = billdetail.crminvexportheaders.orderDate,
                        FBookDate = billdetail.crminvexportheaders.customerDate,
                        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        FContact = billdetail.crminvexportheaders.contactName,
                        FContactPhone = billdetail.crminvexportheaders.contactTel,
                        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FCustomer = billdetail.crminvexportheaders.contactName,
                        FRobam_PurchaseOrderEntity = list,
                        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        FOrgID = { FNumber = forgid },
                        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_PartsInstock", JsonConvert.SerializeObject(bill) });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //配件出库订单
        public SyncResult SyncPartsOutstockBill(CRM_OutStockDetail billdetail, Robam_CRM robam, string forgid)
        {
            try
            {
                //创建入库单
                //K3Cloud_InstockBill ki = new K3Cloud_InstockBill();
                //K3Cloud_InstockBill_Model_FInStockEntry kimfe = new K3Cloud_InstockBill_Model_FInStockEntry();


                //var list = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
                //foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                //{
                //    var sublist = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
                //    var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                //    foreach (var code in qrcodes.barcodeList)
                //    {
                //        if (code.materialCode == item.materialCode)
                //        {
                //            sublist.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity() { FQrCodeText = code.barcode });
                //        }
                //    }

                //    list.Add(new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity()
                //    {
                //        FMaterialID = { FNUMBER = item.materialCode },
                //        FUnitID = { FNUMBER = item.unitCode },
                //        FQty_Fact = Convert.ToDecimal(item.actualQuantity),
                //        FQty = Convert.ToDecimal(item.quantity),
                //        FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                //        FSourceBillNo = item.sourceOrderNo,
                //        FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                //        FRobam_PurchaseOrderCodeEntity = sublist,
                //        FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                //    });
                //}
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                //var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "PLYE_PartsOutstock", JsonConvert.SerializeObject(bill) });

                //K3Cloud_OutStockBill kos = new K3Cloud_OutStockBill();


                var list = new List<FEntityItem>();
                foreach (var item in billdetail.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    //var sublist = new List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity>();
                    //var qrcodes = robam.GetBillQrcode(billdetail.crminvexportheaders.orderNo);
                    //foreach (var code in qrcodes.barcodeList)
                    //{
                    //    if (code.materialCode == item.materialCode)
                    //    {
                    //        sublist.Add(new K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity() { FQrCodeText = code.barcode });
                    //    }
                    //}

                    list.Add(new FEntityItem()
                    {
                        FMaterialID = { FNumber = item.materialCode },
                        FUnitID = { FNumber = item.unitCode },
                        FRealQty = Convert.ToDecimal(item.actualQuantity),
                        FPRICEBASEQTY = Convert.ToDecimal(item.actualQuantity),
                        //FRemainInStockQty = Convert.ToDecimal(item.actualQuantity),
                        //FRemainInStockBaseQty = Convert.ToDecimal(item.actualQuantity),
                        //FAPNotJoinQty = Convert.ToDecimal(item.actualQuantity),
                        //FPriceUnitID = { FNumber = item.unitCode },
                        //FRemainInStockUnitId = { FNumber = item.unitCode },
                        FStockID = { FNumber = billdetail.crminvexportheaders.inventoryCode },
                        FStockStatusID = { FNumber = item.deliveryGoodsStatus },
                        FRobamPrice = Convert.ToDecimal(item.netPrice),
                        FRobamAmount = Convert.ToDecimal(item.approveAmount)
                        //FRobam_SubEntity = sublist,
                        //FQty_Recive = Convert.ToDecimal(item.customerRealQty),
                        //FSourceBillNo = item.sourceOrderNo,
                        //FInventory = { FNUMBER = billdetail.crminvexportheaders.inventoryCode },
                        //FRobam_PurchaseOrderCodeEntity = sublist,
                        //FStockStatusId = { FNUMBER = item.deliveryGoodsStatus },

                    });
                }
                //var bill = new K3Cloud_PurchaseOrder()
                //{
                //    Model =
                //    {
                //        FBillNo = billdetail.crminvexportheaders.orderNo,
                //        FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                //        FBillTypeID = {FNUMBER = billdetail.crminvexportheaders.orderTypeCode },
                //        FDate = billdetail.crminvexportheaders.orderDate,
                //        FBookDate = billdetail.crminvexportheaders.customerDate,
                //        FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                //        FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                //        FContact = billdetail.crminvexportheaders.contactName,
                //        FContactPhone = billdetail.crminvexportheaders.contactTel,
                //        FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                //        FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                //        FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                //        FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                //        FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                //        FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FCustomer = billdetail.crminvexportheaders.contactName,
                //        FRobam_PurchaseOrderEntity = list,
                //        FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                //        FOrgID = { FNumber = forgid },
                //        FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                //        FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                //    }
                //};
                var bill = new K3Cloud_OutStockBill()
                {
                    Model =
                    {
                        FBillNo = billdetail.crminvexportheaders.orderNo,
                        //FRobamBillNo = billdetail.crminvexportheaders.orderNo,
                        //FCompany = {FNUMBER = billdetail.crminvexportheaders.orgId.ToString()},
                        FBillTypeID = {FNumber = ("SAL_OUTSTOCK").ToUpper() + "_" + billdetail.crminvexportheaders.orderTypeName },
                        FDate = billdetail.crminvexportheaders.orderDate,
                        FCustomerID = { FNumber = "001"},
                        FEntity = list,
                        FSaleDeptID = { FNumber = "BM000001" },
                        FReceiverID = {FNumber = "001" },
                        FHeadLocationId = { FNumber = ""},
                        FRobamBillNo = billdetail.crminvexportheaders.orderNo,
                        FRobamDate =  billdetail.crminvexportheaders.creationDate,
                        FRobamCompany = { FNumber = forgid},
                        //FBookDate = billdetail.crminvexportheaders.customerDate,
                        //FDisBillNo = billdetail.crminvexportheaders.sourceOrderNo,
                        //FOrgBillTypeID = billdetail.crminvexportheaders.orderTypeName,
                        //FContact = billdetail.crminvexportheaders.contactName,
                        //FContactPhone = billdetail.crminvexportheaders.contactTel,
                        //FContactAddress = billdetail.crminvexportheaders.inceptAddress,
                        //FSendUnitType = billdetail.crminvexportheaders.inventoryCode != null ? "PLYE_Company" : "",
                        //FSendUnit = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode??"" },
                        //FSendInventory = {FNUMBER = billdetail.crminvexportheaders.inventoryCode ?? "" },
                        //FReciveUnitType = (billdetail.crminvexportheaders.customerCode != null ? "PLYE_Company" : ""),
                        //FReciveUnit = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FCustomer = billdetail.crminvexportheaders.contactName,
                        //FRobam_PurchaseOrderEntity = list,
                        //FOrgBillNo = billdetail.crminvexportheaders.orderNo,
                        //FOrgID = { FNumber = forgid },
                        //FReciveCompany = {FNumber = billdetail.crminvexportheaders.customerCode ?? "" },
                        //FSendCompany = {FNumber = billdetail.crminvexportheaders.deliveryCustomerCode ?? "" },
                    }
                };
                string paras = JsonConvert.SerializeObject(bill);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "SAL_OUTSTOCK", paras });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
        //获取已同步入库单据原始单号
        public List<string> GetSyncedBillNoList(string startDate ,string endDate,string formid = "PLYE_Purchaseorder")
        {
            try
            {
                if (!checkLogin())
                {
                    return null;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = formid,
                    FilterString = " FRobamDate between '" + startDate + "' and '" + endDate + "' ",
                    FieldKeys = "FOrgBillNo,FDocumentStatus"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                List<string> ret = new List<string>();
                var list = JArray.Parse(queryResult);
                if(list.Count > 0)
                {
                    foreach (var i in list)
                    {
                        ret.Add(i[0].ToString());
                    }
                    return ret;
                }
                else
                {
                    return ret;
                }
            }
            catch(Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        public List<string[]> GetSyncedBillNoList2(string billno, string formid = "PLYE_Purchaseorder")
        {
            try
            {
                if (!checkLogin())
                {
                    return null;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = formid,
                    FilterString = " FBillNo like '" + billno + "' ",
                    FieldKeys = "FOrgBillNo,FDocumentStatus"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                List<string[]> ret = new List<string[]>();
                string[] bs = new string[2];
                var list = JArray.Parse(queryResult);
                if (list.Count > 0)
                {
                    foreach (var i in list)
                    {
                        bs[0] = i[0].ToString();
                        bs[1] = i[1].ToString();
                    }
                    ret.Add(bs);
                    return ret;
                }
                else
                {
                    return ret;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        public List<string> GetSyncedBillNoList(string billno, string formid = "PLYE_Purchaseorder")
        {
            try
            {
                if (!checkLogin())
                {
                    return null;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = formid,
                    FilterString = " FBillNo like '" + billno + "' ",
                    FieldKeys = "FOrgBillNo,FDocumentStatus"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                List<string> ret = new List<string>();
                string[] bs = new string[2]; 
                var list = JArray.Parse(queryResult);
                if (list.Count > 0)
                {
                    foreach (var i in list)
                    {
                        ret.Add(i[0].ToString());
                    }

                    return ret;
                }
                else
                {
                    return ret;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        //获取已同步出库单原始单号
        public List<string> GetSyncedOutStockBillNo(string startDate, string endDate)
        {
            try
            {
                if (!checkLogin())
                {
                    return null;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = "PLYE_SaleOrder",
                    FilterString = " FDate between '" + startDate + "' and '" + endDate + "' ",
                    FieldKeys = "FBillNo"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                List<string> ret = new List<string>();
                var list = JArray.Parse(queryResult);
                if (list.Count > 0)
                {
                    foreach (var i in list)
                    {
                        ret.Add(i[0].ToString());
                    }
                    return ret;
                }
                else
                {
                    return ret;
                }
            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        //获取不需要同步的单据类型
        public List<string> UnsyncBillType(string billtype = "PLYE_Purchaseorder")
        {
            List<string> list = new List<string>();
            try
            {
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FNumber",
                    FormId = "BOS_BillType",
                    FilterString = "FBillFormID=\'" + billtype + "\' AND FRobam_ForbidImport = 1"
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });

                var jrr = JArray.Parse(queryResult);
                foreach(JArray i in jrr)
                {
                    list.Add(i[0].ToString().Trim());
                }
            }
            catch (Exception exp)
            {
                m_ErrorMessage = exp.Message;
                Logger.DebugLog2(exp.Message);
            }
            return list;
        }
        public string GetAStock()
        {
            try
            {
                var filter = new K3Cloud_Current_Query()
                {
                    FieldKeys = "FNumber",
                    FormId = "BD_STOCK",
                    FilterString = ""
                };
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                var arr = JArray.Parse(queryResult);
                if (arr.Count > 0)
                {
                    return arr[0][0].ToString();
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return "";
        }
        public bool GetSalerOrderBillNo(string billno)
        {
            try
            {
                if (!checkLogin())
                {
                    return false;
                }
                var filter = new List<K3Cloud_Current_Query>(){ new K3Cloud_Current_Query()
                {
                    FormId = "PLYE_SaleOrder",
                    FilterString = " FOrgBillNo = '" + billno + "'",
                    FieldKeys = "FOrgBillNo"
                } };
                var para = JsonConvert.SerializeObject(filter);
                var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, para);
                List<string> ret = new List<string>();
                var list = JArray.Parse(queryResult);
                if (list.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return false;
        }
        //销售订单下推
        public bool PushXSDDToXSCK(string saleorderbillno,out string failreason,out string fbillno)
        {
            failreason = "";
            fbillno = "";
            K3Cloud_Push kp = new K3Cloud_Push();
            kp.Numbers.Add(saleorderbillno);
            kp.RuleId = "95244c21-7a23-430d-ba95-106387c37f49";
            //kp.IsDraftWhenSaveFail = true;
            try
            {
                var s = sendRepuest(K3Cloud_AddressType.Push, JsonConvert.SerializeObject(new object[] { "PLYE_SaleOrder", kp }));
                var jobj = JObject.Parse(s);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    fbillno = jobj.SelectToken("Result.['ResponseStatus'].['SuccessEntitys'].[0].['Number']").Value<string>();
                    return true;
                }
                else
                {
                    failreason = jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess'].['Errors'].[0].['Message']").Value<string>();
                    return false;
                }
                var jarr = JArray.Parse(s);

            }
            catch (Exception exp)
            {
                ZJF_LOGGER.log(exp.Message);
                return false;
            }
  
        }
        //更新销售出库单
        public bool SaveXSCK(K3Cloud_OutStockBill bill)
        {
            try
            {
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "SAL_OUTSTOCK", JsonConvert.SerializeObject(bill) });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    //fbillno = jobj.SelectToken("Result.['ResponseStatus'].['SuccessEntitys'].[0].['Number']").Value<string>();
                    return true;
                }
                else
                {
                    //failreason = jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess'].['Errors'].[0].['Message']").Value<string>();
                    return false;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return false;
        }
        public JObject GetXSCK(string fbillno)
        {
            try
            {
                JObject jobj = new JObject();
                jobj["Number"] = fbillno;
                jobj["Id"] = "";
                jobj["IsSortBySeq"] = false;
                var s = ZJF.ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.View, JsonConvert.SerializeObject(new object[] { "SAL_OUTSTOCK", jobj}));
                return JObject.Parse(s);
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return null;
        }
        public bool SubmitBill(string formid,string fbillno)
        {
            try
            {
                JObject commitObj = new JObject();
                commitObj["Numbers"] = new JArray() {  fbillno  };
                commitObj["Ids"] = "";// retResult.SelectToken("Result.['ResponseStatus'].['SuccessEntitys'][0].['Id']").Value<string>();
                commitObj["PkEntryIds"] = new JArray();
                commitObj["NetworkCtrl"] = "";
                commitObj["IgnoreInterationFlag"] = "";
                var commitResult = JObject.Parse(sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { formid,"Commit", commitObj }));
                if (commitResult.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
                return false;
            }
        }
        public bool AuditBill(string formid,string fbillno)
        {
            try
            {
                JObject commitObj = new JObject();
                commitObj["Numbers"] = new JArray() { fbillno };
                commitObj["Ids"] = "";// retResult.SelectToken("Result.['ResponseStatus'].['SuccessEntitys'][0].['Id']").Value<string>();
                commitObj["PkEntryIds"] = new JArray();
                commitObj["NetworkCtrl"] = "";
                commitObj["IgnoreInterationFlag"] = "";
                var commitResult = JObject.Parse(sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { formid, "Audit", commitObj }));
                if (commitResult.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exp)
            {
                Logger.log(exp.Message);
                return false;
            }
        }
        //获取源单信息
        public List<K3Cloud_OutStockSourceInfo> GetOutStockSourceBillInfo(string BillNo)
        {
            try
            {
                //if (ZJF_WEBAPI.init(host, acctid, username, pwd))
                {
                    var ret = new List<K3Cloud_OutStockSourceInfo>();
                    acctList.Clear();
                    var filter = new K3Cloud_Current_Query()
                    {
                        FormId = "PLYE_SaleOrder",
                        FilterString = "FBillNo = '" + BillNo + "'",
                        FieldKeys = "FID,FRobam_SaleOrderEntity_FEntryID,FItemNo,FPrice,FAmount"
                    };
                    var queryResult = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { JsonConvert.SerializeObject(filter) });
                    var jrr = JArray.Parse(queryResult);
                    if (jrr.Count == 0)
                    {
                        return null;
                    }
                    else if (jrr.Count > 0)
                    {
                        foreach (var item in jrr)
                        {
                            var ins = new K3Cloud_OutStockSourceInfo();
                            ins.FID = Convert.ToInt32(item[0].ToString());
                            ins.FEntryID = Convert.ToInt32(item[1].ToString());
                            ins.FItemNo = Convert.ToInt32(item[2].ToString());
                            ins.FPrice = Convert.ToDecimal(item[3].ToString());
                            ins.FAmount = Convert.ToDecimal(item[4].ToString());
                            ret.Add(ins);
                        }
                        return ret;
                    }
                }

            }
            catch (Exception exp)
            {
                Logger.DebugLog2(exp.Message);
            }
            return null;
        }
        public K3Cloud_OutStockBill TranslateCRMOutStockToKingdeeOutStockBill(CRM_OutStockDetail crmoutstock,string forgid)
        {
            try
            {
                var list = new List<FEntityItem>();
                var source = GetOutStockSourceBillInfo(crmoutstock.crminvexportheaders.fxOrderNo);
                if(source == null || source.Count <= 0)
                {
                    return null;
                }
                foreach (var item in crmoutstock.crminvexportheaders.crmInvExOrderLinesVs)
                {
                    //查询源单内码
                    var itemno = item.itemNo;
                    var s = source.Where(i => i.FItemNo == itemno).FirstOrDefault();
                    if(s == null)
                    {
                        return null;
                    }
                    list.Add(new FEntityItem(){
                        FMaterialID = { FNumber = item.materialCode },
                        FUnitID = { FNumber = item.unitCode },
                        FRealQty = Convert.ToDecimal(item.actualQuantity),
                        FPRICEBASEQTY = Convert.ToDecimal(item.actualQuantity),
                        //FRemainInStockQty = Convert.ToDecimal(item.actualQuantity),
                        //FRemainInStockBaseQty = Convert.ToDecimal(item.actualQuantity),
                        FARNOTJOINQTY = Convert.ToDecimal(item.actualQuantity),
                        FSalUnitID = { FNumber = item.unitCode },
                        //FRemainInStockUnitId = { FNumber = item.unitCode },
                        FStockID = { FNumber = crmoutstock.crminvexportheaders.inventoryCode },
                        FStockStatusID = { FNumber = item.deliveryGoodsStatus },
                        //FRobam_SubEntity = sublist,
                        FItemNo = (item.itemNo ?? 0),
                        FEntity_Link = new List<FEntity_Link>() { 
                            new FEntity_Link(){ 
                                //关联关系设置地址 https://vip.kingdee.com/article/178896?channel_level=kdclub&productLineId=1
                                FEntity_Link_FRuleId = "95244c21-7a23-430d-ba95-106387c37f49",
                                FEntity_Link_FSTableName = "Robam_SaleOrderEntity",
                                FEntity_Link_FSBillId = s.FID,
                                FEntity_Link_FSId = s.FEntryID,
                                FEntity_Link_FAuxUnitQty = Convert.ToDecimal( item.actualQuantity),
                                FEntity_Link_FAuxUnitQtyOld = Convert.ToDecimal( item.actualQuantity),
                                FEntity_Link_FBaseUnitQty = Convert.ToDecimal( item.actualQuantity),
                                FEntity_Link_FBaseUnitQtyOld = Convert.ToDecimal( item.actualQuantity),
                            }
                        },
                        FRobam_Price = s.FPrice,
                        FRobam_Amount = s.FAmount,
                        FRobamPrice = s.FPrice,
                        FRobamAmount = s.FAmount,
                    });
                }
                K3Cloud_OutStockBill ins = new K3Cloud_OutStockBill
                {
                    Model =
                    {
                        FBillNo = crmoutstock.crminvexportheaders.orderNo,
                        FRobamBillNo = crmoutstock.crminvexportheaders.orderNo,
                        FBillTypeID = { FNumber = ("SAL_OUTSTOCK").ToUpper() + "_" + crmoutstock.crminvexportheaders.orderTypeName },
                        FRobamDate = crmoutstock.crminvexportheaders.orderDate,
                        FCustomerID = { FNumber = crmoutstock.crminvexportheaders.customerCode??"001" },
                        FEntity = list,
                        FRobamCompany = {FNumber = forgid },
                        FLinkMan = crmoutstock.crminvexportheaders.customerName,
                        FLinkPhone = crmoutstock.crminvexportheaders.customerContact,
                    }
                };
                return ins;
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);    
            }
            return null;
        }
        public SyncResult SyncRealOutStockBill(K3Cloud_OutStockBill ko)
        {
            try
            {
                string paras = JsonConvert.SerializeObject(ko);
                var ret = ZJF_WEBAPI.sendRepuest(K3Cloud_AddressType.save, new object[] { "SAL_OUTSTOCK", paras });
                var jobj = JObject.Parse(ret);
                if (jobj.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return SyncResult.AllSuccess;
                }
                else
                {
                    return SyncResult.ErrorDuringSync;
                }
            }
            catch(Exception exp)
            {
                Logger.log(exp.Message);
            }
            return SyncResult.ErrorDuringSync;
        }
    }
}
