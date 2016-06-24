<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="AgentReport.aspx.cs" Inherits="Edit_AgentReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../js/TableSearch.js"></script>
    <script src="../js/jquery.tablesorter.js"></script>
    <script src="../js/Cookie.js"></script>
    <style type="text/css">
        body
        {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top; width: 200px;">
              <asp:ListBox ID="lstCity" runat="server" AutoPostBack="True" Height="750px"  Width="197px" OnSelectedIndexChanged="lstCity_SelectedIndexChanged"></asp:ListBox>
            </td>
            <td style="vertical-align: top;">
                <div>
                    <input id='txtName' type='text' placeholder='Search…' style='margin-top: 5px; border-radius: 2px;' autocomplete="off" />
                    <div style="max-height: 10px; display: inline;">
                        <asp:CheckBox ID="ChkAll" runat="server" Text="All" AutoPostBack="True" />
                        <asp:CheckBox ID="chkVerified" runat="server" Text="Verified" Checked="true" AutoPostBack="True" OnCheckedChanged="chkVerified_CheckedChanged"/>
                        <asp:CheckBox ID="ChkDeleted" runat="server" Text="Deleted" AutoPostBack="True" OnCheckedChanged="ChkDeleted_CheckedChanged"/>
                    </div>
                    <div  style="display:inline; margin-left:20px;"><a id="AddProject"  class="fancybox fancybox.iframe" href="EditAgent.aspx?ID=0&ShowList=1">Add New Agent</a></div>
                    <asp:Literal ID="ltList" runat="server"></asp:Literal>
                </div>
             </td>
          </tr>
    </table>
</asp:Content>


