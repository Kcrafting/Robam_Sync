using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using static System.Net.WebRequestMethods;
using Models;
using Utils;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Threading;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.ClearScript.V8.V8CpuProfile;
using System.IO;
using System.Collections;
using System.Globalization;
using Robam_Sync;

namespace Utils
{
    
    public enum UrlType
    {
        crm_signin = 0,
        distribution_origin = 1000,
        distribution_signin = 1001,
        distribution_outstock = 1002,
        distribution_qrcode = 1003,
        distribution_outstockdetail = 1004,
    }
    public class RobamApi
    {
        #region Urls
        public static Dictionary<UrlType, string> urls = new Dictionary<UrlType, string>() {
            { UrlType.crm_signin,@""},
            { UrlType.distribution_origin,@"http://ims.hzrobam.com"},
            { UrlType.distribution_signin,@"http://ims.hzrobam.com/robamIMS/com.saip.application.login.flow"},
            { UrlType.distribution_outstock,@"http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.ims.reportForms.crmInvexorderheaders.queryCrmInvExOrderHeaders.biz.ext" },
            { UrlType.distribution_qrcode,@"http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.inv.robam.crmbarcode.queryBarcodeList.biz.ext"},
            { UrlType.distribution_outstockdetail ,@"http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.inv.crminvexportheadersbiz.getCrmInvExportHeadersById.biz.ext?exOrderHeadersId=%1&_=%2"}
        };
        #endregion Urls
        #region A1
        static List<UserAccount> AccountList = new List<UserAccount>();
        public static void RecordError(string txt, [CallerMemberName] string funcname = "", [CallerFilePath] string filename = "", [CallerLineNumber] int linenumber = 0)
        {
            Sqlite_Helper_Static.write(new Sqlite_Models_ErrorMessage()
            {
                FRecordTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                FErrorFilepath = filename,
                FErrorFunction = funcname,
                FErrorLinenumber = linenumber.ToString(),
                FErrorMessage = txt
            });
        }
        public enum ServerType
        {
            //老板CRM系统
            Robam_Crm = 0,
            //老板分销系统
            Robam_Distribution = 1,
            //未设置
            Robam_Unset = 2
        }
        public enum BusinessType
        {
            //配件
            Robam_PJ = 0,
            //产品
            Robam_CP = 1,
            //未设置
            Robam_Unset = 2,
        }
        public enum Area
        {
            //廊坊
            LF = 0,
            //沧州
            CZ = 1,
            //未设置
            Unset = 2,
        }
        #endregion A1
        #region A2
        public class UserAccount : K3Cloud_RobamAccount
        {
            public string Account { get { return base.FAccount; } set { base.FAccount = value; } }
            public string Password { get { 
                    if(this.ServerType == ServerType.Robam_Distribution)
                    {
                        return Utils.DecryptPassword(base.FPWD);
                    }
                    else
                    {
                        return base.FPWD;
                    }
                
                } set { base.FPWD = value; } }
            public string Token { get; set; }
            public ServerType ServerType
            {
                get
                {
                    if (base.FAccountType == "CRM")
                    {
                        return ServerType.Robam_Crm;
                    }
                    else if (base.FAccountType == "FX")
                    {
                        return ServerType.Robam_Distribution;
                    }
                    return ServerType.Robam_Unset;
                }
                set
                {
                    if (value == ServerType.Robam_Crm)
                    {
                        base.FAccountType = "CRM";
                    }
                    else if (value == ServerType.Robam_Distribution)
                    {
                        base.FAccountType = "FX";
                    }
                    base.FAccountType = "";
                }
            }
            public BusinessType BusinessType
            {
                get
                {
                    if (base.FAccountRight == "PJ")
                    {
                        return BusinessType.Robam_PJ;
                    }
                    else if (base.FAccountRight == "CP")
                    {
                        return BusinessType.Robam_CP;
                    }
                    return BusinessType.Robam_Unset;
                }
                set
                {
                    if (value == BusinessType.Robam_PJ)
                    {
                        base.FAccountRight = "PJ";
                    }
                    else if (value == BusinessType.Robam_CP)
                    {
                        base.FAccountRight = "CP";
                    }
                    base.FAccountRight = "";
                }
            }
            public Area Area
            {
                get
                {
                    if (base.FAccountArea == "LF")
                    {
                        return Area.LF;
                    }
                    else if (base.FAccountArea == "CZ")
                    {
                        return Area.CZ;
                    }
                    return Area.Unset;
                }
                set
                {
                    if (value == Area.CZ)
                    {
                        base.FAccountArea = "CZ";
                    }
                    else if (value == Area.LF)
                    {
                        base.FAccountArea = "LF";
                    }
                    base.FAccountArea = "";
                }
            }
            public bool GetToken()
            {
                try
                {
                    if (ServerType == ServerType.Robam_Crm)
                    {

                    }
                    else if (ServerType == ServerType.Robam_Distribution)
                    {

                    }
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }

                return false;
            }
        }
        public abstract class Robam
        {
            protected UserAccount m_Account;
            protected string m_Token;
            volatile protected bool m_login;
            protected DateTime m_loginTime;
            public string m_errorString { get; set; }
            protected abstract void Checklogin();
            public abstract bool SignIn();
        }
        #endregion A2
        #region CRM
        public class Robam_CRM : Robam
        {
            /*
            老板CRM系统登录过程
            输入用户密码：登录 ->返回一个选择用户表格
            获取用户相关角色
            获取角色权限
            调用接口
             */
            public CRM_AcctMsg m_CurrentAcct = null;
            public string m_operatorId = "";
            public Robam_CRM(UserAccount account)
            {
                m_Account = account;
            }
            void getCookie()
            {
                var client = new RestClient(

                //new HttpClient() {BaseAddress = new Uri(
                    urls[UrlType.distribution_signin]
                    //),Timeout = TimeSpan.FromMinutes(20),}
                );
                var request = new RestRequest();
                
                request.Method = Method.Get;
                //request.AddHeader("Cookie", "JSESSIONID=E8E0FEAA1D29A55379B5A4085E4850FE.s2");
                RestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                m_Token = client.CookieContainer.GetCookieHeader(new Uri(urls[UrlType.distribution_signin]));
            }
            RestRequest createRequest()
            {
                var request = new RestRequest();
                request.Method = Method.Post;
                request.Timeout = 20 * 60 * 1000;
                request.AddHeader("Accept", "application/json,text/javascript,*/*;q=0.01");
                request.AddHeader("Accept-Encoding", "gzip,deflate");
                request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + "cod=826.136;csd=10011419;");
                request.AddHeader("Host", "ims.hzrobam.com");
                request.AddHeader("Origin", "http://ims.hzrobam.com");
                request.AddHeader("Pragma", "no-cache");
                //request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/invExport/CrmInvExportdAllocation.jsp?processDefName=1&functionType=8&partOrGood=GOOD&printBathBtn2=&invEpBHZ=&editBtn=&exportHeadData=&startUpload=&invEpB=&synFxBtn=&updateBtn=&wtInConfirm=&saveBtn=&_t=495456");
                request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                //var json = JObject.Parse(body);
                //request.AddJsonBody(json.ToString(Newtonsoft.Json.Formatting.None));
                return request;
            }
            /// <summary>
            /// 系统登录
            /// </summary>
            /// <returns>登录结果</returns>
            public override bool SignIn()
            {
                try
                {
                    if (!m_login || (DateTime.Now - m_loginTime) > TimeSpan.FromMinutes(30))
                    {
                        getCookie();
                        var client = new RestClient(
                            new HttpClient()
                            {
                                BaseAddress = new Uri(
                                urls[UrlType.distribution_signin]
                                ),
                                Timeout = TimeSpan.FromMinutes(20)
                            }
                            );
                        var body = @"_eosFlowAction=login&userid=" + (m_Account?.Account ?? "") + "&password=" + (m_Account?.Password ?? "");
                        var request = new RestRequest();
                        request.Timeout = -1;
                        request.Method = Method.Post;
                        request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                        request.AddHeader("Accept-Encoding", "gzip, deflate");
                        request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                        request.AddHeader("Cache-Control", "no-cache");
                        request.AddHeader("Connection", "keep-alive");
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + "cod=826.136; csd=10011419; SIESAIP=");
                        request.AddHeader("Host", "ims.hzrobam.com");
                        request.AddHeader("Origin", urls[UrlType.distribution_origin]);
                        request.AddHeader("Pragma", "no-cache");
                        request.AddHeader("Referer", urls[UrlType.distribution_signin]);
                        request.AddHeader("Upgrade-Insecure-Requests", "1");
                        request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54");

                        request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        if (response.Content.Contains("请选择角色与相应账套"))
                        {
                            m_loginTime = DateTime.Now;
                            m_login = true;
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return false;
            }
            /// <summary>
            /// 获取operatorId
            /// </summary>
            /// <returns></returns>
            public string GetoperatorId()
            {
                var client = new RestClient(
                    new HttpClient()
                    {
                        BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/com.saip.application.login.flow"
                        ),
                        Timeout = TimeSpan.FromMinutes(20)
                    }
                    );

                var request = new RestRequest();
                request.Method = Method.Get;
                request.Timeout = -1;
                //request.AddHeader("Cookie", "JSESSIONID=E0A02531B05F559F5B145F73F3DFEC06.s2");
                request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                RestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                string html = response.Content;
                //判断是否登录成功
                if (html.Contains("请选择角色与相应账套"))
                {
                    var ret = FromJSPGetoperatorId();
                    return ret;
                }
                else
                {
                    return "";
                }
            }
            /// <summary>
            /// JSP文件中获取operatorId
            /// </summary>
            /// <returns></returns>
            public string FromJSPGetoperatorId()
            {
                try
                {
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/login/CrmSelectRoleAndSob.jsp"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );

                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.Timeout = -1;
                    //request.AddHeader("Cookie", "JSESSIONID=E0A02531B05F559F5B145F73F3DFEC06.s2");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                    RestResponse response = client.Execute(request);
                    string html = response.Content;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNode node = doc.DocumentNode.SelectSingleNode(@"/html/body/div/table/tr/td/input");
                    var _operator = new Regex(@"[0-9]{1,}").Match(node.OuterHtml.Substring(node.OuterHtml.IndexOf("value"), node.OuterHtml.Length - node.OuterHtml.IndexOf("value"))).Value;
                    return _operator;
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return "";
            }
            /// <summary>
            /// 获取角色列表
            /// </summary>
            /// <returns>角色列表</returns>
            public CRM_Roles GetRoleList()
            {
                try
                {
                    m_operatorId = GetoperatorId();
                    var client = new RestClient(
                        new HttpClient()
                        {
                         BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/login/com.sie.ims.login.selectRole.getRolesByOperatorId.biz.ext"
                        ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/login/CrmSelectRoleAndSob.jsp");
                    request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{operatorId: """ + m_operatorId.ToString() + @"""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    var ret = JObject.Parse(response.Content).ToObject<CRM_Roles>();
                    return ret;
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            /// <summary>
            /// 注册和用户类型一致的角色
            /// </summary>
            /// <param name="cr">账套信息</param>
            /// <returns>注册结果</returns>
            public bool RegistDefaultRole()
            {
                var cr = GetRoleList();
                if(m_operatorId == "")
                {
                    m_errorString = "获取用户内码错误！";
                    return false;
                }
                if (m_Account.BusinessType == BusinessType.Robam_CP && m_Account.ServerType == ServerType.Robam_Crm)
                {
                    var ri = cr.roles.Where(i => i.rolename == "仓库管理员")?.FirstOrDefault();
                    if(ri == null)
                    {
                        m_errorString = "所选用户没有产品管理权限!";
                        return false;
                    }
                    m_CurrentAcct = RegistRole(ri, m_operatorId);
                }
                else if(m_Account.BusinessType == BusinessType.Robam_PJ && m_Account.ServerType == ServerType.Robam_Crm)
                {
                    var ri = cr.roles.Where(i => i.rolename == "配件管理员")?.FirstOrDefault();
                    if (ri == null)
                    {
                        m_errorString = "所选用户没有配件管理权限!";
                        return false;
                    }
                    m_CurrentAcct = RegistRole(ri, m_operatorId);
                }
                else
                {
                    m_errorString = "所选用户的类型或角色未设置或是不是CRM账户!";
                    return false;
                }
                return true;
            }
            /// <summary>
            /// 注册用户角色
            /// </summary>
            /// <param name="cr">账套信息</param>
            /// <param name="operatorID">用户内码</param>
            /// <returns>账套信息</returns>
            public CRM_AcctMsg RegistRole(CRM_Role cr,string operatorID)
            {
                try
                {
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/login/com.sie.ims.login.selectRole.getSobsByRoleID.biz.ext"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/login/CrmSelectRoleAndSob.jsp");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""roleId"":" + cr.roleid.ToString() + @",""operatorId"":""" + operatorID +  @"""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_AcctMsg>();
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            //获取权限
            public void registMUOByRoleAndSob(string para_body = @"{""roleId"":1605,""sobId"":1,""operatorId"":""4963957"",""rolename"":""成品管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}")
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return;
                        }
                    }
                    var client = new RestClient(new HttpClient() { BaseAddress = new Uri("http://ims.hzrobam.com/robamIMS/login/com.sie.ims.login.selectRole.registMUOByRoleAndSob.biz.ext"), Timeout = TimeSpan.FromMinutes(20) });
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " csd=10011419; cod=;");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/login/CrmSelectRoleAndSob.jsp?_winid=w3254&_t=717460");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = para_body;
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
            }
            public CRM_OutstockList GetOutstockbill(string startdate, string enddate)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    DateTime temp;
                    if (!DateTime.TryParse(startdate, out temp))
                    {
                        return null;
                    }
                    if (!DateTime.TryParse(enddate, out temp))
                    {
                        return null;
                    }

                    using var client = new RestClient(new HttpClient() { BaseAddress = new Uri(urls[UrlType.distribution_outstock]), Timeout = TimeSpan.FromMinutes(20) });

                    Func<int, int, int, RestRequest> lbm = (pageIndex, begin, length) =>
                    {
                        var json = JObject.Parse(@"{""criteria"":
                        {""_entity"":""com.sie.crm.pub.dataset.INVConfig.CrmInvExOrderHeadersV"",
                        ""_expr"":
                            [{""sourceOrderNo"":"""",""_op"":""like""},
                            {""orderNo"":"""",""_op"":""like""},
                            {""sourceType"":"""",""_op"":""like""},
                            {""status"":""D2"",""_op"":""in""},
                            {""reservationDeliveryDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},
                            {""temp"":""8"",""_op"":""=""},
                            {""reservationDeliveryDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},
                            {""inventoryCode"":"""",""_op"":""=""},
                            {""receiveInventoryCode"":"""",""_op"":""=""},
                            {""dmdNum"":"""",""_op"":""like""},
                            {""receiptFlag"":"""",""_op"":""=""},
                            {""customerReceiveFlag"":"""",""_op"":""=""},
                            {""locationDesc"":"""",""_op"":""like""},
                            {""receiveLocationDesc"":"""",""_op"":""like""},
                            {""orderSourceTypeCode"":"""",""_op"":""like""},
                            {""customerTypeId"":"""",""_op"":""=""},
                            {""productOrgName"":"""",""_op"":""=""},
                            {""vendorName"":"""",""_op"":""like""},
                            {""salesAreaName"":"""",""_op"":""like""},
                            {""exportConfirmDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},
                            {""exportConfirmDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},
                            {""receiveDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},
                            {""receiveDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},
                            {""productOrgName2"":"""",""_op"":""=""},
                            {""customerName"":"""",""_op"":""like""},
                            {""customerCode"":"""",""_op"":""=""},
                            {""orderTypeId"":"""",""_op"":""=""},
                            {""salesAreaName"":"""",""_op"":""like""},
                            {""orderTypeName"":"""",""_op"":""like""},
                            {""orderDate"":""" + DateTime.Parse(startdate).ToString("yyyy-MM-dd") + @""",""_op"":"">=""},
                            {""orderDate"":""" + DateTime.Parse(enddate).ToString("yyyy-MM-dd") + @""",""_op"":""<=""},
                            {""_op"":""like"",""exportConfirmByName"":""""},
                            {""_op"":""="",""actualQuantityFlag"":""""},
                            {""_op"":""="",""printFlag"":""""},
                            {""_op"":""="",""lastUpdatedByName"":""""},
                            {""_op"":""like"",""material_desc_op"":""""},
                            {""_op"":""like"",""material_code_op"":""""},
                            {""_op"":""like"",""product_type_name_op"":""""},
                            {""_op"":""like"",""product_type_code_op"":""""},
                            {""is_lock_flag"":"""",""_op"":""=""},
                            {""_op"":""=""},
                            {""sortField1"":"""",""sortField2"":"""",""_op"":""=""},
                            {""deliveryCustomerCode"":"""",""_op"":""=""},
                            {""deliveryCustomerName"":"""",""_op"":""=""},
                            {""fxOrderNo"":"""",""_op"":""like"",""contactName"":"""",""contactTel"":"""",""chauffeur"":"""",""sourceNo"":""""},
                            {""partorgood"":""GOOD"",""_op"":""=""},
                            {""lastupdateby"":"""",""_op"":""like""}]
                            },""pageIndex"":" + pageIndex.ToString() +
                            @",""pageSize"":100,
                            ""sortField"":"""",
                            ""sortOrder"":"""",
                            ""page"":{""begin"":" + begin.ToString() + @",
                            ""length"":" + length.ToString() + @",
                            ""isCount"":true}}");
                        var request = createRequest();
                        request.AddJsonBody(json.ToString(Newtonsoft.Json.Formatting.None));
                        return request;
                    };

                    RestResponse response = client.Execute(lbm.Invoke(0, 0, 1000));

                    Console.WriteLine(response.Content);

                    var j2 = JObject.Parse(response.Content);

                    //var dewww = JsonConvert.DeserializeObject<CRM_OutstockList>(response.Content);
                    var ret = j2.ToObject<CRM_OutstockList>();
                    //var listnum = Convert.ToInt32(j2.SelectToken(@"page.['count']")?.ToString() ?? "0");
                    //var listtotlepage = Convert.ToInt32(j2.SelectToken(@"page.['totalPage']")?.ToString() ?? "0");
                    //if(listtotlepage > 1)
                    //{
                    //    for(int i = 2; i <= listtotlepage; i++)
                    //    {
                    //        var dd1 = client.Execute(lbm.Invoke(i, i * 100, (listnum - i * 100 > 100 ? 100 : listnum - i * 100))).Content;
                    //        j2.Merge(JObject.Parse(dd1));
                    //    }
                    //}
                    return ret;
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);

                }
                finally
                {

                }
                return null;
            }
            public CRM_OutStockDetail GetOutstockbillDetail(string exOrderHeadersId)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));// Local. .ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
                    string url = urls[UrlType.distribution_outstockdetail].Replace("%1", exOrderHeadersId).Replace("%2", timeStamp.ToString());
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        url
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.Timeout = 600 * 1000;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + "cod=826.136;");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/invExport/CrmInvExportdAllocation.jsp?processDefName=1&functionType=8&partOrGood=GOOD&printBathBtn2=&invEpBHZ=&editBtn=&exportHeadData=&startUpload=&invEpB=&synFxBtn=&updateBtn=&wtInConfirm=&saveBtn=&_t=223948");
                    //client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54";
                    request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.54");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddHeader("Content-Type", "text/plain");
                    var body = @"exOrderHeadersId=" + exOrderHeadersId + "&_=" + timeStamp.ToString();
                    request.AddParameter("text/plain", body, ParameterType.RequestBody);

                    RestResponse response = client.Execute(request);
                    var j1 = JObject.Parse(response.Content);
                    return j1.ToObject<CRM_OutStockDetail>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                finally
                {

                }
                return null;
            }
            public CRM_Qrcode GetBillQrcode(string billno)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (string.IsNullOrEmpty(billno))
                    {
                        return null;
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        urls[UrlType.distribution_qrcode]
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var json = JObject.Parse(@"{""criteria"":{""_entity"":""com.sie.crm.inv.robam.barcode.CrmInvExOrderLinesnV"",""_expr"":[{""orderNumber"":""" + billno + @""",""_op"":""=""},{""barcode"":"""",""_op"":""like""},{""materialCode"":"""",""_op"":""like""},{""materialName"":"""",""_op"":""like""},{""invType"":""IN"",""_op"":""=""},{""checkResult"":""C"",""_op"":""=""}],""_orderby"":[{""_sort"":""ASC"",""_property"":""materialCode""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}");
                    var r = createRequest();
                    r.AddJsonBody(json.ToString(Newtonsoft.Json.Formatting.None));
                    RestResponse response = client.Execute(r);

                    var j2 = JObject.Parse(response.Content);
                    return j2.ToObject<CRM_Qrcode>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                    return null;
                }
                finally
                {

                }
            }
            public CRM_ItemDetail GetItemDetail(string fnumber)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                    new HttpClient() { BaseAddress = new Uri("http://ims.hzrobam.com/robamIMS/baseconfig/materias/com.sie.crm.pub.baseconfig.crmecreference.qeryMaterialsGood.biz.ext"), Timeout = TimeSpan.FromMinutes(20) });
                    var body = @"{""paramMap"":{""functionType"":""4"",""materialName"":"""",""salesStatus"":"""",""productTypeName"":"""",""setOfBookName"":""老板电器账套"",""userId"":""4963957"",""orgName"":"""",""materialType"":"""",""isConf"":"""",""scanBar"":"""",""materialCode"":""" + fnumber + @""",""saleableEnabledFlag"":"""",""specification"":"""",""customerName"":""""},""page"":{""begin"":0,""length"":20,""isCount"":true},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":""""}";
                    var jbody = JObject.Parse(body);
                    string pbody = jbody.ToString(Formatting.None);
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=149.122; csd=10011528;");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/baseconfig/materias/CrmPubMaterialsMaintain.jsp?functionType=4&_t=900584");
                    request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddJsonBody(pbody);
                    //request.AddParameter("application/json", pbody, ParameterType.RequestBody);

                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_ItemDetail>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);

                }
                return null;
            }
            public CRM_INBillType GetInBillType()
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(new HttpClient() { BaseAddress = new Uri("http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.pub.util.pubDatas.queryDataObjectsWithPage.biz.ext"), Timeout = TimeSpan.FromMinutes(20) });
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.136; csd=10011419;");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    //request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/invExport/CrmInvExportdAllocation.jsp?processDefName=1&functionType=8&partOrGood=GOOD&printBathBtn2=&invEpBHZ=&editBtn=&exportHeadData=&startUpload=&invEpB=&synFxBtn=&updateBtn=&wtInConfirm=&saveBtn=&_t=12208");
                    request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.lookup.pubEntityQE.CrmPubOrderTypeQE"",""_expr"":[{""orderTypeName"":"""",""_op"":""like""},{""isOptional"":""Y"",""_op"":""=""},{""partorgood"":""GOOD"",""_op"":""=""},{""invFlag"":""2,3"",""_op"":""in""}]},""pageIndex"":0,""pageSize"":100,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":100,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_INBillType>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return null;
            }
            //同步仓库
            protected override void Checklogin()
            {
                if (m_login && (DateTime.Now - m_loginTime) < TimeSpan.FromMinutes(30))
                {
                    return;
                }
                else
                {
                    if (SignIn())
                    {
                        m_login = true;
                    }
                }
            }
            public CRM_OutstockList GetRealStockBill(string startDate,string endDate)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/inv/com.sie.crm.ims.reportForms.crmInvexorderheaders.queryCrmInvExOrderHeaders.biz.ext"
                        ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    //request.Timeout 
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + "; cod=826.10010090.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/Confirm.jsp?partOrGood=GOOD&functionType=2&menuId=2&invmyNo=&printBathBtn3=&printBathBtn4=&invmyHZ=&isBtn4=&cancel=&exportHeadData=&startUpload=&invmy=&synFxBtn=&updateBtn=&printBathBtn2=&isBtn2=&isBtn=&saveBtn=&editBtn=&_t=41350");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.INVConfig.CrmInvExOrderHeadersV"",""_expr"":[{""sourceOrderNo"":"""",""_op"":""like""},{""orderNo"":"""",""_op"":""like""},{""sourceType"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""orgId"":"""",""_op"":""=""},{""temp"":""2"",""_op"":""=""},{""productOrgId"":"""",""_op"":""=""},{""inventoryCode"":"""",""_op"":""=""},{""receiveInventoryCode"":"""",""_op"":""=""},{""dmdNum"":"""",""_op"":""like""},{""receiptFlag"":"""",""_op"":""=""},{""customerReceiveFlag"":"""",""_op"":""=""},{""locationDesc"":"""",""_op"":""like""},{""receiveLocationDesc"":"""",""_op"":""like""},{""orderSourceTypeCode"":"""",""_op"":""like""},{""customerTypeId"":"""",""_op"":""=""},{""productOrgName"":"""",""_op"":""=""},{""vendorName"":"""",""_op"":""like""},{""salesAreaName"":"""",""_op"":""like""},{""exportConfirmDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""exportConfirmDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""productOrgName2"":""配件"",""_op"":""=""},{""customerName"":"""",""_op"":""=""},{""customerCode"":"""",""_op"":""=""},{""orderTypeId"":"""",""_op"":""=""},{""salesAreaName"":"""",""_op"":""like""},{""orderTypeName"":"""",""_op"":""like""},{""orderDate"":""" + startDate + @""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderDate"":""" + endDate + @""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""_op"":""like"",""exportConfirmByName"":""""},{""_op"":""=""},{""_op"":""="",""printFlag"":""""},{""_op"":""="",""lastUpdatedByName"":""""},{""_op"":""like"",""material_desc_op"":""""},{""_op"":""like"",""material_code_op"":""""},{""_op"":""like"",""product_type_name_op"":""""},{""_op"":""like"",""product_type_code_op"":""""},{""is_lock_flag"":"""",""_op"":""=""},{""_op"":""=""},{""_op"":""=""},{""deliveryCustomerCode"":"""",""_op"":""=""},{""deliveryCustomerName"":"""",""_op"":""=""},{""fxOrderNo"":"""",""_op"":""like"",""chauffeur"":"""",""vehicleBrand"":"""",""contactTel"":"""",""createdByName"":"""",""materialCode"":"""",""receiptStatusName"":"""",""exFlag"":"""",""receiptFlag"":"""",""contractNumber"":"""",""inceptAreaName"":"""",""strategyContract"":"""",""engineeringProject"":"""",""transType"":""""},{""partorgood"":""GOOD"",""_op"":""=""},{},{},{}]},""pageIndex"":0,""pageSize"":500,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":500,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    var list = JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                    return list;
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public CRM_OutstockList GetRealStockBill(string billno)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/inv/com.sie.crm.ims.reportForms.crmInvexorderheaders.queryCrmInvExOrderHeaders.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    //request.Timeout 
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + "; cod=826.10010090.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/Confirm.jsp?partOrGood=GOOD&functionType=2&menuId=2&invmyNo=&printBathBtn3=&printBathBtn4=&invmyHZ=&isBtn4=&cancel=&exportHeadData=&startUpload=&invmy=&synFxBtn=&updateBtn=&printBathBtn2=&isBtn2=&isBtn=&saveBtn=&editBtn=&_t=41350");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.INVConfig.CrmInvExOrderHeadersV"",""_expr"":[{""sourceOrderNo"":"""",""_op"":""like""},{""orderNo"":""" + billno + @""",""_op"":""like""},{""sourceType"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""orgId"":"""",""_op"":""=""},{""temp"":""2"",""_op"":""=""},{""productOrgId"":"""",""_op"":""=""},{""inventoryCode"":"""",""_op"":""=""},{""receiveInventoryCode"":"""",""_op"":""=""},{""dmdNum"":"""",""_op"":""like""},{""receiptFlag"":"""",""_op"":""=""},{""customerReceiveFlag"":"""",""_op"":""=""},{""locationDesc"":"""",""_op"":""like""},{""receiveLocationDesc"":"""",""_op"":""like""},{""orderSourceTypeCode"":"""",""_op"":""like""},{""customerTypeId"":"""",""_op"":""=""},{""productOrgName"":"""",""_op"":""=""},{""vendorName"":"""",""_op"":""like""},{""salesAreaName"":"""",""_op"":""like""},{""exportConfirmDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""exportConfirmDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""productOrgName2"":""配件"",""_op"":""=""},{""customerName"":"""",""_op"":""=""},{""customerCode"":"""",""_op"":""=""},{""orderTypeId"":"""",""_op"":""=""},{""salesAreaName"":"""",""_op"":""like""},{""orderTypeName"":"""",""_op"":""like""},{""orderDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""_op"":""like"",""exportConfirmByName"":""""},{""_op"":""=""},{""_op"":""="",""printFlag"":""""},{""_op"":""="",""lastUpdatedByName"":""""},{""_op"":""like"",""material_desc_op"":""""},{""_op"":""like"",""material_code_op"":""""},{""_op"":""like"",""product_type_name_op"":""""},{""_op"":""like"",""product_type_code_op"":""""},{""is_lock_flag"":"""",""_op"":""=""},{""_op"":""=""},{""_op"":""=""},{""deliveryCustomerCode"":"""",""_op"":""=""},{""deliveryCustomerName"":"""",""_op"":""=""},{""fxOrderNo"":"""",""_op"":""like"",""chauffeur"":"""",""vehicleBrand"":"""",""contactTel"":"""",""createdByName"":"""",""materialCode"":"""",""receiptStatusName"":"""",""exFlag"":"""",""receiptFlag"":"""",""contractNumber"":"""",""inceptAreaName"":"""",""strategyContract"":"""",""engineeringProject"":"""",""transType"":""""},{""partorgood"":""GOOD"",""_op"":""=""},{},{},{}]},""pageIndex"":0,""pageSize"":500,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":500,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    var list = JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                    return list;
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutStockDetail GetRealStockBillDetail(string exOrderHeadersId)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/inv/com.sie.crm.inv.crminvexportheadersbiz.getCrmInvExportHeadersById.biz.ext?exOrderHeadersId=" + exOrderHeadersId + "&_=" + Utils.GetMillisecondsTimeStemp().ToString()
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/Confirm.jsp?partOrGood=GOOD&functionType=2&menuId=2&exportHeadData=&startUpload=&invmy=&synFxBtn=&updateBtn=&printBathBtn2=&isBtn2=&isBtn=&saveBtn=&editBtn=&invmyHZ=&isBtn4=&printBathBtn4=&cancel=&invmyNo=&printBathBtn3=&_t=254473");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutStockDetail>();

                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_Qrcode GetRealStockBillQrcode(string orderNumber)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.inv.robam.crmbarcode.queryBarcodeList.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " csd=10010246; cod=826.10011418.136.10010090");
                    request.AddHeader("Host", " ims.hzrobam.com");
                    request.AddHeader("Origin", " http://ims.hzrobam.com");
                    request.AddHeader("Pragma", " no-cache");
                    request.AddHeader("Referer", " http://ims.hzrobam.com/robamIMS/inv/invExport/AddInvBarcodeList.jsp?_winid=w9871&_t=57439");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.inv.robam.barcode.CrmInvExOrderLinesnV"",""_expr"":[{""orderNumber"":""" + orderNumber + @""",""_op"":""=""},{""barcode"":"""",""_op"":""like""},{""materialCode"":"""",""_op"":""like""},{""materialName"":"""",""_op"":""like""},{""invType"":""OUT"",""_op"":""=""},{""checkResult"":""C"",""_op"":""=""}],""_orderby"":[{""_sort"":""ASC"",""_property"":""materialCode""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_Qrcode>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public void registMUOByRoleAndSob()
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return ;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return ;
                        }
                    }
                    //
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return ;
            }

            public CRM_OutstockList GetPartsInstockBill(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    RegistDefaultRole();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    //var _buy = GetPartsInstockBillForBuy( startDate,  endDate);
                    //var _return = GetPartsInstockBillForReturn(startDate, endDate);
                    //if(_buy?.crminvexportheaderss!=null && _return?.crminvexportheaderss != null)
                    //{
                    //    _buy.crminvexportheaderss.AddRange(_return.crminvexportheaderss);
                    //    return _buy;
                    //}
                    //else if(_buy?.crminvexportheaderss != null && _return?.crminvexportheaderss == null)
                    //{
                    //    return _buy;
                    //}
                    //else if(_buy?.crminvexportheaderss == null && _return?.crminvexportheaderss != null)
                    //{
                    //    return _return;
                    //}
                    return GetPartsInstockBill_All(startDate, endDate);
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public CRM_OutstockList GetPartsReturnBack(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    RegistDefaultRole();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    //配件退回 生成红字入库单
                    DateTime sd = new DateTime();
                    DateTime ed = new DateTime();
                    if(!DateTime.TryParse(startDate,out sd))
                    {
                        m_errorString = "开始日期未能转换成datetime!";
                        return null;
                    }
                    if (!DateTime.TryParse(endDate, out ed))
                    {
                        m_errorString = "结束日期未能转换成datetime!";
                        return null;
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/so/order/com.sie.crm.so.crmvaluationapplication.queryValuationHeader.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " ; cod=826.377.136; csd=10011265");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    //request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/so/order/ValuationApplicationView.jsp?processDefName=ValuationApplication&statusType=A&functionType=PARTS_APPLY&IntroducedBtn=&addLineBtn2=&removeLineBtn=&addLineBtn=&zbPrint=&btnPrint=&submitBtn=&saveBtn=&printBathBtn2=&deleteBtn=&editBtn=&addBtn=&_t=109845");
                    request.AddHeader("UserAgent" , " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.54");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{""functionType"":""PARTS_APPLY"",""criteria"":{""_entity"":""com.sie.crm.pub.dataset.soEntity.CrmSoOrderHeaderQv1"",""_expr"":[{""orderNum"":"""",""_op"":""=""},{""orderTypeName"":"""",""_op"":""=""},{""customerName"":"""",""_op"":""like""},{""creationDate"":""" + sd.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""creationDate"":""" + ed.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""salesAreaName"":"""",""_op"":""like""},{""deliveryCustomerName"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""customerTypeName"":"""",""_op"":""like""},{""deliveryCustomerTypeName"":"""",""_op"":""like""},{""superiorCustomerName"":"""",""_op"":""like""},{""goodsTypeName"":"""",""_op"":""like""},{""writeRaIfFlag"":"""",""_op"":""=""},{""exportFlag"":"""",""_op"":""=""},{""exportFinishFlag"":"""",""_op"":""=""},{""functionType"":""PARTS_APPLY"",""_op"":""=""},{""isInTransit"":"""",""_op"":""=""},{""needErpFlag"":"""",""_op"":""=""},{""erpFlag"":"""",""_op"":""=""},{""writeInvIfFlag"":"""",""_op"":""=""},{""acUserId"":""4964588"",""_op"":""=""},{""branchApprovalTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""branchApprovalTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""lastUpdatedName"":"""",""_op"":""like""},{""orderCommitTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderCommitTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""}],""_orderby"":[{""_sort"":""desc"",""_property"":""orderId""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutStockDetail GetPartsInstockBillDetail(string exOrderHeadersId)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_Qrcode GetPartsInStockBillQrcode(string orderNumber)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public CRM_OutstockList GetPartsInstockBillForBuy(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/so/order/com.sie.crm.so.crmvaluationapplication.queryValuationHeader.biz.ext"
                            ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.145.142.377.10011418.10011324.136; csd=10011274");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/so/order/ValuationApplicationView.jsp?processDefName=ValuationApplication&statusType=A&functionType=PARTS_RETURN&IntroducedBtn=&addLineBtn2=&removeLineBtn=&addLineBtn=&zbPrint=&btnPrint=&submitBtn=&saveBtn=&printBathBtn2=&deleteBtn=&editBtn=&addBtn=&_t=248600");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""functionType"":""PARTS_RETURN"",""criteria"":{""_entity"":""com.sie.crm.pub.dataset.soEntity.CrmSoOrderHeaderQv1"",""_expr"":[{""orderNum"":"""",""_op"":""=""},{""orderTypeName"":"""",""_op"":""=""},{""customerName"":"""",""_op"":""like""},{""creationDate"":""" + startDate.ToString() + @""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""creationDate"":""" + endDate +  @""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""salesAreaName"":"""",""_op"":""like""},{""deliveryCustomerName"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""customerTypeName"":"""",""_op"":""like""},{""deliveryCustomerTypeName"":"""",""_op"":""like""},{""superiorCustomerName"":"""",""_op"":""like""},{""goodsTypeName"":"""",""_op"":""like""},{""writeRaIfFlag"":"""",""_op"":""=""},{""exportFlag"":"""",""_op"":""=""},{""exportFinishFlag"":"""",""_op"":""=""},{""functionType"":""PARTS_RETURN"",""_op"":""=""},{""isInTransit"":"""",""_op"":""=""},{""needErpFlag"":"""",""_op"":""=""},{""erpFlag"":"""",""_op"":""=""},{""writeInvIfFlag"":"""",""_op"":""=""},{""acUserId"":""4964588"",""_op"":""=""},{""branchApprovalTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""branchApprovalTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""lastUpdatedName"":"""",""_op"":""like""},{""orderCommitTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderCommitTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""}],""_orderby"":[{""_sort"":""desc"",""_property"":""orderId""}]},""pageIndex"":0,""pageSize"":200,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":200,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutStockDetail GetPartsInstockBillDetailForBuy(string exOrderHeadersId)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/so/order/com.sie.crm.so.crmsoorderheaderbiz.getCrmSoOrderHeaderById.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.145.142.377.10011418.10011324.136; csd=10011274");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/so/order/ValuationApplicationDetail.jsp?processDefName=ValuationApplication&businessid=31975091&functionType=PARTS_RETURN&statusType=A&processDefName=ValuationApplication&serviceType=&normalEditType=&businessType=F_SoOrderHeader&parentTabName=tab$10011274&_t=496746");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""orderId"":""" + exOrderHeadersId + @"""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutStockDetail>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_Qrcode GetPartsInStockBillQrcodeForBuy(string orderNumber)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public CRM_OutstockList GetPartsInstockBillForReturn(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/so/order/com.sie.crm.so.crmvaluationapplication.queryValuationHeader.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.145.142.377.10011418.10011324.136; csd=10011274");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/so/order/ValuationApplicationView.jsp?processDefName=ValuationApplication&statusType=A&functionType=PARTS_RETURN&IntroducedBtn=&addLineBtn2=&removeLineBtn=&addLineBtn=&zbPrint=&btnPrint=&submitBtn=&saveBtn=&printBathBtn2=&deleteBtn=&editBtn=&addBtn=&_t=677029");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""functionType"":""PARTS_RETURN"",""criteria"":{""_entity"":""com.sie.crm.pub.dataset.soEntity.CrmSoOrderHeaderQv1"",""_expr"":[{""orderNum"":"""",""_op"":""=""},{""orderTypeName"":"""",""_op"":""=""},{""customerName"":"""",""_op"":""like""},{""creationDate"":""2022-09-23 00:00:00"",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""creationDate"":""2022-09-24 00:00:00"",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""salesAreaName"":"""",""_op"":""like""},{""deliveryCustomerName"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""customerTypeName"":"""",""_op"":""like""},{""deliveryCustomerTypeName"":"""",""_op"":""like""},{""superiorCustomerName"":"""",""_op"":""like""},{""goodsTypeName"":"""",""_op"":""like""},{""writeRaIfFlag"":"""",""_op"":""=""},{""exportFlag"":"""",""_op"":""=""},{""exportFinishFlag"":"""",""_op"":""=""},{""functionType"":""PARTS_RETURN"",""_op"":""=""},{""isInTransit"":"""",""_op"":""=""},{""needErpFlag"":"""",""_op"":""=""},{""erpFlag"":"""",""_op"":""=""},{""writeInvIfFlag"":"""",""_op"":""=""},{""acUserId"":""4964588"",""_op"":""=""},{""branchApprovalTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""branchApprovalTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""lastUpdatedName"":"""",""_op"":""like""},{""orderCommitTime"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderCommitTime"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""}],""_orderby"":[{""_sort"":""desc"",""_property"":""orderId""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutstockList GetPartsInstockBill_All(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                    DateTime sd = new DateTime();
                    DateTime ed = new DateTime();
                    if(!DateTime.TryParse(startDate,out sd))
                    {
                        m_errorString = "错误:startDate转换为DataTime发生错误!";
                        return null;
                    }
                    if(!DateTime.TryParse(endDate,out ed))
                    {
                        m_errorString = "错误:endDate转换为DataTime发生错误!";
                        return null;
                    }

                    var client = new RestClient(
                        new HttpClient() { BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.ims.reportForms.crmInvexorderheaders.queryCrmInvExOrderHeaders.biz.ext"
                        ), Timeout = TimeSpan.FromMinutes(20) } );

                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.Timeout = 0;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", m_Token + ";cod=826.136; csd=10011324");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/invExport/CrmInvExportdAllocation.jsp?functionType=8&processDefName=1&partOrGood=PART&printBathBtn2=&invEpBHZ=&editBtn=&exportHeadData=&startUpload=&invEpB=&synFxBtn=&updateBtn=&wtInConfirm=&saveBtn=&_t=429470");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.52");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.INVConfig.CrmInvExOrderHeadersV"",""_expr"":[{""sourceOrderNo"":"""",""_op"":""like""},{""orderNo"":"""",""_op"":""like""},{""sourceType"":"""",""_op"":""like""},{""status"":"""",""_op"":""in""},{""reservationDeliveryDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""temp"":""8"",""_op"":""=""},{""reservationDeliveryDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""inventoryCode"":"""",""_op"":""=""},{""receiveInventoryCode"":"""",""_op"":""=""},{""dmdNum"":"""",""_op"":""like""},{""receiptFlag"":"""",""_op"":""=""},{""customerReceiveFlag"":"""",""_op"":""=""},{""locationDesc"":"""",""_op"":""like""},{""receiveLocationDesc"":"""",""_op"":""like""},{""orderSourceTypeCode"":"""",""_op"":""like""},{""customerTypeId"":"""",""_op"":""=""},{""productOrgName"":"""",""_op"":""=""},{""vendorName"":"""",""_op"":""like""},{""salesAreaName"":"""",""_op"":""like""},{""exportConfirmDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""exportConfirmDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""productOrgName2"":"""",""_op"":""=""},{""customerName"":"""",""_op"":""like""},{""customerCode"":"""",""_op"":""=""},{""orderTypeId"":"""",""_op"":""=""},{""salesAreaName"":"""",""_op"":""like""},{""orderTypeName"":"""",""_op"":""like""},"+
                    @"{""orderDate"":""" + sd.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":"">=""}," +
                    @"{""orderDate"":""" + ed.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":""<=""},{""_op"":""like"",""exportConfirmByName"":""""},{""_op"":""="",""actualQuantityFlag"":""""},{""_op"":""="",""printFlag"":""""},{""_op"":""="",""lastUpdatedByName"":""""},{""_op"":""like"",""material_desc_op"":""""},{""_op"":""like"",""material_code_op"":""""},{""_op"":""like"",""product_type_name_op"":""""},{""_op"":""like"",""product_type_code_op"":""""},{""is_lock_flag"":"""",""_op"":""=""},{""_op"":""=""},{""sortField1"":"""",""sortField2"":"""",""_op"":""=""},{""deliveryCustomerCode"":"""",""_op"":""=""},{""deliveryCustomerName"":"""",""_op"":""=""},{""fxOrderNo"":"""",""fxOrderNo_op"":""like"",""contactName"":"""",""_op"":""like"",""contactTel"":"""",""chauffeur"":"""",""sourceNo"":""""},{""partorgood"":""PART"",""_op"":""=""},{""lastupdateby"":"""",""_op"":""like""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    return JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutStockDetail GetPartsInstockBillDetailForReturn(string exOrderHeadersId)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/so/order/com.sie.crm.so.crmsoorderheaderbiz.getCrmSoOrderHeaderById.biz.ext"
                            ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                   
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.145.142.377.10011418.10011324.136; csd=10011274");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/so/order/ValuationApplicationDetail.jsp?processDefName=ValuationApplication&businessid=31973372&functionType=PARTS_RETURN&statusType=A&processDefName=ValuationApplication&serviceType=&normalEditType=&businessType=F_SoOrderHeader&parentTabName=tab$10011274&_t=13946");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""orderId"":""31973372""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutStockDetail>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_Qrcode GetPartsInStockBillQrcodeForReturn(string orderNumber)
            {
                try
                {
                    Checklogin();
                    registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }

                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public CRM_OutstockList GetPartsOutstockBill(string startDate, string endDate)
            {
                try
                {
                    Checklogin();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    RegistDefaultRole();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    DateTime sd = new DateTime();
                    DateTime ed = new DateTime();
                    if(!DateTime.TryParse(startDate,out sd))
                    {
                        m_errorString = "错误:startDate无法转换为DateTime!";
                        return null;
                    }
                    if(!DateTime.TryParse(endDate,out ed))
                    {
                        m_errorString = "错误:endDate无法转换为DateTime!";
                        return null;
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/inv/com.sie.crm.ims.reportForms.crmInvexorderheaders.queryCrmInvExOrderHeaders.biz.ext"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }

                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/Confirm.jsp?partOrGood=GOOD&functionType=2&menuId=2&invmyNo=&printBathBtn3=&printBathBtn4=&invmyHZ=&isBtn4=&cancel=&exportHeadData=&startUpload=&invmy=&synFxBtn=&updateBtn=&printBathBtn2=&isBtn2=&isBtn=&saveBtn=&editBtn=&_t=7088");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.INVConfig.CrmInvExOrderHeadersV"",""_expr"":[{""sourceOrderNo"":"""",""_op"":""like""},{""orderNo"":"""",""_op"":""like""},{""sourceType"":"""",""_op"":""like""},{""status"":"""",""_op"":""=""},{""orgId"":"""",""_op"":""=""},{""temp"":""2"",""_op"":""=""},{""productOrgId"":"""",""_op"":""=""},{""inventoryCode"":"""",""_op"":""=""},{""receiveInventoryCode"":"""",""_op"":""=""},{""dmdNum"":"""",""_op"":""like""},{""receiptFlag"":"""",""_op"":""=""},{""customerReceiveFlag"":"""",""_op"":""=""},{""locationDesc"":"""",""_op"":""like""},{""receiveLocationDesc"":"""",""_op"":""like""},{""orderSourceTypeCode"":"""",""_op"":""like""},{""customerTypeId"":"""",""_op"":""=""},{""productOrgName"":"""",""_op"":""=""},{""vendorName"":"""",""_op"":""like""},{""salesAreaName"":"""",""_op"":""like""},{""exportConfirmDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""exportConfirmDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""receiveDate"":"""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""productOrgName2"":""配件"",""_op"":""=""},{""customerName"":"""",""_op"":""=""},{""customerCode"":"""",""_op"":""=""},{""orderTypeId"":"""",""_op"":""=""},{""salesAreaName"":"""",""_op"":""like""},{""orderTypeName"":"""",""_op"":""like""},{""orderDate"":""" +
                        sd.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":"">="",""_pattern"":""yyyy-MM-dd""},{""orderDate"":""" +
                        ed.ToString("yyyy-MM-dd HH:mm:ss") + @""",""_op"":""<="",""_pattern"":""yyyy-MM-dd""},{""_op"":""like"",""exportConfirmByName"":""""},{""_op"":""=""},{""_op"":""="",""printFlag"":""""},{""_op"":""="",""lastUpdatedByName"":""""},{""_op"":""like"",""material_desc_op"":""""},{""_op"":""like"",""material_code_op"":""""},{""_op"":""like"",""product_type_name_op"":""""},{""_op"":""like"",""product_type_code_op"":""""},{""is_lock_flag"":"""",""_op"":""=""},{""_op"":""=""},{""_op"":""=""},{""deliveryCustomerCode"":"""",""_op"":""=""},{""deliveryCustomerName"":"""",""_op"":""=""},{""fxOrderNo"":"""",""_op"":""like"",""chauffeur"":"""",""vehicleBrand"":"""",""contactTel"":"""",""createdByName"":"""",""materialCode"":"""",""receiptStatusName"":"""",""exFlag"":"""",""receiptFlag"":"""",""contractNumber"":"""",""inceptAreaName"":"""",""strategyContract"":"""",""engineeringProject"":"""",""transType"":""""},{""partorgood"":""GOOD"",""_op"":""=""},{},{},{}]},""pageIndex"":0,""pageSize"":500,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":500,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);

                    RestResponse response = client.Execute(request);
                    Console.WriteLine();
                    return JObject.Parse(response.Content).ToObject<CRM_OutstockList>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_OutStockDetail GetPartsOutstockBillDetail(string exOrderHeadersId)
            {

                try
                {
                    Checklogin();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/inv/com.sie.crm.inv.crminvexportheadersbiz.getCrmInvExportHeadersById.biz.ext?exOrderHeadersId=" + exOrderHeadersId + @"&_=" + Utils.GetMillisecondsTimeStemp().ToString()
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/Confirm.jsp?partOrGood=GOOD&functionType=2&menuId=2&invmyNo=&printBathBtn3=&printBathBtn4=&invmyHZ=&isBtn4=&cancel=&exportHeadData=&startUpload=&invmy=&synFxBtn=&updateBtn=&printBathBtn2=&isBtn2=&isBtn=&saveBtn=&editBtn=&_t=7088");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_OutStockDetail>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;

                
            }
            public CRM_Qrcode GetPartsOutStockBillQrcode(string orderNumber)
            {


                try
                {
                    Checklogin();
                    //registMUOByRoleAndSob(@"{""roleId"":1604,""sobId"":1,""operatorId"":""4964588"",""rolename"":""仓库管理员"",""orgCode"":""zgs"",""orgName"":""老板电器账套""}");
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/inv/invExport/com.sie.crm.inv.robam.crmbarcode.queryBarcodeList.biz.ext"
                            ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                  
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ";") + " cod=826.136; csd=10011418");
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/inv/invExport/AddInvBarcodeList.jsp?_winid=w3812&_t=159606");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.inv.robam.barcode.CrmInvExOrderLinesnV"",""_expr"":[{""orderNumber"":""" + orderNumber + @""",""_op"":""=""},{""barcode"":"""",""_op"":""like""},{""materialCode"":"""",""_op"":""like""},{""materialName"":"""",""_op"":""like""},{""invType"":""OUT"",""_op"":""=""},{""checkResult"":""C"",""_op"":""=""}],""_orderby"":[{""_sort"":""ASC"",""_property"":""materialCode""}]},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":"""",""page"":{""begin"":0,""length"":20,""isCount"":true}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_Qrcode>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;


               
            }
            public CRM_ItemDetail GetItemList()
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (!RegistDefaultRole())
                    {
                        m_errorString = "注册角色错误!";
                        return null;
                    }

                    Func<string, CRM_ItemDetail> Act = new Func<string, CRM_ItemDetail>((json) => {
                        var client = new RestClient(
                   new HttpClient() { BaseAddress = new Uri("http://ims.hzrobam.com/robamIMS/baseconfig/materias/com.sie.crm.pub.baseconfig.crmecreference.qeryMaterialsGood.biz.ext"), Timeout = TimeSpan.FromMinutes(20) });
                        var body = json;// @"{""paramMap"":{""functionType"":""4"",""materialName"":"""",""salesStatus"":"""",""productTypeName"":"""",""setOfBookName"":""" + m_CurrentAcct.sobs.FirstOrDefault().orgName + @""",""userId"":""" + m_operatorId + @""",""orgName"":"""",""materialType"":"""",""isConf"":""N"",""scanBar"":"""",""materialCode"":"""",""saleableEnabledFlag"":"""",""specification"":"""",""customerName"":""""},""page"":{""begin"":0,""length"":10000,""isCount"":true},""pageIndex"":0,""pageSize"":10000,""sortField"":"""",""sortOrder"":""""}";
                        var jbody = JObject.Parse(body);
                        string pbody = jbody.ToString(Formatting.None);
                        var request = new RestRequest();
                        request.Method = Method.Post;
                        request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                        request.AddHeader("Accept-Encoding", "gzip, deflate");
                        request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                        request.AddHeader("Cache-Control", "no-cache");
                        request.AddHeader("Connection", "keep-alive");
                        request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                        request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + "") + "");
                        request.AddHeader("Host", "ims.hzrobam.com");
                        request.AddHeader("Origin", "http://ims.hzrobam.com");
                        request.AddHeader("Pragma", "no-cache");
                        request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/baseconfig/materias/CrmPubMaterialsMaintain.jsp?functionType=4&_t=900584");
                        request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                        request.AddHeader("X-Requested-With", "XMLHttpRequest");
                        request.AddJsonBody(pbody);
                        //request.AddParameter("application/json", pbody, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);
                        return JObject.Parse(response.Content).ToObject<CRM_ItemDetail>();
                    });
                   
                    //Console.WriteLine(response.Content);
                    //try
                    //{
                    //    if (!Directory.Exists(@"./result/ret"))
                    //    {
                    //        Directory.CreateDirectory(@"./result/ret");
                    //    }
                    //    string pathName = @"./result/ret/s.txt";
                    //    FileStream fs = new FileStream(pathName, FileMode.OpenOrCreate);
                    //    StreamWriter wr = null;
                    //    wr = new StreamWriter(fs);
                    //    wr.WriteLine(response.Content);
                    //    wr.Close();
                    //}
                    //catch(Exception exp)
                    //{
                    //    Logger.log(exp.Message);
                    //    m_errorString = exp.Message;
                    //}
                    var ci = Act(@"{""paramMap"":{""functionType"":""4"",""materialName"":"""",""salesStatus"":"""",""productTypeName"":"""",""setOfBookName"":""" + m_CurrentAcct.sobs.FirstOrDefault().orgName + @""",""userId"":""" + m_operatorId + @""",""orgName"":"""",""materialType"":"""",""isConf"":""N"",""scanBar"":"""",""materialCode"":"""",""saleableEnabledFlag"":"""",""specification"":"""",""customerName"":""""},""page"":{""begin"":0,""length"":10000,""isCount"":true},""pageIndex"":0,""pageSize"":10000,""sortField"":"""",""sortOrder"":""""}");
                    int Pagestart = 0;
                    int PageNumber = 10000;
                    //查看多页
                    int Pages = 0;
                    if (ci.page.count > PageNumber)
                    {
                        Pages = (ci.page.count ?? 0) / PageNumber;
                        int c = (ci.page.count ?? 0) % PageNumber;
                        if (c > 0)
                        {
                            Pages++;
                        }
                        for (int i = 1; i < Pages; i++)
                        {
                            Thread.Sleep(1500);
                            Pagestart = i;
                            string body_temp = "";
                            if (i + 1 == Pages)
                            {
                                body_temp = @"{""paramMap"":{""functionType"":""4"",""materialName"":"""",""salesStatus"":"""",""productTypeName"":"""",""setOfBookName"":""" + m_CurrentAcct.sobs.FirstOrDefault().orgName + @""",""userId"":""" + m_operatorId + @""",""orgName"":"""",""materialType"":"""",""isConf"":""N"",""scanBar"":"""",""materialCode"":"""",""saleableEnabledFlag"":"""",""specification"":"""",""customerName"":""""},""page"":{""begin"":" + (Pagestart * PageNumber).ToString() +  @",""length"":" + c.ToString() + @",""isCount"":true},""pageIndex"":" + Pagestart.ToString() + @",""pageSize"":" + c.ToString() + @",""sortField"":"""",""sortOrder"":""""}";
                            }
                            else
                            {
                                body_temp = @"{""paramMap"":{""functionType"":""4"",""materialName"":"""",""salesStatus"":"""",""productTypeName"":"""",""setOfBookName"":""" + m_CurrentAcct.sobs.FirstOrDefault().orgName + @""",""userId"":""" + m_operatorId + @""",""orgName"":"""",""materialType"":"""",""isConf"":""N"",""scanBar"":"""",""materialCode"":"""",""saleableEnabledFlag"":"""",""specification"":"""",""customerName"":""""},""page"":{""begin"":" + (Pagestart * PageNumber).ToString() + @",""length"":" + PageNumber.ToString() + @",""isCount"":true},""pageIndex"":" + Pagestart.ToString() + @",""pageSize"":" + PageNumber.ToString() + @",""sortField"":"""",""sortOrder"":""""}";
                            }
                            var c2 = Act(body_temp);
                            ci.materials.AddRange(c2.materials);
                        }
                    }
                    return ci;
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);

                }
                return null;
            }
            public CRM_ItemDetail GetItemPartList()
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            Robam_Sync.Robam_Sync.wl_jczldr("没能成功登录crm系统!",true);
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            Robam_Sync.Robam_Sync.wl_jczldr("没能成功登录crm系统!", true);
                            return null;
                        }
                    }
                    if (!RegistDefaultRole())
                    {
                        Robam_Sync.Robam_Sync.wl_jczldr("注册角色错误!", true);
                        m_errorString = "注册角色错误!";
                        return null;
                    }
                    Func<string, CRM_ItemDetail> Act = new Func<string, CRM_ItemDetail>((json) => {
                        var client = new RestClient(
                            new HttpClient()
                            {
                                BaseAddress = new Uri(
                            "http://ims.hzrobam.com/robamIMS/baseconfig/materias/com.sie.crm.pub.baseconfig.crmecreference.qeryMaterialsPart.biz.ext"
                            ),
                                Timeout = TimeSpan.FromMinutes(20)
                            }
                            );
                        var request = new RestRequest();
                        request.Method = Method.Post;
                        request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                        request.AddHeader("Accept-Encoding", "gzip, deflate");
                        request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                        request.AddHeader("Cache-Control", "no-cache");
                        request.AddHeader("Connection", "keep-alive");
                        request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                        request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                        request.AddHeader("Host", "ims.hzrobam.com");
                        request.AddHeader("Origin", "http://ims.hzrobam.com");
                        request.AddHeader("Pragma", "no-cache");
                        request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/baseconfig/materias/CrmPartMaterialsMaintain.jsp?functionType=3&good_addBatchBtn=&good_remove=&good_addBtn=&apply_remove=&customer_remove=&btn_save=&btn_edit=&btn_add=&_t=861738");
                        request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 Edg/106.0.1370.52");
                        request.AddHeader("X-Requested-With", "XMLHttpRequest");
                        var body = json;
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        return JObject.Parse(response.Content).ToObject<CRM_ItemDetail>();
                    });
                    int Pagestart = 0;
                    int PageNumber = 30000;
                    var ci = Act(@"{""paramMap"":{""materialCode"":"""",""materialName"":"""",""prodType"":"""",""isEbsImport"":"""",""productModelCode"":"""",""isMarking"":"""",""isMaintServ"":"""",""isConf"":""N"",""isReturn"":"""",""productTypeLine"":"""",""userId"":""" + m_operatorId + @""",""functionType"":""3"",""costEnabledFlag"":""Y""},""page"":{""begin"":" + Pagestart.ToString() + @",""length"":" + PageNumber.ToString() + @",""isCount"":true},""pageIndex"":" + Pagestart.ToString() + @",""pageSize"":" + PageNumber.ToString() + @",""sortField"":"""",""sortOrder"":""""}");
                    //查看多页
                    int Pages = 0;
                    if(ci.page.count > PageNumber)
                    {
                        Pages = (ci.page.count ?? 0) / PageNumber;
                        int c = (ci.page.count ?? 0) % PageNumber;
                        if (c > 0)
                        {
                            Pages++;
                        }
                        for(int i = 1; i < Pages; i ++)
                        {
                            Thread.Sleep(1500);
                            Pagestart = i;
                            string body_temp = "";
                            if(i + 1 == Pages)
                            {
                                body_temp = @"{""paramMap"":{""materialCode"":"""",""materialName"":"""",""prodType"":"""",""isEbsImport"":"""",""productModelCode"":"""",""isMarking"":"""",""isMaintServ"":"""",""isConf"":""N"",""isReturn"":"""",""productTypeLine"":"""",""userId"":""" + m_operatorId + @""",""functionType"":""3"",""costEnabledFlag"":""Y""},""page"":{""begin"":" + (Pagestart * PageNumber).ToString() + @",""length"":" + c.ToString() + @",""isCount"":true},""pageIndex"":" + Pagestart.ToString() + @",""pageSize"":" + c.ToString() + @",""sortField"":"""",""sortOrder"":""""}";
                            }
                            else
                            {
                                body_temp = @"{""paramMap"":{""materialCode"":"""",""materialName"":"""",""prodType"":"""",""isEbsImport"":"""",""productModelCode"":"""",""isMarking"":"""",""isMaintServ"":"""",""isConf"":""N"",""isReturn"":"""",""productTypeLine"":"""",""userId"":""" + m_operatorId + @""",""functionType"":""3"",""costEnabledFlag"":""Y""},""page"":{""begin"":" + (Pagestart * PageNumber) + @",""length"":" + PageNumber.ToString() + @",""isCount"":true},""pageIndex"":" + Pagestart.ToString() + @",""pageSize"":" + PageNumber.ToString() + @",""sortField"":"""",""sortOrder"":""""}";
                            }
                            var c2 = Act(body_temp);
                            ci.materials.AddRange(c2.materials);
                        }
                    }
                    return ci;
                }
                catch(Exception exp)
                {
                    Robam_Sync.Robam_Sync.wl_jczldr("导入发生错误:");
                    Logger.log(exp.Message);
                }
                return null;
            }
            public CRM_ItemDetail GetItemListFromText()
            {
                //string pathName = @"./result/ret/s.txt";
                //FileStream fs = new FileStream(pathName, FileMode.OpenOrCreate);
                //StreamReader wr = null;
                //wr = new StreamReader(fs);
                string txt = Resource.Materials;
                //string ret = wr.ReadToEnd();
                //wr.Close();
                return JObject.Parse(txt).ToObject<CRM_ItemDetail>();
            }
            ///同步价格
            public void GetPriceScheme()
            {
                try
                {
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/price/com.sie.crm.price.crmpricebiz.queryCrmPrices.biz.ext"
                        ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/price/CrmPriceMaintain.jsp?functionType2=4&queryType=inte_Price&_t=230738");
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 Edg/106.0.1370.52");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.PriceManagementDataEntity.CrmPriceV1"",""_expr"":[{""priceName"":"""",""_op"":""like""},{""priceTypeName"":"""",""_op"":""=""},{""orgName"":"""",""_op"":""=""},{""priceType"":"""",""_op"":""<>""},{""orgId"":"""",""_op"":""=""},{""priceType"":""PJ_BRANCH_INTERNAL_PRICE"",""_op"":""in""},{""beginDate"":"""",""_op"":"">=""},{""beginDate"":"""",""_op"":""<=""},{""endDate"":"""",""_op"":"">=""},{""endDate"":"""",""_op"":""<=""},{""companyName"":"""",""_op"":""like""},{""customerName"":"""",""_op"":""like""},{""userId"":""4964588"",""_op"":""=""}],""_orderby"":[{""_sort"":""asc"",""_property"":""priceId""}]},""page"":{""begin"":0,""length"":20,""isCount"":true},""pageIndex"":0,""pageSize"":20,""sortField"":"""",""sortOrder"":""""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                finally
                {

                }
            }
            public CRM_PartsPrice GetPartsPrice()
            {
                try
                {
                    Checklogin();
                    if (!m_login)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (DateTime.Compare(m_loginTime, DateTime.Now) > 30 * 60)
                    {
                        if (!SignIn())
                        {
                            return null;
                        }
                    }
                    if (!RegistDefaultRole())
                    {
                        m_errorString = "注册角色错误!";
                        return null;
                    }

                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                        "http://ims.hzrobam.com/robamIMS/price/com.sie.crm.price.crmpricebiz.queryline.biz.ext"
                        ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Cookie", (m_Token == "" ? "" : m_Token + ""));
                    request.AddHeader("Host", "ims.hzrobam.com");
                    request.AddHeader("Origin", "http://ims.hzrobam.com");
                    request.AddHeader("Referer", "http://ims.hzrobam.com/robamIMS/price/CrmPriceMaintain.jsp?functionType2=6&queryType=Cust_Price&_t=630224");
                    request.AddHeader("UserAgent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.52");
                    request.AddHeader("X-Requested-With", " XMLHttpRequest");
                    var body = @"{""criteria"":{""_entity"":""com.sie.crm.pub.dataset.PriceManagementDataEntity.CrmPriceLineV"",""_expr"":[{""priceId"":1009129,""_op"":""=""}],""_orderby"":[{""_sort"":""asc"",""_property"":""priceLineId""}]},""page"":{""begin"":0,""length"":50000,""isCount"":true},""pageIndex"":0,""pageSize"":50000,""sortField"":"""",""sortOrder"":""""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<CRM_PartsPrice>();
                }
                catch( Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }

            public bool UpdatePartsPriceWithDb(CRM_PartsPrice cr)
            {
                try
                {
                    string sql = "";
                    foreach(var item in cr.crmPriceLineVs)
                    {
                        sql += "update A set A.FROBAM_PURCHASEPRICE = " + item.price + " from T_BD_MATERIAL A Inner join T_BD_MATERIAL_L C ON A.FMATERIALID = C.FMATERIALID WHERE FNUMBER = '" + item.materialCode + "';";
                    }
                    if (Utils.updateLocalDB(sql))
                    {
                        return true;
                    }
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return false;
            }
          
        }
        #endregion CRM
        #region Dis
        public class Robam_Distribution : Robam
        {
            public Robam_Distribution(UserAccount account)
            {
                m_Account = account;
            }
            public override bool SignIn()
            {
                try
                {
                    var client = new RestClient(
                        //new HttpClient(){BaseAddress = new Uri(
                        "http://fx.hzrobam.com/DWGateway/restful/Sys/ILoginService/login"
                        //),Timeout = TimeSpan.FromMinutes(20)}
                        );
                    //client.Timeout = -1;
                    //var request = new RestRequest(Method.POST);
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.Timeout = 600 * 1000;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "login");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.63");
                    var body = @"{""params"":{""userId"":""" + m_Account.Account + @""",""passwordType"":""hash"",""password"":""" + m_Account.FPWD + @"""}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    var jobj = JObject.Parse(response.Content).ToObject<DIS_Signin>();
                    if(jobj?.Response?.Token != null)
                    {
                        m_Token = jobj.Response.Token;
                        return true;
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
                finally
                {

                }
                return false;
            }
            public DIS_Saleorder GetSaleorderList(string startdate, string enddate)
            {
                try
                {
                    DateTime startDate, endDate;
                    if (!DateTime.TryParse(startdate, out startDate))
                    {
                        return null;
                    }
                    if (!DateTime.TryParse(enddate, out endDate))
                    {
                        return null;
                    }
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Order/ISamSalesorderMasterService/getSalesOrderListNew"
                            ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    ///client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "drp_sam_s02_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.63");
                    var body = @"{""param"":{""isaccurate"":""Y"",""conditions"":[{""type"":""datetime"",""field"":""data_created_date"",""data"":[""" + startDate.ToString("yyyy-MM-dd HH:mm") + @""",""" + endDate.ToString("yyyy-MM-dd HH:mm") + @"""],""operator"":""between""}],""is_picture_upload"":[],""action_no"":""btnQry""},""startPage"":1,""pageSize"":50}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    var j1 = JObject.Parse(response.Content);
                    return j1.ToObject<DIS_Saleorder>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                finally
                {

                }
                return null;
            }
            public DIS_SaleorderDetail GetSaleorderDetail(string billno)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Order/ISamSalesorderMasterService/getOrderInfoAllByNo"
                            ),
                                Timeout = TimeSpan.FromMinutes(20)
                            }
                        );

                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.Timeout = 600 * 1000;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "drp_sam_s02_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.63");
                    var body = @"{""param"":{""document_no"":""" + billno + @""",""action_list"":[{""action_no"":""btnEdit""},{""action_no"":""btnEditInvoice""},{""action_no"":""btnGrantGift""},{""action_no"":""btnOtherNote""},{""action_no"":""btnEditSalesDate""},{""action_no"":""btnNumModify""},{""action_no"":""btnEditForExecutor""},{""action_no"":""btnSpecialData""},{""action_no"":""btnEditSNCode""},{""action_no"":""btnUpdateDeposit""}]}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    var j1 = JObject.Parse(response.Content);
                    var r = j1.ToObject<DIS_SaleorderDetail>();
                    return r;
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                finally
                {

                }
                return null;
            }
            public DIS_Channel GetChannelDetail(string number)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Base/IChannelBusinessService/getChannelBussinessDetail"
                            ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "cnm/drp_cnm_s05/drp_cnm_s05_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                    var body = @"{""params"":{""customer_vendor"":""" + number + @""",""operation_organization"":""130103"",""language"":""zh_CN"",""action_list"":[{""action_no"":""btnEdit""},{""action_no"":""btnCopy""},{""action_no"":""btnRejectAudit""}]}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<DIS_Channel>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return null;
            }
            public List<DIS_SaleReport> GetSaleReports(string startdate, string enddate)
            {
                try
                {
                    Checklogin();
                    //传参
                    var fine_digital_signature = GetSaleReportGetFRSignature();
                    if (fine_digital_signature == "")
                    {
                        return null;
                    }
                    var sessionid = GetSaleReportGetSessionID(fine_digital_signature);
                    if (sessionid == "")
                    {
                        return null;
                    }

                    string timestemp = Utils.GetMillisecondsTimeStemp().ToString();
                    string Referer = "http://fx.hzrobam.com/WebReport/decision/view/report?" +
                          "viewlet=Robam/drp_rpm_s01.cpt" +
                          "&op=page" +
                          "&programCode=fr://drp_rpm_s01" +
                          "&fine_digital_signature=" + fine_digital_signature +
                          "&fr_dscurrenttime=" + timestemp +
                          "&token=" + m_Token +
                          "&userId=" + m_Account.Account;
                    string fineMarkId = Guid.NewGuid().ToString().Replace("-", "");
                    //var s = Guid.NewGuid().ToString();
                    List<JObject> data = new List<JObject>();
                    if (GetSaleReportPraseParameter(startdate, enddate, sessionid, Referer, fineMarkId))
                    {
                        var info = GetSaleReportGetResult(sessionid, Referer, fineMarkId, ref data, fine_digital_signature);
                        if (info.totlePage == 0)
                        {
                            //发生错误
                        }
                        else
                        {
                            //循环取得所有结果
                            for (int cp = info.currentPage + 1; cp <= info.totlePage; cp++)
                            {
                                //添加延迟，避免服务器ban ip
                                Thread.Sleep(1000);
                                GetSaleReportGetResult(sessionid, Referer, fineMarkId, ref data, fine_digital_signature, cp);
                            }
                        }
                    }
                    //同单合并
                    //var ret = JsonConvert.SerializeObject(data);

                    var tempList = data.Select(i => i.ToObject<DIS_SaleReport>()).ToList();
                    return tempList;
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public List<DIS_SaleReport> GetSaleReports(string billno)
            {
                try
                {
                    Checklogin();
                    //传参
                    var fine_digital_signature = GetSaleReportGetFRSignature();
                    if (fine_digital_signature == "")
                    {
                        return null;
                    }
                    var sessionid = GetSaleReportGetSessionID(fine_digital_signature);
                    if (sessionid == "")
                    {
                        return null;
                    }

                    string timestemp = Utils.GetMillisecondsTimeStemp().ToString();
                    string Referer = "http://fx.hzrobam.com/WebReport/decision/view/report?" +
                          "viewlet=Robam/drp_rpm_s01.cpt" +
                          "&op=page" +
                          "&programCode=fr://drp_rpm_s01" +
                          "&fine_digital_signature=" + fine_digital_signature +
                          "&fr_dscurrenttime=" + timestemp +
                          "&token=" + m_Token +
                          "&userId=" + m_Account.Account;
                    string fineMarkId = Guid.NewGuid().ToString().Replace("-", "");
                    //var s = Guid.NewGuid().ToString();
                    List<JObject> data = new List<JObject>();
                    if (GetSaleReportPraseParameter(billno, sessionid, Referer, fineMarkId))
                    {
                        var info = GetSaleReportGetResult(sessionid, Referer, fineMarkId, ref data, fine_digital_signature);
                        if (info.totlePage == 0)
                        {
                            //发生错误
                        }
                        else
                        {
                            //循环取得所有结果
                            for (int cp = info.currentPage + 1; cp <= info.totlePage; cp++)
                            {
                                //添加延迟，避免服务器ban ip
                                Thread.Sleep(1000);
                                GetSaleReportGetResult(sessionid, Referer, fineMarkId, ref data, fine_digital_signature, cp);
                            }
                        }
                    }
                    //同单合并
                    //var ret = JsonConvert.SerializeObject(data);

                    var tempList = data.Select(i => i.ToObject<DIS_SaleReport>()).ToList();
                    return tempList;//.FirstOrDefault();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public List<K3Cloud_SaleOrder> GetSaleOrder(List<DIS_SaleReport> billList, KingdeeApi k3cloud)
            {
                List<K3Cloud_SaleOrder> result = new List<K3Cloud_SaleOrder>();
                var slist = k3cloud.GetProductstatusList();
                try
                {
                    Checklogin();
                    var tempList = billList;
                    //取单号
                    var fbillnos = tempList.Select(i => i.订单单号).ToList<string>().Distinct();
                    for(int i = 0;i < fbillnos.Count<string>(); i++)
                    {
                        var header = tempList.Where(item => item.订单单号 == fbillnos.ElementAt(i)).FirstOrDefault();
                        var entity = tempList.Where(item => item.订单单号 == fbillnos.ElementAt(i)).Select(i => new K3Cloud_SaleOrder_Model_FRobam_SaleOrderEntity()
                        {
                            FMaterialID = { FNumber = i.实际销售码 ?? "" },
                            FUnitID = {FNumber = ""},
                            FQty = Convert.ToDecimal(i.数量 ?? "0"),
                            FPrice = Convert.ToDecimal(i.实际价格),
                            FTicketPrice = Convert.ToDecimal(i.票据价格),
                            FStockStatusId = {FNumber = (slist.Where(ii=> i.商品状态.Contains(ii.FName))?.FirstOrDefault()?.FNumber ?? "" ) },
                            FAmount = Convert.ToDecimal(i.金额 ?? "0"),
                            FIsFree = (i.活动方案说明.Contains("赠品") || Convert.ToDecimal(i.票据价格)== 0m ? true:false),
                            FOutStock = { FNumber = i.出库仓库 },
                            //2022-12-3 增加单据项次
                            FItemNo = Convert.ToInt32(i.单据项次),
                            //FStockStatusId = { FNumber = (slist.Where(ii=>i.商品状态.Contains( ii.FName)).FirstOrDefault()?.FNumber??"")}

                        }).ToList<K3Cloud_SaleOrder_Model_FRobam_SaleOrderEntity>();
                        var saleBill = new K3Cloud_SaleOrder() {
                            Model =
                            {
                                FBillNo = header.订单单号,
                                FBillTypeID =
                                {
                                    FNumber=header.单据类型??""
                                },
                                FDate=header.订单日期??"",
                                FTotoleAmountz = 0,
                                FReceiveAmount = 0,
                                FSaler = { FNumber = header.导购员 ?? "" },
                                FShop = { FNumber = header.门店 ?? "" },
                                FChannel = { FNumber = header.渠道商 ?? "" },
                                FCustomer = header.顾客姓名??"",
                                FCustomerAddress = header.地址??"",
                                FCustomerPhone = header.手机号??"",
                                FOrgID = {FNumber = header.所属组织名称 == "沧州"? "CZ":"LF" },
                                FRobam_SaleOrderEntity = entity,
                                FOrgBillNo = header.订单单号,
                                
                            }
                        };
                        result.Add(saleBill);
                    }
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return result;
            }
            public struct importMsg
            {
                public int totlePage { get; set; }
                public int currentPage { get; set; }
            }
            bool GetSaleReportPraseParameter(string startdate, string enddate, string sessionid, string referer, string finemarkid)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            @"http://fx.hzrobam.com/WebReport/decision/view/report?op=fr_dialog&cmd=parameters_d"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Authorization", "Bearer null");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Cookie", "fineMarkId=" + finemarkid);
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", @"http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", referer);
                    request.AddHeader("sessionID", sessionid);
                    //client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27";
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddParameter("__parameters__", "%7B%22P_DATE_RANGE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C_C%22%3A%22%5B4e00%5D%5B7ea7%5D%5B6e20%5D%5B9053%5D%22%2C%22LABEL9_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL3_C_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C%22%3A%22*%22%2C%22P_ACTUAL_MODEL_NO_SELECT%22%3A%22%22%2C%22P_CATEGORY_NO_SELECT%22%3A%22%22%2C%22P_CHANNEL_SELECT%22%3A%2201'%2C'0101'%2C'010101'%2C'010102'%2C'0102'%2C'010201'%2C'0103'%2C'010301'%2C'010302'%2C'0104'%2C'010401'%2C'010402'%2C'0105'%2C'010501'%2C'010502'%2C'0106'%2C'010601'%2C'010602'%2C'010603'%2C'0107'%2C'010701'%2C'010702'%2C'010703'%2C'010704'%2C'010705'%2C'0108'%2C'010801'%2C'010802'%2C'010803'%2C'010804'%2C'0109'%2C'010901'%2C'010902'%2C'010903'%2C'010904'%2C'010905'%2C'010906'%2C'0110'%2C'011001'%2C'011002'%2C'011101'%2C'0112'%2C'011201'%2C'0113'%2C'011301'%2C'0114'%2C'011401'%2C'011402'%2C'0115'%2C'011501'%2C'02'%2C'0201'%2C'020101'%2C'020102'%2C'020103'%2C'0202'%2C'020201'%2C'020202'%2C'020203'%2C'0203'%2C'020301'%2C'020302'%2C'020303'%2C'020304'%2C'0204'%2C'020401'%2C'020402'%2C'0205'%2C'020501'%2C'020502'%2C'0206'%2C'020601'%2C'020602'%2C'03'%2C'0301'%2C'0302'%2C'0303'%2C'0304'%2C'0305'%2C'0306'%2C'0307'%2C'04'%2C'0401'%2C'0402'%2C'0403'%2C'0404'%2C'0405'%2C'0406'%2C'0407'%2C'0408'%2C'0409'%2C'0410'%2C'0411'%2C'05'%2C'0501'%2C'0502'%2C'0503'%2C'0504'%2C'0505'%2C'0506'%2C'0607%22%2C%22LABEL9_C_C_C%22%3A%22%5B53d1%5D%5B9001%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL9_C_C%22%3A%22%5B7ed3%5D%5B7b97%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL6_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B578b%5D%5B53f7%5D%22%2C%22LABEL5_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C%22%3A%22%5B54c1%5D%5B7c7b%5D%22%2C%22LABEL5_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL6_C%22%3A%22%5B603b%5D%5B90e8%5D%5B6e20%5D%5B9053%5D%5B5206%5D%5B7c7b%5D%22%2C%22LABEL7_C%22%3A%22%5B6e20%5D%5B9053%5D%5B5546%5D%22%2C%22LABEL8_C%22%3A%22%5B9500%5D%5B552e%5D%5B95e8%5D%5B5e97%5D%22%2C%22LABEL5_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6027%5D%5B8d28%5D%22%2C%22P_DATE_TYPE_SELECT%22%3A%221%22%2C%22LABEL0_C%22%3A%22%5B65e5%5D%5B671f%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL1_C%22%3A%22%5B5f00%5D%5B59cb%5D%5B65f6%5D%5B95f4%5D%22%2C%22LABEL2_C%22%3A%22%5B7ed3%5D%5B675f%5D%5B65f6%5D%5B95f4%5D%22%2C%22P_DATE_TYPE_SDATE%22%3A%22" + startdate + "%22%2C%22P_DATE_TYPE_EDATE%22%3A%22" + enddate + "%22%2C%22P_DATE_TYPE_SDATETIME%22%3A%2200%3A00%3A00%22%2C%22P_DATE_TYPE_EDATETIME%22%3A%2223%3A59%3A59%22%2C%22P_DISTRIBUTION_STATUS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B59d3%5D%5B540d%5D%22%2C%22LABEL6_C_C_C_C_C_C_C%22%3A%22%5B624b%5D%5B673a%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C%22%3A%22%5B6e20%5D%5B9053%5DSKU%22%2C%22P_CHANNEL_SKU_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C%22%3A%22%5B6d3b%5D%5B52a8%5D%5B65b9%5D%5B6848%5D%22%2C%22P_PROMOTION_PROGRAM_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C%22%3A%22%5B8fd4%5D%5B5229%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C%22%3A%22%5B6263%5D%5B7387%5D%2525%22%2C%22P_DEPOSIT_RATE_NUM%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C%22%3A%22%5B9000%5D%5B8d27%5D%5B72b6%5D%5B6001%5D%22%2C%22P_GOODS_RETURN_STATUS_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bfc%5D%5B8d2d%5D%5B5458%5D%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5b9e%5D%5B9645%5D%5B9500%5D%5B552e%5D%5B7801%5D%22%2C%22P_ACTUAL_SELLING_GOODS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B539f%5D%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_GOODS_TYPE_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B7968%5D%5B636e%5D%5B578b%5D%5B53f7%5D%22%2C%22P_ACTUAL_BILL_MODEL_NO_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5e73%5D%5B5e93%5D%5B5426%5D%22%2C%22P_IS_BALANCE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5de5%5D%5B7a0b%5D%5B9879%5D%5B76ee%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B52a1%5D%5B5458%5D%22%2C%22P_SALESMAN_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6765%5D%5B6e90%5D%22%2C%22P_SOURCE_TYPE_SELECT%22%3A%22%22%2C%22P_DELIVERY_ORGANIZATION_SELECT%22%3A%22130103%22%2C%22LABEL5_C_C_C_C%22%3A%22%5B53d1%5D%5B8d27%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B90e8%5D%5B95e8%5D%22%2C%22P_BELONGED_DEPARTMENT_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B662f%5D%5B5426%5D%5B5957%5D%5B9910%5D%22%2C%22P_PACKAGE_SELECT%22%3A%22%22%2C%22LABEL10%22%3A%22%5B5feb%5D%5B901f%5D%5B67e5%5D%5B8be2%5D%22%2C%22P_QUICK_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B914d%5D%5B9001%5D%5B670d%5D%5B52a1%5D%5B5355%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bf9%5D%5B8d26%5D%5B7801%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B884c%5D%5B53f7%5D%22%2C%22P_LINE_NO_SELECT%22%3A%22%22%2C%22P_BELONGED_TOP_CHANNEL%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5957%5D%5B9910%5D%5B7f16%5D%5B53f7%5D%22%2C%22P_PACKAGE_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C%22%3A%22%5B521b%5D%5B5efa%5D%5B4eba%5D%22%2C%22P_DATA_CREATED_BY_SELECT%22%3A%22%22%2C%22LABEL8_C_C%22%3A%22%5B9500%5D%5B552e%5D%5B7247%5D%5B533a%5D%22%2C%22P_SALES_AREA_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B90ae%5D%5B7bb1%5D%22%2C%22P_CUSTOMER_NO_SELECT%22%3A%22%22%2C%22P_CUSTOMER_NO_SELECT_C%22%3A%22%22%2C%22P_STORE_NO_SELECT%22%3A%22%22%2C%22P_STORE_NO_SELECT_C%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT_C%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT_C%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT_C%22%3A%22%22%2C%22P_MOBILE_NO_SELECT%22%3A%22%22%2C%22P_MOBILE_NO_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_DOCUMENT_NO_SELECT_C%22%3A%22%22%2C%22P_DOCUMENT_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT_C%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT_C%22%3A%22%22%2C%22P_OPERATION_ORGANIZATION_SELECT%22%3A%22%22%2C%22LABEL9_INM_WAREHOUSE%22%3A%22%5B51fa%5D%5B5e93%5D%5B4ed3%5D%5B5e93%5D%22%2C%22P_WAREHOUSE_NO_SELECT%22%3A%22%22%2C%22P_OTHER_WAREHOSE_CHECKBOX%22%3Afalse%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6838%5D%5B7b97%5D%5B5de5%5D%5B8d44%5D%5B6708%5D%5B4efd%5D%22%2C%22P_SALARY_DATE%22%3A%22%22%2C%22LABEL5_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B7ee9%5D%5B5f52%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22P_PERFORMANCE_SELECTION%22%3A%22%22%2C%22P_ZB_CXQD_CHECKBOX%22%3Afalse%2C%22P_DOCUMENT_TYPE_SELECT%22%3A%221'%2C'3'%2C'4%22%2C%22P_RECONCILIATION_NO_SELECT%22%3A%22%22%2C%22P_REBATE_TYPE_SELECT%22%3A%22%22%2C%22P_DOCUMENT_PROPERTY_SELECT%22%3A%22%22%2C%22P_STATUS_CODE_SELECT_D%22%3A%22%22%2C%22P_GOODS_STATUS_SELECT%22%3A%22%22%2C%22P_EMAIL_SELECT%22%3A%22%22%2C%22P_SETTLEMENT_STATUS_SELECT%22%3A%22%22%2C%22P_IS_EXIST%22%3A%22%22%2C%22LABEL9_IS_EXISTS%22%3A%22%5B53a8%5D%5B623f%5D%5B662f%5D%5B5426%5D%5B5df2%5D%5B6709%5D%5B70df%5D%5B673a%5D%5B6216%5D%5B7076%5D%5B5177%5D%22%7D");
                    //request.AddParameter("__parameters__", "%7B%22P_DATE_RANGE_SELECT%22%3A%223%22%2C%22LABEL9_C_C_C_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C_C%22%3A%22%5B4e00%5D%5B7ea7%5D%5B6e20%5D%5B9053%5D%22%2C%22LABEL9_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL3_C_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C%22%3A%22*%22%2C%22P_ACTUAL_MODEL_NO_SELECT%22%3A%22%22%2C%22P_CATEGORY_NO_SELECT%22%3A%22%22%2C%22P_CHANNEL_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C%22%3A%22%5B53d1%5D%5B9001%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL9_C_C%22%3A%22%5B7ed3%5D%5B7b97%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL6_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B578b%5D%5B53f7%5D%22%2C%22LABEL5_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C%22%3A%22%5B54c1%5D%5B7c7b%5D%22%2C%22LABEL5_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL6_C%22%3A%22%5B603b%5D%5B90e8%5D%5B6e20%5D%5B9053%5D%5B5206%5D%5B7c7b%5D%22%2C%22LABEL7_C%22%3A%22%5B6e20%5D%5B9053%5D%5B5546%5D%22%2C%22LABEL8_C%22%3A%22%5B9500%5D%5B552e%5D%5B95e8%5D%5B5e97%5D%22%2C%22LABEL5_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6027%5D%5B8d28%5D%22%2C%22P_DATE_TYPE_SELECT%22%3A%221%22%2C%22LABEL0_C%22%3A%22%5B65e5%5D%5B671f%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL1_C%22%3A%22%5B5f00%5D%5B59cb%5D%5B65f6%5D%5B95f4%5D%22%2C%22LABEL2_C%22%3A%22%5B7ed3%5D%5B675f%5D%5B65f6%5D%5B95f4%5D%22%2C%22P_DATE_TYPE_SDATE%22%3A%222022-09-03%22%2C%22P_DATE_TYPE_EDATE%22%3A%222022-09-06%22%2C%22P_DATE_TYPE_SDATETIME%22%3A%2200%3A00%3A00%22%2C%22P_DATE_TYPE_EDATETIME%22%3A%2223%3A59%3A59%22%2C%22P_DISTRIBUTION_STATUS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B59d3%5D%5B540d%5D%22%2C%22LABEL6_C_C_C_C_C_C_C%22%3A%22%5B624b%5D%5B673a%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C%22%3A%22%5B6e20%5D%5B9053%5DSKU%22%2C%22P_CHANNEL_SKU_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C%22%3A%22%5B6d3b%5D%5B52a8%5D%5B65b9%5D%5B6848%5D%22%2C%22P_PROMOTION_PROGRAM_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C%22%3A%22%5B8fd4%5D%5B5229%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C%22%3A%22%5B6263%5D%5B7387%5D%2525%22%2C%22P_DEPOSIT_RATE_NUM%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C%22%3A%22%5B9000%5D%5B8d27%5D%5B72b6%5D%5B6001%5D%22%2C%22P_GOODS_RETURN_STATUS_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bfc%5D%5B8d2d%5D%5B5458%5D%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5b9e%5D%5B9645%5D%5B9500%5D%5B552e%5D%5B7801%5D%22%2C%22P_ACTUAL_SELLING_GOODS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B539f%5D%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_GOODS_TYPE_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B7968%5D%5B636e%5D%5B578b%5D%5B53f7%5D%22%2C%22P_ACTUAL_BILL_MODEL_NO_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5e73%5D%5B5e93%5D%5B5426%5D%22%2C%22P_IS_BALANCE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5de5%5D%5B7a0b%5D%5B9879%5D%5B76ee%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B52a1%5D%5B5458%5D%22%2C%22P_SALESMAN_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6765%5D%5B6e90%5D%22%2C%22P_SOURCE_TYPE_SELECT%22%3A%22%22%2C%22P_DELIVERY_ORGANIZATION_SELECT%22%3A%22%22%2C%22LABEL5_C_C_C_C%22%3A%22%5B53d1%5D%5B8d27%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B90e8%5D%5B95e8%5D%22%2C%22P_BELONGED_DEPARTMENT_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B662f%5D%5B5426%5D%5B5957%5D%5B9910%5D%22%2C%22P_PACKAGE_SELECT%22%3A%22%22%2C%22LABEL10%22%3A%22%5B5feb%5D%5B901f%5D%5B67e5%5D%5B8be2%5D%22%2C%22P_QUICK_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B914d%5D%5B9001%5D%5B670d%5D%5B52a1%5D%5B5355%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bf9%5D%5B8d26%5D%5B7801%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B884c%5D%5B53f7%5D%22%2C%22P_LINE_NO_SELECT%22%3A%22%22%2C%22P_BELONGED_TOP_CHANNEL%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5957%5D%5B9910%5D%5B7f16%5D%5B53f7%5D%22%2C%22P_PACKAGE_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C%22%3A%22%5B521b%5D%5B5efa%5D%5B4eba%5D%22%2C%22P_DATA_CREATED_BY_SELECT%22%3A%22%22%2C%22LABEL8_C_C%22%3A%22%5B9500%5D%5B552e%5D%5B7247%5D%5B533a%5D%22%2C%22P_SALES_AREA_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B90ae%5D%5B7bb1%5D%22%2C%22P_CUSTOMER_NO_SELECT%22%3A%22%22%2C%22P_CUSTOMER_NO_SELECT_C%22%3A%22%22%2C%22P_STORE_NO_SELECT%22%3A%22%22%2C%22P_STORE_NO_SELECT_C%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT_C%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT_C%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT_C%22%3A%22%22%2C%22P_MOBILE_NO_SELECT%22%3A%22%22%2C%22P_MOBILE_NO_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_DOCUMENT_NO_SELECT_C%22%3A%22%22%2C%22P_DOCUMENT_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT_C%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT_C%22%3A%22%22%2C%22P_OPERATION_ORGANIZATION_SELECT%22%3A%22130103%22%2C%22LABEL9_INM_WAREHOUSE%22%3A%22%5B51fa%5D%5B5e93%5D%5B4ed3%5D%5B5e93%5D%22%2C%22P_WAREHOUSE_NO_SELECT%22%3A%22%22%2C%22P_OTHER_WAREHOSE_CHECKBOX%22%3Afalse%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6838%5D%5B7b97%5D%5B5de5%5D%5B8d44%5D%5B6708%5D%5B4efd%5D%22%2C%22P_SALARY_DATE%22%3A%22%22%2C%22LABEL5_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B7ee9%5D%5B5f52%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22P_PERFORMANCE_SELECTION%22%3A%22%22%2C%22P_ZB_CXQD_CHECKBOX%22%3Afalse%2C%22P_DOCUMENT_TYPE_SELECT%22%3A%221%22%2C%22P_RECONCILIATION_NO_SELECT%22%3A%22%22%2C%22P_REBATE_TYPE_SELECT%22%3A%22%22%2C%22P_DOCUMENT_PROPERTY_SELECT%22%3A%22%22%2C%22P_STATUS_CODE_SELECT_D%22%3A%22Y%22%2C%22P_GOODS_STATUS_SELECT%22%3A%22%22%2C%22P_EMAIL_SELECT%22%3A%22%22%2C%22P_SETTLEMENT_STATUS_SELECT%22%3A%22%22%2C%22P_IS_EXIST%22%3A%22%22%2C%22LABEL9_IS_EXISTS%22%3A%22%5B53a8%5D%5B623f%5D%5B662f%5D%5B5426%5D%5B5df2%5D%5B6709%5D%5B70df%5D%5B673a%5D%5B6216%5D%5B7076%5D%5B5177%5D%22%7D");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    if (JObject.Parse(response.Content).SelectToken("status").Value<string>() == "success")
                    {
                        return true;
                    }
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return false;
            }

            bool GetSaleReportPraseParameter(string billno, string sessionid, string referer, string finemarkid)
            {
                try
                {
                    Checklogin();
                    string startdate = "2010-01-01 00:00:00";
                    string enddate = "2050-01-01 00:00:00";
                    var client = new RestClient(

                    new HttpClient()
                    {
                        BaseAddress = new Uri(
                    @"http://fx.hzrobam.com/WebReport/decision/view/report?op=fr_dialog&cmd=parameters_d"
                    ),
                        Timeout = TimeSpan.FromMinutes(20)
                    }
                    );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Authorization", "Bearer null");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    request.AddHeader("Cookie", "fineMarkId=" + finemarkid);
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", @"http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", referer);
                    request.AddHeader("sessionID", sessionid);
                    //client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27";
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    request.AddParameter("__parameters__", "%7B%22P_DATE_RANGE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C_C%22%3A%22%5B4e00%5D%5B7ea7%5D%5B6e20%5D%5B9053%5D%22%2C%22LABEL9_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL3_C_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C%22%3A%22*%22%2C%22P_ACTUAL_MODEL_NO_SELECT%22%3A%22%22%2C%22P_CATEGORY_NO_SELECT%22%3A%22%22%2C%22P_CHANNEL_SELECT%22%3A%2201'%2C'0101'%2C'010101'%2C'010102'%2C'0102'%2C'010201'%2C'0103'%2C'010301'%2C'010302'%2C'0104'%2C'010401'%2C'010402'%2C'0105'%2C'010501'%2C'010502'%2C'0106'%2C'010601'%2C'010602'%2C'010603'%2C'0107'%2C'010701'%2C'010702'%2C'010703'%2C'010704'%2C'010705'%2C'0108'%2C'010801'%2C'010802'%2C'010803'%2C'010804'%2C'0109'%2C'010901'%2C'010902'%2C'010903'%2C'010904'%2C'010905'%2C'010906'%2C'0110'%2C'011001'%2C'011002'%2C'011101'%2C'0112'%2C'011201'%2C'0113'%2C'011301'%2C'0114'%2C'011401'%2C'011402'%2C'0115'%2C'011501'%2C'02'%2C'0201'%2C'020101'%2C'020102'%2C'020103'%2C'0202'%2C'020201'%2C'020202'%2C'020203'%2C'0203'%2C'020301'%2C'020302'%2C'020303'%2C'020304'%2C'0204'%2C'020401'%2C'020402'%2C'0205'%2C'020501'%2C'020502'%2C'0206'%2C'020601'%2C'020602'%2C'03'%2C'0301'%2C'0302'%2C'0303'%2C'0304'%2C'0305'%2C'0306'%2C'0307'%2C'04'%2C'0401'%2C'0402'%2C'0403'%2C'0404'%2C'0405'%2C'0406'%2C'0407'%2C'0408'%2C'0409'%2C'0410'%2C'0411'%2C'05'%2C'0501'%2C'0502'%2C'0503'%2C'0504'%2C'0505'%2C'0506'%2C'0607%22%2C%22LABEL9_C_C_C%22%3A%22%5B53d1%5D%5B9001%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL9_C_C%22%3A%22%5B7ed3%5D%5B7b97%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL6_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B578b%5D%5B53f7%5D%22%2C%22LABEL5_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C%22%3A%22%5B54c1%5D%5B7c7b%5D%22%2C%22LABEL5_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL6_C%22%3A%22%5B603b%5D%5B90e8%5D%5B6e20%5D%5B9053%5D%5B5206%5D%5B7c7b%5D%22%2C%22LABEL7_C%22%3A%22%5B6e20%5D%5B9053%5D%5B5546%5D%22%2C%22LABEL8_C%22%3A%22%5B9500%5D%5B552e%5D%5B95e8%5D%5B5e97%5D%22%2C%22LABEL5_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6027%5D%5B8d28%5D%22%2C%22P_DATE_TYPE_SELECT%22%3A%221%22%2C%22LABEL0_C%22%3A%22%5B65e5%5D%5B671f%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL1_C%22%3A%22%5B5f00%5D%5B59cb%5D%5B65f6%5D%5B95f4%5D%22%2C%22LABEL2_C%22%3A%22%5B7ed3%5D%5B675f%5D%5B65f6%5D%5B95f4%5D%22%2C%22P_DATE_TYPE_SDATE%22%3A%22" + startdate + "%22%2C%22P_DATE_TYPE_EDATE%22%3A%22" + enddate + "%22%2C%22P_DATE_TYPE_SDATETIME%22%3A%2200%3A00%3A00%22%2C%22P_DATE_TYPE_EDATETIME%22%3A%2223%3A59%3A59%22%2C%22P_DISTRIBUTION_STATUS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B59d3%5D%5B540d%5D%22%2C%22LABEL6_C_C_C_C_C_C_C%22%3A%22%5B624b%5D%5B673a%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C%22%3A%22%5B6e20%5D%5B9053%5DSKU%22%2C%22P_CHANNEL_SKU_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C%22%3A%22%5B6d3b%5D%5B52a8%5D%5B65b9%5D%5B6848%5D%22%2C%22P_PROMOTION_PROGRAM_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C%22%3A%22%5B8fd4%5D%5B5229%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C%22%3A%22%5B6263%5D%5B7387%5D%2525%22%2C%22P_DEPOSIT_RATE_NUM%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C%22%3A%22%5B9000%5D%5B8d27%5D%5B72b6%5D%5B6001%5D%22%2C%22P_GOODS_RETURN_STATUS_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bfc%5D%5B8d2d%5D%5B5458%5D%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5b9e%5D%5B9645%5D%5B9500%5D%5B552e%5D%5B7801%5D%22%2C%22P_ACTUAL_SELLING_GOODS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B539f%5D%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_GOODS_TYPE_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B7968%5D%5B636e%5D%5B578b%5D%5B53f7%5D%22%2C%22P_ACTUAL_BILL_MODEL_NO_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5e73%5D%5B5e93%5D%5B5426%5D%22%2C%22P_IS_BALANCE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5de5%5D%5B7a0b%5D%5B9879%5D%5B76ee%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B52a1%5D%5B5458%5D%22%2C%22P_SALESMAN_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6765%5D%5B6e90%5D%22%2C%22P_SOURCE_TYPE_SELECT%22%3A%22%22%2C%22P_DELIVERY_ORGANIZATION_SELECT%22%3A%22130103%22%2C%22LABEL5_C_C_C_C%22%3A%22%5B53d1%5D%5B8d27%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B90e8%5D%5B95e8%5D%22%2C%22P_BELONGED_DEPARTMENT_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B662f%5D%5B5426%5D%5B5957%5D%5B9910%5D%22%2C%22P_PACKAGE_SELECT%22%3A%22%22%2C%22LABEL10%22%3A%22%5B5feb%5D%5B901f%5D%5B67e5%5D%5B8be2%5D%22%2C%22P_QUICK_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B914d%5D%5B9001%5D%5B670d%5D%5B52a1%5D%5B5355%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bf9%5D%5B8d26%5D%5B7801%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B884c%5D%5B53f7%5D%22%2C%22P_LINE_NO_SELECT%22%3A%22%22%2C%22P_BELONGED_TOP_CHANNEL%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5957%5D%5B9910%5D%5B7f16%5D%5B53f7%5D%22%2C%22P_PACKAGE_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C%22%3A%22%5B521b%5D%5B5efa%5D%5B4eba%5D%22%2C%22P_DATA_CREATED_BY_SELECT%22%3A%22%22%2C%22LABEL8_C_C%22%3A%22%5B9500%5D%5B552e%5D%5B7247%5D%5B533a%5D%22%2C%22P_SALES_AREA_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B90ae%5D%5B7bb1%5D%22%2C%22P_CUSTOMER_NO_SELECT%22%3A%22%22%2C%22P_CUSTOMER_NO_SELECT_C%22%3A%22%22%2C%22P_STORE_NO_SELECT%22%3A%22%22%2C%22P_STORE_NO_SELECT_C%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT_C%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT_C%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT_C%22%3A%22%22%2C%22P_MOBILE_NO_SELECT%22%3A%22%22%2C%22P_MOBILE_NO_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_DOCUMENT_NO_SELECT_C%22%3A%22" 
                        + billno + 
                        "%22%2C%22P_DOCUMENT_NO_SELECT%22%3A%22" + billno + "%22%2C%22P_ORIGINAL_ORDER_NO_SELECT%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT_C%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT_C%22%3A%22%22%2C%22P_OPERATION_ORGANIZATION_SELECT%22%3A%22%22%2C%22LABEL9_INM_WAREHOUSE%22%3A%22%5B51fa%5D%5B5e93%5D%5B4ed3%5D%5B5e93%5D%22%2C%22P_WAREHOUSE_NO_SELECT%22%3A%22%22%2C%22P_OTHER_WAREHOSE_CHECKBOX%22%3Afalse%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6838%5D%5B7b97%5D%5B5de5%5D%5B8d44%5D%5B6708%5D%5B4efd%5D%22%2C%22P_SALARY_DATE%22%3A%22%22%2C%22LABEL5_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B7ee9%5D%5B5f52%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22P_PERFORMANCE_SELECTION%22%3A%22%22%2C%22P_ZB_CXQD_CHECKBOX%22%3Afalse%2C%22P_DOCUMENT_TYPE_SELECT%22%3A%221'%2C'3'%2C'4%22%2C%22P_RECONCILIATION_NO_SELECT%22%3A%22%22%2C%22P_REBATE_TYPE_SELECT%22%3A%22%22%2C%22P_DOCUMENT_PROPERTY_SELECT%22%3A%22%22%2C%22P_STATUS_CODE_SELECT_D%22%3A%22%22%2C%22P_GOODS_STATUS_SELECT%22%3A%22%22%2C%22P_EMAIL_SELECT%22%3A%22%22%2C%22P_SETTLEMENT_STATUS_SELECT%22%3A%22%22%2C%22P_IS_EXIST%22%3A%22%22%2C%22LABEL9_IS_EXISTS%22%3A%22%5B53a8%5D%5B623f%5D%5B662f%5D%5B5426%5D%5B5df2%5D%5B6709%5D%5B70df%5D%5B673a%5D%5B6216%5D%5B7076%5D%5B5177%5D%22%7D");
                    //request.AddParameter("__parameters__", "%7B%22P_DATE_RANGE_SELECT%22%3A%223%22%2C%22LABEL9_C_C_C_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C_C%22%3A%22%5B4e00%5D%5B7ea7%5D%5B6e20%5D%5B9053%5D%22%2C%22LABEL9_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL3_C_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C_C%22%3A%22*%22%2C%22LABEL3_C_C_C_C%22%3A%22*%22%2C%22P_ACTUAL_MODEL_NO_SELECT%22%3A%22%22%2C%22P_CATEGORY_NO_SELECT%22%3A%22%22%2C%22P_CHANNEL_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C%22%3A%22%5B53d1%5D%5B9001%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL9_C_C%22%3A%22%5B7ed3%5D%5B7b97%5D%5B72b6%5D%5B6001%5D%22%2C%22LABEL6_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL6_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B578b%5D%5B53f7%5D%22%2C%22LABEL5_C_C%22%3A%22%5B5546%5D%5B54c1%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C%22%3A%22%5B54c1%5D%5B7c7b%5D%22%2C%22LABEL5_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL6_C%22%3A%22%5B603b%5D%5B90e8%5D%5B6e20%5D%5B9053%5D%5B5206%5D%5B7c7b%5D%22%2C%22LABEL7_C%22%3A%22%5B6e20%5D%5B9053%5D%5B5546%5D%22%2C%22LABEL8_C%22%3A%22%5B9500%5D%5B552e%5D%5B95e8%5D%5B5e97%5D%22%2C%22LABEL5_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6027%5D%5B8d28%5D%22%2C%22P_DATE_TYPE_SELECT%22%3A%221%22%2C%22LABEL0_C%22%3A%22%5B65e5%5D%5B671f%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL1_C%22%3A%22%5B5f00%5D%5B59cb%5D%5B65f6%5D%5B95f4%5D%22%2C%22LABEL2_C%22%3A%22%5B7ed3%5D%5B675f%5D%5B65f6%5D%5B95f4%5D%22%2C%22P_DATE_TYPE_SDATE%22%3A%222022-09-03%22%2C%22P_DATE_TYPE_EDATE%22%3A%222022-09-06%22%2C%22P_DATE_TYPE_SDATETIME%22%3A%2200%3A00%3A00%22%2C%22P_DATE_TYPE_EDATETIME%22%3A%2223%3A59%3A59%22%2C%22P_DISTRIBUTION_STATUS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B59d3%5D%5B540d%5D%22%2C%22LABEL6_C_C_C_C_C_C_C%22%3A%22%5B624b%5D%5B673a%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C%22%3A%22%5B6e20%5D%5B9053%5DSKU%22%2C%22P_CHANNEL_SKU_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C%22%3A%22%5B6d3b%5D%5B52a8%5D%5B65b9%5D%5B6848%5D%22%2C%22P_PROMOTION_PROGRAM_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C%22%3A%22%5B8fd4%5D%5B5229%5D%5B7c7b%5D%5B578b%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C%22%3A%22%5B6263%5D%5B7387%5D%2525%22%2C%22P_DEPOSIT_RATE_NUM%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C%22%3A%22%5B9000%5D%5B8d27%5D%5B72b6%5D%5B6001%5D%22%2C%22P_GOODS_RETURN_STATUS_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bfc%5D%5B8d2d%5D%5B5458%5D%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5b9e%5D%5B9645%5D%5B9500%5D%5B552e%5D%5B7801%5D%22%2C%22P_ACTUAL_SELLING_GOODS_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B539f%5D%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_GOODS_TYPE_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B7968%5D%5B636e%5D%5B578b%5D%5B53f7%5D%22%2C%22P_ACTUAL_BILL_MODEL_NO_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5e73%5D%5B5e93%5D%5B5426%5D%22%2C%22P_IS_BALANCE_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5de5%5D%5B7a0b%5D%5B9879%5D%5B76ee%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B52a1%5D%5B5458%5D%22%2C%22P_SALESMAN_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5355%5D%5B636e%5D%5B6765%5D%5B6e90%5D%22%2C%22P_SOURCE_TYPE_SELECT%22%3A%22%22%2C%22P_DELIVERY_ORGANIZATION_SELECT%22%3A%22%22%2C%22LABEL5_C_C_C_C%22%3A%22%5B53d1%5D%5B8d27%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6240%5D%5B5c5e%5D%5B90e8%5D%5B95e8%5D%22%2C%22P_BELONGED_DEPARTMENT_SELECT%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B662f%5D%5B5426%5D%5B5957%5D%5B9910%5D%22%2C%22P_PACKAGE_SELECT%22%3A%22%22%2C%22LABEL10%22%3A%22%5B5feb%5D%5B901f%5D%5B67e5%5D%5B8be2%5D%22%2C%22P_QUICK_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B914d%5D%5B9001%5D%5B670d%5D%5B52a1%5D%5B5355%5D%5B53f7%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5bf9%5D%5B8d26%5D%5B7801%5D%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B884c%5D%5B53f7%5D%22%2C%22P_LINE_NO_SELECT%22%3A%22%22%2C%22P_BELONGED_TOP_CHANNEL%22%3A%22%22%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B5957%5D%5B9910%5D%5B7f16%5D%5B53f7%5D%22%2C%22P_PACKAGE_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C%22%3A%22%5B521b%5D%5B5efa%5D%5B4eba%5D%22%2C%22P_DATA_CREATED_BY_SELECT%22%3A%22%22%2C%22LABEL8_C_C%22%3A%22%5B9500%5D%5B552e%5D%5B7247%5D%5B533a%5D%22%2C%22P_SALES_AREA_SELECT%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B987e%5D%5B5ba2%5D%5B90ae%5D%5B7bb1%5D%22%2C%22P_CUSTOMER_NO_SELECT%22%3A%22%22%2C%22P_CUSTOMER_NO_SELECT_C%22%3A%22%22%2C%22P_STORE_NO_SELECT%22%3A%22%22%2C%22P_STORE_NO_SELECT_C%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT%22%3A%22%22%2C%22P_GUIDE_PURCHASER_SELECT_C%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT%22%3A%22%22%2C%22P_ENGINEERING_PROJECT_SELECT_C%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT%22%3A%22%22%2C%22P_CLIENT_NAME_SELECT_C%22%3A%22%22%2C%22P_MOBILE_NO_SELECT%22%3A%22%22%2C%22P_MOBILE_NO_SELECT_C%22%3A%22%22%2C%22LABEL6_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B8ba2%5D%5B5355%5D%5B5355%5D%5B53f7%5D%22%2C%22P_DOCUMENT_NO_SELECT_C%22%3A%22%22%2C%22P_DOCUMENT_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT%22%3A%22%22%2C%22P_ORIGINAL_ORDER_NO_SELECT_C%22%3A%22%22%2C%22P_DISTRIBUTION_SERVICE_NO_SELECT_C%22%3A%22%22%2C%22P_OPERATION_ORGANIZATION_SELECT%22%3A%22130103%22%2C%22LABEL9_INM_WAREHOUSE%22%3A%22%5B51fa%5D%5B5e93%5D%5B4ed3%5D%5B5e93%5D%22%2C%22P_WAREHOUSE_NO_SELECT%22%3A%22%22%2C%22P_OTHER_WAREHOSE_CHECKBOX%22%3Afalse%2C%22LABEL9_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C_C%22%3A%22%5B6838%5D%5B7b97%5D%5B5de5%5D%5B8d44%5D%5B6708%5D%5B4efd%5D%22%2C%22P_SALARY_DATE%22%3A%22%22%2C%22LABEL5_C_C_C_C_C%22%3A%22%5B4e1a%5D%5B7ee9%5D%5B5f52%5D%5B5c5e%5D%5B7ec4%5D%5B7ec7%5D%22%2C%22P_PERFORMANCE_SELECTION%22%3A%22%22%2C%22P_ZB_CXQD_CHECKBOX%22%3Afalse%2C%22P_DOCUMENT_TYPE_SELECT%22%3A%221%22%2C%22P_RECONCILIATION_NO_SELECT%22%3A%22%22%2C%22P_REBATE_TYPE_SELECT%22%3A%22%22%2C%22P_DOCUMENT_PROPERTY_SELECT%22%3A%22%22%2C%22P_STATUS_CODE_SELECT_D%22%3A%22Y%22%2C%22P_GOODS_STATUS_SELECT%22%3A%22%22%2C%22P_EMAIL_SELECT%22%3A%22%22%2C%22P_SETTLEMENT_STATUS_SELECT%22%3A%22%22%2C%22P_IS_EXIST%22%3A%22%22%2C%22LABEL9_IS_EXISTS%22%3A%22%5B53a8%5D%5B623f%5D%5B662f%5D%5B5426%5D%5B5df2%5D%5B6709%5D%5B70df%5D%5B673a%5D%5B6216%5D%5B7076%5D%5B5177%5D%22%7D");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    if (JObject.Parse(response.Content).SelectToken("status").Value<string>() == "success")
                    {
                        return true;
                    }
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return false;
            }
            importMsg GetSaleReportGetResult(string sessionid, string referer, string fineMarkId, ref List<JObject> list, string fine_digital_signature, int page = 1)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                       new HttpClient()
                       {
                           BaseAddress = new Uri(
                        @"http://fx.hzrobam.com/WebReport/decision/view/report?_=" + Utils.GetMillisecondsTimeStemp().ToString() + "&__boxModel__=true&op=page_content&pn=" + page.ToString() + "&__webpage__=true&_paperWidth=1106&_paperHeight=267&__fit__=false"
                        ), Timeout = TimeSpan.FromMinutes(20) }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.Timeout = 60 * 1000;
                    request.AddHeader("Accept", "text/html,*/*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Authorization", "Bearer null");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", "fineMarkId=" + fineMarkId);
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", referer);
                    request.AddHeader("sessionID", sessionid);
                    //client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27";
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    //获取页码
                    int tp = 0, cp = 0;
                    string s = response.Content.Substring(0, 200).Replace(" ", "");
                    int _p1 = s.IndexOf("FR._p.currentPageIndex=");
                    int _p2 = s.IndexOf("FR._p.reportTotalPage=");
                    string stp = "", scp = "";
                    for (int i = _p1 + ("FR._p.currentPageIndex=").Length; i < s.Length; i++)
                    {
                        if (s[i] != ';')
                        {
                            scp += s[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = _p2 + ("FR._p.reportTotalPage=").Length; i < s.Length; i++)
                    {
                        if (s[i] != ';')
                        {
                            stp += s[i];
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (int.TryParse(stp, out tp))
                    {

                    }
                    if (int.TryParse(scp, out cp))
                    {

                    }
                    GetSaleReportAnalysisData(response.Content, ref list, sessionid, fineMarkId, fine_digital_signature);
                    return new importMsg() { totlePage = tp, currentPage = cp };

                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return new importMsg();
            }
            string translateword(string s)
            {
                Regex regex = new Regex(@"&#[0-9]*;");
                Regex numberRegex = new Regex(@"[0-9]{1,3}");
                string ret = s;
                Match r = regex.Match(s);
                while (r.Success)
                {
                    var rr = numberRegex.Match(r.Value);
                    int d = Convert.ToInt32(rr.Value);

                    ret = ret.Replace(r.Value, Convert.ToString((char)d));
                    r = regex.Match(ret);
                }
                return ret;
            }
            public void GetSaleReportAnalysisData(string txt, ref List<JObject> list, string session, string fineMarkId, string fine_digital_signature)
            {

                try
                {
                    txt = txt.Replace("{\"html\":\"", "");
                    txt = txt.Replace("\",\"watermark\":{},\"sheets\":[{\"id\":\"sheet20\",\"lazyload\":true,\"closable\":true,\"title\":\"sheet2\"}],\"copyright\":{\"templateCopyright\":\"\",\"url\":\"\"}}", "");
                    List<List<string>> data = new List<List<string>>();
                    List<List<JObject>> jList = new List<List<JObject>>();
                    //var html = JObject.Parse(txt).SelectToken("html").ToString();
                    HtmlDocument doc = new HtmlDocument();

                    doc.LoadHtml(txt);
                    //每表
                    foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//tbody"))
                    {
                        Console.WriteLine("Found: " + table.Id);
                        var c1 = table.SelectNodes("tr").Count;
                        int index = 0;
                        //每行
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            var rowdata = new List<string>();
                            Console.WriteLine("row");
                            //Console.WriteLine(row.InnerHtml);
                            System.Diagnostics.Trace.WriteLine(row.InnerHtml);
                            List<JObject> jj = new List<JObject>();
                            //每单元格
                            foreach (HtmlNode cell in row.SelectNodes("td"))
                            {
                                /*
                                 * 如果单元格是条码查看
                                 * 此处不再查询出库条码，因查询速度太快被服务器Ban掉Ip
                                 */
                                //if (cell.InnerHtml.Contains("条码串查"))
                                //{
                                //    if (cell.InnerHtml.Contains("span"))
                                //    {
                                //        int? cc = cell.SelectNodes("span")?.Count;
                                //        if (cc != null && cc > 0)
                                //        {
                                //            foreach (HtmlNode sp in cell.SelectNodes("span"))
                                //            {
                                //                if (sp.InnerText == "条码串查" && index == 0)
                                //                {
                                //                    try
                                //                    {
                                //                        string ret = sp.GetAttributeValue("onclick", "");
                                //                        string r1 = translateword(ret).Trim().Replace(" ", "");
                                //                        int startp = r1.IndexOf("FR.doHyperlinkByGet4Reportlet");
                                //                        int endp = r1.IndexOf(")},this,as)");
                                //                        string json = r1.Substring(startp + ("FR.doHyperlinkByGet4Reportlet").Length + 1, endp - startp - ("FR.doHyperlinkByGet4Reportlet").Length - 1).Replace("\\", "");
                                //                        var jobj = JObject.Parse(json).ToObject<K3Cloud_Common.K3Cloud_View_Qrcode>();
                                //                        string ts = Utils.GetMillisecondsTimeStemp().ToString();
                                //                        var subsession = GetSaleReportGetQrcodeGetSessionID(jobj, ts, fine_digital_signature);
                                //                        if (subsession != "")
                                //                        {
                                //                            GetSaleReportGetQrcodePraseParameter(jobj, subsession, fineMarkId, ts, fine_digital_signature);
                                //                            jj = GetSaleReportGetQrcode(jobj, subsession, fineMarkId, ts, fine_digital_signature);
                                //                            if (jj.Count > 0)
                                //                            {
                                //                            }
                                //                        }
                                //                        break;
                                //                    }
                                //                    catch (Exception exp)
                                //                    {
                                //                        Logger.DebugLog2(exp.Message);
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}

                                rowdata.Add(cell.InnerText);
                                Console.WriteLine("cell: " + cell.InnerText);
                            }
                            data.Add(rowdata);
                            jList.Add(jj);
                        }
                    }

                    //var columns = String.Join(",", from c in data[3] select c);
                    //当前确认 表的第4行(index 3) 为标题 

                    for (int i = 4; i < data.Count; i++)
                    {
                        JObject jobj = new JObject();
                        for (int itemindex = 0; itemindex < data[i].Count; itemindex++)
                        {
                            if (data[i].Count > data[3].Count)
                            {
                                return;
                            }
                            string key = data[3][itemindex].ToString();
                            int c = 1;
                            while (jobj[key] != null)
                            {
                                key += c.ToString();
                                c++;
                            }
                            jobj.Add(key, data[i][itemindex]);
                        }
                        //jobj.Add("Qrcode_KH", JsonConvert.SerializeObject( jList[i]));
                        list.Add(jobj);
                    }



                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }

                return;
            }
            string GetSaleReportGetFRSignature()
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(new HttpClient() { BaseAddress = new Uri("http://fx.hzrobam.com/DWGateway/restful/FR/IReportService/getFRSignature"), Timeout = TimeSpan.FromMinutes(20) });
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "report");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");
                    var body = @"{""userId"":""" + m_Account.Account + @""",""targetReportletPath"":""Robam/drp_rpm_s01.cpt""}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).SelectToken("response.['fine_digital_signature']").Value<string>();
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return "";
            }
            string GetSaleReportGetSessionID(string fine_digital_signature)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(new HttpClient()
                    {
                        BaseAddress = new Uri(
                            "http://fx.hzrobam.com/WebReport/decision/view/report?viewlet=Robam/drp_rpm_s01.cpt" +
                            "&op=page" +
                            "&programCode=fr://drp_rpm_s01" +
                            "&fine_digital_signature=" + fine_digital_signature + //"eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIxMzAxMDMxMDAyIiwic3ViIjoiUm9iYW0vZHJwX3JwbV9zMDEuY3B0IiwiZXhwIjoxNjYyNDQ5NTQxLCJpYXQiOjE2NjI0NDU5NDEsImp0aSI6IiJ9.I1d-iEKmE0lBYRFktFttTUm_Qcxz0u-C_VHBBTjphpg" +
                            "&fr_dscurrenttime=" + Utils.GetMillisecondsTimeStemp().ToString() +
                            "&token=" + m_Token + //"eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ7XCJwcm9maWxlXCI6XCJ7XFxcIk9yZ0lkXFxcIjpcXFwiMTMwMTAzXFxcIixcXFwidXNlcl90eXBlXFxcIjpcXFwiM1xcXCIsXFxcIlVzZXJOYW1lXFxcIjpcXFwi5p2O546J6IOcXFxcIixcXFwicm9sZV9saXN0XFxcIjpcXFwie3JvbGVfbmFtZVxcXFx1MDAzZOS4gOiIrOWRmOW3pSwgcm9sZV9ub1xcXFx1MDAzZDAwMDF9XFxcIixcXFwicHJpbWVyS2V5XFxcIjpcXFwiMTMwMTAzMTAwMlxcXCIsXFxcIlVzZXJJZFxcXCI6XFxcIjEzMDEwMzEwMDJcXFwiLFxcXCJEZXB0VXJpXFxcIjpcXFwiXFxcIixcXFwiT3JnTmFtZVxcXCI6XFxcIuayp-W3nlxcXCIsXFxcIkRlcHROYW1lXFxcIjpcXFwi6LSi5Yqh6YOoXFxcIixcXFwiT3JnVXJpXFxcIjpcXFwiXFxcIixcXFwiRGVwdElkXFxcIjpcXFwiMTMwMTAzMDJcXFwifVwiLFwiaXNzdWVkQXRcIjoxNjYyNDQ0NTE0ODIzLFwibG9naW5Qb2xpY3lcIjpcIkFMTE9XX01VTFRJUExFX0xPR0lOXCJ9IiwiZXhwIjoxNjYyNTMwOTE0LCJpYXQiOjE2NjI0NDQ1MTQsImp0aSI6IjEzMDEwMzEwMDIifQ.xfIXRvppRJ-QDwikeylH_EpXLNpJd03nincFXJr1dtNdhQks_-R6DXSfyzXl3pVNA2RXeGOBO2CbJwr21Umung" +
                            "&userId=" + m_Account.Account),
                        Timeout = TimeSpan.FromMinutes(20)
                    });

                    var request = new RestRequest();
                    request.Method = Method.Get;
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    WebClient wc = new WebClient();

                    string strToFind = "this.currentSessionID = '";
                    int startp = response.Content.IndexOf(strToFind);
                    string l = "";
                    if (startp != -1)
                    {
                        for (int i = startp + strToFind.Length; i <= response.Content.Length; i++)
                        {
                            if (response.Content[i] == '\'')
                            {
                                break;
                            }
                            else
                            {
                                l += response.Content[i];
                            }
                        }
                    }
                    return l;
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return "";
            }
            List<JObject> GetSaleReportGetQrcode(K3Cloud_Common.K3Cloud_View_Qrcode ins, string session, string fineMarkId, string timestemp, string fine_digital_signature)
            {
                List<List<string>> dataArray = new List<List<string>>();
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                    "http://fx.hzrobam.com/WebReport/decision/view/report?_=" +
                    Utils.GetMillisecondsTimeStemp().ToString() +
                    @"&__boxModel__=true&op=page_content&pn=1&__webpage__=true&_paperWidth=514&_paperHeight=613&__fit__=false"
                    ),
                                                Timeout = TimeSpan.FromMinutes(20)
                                            }
                    );
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "text/html, */*; q=0.01");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Authorization", "Bearer null");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", "fineMarkId=" + fineMarkId);
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");

                    request.AddHeader("Referer", @"http://fx.hzrobam.com" +
                        //"/WebReport/decision/view/report?viewlet=%252FRobam%252Fdrp_rpm_s01_sn.cpt" +
                        ins.url +
                        @"&__parameters__=%257B%2522__pi__%2522%253Atrue%252C%2522detailURL%2522%253A%2522http%253A%252F%252Fdev-fx.hzrobam.com%252F%2523%252Ffenxiao%252F%2522%252C%2522urlPath%2522%253A%2522http%253A%252F%252F172.18.8.36%252FDWGateway%252Frestful%252F%2522%252C%2522testUser%2522%253A%2522" +
                        //"administrator" +
                        ins.para.testUser +
                        @"%2522%252C%2522P_ORDER_NO_SELECT%2522%253A%2522" +
                        //"130103S99-2209040005" +
                        ins.para.P_ORDER_NO_SELECT +
                        @"%2522%252C%2522testPassword%2522%253A%2522fxrobam%2522%252C%2522ss%2522%253A%2522%25E9%2594%2580%25E5%2594%25AE%25E5%2587%25BA%25E5%25BA%2593%2522%252C%2522fine_hyperlink%2522%253A%2522" +
                        //"43b9d3e7-78c3-49e0-a553-6fd2a93db276" +
                        ins.para.fine_hyperlink +
                        @"%2522%257D" +
                        @"&_=" + timestemp);
                    request.AddHeader("sessionID", session);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    HtmlDocument doc = new HtmlDocument();

                    doc.LoadHtml(response.Content);

                    foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//tbody"))
                    {
                        Console.WriteLine("Found: " + table.Id);
                        var c1 = table.SelectNodes("tr").Count;
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            var rowdata = new List<string>();
                            Console.WriteLine("row");
                            foreach (HtmlNode cell in row.SelectNodes("td"))
                            {
                                if(cell.InnerText == "")
                                {
                                    break;
                                }
                                rowdata.Add(cell.InnerText);
                                Console.WriteLine("cell: " + cell.InnerText);
                            }
                            if(rowdata.Count != 0)
                            {
                                dataArray.Add(rowdata);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                var retlist = new List<JObject>();
                if (dataArray.Count >1)
                {
                    var jobj = new JObject();
                    for(int i = 1; i< dataArray.Count; i++)
                    {
                        for(int ic = 0; ic < dataArray[i].Count; ic++)
                        {
                            jobj.Add(dataArray[0][ic], dataArray[i][ic]);
                        }
                    }
                    retlist.Add(jobj);
                }
                else
                {

                }
                return retlist;
            }
            void GetSaleReportGetQrcodePraseParameter(K3Cloud_Common.K3Cloud_View_Qrcode ins, string session, string fineMarkId, string timestemp, string fine_digital_signature)
            {
                try
                {
                    Checklogin();
                    string url = "http://fx.hzrobam.com" +
                        //"/WebReport/decision/view/report?viewlet=%252FRobam%252Fdrp_rpm_s01_sn.cpt" +
                        ins.url +
                        "&__parameters__=%257B%2522__pi__%2522%253Atrue%252C%2522detailURL%2522%253A%2522http%253A%252F%252Fdev-fx.hzrobam.com%252F%2523%252Ffenxiao%252F%2522%252C%2522urlPath%2522%253A%2522http%253A%252F%252F172.18.8.36%252FDWGateway%252Frestful%252F%2522%252C%2522testUser%2522%253A%2522" +
                        //"administrator" +
                        ins.para.testUser +
                        "%2522%252C%2522P_ORDER_NO_SELECT%2522%253A%2522" +
                        //"130103S99-2209040004" +
                        ins.para.P_ORDER_NO_SELECT +
                        "%2522%252C%2522testPassword%2522%253A%2522fxrobam%2522%252C%2522ss%2522%253A%2522%25E9%2594%2580%25E5%2594%25AE%25E5%2587%25BA%25E5%25BA%2593%2522%252C%2522fine_hyperlink%2522%253A%2522" +
                        //"15e0eb98-ae8f-4682-89e3-27f7eadc4942" +
                        ins.para.fine_hyperlink +
                        "%2522%257D" +
                        "&_=" + timestemp;
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            url
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    //client.Timeout = -1;
                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Cookie", "fineMarkId=" + fineMarkId);
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/WebReport/decision/view/report?viewlet=Robam/drp_rpm_s01.cpt" +
                        "&op=page" +
                        "&programCode=fr://drp_rpm_s01" +
                        "&fine_digital_signature=" + fine_digital_signature + //eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIxMzAxMDMxMDAyIiwic3ViIjoiUm9iYW0vZHJwX3JwbV9zMDEuY3B0IiwiZXhwIjoxNjYyNTMyMzI4LCJpYXQiOjE2NjI1Mjg3MjgsImp0aSI6IiJ9.cCos6MBsqFx1W4stAciWLfxGUiB4rkl7rVp6g67BLC4
                        "&fr_dscurrenttime=" + timestemp +
                        "&token=" + m_Token +
                        "&userId=" + m_Account.Account);
                    request.AddHeader("Upgrade-Insecure-Requests", " 1");
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
            }
            string GetSaleReportGetQrcodeGetSessionID(K3Cloud_Common.K3Cloud_View_Qrcode ins, string timestemp, string fine_digital_signature)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient
                    (
                            new HttpClient()
                            {
                                BaseAddress = new Uri(
                            //"http://fx.hzrobam.com/WebReport/decision/view/report?viewlet=Robam/drp_rpm_s01.cpt" +
                            //"&op=page" +
                            //"&programCode=fr://drp_rpm_s01" +
                            //"&fine_digital_signature=" + fine_digital_signature + //"eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIxMzAxMDMxMDAyIiwic3ViIjoiUm9iYW0vZHJwX3JwbV9zMDEuY3B0IiwiZXhwIjoxNjYyNDQ5NTQxLCJpYXQiOjE2NjI0NDU5NDEsImp0aSI6IiJ9.I1d-iEKmE0lBYRFktFttTUm_Qcxz0u-C_VHBBTjphpg" +
                            //"&fr_dscurrenttime=" + Utils.GetMillisecondsTimeStemp().ToString() +
                            //"&token=" + m_Token + //"eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ7XCJwcm9maWxlXCI6XCJ7XFxcIk9yZ0lkXFxcIjpcXFwiMTMwMTAzXFxcIixcXFwidXNlcl90eXBlXFxcIjpcXFwiM1xcXCIsXFxcIlVzZXJOYW1lXFxcIjpcXFwi5p2O546J6IOcXFxcIixcXFwicm9sZV9saXN0XFxcIjpcXFwie3JvbGVfbmFtZVxcXFx1MDAzZOS4gOiIrOWRmOW3pSwgcm9sZV9ub1xcXFx1MDAzZDAwMDF9XFxcIixcXFwicHJpbWVyS2V5XFxcIjpcXFwiMTMwMTAzMTAwMlxcXCIsXFxcIlVzZXJJZFxcXCI6XFxcIjEzMDEwMzEwMDJcXFwiLFxcXCJEZXB0VXJpXFxcIjpcXFwiXFxcIixcXFwiT3JnTmFtZVxcXCI6XFxcIuayp-W3nlxcXCIsXFxcIkRlcHROYW1lXFxcIjpcXFwi6LSi5Yqh6YOoXFxcIixcXFwiT3JnVXJpXFxcIjpcXFwiXFxcIixcXFwiRGVwdElkXFxcIjpcXFwiMTMwMTAzMDJcXFwifVwiLFwiaXNzdWVkQXRcIjoxNjYyNDQ0NTE0ODIzLFwibG9naW5Qb2xpY3lcIjpcIkFMTE9XX01VTFRJUExFX0xPR0lOXCJ9IiwiZXhwIjoxNjYyNTMwOTE0LCJpYXQiOjE2NjI0NDQ1MTQsImp0aSI6IjEzMDEwMzEwMDIifQ.xfIXRvppRJ-QDwikeylH_EpXLNpJd03nincFXJr1dtNdhQks_-R6DXSfyzXl3pVNA2RXeGOBO2CbJwr21Umung" +
                            //"&userId=" + m_Account.Account
                            "http://fx.hzrobam.com" +
                            //"/WebReport/decision/view/report?viewlet=%252FRobam%252Fdrp_rpm_s01_sn.cpt" +
                            ins.url +
                            "&__parameters__=%257B%2522__pi__%2522%253Atrue%252C%2522detailURL%2522%253A%2522http%253A%252F%252Fdev-fx.hzrobam.com%252F%2523%252Ffenxiao%252F%2522%252C%2522urlPath%2522%253A%2522http%253A%252F%252F172.18.8.36%252FDWGateway%252Frestful%252F%2522%252C%2522testUser%2522%253A%2522" +
                            //"administrator" +
                            ins.para.testUser +
                            "%2522%252C%2522P_ORDER_NO_SELECT%2522%253A%2522" +
                            //"130103S99-2209040005" +
                            ins.para.P_ORDER_NO_SELECT +
                            "%2522%252C%2522testPassword%2522%253A%2522fxrobam%2522%252C%2522ss%2522%253A%2522%25E9%2594%2580%25E5%2594%25AE%25E5%2587%25BA%25E5%25BA%2593%2522%252C%2522fine_hyperlink%2522%253A%2522" +
                            //"0b02898c-f79e-4d39-96b6-9d0b5a27d7b9" +
                            ins.para.fine_hyperlink +
                            "%2522%257D" +
                            "&_=" + Utils.GetMillisecondsTimeStemp().ToString()
                            ),
                                Timeout = TimeSpan.FromMinutes(20)
                            }
                      );

                    var request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    //request.AddHeader("Cookie", "fineMarkId=a2e9ea8139e50e62437708b824134084");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Referer", "http://fx.hzrobam.com" +
                        "/WebReport/decision/view/report?viewlet=Robam/drp_rpm_s01.cpt" +
                        "&op=page" +
                        "&programCode=fr://drp_rpm_s01" +
                        "&fine_digital_signature=" + fine_digital_signature +
                        "&fr_dscurrenttime=" + timestemp +
                        "&token=" + m_Token +
                        "&userId=" + m_Account.FAccount);
                    request.AddHeader("Upgrade-Insecure-Requests", " 1");
                    request.AddHeader("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    WebClient wc = new WebClient();

                    string strToFind = "this.currentSessionID = '";
                    int startp = response.Content.IndexOf(strToFind);
                    string l = "";
                    if (startp != -1)
                    {
                        for (int i = startp + strToFind.Length; i <= response.Content.Length; i++)
                        {
                            if (response.Content[i] == '\'')
                            {
                                break;
                            }
                            else
                            {
                                l += response.Content[i];
                            }
                        }
                    }
                    return l;
                }
                catch (Exception exp)
                {
                    Logger.DebugLog2(exp.Message);
                }
                return "";
            }
            protected override void Checklogin()
            {
                if (!m_login || (DateTime.Now - m_loginTime) > TimeSpan.FromMinutes(30))
                {
                    if (SignIn())
                    {
                        m_login = true;
                    }
                }
            }
            public DIS_Shop GetShops()
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient(){BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Base/IOrganizationSiteBasedService/getOrganizationBasedList"
                            ),Timeout = TimeSpan.FromMinutes(20)}
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "drp_bas_m38_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.26");
                    var body = @"{""param"":{""isaccurate"":""Y"",""conditions"":[{""type"":""fixed_text"",""field"":""status_code"",""data"":[""Y""]}],""action_no"":""btnQuery""},""startPage"":1,""pageSize"":1000}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<DIS_Shop>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public DIS_ShopDetail GetShopDetail(string id)
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Base/IOrganizationSiteBasedService/getOrganizationBasedDetail"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "drp_bas_m38_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.42");
                    var body = @"{""param"":{""organization"":""" + id + @""",""action_list"":[{""action_no"":""btnEdit""}]}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<DIS_ShopDetail>();
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public DIS_Saler GetSalers()
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Base/IEmployeeGuideService/getEmployeeGuideList"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "bas/drp_bas_s18/drp_bas_s18_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.42");
                    var body = @"{""param"":{""isaccurate"":""Y"",""conditions"":[],""action_no"":""btnQuery""},""pageSize"":10,""startPage"":1}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<DIS_Saler>();
                }
                catch (Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
            public DIS_SecondChannel GetSecondChannelType()
            {
                try
                {
                    Checklogin();
                    var client = new RestClient(
                        new HttpClient()
                        {
                            BaseAddress = new Uri(
                            "http://fx.hzrobam.com/DWGateway/restful/Base/ISccCodeServrce/getSccCode"
                            ),
                            Timeout = TimeSpan.FromMinutes(20)
                        }
                        );
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                    request.AddHeader("Host", "fx.hzrobam.com");
                    request.AddHeader("Origin", "http://fx.hzrobam.com");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Program-Code", "drp_bas_m38_s01");
                    request.AddHeader("Referer", "http://fx.hzrobam.com/");
                    request.AddHeader("token", m_Token);
                    request.AddHeader("UserAgent","Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.42");
                    var body = @"{""param"":{""language"":""zh_CN"",""option_type_no"":""10""}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    return JObject.Parse(response.Content).ToObject<DIS_SecondChannel>();
                }
                catch(Exception exp)
                {
                    Logger.log(exp.Message);
                }
                return null;
            }
        }
        #endregion Dis
    }
}
