<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComboEdit.aspx.cs" Inherits="Edit_ComboEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script src="../js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="../js/QueryString.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            var q = new QueryString();
            q.read();
            ProjID = q.getQueryString("SocietyID");
            SubCityId = q.getQueryString("SubCityID");

            $("#frameEditSociety")[0].src = "EditSociety.aspx?ID=" + ProjID;
            $("#frameMap")[0].src = $("#frameMap")[0].src = "../Map.htm?ID=" + ProjID + "&SubCityId=" + SubCityId;
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="border-spacing: 1px">
                <tr>
                    <td style="width: 400px; height:800px; vertical-align: top;">
                        <iframe id="frameEditSociety" src="EditSociety.aspx" style="width: 98%; height:98%; border: 0px;"></iframe>
                    </td>
                    <td style="width:800px; height:100%;">
                        <iframe id="frameMap" style="width: 100%; height: 99%; border: 0px;"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
