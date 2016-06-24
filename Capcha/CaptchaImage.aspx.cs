using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

public partial class Common_CaptchaImage : System.Web.UI.Page
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        Bitmap objBMP = new System.Drawing.Bitmap(22, 17);
        Graphics objGraphics = System.Drawing.Graphics.FromImage(objBMP);
        objGraphics.Clear(Color.Brown);
        objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

        //' Configure font to use for text
        Font objFont = new Font("Arial", 12, FontStyle.Bold);
        string randomStr = "";
        int[] myIntArray = new int[5];
        int x;

        //That is to create the random # and add it to our string
        Random autoRand = new Random();
        for (x = 0; x < 2; x++)
        {
            myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
            randomStr += (myIntArray[x].ToString());
        }
        //This is to add the string to session cookie, to be compared later
        Global.Captcha = randomStr;
        
        //' Write out the text
        objGraphics.DrawString(randomStr, objFont, Brushes.White, 0, 0);
        //' Set the content type and return the image
        Response.ContentType = "image/GIF";
        objBMP.Save(Response.OutputStream, ImageFormat.Gif);
        objFont.Dispose();
        objGraphics.Dispose();
        objBMP.Dispose();
    }
}
