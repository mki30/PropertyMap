<%@ Page Title="" Language="C#" MasterPageFile="~/Edit/MasterPageEdit.master" AutoEventWireup="true" CodeFile="EditBlog_Old.aspx.cs" Inherits="Edit_EditBlog" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tinymce/tinymce.min.js"></script>
    <script src="../tinymce/createedit.js"></script>
    <script type="text/javascript">
        var CityList = null;

        $(document).ready(function ()
        {
            //fillCity(0, $('#ddCity'));

            //$("#ddCity").on("change", function ()
            //{
            //    fillCity($(this).val(), $('#ddSubCity')) //fill SubCity
            //});
        });
        createEditor("#txtEditor");

        //function fillCity(parentId, listtofill)
        //{
        //    $(listtofill).empty()
        //    $.ajax({
        //        url: "../Data.aspx?Action=CityList&Data1=" + parentId, cache: false, success:
        //        function (Data)
        //        {
        //            var list = JSON.parse(Data);
        //            $(list).each(function ()
        //            {
        //                //if (parentId = 0)
        //                //    if (this.ID == 1 || this.ID == 4 || this.ID == 173 || this.ID == 392)
        //                //        return;
        //                $(listtofill).append($('<option></option>').val(this.ID).html(this.Name)
        //            );
        //            });
        //        }
        //    });
        //}

        //function CheckSubcity()
        //{
        //    alert();
        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <div>
                    <%--<select id="ddCity" nane="City" runat="server"></select>
                    <select id="ddSubCity" runat="server"></select>--%>
                    <asp:DropDownList ID="ddCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddCity_SelectedIndexChanged"></asp:DropDownList>
                    <asp:DropDownList ID="ddSubCity" runat="server"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="SubCityValidator" runat="server" ErrorMessage="Subcity not selectd!" ControlToValidate="ddSubCity" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <div>
                        <asp:TextBox ID="txtHeader" runat="server" Style="width: 600px;"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="HeaderValidator" runat="server" ErrorMessage="Title is empty!" ControlToValidate="txtHeader" ForeColor="#FF3300" Display="None"></asp:RequiredFieldValidator>
                    </div>
                    <asp:TextBox ID="txtEditor" runat="server" TextMode="MultiLine" Style="width: 600px;"></asp:TextBox>
                </div>
                <asp:Button ID="btnSavePost" runat="server" Text="Save" OnClick="btnSavePost_Click" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="#FF3300" />
            </td>
            <td style="vertical-align: top;">
                <asp:TreeView ID="ArchieveTree" runat="server" OnSelectedNodeChanged="ArchieveTree_SelectedNodeChanged">
                </asp:TreeView>
            </td>
        </tr>
    </table>
</asp:Content>

