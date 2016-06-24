using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Upload : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        var imageName = QueryString("ImageName");

        HttpContext postedContext = HttpContext.Current;
        if (postedContext.Request.Files.AllKeys.Count() > 0)
        {
            HttpPostedFile file = postedContext.Request.Files[0];
            string name = file.FileName;
            byte[] binaryWriteArray = new byte[file.InputStream.Length];
            file.InputStream.Read(binaryWriteArray, 0, (int)file.InputStream.Length);
            FileStream objfilestream = new FileStream(Server.MapPath("~/Data/" + imageName + ".jpg" + name.Substring(name.IndexOf('.'))), FileMode.Create, FileAccess.ReadWrite);//save Image as it is.
            objfilestream.Write(binaryWriteArray, 0, binaryWriteArray.Length);
            objfilestream.Close();
            string[][] JaggedArray = new string[1][];
            JaggedArray[0] = new string[] { "File was uploaded successfully" };
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(JaggedArray);
            Response.Write(strJSON);
            Response.End();
        }
    }
}
