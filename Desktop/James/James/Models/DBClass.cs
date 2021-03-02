using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace James.Models
{
    public class DBClass
    {
        public static string myDBConnectionString = connectString();

        public static string connectString()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @"(LocalDB)\MSSQLLocalDB";

            // 不可用 @"..//..//Database2.mdf";  ??
            scsb.AttachDBFilename = @"|DataDirectory|dbj.mdf"; // |DataDirectory| 預設-> \bin\Debug\
                                                                     // scsb.InitialCatalog = "mydb"; // 資料庫名稱
            scsb.IntegratedSecurity = true; // 整合驗證

            return scsb.ToString();


        }
        public static int SQLExecute(string sSQL, SqlParameter[] values)
        {

            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.Parameters.AddRange(values);

            int row = cmd.ExecuteNonQuery();
            con.Close();

            return row;
        }

        public static void SQLReader(string sSQL, SqlParameter[] sqlArgs, Action<SqlDataReader> action)
        {
            SqlConnection con = new SqlConnection(myDBConnectionString);
            con.Open();

            // 加入SQL 字串
            SqlCommand cmd = new SqlCommand(sSQL, con);
            // 加入SQL 參數
            cmd.Parameters.AddRange(sqlArgs);
            SqlDataReader reader = cmd.ExecuteReader();

            // callback reader
            action(reader);

            reader.Close();
            con.Close();
        }
    }
}