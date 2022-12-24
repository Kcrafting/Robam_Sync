#if __ZJF_COMM_
#define __JSON_
#endif
#if __Weigh_bridge__
using DRTDLib;
#endif
#if __JSON_
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
#if __K3_Cloud_WebApi_
using Kingdee.BOS.WebApi.Client;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
#if __ZJF_WebAPI_
using static ZJF.ZJF_WEBAPI;
using System.Globalization;
#if KBT_Project
using KBT_Project.Models;
#endif
#endif
// using static ZJF.ZJF_WEBAPI;
#if __ANDROID__
using SQLite;
#endif

#if __TTS_
using System.Speech.Synthesis;
#endif
namespace ZJF
{
    #region COM_Deivce
#if __ZJF_COMM_
    public class ZJF_SimpleTcpClient
    {

    }
    public class ZJF_SimpleTcpServer
    {
        public Socket m_Socket_Client { get; private set; }
    }

    public class ZJF_TCP_SERVER
    {
        protected Socket m_Socket_Server;
        protected static List<ZJF_TCP_CLIENT> m_clients { get; private set; } = new List<ZJF_TCP_CLIENT>();
        private static object m_clinets_Locker = new object();

        public event ZJF_TCP_CLIENT.ReciveData onServerReciveDate;
        //private string m_Host_Ip = "";
        private int m_Port = 0;
        private int m_QuequeLen;
        private bool m_Server_Start = false;
        public string errorString { get; private set; } = "";
        static void ConnectClient(ZJF_TCP_CLIENT client)
        {
            ZJF_LOGGER.log("new client uuid is " + client.m_Guid.ToString()) ; 
            lock (m_clinets_Locker)
            {
                m_clients.Add(client);
            }
        }

        public static ZJF_TCP_CLIENT getClient(string uuid)
        {
            return m_clients.Find(c => c.m_Guid.ToString() == uuid);
        }
        static void DisconnectClient(ZJF_TCP_CLIENT client)
        {
            ZJF_LOGGER.log("disconnect client uuid is " + client.m_Guid.ToString());
            lock (m_clinets_Locker)
            {
                m_clients.Remove(client);
            }
        }

        public static string ErgodicClients()
        {
            string ret = "";
            foreach (var item in m_clients)
            {
                ret += "客户端 " + item.m_OrgAuthKey.ToString() + " 线程连接中,客户端类型" + item.m_clientType.ToString() +" \n ";
            }
            return ret;
        }
        public ZJF_TCP_SERVER(/*string ip,*/ int port, int quequelen = 100)
        {
            //m_Host_Ip = ip;
            m_Port = port;
            m_QuequeLen = quequelen;
        }

        public bool start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, m_Port);
                m_Socket_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Socket_Server.Bind(endPoint);
                m_Socket_Server.Listen(m_QuequeLen);
                ZJF_LOGGER.log("服务器监听已开始！\n");
                m_Server_Start = true;
                new Thread(ProcessClientConnect) { IsBackground = true }.Start(m_Socket_Server);
            }
            catch (SocketException sexp)
            {
                errorString = sexp.Message;
                return false;
            }
            return true;
        }

        public void broadcastMsg(ClientType type, string data, string guid = "")
        {
            foreach (var item in m_clients)
            {
                if (item.m_clientType == type && item.m_AuthKey == (guid == "" ? item.m_AuthKey : guid))
                {
                    item.send(data);
                }
            }
        }
        private void ProcessClientConnect(object socket)
        {
            var server = socket as Socket;
            while (m_Server_Start)
            {
                var client = server.Accept();
                ZJF_LOGGER.log("客户端接入！\n");
                ZJF_TCP_CLIENT client_ins = new ZJF_TCP_CLIENT(client);
                client_ins.OnDataRecive += onServerReciveDate;
                ConnectClient(client_ins);
                client_ins.OnDisconnect += (ZJF_TCP_CLIENT.Disconnect)((ZJF_TCP_CLIENT _client) =>
                {
                    ZJF_LOGGER.log("断开连接！");
                    DisconnectClient(_client);
                });
                client_ins.OnDataRecive += (ZJF_TCP_CLIENT.ReciveData)((ZJF_TCP_CLIENT _client, string data) =>
                {
                    ZJF_LOGGER.log("接收到消息：" + data);
                    if (data == "")
                    {

                    }
                    else
                    {
                        onServerReciveDate?.Invoke(_client,data);
                    }
                });
            }
        }
    }
    public enum ClientType
    {
        NotSet = 0,
        Mobile = 1,
        KingHoo_Communication_Service = 2,
        UIApplication = 3,
        UserConfirm = 4,
        KingHoo_Customer_Service = 5,
        KingHoo_Operation_Service = 6,
        Testing = 7
    }
    public enum Type_ZJF_TCP_CLIENT
    {
        NotSet = 0,
        OnClient = 1,
        OnServer = 2
    }
    public class ZJF_TCP_CLIENT
    {
        public Socket m_Socket_Client { get; private set; }
        public delegate void ReciveData(ZJF_TCP_CLIENT client, string data);
        public event ReciveData OnDataRecive;
        public delegate void Disconnect(ZJF_TCP_CLIENT client);
        public event Disconnect OnDisconnect;
        public bool m_Connect_Status = false;
        public ClientType m_clientType = ClientType.NotSet;
        public Guid m_Guid { get; private set; } = Guid.NewGuid();
        public bool m_Auth { get; set; } = false;
        public bool m_ClientDicExists { get; private set; } = false;
        public bool m_AuthSend { get; set; } = false;
        public string m_AuthKey { get; set; } = "";
        public Guid m_OrgAuthKey { get; private set; } = Guid.Empty;
        private int m_Port = 0;
        private string m_IpAddress = "";
        private Type_ZJF_TCP_CLIENT m_Type = Type_ZJF_TCP_CLIENT.NotSet;
        public string errorString { get; private set; } = "";
        public ZJF_TCP_CLIENT(Socket client)
        {
            m_Socket_Client = client;
            m_Type = Type_ZJF_TCP_CLIENT.OnServer;
            m_Connect_Status = true;
            m_OrgAuthKey = Guid.NewGuid();
            new Thread(onDataRecive) { IsBackground = true }.Start(m_Socket_Client);
        }
        public ZJF_TCP_CLIENT(string ip, int port, ClientType type)
        {
            m_clientType = type;
            m_Port = port;
            m_IpAddress = ip;
            m_Type = Type_ZJF_TCP_CLIENT.OnClient;
            m_Socket_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public bool connect()
        {
            if (m_Type == Type_ZJF_TCP_CLIENT.OnClient)
            {
                try
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(m_IpAddress), m_Port);
                    m_Socket_Client.Connect(ep);
                }
                catch (SocketException sexp)
                {
                    errorString = sexp.Message;
                    return false;
                }
                catch (Exception exp)
                {
                    errorString = exp.Message;
                    return false;
                }
            }
            m_Connect_Status = true;
            new Thread(onDataRecive) { IsBackground = true }.Start(m_Socket_Client);
            return true;
        }
        public bool send(string data)
        {
            if (m_Connect_Status && m_Auth)
            {
                try
                {
                    m_Socket_Client.Send(Encoding.UTF8.GetBytes(data));
                }
                catch (SocketException sexp)
                {
                    errorString = sexp.Message;
                    return false;
                }
                return true;
            }
            return false;
        }
        private void ShowMessage(JObject ret_json)
        {
            if (ZJF.ZJF_PROMPT.m_Prompt == null || (ZJF.ZJF_PROMPT.m_Prompt != null && ZJF.ZJF_PROMPT.m_Prompt.HasExited))
            {
                ZJF_LOGGER.log("窗口提示已经展示！");
                PromptMessage(ret_json);
            }
            else
            {
                ZJF_LOGGER.log("上个程序广播还没有关闭！请等待");
            }
        }
        public bool disconnect()
        {
            try
            {
                m_Socket_Client.Disconnect(false);
                m_Socket_Client.Shutdown(SocketShutdown.Both);
                m_Socket_Client.Dispose();
            }
            catch(SocketException sexp)
            {
                errorString = sexp.Message;
                return false;
            }
            return true;
        }
        private JObject Formated_Json(string type,int clienttype,string result,string des,int code,string entry_authcode )
        {
            JObject main = new JObject();
            JObject subNode = new JObject();
            main.Add("Type", type);
            main.Add("Result", result);
            main.Add("Des", des);
            main.Add("Code", code);
            main.Add("ClientType", clienttype);
            subNode.Add("AuthCode", entry_authcode);
            main.Add(new JProperty("Data", subNode));
            return main;
        }

        private void onDataRecive(object socket)
        {
            var client = socket as Socket;
            client.ReceiveTimeout = 3000;
            try
            {
                if (m_Type == Type_ZJF_TCP_CLIENT.OnClient && !m_Auth && !m_ClientDicExists)
                {
                    if (!ZJF_CRYPTOR.DicExists())
                    {
                        //同步字典
                        ZJF_LOGGER.Debug("字典不存在，同步字典");
                        client.Send(Encoding.UTF8.GetBytes(ZJF_CRYPTOR.RequireDic(false)));
                    }
                    else
                    {
                        client.Send(Encoding.UTF8.GetBytes(ZJF_CRYPTOR.RequireDic(true)));
                        m_ClientDicExists = true;
                    }
                }
                while (m_Connect_Status)
                {
                    try
                    {

                        if (m_Type == Type_ZJF_TCP_CLIENT.OnServer && !m_Auth && !m_AuthSend && m_ClientDicExists)
                        {
                            ZJF_LOGGER.Debug("已连接，开始验证");
                            if (!ZJF_CRYPTOR.DicExists())
                            {
                                ZJF.ZJF_LOGGER.log("服务器字典不存在！线程将终止，客户端将断开！");
                                break;
                            }
                            else
                            {
                                ZJF_LOGGER.Debug("字典存在，初始化");
                                ZJF_CRYPTOR.init();
                            }
                            //if (ZJF_CRYPTOR.m_DicLoadError)
                            //{
                            //    errorString = "字典载入失败！禁止通讯！";
                            //    break;
                            //}
                            m_AuthKey = ZJF_CRYPTOR.encrypt(m_OrgAuthKey);
                            byte[] buf = Encoding.UTF8.GetBytes(Formated_Json("Auth",0, "", "", 5, m_AuthKey).ToString());
                            client.Send(buf);

                            m_AuthSend = true;
                        }
                        byte[] buffer = new byte[1024 * 512];
                        var recive_bytes = client.Receive(buffer);
                        client.Send(Encoding.UTF8.GetBytes("\0"));
                        var recive_string = Encoding.UTF8.GetString(buffer).Replace("\0", "");
                        if (recive_bytes == 0 || string.IsNullOrEmpty(recive_string))
                        {
                            continue;
                        }
                        JObject ret_json;
                        try
                        {
                            ret_json = JObject.Parse(recive_string);
                        }
                        catch
                        {
                            break;
                        }
                        //判断字典是否存在，不存在请求字典
                        if (m_Type == Type_ZJF_TCP_CLIENT.OnServer && !m_Auth && !m_ClientDicExists)
                        {
                            if (ret_json.ContainsKey("Type") && ret_json.ContainsKey("Code") && ret_json.ContainsKey("Data") && ret_json["Data"]["AuthCode"] != null)
                            {
                                if (ret_json["Type"].ToString() == "RequestDic" &&
                                    ret_json["Code"].ToString() == "9" &&
                                    ret_json["Data"]["AuthCode"].ToString() == "{1D44BF52-20E2-4E15-B593-2F2DCE47D0A3}")
                                {
                                    ZJF_LOGGER.Debug("字典被请求");
                                    string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\main.dic";
#if __ANDROID__
                                        filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "main.dic");
#endif
                                    if (!File.Exists(filePath))
                                    {
                                        ZJF.ZJF_LOGGER.log("客户端请求字典不存在，连接将断开！");
                                        return;
                                    }
                                    var streamReader = new StreamReader(filePath);
                                    var ret = streamReader.ReadToEnd();                                    

                                    ZJF_LOGGER.Debug("发送字典");
                                    client.Send(Encoding.UTF8.GetBytes(Formated_Json("RequestDic",0, "", "", 10, ret).ToString()));
                                    continue;
                                }
                                if (ret_json["Type"].ToString() == "RequestDic" &&
                                    ret_json["Code"].ToString() == "11" &&
                                    ret_json["Data"]["AuthCode"].ToString() == "{1D44BF52-20E2-4E15-B593-2F2DCE47D0A3}")
                                {
                                    m_ClientDicExists = true;
                                    continue;
                                }
                            }

                        }
                        //接收字典
                        if (m_Type == Type_ZJF_TCP_CLIENT.OnClient && !m_Auth && !m_ClientDicExists)
                        {
                            if (ret_json.ContainsKey("Type") && ret_json.ContainsKey("Code") && ret_json.ContainsKey("Data") && ret_json["Data"]["replyDic"] != null)
                            {
                                ZJF_LOGGER.Debug("收到字典！");
                                if (ret_json["Type"].ToString() == "RequestDic" &&
                                    ret_json["Code"].ToString() == "10")
                                {
                                    if (ZJF_CRYPTOR.CreateDic(ret_json["Data"]["replyDic"].ToString()))
                                    {
                                        client.Send(Encoding.UTF8.GetBytes(ZJF_CRYPTOR.RequireDic(true)));
                                        m_ClientDicExists = true;
                                    }
                                }
                            }
                        }


                        if (m_Type == Type_ZJF_TCP_CLIENT.OnClient && !m_Auth)
                        {
                            if (!m_AuthSend && ret_json["Type"].ToString() == "Auth" && ret_json["Code"].ToString() == "5")
                            {
                                var decrypt_string = ZJF_CRYPTOR.decrypt(ret_json["Data"]["AuthCode"].ToString());

                                byte[] buf = Encoding.UTF8.GetBytes(Formated_Json("Auth", (int)m_clientType, "", "", 1, decrypt_string).ToString());
                                client.Send(buf);
                                m_AuthSend = true;
                                m_Auth = true;
                                ZJF_LOGGER.log("验证成功！" + m_AuthKey.ToString());
                                continue;
                            }
                            if (ret_json["Type"].ToString() == "Auth" && ret_json["Code"].ToString() == "1")
                            {
                                m_Auth = true;
                                continue;
                            }
                        }




                        if (m_Type == Type_ZJF_TCP_CLIENT.OnServer && !m_Auth && m_AuthSend)
                        {
                            if (ret_json["Data"]["AuthCode"].ToString().Contains(m_OrgAuthKey.ToString()) && ret_json.ContainsKey("ClientType"))
                            {
                                m_Auth = true;
                                m_clientType = (ClientType)Convert.ToInt32(ret_json["ClientType"].ToString());
                                byte[] buf = Encoding.UTF8.GetBytes(Formated_Json("Auth",0, "Success","",1,"").ToString());
                                client.Send(buf);
                                ZJF_LOGGER.log("验证成功！" + m_OrgAuthKey.ToString());
                                continue;
                            }
                        }


                        if (m_Type == Type_ZJF_TCP_CLIENT.OnServer && m_Auth)
                        {
                            //string _Type = ret_json["Type"].ToString();
                            OnDataRecive?.Invoke(this, recive_string);
                        }
                        if(m_Type == Type_ZJF_TCP_CLIENT.OnClient && m_Auth)
                        {
                            OnDataRecive?.Invoke(this, recive_string);
                        }
                    }
                    catch (SocketException sexp)
                    {
                        if (sexp.ErrorCode == 10060)
                        {

                        }
                        else
                        {
                            errorString = sexp.Message;
                        }
                        if (!client.Connected)
                        {
                            m_Connect_Status = false;
                            break;
                        }
                    }
                    catch (Exception exp)
                    {
                        errorString = exp.Message;
                    }
                }
            }
            finally
            {
                OnDisconnect?.Invoke(this);
                try
                {
                    client.Shutdown(SocketShutdown.Both);

                }
                catch (SocketException sexp)
                {
                    ZJF_LOGGER.Debug(sexp.Message);
                }
                client.Disconnect(false);
                client.Close();
            }
        }
        private void PromptMessage(JObject json_)
        {
            try
            {
                if (json_["Data"]["FAsk"].ToString() == "Y")
                {
                    string content = json_["Data"]["FAsk_Content"].ToString();
                    string title = json_["Data"]["FAsk_Title"].ToString();
                    string defaultButton = json_["Data"]["FAsk_DefaultButton"].ToString();
                    string delay = json_["Data"]["FAsk_Delay"].ToString();
                    string namePipe = Guid.NewGuid().ToString();
#if __Windows_
                    String applicationName = @"E:\Kingdee_Project\Kingdee_K3_Cloud\康奥电力\implementation\地磅方案\开发\确认通知程序\Notifier_KangAo2\Notifier_KangAo2\bin\Debug\Notifier_KangAo2.exe ";
#endif
                    if (!ZJF.ZJF_UDP.m_Socket_Server_Running)
                    {
                        ZJF.ZJF_UDP udp = new ZJF_UDP(UDP_TYPE.UDP_SERVER);
                        udp.OnreciveFrom += (string txt) => {
                            try
                            {
                                JObject json = JObject.Parse(txt);
                                ZJF_LOGGER.log(json.ToString());
                            }
                            catch (Exception exp)
                            {
                                errorString = exp.Message;
                            }
                        };
                    }

#if __Windows_
                    ZJF.ZJF_PROMPT.ShowPrompt(applicationName,content,title,Convert.ToInt32( delay),defaultButton,"");
#endif
                }
            }
            catch (Exception exp)
            {
                errorString = exp.Message;
            }
        }
    }
