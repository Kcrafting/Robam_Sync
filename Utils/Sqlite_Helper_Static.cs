//using Microsoft.Extensions.Logging;
using SQLite;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public abstract class Sqlite_HelperBase
    {
        protected static string fileName { get; private set; } = "wfzhgj.db3";
        protected static string path { get; private set; } = "";
        static SQLiteConnection db = null;
        static bool inited = false;
        static object locker = new object();
        public static string ErrorString { get; private set; } = "";
        public static void Init()
        {
            try
            {
                //Android.Provider.MediaStore.Downloads.ExternalContentUri.Path
                path = System.IO.Path.Combine(/*(string)Android.Provider.MediaStore.Downloads.ExternalContentUri.Path*/ System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), fileName);
                db = new SQLiteConnection(path);
                inited = true;
            }
            catch (SQLiteException sexp)
            {
                Logger.log(sexp.Message);
            }

        }

        public static List<T> read<T>() where T : class, new()
        {
            if (!inited)
                Init();
            if (!tableExist<T>())
            {
                createTable<T>();
            }
            lock (locker)
            {
                try
                {
                    string tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute))).Name;
                    //tableName = tableName.Substring(tableName.IndexOf('+') + 1, tableName.Length - tableName.IndexOf('+') - 1);
                    return db.Query<T>("select * from " + tableName);
                }
                catch (SQLiteException slex)
                {
                    ErrorString = slex.Message;

                }
                return null;
            }
        }
        public static void createTable<T>()
        {
            if (!inited)
                Init();
            db.CreateTable<T>();
            var mp = db.GetMapping<T>();
            var tn = mp.TableName;
            var pk = mp.PK;
            Logger.log(tn ?? "" + " has been created ,pk is " + pk?.PropertyName ?? "");
        }
        public static bool tableExist<T>()
        {
            if (!inited)
                Init();
            TableAttribute tab = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            var info = db.GetTableInfo(tab.Name);
            if (info.Any())
            {
                return true;
            }
            return false;
        }
        public static bool valueExists<T>(Func<T, bool> predicate) where T : class, new()
        {
            var list = read<T>();
            return list != null ? list.Where<T>(predicate).ToList().Count > 0 : false;
        }
        public static int write<T>(List<T> list)
        {
            if (!inited)
                Init();
            TableAttribute tab = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            if (!tableExist<T>())
            {
                createTable<T>();
            }
            if (list == null)
            {
                return 0;
            }

            lock (locker)
            {
                try
                {
                    return db.InsertAll(list);
                }
                catch (SQLiteException slex)
                {
                    ErrorString = slex.Message;
                }
                return 0;
            }

        }
        public static T select_<T>(Func<T, bool> predicate) where T : class, new()
        {
            return read<T>().Where(predicate).FirstOrDefault();
        }
        public static int write<T>(T ins)
        {
            if (!inited)
                Init();
            if (!tableExist<T>())
            {
                createTable<T>();
            }
            if (ins == null)
            {
                return 0;
            }
            lock (locker)
            {
                try
                {
                    return db.Insert(ins);
                }
                catch (SQLiteException slex)
                {
                    ErrorString = slex.Message;
                }
                return 0;
            }

        }
        public static int update(string sqlTxt)
        {
            if (!inited)
                Init();
            lock (locker)
            {
                try
                {
                    return db.Execute(sqlTxt);
                }
                catch (SQLiteException slex)
                {
                    ErrorString = slex.Message;
                }
                return 0;
            }
        }
        public static int update<T>(T ins)
        {
            if (!inited)
                Init();
            if (tableExist<T>())
            {
                lock (locker)
                {
                    try
                    {
                        return db.Update(ins, typeof(T));
                    }
                    catch (SQLiteException slex)
                    {
                        ErrorString = slex.Message;
                    }
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static void downloadFromServer<T>()
        {
            //try
            //{
            //    using SqlConnection con = new SqlConnection();
            //    con.Open();

            //}catch(SqlException sexp)
            //{
            //    Logger.log(sexp.StackTrace);
            //}

        }
        public static void uploadToServer<T>()
        {

        }
        public static int update<T>(Func<T, bool> predicate, T newone) where T : class, new()
        {
            if (!inited)
                Init();
            lock (locker)
            {
                try
                {
                    var list = read<T>();
                    var list2 = list.Where(predicate).ToList();
                    foreach (var item in list2)
                    {
                        db.Delete(item);
                    }
                    //db.DeleteAll<T>();
                    db.Insert(newone);
                }
                catch (SQLiteException slex)
                {
                    ErrorString = slex.Message;
                }
                return 0;
            }
        }
        public static void deleteall<T>()
        {
            if (!tableExist<T>())
            {
                createTable<T>();
            }
            db.DeleteAll<T>();
        }
        public static void delete<T>(Func<T, bool> predicate) where T : class, new()
        {
            var ret = read<T>()?.Where(predicate).ToList();
            var rr = read<T>();
            foreach (var i in ret)
            {
                var tp = i.GetType().GetProperties().Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null).FirstOrDefault();
                var ind = tp.GetValue(i);
                db.Delete<T>(ind);
                db.Commit();

            }
        }
        public static void droptable<T>()
        {
            try
            {
                if (tableExist<T>())
                {
                    db.DropTable<T>();
                }
                
            }
            catch
            {

            }
            
        }
    }
    public class Sqlite_Helper_Static : Sqlite_HelperBase
    {
        private static string constr = "Data Source = 39.98.121.252 ; Initial Catalog = master; User ID = sa; Password=Zjf744518.";
        public static void setConstr(string str)
        {
            constr = str;
        }
    }
}
