using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;

public partial class GeneratingHTML : System.Web.UI.Page
{
    
    static string[] Files = null;
    void UpdateData()
    {

        //Dictionary<string, string> KohlersList = new Dictionary<string, string>();
        //string DataFile = Server.MapPath(@"~\Brands\Lg\TvAudioVideo.txt");
        //if (File.Exists(DataFile))
        //{
        //    string[] DataLines = File.ReadAllLines(DataFile);
        //    foreach (string s in DataLines)
        //    {
        //        string ModelNumber = s.Split(',')[0];
        //        if (!KohlersList.ContainsKey(ModelNumber))
        //            KohlersList.Add(ModelNumber, s);
        //    }
        //}

        //string[] Lines = txtPrcessedData.Text.Split('~');
        //DatabaseCE db = new DatabaseCE();
        //try
        //{
        //Item I = new Item(0);


        //string ModelNumber = "";
        //string ImagePath = "";
        //string KeyFeatures = "";
        //string Specification = "";

        //for (int i = 0; i < Lines.Length - 1; i++)
        //{
        //    if (Lines[i] == "")
        //        continue;

        //    string[] Fields = Lines[i].Replace("\r\n", "^").Split('^');

        //    if (i == 0)
        //    {
        //        foreach (string F in Fields)
        //        {
        //            if (F.Trim() != "")
        //                ModelNumber += F.Trim() + ",";

        //        }
        //    }

        //    if (i == 1)
        //    {
        //        foreach (string F in Fields)
        //        {
        //            if (F.Trim() != "")
        //                ImagePath += F.Trim() + ",";
        //        }
        //    }
        //    if (i == 2)
        //    {
        //        foreach (string F in Fields)
        //        {
        //            if (F.Trim() != "")
        //                KeyFeatures += F.Trim() + ", ";
        //        }
        //    }
        //    if (i == 3)
        //    {
        //        foreach (string F in Fields)
        //        {
        //            if (F.Trim() != "")
        //                Specification += F.Trim();
        //        }
        //    }

        //}

        //if (!KohlersList.ContainsKey(ModelNumber))
        //    KohlersList.Add(ModelNumber, "");

        //KohlersList[ModelNumber] = (ModelNumber + "^" + ImagePath + "^" + KeyFeatures + "^"+ Specification).Replace(Environment.NewLine, " ");;

        //}
        //catch (Exception ex)
        //{
        //    Response.Write(ex.Message);
        //}
        //finally
        //{
        //    db.Close();
        //}

        //StringBuilder sb = new StringBuilder();
        //foreach (string s in KohlersList.Values)
        //{
        //    sb.AppendLine(s);
        //}

        //File.WriteAllText(DataFile, sb.ToString());
    }



    protected void btnProcess_Click(object sender, EventArgs e)
    {
        if (txtPrcessedData.Text != "")
            UpdateData();

        if (Files == null)
            Files = Directory.GetFiles(Server.MapPath(@"~\Brands\Lg\TvAudioVideo"));

        int Index = Cmn.ToInt(txtLastIndex.Text);

        if (Index >= 0 && Index < Files.Length)
        {
            string FileName = Files[Index];
            txtData.Text = Server.HtmlEncode(File.ReadAllText(FileName));
            txtLastIndex.Text = (++Index).ToString();

            FileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            FileName = FileName.Replace("-", "?");
            txtProductURL.Text = FileName.Replace(".htm", "");
        }
        else
        {
            txtData.Text = "";
        }
    }
}