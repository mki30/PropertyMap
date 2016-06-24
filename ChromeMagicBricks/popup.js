function click(e)
{
    chrome.tabs.executeScript(null, { code: $(e.target).data("script") });
    //window.close();
}

document.addEventListener('DOMContentLoaded', function ()
{
  var divs = document.querySelectorAll('div');
  for (var i = 0; i < divs.length; i++) {
    divs[i].addEventListener('click', click);
  }
});
