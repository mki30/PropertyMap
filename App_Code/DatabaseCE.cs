using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public class DatabaseCE
    {
        public IDbConnection myconnection = null;
        private IDbCommand cmd;
        public string Error = "";

        public void CreateDB(string Path, string strAccessConn)
        {
            if (!File.Exists(Path))
            {
                SqlCeEngine engine = new SqlCeEngine(strAccessConn);
                engine.CreateDatabase();
            }
        }

        public DatabaseCE(string strAccessConn = "")
        {
            if (string.IsNullOrWhiteSpace(strAccessConn))
                strAccessConn = @"Data Source=|DataDirectory|\PropertyList.sdf;";

            string Error = "";
            try
            {
                if (strAccessConn.ToLower().Contains(".mdb") || strAccessConn.ToLower().Contains(".xls"))
                {
                    myconnection = new OleDbConnection(strAccessConn);
                    cmd = new OleDbCommand();
                }
                else if (strAccessConn.ToLower().Contains(".sdf"))
                {
                    myconnection = new SqlCeConnection(strAccessConn);
                    cmd = new SqlCeCommand();
                }
                else if (strAccessConn.ToLower().Contains(".mdf"))
                {
                    myconnection = new SqlConnection(strAccessConn);
                    cmd = new SqlCommand();
                }

                myconnection.Open();
                cmd.Connection = myconnection;
            }
            catch (Exception ex)
            {
                //Cmn.LogError(ex, "DataBase_Database()");
                Error = ex.Message;
            }
        }
        
        //public DatabaseCE()
        //{
        //    string strAccessConn = @"Data Source=|DataDirectory|\PropertyList.sdf";
        //    try
        //    {
        //        myconnection = new SqlCeConnection(strAccessConn);
        //        cmd = new SqlCeCommand();
        //        myconnection.Open();
        //        cmd.Connection = myconnection;
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = ex.Message;
        //    }
        //} 
        
        public void Close()
        {
            if(myconnection!=null)
                if (myconnection.State == System.Data.ConnectionState.Open)
                    myconnection.Close();
        }
        
        // run query and set value in data set
        public DataSet GetDataSet(string sqlQuery, ref string vError)
        {
            IDbDataAdapter dbAdapter = new SqlCeDataAdapter(sqlQuery, myconnection.ConnectionString);
            DataSet resultsDataSet = new DataSet();
            
            try
            {
                dbAdapter.Fill(resultsDataSet);
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
            return resultsDataSet;
        }
        
        public IDataReader GetDataReader(string SQL, ref string vError)
        {
            cmd.CommandText = SQL;
            cmd.CommandType = CommandType.Text;

            IDataReader dataReader = null;
            try
            {
                dataReader = cmd.ExecuteReader();
            }
            catch (Exception Ex)
            {
                if (dataReader != null)
                    dataReader.Close();
                    
                vError = Ex.Message;
            }
            return dataReader;
        }
    
        // run only query 
        public string RunQuery(string sqlQuery)
        {
            try
            {
                cmd.CommandText = sqlQuery;
                int n= cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message + "~~" + sqlQuery;
            }
            return "";
        }
        
        // ExecuteScalar
        public object ExecuteScalar(string sqlQuery, ref string vError)
        {
            try
            {
                cmd.CommandText = sqlQuery;
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
            return null;
        }

        public string GetFieldValue(string SQL, ref string vError)
        {
            string ret = "";
            vError = "";

            cmd.CommandText = SQL;
            IDataReader dataReader = null;

            try
            {
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ret = dataReader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
            return ret;
        }

        // GetMax
        public int GetMax(string TableName, string FieldName, string where, ref string vError)
        {
            string sqlQuery;
            int max = 0;
            object ob;
            try
            {
                sqlQuery = "select max(" + FieldName + ") from " + TableName + (where.Length > 0 ? " where " + where : "");
                cmd.CommandText = sqlQuery;
                ob = cmd.ExecuteScalar();
                if (ob.GetType().ToString() != "System.DBNull")
                    max = int.Parse(ob.ToString());
                vError = "";
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
            return max;
        }

        //dublicate record
        public int GetCount(string sqlQuery, ref string vError)
        {
            int CountRecord = 0;
            object ob;

            try
            {
                cmd.CommandText = sqlQuery;
                ob = cmd.ExecuteScalar();
                if (ob != null)
                    CountRecord = Convert.ToInt32(ob);

                return CountRecord;
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
            return 0;
        }

        public void FillCombo(string sqlQuery, DropDownList combo, string FirstField, string FirstValue, string DisplayField, string DisplayValue, ref string vError)
        {
            vError = "";

            cmd.CommandText = sqlQuery;
            combo.Items.Clear();
            
            try
            {
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    if (FirstField != "")
                        combo.Items.Add(new ListItem(FirstField, FirstValue));

                    while (dataReader.Read())
                    {
                        combo.Items.Add(new ListItem(dataReader[DisplayField.Trim()].ToString(), dataReader[DisplayValue.Trim()].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                vError = ex.Message;
            }
         }
}