#endif
    #endregion

    #region Cryption
#if __JSON_
    public class ZJF_CRYPTOR
    {
        static List<char> m_CharSets = new List<char>() {
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        };
        static List<int> m_NumberSets = new List<int>() {
        0,1,2,3,4,5,6,7,8,9,17,18,19,20,21,22,23,24,25,26,27,28,29,30,
        31,32,33,34,35,36,37,38,39,40,41,42,49,50,51,52,53,54,55,56,57,
        58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74
        };
        static int m_Base_Number = 0x30;
        static Dictionary<int, char> m_DefaultDic = null;// new Dictionary<int, char>();
        public static string errorString { get; private set; }
        public static bool m_DicInit { get; private set; } = false;
        public static bool m_DicLoadError { get; private set; } = false;
        public static bool DicExists()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\main.dic";
#if __ANDROID__
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "main.dic");
#endif
            return File.Exists(filePath);

        }
        public static bool CreateDic(string txt)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\main.dic";
#if __ANDROID__
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "main.dic");
#endif
                var stream = new StreamWriter(filePath, false, Encoding.Default);
                stream.WriteLine(txt);
                stream.Flush();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string RequireDic(bool existStatus)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\main.dic";
#if __ANDROID__
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "main.dic");
#endif
            JObject main = new JObject();
            JObject subNode = new JObject();
            main.Add("Type", "RequestDic");
            main.Add("Result", "");
            main.Add("Des", "");
            if (existStatus)
            {
                main.Add("Code", 11);
            }
            else
            {
                main.Add("Code", 9);
            }

            subNode.Add("AuthCode", "{1D44BF52-20E2-4E15-B593-2F2DCE47D0A3}");
            main.Add(new JProperty("Data", subNode));
            return main.ToString();

        }
        public static Dictionary<int, char> init()
        {
            m_DefaultDic = new Dictionary<int, char>();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\main.dic";
#if __ANDROID__
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "main.dic");
#endif
            if (!DicExists())
            {
                m_DicLoadError = true;
                errorString = "字典文件不存在";
                //同步加密字典

                return null;
            }
            var streamReader = new StreamReader(filePath);
            var ret = streamReader.ReadToEnd();
            streamReader.Close();
            var dicTxt = HexStringToString(ret, Encoding.UTF8);
            dicTxt = dicTxt.Replace("][", "|").Replace("[", "").Replace("]", "").Replace("\0", "").Replace("\r", "").Replace("\n", "");
            try
            {
                var arr = dicTxt.Split('|');
                if (arr.Count() != 62)
                {
                    errorString = "字典格式不正确";
                    m_DicLoadError = true;
                    return null;
                }

                for (int i = 0; i < arr.Count(); i++)
                {
                    var pair = arr[i].Replace(" ", "").Split(',');
                    m_DefaultDic[Convert.ToInt32(pair[0])] = pair[1][0];
                }
            }
            catch
            {
                errorString = "字典解析出现错误";
                m_DicLoadError = true;
            }
            m_DicInit = true;
            return m_DefaultDic;
        }

        private static string StringToHexString(string s, Encoding encode, string spanString)
        {
            byte[] b = encode.GetBytes(s);
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)
            {
                result += Convert.ToString(b[i], 16) + spanString;
            }
            return result;
        }

        private static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                try
                {
                    strTemp = hs.Substring(i * 2, 2);
                    b[i] = Convert.ToByte(strTemp, 16);
                }
                catch
                {

                }

            }
            return encode.GetString(b);
        }

        public static string encrypt(Guid guid)
        {
            if (!m_DicInit)
            {
                init();
            }
            if (m_DefaultDic == null && m_DefaultDic.Count != 62) return "";
            try
            {
                string result = "";
                string shortDic = DateTime.Now.ToString("yyyyMMdd");
                var t = guid.ToString();
                for (var i = 0; i < t.Length; i++)
                {
                    int c = 0;
                    if ((t[i] >= 'A' && t[i] <= 'Z') || (t[i] >= '0' && t[i] <= '9') || (t[i] >= 'a' && t[i] <= 'z'))
                    {
                        if (Int32.TryParse(shortDic.Substring(i % 6, 1), out c))
                        {
                            try
                            {
                                result += m_DefaultDic[m_NumberSets[((int)t[i] - m_Base_Number + c) % 62]].ToString();
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        result += t[i];
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        public static string decrypt(string str)
        {
            if (!m_DicInit)
            {
                init();
            }
            if (m_DefaultDic == null && m_DefaultDic.Count != 62) return "";
            try
            {
                string result = "";
                string shortDic = DateTime.Now.ToString("yyyyMMdd");
                for (var i = 0; i < str.Length; i++)
                {
                    var r = i % 6;
                    int c = 0;
                    if ((str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= '0' && str[i] <= '9') || (str[i] >= 'a' && str[i] <= 'z'))
                    {
                        if (Int32.TryParse(shortDic.Substring(r, 1), out c))
                        {
                            var ret1 = m_NumberSets.FindIndex(item => item == m_DefaultDic.Where(ii => (int)ii.Value == (int)str[i]).First().Key);
                            result += Char.ConvertFromUtf32(ret1 - c < 0 ? ret1 - c + m_Base_Number + 62 : ret1 - c + m_Base_Number);
                        }
                    }
                    else
                    {
                        result += str[i];
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }

        }

    }
#endif
    #endregion

    #region Logger
    public class ZJF_LOGGER
    {
        private static object m_Write_locker = new object();
        private static string m_Log_Path = "";
        private static bool m_Init = false;
        private static StreamWriter m_Content_Writer = null;
        public static string errorString { get; private set; }
        public static bool init(string filename, bool basePath = false)
        {
            try
            {
                if (basePath)
                {
                    m_Log_Path = filename;
                }
                else
                {
                    m_Log_Path = AppDomain.CurrentDomain.BaseDirectory + filename + ".txt";
                }

#if __ANDROID__
                    m_Log_Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename + ".txt");
#endif

                m_Content_Writer = new StreamWriter(m_Log_Path, false, Encoding.Default);
                m_Init = true;
            }
            catch (Exception exp)
            {
                errorString = exp.Message;
                return false;
            }
            return true;
        }
        public static void log(string txt)
        {
            try
            {
                if (!m_Init)
                {
                    init("log");
                }

            }
            catch (Exception exp)
            {
                errorString = exp.Message;
                return;
            }
            lock (m_Write_locker)
            {
                try
                {
                    if (txt != null)
                    {
                        m_Content_Writer.WriteLine(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") + " " +
                    DateTime.Now.Millisecond.ToString() + " Thread:" + Thread.CurrentThread.ManagedThreadId.ToString() + " " +
                    (new StackTrace()).GetFrame(1).GetMethod().Name + " \n" + txt);
                        m_Content_Writer.Flush();
                    }
                }
                catch (Exception exp)
                {
                    errorString = exp.Message;
                    return;
                }
            }

        }
        public static void Info(string debugMsg)
        {
#if __Info_
                log(debugMsg);
#endif
        }
        public static void Debug(string debugMsg)
        {
#if __Debug_
            log(debugMsg);
#endif
        }
        public static void Error(string debugMsg)
        {
#if __Error_
                log(debugMsg);
#endif
        }
        public static void Verbose(string debugMsg)
        {
#if __Verbose_
                log(debugMsg);
#endif
        }
        public static void Warn(string debugMsg)
        {
#if __Warn_
                 log(debugMsg);
#endif
        }
        public static void Fatal(string debugMsg)
        {
#if __Warn_
                 log(debugMsg);
#endif
        }

        public static void close()
        {
            if (m_Content_Writer != null)
            {
                m_Content_Writer.Close();
                m_Content_Writer.Dispose();
            }
        }
    }
    #endregion Logger

    public class ZJF_DATABASE
    {

    }
    #region WebApi
#if __ZJF_WebAPI_
    public class ZJF_WEBAPI
    {
        public static string m_errorString { get; private set; } = "";
        private static string m_Host_Ip { get; set; }
        private static string m_Acctid { get; set; }
        private static string m_UserName { get; set; }
        private static string m_Password { get; set; }
        private static CookieContainer m_Cookie = new CookieContainer();
        public enum K3Cloud_AddressType
        {

            LoginByAppSecret = 0,
            ExecuteBillQuery = 1,
            View = 2,
            Push = 3,
            approvalPath = 4,
            forwardPath = 5,
            addSignPath = 6,
            attachmentPath = 7,
            mobileBillDetailPath = 8,
            Login = 9,
            pdafromscddpushrkd = 10,
            pdafromscddpushlld = 11,
            pdafromscddpushhbd = 12,
            save = 13,
            SubmitPath = 14,
            AuditPath = 15,
            Sendmsg = 16,
            groupsave = 17,
            groupquery = 18,
        }
        public static Dictionary<K3Cloud_AddressType, string> PathMap = new Dictionary<K3Cloud_AddressType, string>() {
        {K3Cloud_AddressType.LoginByAppSecret ,           @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret.common.kdsvc"},
        {K3Cloud_AddressType.ExecuteBillQuery ,           @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery.common.kdsvc"},
        {K3Cloud_AddressType.View ,                       @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View.common.kdsvc"},
        {K3Cloud_AddressType.Push ,                       @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push.common.kdsvc"},
        {K3Cloud_AddressType.approvalPath ,               @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExcuteOperation.common.kdsvc"},
        {K3Cloud_AddressType.forwardPath ,                @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExcuteOperation.common.kdsvc"},
        {K3Cloud_AddressType.addSignPath ,                @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExcuteOperation.common.kdsvc"},
        {K3Cloud_AddressType.attachmentPath ,             @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExcuteOperation.common.kdsvc"},
        {K3Cloud_AddressType.mobileBillDetailPath ,       @"/K3Cloud/SZGD.K3.Business.BillPulgin.WebAPI.MobileViewAPI.ExecuteService,SZGD.K3.Business.BillPulgin.common.kdsvc"},
        {K3Cloud_AddressType.Login ,                      @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser.common.kdsvc"},
        //{K3Cloud_AddressType.pdafromscddpushrkd ,         @"/k3cloud/WebAPI_Customer_KingHoo.WebAPI_PDAReport.PDAFromSCDDPushRKD,WebAPI_Customer_KingHoo.common.kdsvc"},
        //{K3Cloud_AddressType.pdafromscddpushlld ,         @"/k3cloud/WebAPI_Customer_KingHoo.WebAPI_PDAReport.PDAFromSCDDPushLLD,WebAPI_Customer_KingHoo.common.kdsvc"},
        //{K3Cloud_AddressType.pdafromscddpushhbd ,         @"/k3cloud/WebAPI_Customer_KingHoo.WebAPI_PDAReport.PDAFromGXJHPushHBD,WebAPI_Customer_KingHoo.common.kdsvc"},
        {K3Cloud_AddressType.save,                        @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save.common.kdsvc" },
        {K3Cloud_AddressType.SubmitPath ,                 @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit.common.kdsvc" },
        {K3Cloud_AddressType.AuditPath,                   @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit.common.kdsvc" },
        {K3Cloud_AddressType.Sendmsg,                     @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg.common.kdsvc" },
        {K3Cloud_AddressType.groupsave,                   @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave.common.kdsvc" },
        {K3Cloud_AddressType.groupquery,                  @"/K3Cloud/Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.QueryGroupInfo.common.kdsvc" },
    };
        private static bool m_Init_Status = false;
        public static bool m_Login_Status { get; private set; } = false;
        public static bool init(string host = "", string acctid = "", string username = "", string password = "")
        {
            if (host != "")
                m_Host_Ip = host;
            if (acctid != "")
                m_Acctid = acctid;
            if (username != "")
                m_UserName = username;
            if (password != "")
                m_Password = password;

            m_Init_Status = true;
            return Login();
        }

        /*
        public class DeviceInfo
        {
            public string IP { get; set; }
            public string DeviceName { get; set; }
            public ushort TcpPort { get; set; }
            public ushort UdpPort { get; set; }
        }
        */
#if __Weigh_bridge__
        public static List<ZJF_DeviceInfo> DoorAccess_GetDeviceInfo()
        {
            var retList = new List<ZJF_DeviceInfo>();
            try
            {
                var ret = BatchQuery("kao_DoorAccessControler", "FNumber,FName,FLocation,FBillStatus,FUUIID,FDeviceName,FIP,FTCPPort,FUDPPort,FFingerPrint,FIDCard,FQRCode,FDoorType,FID,FUserName,FUserPassword,FDoorType.FName,FTriggerAreaSpeak");
                var list = JArray.Parse(ret);
                for (int i = 0; i < list.Count; i++)
                {
                    var ins = new ZJF_DeviceInfo() { IP = list[i][6].ToString(), DeviceName = list[i][5].ToString(), TcpPort = (ushort)Convert.ToUInt32(list[i][7].ToString()), UdpPort = (ushort)Convert.ToUInt32(list[i][8].ToString()) };
                    ins.FNumber = list[i][0].ToString();
                    ins.FName = list[i][1].ToString();
                    ins.FLocation = list[i][2].ToString();
                    ins.FBillStatus = list[i][3].ToString();
                    ins.FUUIID = list[i][4].ToString();
                    ins.FDoorType = list[i][12].ToString();
                    ins.FID = list[i][13].ToString();
                    ins.FUserName = list[i][14].ToString();
                    ins.FUserPassword = list[i][15].ToString();
                    ins.FDoorTypeName = list[i][16].ToString();
                    ins.FTriggerAreaSpeak = list[i][17].ToString();
                    retList.Add(ins);
                }
            }
            catch (Exception exp)
            {
                m_errorString = exp.Message;
            }

            return retList;
        }
#endif
        public static string QueryAccount(string username, string password)
        {
            try
            {
                var ret = BatchQuery("kao_DoorAccessAppAcct", "fid", "FAccount='" + username + "' and fpassword='" + password + "'");
                var jret = JArray.Parse(ret);
                if (jret != null && jret.Count > 0)
                {
                    return jret[0][0]?.ToString() ?? "";
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static JArray QueryAccountWidthRights(string username, string password)
        {
            try
            {
                var ret = BatchQuery("kao_DoorAccessAppAcct", "FID,FCharacter.FCaption,FAccount,FRight_GXHB,FRight_GXHB_View,FRight_GXHB_Save,FRight_ZXZT,FRight_ZXZT_QrCode,FRight_MJZX,FRight_MJZX_CK_KCDCZ,FRight_MJZX_CK_KCDZH,FRight_MJZX_CK_ZHZ,FRight_MJZX_CK_ZHWC,FRight_MJZX_CK_MCDCZ,FRight_MJZX_CK_DCC,FRight_MJZX_RK_MCDCZ,FRight_MJZX_RK_MCDXH,FRight_MJZX_RK_XHZ,FRight_MJZX_RK_XHWC,FRight_MJZX_RK_KCDCZ,FRight_MJZX_RK_DCC,FRight_MJZX_CLCZ,FRight_MJZX_HWCZ,FRight_MJZX_HWCZ_View,FRight_MJZX_HWCZ_Save ", "FAccount='" + username + "' and fpassword='" + password + "'");
                var jret = JArray.Parse(ret);
                if (jret != null && jret.Count > 0)
                {
                    return JArray.Parse(jret[0].ToString());
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        private static string SyncRequest(K3Cloud_AddressType urlpath, string para)
        {
            if (!m_Init_Status)
            {
                init();
            }
            string url = (m_Host_Ip.Substring(m_Host_Ip.Length - 1, 1) == @"/" ? m_Host_Ip.Substring(0, m_Host_Ip.Length - 1) : m_Host_Ip) + PathMap[urlpath];
            //ZJF.ZJF_LOGGER.log("Request url is " + url + "\r\n" + "para:" +  para);
            HttpWebRequest httpRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.CookieContainer = m_Cookie;
            httpRequest.Timeout = 1000 * 60 * 10;//10min
            using (Stream reqStream = httpRequest.GetRequestStream())
            {
                JObject jObj = new JObject();
                jObj.Add("format", 1);
                jObj.Add("useragent", "ApiClient");
                jObj.Add("rid", Guid.NewGuid().ToString().GetHashCode().ToString());
                jObj.Add("parameters", para);
                jObj.Add("timestamp", DateTime.Now);
                jObj.Add("v", "1.0");
                string sContent = jObj.ToString();
                var bytes = UnicodeEncoding.UTF8.GetBytes(sContent);
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Flush();
            }
            using (var repStream = httpRequest.GetResponse().GetResponseStream())
            {
                using (var reader = new StreamReader(repStream))
                {
                    string Ret = reader.ReadToEnd();
                    try
                    {
                        var jret = JObject.Parse(Ret);
                        if (jret["Result"] != null && jret["Result"]["ResponseStatus"] != null)
                        {
                            if ((bool)jret["Result"]["ResponseStatus"]["IsSuccess"] == false)
                            {
                                if ((int)jret["Result"]["ResponseStatus"]["ErrorCode"] == 500)
                                {
                                    if (jret["Result"]["ResponseStatus"]["Errors"]["Message"].ToString().Contains("未将对象引用设置到对象的实例"))
                                    {
                                        ZJF_LOGGER.log("SyncRequest failed with reason : " + Ret);
                                        return "";
                                    }
                                    if (jret["Result"]["ResponseStatus"]["Errors"]["Message"].ToString().Contains("会话信息已丢失"))
                                    {
                                        if (Login())
                                        {
                                            return SyncRequest(urlpath, para);
                                        }
                                    }

                                }
                                else
                                {
                                    ZJF_LOGGER.log("SyncRequest failed with reason : " + Ret);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }


                    //ZJF.ZJF_LOGGER.log("Result:" + Ret);
                    //m_Text_Ret = Ret;
                    if (Ret.StartsWith("response_error:"))
                    {
                        errorString = Ret.TrimStart("response_error:".ToCharArray());
                        ZJF_LOGGER.log("SyncRequest error :" + errorString);
                        return null;
                    }
                    else
                    {
                        return Ret;//JObject.Parse(Ret);
                    }
                }
            }
        }
        public static string CustDicToJson(Dictionary<string, JToken> maps)
        {
            JObject json = new JObject();
            foreach (var item in maps)
            {
                json.Add(item.Key, item.Value);
            }
            //main.Add(new JProperty("data", json));
            return json.ToString();
        }
        public static string sendRepuest(K3Cloud_AddressType path, Dictionary<string, JToken> maps)
        {
            return SyncRequest(path, JsonConvert.SerializeObject(new object[] { CustDicToJson(maps) }));
        }
        public static string sendRepuest(K3Cloud_AddressType path, object[] ol)
        {
            return SyncRequest(path, JsonConvert.SerializeObject(ol));
        }
        public static string sendRepuest(K3Cloud_AddressType path, string json)
        {
            return SyncRequest(path, json);
        }
        public static string PublicsendRepuest(K3Cloud_AddressType path, object[] ol)
        {
            return sendRepuest(path, ol);
        }

        /*
        *******************采购********************
        Wait_For_Enter = 0,             //待入厂
        Empty_Wait_For_Weigh = 1,       //空车待称重
        Empty_For_Loading = 2,          //空车待称重
        On_Loading = 3,                 //装车中
        Finish_Loading = 4,             //完成装货
        Full_Wait_For_Weigh = 8,        //满车待称重
        Wait_For_Exit = 9               //待出厂
        */

        /*
        ******************销售*********************
        Wait_For_Enter = 0,             //待入厂
        Full_Wait_For_Weigh = 8,        //满车待称重
        Full_For_Unloading = 5,         //满车待卸货
        On_Unloading = 6,               //卸货中
        Finish_Unloading = 7,           //完成卸货
        Empty_Wait_For_Weigh = 1,       //空车待称重
        Wait_For_Exit = 9               //待出厂
        */
        public enum QRBill_Status_Sale
        {
            Wait_For_Enter = 0,             //待入厂
            Empty_Wait_For_Weigh = 1,       //空车待称重
            Empty_For_Loading = 2,          //空车待装货
            On_Loading = 3,                 //装车中
            Finish_Loading = 4,             //完成装货
            Full_Wait_For_Weigh = 5,        //满车待称重
            Wait_For_Exit = 6,              //待出厂
            Done = 7                        //已完成
        }
        public enum QRBill_Status_Buy
        {
            Wait_For_Enter = 0,             //待入厂
            Full_Wait_For_Weigh = 1,        //满车待称重
            Full_For_Unloading = 2,         //满车待卸货
            On_Unloading = 3,               //卸货中
            Finish_Unloading = 4,           //完成卸货
            Empty_Wait_For_Weigh = 5,       //空车待称重 
            Wait_For_Exit = 6,              //待出厂
            Done = 7                        //已完成
        }
        public class QRBillEntry
        {
            string FItemID_Number { get; set; }
            string FItemID_Name { get; set; }
            string FItemID_Model { get; set; }
            string FQty { get; set; }
            string FTakeLocation { get; set; }
            string FUnitID { get; set; }
            string FStockID { get; set; }
        }

        public class QRBill
        {
            //单据编号
            public string FBillNo { get; set; }
            //单据类型
            public string FBillType { get; set; }
            //业务日期
            public string FBusinessDate { get; set; }
            //单据状态
            public string FBillStatus { get; set; }
            //条码执行状态
            public string FQRBillExcuteStatus { get; set; }
            //开门类型
            public string FDoorOpenType { get; set; }
            //单据提起
            public string FDate { get; set; }

            public List<QRBillEntry> FEntry { get; set; }
        }

        public class QRBill_WB
        {
            //单据编号
            public string FBillNo { get; set; }
            //单据类型
            public string FBillType { get; set; }
            //业务日期
            public string FQRCodeStatus { get; set; }
            //单据状态
            public string FWhichDoor { get; set; }
            //条码执行状态
            public string FPrintCode { get; set; }
            //单据提起
            public string FDate { get; set; }

            public List<QRBillEntry> FEntry { get; set; }
        }
        public void Update_QRBill_Status()
        {

        }

        public static string GetMoBill(string BillNo)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = BillNo;
            pair["Id"] = 0;
            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "PRD_MO", paras });
            if (string.IsNullOrEmpty(ret))
                return null;
            return ret;
        }
        public static string GetMoBillDetail(string BillNo)
        {
            return "";
        }

        public static string GetOperationPlanning(string BillNo, int EntryId)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = "SFC_OperationPlanning";
            pair["FieldKeys"] = "FBillNo";
            pair["FilterString"] = @"FMONumber='" + BillNo + @"' and FMOEntryId =  " + EntryId.ToString();
            pair["OrderString"] = "";
            pair["TopRowCount"] = 0;
            pair["StartRow"] = 0;
            pair["Limit"] = 0;

            return sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair);

        }
        public static bool SendAuditMeg(string fbillno, string username, string billid)
        {
            try
            {
                var ret = SyncRequest(K3Cloud_AddressType.Sendmsg, JsonConvert.SerializeObject(new List<object>(new object[] { "{\"Model\":[{\"FTitle\":\"条码派工单" + fbillno + "需要审批" + DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss") + "\",\"FContent\":\"条码派工单已到达指定阶段," + fbillno + "需要审批!消息发送时间" + DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss") + ",请查确认单据后，将单据更新至【待出厂】状态，使司机可以扫码出厂！\",\"FReceivers\":\"" + username + "\",\"FType\":\"0\",\"FobjectTypeId\":\"kao_TMPGD_KingHoo\",\"FkeyValue\":\"" + billid + "\"}]}" })));
                if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    return true;
                }
            }
            catch (Exception exp)
            {
                errorString = exp.Message;
            }
            return false;
        }

        public static string GetOperationPlanningForText(string billno)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = billno;
            pair["Id"] = 0;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "SFC_OperationPlanning", paras });
            if (string.IsNullOrEmpty(ret))
                return null;
            return ret;
        }

        public static JObject GetOperationPlanning(string billno)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = billno;
            pair["Id"] = 0;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "SFC_OperationPlanning", paras });
            if (string.IsNullOrEmpty(ret))
                return null;
            return JObject.Parse(ret);
        }
        public static string BatchQuery(string formid, string fieldkeys, string filterstring = "", string orderstring = "", int toprowcount = 0, int startrow = 0, int limit = 0)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = formid;
            pair["FieldKeys"] = fieldkeys;
            pair["FilterString"] = filterstring;
            pair["OrderString"] = orderstring;
            pair["TopRowCount"] = toprowcount;
            pair["StartRow"] = startrow;
            pair["Limit"] = limit;

            return sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair);
        }
#if __ANDROID__

        public static string GetWeighBridgeList(Predicate<KingHoo_App.Models.FWeighBridge> where = null)
        {
            if (!ValidateLogin()) return null;
            return JArray.Parse(BatchQuery("kao_WeighBridge",
                "FNumber,FName,FLocation,FBillStatus,FUUIID,FProtocol,FIP,FPortName,FBaudRate,FParity,FDataBits,FStopBits")).ToString();
        }
#endif
        public static /*List<QRBill>*/string GetQRBillList(Predicate<QRBill> where = null)
        {
            if (!ValidateLogin()) return null;

            //默认不查询已完成的单据
            // 已完成单据的判定标准：
            //      条码状态不是完成

            return JArray.Parse(BatchQuery("kao_TMPGD_KingHoo",
                "FBillNo,FBillTypeID,FDate,FBillStatus,FQRCodeStatus,FWhichDoor,FMaterialID,FQty,FUnitID,FStockID,FLocation,FCustID")).ToString();

            //return JArray.Parse(BatchQuery("kao_TMPGD_KingHoo","FBillNo,FBillTypeID,FDate,FBillStatus,FQRCodeStatus,FWhichDoor,FMaterialID,FQty,FUnitID,FStockID,FLocation,FCustID")).Where<QRBill>(where).ToString();

        }

        public static List<QRBill_WB> GetQRBillListWithUserID(string FID, System.Func<QRBill_WB, bool> where = null)
        {
            if (!ValidateLogin()) return null;

            //默认不查询已完成的单据
            // 已完成单据的判定标准：
            //      条码状态不是完成


            //return JArray.Parse(BatchQuery("kao_TMPGD_KingHoo",
            //    "FBillNo,FBillTypeID,FDate,FBillStatus,FQRCodeStatus,FWhichDoor,FMaterialID,FQty,FUnitID,FStockID,FLocation,FCustID", @"(FCustID = \'" + FID + "\' and FQRCodeStatus != \'K\')")).Select(item => new QRBill
            //    {
            //        FBillNo = item[0].ToString(),
            //        FBillType = item[1].ToString(),
            //        Fdate
            //    }).ToList();
            var ret = JArray.Parse(BatchQuery("kao_TMPGD_KingHoo",
              "FBillNo,FDate,FBillTypeID.FName,FQRCodeStatus,FWhichDoor.FName,FPrintCode", @"(FCustID = \'" + FID + "\' and FQRCodeStatus != \'K\')")).Select(item => new QRBill_WB
              {
                  FBillNo = item[0].ToString(),
                  FDate = item[1].ToString(),
                  FBillType = item[2].ToString(),
                  FQRCodeStatus = item[3].ToString(),
                  FWhichDoor = item[4].ToString(),
                  FPrintCode = item[4].ToString()
              }).ToList<QRBill_WB>();
            return where == null ? ret : ret.Where(where).ToList();

        }

        public static string GetQRBillListWithUserID(string FID)
        {
            if (!ValidateLogin()) return null;

            //默认不查询已完成的单据
            // 已完成单据的判定标准：
            //      条码状态不是完成


            //return JArray.Parse(BatchQuery("kao_TMPGD_KingHoo",
            //    "FBillNo,FBillTypeID,FDate,FBillStatus,FQRCodeStatus,FWhichDoor,FMaterialID,FQty,FUnitID,FStockID,FLocation,FCustID", @"(FCustID = \'" + FID + "\' and FQRCodeStatus != \'K\')")).Select(item => new QRBill
            //    {
            //        FBillNo = item[0].ToString(),
            //        FBillType = item[1].ToString(),
            //        Fdate
            //    }).ToList();
            var ret = JArray.Parse(BatchQuery("kao_TMPGD_KingHoo", "FBillNo,FDate,FBillTypeID.FName,FQRCodeStatus,FWhichDoor.FName,FPrintCode,FStartDate,FEndDate,FWhichDoor.FName,FQRCodeStatus.FCaption,FManNum", "(FCustID = \'" + FID + "\' and FQRCodeStatus != \'K\')")).ToString();

            return ret;

        }
        //查询所有核销单
        public static List<string> queryHXDBills()
        {
            //var ret = BatchQuery("",);
            return null;
        }

        public static bool SaveTMPGDBill(string updatefields, JObject data)
        {
            JObject subJson = new JObject();
            subJson["FID"] = data["Id"];
            subJson["FDate"] = DateTime.Parse(data["FDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FStartDate"] = DateTime.Parse(data["FStartDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FEndDate"] = DateTime.Parse(data["FEndDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FWhichDoor"] = data["FWhichDoor"];
            subJson["FCodeType"] = data["FCodeType"];
            subJson["FInOutTimes"] = data["FInOutTimes"];
            subJson[updatefields] = data[updatefields].ToString();

            JObject jobj = new JObject();
            jobj["NeedUpDateFields"] = new JArray() { updatefields };
            jobj["NeedReturnFields"] = new JArray();
            jobj["IsDeleteEntry"] = true;
            jobj["SubSystemId"] = "";
            jobj["IsVerifyBaseDataField"] = false;
            jobj["IsEntryBatchFill"] = true;
            jobj["ValidateFlag"] = true;
            jobj["NumberSearch"] = true;
            jobj["InterationFlags"] = "";
            jobj["Model"] = subJson;

            var ret = sendRepuest(K3Cloud_AddressType.save, new object[] { "kao_TMPGD_KingHoo", jobj.ToString() }); ;
            if (string.IsNullOrEmpty(ret)) return false;
            return true;
        }

        public static bool SaveTMPGDBillDirect(string updatefields, JObject data)
        {
            JObject subJson = new JObject();
            subJson["FID"] = data["Id"];
            subJson["FDate"] = DateTime.Parse(data["FDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FStartDate"] = DateTime.Parse(data["FStartDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FEndDate"] = DateTime.Parse(data["FEndDate"].ToString()).ToString("yyyy-MM-dd");
            subJson["FWhichDoor"] = data["FWhichDoor"];
            subJson["FCodeType"] = data["FCodeType"];
            subJson["FInOutTimes"] = data["FInOutTimes"];
            subJson["FCarWeightDiff_Head"] = data["FCarWeightDiff_Head"];
            subJson["FInOutWeightEntity"] = data["FInOutWeightEntity"];
            subJson["FTab2_Tab1_Entity"] = data["FTab2_Tab1_Entity"];
            //subJson[updatefields] = data[updatefields].ToString();
            subJson["FhandOutEntity"] = data["FhandOutEntity"];

            JObject jobj = new JObject();
            jobj["NeedUpDateFields"] = new JArray() { };
            jobj["NeedReturnFields"] = new JArray() { };
            jobj["IsDeleteEntry"] = true;
            jobj["SubSystemId"] = "";
            jobj["IsVerifyBaseDataField"] = false;
            jobj["IsEntryBatchFill"] = true;
            jobj["ValidateFlag"] = true;
            jobj["NumberSearch"] = true;
            jobj["InterationFlags"] = "";
            jobj["Model"] = subJson;

            var ret = sendRepuest(K3Cloud_AddressType.save, new object[] { "kao_TMPGD_KingHoo", jobj.ToString() });
            if (string.IsNullOrEmpty(ret)) return false;
            if (JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                return true;
            }
            else
            {
                try
                {
                    m_errorString = JObject.Parse(ret).SelectToken("Result.['ResponseStatus'].['Errors'][0]['Message']").Value<string>();
                    return false;
                }
                catch (Exception exp)
                {
                    ZJF_LOGGER.log(exp.Message);
                    return false;
                }
            }
        }
        public static JObject GetQRBillView(string fbillno)
        {
            if (!ValidateLogin()) return null;
            Login();
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = fbillno;
            pair["Id"] = 0;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "kao_TMPGD_KingHoo", paras });
            if (string.IsNullOrEmpty(ret)) return null;
            var jret = JObject.Parse(ret);
            //登录过期处理
            if (jret.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
            {
                //审核失败
                JObject Jobj = new JObject();
                Jobj["billno"] = fbillno;
                Jobj["state"] = "success";
                Jobj["data"] = jret.SelectToken("Result.['Result']");
                Jobj["message"] = "";
                return Jobj;
            }
            else
            {
                JObject Jobj3 = new JObject();
                Jobj3["billno"] = fbillno;
                Jobj3["state"] = "failed";
                Jobj3["data"] = "";
                Jobj3["message"] = jret.SelectToken("Result.['ResponseStatus'].['Errors'][0]['Message']").Value<string>();
                return Jobj3;
            }
        }

        public static string CreateProcessReport(JObject jobj)
        {
            var ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "SFC_OperationReport", "GenerateOperationReport", jobj });

            return ret;
        }

        public static JObject CreateProcessReportAndAudit(JObject para_jobj)
        {
            try
            {
                var ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "SFC_OperationReport", "GenerateOperationReport", para_jobj });
                var retResult = JObject.Parse(ret);
                if (retResult.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                {
                    //提交单据单据
                    JObject commitObj = new JObject();
                    commitObj["SFC_OperationReport"] = 0;
                    commitObj["Numbers"] = new JArray();
                    commitObj["Ids"] = retResult.SelectToken("Result.['ResponseStatus'].['SuccessEntitys'][0].['Id']").Value<string>();
                    commitObj["SelectedPostId"] = 0;
                    commitObj["NetworkCtrl"] = 0;
                    var commitResult = JObject.Parse(sendRepuest(K3Cloud_AddressType.SubmitPath, new object[] { "SFC_OperationReport", commitObj }));
                    if (commitResult.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                    {
                        var aduitResult = JObject.Parse(sendRepuest(K3Cloud_AddressType.AuditPath, new object[] { "SFC_OperationReport", commitObj }));
                        if (aduitResult.SelectToken("Result.['ResponseStatus'].['IsSuccess']").Value<bool>())
                        {
                            JObject Jobj = new JObject();
                            Jobj["billno"] = para_jobj.SelectToken("Numbers[0]").Value<string>();
                            Jobj["state"] = "success";
                            Jobj["message"] = "";
                            return Jobj;
                        }
                        else
                        {
                            //审核失败
                            JObject Jobj = new JObject();
                            Jobj["billno"] = retResult.SelectToken("Numbers[0]").Value<string>();
                            Jobj["state"] = "failed";
                            Jobj["message"] = commitResult.SelectToken("Result.['ResponseStatus'].['Errors'][0]").Value<string>();
                            return Jobj;
                        }
                    }
                    else
                    {
                        //提交失败
                        JObject Jobj2 = new JObject();
                        Jobj2["billno"] = retResult.SelectToken("Numbers[0]").Value<string>();
                        Jobj2["state"] = "failed";
                        Jobj2["message"] = commitResult.SelectToken("Result.['ResponseStatus'].['Errors'][0]").Value<string>();
                        return Jobj2;
                    }
                }
                else
                {
                    JObject Jobj3 = new JObject();
                    Jobj3["billno"] = para_jobj.SelectToken("Numbers[0]").Value<string>();
                    Jobj3["state"] = "failed";
                    Jobj3["message"] = retResult.SelectToken("Result.['ResponseStatus'].['Errors'][0]['Message']").Value<string>();
                    return Jobj3;
                }
            }
            catch
            {
                return null;
            }

        }


        public static JArray GetOpertionReportWithBillNoAndEntryID(JObject jobj)
        {
            var ret = sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, new object[] { jobj });
            JArray JArr = new JArray();
            JArray RefArr = JArray.Parse(ret);
            try
            {
                foreach (var i in RefArr)
                {
                    var newObj = new JObject();
                    newObj["FTime"] = i[0].ToString();
                    newObj["FBillNo"] = i[1].ToString();
                    newObj["FProcess"] = i[2].ToString();
                    newObj["FQty"] = i[3].ToString();
                    JArr.Add(newObj);
                }
            }
            catch (Exception exp)
            {
                m_errorString = exp.Message;
            }
            return JArr;
        }


        public static bool SetQRBillStatus(string fids, string billstatus, out string retTxt)
        {
            /*
            入库状态：
            0.待入厂

            1.满车待称重
            2.满车待卸货
            3.卸货中
            4.完成卸货
            5.空车待称重

            6.待出厂
            7.已完成
            PDA扫码必须判断顺序，必须按照顺序扫码
            出库状态：
            0.待入厂

            1.空车待称重
            2.空车待装货
            3.装车中
            4.完成装货
            5.满车待称重

            6.待出场
            7.已完成
            */
            string ret = "";
            bool ExeRet = true;
            switch (billstatus)
            {
                case "空车待称重":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Empty_For_Weight", jobj });

                    }
                    break;
                case "空车待装货":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Empty_Wait_For_Load", jobj });
                    }
                    break;
                case "装车中":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Now_Loading", jobj });
                    }
                    break;
                case "完成装货":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Load_finished", jobj });
                    }
                    break;
                case "满车待称重":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Full_for_Weight2", jobj });

                    }
                    break;

                case "满车待卸货":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Full_For_Unload", jobj });
                    }
                    break;
                case "卸货中":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Unloading", jobj });

                    }
                    break;
                case "完成卸货":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Unload_Finished", jobj });
                    }
                    break;
                //case "空车待称重":
                //    {

                //    }
                //    break;
                case "待出厂":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Wait_For_Out", jobj });

                    }
                    break;
                case "已完成":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "finish", jobj });
                    }
                    break;
                case "C":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Empty_For_Weight", jobj });

                    }
                    break;
                case "E":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Empty_Wait_For_Load", jobj });
                    }
                    break;
                case "G":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Now_Loading", jobj });
                    }
                    break;
                case "I":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Load_finished", jobj });
                    }
                    break;
                case "D":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Full_for_Weight2", jobj });

                    }
                    break;

                case "F":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Full_For_Unload", jobj });
                    }
                    break;
                case "H":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Unloading", jobj });

                    }
                    break;
                case "J":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Unload_Finished", jobj });
                    }
                    break;
                //case "空车待称重":
                //    {

                //    }
                //    break;
                case "B":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "Audit_Wait_For_Out", jobj });

                    }
                    break;
                case "K":
                    {
                        JObject jobj = new JObject();
                        jobj["CreateOrgId"] = 0;
                        jobj["Numbers"] = new JArray();
                        jobj["Ids"] = fids;
                        jobj["PkEntryIds"] = new JArray();
                        jobj["NetworkCtrl"] = "";

                        ret = sendRepuest(K3Cloud_AddressType.approvalPath, new object[] { "kao_TMPGD_KingHoo", "finish", jobj });
                    }
                    break;
                default:
                    {
                        ExeRet = false;
                    }
                    break;
            }
            retTxt = ret;
            return ExeRet;
        }

        public static JObject GetDoorControllerView(string fid)
        {
            if (!ValidateLogin()) return null;
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = "";
            pair["Id"] = fid;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "kao_DoorAccessControler", paras });
            if (string.IsNullOrEmpty(ret)) return null;
            return JObject.Parse(ret);
        }

        public static List<WeightBridgeDevice> GetDoorControllerList()
        {
            if (!ValidateLogin()) return null;
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = "kao_WeighBridge";
            pair["FieldKeys"] = "FUUIID,FPROTOCOL,FIP,FTcpPort,FDevice,FName,FNumber,FID";
            pair["FilterString"] = "";
            pair["OrderString"] = "";
            pair["TopRowCount"] = 0;
            pair["StartRow"] = 0;
            pair["Limit"] = 0;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair);
            if (string.IsNullOrEmpty(ret)) return null;
            var jarr = JArray.Parse(ret);
            if (jarr.Count > 0)
            {
                List<WeightBridgeDevice> lis = new List<WeightBridgeDevice>();
                foreach (var item in jarr)
                {
                    var ii = new WeightBridgeDevice()
                    {
                        FUUID = item[0]?.ToString(),
                        FProtocal = item[1]?.ToString(),
                        FIP = item[2]?.ToString(),
                        FPort = Convert.ToUInt16(item[3]?.ToString()),
                        FWeightBridgeType = item[4]?.ToString(),
                        FName = item[5]?.ToString(),
                        FNumber = item[6]?.ToString(),
                        FID = item[7]?.ToString(),
                    };
                    lis.Add(ii);
                }
                return lis;
            }
            else
            {
                return null;
            }
            //return JArray.Parse(ret);
        }
        public class WeightBridgeDevice
        {
            public string FName { get; set; }
            public string FNumber { get; set; }
            public string FID { get; set; }
            public string FUUID { get; set; }
            public string FLocalation { get; set; }
            public string FIP { get; set; }
            public ushort FPort { get; set; }
            public string FProtocal { get; set; }
            public string FWeightBridgeType { get; set; }
            public WeightBridgeDevice()
            {

            }
        }
        public static JObject GetVisitAreaView(string fid)
        {
            if (!ValidateLogin()) return null;
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = "";
            pair["Id"] = fid;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "kao_VisitArea", paras });
            if (string.IsNullOrEmpty(ret)) return null;
            return JObject.Parse(ret);
        }

        public class CarAccessCard
        {
            public string FCardType { get; set; }
            public string FCardTypeName { get; set; }
            public DateTime FStartDate { get; set; }
            public DateTime FEndDate { get; set; }
            public DateTime FOpenDoorLimitStart { get; set; }
            public DateTime FOpenDoorLimitEnd { get; set; }
            public int FLimitTimes { get; set; }
            public string FID { get; set; }
            public string FNumber { get; set; }
        }

        public static List<CarAccessCard> GetPlateNumber(string plateNumber)
        {
            if (!ValidateLogin()) return null;
            List<CarAccessCard> list = new List<CarAccessCard>();
            var ret = BatchQuery("kao_CarAccessCard", "FCardType,FStartDate,FEndDate,FOpenDoorLimitStart,FOpenDoorLimitEnd,FLimitTimes,FCardType.FName,FID,FNumber",
                //"FPlateNumber like '%" + plateNumber + "%' "
                "CHARINDEX(FPlateNumber,'" + plateNumber + "',1) > 0"
                );
            try
            {
                foreach (var i in JArray.Parse(ret))
                {
                    CarAccessCard cac = new CarAccessCard();
                    cac.FCardType = i[0].ToString();
                    cac.FCardTypeName = i[6].ToString();
                    IFormatProvider ifp = new CultureInfo("zh-CN", true);
                    cac.FStartDate = DateTime.Parse(i[1].ToString());
                    cac.FEndDate = DateTime.Parse(i[2].ToString());
                    //cac.FOpenDoorLimitStart = DateTime.ParseExact(i[3].ToString(), "HH:mm:ss", ifp);
                    //cac.FOpenDoorLimitEnd = DateTime.ParseExact(i[4].ToString(), "HH:mm:ss", ifp);
                    cac.FOpenDoorLimitStart = DateTime.Parse(i[3].ToString());
                    cac.FOpenDoorLimitEnd = DateTime.Parse(i[4].ToString());
                    cac.FLimitTimes = int.Parse(i[5].ToString());
                    cac.FID = i[7].ToString();
                    cac.FNumber = i[8].ToString();
                    list.Add(cac);
                }
            }
            catch (Exception exp)
            {
                m_errorString = exp.Message;
            }
            return list;
        }
        public static JObject GetPlateNumberView(string fid)
        {
            if (!ValidateLogin()) return null;
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["CreateOrgId"] = 0;
            pair["Number"] = "";
            pair["Id"] = fid;

            string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.View, new object[] { "kao_CarAccessCard", paras });
            if (string.IsNullOrEmpty(ret)) return null;
            return JObject.Parse(ret);
        }

        public static bool SavePlateNumber(string fields, JObject obj)
        {
            JObject jobj = new JObject();
            jobj.Add(new JProperty("NeedUpDateFields", new JArray()));
            jobj.Add(new JProperty("NeedReturnFields", new JArray()));
            jobj["IsDeleteEntry"] = true;
            jobj["SubSystemId"] = "";
            jobj["IsVerifyBaseDataField"] = false;
            jobj["IsEntryBatchFill"] = true;
            jobj["ValidateFlag"] = true;
            jobj["NumberSearch"] = true;
            jobj["InterationFlags"] = "";

            string enddate = obj["FEndDate"].ToString();
            obj.Remove("FEndDate");
            string fid = obj["Id"].ToString();

            string t2 = obj.ToString().Replace("\"Id\": " + obj["Id"].ToString() + ",", "\"FID\": " + obj["Id"].ToString() + ",");
            string t3 = t2.Replace("\"msterID\": " + obj["msterID"].ToString() + ",", "\"FEndDate\": \"" + enddate + "\",");


            var t4 = JObject.Parse(t3);
            t4["FDate"] = "2021-01-01 00:00:00";
            jobj.Add(new JProperty("Model", t4));

            var ret = sendRepuest(K3Cloud_AddressType.save, new object[] { "kao_CarAccessCard", jobj.ToString() });
            if (string.IsNullOrEmpty(ret)) return false;
            return true;

        }

        public static bool SaveNewQRBillOpenDoorEntry(
            string FID,
            string DoorController,
            string FSettingTimes,
            string FOpenTimes,
            string FBusinessFinish,
            JArray FEntryOrgDate
            )
        {
            if (!ValidateLogin()) return false;
            //Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            JObject pair = new JObject();
            JArray NeedUpDateFields = new JArray();
            pair.Add(new JProperty("NeedUpDateFields", NeedUpDateFields));
            //pair["NeedUpDateFields"] = NeedUpDateFields;
            JArray NeedReturnFields = new JArray();
            pair.Add(new JProperty("NeedReturnFields", NeedReturnFields));
            //pair["NeedReturnFields"] = NeedReturnFields;
            pair["IsDeleteEntry"] = true;
            pair["SubSystemId"] = "";
            pair["IsVerifyBaseDataField"] = false;
            pair["IsEntryBatchFill"] = true;
            pair["ValidateFlag"] = true;
            pair["NumberSearch"] = true;
            pair["InterationFlags"] = "";
            JObject Model = new JObject();
            Model["FID"] = FID;

            JObject entry = new JObject();
            int maxId = 0;
            foreach (var i in FEntryOrgDate)
            {
                if (Convert.ToInt32(i["Id"].ToString()) > maxId)
                {
                    maxId = Convert.ToInt32(i["Id"].ToString());
                }
            }
            entry["Id"] = maxId + 1;

            //entry.Add("", new JProperty(new JObject()));

            entry["FDoorController"] = (new JObject())["FNUMBER"] = GetDoorControllerView(DoorController)["Result"]["Result"];
            entry["FSettingTimes"] = FSettingTimes;
            entry["FOpenTimes"] = FOpenTimes;
            entry["FBusinessFinish"] = FBusinessFinish;
            FEntryOrgDate.Add(entry);

            Model.Add(new JProperty("FDoorOpenTimes", FEntryOrgDate));
            pair.Add(new JProperty("Model", Model));

            //string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.save, new object[] { "kao_TMPGD_KingHoo", pair.ToString() });
            if (string.IsNullOrEmpty(ret)) return false;
            return true;
        }

        public static bool UpdateQRBillOpenDoorEntry(
            string FID,
            JArray FEntryOrgDate
            )
        {
            if (!ValidateLogin()) return false;
            //Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            JObject pair = new JObject();
            JArray NeedUpDateFields = new JArray();
            pair.Add(new JProperty("NeedUpDateFields", NeedUpDateFields));
            //pair["NeedUpDateFields"] = NeedUpDateFields;
            JArray NeedReturnFields = new JArray();
            pair.Add(new JProperty("NeedReturnFields", NeedReturnFields));
            //pair["NeedReturnFields"] = NeedReturnFields;
            pair["IsDeleteEntry"] = true;
            pair["SubSystemId"] = "";
            pair["IsVerifyBaseDataField"] = false;
            pair["IsEntryBatchFill"] = true;
            pair["ValidateFlag"] = true;
            pair["NumberSearch"] = true;
            pair["InterationFlags"] = "";
            JObject Model = new JObject();
            Model["FID"] = FID;
            Model.Add(new JProperty("FDoorOpenTimes", FEntryOrgDate));
            pair.Add(new JProperty("Model", Model));
            //string paras = CustDicToJson(pair);
            var ret = sendRepuest(K3Cloud_AddressType.save, new object[] { "kao_TMPGD_KingHoo", pair.ToString() });
            if (string.IsNullOrEmpty(ret)) return false;
            return true;
        }

        public static bool ValidateLogin(string userName = "", string password = "", string acctid = "", string host = "")
        {
            bool hasChange = false;
            if (host != "" && m_Host_Ip != host)
            {
                hasChange = true;
                m_Host_Ip = host;
            }
            if (acctid != "" && m_Acctid != acctid)
            {
                hasChange = true;
                m_Acctid = acctid;
            }
            if (userName != "" && userName != m_UserName)
            {
                hasChange = true;
                m_UserName = userName;
            }
            if (password != "" && password != m_Password)
            {
                hasChange = true;
                m_Password = password;
            }
            //if (!m_Login_Status || hasChange)
            {
                m_Login_Status = Login();
            }
            return m_Login_Status;
        }

        public static string errorString { get; private set; } = "";
        public static bool Login()
        {
            List<object> para = new List<object>();
            para.Add(m_Acctid);                     //帐套Id
            para.Add(m_UserName);                   //用户名
            para.Add(m_Password);                   //密码
            para.Add(2052);                         //icid
            var para_string = JsonConvert.SerializeObject(para);
            try
            {
                var ret = SyncRequest(K3Cloud_AddressType.Login, para_string);
                if (ret != null)
                {
                    var jobj = JObject.Parse(ret);
                    if (jobj?["LoginResultType"] != null && jobj?["LoginResultType"].ToString() == "1")
                    {
                        m_Login_Status = true;
                    }
                }
                else
                {
                    ZJF.ZJF_LOGGER.log(ret);
                    m_Login_Status = false;
                }
            }
            catch (Exception exp)
            {
                errorString = exp.Message;
                m_Login_Status = false;
            }
            return m_Login_Status;
        }

        public enum Translate_QueryType
        {
            UseVaue = 0,
            UseKey = 1
        }
        public static string translateBillType(string code, Translate_QueryType type = Translate_QueryType.UseKey)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                        {"5fe1a044fdf63f","委外出库"},
                        {"5fe1a05ffdf659","委外入库"},
                        {"5fe1a06dfdf673","工序委外出库"},
                        {"5fe1a07afdf68d","工序委外入库"},
                        {"5fe1a088fdf6a7","采购入库"},
                        {"5fe1a096fdf6c1","销售出库"},
                        {"5fe1a0a4fdf6db","下脚料出库"},
                        {"5fe1a0b2fdf702","销售退货入库"},
                        {"5fe1a0bffdf71c","采购退货出库"},
                        {"5ff91d10a8e8bd","客访"}};
            if (type == Translate_QueryType.UseKey)
            {
                return dic[code];
            }
            else
            {
                var ret = dic.Where(i => i.Value == code).Select(i => i.Key);
                return ret.Count() > 0 ? ret.ElementAt(1) : "";
            }
        }
        public static string translateBillStatus(string code, Translate_QueryType type = Translate_QueryType.UseKey)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                {"Z","暂存" },
                {"A","创建" },
                {"B","审核中" },
                {"C","已审核" },
                {"D","重新审核" }
            };
            if (type == Translate_QueryType.UseKey)
            {
                return dic[code];
            }
            else
            {
                var ret = dic.Where(i => i.Value == code).Select(i => i.Key);
                return ret.Count() > 0 ? ret.ElementAt(1) : "";
            }
        }
        public string translateWhichDoor(string code, Translate_QueryType type = Translate_QueryType.UseKey)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                {"A","物流门" },
                {"B","人流门" }
            };
            if (type == Translate_QueryType.UseKey)
            {
                return dic[code];
            }
            else
            {
                var ret = dic.Where(i => i.Value == code).Select(i => i.Key);
                return ret.Count() > 0 ? ret.ElementAt(1) : "";
            }
        }
        public static string translateQRStatus(string code, Translate_QueryType type = Translate_QueryType.UseKey)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                {"A","待入厂" },
                {"B","待出厂" },
                {"C","已审核" },
                {"D","空车待称重" },
                {"E","满车待称重" },
                {"F","满车待卸货" },
                {"G","装货中" },
                {"H","卸货中" },
                {"I","装货完成" },
                {"J","卸货完成" },
                {"K","完成" }
            };
            if (type == Translate_QueryType.UseKey)
            {
                return dic[code];
            }
            else
            {
                var ret = dic.Where(i => i.Value == code).Select(i => i.Key);
                return ret.Count() > 0 ? ret.ElementAt(1) : "";
            }
        }
        public class Source_Info
        {
            public int FSID { get; set; }
            public int FSEntryID { get; set; }
            public override string ToString()
            {
                return " ( FID = " + FSID.ToString() + " AND FENTRYID = " + FSEntryID.ToString() + " ) ";
            }
        }
        public static JArray GetCGDDHasPushed(List<Source_Info> sou)
        {
            string filter = String.Join(",", from str in sou select str);
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = "SFC_OperationPlanning";
            pair["FieldKeys"] = "FBillNo";
            pair["FilterString"] = filter;
            pair["OrderString"] = "";
            pair["TopRowCount"] = 0;
            pair["StartRow"] = 0;
            pair["Limit"] = 0;
            JArray jarr = null; ;
            try
            {
                jarr = JArray.Parse(sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair));
            }
            catch (Exception exp)
            {
                ZJF_LOGGER.log(exp.Message);

            }
            return jarr;
            //return null;
        }
