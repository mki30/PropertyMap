<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignAvailability.aspx.cs" Inherits="AssignAvailability" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/js/jquery-1.9.1.min.js"></script>
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <title></title>
    <style type="text/css">
        /*td
        {
            white-space: nowrap;
        }*/
        body
        {
            font-size:10px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('.chk').bind('change', function ()
            {
                var dataids = $(this).data('id');

                var initialstate=$(this).prop('checked');
                var task = 'add';
                if ($(this).prop('checked') == true)
                    task = 'add';
                else
                    task = 'remove';
                
                $.post("./Data.aspx?Action=SaveAssignClient&Data1=" + task + "&Data2=" + dataids, function (data)
                {
                    if (data.replace('~','').trim() == "")
                    {
                    }
                    else
                    {
                        alert("Error:" + data);
                        //$(this).prop('checked') = initialstate;
                    }
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <asp:Literal ID="ltAssignAvailability" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
