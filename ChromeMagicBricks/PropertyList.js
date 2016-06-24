var URLlist = new Array();

chrome.extension.onRequest.addListener(
function (request, sender, sendResponse)
{
    var Download = false;
    alert(request.Action);
    switch (request.Action)
    {
        case "MoveNext":
            Download = true;
            break;
        case "GetPages":

            URLs = request.Data;
            alert(URLs);
            Download = true;
            break;
    }
    //    if (Download)
    //    {
    //        if (URLs.length > 0)
    //        {
    //            alert("loading page" + URLs[0]);
    //            DownloadPage(URLs[0]);
    //            URLs.splice(0, 1);
    //        }
    //    }

    sendResponse({});
});

//function DownloadPage(URL)
//{
//    chrome.tabs.getSelected(null, function (tab)
//    {
//        chrome.tabs.executeScript(tab.id, { code: "location.href = '" + URL + "';" }); 
//    });
//}