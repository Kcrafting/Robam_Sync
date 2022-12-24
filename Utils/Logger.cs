using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System;
using System.Runtime.CompilerServices;

namespace Utils
{
    public class Logger
    {
        private static string m_path = "";
        private static StreamWriter m_write = null;
        private static bool m_init = false;
        private static object locker = new object();
        public static string m_errorstring { private set; get; } = "";
        public static void init(string filename = "clientlog", bool notcreatenew = true)
        {
            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
#if WINDOWS_UWP
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup);
#endif

                m_path = Path.Combine(path, filename + ".txt");
                m_write = new StreamWriter(m_path, notcreatenew, Encoding.Default);
                m_init = true;
            }
            catch (Exception exp)
            {
                m_errorstring = exp.Message;
                //m_path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), filename + ".txt");
                //m_write = new StreamWriter(m_path, false, Encoding.Default);
                //m_init = true;
            }

        }
        public static void log(params string[] txt)
        {
            if (!m_init)
            {
                init();
            }
            lock (locker)
            {
                try
                {
                    if (txt != null)
                    {
                        m_write.WriteLine(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") + " " + DateTime.Now.Millisecond.ToString() + " Thread:" + Thread.CurrentThread.ManagedThreadId.ToString() + " " + (new StackTrace()).GetFrame(1).GetMethod().Name + " \n" + txt);
                        m_write.Flush();
                    }
                }
                catch (Exception exp)
                {
                    m_errorstring = exp.Message;
                    return;
                }
            }

        }
        public static void DebugLog( string txt,  [CallerMemberName] string funcname = "", [CallerFilePath] string filename = "", [CallerLineNumber] int linenumber = 0)
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + '\n');
            if (st.GetFrame(1) != null)
            {
                log(st.GetFrame(1).GetFileName() + '\n');
                log(st.GetFrame(1).GetMethod().Name + '\n');
                log(st.GetFrame(1).GetFileLineNumber().ToString() + " : " + st.GetFrame(0).GetFileColumnNumber().ToString() + '\n');
                log(st.GetFrame(1).GetFileName() + '\n');
            }
            else if (st.GetFrame(0) != null)
            {
                log(st.GetFrame(0).GetFileName() + '\n');
                log(st.GetFrame(0).GetMethod().Name + '\n');
                log(st.GetFrame(0).GetFileLineNumber().ToString() + " : " + st.GetFrame(0).GetFileColumnNumber().ToString() + '\n');
                log(st.GetFrame(0).GetFileName() + '\n');
            }
            log(txt);
        }
        public static void DebugLog2(params string[] txt)
        {
            //当前堆栈信息
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame[] sfs = st.GetFrames();
            //过虑的方法名称,以下方法将不会出现在返回的方法调用列表中
            string _filterdName = "ResponseWrite,ResponseWriteError,";
            string _fullName = string.Empty, _methodName = string.Empty;
            string writeTxt = "";
            for (int i = 1; i < sfs.Length; ++i)
            {
                //非用户代码,系统方法及后面的都是系统调用，不获取用户代码调用结束
                if (System.Diagnostics.StackFrame.OFFSET_UNKNOWN == sfs[i].GetILOffset()) break;
                _methodName = sfs[i].GetMethod().Name;//方法名称
                                                      //sfs[i].GetFileLineNumber();//没有PDB文件的情况下将始终返回0
                if (_filterdName.Contains(_methodName)) continue;
                _fullName = _methodName + "()->" + _fullName;
                writeTxt += _fullName;
                writeTxt += " File: {0}" + sfs[i].GetFileName() + "\n";                                                //文件名
                writeTxt += " Method: {0}" + sfs[i].GetMethod().Name +"\n";                                 //函数名
                writeTxt += " Line Number: {0}" + sfs[i].GetFileLineNumber() + "\n";                  //文件行号，需要项目有调试需要的PDB文件,否则就返回0
                writeTxt += " Column Number: {0}" + sfs[i].GetFileColumnNumber() + "\n";
                writeTxt += " DeclaringType FullName: {0}" + sfs[i].GetMethod().DeclaringType.FullName + "\n";
            }
            st = null;
            sfs = null;
            _filterdName = _methodName = null;
            return;// _fullName.TrimEnd('-', '>');
        }
        public static string readlog()
        {
            if (!m_init)
            {
                init();
            }
            lock (locker)
            {
                try
                {
                    var reader = new StreamReader(m_path);
                    var ret = reader.ReadToEnd();
                    return ret;
                }
                catch (Exception exp)
                {
                    log(exp.Message);
                    return "";
                }

            }
        }
    }
}
