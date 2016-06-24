
var TabID = 0;

chrome.extension.onMessage.addListener(
  function (request, sender, sendResponse)
  {
      TabID = sender.tab.id;
        switch (request.Action)
        {
            case "TabCount":
                var Count = 0;

                chrome.tabs.getAllInWindow(null, function(tabs) 
                {
                    chrome.tabs.sendMessage(TabID, { Data: tabs.length },function (response) { });

                });
                return;
        }
        sendResponse({});
});

