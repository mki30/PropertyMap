<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="PdfDownload.aspx.cs" Inherits="Edit_PdfDownload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
        });
        function Download(url,urlName,id)
        {
            $("#download_"+id).html("<img src='/images/progress2.gif'/>");
            
            var data = "/Data.aspx?Action=DownloadPdf&Data1=" + url + "&Data2="+urlName;

            $.ajax({
                url:data, cache: false, success: function (data)
                {
                    $("#download_" + id).html(data);
                },
                error: function (e)
                {
                    alert("Error:" + e);
                }
            })
            return false;
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="ltList" runat="server"></asp:Literal>
</asp:Content>