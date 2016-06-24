<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageImageMain.aspx.cs" Inherits="Edit_ManageImageMain" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function showData(referenceId, imageType)
        {
            $("#frameManagesImage")[0].src = "ManageImages.aspx?referenceid=" + referenceId + "&imagetype=" + imageType + "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width: auto; border-spacing: 1px;">
            <tr>
                <td style="width: 20%; vertical-align: top;">
                    <iframe id="frameManageImageselect" src="ManageImageselect.aspx" style="width: 98%; height: 800px; border: 0px;"></iframe>
                </td>
                <td style="width:80%; vertical-align: top;">
                    <iframe id="frameManagesImage" src="ManageImages.aspx" style="width: 98%; height: 800px; border: 0px;"></iframe>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
