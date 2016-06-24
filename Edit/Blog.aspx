<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Blog.aspx.cs" Inherits="Edit_Blog" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../jstree/style.css" rel="stylesheet" />
    <style>
        body
        {
            margin: 0;
            margin-top: 1px;
            background-color: white;
        }
        #ctrls
        {
            background: #ebebeb;
        }
        #btnadd
        {
            cursor: pointer;
        }
    </style>
     <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="ctrls">
                <input type="button" id="btnadd" title="click to add new blog" value="add new" />
            </div>
            <div>
                <div class="row-fluid">
                    <div class="span3">
                        <div id="nav1"></div>
                    </div>
                    <div class="span9">
                        <div id="edit">*</div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script src="../js/jquery-1.9.1.min.js"></script>
<script src="../jstree/jquery.jstree.js"></script>
<script src="../bootstrap/js/bootstrap.min.js"></script>
<script>
    $(document).ready(function ()
    {
        $("#btnadd").click(function ()
        {
            $("#edit").html("<iframe id='framedialog' style='width:" + ($(window).width() - 400) + "px; height:" + ($(window).height() - 50) + "px;'  frameborder='0' src='/edit/editblog.aspx?id=0'></iframe>");
        });
        buildtree();
    });

    function buildtree()
    {
        $('#nav1').jstree({
            "plugins": ["json_data", "themes", "ui", "crrm", "dnd", "localStorage"],
            "themes": { "theme": "default" },
            "contextmenu": { "select_node": "true" },
            "json_data":
            {
                "ajax":
                {
                    "url": "/edit/blog.aspx",
                    "data": function (node)
                    {
                        return {
                            "data5": "10"
                        };
                    }
                }
            },
            "localStorage": {
                "localStorage_options": {
                    "prefix": "blog"
                }
            }
        }).bind("select_node.jstree", function (e, data)
        {
            try
            {
                var id = data.rslt.obj.attr("id");
                if (id != undefined)
                    $("#edit").html("<iframe id='framedialog' style='width:" + ($(window).width() - 400) + "px; height:" + ($(window).height() - 50) + "px;' frameborder='0' src='/edit/editblog.aspx?id=" + id + "'></iframe>");
            } catch (x)
            {
            }
        });
    }

</script>
</html>