#if KBT_Project
        public static JArray GetFHTZDPushedInfo(FHTZD ins)
        {
            //string filter = String.Join(",", from str in sou select str);
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = "SAL_DELIVERYNOTICE";
            pair["FieldKeys"] = "FJoinOutQty,FQty,FEntity_FEntryID";
            pair["FilterString"] = ins.Filter;
            pair["OrderString"] = "";
            pair["TopRowCount"] = 0;
            pair["StartRow"] = 0;
            pair["Limit"] = 0;
            JArray jarr = null; ;
            try
            {
                jarr = JArray.Parse(sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair));
            }
            catch (Exception exp)
            {
                ZJF_LOGGER.log(exp.Message);
            }
            return jarr;
        }
#endif
        public static string PushOutStockBill(string entryids)
        {
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["Ids"] = "";
            pair["Numbers"] = new JArray();
            pair["EntryIds"] = entryids;
            pair["RuleId"] = "";
            pair["TargetBillTypeId"] = "";
            pair["TargetOrgId"] = 0;
            pair["TargetFormId"] = "";
            pair["IsEnableDefaultRule"] = "false";
            pair["IsDraftWhenSaveFail"] = "false";
            pair["CustomParams"] = new JObject();

            JArray jarr = null; ;
            try
            {
                jarr = JArray.Parse(sendRepuest(K3Cloud_AddressType.Push, pair));
            }
            catch (Exception exp)
            {
                ZJF_LOGGER.log(exp.Message);
                return "";
            }
            return jarr.ToString();
        }

