<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectQ.aspx.cs" Inherits="ProjectQ" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ask a quetion to us</title>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery-1.9.1.min.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery-ui-1.10.2.custom.min.js")%>'></script>
    <link href="<%=ResolveClientUrl("~/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet">
    <link href="<%=ResolveClientUrl("~/bootstrap/css/bootstrap-responsive.min.css")%>" rel="stylesheet">
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/bootstrap.min.js")%>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/bootstrap/js/bootstrap.min.js")%>'></script>
    <%--<script src="js/jquery.realperson.min.js"></script>--%>
    <%--<link href="css/jquery.realperson.css" rel="stylesheet" />--%>
    <script src="js/QueryString.js"></script>
    <meta name="description" content="ask a question to propertymap"/>  
    <script type="text/javascript">
        //$(document).ready(function ()
        //{
        //    var q = new QueryString();
        //    q.read();
        //    var ProjID = q.getQueryString("ProjectID");

        //    $("#submit").click(function ()
        //    {
        //        if ($("#txtSubject").val() == "")
        //        {
        //            $("#submsg").text("mandatory!");
        //            return;
        //        }
        //        if ($("#txtName").val() == "")
        //        {
        //            $("#namemsg").text(" mandatory!");
        //            return;
        //        }
        //        if ($("#txtEmail").val() == "")
        //        {
        //            $("#emailmsg").text(" mandatory!");
        //            return;
        //        }
        //        if ($("#txtSubject").val() == "")
        //        {
        //            $("#comentmsg").text(" mandatory!");
        //            return;
        //        }
        //        if(!validateEmail(($("#txtEmail").val())))
        //        {
        //            $("#emailmsg").text(" invalid mail!");
        //            return;
        //        }
        //        //console.log($("#QueryForm").serialize());
        //        //console.log($("#QueryForm").serializeArray())
        //        $.post("./Data.aspx?Action=SaveQuestionForm&Data1=" + ProjID, $("#QueryForm").serialize(), function (data)
        //        {
        //            if (data == "")
        //            {
        //                $("#Message").text("Submited Thank You!");
        //                $("#Message").css('color', 'green');
        //            }
        //            else
        //            {
        //                if (data.replace('~', '').trim() == "CVFAILED")
        //                    $("#Message").text("Text Not Mach! Or Empty!");
        //                else
        //                    $("#Message").text("Some Error Occured!");
        //                $("#Message").css('color', 'red');
        //            }
        //        });
        //    });
        //});
        //function validateEmail(email) //email validation
        //{
        //    alert(email);
        //    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        //    if( !emailReg.test(email) ) {
        //        return false;
        //    } else {
        //        return true;
        //    }
        //}
        //$(function ()
        //{
        //    $('#realPerson').realperson({ length: 4 });
        //});
    </script>
    <style type="text/css">
        form
        {
            margin: 0px;
        }
        .validationmsg
        {
            color:red;
            font-size:11px;
        }
    </style>
</head>
<body>
    <form id="QueryForm" class="form-horizontal" runat="server">
        <h1 style="margin-top:-50px;font-size:8px;">Ask a queston</h1>
        <div style="padding: 5px;">
            <div class="control-group">
                <label class="control-label" for="txtSubject">Subject</label>
                <div class="controls">
                    <asp:TextBox ID="txtSubject" runat="server">
                    </asp:TextBox><asp:RequiredFieldValidator ID="txtSubjectdRequired" runat="server" ErrorMessage=" required!" ForeColor="Red" ControlToValidate="txtSubject"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="inputEmail">Name</label>
                <div class="controls">
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtNameRequired" runat="server" ErrorMessage="required!" ForeColor="Red" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="inputPassword">Email</label>
                <div class="controls">
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtEmailRequired" runat="server" ErrorMessage="required!" ForeColor="Red" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="txtEmailExpression" runat="server" ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="inputPassword">comments & Questions</label>
                <div class="controls">
                    <asp:TextBox ID="txtQuestion" TextMode="MultiLine" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtQuestionRequired" runat="server" ErrorMessage="required!" ForeColor="Red" ControlToValidate="txtQuestion"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group" runat="server" id="divCapcha">
                    <label class="control-label">
                         <img src="Capcha/CaptchaImage.aspx" alt="CAPCHA" /></label>
                    <div class="controls">
                        <asp:TextBox ID="txtCaptcha" runat="server" placeholder="Fill the given text"></asp:TextBox>
                    </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button ID="btnSubmit" runat="server" Text="Button" OnClick="btnSubmit_Click" /><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
