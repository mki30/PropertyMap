<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageImages.aspx.cs" Inherits="Edit_ManageImages" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.1.min.js"></script>
    <script src="../js/jquery-ui-1.9.2.custom.min.js"></script>
    <asp:Literal ID="ltmce" runat="server" Visible="false">        
      <script src="../tinymce/tinymce.min.js"></script>        
        <script>
            tinymce.init({
                selector: "#txtImgDesc",
                theme: "modern",
                height: 200,
                inline_styles: false,
                paste_as_text: true,
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor"
                ],
                toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview | forecolor backcolor",
                toolbar_items_size: 'small',
                image_advtab: true
            });

            $(document).ready(function ()
            {
                //BuildTree($("#lblDetailID").html() + "imgd", "#Node9", 9, -1);
            });

            </script>
    </asp:Literal>
    <style>
        .FieldContainer {
            font-family: tahoma;
        }

        .imDV {
            margin: 3px;
            border: 1px dashed #0da3fd;
            background-color: #e8efff;
            width: auto;
            float: left;
        }

        .no-btn {
            border: none;
            background: #f5f5f5;
            box-shadow: none;
            padding: 0;
        }

        .selected {
            border: 1px dashed red;
        }

        .none {
            display: none;
        }

        #divEditImg {
            padding: 5px;
        }

            #divEditImg table {
                width: 100%;
            }

                #divEditImg table textarea, #divEditImg table input[type='text'] {
                    width: 95%;
                }

                #divEditImg table input.W_5 {
                    width: 46%;
                }

        input[type="text"]:focus, input[type="password"]:focus {
            border-color: none;
            box-shadow: none;
        }

        .well {
            margin-bottom: 0;
        }

        .mgt-5 {
            margin-top: -5px;
        }

        #btnAddImage {
            margin-left: 15px;
        }

        .padtop {
            padding-top: 45px !important;
        }
    </style>
    <script>var reloadWindow = 0;</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="well well-small navbar-fixed-top">
                <div class="pull-left btn no-btn">
                    <%--<b>CompID: </b>
                    <asp:Label ID="lblCompID" runat="server" Text="0"></asp:Label>
                    |--%> <b>RefID: </b>
                    <asp:Label ID="lblRefID" runat="server" Text="0"></asp:Label>
                    | <b>Type: </b>
                    <asp:Label ID="lblImageType" runat="server" Text="0"></asp:Label>
                    <input type="hidden" id="txtCompanyID" runat="server" />
                    <input type="hidden" id="txtReferenceID" runat="server" />
                    <input type="hidden" id="txtImageTypeID" runat="server" />
                    <input type="hidden" id="txtImageLocation" runat="server" />
                    <input type="hidden" id="txtSelectedImg" runat="server" />
                </div>
                &nbsp;&nbsp;
                <asp:Panel ID="pnlcmn" runat="server" CssClass="pull-right mgt-5">
                    <a href="javascript:void(0)" id="btnADD" class="btn"><i class="fa fa-plus"></i>&nbsp;Add Image</a>
                    <a href="javascript:void(0)" id="btnSave" class="btn"><i class="fa fa-save"></i>&nbsp;Update Image Seq.</a>
                    <a href="javascript:void(0)" id="btnEdit" class="btn"><i class="fa fa-edit"></i>&nbsp;Edit Image</a>
                </asp:Panel>
                <asp:Panel ID="pnledt" runat="server" Visible="false" CssClass="pull-right mgt-5">
                    <a href="javascript:void(0)" id="btnBK" class="btn"><i class="fa fa-step-backward"></i>&nbsp;Back</a>
                    <button id="btnDoneEdit" runat="server" class="btn" onserverclick="btnDoneEdit_ServerClick"><i class="fa fa-save"></i>&nbsp;Update Image Details</button>
                </asp:Panel>
            </div>
            <div id="imgDiv" class="FieldContainer clearfix padtop" runat="server"></div>
            <div id="divEditImg" class="clearfix padtop" runat="server" visible="false">
                <div style="position:fixed; background:#fff;position: fixed;background: #fff;width: 100%;z-index: 99;">
                    <table>
                        <tr>
                            <td style="width: 100px;">
                                <img id="imgPrev" src="/images/grey.gif" runat="server" width="150" height="150" />
                            </td>
                            <td style="vertical-align: top; padding: 5px">
                                <p>
                                    <b>Detail ID: </b>
                                    <asp:Label ID="lblDetailID" runat="server" Text="0"></asp:Label>
                                    <asp:Button ID="btnDelete" Visible="false" OnClientClick="return confirm('Confirm Delete')" OnClick="btnDelete_Click" runat="server" Text="Delete" />
                                </p>
                                <p>
                                    <b>Size(WxH): </b>
                                    <asp:Label ID="lblSize" runat="server" Text="0x0"></asp:Label>
                                </p>
                                <p>
                                    <span class="pull-left">
                                        <b>ImageID: </b>
                                        <asp:Label ID="lblImgID" runat="server" Text="0"></asp:Label>
                                    </span>
                                    <span class="pull-left">
                                        <span class="none">
                                            <asp:FileUpload ID="drpImg" runat="server"></asp:FileUpload>
                                        </span>
                                        <asp:Button ID="btnAddImage" Text="Add Image" runat="server" />
                                    </span>
                                </p>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="padding-top:103px;">
                    <table>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="lblStatus" ForeColor="Red" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-bottom: 10px;">
                                <b>Description</b><br />
                                <asp:TextBox ID="txtImgDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Lat / Lng </td>
                            <td>

                                <input id="txtLat" runat="server" type="text" class="W_5" />
                                <input id="txtLng" runat="server" type="text" class="W_5" />
                            </td>
                        </tr>
                        <tr>
                            <td>Courtesy</td>
                            <td>
                                <input id="txtCourtesy" type="text" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Tagline</td>
                            <td>
                                <input type="text" id="txtTagline" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Reference Link</td>
                            <td>
                                <input type="text" id="txtReferenceLink" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Url Name:</td>
                            <td>
                                <input type="text" id="txtUrlName" runat="server" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2">
                                <b>Link With Menu:-</b><br />
                                <div id="Node9" class="menu"></div>
                            </td>
                        </tr>--%>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