#if KBT_Project
        public static JArray QueryOutStockBillWidthPushMsg(List<FHTZD> list)
        {
            var filter = string.Join(" OR ", from str in list.Select(i => i.Filter).ToList() select str);
            Dictionary<string, JToken> pair = new Dictionary<string, JToken>();
            pair["FormId"] = "SAL_DELIVERYNOTICE";
            pair["FieldKeys"] = "FJoinOutQty,FQty,FEntity_FEntryID";
            pair["FilterString"] = filter;
            pair["OrderString"] = "";
            pair["TopRowCount"] = 0;
            pair["StartRow"] = 0;
            pair["Limit"] = 0;
            JArray jarr = null; ;
            try
            {
                jarr = JArray.Parse(sendRepuest(K3Cloud_AddressType.ExecuteBillQuery, pair));
            }
            catch (Exception exp)
            {
                ZJF_LOGGER.log(exp.Message);
            }
            return jarr;
        }
#endif
    }

#endif
    #endregion WebApi

    #region Prompt
    public class ZJF_PROMPT
    {
        public static Process m_Prompt = null;
#if __Windows_
        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public int cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        #endregion

        #region Enumerations

        enum TOKEN_TYPE : int
        {
            TokenPrimary = 1,
            TokenImpersonation = 2
        }

        enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3,
        }

        #endregion

        #region Constants

        public const int TOKEN_DUPLICATE = 0x0002;
        public const uint MAXIMUM_ALLOWED = 0x2000000;
        public const int CREATE_NEW_CONSOLE = 0x00000010;

        public const int IDLE_PRIORITY_CLASS = 0x40;
        public const int NORMAL_PRIORITY_CLASS = 0x20;
        public const int HIGH_PRIORITY_CLASS = 0x80;
        public const int REALTIME_PRIORITY_CLASS = 0x100;

        #endregion

        #region Win32 API Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hSnapshot);

        [DllImport("kernel32.dll")]
        static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static bool CreateProcessAsUser(IntPtr hToken, String lpApplicationName, String lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvironment,
            String lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll")]
        static extern bool ProcessIdToSessionId(uint dwProcessId, ref uint pSessionId);

        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public extern static bool DuplicateTokenEx(IntPtr ExistingTokenHandle, uint dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes, int TokenType,
            int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        #endregion

        /// <summary>
        /// Launches the given application with full admin rights, and in addition bypasses the Vista UAC prompt
        /// </summary>
        /// <param name="applicationName">The name of the application to launch</param>
        /// <param name="procInfo">Process information regarding the launched application that gets returned to the caller</param>
        /// <returns></returns>
        public static bool StartProcessAndBypassUAC(String applicationName, out PROCESS_INFORMATION procInfo)
        {
            uint winlogonPid = 0;
            IntPtr hUserTokenDup = IntPtr.Zero, hPToken = IntPtr.Zero, hProcess = IntPtr.Zero;
            procInfo = new PROCESS_INFORMATION();

            // obtain the currently active session id; every logged on user in the system has a unique session id
            uint dwSessionId = WTSGetActiveConsoleSessionId();

            // obtain the process id of the winlogon process that is running within the currently active session
            Process[] processes = Process.GetProcessesByName("winlogon");
            foreach (Process p in processes)
            {
                if ((uint)p.SessionId == dwSessionId)
                {
                    winlogonPid = (uint)p.Id;
                }
            }
            if (winlogonPid == 0)
            {

            }
            // obtain a handle to the winlogon process
            hProcess = OpenProcess(MAXIMUM_ALLOWED, false, winlogonPid);

            // obtain a handle to the access token of the winlogon process
            if (!OpenProcessToken(hProcess, TOKEN_DUPLICATE, ref hPToken))
            {
                CloseHandle(hProcess);
                return false;
            }

            // Security attibute structure used in DuplicateTokenEx and CreateProcessAsUser
            // I would prefer to not have to use a security attribute variable and to just 
            // simply pass null and inherit (by default) the security attributes
            // of the existing token. However, in C# structures are value types and therefore
            // cannot be assigned the null value.
            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);

            // copy the access token of the winlogon process; the newly created token will be a primary token
            if (!DuplicateTokenEx(hPToken, MAXIMUM_ALLOWED, ref sa, (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, (int)TOKEN_TYPE.TokenPrimary, ref hUserTokenDup))
            {
                CloseHandle(hProcess);
                CloseHandle(hPToken);
                return false;
            }

            // By default CreateProcessAsUser creates a process on a non-interactive window station, meaning
            // the window station has a desktop that is invisible and the process is incapable of receiving
            // user input. To remedy this we set the lpDesktop parameter to indicate we want to enable user 
            // interaction with the new process.
            STARTUPINFO si = new STARTUPINFO();
            si.cb = (int)Marshal.SizeOf(si);
            si.lpDesktop = @"winsta0\default"; // interactive window station parameter; basically this indicates that the process created can display a GUI on the desktop

            // flags that specify the priority and creation method of the process
            int dwCreationFlags = NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE;

            // create a new process in the current user's logon session
            bool result = CreateProcessAsUser(hUserTokenDup,        // client's access token
                                            null,                   // file to execute
                                            applicationName,        // command line
                                            ref sa,                 // pointer to process SECURITY_ATTRIBUTES
                                            ref sa,                 // pointer to thread SECURITY_ATTRIBUTES
                                            false,                  // handles are not inheritable
                                            dwCreationFlags,        // creation flags
                                            IntPtr.Zero,            // pointer to new environment block 
                                            null,                   // name of current directory 
                                            ref si,                 // pointer to STARTUPINFO structure
                                            out procInfo            // receives information about new process
                                            );

            // invalidate the handles
            CloseHandle(hProcess);
            CloseHandle(hPToken);
            CloseHandle(hUserTokenDup);

            return result; // return the result
        }
        public static string errorString { get; private set; }
        public delegate void UdpReciveFromMessage(string Data);
        public static event UdpReciveFromMessage OnUdpReciveFromMessage = null;
        public static bool ShowPrompt(string appPath, string content ,string title ,int delay ,string defaultButton,string clientuuid )
        {
#if __Windows_

            if (!ZJF.ZJF_UDP.m_Socket_Server_Running)
            {
                ZJF.ZJF_UDP udp = new ZJF_UDP(UDP_TYPE.UDP_SERVER);
                udp.OnreciveFrom += (string txt) => {
                    try
                    {
                        JObject json = JObject.Parse(txt);
                        OnUdpReciveFromMessage?.Invoke(json.ToString());
                        ZJF_LOGGER.log(json.ToString());
                    }
                    catch (Exception exp)
                    {
                        errorString = exp.Message;
                    }
                };
            }

            if (m_Prompt == null || (m_Prompt!=null && m_Prompt.HasExited))
            {
                ZJF_PROMPT.PROCESS_INFORMATION procInfo;
                ZJF_PROMPT.StartProcessAndBypassUAC(appPath + " " + content + " " + title + " " + delay.ToString() + " " + defaultButton + " " + clientuuid, out procInfo);
                m_Prompt = Process.GetProcessById((int)procInfo.dwProcessId);
                return true;
            }
            else
            {
                return false;
            }
#endif
            
        }
        public static void KillPrompt()
        {
            if (m_Prompt != null && !m_Prompt.HasExited)
            {
                m_Prompt.Kill();
                m_Prompt = null;
            }
            else
            {
                m_Prompt = null;
            }
        }
#endif
    }
    #endregion Prompt

    public class ZJF_WEIGHBRIDGE
    {

    }
    public class ZJF_ENTRANCE
    {

    }
#if __JSON_
    public class ZJF_PIPE
    {
        NamedPipeServerStream m_Server = null;
        NamedPipeClientStream m_Client = null;
        private bool m_Server_Running = false;
        //private bool m_Client_Running = false;
        public string errorString { get; private set; }
        public delegate void UserReply(string clickButton);
        public event UserReply OnUserReply;

        public void ServerStart(string ServerName)
        {
            m_Server = new NamedPipeServerStream(ServerName, PipeDirection.InOut, 30);
            new Thread(() =>
            {
                try
                {
                    m_Server_Running = true;
                    m_Server.WaitForConnection();
                    JObject json = null;
                    var reader = new StreamReader(m_Server);
                    while (m_Server_Running)
                    {
                        var ret = reader.ReadLine();
                        if (!string.IsNullOrEmpty(ret))
                        {
                            try
                            {
                                json = JObject.Parse(ret);
                            }
                            catch (Exception exp)
                            {
                                errorString = exp.Message;
                            }
                            if (json["Data"]["UserClickButton"].ToString() == "")
                            {
                                OnUserReply?.Invoke(json["Data"]["UserClickButton"].ToString());
                                break;
                            }
                        }
                    }

                }
                catch (Exception exp)
                {
                    errorString = exp.Message;
                }
                finally
                {
                    m_Server.Close();
                }
            })
            { IsBackground = true }.Start();



        }
        public void connect(string namePipe, string content)
        {
            m_Client = new NamedPipeClientStream("localhost", namePipe, PipeDirection.InOut);
            try
            {
                //m_Client_Running = true;
                m_Client.Connect(5000);
                new Thread(() =>
                {
                    using (var writer = new StreamWriter(m_Client))
                    {
                        JObject main = new JObject();
                        JObject subNode = new JObject();
                        main.Add("Type", "Reply");
                        main.Add("Result", "");
                        main.Add("Des", "");
                        main.Add("Code", 1);
                        subNode.Add("UserClickButton", content);
                        main.Add(new JProperty("Data", subNode));
                        writer.Write(main.ToString());

                    }
                })
                { IsBackground = true }.Start();
            }
            catch (Exception exp)
            {
                errorString = exp.Message;
            }
            finally
            {
                m_Client.Close();
            }
        }
    }
#endif
    public enum UDP_TYPE
    {
        UDP_SERVER = 0,
        UDP_CLIENT = 1,
        UDP_EMPTY = 3
    }
    public class ZJF_UDP
    {
        public static bool m_Socket_Server_Running = false;
        Socket m_socket = null;
        IPEndPoint m_iep = null;
        public delegate void reciveFrom(string Txt);
        public event reciveFrom OnreciveFrom;
        public bool m_Running { get; private set; } = false;
        private UDP_TYPE m_Type = UDP_TYPE.UDP_EMPTY;
        public ZJF_UDP(UDP_TYPE type)
        {
            m_Type = type;
            if (type == UDP_TYPE.UDP_SERVER)
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_iep = new IPEndPoint(IPAddress.Any, 3039);
                m_Socket_Server_Running = true;
                m_socket.Bind(m_iep);
            }
            if (type == UDP_TYPE.UDP_CLIENT)
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3039);
            }
            new Thread(recivefrom) { IsBackground = true }.Start(m_socket);
            m_Running = true;
        }
        public int sendTo(string txt)
        {
            if (m_Type == UDP_TYPE.UDP_CLIENT)
            {
                var ep = (EndPoint)m_iep;
                byte[] buffer = Encoding.ASCII.GetBytes(txt);
                return m_socket.SendTo(buffer, buffer.Length, SocketFlags.None, ep);
            }
            return 0;
        }
        private void recivefrom(object socket)
        {
            if (m_Type == UDP_TYPE.UDP_SERVER)
            {
                var _socket = socket as Socket;
                IPEndPoint ciep = new IPEndPoint(IPAddress.Any, 3039);
                byte[] ret = new byte[1024 * 10];
                var ep = (EndPoint)ciep;
                var c = _socket.ReceiveFrom(ret, ref ep);
                var _result = Encoding.ASCII.GetString(ret, 0, c);
                if (!string.IsNullOrEmpty(_result))
                {
                    OnreciveFrom?.Invoke(_result);
                }
                close(_socket);
            }
        }

        private void close(Socket socket)
        {
            socket.Close();
#if _CRT_NEW_
            socket.Dispose();
#endif
            m_Socket_Server_Running = false;
        }
    }
