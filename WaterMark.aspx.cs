using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


public partial class WaterMarkTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WaterMark.CreateImage(@"c:\Developement\PropertyMap\Data\5827.jpg", @"c:\Developement\PropertyMap\Data\5827_final.jpg", "http://PropertyMap.info");
    }
}