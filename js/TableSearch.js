$(function ()
{
    $("#txtName").on("keyup", function (e)
    {
        var txt = $(this).val().toUpperCase().trim();

        $("#DataTable").css("display", "none"); // hide table
        //$("#DataTable").removeHighlight();

        $("#DataTable tr").not(':first').each(function (index)
        {
            $(this).css("display", $(this).text().toUpperCase().indexOf(txt) > -1 || txt.length==0 ? "" : "none");
        });

        $("#DataTable").css("display", ""); // show table
    });
});

$.fn.removeHighlight = function ()
{
    return this.find(".highlight").each(function ()
    {
        this.parentNode.firstChild.nodeName;
        with (this.parentNode)
        {
            replaceChild(this.firstChild, this);
            normalize();
        }
    }).end();
};

$.fn.highlighttext = function (pat)
{
    if (pat != "")
    {
        function innerHighlight(node, pat)
        {
            var skip = 0;
            if (node.nodeType == 3)
            {
                var pos = node.data.toUpperCase().indexOf(pat);
                if (pos >= 0)
                {
                    var spannode = document.createElement('span');
                    spannode.className = 'highlight';
                    var middlebit = node.splitText(pos);
                    var endbit = middlebit.splitText(pat.length);
                    var middleclone = middlebit.cloneNode(true);
                    spannode.appendChild(middleclone);
                    middlebit.parentNode.replaceChild(spannode, middlebit);
                    skip = 1;
                }
            }
            else if (node.nodeType == 1 && node.childNodes && !/(script|style)/i.test(node.tagName))
            {
                for (var i = 0; i < node.childNodes.length; ++i)
                {
                    i += innerHighlight(node.childNodes[i], pat);
                }
            }
            return skip;
        }
        return this.each(function ()
        {
            innerHighlight(this, pat.toUpperCase());
        });
    }
};