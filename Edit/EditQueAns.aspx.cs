using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PropertyListModel;
using System.Net.Mail;
using System.Net;

public partial class Edit_EditQueAns : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        string ID = Request.QueryString["ID"];
        if(!IsPostBack)
        ShowData(ID);
    }
    private void ShowData(string ID)
    {
        ProjectQuestion PQ = ProjectQuestion.GetByID(Cmn.ToInt(ID));
        lblID.Text = ID;
        txtAns.Text = string.IsNullOrEmpty(PQ.Answer)?"":PQ.Answer.ToString();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Update();
    }
    private void Update()
    {
        try
        {
            ProjectQuestion PQ1 = ProjectQuestion.GetByID(Cmn.ToInt(lblID.Text));
            PQ1.Answer = txtAns.Text;
            PQ1.Save();
            //SendEmail(PQ1.Subject, PQ1.Email, PQ1.Name, PQ1.Answer);
        }
        catch (Exception ex)
        {

        }
        ShowData(lblID.Text);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ProjectQuestion PQ1 = ProjectQuestion.GetByID(Cmn.ToInt(lblID.Text));
        PQ1.Delete();
        WriteClientScript("parent.RefreshPage();");
    }

    public string SendEmail(string Subject, string EmailTo, string Name, string Description)
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
        msg.Body = string.Format("<html><head>Thanks for showing your intrest "+Name+"</head><body><p>" + Description +
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