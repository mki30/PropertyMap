using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;

public partial class ProjectQ : System.Web.UI.Page
{
    string PageUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //base.Page_Load(sender, e);
        PageUrl = Request.QueryString["url"] != null ? Request.QueryString["Url"].ToString() : "";
        if (!IsPostBack)
        {
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Global.Captcha == txtCaptcha.Text)
        {
            ProjectQuestion PQ = new ProjectQuestion();
            PQ.ID = 0;
            //PQ.ProjectID =Data1 ;
            PQ.Subject = txtSubject.Text;
            PQ.Name = txtName.Text;
            PQ.Email = txtEmail.Text;
            PQ.Question = txtQuestion.Text;
            PQ.PageUrl = PageUrl;
            PQ.Approved = 0;
            //PQ.Save();
            //SendEmail(PQ.Subject, "psu.singh@gmail.com", PQ.Name, PQ.Email, PQ.Question);
        }
        else
        {
            lblMessage.Text = " Invalid Captcha ";
            lblMessage.ForeColor = System.Drawing.Color.Red;    
        }
    }

    public static string SendEmail(string Subject, string EmailTo, string Name, string Usermail, string Description)
    {
        SmtpClient client = new SmtpClient();

        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;

        // setup Smtp authentication
        NetworkCredential credentials = new NetworkCredential("rraprop@gmail.com", "rra12345");
        client.UseDefaultCredentials = false;
        client.Credentials = credentials;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("rraprop@gmail.com");
        msg.To.Add(new MailAddress(EmailTo));
        msg.CC.Add(new MailAddress("rraprop@gmail.com"));
        //msg.Bcc.Add(new MailAddress("uam2014.oc@gmail.com"));
        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = string.Format("<html><head>PropertyMap Feedback</head><body><p>" + Description +
        "<p>" + Usermail + "</p>" +
        "<p><b>" + Name + "<b/></p>" +
        "</body></html>");
        try
        {
            client.Send(msg);
            return "";
        }
        catch (Exception ex)
        {
            return "Error occured while sending your message." + ex.Message;
        }
    }
}