<script>
    function imgs(imageID, newIndex)
    {
        this.id = imageID;
        this.seq = newIndex;
    }
    var ImagesArray = new Array();
    $(document).ready(function ()
    {
        $(".FieldContainer").sortable({ items: ".imDV", distance: 10 });
        $(".FieldContainer .imDV").click(function ()
        {
            $(".imDV").removeClass("selected");
            $(this).addClass("selected");
            $("#txtSelectedImg").val($(this).data("id"));
        });
        $("#drpImg").on('change', function ()
        {
            var imgval = $("#txtSelectedImg").val();
            var txt = (imgval == "0_0.jpg" ? "Add Image " : "Replace With ");
            if (this.value == "")
                txt = "Change Image";
            $("#btnAddImage").val(txt + this.value.substr(12, this.value.length));
        });

        $("#btnEdit,#btnBK,#btnADD").click(function ()
        {
            debugger;
            var url = "manageImages.aspx?referenceid=" + $("#txtReferenceID").val() + "&imagetype=" + $("#txtImageTypeID").val();
            if ($(this).attr("id") == "btnEdit")
            {
                var imgID = $("#txtSelectedImg").val();
                if (imgID == undefined || imgID == "")
                {
                    alert("Please Select Image to Edit");
                    return;
                }
                url += "&img=" + imgID;
            }
            else if ($(this).attr("id") == "btnADD")
            {
                url += "&img=0_0.jpg";
            }
            window.location.replace(url);
        });

        $("#btnSave").click(function ()
        {
            if (confirm("Confirm Save Image Sequence"))
            {
                ImagesArray.length = 0;
                $("#result").html();
                $(".FieldContainer .imDV").each(function (index)
                {
                    ImagesArray.push(new imgs($(this).data("id"), (index + 1)));
                });
                if (ImagesArray.length > 0)
                {
                    $.post('/data.aspx?Action=SAVE_IAMGE_DETAILS', {

                        imgData: JSON.stringify(ImagesArray),
                        CompanyID: $("#txtCompanyID").val(),
                        ReferenceID: $("#txtReferenceID").val(),
                        ImageType: $("#txtImageTypeID").val()

                    }, function (r)
                    {
                        alert(r);
                        window.top.ReloadProjectImages();
                    });
                }
            }
        });
        if (reloadWindow == 1)
        {
            getBacka();
            window.top.ReloadProjectImages();
        }
        if (reloadWindow == 2)
        {
            window.top.ReloadProjectImages();
        }

    });
    function getBacka()
    {
        window.location.replace("manageImages.aspx?referenceid=" + $("#txtReferenceID").val() + "&imagetype=" + $("#txtImageTypeID").val());
    }
    function AddSearchedValue(MenuID, type)
    {
        var imgID = parseInt($("#lblDetailID").html());
        if (imgID != 0)
        {
            $.post("/data.aspx?Action=LINK_IMAGES_CATEGORIES&Data1=" + type,
                  {
                      MenuID: MenuID,
                      ImageID: imgID
                  }, function (r)
                  {
                      $.notific8(r, { life: 3000 });
                  });
        }
    }
</script>
</html>


