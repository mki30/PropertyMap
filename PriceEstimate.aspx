<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PriceEstimate.aspx.cs" Inherits="PriceEstimate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            estimatePrice();
            $("#ddBSPPlan").change(estimatePrice);
            $('input[name=radioPB]').change(estimatePrice);
        });

        function estimatePrice()
        {
            var area = $("#lblArea").text();
            var price = parseInt($("#ddBSPPlan").val()) * area
                + parseInt($("#ddLocationPLC").val()) * area
                + parseInt($("#txtLeaseRent").val()) * area
                + parseInt($("#txtClubMembrship").val())
                + parseInt($("#txtFFC").val())
                + parseInt($("#txtEEC").val())
                + parseInt($("#txtPowerBackup").val()) * parseInt($('input[name=radioPB]:checked').val());
                + parseInt($("#ddCarParking").val());
                $("#lblEstimatedPrice").text(price);
                console.log(parseInt($('input[name=radioPB]:checked').val()));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table class="table table-bordered">
        <tr>
            <td colspan="2">
                <h1>
                    <asp:Label ID="lblSociety" runat="server" Text=""></asp:Label>
                    <span style="font-size: 20px;">
                        <asp:Label ID="lblTypeName" runat="server" Text=""></asp:Label>-
                        <asp:Label ID="lblArea" runat="server" Text="0"></asp:Label>
                    </span>
                </h1>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblPlan" runat="server" Text="Label"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddBSPPlan" runat="server"></asp:DropDownList></td>
        </tr>
        
        <tr>
            <td>Location PLC</td>
            <td>
                <asp:DropDownList ID="ddLocationPLC" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Lease Rent</td>
            <td>
                <asp:TextBox ID="txtLeaseRent" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Club Membership</td>
            <td>
                <asp:TextBox ID="txtClubMembrship" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>FFC</td>
            <td>
                <asp:TextBox ID="txtFFC" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>EEC</td>
            <td>
                <asp:TextBox ID="txtEEC" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>PowerBackup
                <input type="radio" name="radioPB" value="1" checked="checked">1 KVA
                <input type="radio" name="radioPB" value="2">2 KVA
                 <input type="radio" name="radioPB" value="5">5 KVA


            </td>
            <td>
                <asp:TextBox ID="txtPowerBackup" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Car Parking</td>
            <td>
                <asp:DropDownList ID="ddCarParking" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Estimated price</td>
            <td>
                <b><asp:Label ID="lblEstimatedPrice" runat="server" Text=""></asp:Label></b>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblSalePrice" runat="server" Text="Label"></asp:Label>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
</asp:Content>--%>

