using System;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
	public Utility()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    public static void GetAptNo(ListControl lb, int SocietyID)
    {
        DatabaseCE db = new DatabaseCE();
        string Error = "";
        try
        {
            lb.Items.Clear();
            DataSet dsAptNo = db.GetDataSet("Select * from SocietyApartment where SocietyID=" + SocietyID + " order by Block,ApartmentNumber", ref Error);
            foreach (DataRow dr in dsAptNo.Tables[0].Rows)
            {
                lb.Items.Add(new ListItem(dr["Block"] + "-" + dr["ApartmentNumber"], dr["ID"].ToString()));
            }

        }
        catch (Exception)
        {
        }
        
        finally
        {
            db.Close();
        }
    }

    public static void SocietyName(ListControl lb,string tablename)
    {
        DatabaseCE db = new DatabaseCE();
        string Error = "";
        try
        {
            lb.Items.Clear();

            DataSet ds = db.GetDataSet("Select distinct SocietyName, id from " + tablename + " order by SocietyName", ref Error);
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if(dr["SocietyName"].ToString()!="")
                lb.Items.Add(new ListItem(dr["SocietyName"].ToString(), dr["ID"].ToString()));
            }

        }
        
        catch (Exception)
        {
        }
        
        finally
        {
            db.Close();
        }
    }
}