<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PostProperty.aspx.cs" Inherits="PostProperty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function RefreshIframe(SocietyID)
        {
            console.log(sellerId + "-" + societyId + "-" + city + "-" + area);
            $(".iframe1")[0].src = "Edit/EditAvailability.aspx?CSS=1&ReqType=user&SocietyID=" + societyId+"&City="+city+"&Area="+area+"&SellerID="+sellerId+"&SellerType="+sellerType;
        }
        var city, societyId, area,sellerId,sellerType;

        $(document).ready(function ()
        {
            RefreshIframe();
        });

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div style="background: -webkit-gradient(linear, left top, left bottom, from(#DBE7B3), to(white)); padding-left:10px;">
        <div style="height: 50px; text-align: left;">
            <br />
            <span style="margin-left: 10px; font-size: 20px;">Posting</span>
        </div>
        <div style="width: 500px; text-align: center; border: 2px solid #c3c2c2; background-color:#e1e1d8; border-radius: 5px; padding-top:10px;">
            <div>
                <div>
                    <span style="float:left; margin-left:80px;margin-top:5px;">City</span>
                    <span style="margin-left:-60px;">
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="drp" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                    <asp:ListItem>New Delhi</asp:ListItem>
                    <asp:ListItem>Noida</asp:ListItem>
                    <asp:ListItem>Ghaziabad</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                </div>
                <br/>
                <div>
                    <span style="float:left; margin-left:80px;margin-top:5px;">Area</span>
                    <span style="margin-left:-65px;">
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="drp" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                        <asp:ListItem>Indrapuram</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                </div>
            </div>
            <div>
               <iframe id="Iframe1" class="iframe1" style="width: 350px; height: 600px; border: 0px;" runat="server"></iframe>
           </div>
            </div>
        </div>
    <br/>
</asp:Content>
