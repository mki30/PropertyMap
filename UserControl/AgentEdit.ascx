<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgentEdit.ascx.cs" Inherits="UserControl_AgentEdit" %>

<style type="text/css">
    #agentImg
    {
        width: 200px;
        height: 100px;
    }
</style>

<script type="text/javascript" src="//maps.googleapis.com/maps/api/js?sensor=false"></script>
<script type="text/javascript" src='<%=ResolveClientUrl("~/js/AgentPage.js")%>'></script>
<script src="/js/jquery.dataTables.min.js"></script>

<script type="text/javascript">

    $(document).ready(function ()
    {
        $(".fancybox1").fancybox(
         {
             width: '310',
             autoDimension: true
         });

         $(".fancybox2").fancybox(
         {
             width: '350',
             autoDimension: true
         });
    });

    function SetTab(tab)
    {
        if (tab == "prof")
        {
            $("#Post").removeClass("active");
            $("#Prof").addClass("active");
        }
    }

    function GetClientAvl(AgentID, ClientID)
    {
        $.ajax({
            url: "/Data.aspx?Action=GetClientAssign&Data1=" + AgentID + "&Data2=" + ClientID + "", async: false, cache: false, success: function (data)
            {
                $("#ClientAvl").html(data.replace("~", ""));
            }
        });
    }

    function ShowHiddenTextBox(id)
    {
        $("#edit" + id).show();
        $("#desc" + id).hide();
    }

    function SaveDesc(ID)
    {
        $.post("/Data.aspx?Action=UpdateAssignClientDesc&Data1=" + ID,
        {
            Desc: $("#descText").val()
        },
        function (data)
        {
            //if (data == "")
            //alert("Saved");
        });
    }
</script>

<span class="pull-right">
    <a id="addNewClient" class='fancybox1 fancybox.iframe btn' runat="server">New Client</a>
    <a id="addNewPost" class='fancybox2 fancybox.iframe btn' runat="server">Post New Property</a>
    <asp:Button ID="btnLogout" CssClass="btn" Text="Log Out" runat="server" Style="margin-left: 5px;" OnClick="btnLogout_Click" />
</span>
<ul id="myTab" class="nav nav-tabs">
    <li class="active"><a href="#Post" data-toggle="tab">Postings</a></li>
    <li><a href="#Clients" data-toggle="tab">Clients</a></li>
    <li><a href="#Prof" data-toggle="tab">Profile</a></li>
</ul>

<script type="text/javascript">
    var Lat = "<%=Lat%>"
    Lng = "<%=Lng%>"
    Edit = "<%=Edit%>"
    AgentID = "<%=agent.ID%>"
</script>

<div id="myTabContent" class="tab-content">
    <div class="tab-pane fade in active" id="Post">
        <div class='row-fluid'>
            <div class='span12'>
                <div id='Postings' style='width: 100%;'>
                    <asp:Literal ID="ltAgentPostingEdit" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <div class="tab-pane fade" id="Prof">
        <div class="row-fluid" id="profile">
            <div class="span12">
                <div id="divEdit" style="margin-right: 20px; margin-left: 20px;" runat="server">
                    <div class="row-fluid">
                        <div class="span12 page-header">
                            <h4>About<span class="text-success" id="spnCompName" runat="server"></span>
                                <input type="button" class="btn btn-info pull-right" value="Preview" onclick="ShowPreview()" />
                                <asp:Button ID="btnSave" class="btn btn-warning pull-right" runat="server" OnClick="btnSave_Click" Text="Save" Style="margin-right: 10px;" />
                                &nbsp;</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span3">
                            <label for="txtCompanyName">Company Name</label>
                            <input type="text" id="txtCompanyName" runat="server" placeholder="Company Name" />
                        </div>
                        <div class="span3">
                            <label for="txtAgentName">Agent Name</label>
                            <input type="text" id="txtAgentName" runat="server" placeholder="Agent Name" />
                        </div>
                        <div class="span6">
                            <div class="pull-right">
                            <span class="label label-warning" id="spnAgentID" runat="server">0</span>
                            <span id="spnEmailID" runat="server" class="label"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12 page-header">
                            <h4>Address</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <label for="txtAddress">Address</label>
                            <textarea rows="5" type="text" id="txtAddress"  runat="server" placeholder="Address" class="span12" style="width:50%;height:50px;" ></textarea>
                        </div>
                    </div>

                    <div class="row-fluid">
                        <div class="span3">
                            <label for="ddCity">City</label>
                            <asp:DropDownList ID="ddCity" runat="server"></asp:DropDownList>
                        </div>
                        <div class="span3">
                            <label for="txtArea">Area</label>
                            <input type="text" id="txtArea" runat="server" placeholder="Area" class="span12" />
                        </div>
                        <div class="span3">
                            <label for="txtPinCode">Pin</label>
                            <input type="text" id="txtPinCode" runat="server" placeholder="Pin Code" class="span12" />
                        </div>
                    </div>

                    <div class="row-fluid">
                        <div class="span12 page-header">
                            <h4>Contact</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span3">
                            <label for="txtPhone1">Phone1</label>
                            <input type="text" id="txtPhone1" runat="server" placeholder="Phone1" />
                        </div>
                        <div class="span3">
                            <label for="txtPhone2">Phone2</label>
                            <input type="text" id="txtPhone2" runat="server" placeholder="Phone2" />
                        </div>
                        <div class="span3">
                            <label for="txtMobile1">Mobile1</label>
                            <input type="text" id="txtMobile1" runat="server" placeholder="Mobile1" />
                        </div>
                        <div class="span3">
                            <label for="txtMobile2">Mobile2</label>
                            <input type="text" id="txtMobile2" runat="server" placeholder="Mobile2" />
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <label id="lblCheckBoxes" runat="server"></label>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12 page-header">
                            <h4>Details</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <textarea id="txtDetails" runat="server" style="height: 100px; width: 98%;"></textarea>
                        </div>
                    </div>
                    <div class="row-fluid" id="divUpload" runat="server">
                        <div class="span12 page-header">
                            <h4>Image</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12 well well-small">
                            <asp:FileUpload ID="uploadImage" runat="server" CssClass="btn" />
                            <asp:Button CssClass="btn btn-inverse" Text="Upload Image" ID="UploadImageBtn" runat="server" OnClick="UploadImageBtn_Click" />
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12 page-header">
                            <h4>Location</h4>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class='span12'>
                            Latitude  :<input type="text" id="txtLat" runat="server" />&nbsp;&nbsp; Longitude :<input type="text" id="txtLng" runat="server" />
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class='span12'>
                            <div id='map_canvas_edit' style='width: 100%; height: 350px;'></div>
                            <h4>Find address on Map</h4>
                            <input type="text" style="width: 450px;" id="txtCodeAddress" />
                            <input type="button" class="btn btn-inverse" value="Locate Address" onclick="CodeAddress()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="tab-pane fade" id="Clients">
        <div class='row-fluid'>
            <div class="span12">
                <asp:Literal ID="ltClients" runat="server"></asp:Literal>
            </div>
            <div id="ClientAvl" style="display: none">
                Avl
            </div>
    </div>
    </div>
</div>





