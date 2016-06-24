using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContactUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        string Subject = "Contact Enquiery from" + Full_Name.Value;
        string EmailTo="psu.singh@gmail.com";
        string SenderName = Full_Name.Value;
        string Sendermail = Email_Address.Value;
        string SenderPhone = Telephone_Number.Value;
        string Msgbody = "<div style='backgroung-color:blue;'>Name: "+SenderName+"</div><div>Email: " + Sendermail+"</div><div>Phone: " + SenderPhone+"</div><div>Message: "+Your_Message.Value+"</div>"; 
        SendEmail(Subject, EmailTo, Msgbody);
    }
    public static string SendEmail(string Subject, string EmailTo, string Msgbody)
    {
        SmtpClient client = new SmtpClient();

        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;

        // setup Smtp authentication
        NetworkCredential credentials = new NetworkCredential("rrasoftmail@gmail.com", "rrasoft123");

        client.UseDefaultCredentials = false;
        client.Credentials = credentials;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("rrasoftmail@gmail.com");
        //  msg.Bcc.Add(new MailAddress(EmailTo));
        msg.To.Add(new MailAddress(EmailTo));
        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = string.Format("<html><body style='background-color:orange;'>" + Msgbody + "</body></html>");

        try
        {
            client.Send(msg);
            return "Mail sent.";
        }
        catch (Exception ex)
        {
            return "Error occured while sending your message." + ex.Message;
        }
    }
}