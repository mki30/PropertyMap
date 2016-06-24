<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentMain.aspx.cs" Inherits="AgentMain" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/Simple.css" />
    <link rel="stylesheet/less" type="text/css" href="css/StyleSheet.less">
    <script type="text/javascript" src="js/less-1.3.0.min.js"></script>
    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/Common.js"></script>
    <script type="text/javascript" src="js/PropertyList.js"></script>
    <script type="text/javascript">
        SellerID="<%=CSVar%>";
    </script>
<style type="text/css">
        #navdiv span
        {
            padding-left:10px;
            padding-top:5px;
        }
        
        #navdiv span a
        {
            color:white;
            text-decoration:none;
        }
    </style>
</head>
<body class="bodyAgentMain">
    <form id="form1" runat="server">
    <div>
        <div style="width:100%;height:20px; background-color:#1490cf;" id="navdiv"><span style="color:white;"><a href="Default.aspx">Home</a> | <a href="Logout.aspx">Logout</a></span></div>
        <table style="border-collapse: collapse; background-color: white; border-bottom:1px solid  blacK;">
            <tr>
                <td style="vertical-align: top; min-width: 650px; background-color: #E5FFFF">
                    <div id="divAvlList" style="height: 650px; overflow: auto;"></div>
                </td>
                <td style="vertical-align: top">
                    <div style="border:1px solid #E2E2E2;">
                        <div style="text-align:center; border:1px solid #E2E2E2;" class="tablehead">Availability</div>
                    <iframe id="frameEditAvailability" src="Edit/EditAvailability.aspx" style="width: 350px; height: 650px; border: 0px"></iframe>
                    </div>
                </td>
                <td style="vertical-align: top; background-color: #E5FFFF">
                    <asp:TextBox ID="txtFilter" runat="server" required="required" placeholder="Filter Names" Style="border-radius: 4px; height: 20px; width: 150px; border: 2px solid #B2B9BB; margin-left: -3px; outline: none;"></asp:TextBox>
                    <div id="divClientList" style="height: 625px; overflow: auto;"></div>
                </td>
                
                <td style="vertical-align: top">
                    <input id="btnAsignAgent" type="button" value="Asign to this Client" style="display: none;" class="btn" onclick="AssignClientToProperty()" />
                    <br />
                    <div style="border:1px solid #E2E2E2;">
                        <div style="text-align:center; border:1px solid #E2E2E2;" class="tablehead">Availability</div>
                    <iframe id="frameClientDetail" src="Edit/EditAgentClient.aspx?AgentID=<%=CSVar%>" style="width: 300px; height: 300px; border: 0px"></iframe>
                        </div>
                    <div id="divClientAssignedAvl"></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
