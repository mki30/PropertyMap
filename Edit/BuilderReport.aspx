<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="BuilderReport.aspx.cs" Inherits="Edit_BuilderReport" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/TableSearch.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <asp:Literal ID="ltChar" runat="server"></asp:Literal>
            </td>
            <td>
                <input id='txtName' type='text' placeholder='Search…' autocomplete="off"  style="margin:0;padding:0;border-radius:2px;"/>
            </td>
        </tr>
    </table>

    <asp:Literal ID="ltData" runat="server"></asp:Literal>
</asp:Content>