#if ZJF_MSSQL
    public class ZJF_MSSQL
    {
        //public static SqlConnection m_sql_ = new SqlConnection("Data Source=(local);Initial Catalog=master;Integrated Security=SSPI;");
        const string localdataSource = "Data Source=(local);Initial Catalog=master;Integrated Security=SSPI;";
        private string netDataSource,testDataSource;
        private bool testPassed = false;
        private bool connectionOK = false;
        public static string errorString { get; private set; } = "";
        public static DataTable CheckQRBill(string sql)
        {
            using (SqlConnection sqlcon = new SqlConnection(localdataSource))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, sqlcon);
                    sqlcon.Open();
                    var reader = command.ExecuteReader();
                    DataTable ret = new DataTable();
                    ret.Load(reader);
                    return ret;
                }
                catch (SqlException sexp)
                {
                    errorString = sexp.Message;
                }
            }
            return null;
        }
        public ZJF_MSSQL(string host,string db,string username,string password)
        {
            testDataSource = "Data Source=" + host + ";Initial Catalog=master;User Id =" + username + ";Password =" + password;
            netDataSource = "Data Source=" + host + ";Initial Catalog=" + db + ";User Id =" + username + ";Password =" + password;
        }
        //cost little time so better run this function on another thread
        public bool connectTest()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(testDataSource))
                {
                    testPassed = true;
                    using (SqlConnection sqlconMian = new SqlConnection(netDataSource))
                    {
                        connectionOK = true;
                    }
                }
            }
            catch (SqlException sexp)
            {
                Debug.WriteLine(sexp.Message);
            }
            {
                if(testPassed && connectionOK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ifTableExist(string tableName)
        {
            using (SqlConnection sqlconMian = new SqlConnection(netDataSource))
            {
                try
                {
                    sqlconMian.Open();
                    SqlCommand command = new SqlCommand("select 1 from sys.tables where name like '" + tableName + "'", sqlconMian);

                    var reader = command.ExecuteReader();
                    if(reader.RecordsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException sexp)
                {
                    errorString = sexp.Message;
                    return false;
                }
            }
        }

        public DataTable getItemTable()
        {
            try
            {
                using (SqlConnection sqlconMian = new SqlConnection(netDataSource))
                {
                    string sql = "SELECT FCLASSTYPEID,FNUMBER,FNAME,FUNIQUEFEATURES,FDELETED,FSYNC,FUPDATE FROM T_ITEM WHERE FSYNC=1";
                    sqlconMian.Open();
                    SqlCommand command = new SqlCommand(sql, sqlconMian);
                    var reader = command.ExecuteReader();
                    DataTable ret = new DataTable();
                    ret.Load(reader);
                    return ret;
                }
            }catch(SqlException sexp)
            {
                Debug.WriteLine(sexp.Message);
                return null;
            }
        }
        public bool isBaseitemUpdate()
        {
            using (SqlConnection sqlconMian = new SqlConnection(netDataSource))
            {
                try
                {
                    
                    if (ifTableExist("T_Item"))
                    {
                        sqlconMian.Open();
                        SqlCommand command = new SqlCommand("SELECT 1 FROM T_Item WHERE FSync = 1", sqlconMian);

                        var reader = command.ExecuteReader();
                        if(reader.RecordsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException sexp)
                {
                    errorString = sexp.Message;
                    return false;
                }
            }
        }
    }
#endif
    public class ZJF_INIREADER
    {
        public string inipath;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>  
        /// 构造方法  
        /// </summary>  
        /// <param name="iniFileName">文件路径,文件不需要带后缀</param>  
        public ZJF_INIREADER(string iniFileName)
        {
            inipath = AppDomain.CurrentDomain.BaseDirectory + iniFileName + ".ini";
        }


        /// <summary>  
        /// 写入Ini文件  
        /// </summary>  
        /// <param name="Section">项目名称(如 [TypeName] )</param>  
        /// <param name="Key">键</param>  
        /// <param name="Value">值</param>  
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符。</returns>  
        public long IniWriteValue(string Section, string Key, string Value)
        {
            return WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary>  
        /// 读出INI文件  
        /// </summary>  
        /// <param name="Section">项目名称(如 [TypeName] )</param>  
        /// <param name="Key">键</param>  
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary>  
        /// 验证文件是否存在  
        /// </summary>  
        /// <returns>布尔值</returns>  
        public bool ExistIniFile()
        {
            return File.Exists(inipath);
        }
    }
#if __ZJF_COMM_
    public class ZJF_JSONFORMATTER {
        public enum json_Type
        {
            WWOUT = 0,                  //"委外出库"
            WWIN = 1,                   //委外入库
            GXWWOUT = 2,                //工序委外出库
            GXWWIN = 3,                 //工序委外入库
            CGIN = 4,                   //采购入库
            XSOUT = 5,                  //销售出库
            XJLOUT = 6,                 //下脚料出库
            XSTHIN = 7,                 //销售退货入库
            CGTHOUT = 8,                //采购退货出库
            Auth = 9,                   //验证请求
            RequestDic = 10,            //请求字典
            Reply = 11,                 //用户操作返回
            OpenDoorReply = 12,         //the reply for open door request
            OpenDoorPrompt = 13,        //end request for open the door
            other = 14
        }
        public enum json_Result
        {
            Error = 0,                  //错误
            Failed = 1,                 //失败
            Success = 2,                //成功
            Refuse = 3,                 //拒绝
            NoResponse = 4,             //未响应
            DefaultSuccess = 5,         //无响应默认成功
            DefaultFailed = 6,          //无响应默认失败
            Other = 7
        }
        public enum json_OperateCode
        {
            Success = 1,
            Failed = 2,
            Error = 3


        }
        public static Dictionary<int, string> MessageMap = new Dictionary<int, string>(){
            {1,"操作成功"},
            {2,"操作失败"},
            {3,"发生错误"},
            {4,""},
            {5,""},
            {6,""},
            {7,""},
            {8,""},
            {9,""},
            {10,""},
            {11,""},
            {12,""},
            };

        public class json_Class
        {
            public json_Type _Type { get; set; }
            public ClientType _ClientType { get; set; }
            public json_Result _Result { get; set; }
            public string _ClientUUID { get; set; }
            public string _Des { get; set; }
            public json_OperateCode _Code { get; set; }
            public string _AuthCode { get; set; }
#if __ANDROID__
            public QRBill _QRBill { get; set; }
#endif

        }

        public static JObject StandardJson(json_Class json)
        {
            JObject _mainNode = new JObject();
            _mainNode.Add("IsStandardJson", "{A3785A0C-0DE2-4BE6-A952-29BDEEAEECF6}");
            _mainNode.Add("Type", (int)json._Type);
            _mainNode.Add("ClientType", (int)json._ClientType);
            _mainNode.Add("Result", (int)json._Result);
            _mainNode.Add("ClientUUID", json._ClientUUID);
            _mainNode.Add("Des", json._Des);
            _mainNode.Add("Code", (int)json._Code);
            _mainNode.Add("AuthCode", json._AuthCode);
            JObject _QRBillNode = new JObject();
#if __ANDROID__
            _QRBillNode.Add("FBillNo", json._QRBill.FBillNo);
            _QRBillNode.Add("FBillType", json._QRBill.FBillType);
            _QRBillNode.Add("FBusinessDate", json._QRBill.FBusinessDate);
            _QRBillNode.Add("FBillStatus", json._QRBill.FBillStatus);
            _QRBillNode.Add("FQRBillExcuteStatus", json._QRBill.FQRBillExcuteStatus);
            _QRBillNode.Add("FDoorOpenType", json._QRBill.FDoorOpenType);
#endif
            _mainNode.Add(new JProperty("QRBill", _QRBillNode));


            return _mainNode;
        }

        public static bool IsStandardJson(string json)
        {
            try
            {
                JObject ret = JObject.Parse(json);
                if(ret.ContainsKey("IsStandardJson") && ret["IsStandardJson"].ToString() == "{A3785A0C-0DE2-4BE6-A952-29BDEEAEECF6}")
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
#endif
    public class ZJF_QRBillChecker
    {

    }
#if __JSON_
    public class ZJF_SCANNER
    {
        public class QRBill_QRMsg
        {
            public string FBillNo { get; set; }
            public string FBillType { get; set; }
            public string IsStandardJson { get; private set; } = "{C57FC020-0DA7-4FCB-84F5-1AEF308BD268}";
        }
        public static bool IsStandardJson(string txt)
        {
            try
            {
                JObject ret = JObject.Parse(txt);
                if (ret["IsStandardJson"] != null && ret["IsStandardJson"].ToString() == "{C57FC020-0DA7-4FCB-84F5-1AEF308BD268}")
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        public static QRBill_QRMsg GetQRMsg(string txt)
        {
            QRBill_QRMsg ret = null;
            if (IsStandardJson(txt))
            {
                var temp = JObject.Parse(txt);
                ret = new QRBill_QRMsg();
                ret.FBillNo = temp["FBillNo"].ToString();
                ret.FBillType = temp["FBillType"].ToString();
            }
            return ret;
        }
    }
#endif
    public class ZJF_SQLiteHelper//<T> where T : class, IObject, new()
    {
#if __ANDROID__


        //public static string FilePath { get; private set; } = "";
        //private static SQLiteConnection m_db = null;
        //private static bool m_init = false;
        //private static object _Mutex = new object();
        //public static string errorString { get; private set; } = ""; 
        //public static bool Init(string path)
        //{
        //    try
        //    {
        //        FilePath = /*"KingHoo_App.db3"*/path == ""? path: FilePath;
        //        if(string.IsNullOrEmpty(FilePath))
        //        {
        //            return false;
        //        }
        //        if (FilePath == "") return false;
        //        string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
        //        var dbPath = Path.Combine(libraryPath, FilePath);
        //        m_db = new SQLiteConnection(dbPath);
        //        m_init = true;
        //    }
        //    catch (SQLiteException slex)
        //    {
        //        errorString = slex.Message;
        //        return false;
        //    }
        //    return true;
        //}
        //public static bool tableExist<T>()
        //{
        //    if (!m_init)
        //        if (!init(FilePath))
        //        {
        //            //System.Diagnostics.Debug.Assert(m_init);
        //            return false;
        //        }
        //    var ret = true;
        //    lock (_Mutex)
        //    {
        //        try
        //        {
        //            var info = m_db.GetTableInfo(typeof(T).Name);
        //            if (!info.Any())
        //            {
        //                ret = false;
        //            }
        //        }
        //        catch (SQLiteException slex)
        //        {
        //            errorString = slex.Message;
        //            ret = false;
        //        }
        //    }
        //    return ret;
        //}
#endif
    }
    public class ZJF_InternetControl
    {

    }
    public class ZJF_SignInControl
    {

    }
    //save app running state 
    public class ZJF_StateKeeper
    {

    }
    public class ZJF_ConstString
    {
        public const System.String SCAN_ACTION = "android.scanservice.action.UPLOAD_BARCODE_DATA";
        public const System.String U8000S = "scan.rcv.message";
    }
    public class ZJF_IniFile
    {
        public string path { get; private set; }             //INI文件名  

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key,
                    string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def,
                    StringBuilder retVal, int size, string filePath);

        //声明读写INI文件的API函数  
        public ZJF_IniFile(string INIPath)
        {
            path = INIPath;
        }

        //类的构造函数，传递INI文件名  
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        //写INI文件  
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }

        //读取INI文件指定  
    }
#if __TTS_
    public class ZJF_TTS
    {
        private static object m_locker = new object();
        public static bool m_OnProcessing { get; private set; }
        public static void Speak(string saywords,string savePath = "")
        {
            if(!m_OnProcessing)
            new Thread(() => {
                try
                {
                    lock (m_locker)
                        {
                        m_OnProcessing = true;
                            var synthesizer = new SpeechSynthesizer();
                            if (savePath != "")
                            {
                                synthesizer.SetOutputToWaveFile(savePath);
                            }
                            synthesizer.SetOutputToDefaultAudioDevice();
                            synthesizer.Speak(saywords);
                            if (savePath != "")
                            {
                                synthesizer.SetOutputToDefaultAudioDevice();
                            }
                            ZJF_LOGGER.log("语音正常播放！" + saywords);
                            synthesizer.SetOutputToDefaultAudioDevice();
                            synthesizer.Dispose();
                        m_OnProcessing = false;
                        }
                   
                }
                catch (Exception exp)
                {
                    ZJF_LOGGER.log("播放语音错误！" + exp.Message);
                }

            })
            { IsBackground = true }.Start();
        }
    }
#endif
#if __K3_Cloud_WebApi_
    public class ZJF_StandWebApi
    {
    #region Member
        /// <summary>
        /// 银行
        /// </summary>
        public const string HISBANK_FormId = "ABCD_HISBANK";
        /// <summary>
        /// 部门
        /// </summary>
        public const string HISDEP_FormId = "ABCD_HISDEP";
        /// <summary>
        /// 收入列类别
        /// </summary>
        public const string HISINCOMETYPE_FormId = "ABCD_HISINCOMETYPE";
        /// <summary>
        /// 医保类型
        /// </summary>
        public const string HISYLSUBSIDY_FormId = "ABCD_HISMSTYPE";
        /// <summary>
        /// 药品类别
        /// </summary>
        public const string HISDRUGCATEGORY_FormId = "ABCD_HISDT";
        /// <summary>
        /// 药库
        /// </summary>
        public const string HISPHARMAC_FormId = "ABCD_HISSDS";

        public const string His_Dep_DbTable = "ABCD_t_Cust100003";
        public const string His_Bank_DbTable = "ABCD_t_Cust100002";
        public const string His_Income_DbTable = "ABCD_t_Cust100004";
        public const string His_YBSubsidy_DbTable = "ABCD_t_Cust100005";
        public const string His_DrugCategory_DbTable ="ABCD_t_Cust100006";
        public const string His_Pharmac_DbTable = "ABCD_t_Cust100008";
        public static class ItemClass
        {
            public static string Bank { get; private set; } = "银行";
            public static string Department { get; private set; } = "部门";
            public static string IncomeType { get; private set; } = "收入类别";
            public static string YBSubsidy { get; private set; } = "医保报销";
            public static string DrugCategory { get; private set; } = "药品类别";
            public static string Pharmac { get; private set; } = "药库";
        }
        public static class HisItemClass
        {
            public static string Bank { get; private set; } = "银行";
            public static string Department { get; private set; } = "部门";
            public static string IncomeType { get; private set; } = "收入类别";
            public static string YBSubsidy { get; private set; } = "医保报销";
            public static string DrugCategory { get; private set; } = "药品类别";
            public static string Pharmac { get; private set; } = "药库";
        }

        public const string Check_His_Dep_DbTable = "SELECT * FROM " + His_Dep_DbTable +  " WHERE FMAPDEP = 0 ";
        public const string Check_His_Bank_DbTable = "SELECT * FROM " + His_Bank_DbTable + " WHERE FMAPBANK = 0 ";
        public const string Check_His_Income_DbTable = "SELECT * FROM " + His_Income_DbTable + " WHERE FMAPACCOUNT = 0 ";
        public const string Check_His_YBSubsidy_DbTable = "SELECT * FROM " + His_YBSubsidy_DbTable + " WHERE FMAPACCOUNT = 0 ";
        public const string Check_His_DrugCategory_DbTable = "SELECT * FROM " + His_DrugCategory_DbTable + " WHERE FMAPACCOUNT = 0 ";
        public const string Check_His_Pharmac_DbTable = "SELECT * FROM " + His_Pharmac_DbTable + " WHERE FMAPACCOUNT = 0 ";
    #endregion


        public class ConnectSetting
        {
            public string m_url { get; set; } = "";
            public string m_userName { get; set; } = "";
            public string m_password { get; set; } = "";
            public string m_db { get; set; } = "";
            public int m_lcid { get; set; } = 2052;
            public bool m_connectState { get; set; } = false;
        }
        //HIS
        public class StandardHisItem
        {
            public JArray NeedUpDateFields                  { get; set; } = new JArray();
            public JArray NeedReturnFields                  { get; set; } = new JArray();
            public bool IsDeleteEntry                       { get; set; } = true;
            public string SubSystemId                       { get; set; } = "";
            public bool IsVerifyBaseDataField               { get; set; } = false;
            public bool IsEntryBatchFill                    { get; set; } = true;
            public bool ValidateFlag                        { get; set; } = true;
            public bool NumberSearch                        { get; set; } = true;
            public string InterationFlags                   { get; set; } = "";

            //entry

            public int FID { get; set; }
            public string FNumber { get; set; }
            public string FName { get; set; }

            public override string ToString()
            {
                var json = new JObject();
                json.Add("NeedUpDateFields", NeedUpDateFields);
                json.Add("NeedReturnFields", NeedReturnFields);
                json.Add("IsDeleteEntry", IsDeleteEntry);
                json.Add("SubSystemId", SubSystemId);
                json.Add("IsVerifyBaseDataField", IsVerifyBaseDataField);
                json.Add("IsEntryBatchFill", IsVerifyBaseDataField);
                json.Add("ValidateFlag", IsVerifyBaseDataField);
                json.Add("NumberSearch", IsVerifyBaseDataField);
                json.Add("InterationFlags", IsVerifyBaseDataField);

                var Model = new JObject();
                Model.Add("FID", FID);
                Model.Add("FNumber", FNumber);
                Model.Add("FName", FName);

                json.Add(new JProperty("Model",Model));

                return json.ToString();

            }
        }
        static ConnectSetting m_cs = new ConnectSetting();
        static K3CloudApiClient m_client = null;
        static string m_errorString = "";
        private static void readSetting()
        {
            var ini = new ZJF_INIREADER("setting");
            m_cs.m_userName = ini.IniReadValue("Main", "userName");
            m_cs.m_password = ini.IniReadValue("Main", "password");
            m_cs.m_url = ini.IniReadValue("Main", "url");
            m_cs.m_db = ini.IniReadValue("Main", "db");
        }
        public static bool Init()
        {
            if(m_cs.m_userName == "" || m_cs.m_url == "" || m_cs.m_db == "")
            {
                readSetting();
            }
            if (!m_cs.m_connectState)
            {
                try
                {
                    m_client = new K3CloudApiClient(m_cs.m_url);
                    var loginResult = m_client.ValidateLogin(m_cs.m_db, m_cs.m_userName, m_cs.m_password, 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    if (resultType == 1)
                    {
                        m_cs.m_connectState = true;
                        return m_cs.m_connectState;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception exp)
                {
                    m_errorString = exp.Message;
                    return false;
                }
            }
            else{
                return m_cs.m_connectState;
            }
        }
        public static bool saveHisItem(string FormId, StandardHisItem Json)
        {
            if (Init())
            {
                var ret = m_client.Save(FormId, Json.ToString());
                return true;
            }
            else
            {
                //初始化失败
                return false;
            }
            
        }
        public static int QueryMaxFID(string FormId)
        {
            if (Init())
            {
                JObject jobj = new JObject();
                jobj.Add("FormId", FormId);
                jobj.Add("FieldKeys", "FID");
                jobj.Add("FilterString","");
                jobj.Add("OrderString", "FID DESC");
                jobj.Add("TopRowCount", 1);
                jobj.Add("StartRow", 1);
                jobj.Add("Limit", 1);

                var ret =  m_client.ExecuteBillQuery(jobj.ToString());
                return Convert.ToInt32(ret[0][0]);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 检测基础资料是否设置映射，都设置返回true,存在未设置返回false
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool IfAllItemSetMapping(Kingdee.BOS.Context context,string sql)
        {
            var ret = Kingdee.BOS.App.Data.DBUtils.Execute(context, "/*dialect*/" + sql);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static class BillClass
        {
            /// <summary>
            /// 预收单
            /// </summary>
            public static string YSD { get; private set; } = "预收单";
            /// <summary>
            /// 结算单
            /// </summary>
            public static string JSD { get; private set; } = "结算单";
            /// <summary>
            /// 收款单
            /// </summary>
            public static string SKD { get; private set; } = "收款单";
            /// <summary>
            /// 付款单
            /// </summary>
            public static string FKD { get; private set; } = "付款单";
            /// <summary>
            /// 退款单
            /// </summary>
            public static string TKD { get; private set; } = "退款单";
            /// <summary>
            /// 入库单
            /// </summary>
            public static string RKD { get; private set; } = "入库单";
            /// <summary>
            /// 调拨单
            /// </summary>
            public static string DBD { get; private set; } = "调拨单";
            /// <summary>
            /// 出库单
            /// </summary>
            public static string CKD { get; private set; } = "出库单";
        }
        public static class BillTypeClass
        {
            /// <summary>
            /// 门诊预收单
            /// </summary>
            public static string YSD_MZYSD { get; private set; } = "门诊预收单";
            /// <summary>
            /// 住院预收单
            /// </summary>
            public static string YSD_ZYYSD { get; private set; } = "住院预收单";
            /// <summary>
            /// 门诊结算单
            /// </summary>
            public static string JSD_MZJSD { get; private set; } = "门诊结算单";
            /// <summary>
            /// 住院结算单
            /// </summary>
            public static string JSD_ZYJSD { get; private set; } = "住院结算单";
            /// <summary>
            /// 门诊收款单
            /// </summary>
            public static string SKD_MZSKD { get; private set; } = "门诊收款单";
            /// <summary>
            /// 住院押金收款单
            /// </summary>
            public static string SKD_ZYYJSKD { get; private set; } = "住院押金收款单";
            /// <summary>
            /// 门诊退款单
            /// </summary>
            public static string TKD_MZTKD { get; private set; } = "门诊退款单";
            /// <summary>
            /// 门诊医保报销单
            /// </summary>
            public static string FKD_MZYBBXD { get; private set; } = "门诊医保报销单";
            /// <summary>
            /// 采购入库单
            /// </summary>
            public static string RKD_CGRKD { get; private set; } = "采购入库单";
            /// <summary>
            /// 盘盈入库单
            /// </summary>
            public static string RKD_PYRKD { get; private set; } = "盘盈入库单";
            /// <summary>
            /// 调价入库单
            /// </summary>
            public static string RKD_TJRKD { get; private set; } = "调价入库单";
            /// <summary>
            /// 销售出库单
            /// </summary>
            public static string CKD_XSCKD { get; private set; } = "销售出库单";
            /// <summary>
            /// 职工取药出库单
            /// </summary>
            public static string CKD_ZGQYCKD { get; private set; } = "职工取药出库单";
            /// <summary>
            /// 发各科室出库单
            /// </summary>
            public static string CKD_ZFGKSCKD { get; private set; } = "发各科室出库单";
            /// <summary>
            /// 盘亏出库单
            /// </summary>
            public static string CKD_PKCKD { get; private set; } = "盘亏出库单";
            /// <summary>
            /// 调价出库单
            /// </summary>
            public static string CKD_TJCKD { get; private set; } = "调价出库单";
            /// <summary>
            /// 一般调拨单
            /// </summary>
            public static string DBD_YBDBD { get; private set; } = "一般调拨单";

        }
        public static class BillFormId
        {
            /// <summary>
            /// 预收单
            /// </summary>
            public static string YSD_FormId { get; private set; } = "ABCD_HISAdvanceReceiptBill";
            /// <summary>
            /// 收款单
            /// </summary>
            public static string SKD_FormId { get; private set; } = "ABCD_SKD";
            /// <summary>
            /// 退款单
            /// </summary>
            public static string TKD_FormId { get; private set; } = "ABCD_TKD";
            /// <summary>
            /// 结算单
            /// </summary>
            public static string JSD_FormId { get; private set; } = "ABCD_HISJSD";
            /// <summary>
            /// 付款单
            /// </summary>
            public static string FKD_FormId { get; private set; } = "ABCD_FKD";
            /// <summary>
            /// 入库单
            /// </summary>
            public static string RKD_FormId { get; private set; } = "ABCD_RKD";
            /// <summary>
            /// 出库单
            /// </summary>
            public static string CKD_FormId { get; private set; } = "ABCD_CKD";
            /// <summary>
            /// 调拨单
            /// </summary>
            public static string DBD_FormId { get; private set; } = "ABCD_DBD";
        }
#if __His_
        //public string Format_YSD_Json(DateTime dt,)
        //{
        //    var json ="{\"NeedUpDateFields\":[],\"NeedReturnFields\":[],\"IsDeleteEntry\":\"true\",\"SubSystemId\":\"\",\"IsVerifyBaseDataField\":\"false\",\"IsEntryBatchFill\":\"true\",\"ValidateFlag\":\"true\",\"NumberSearch\":\"true\",\"InterationFlags\":\"\",\"Model\":{\"FID\":0,\"FDate\":\"" + dt.ToString("yyyy-MM-ss") + "\",\"FOrgId\":{\"FNumber\":\"\"},\"FEntity\":[{\"FEntryID\":0,\"FHisDate\":\"1900-01-01\",\"FAmount\":0,\"FHISBank\":{\"FNUMBER\":\"\"},\"FNote\":\"\",\"FSettleType\":{\"FNUMBER\":\"\"},\"FBank\":{\"FNUMBER\":\"\"}}]}}";
        //    return json;
        //}

       
        public static bool saveHisBill_Income(string FormId , ImportDataFormHis.ZXYY.HisBusiness_Income hb)
        {
            bool state = false;
            try
            {
                switch (hb.FBUSINESSTYPE)
                {
                    case "预收单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.YSD_MZYSD)
                            {
                                //var ret = m_client.Save(BillFormId.YSD_FormId,"");

                            }
                            if (hb.FBILLTYPE == BillTypeClass.YSD_ZYYSD)
                            {

                            }
                            break;
                        }
                    case "结算单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.JSD_MZJSD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.JSD_ZYJSD)
                            {

                            }
                            break;
                        }
                    case "收款单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.SKD_MZSKD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.SKD_ZYYJSKD)
                            {

                            }
                            break;
                        }
                    case "付款单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.FKD_MZYBBXD)
                            {

                            }
                            break;
                        }
                    case "退款单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.TKD_MZTKD)
                            {

                            }
                            break;
                        }
                }
                return state;
            }catch(Exception exp)
            {
                m_errorString = exp.Message;
                return false;
            }
        }
        public static bool saveHisBill_Stock(string FormId, ImportDataFormHis.ZXYY.HisBusiness_Stock hb)
        {
            bool state = false;
            try
            {
                switch (hb.FBUSINESSTYPE)
                {
                    case "入库单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.RKD_CGRKD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.RKD_PYRKD)
                            {

                            }
                            break;
                        }
                    case "调拨单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.DBD_YBDBD)
                            {

                            }
                            break;
                        }
                    case "出库单":
                        {
                            if (hb.FBILLTYPE == BillTypeClass.CKD_PKCKD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.CKD_XSCKD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.CKD_ZFGKSCKD)
                            {

                            }
                            if (hb.FBILLTYPE == BillTypeClass.CKD_ZGQYCKD)
                            {

                            }
                            break;
                        }
                }
                return state;
            }
            catch (Exception exp)
            {
                m_errorString = exp.Message;
                return false;
            }
        }


    }
#endif
#endif

    public class ZJF_SendVoiceTxt
    {
        static string Ip;
        static int Port;
        public static void init(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
        public static bool send(string txt)
        {
            bool ret = false;
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Ip), Port);
            try
            {
                client.Connect(ep);
                var recbyteNum = client.Send(Encoding.Unicode.GetBytes(txt));
                if (recbyteNum > 0)
                    ret = true;
            }
            catch (SocketException sexp)
            {
                ZJF.ZJF_LOGGER.log("发送语音错误：" + sexp.Message);
                ret = false;
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
#if _CRT_NEW_
                client.Dispose();   
#endif
            }
            return ret;
        }
    }
    public class ZJF_JustTCPClient : IDisposable
    {
        // 2021 重写
        Socket m_Client = null;
        public bool m_init { get; private set; } = false;
        public string m_errorString { get; private set; } = "";
        public delegate void ReciveTxt(string txt);
        public event ReciveTxt OnReciveTxt;
        bool IsRunning = false;
        Thread main = null;
        //public ZJF_JustTCPClient(Socket socket)
        //{
        //    m_Client = socket;
        //    m_init = true;
        //    IsRunning = true;
        //    main = new Thread(ProcessClientTransfer) { IsBackground = true };
        //    main.Start(m_Client);
        //}
        IPEndPoint m_ep;
        private bool init()
        {
            try
            {
                m_Client.Connect(m_ep);
                m_init = true;
            }
            catch (SocketException sexp)
            {
                m_errorString = "ZJF_JustTCPClient 初始化尝试连接到服务器失败：" + sexp.Message;
                return false;
            }
            IsRunning = true;
            main = new Thread(ProcessClientTransfer) { IsBackground = true };
            main.Start(m_Client);
            return true;
        }
        public ZJF_JustTCPClient(string ip, int port)
        {
            m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ep = new IPEndPoint(IPAddress.Parse(ip), port);
            init();
        }
        public bool send(string txt)
        {
            if (!m_init)
            {
                init();
            }
            try
            {
                m_Client.Send(Encoding.Unicode.GetBytes(txt));
            }
            catch (SocketException sexp)
            {
                m_errorString = sexp.Message;
                m_init = false;
                IsRunning = false;
                try
                {
                    main.Abort();
                }
                catch
                {

                }
                return false;
            }
            return true;
        }
        public void close()
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
#if _CRT_NEW_
            m_Client.Dispose();
#endif
            main.Abort();
            try
            {
                main.Interrupt();
            }
            catch (Exception exp)
            {
                m_errorString = exp.Message;
            }
            m_init = false;
        }

        private void ProcessClientTransfer(object socket)
        {
            var client = socket as Socket;
            ZJF.ZJF_LOGGER.log("TCP新的连接正在建立！");
            try
            {
                while (IsRunning)
                {

                    byte[] buffer = new byte[1024 * 128];
                    int len = client.Receive(buffer, SocketFlags.None);
                    string words = Encoding.Unicode.GetString(buffer, 0, len);
                    ZJF.ZJF_LOGGER.log("recive num :" + len.ToString() + " ");
                    if (len > 0 && words != "")
                    {
                        //ZJF_TTS.Speak(words);
                        OnReciveTxt?.Invoke(words);
                    }
                    else if (len == 0)
                    {
                        //当客户端方 主动切断
                        break;
                    }
                }
            }
            catch (SocketException sexp)
            {
                ZJF.ZJF_LOGGER.log("TCP连接出现问题：" + sexp.Message);
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
#if _CRT_NEW_
                client.Dispose();
#endif
                m_init = false;
            }
            ZJF.ZJF_LOGGER.log("TCP连接已经断开！");
        }

        public void Dispose()
        {
            new Thread(() =>
            {
                m_Client.Disconnect(false);
                m_Client.Shutdown(SocketShutdown.Both);
#if _CRT_NEW_
                m_Client.Dispose();
#endif
            })
            .Start();

        }
    }

    public class ZJF_JustTCPServer
    {
        Socket m_Server = null;
        Thread main = null;
        public int m_Port { get; private set; }
        public bool Running = true;
        public delegate void ReciveTxt(string Txt);
        //public event ReciveTxt onReciveTxt;
        public ZJF_JustTCPServer(int port)
        {
            m_Port = port;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, m_Port);
                m_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Server.Bind(endPoint);

            }
            catch (SocketException sexp)
            {
                ZJF.ZJF_LOGGER.log("TCP服务初始化发生错误：" + sexp.Message);
            }
        }
        public void start()
        {
            main = new Thread(ProcessClientConnect) { IsBackground = true };
            main.Start();
        }
        private void ProcessClientConnect(object socket)
        {
            var server = socket as Socket;
            server.Listen(50);
            while (Running)
            {
                try
                {
                    var client = server.Accept();
                    new Thread((object _client) =>
                    {

                    }).Start(client);
                }
                catch (SocketException sexp)
                {
                    ZJF.ZJF_LOGGER.log("TCP接收新连接时出现问题：" + sexp.Message);
                }
            }
        }

        //private void ProcessClientTransfer(object socket)
        //{
        //    var client = socket as Socket;
        //    ZJF.ZJF_LOGGER.log("TCP新的连接正在建立！");
        //    try
        //    {
        //        while (Running)
        //        {

        //            byte[] buffer = new byte[1024 * 128];
        //            int len = client.Receive(buffer, SocketFlags.None);
        //            string words = Encoding.Unicode.GetString(buffer, 0, len);
        //            ZJF.ZJF_LOGGER.log("recive num :" + len.ToString() + " ");
        //            if (len > 0 && words != "")
        //            {
        //                //ZJF_TTS.Speak(words);
        //                onReciveTxt?.Invoke(words);
        //            }
        //            else if (len == 0)
        //            {
        //                //当客户端方 主动切断
        //                break;
        //            }
        //        }
        //    }
        //    catch (SocketException sexp)
        //    {
        //        ZJF.ZJF_LOGGER.log("TCP连接出现问题：" + sexp.Message);
        //    }
        //    finally
        //    {
        //        client.Shutdown(SocketShutdown.Both);
        //        client.Close();
        //        client.Dispose();
        //    }
        //    ZJF.ZJF_LOGGER.log("TCP连接已经断开！");
        //}

    }


#if __Weigh_bridge__
    public class ZJF_DeviceInfo : DeviceInfo
    {
        public string FID { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public string FLocation { get; set; }
        public string FBillStatus { get; set; }
        public string FUUIID { get; set; }
        public string FDeviceName { get; set; }
        public string FIP { get; set; }
        public string FTCPPort { get; set; }
        public string FUDPPort { get; set; }
        public string FFingerPrint { get; set; }
        public string FIDCard { get; set; }
        public string FQRCode { get; set; }
        public string FDoorType { get; set; }
        public string FDoorTypeName { get; set; }
        public string FUserName { get; set; }
        public string FUserPassword { get; set; }
        public string FTriggerAreaSpeak { get; set; } = "0";
    }
#endif

    public class ZJF_Functions
    {
        public static bool TxtIsIp(string ip)
        {
            var l = ip.Split('.');
            if (l.Length != 4)
            {
                return false;
            }
            for (int i = 0; i < 4; i++)
            {
                int _i = 0;
                if (!int.TryParse(l[i], out _i))
                {
                    return false;
                }
                else
                {
                    if (_i < 0 || _i > 255)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IntIsPort(int port)
        {
            if (port >= 0 && port < 65536)
            {
                return true;
            }
            return false;
        }
    }



    public class ZJF_UDPPoster : IDisposable
    {
        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        static UdpClient udpcRecv = null;

        static IPEndPoint localIpep = null;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        static bool IsUdpcRecvStart = false;
        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        static Thread thrRecv;

        public delegate void ProcessMsg(string txt);
        public static event ProcessMsg onProcessMsg;
        public static void Init(ushort localPort)
        {
            localIpep = new IPEndPoint(IPAddress.Any, localPort); // 本机IP和监听端口号
            udpcRecv = new UdpClient(localIpep);
        }
        public static void SendMessage(string remoteIp, ushort remotePort, string data)
        {
            try
            {
                string message = (string)data;
                byte[] sendbytes = Encoding.Unicode.GetBytes(message);
                IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort); // 发送到的IP地址和端口号
                udpcRecv.Send(sendbytes, sendbytes.Length, remoteIpep);
                //udpcSend.Close();
            }
            catch { }
        }
        public static void StartReceive()
        {
            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {
                thrRecv = new Thread(ReceiveMessage) { IsBackground = true };
                thrRecv.Start();
                IsUdpcRecvStart = true;
            }
        }

        public static void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                udpcRecv.Close();
                IsUdpcRecvStart = false;
                Console.WriteLine("UDP监听器已成功关闭");
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private static void ReceiveMessage(object obj)
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep);
                    string message = Encoding.Unicode.GetString(bytRecv, 0, bytRecv.Length);
                    onProcessMsg?.Invoke(message);
                    Console.WriteLine(string.Format("{0}[{1}]", localIpep, message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        public void Dispose()
        {
            StopReceive();
        }
    }

}

//}