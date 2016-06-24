var ShowData = true;
var reload = true;
function BuildTree(CityID, ID, MenuType)
{
    return $(ID)
       .bind("before.jstree", function (e, data)
       {
           $("#alog").append(data.func + "<br />");
       })
       .jstree({
           "plugins": ["themes", "json_data", "ui", "crrm", "dnd", "search", "types", "hotkeys", "contextmenu"],
           "themes": {
               "theme": "default",
               "dots": true,
               "icons": true
           },
           contextmenu: {
               items: MenuType != 4 ? {
                   "seqUpdate": {
                       "label": "Update Sequence",
                       "action": function (obj, data)
                       {
                           //ShowUpdataSequence(obj.attr("dataid"), obj.attr("id"), obj.attr("sequence"), obj.attr("menutype"));
                       }
                   },
                   "ccp": false
               } : { "ccp": false }
           },
           "json_data": {
               "ajax": {
                   "url": BasePath+"Data.aspx?Action=GET_CHILDREN&Data2=" + CityID,
                   "data": function (n)
                   {
                       return {
                           "Data1": n.attr ? n.attr("id").replace("node_", "") : 0,
                       };
                   },
               }
           },
           "types": {
               "types": {
                   "default": {
                       "icon": {
                           "image": "../PropertyMap/images/folder.png"
                       }
                   },
               }
           },
           "cookies": {
               "cookie_options": {
                   "path": "/"
               }
           },
       })
.bind("select_node.jstree", function (e, data)                       //Select node
{
    try
    {
        console.log(data.rslt.obj.attr("ChildSocietyCount"));
    }
    catch (e) { }
}).bind("rename.jstree", function (e, data)                            //rename node
{
    $.post(
            "../Data.aspx?Action=MENU_ACTION&Data1=1&Data2=" + data.rslt.obj.attr("id"),
            {
                "New_Name": data.rslt.new_name
            }, function (r)
            {
                if (!r == "ok")
                    $.jstree.rollback(data.rlbk);
                else
                    data.inst.refresh();
            });
}).bind("create.jstree", function (e, data)
{
    $.post("../Data.aspx?Action=MENU_ACTION&Data1=2&Data2=" + data.inst._get_parent(data.rslt.obj).attr("id"),    //create new node
                {
                    "New_Name": data.rslt.name,
                    "BrandID": BrandID
                }, function (r)
                {
                    if (!r == "ok")
                        $.jstree.rollback(data.rlbk);
                    else
                        data.inst.refresh();
                });
}).bind("remove.jstree", function (e, data)                                          //remove node
{
    if (confirm("Confirm Delete! Note: if it has child rows then it can't be deleted."))
    {
        $.post("../Data.aspx?Action=MENU_ACTION&Data1=3&Data2=" + data.rslt.obj.attr("id"),
             {
                 "New_Name": data.rslt.name
             }, function (r)
             {
                 if (r != "ok")
                 {
                     alert("Not able to delete, multiple childs found.");
                     $.jstree.rollback(data.rlbk);
                 }
                 else
                     data.inst.refresh();
             });
    }
    else
        $.jstree.rollback(data.rlbk);
}).bind("move_node.jstree", function (event, data)                             //move node by drag n drop
{
    var thisID = data.rslt.o.attr("id"), ToID = data.rslt.np.attr("id").replace("Node", "");
    if (ToID == 1) ToID = 0;

    var thisName = data.rslt.o.attr("name"), thisParent = data.rslt.o.attr("pid"), ToName = data.rslt.np.attr("name");
    if (ToID == 0) ToName = "Home";
    if (thisParent == ToID)
    {
        alert("Already Linked to this Menu");
        return;
    }
    if (confirm("Moving \"" + thisName + "\"" + " TO \"" + ToName + "\""))
    {
        console.log(thisID, ToID);
        $.post("../Data.aspx?Action=MENU_ACTION&Data1=4",
             {
                 "ID": thisID,
                 "TOID": ToID
             }, function (r)
             {
                 if (r != "ok")
                 {
                     alert("Not able to change parent, you may try again.");
                     $.jstree.rollback(data.rlbk);
                 }
                 else
                     data.inst.refresh();
             });
    }
    else
        $.jstree.rollback(data.rlbk);
})
}
