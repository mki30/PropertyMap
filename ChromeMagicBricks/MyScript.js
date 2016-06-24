///<reference path="../js/jquery-1.8.0.min.js" />
var TimerOn = 0;
var basePath = "http://localhost:38592/"

$(document).ready(function ()
{
    //FilterPropertyDetail();

    if (location.href.indexOf("Property-Rates-Trends") > -1)  //start price trend detail download
        ProcessMagicBricksPriceTrends();
});

function FilterPropertyDetail()
{
    var Data = "";
    var Action = "";
    var Domain = "magicbricks.com";
    if (location.href.indexOf(Domain) > -1)
    {
        Action = Domain;

        $("div.propId").each(function ()
        {
            var txt = $(this).text();
            if (txt.indexOf("Property ID") != -1)
            {
                Data = ProcessPropertyDetailsMagicBrick();
            }
        });
        //Data = ProcessProjectDetailsMB();
        //alert(Data);
    }

    Domain = "indiaproperty.com";

    if (location.href.indexOf(Domain) > -1)
    {
        Action = Domain;
        $("div.textright.paddb10").each(function ()
        {
            var a = $(this).text().split('|')[0];
            var txt = a;
            if (txt.indexOf("Property ID") != -1)
            {
                Data = ProcessPropertyDetailsIndiaProperty();
            }
        });
    }

    Domain = "99acres.com";
    if (location.href.indexOf(Domain) > -1)
    {
        Action = Domain;
        Data = ProcessPropertyDetails99Acre();
    }

    Domain = "makaan.com";
    if (location.href.indexOf(Domain) > -1)
    {
        Action = Domain;
        Data = ProcessPropertyDetailsMakaan();
    }

    if (Data != "")
    {
        $.post("http://localhost:4566/PropertyMap/Data.aspx?Action=UpdatePropertyAvailability&Data1=" + Action, { PropertyData: Data }, function (data)
        {
            setTimeout("window.close()", 100);
        });
    }
}

var URls = new Array();

function FilterPropertyLinksMagicBrick()                                        //filter all links
{
    URls.length = 0;
    $("div.propSearchMainContent").each(function ()
    {
        var Link = "";
        $("div.propSearchDtlHead a", $(this)).each(function ()
        {
            Link = $.trim($(this).attr('href'));
        });

        var PropID = "0";

        $("div.searchDetailPanelRgt", $(this)).each(function ()
        {
            var t = $.trim($(this).text());
            var n = t.indexOf('|');
            if (n > 0)
            {
                t = $.trim(t.substring(4, n));
            }
            PropID = t;
        });
        URls.push({ Link: Link, PropID: PropID, Domain: "MB" });
    });

    //alert(URls);
    LoadPropertyDetailPage();
}

function FilterPropertyLinksIndiaProperty()
{
    $("div.srdivborderbg1").each(function ()
    {
        var Link = "";
        $("div.fleft a", $(this)).each(function (index)
        {
            if (index == 1)
            {
                Link = "http://www.indiaproperty.com" + $.trim($(this).attr('href'));
            }
        });
        var PropID = "0";

        $("div.fright.smalltxt1.paddt5.clr6.paddb10", $(this)).each(function ()
        {
            var a = $(this).text().split('|')[0]
            PropID = $.trim(a.replace("Property ID:", ""));
        });
        URls.push({ Link: Link, PropID: PropID, Domain: "IP" });
    });
    //alert(URls);
    //console.log(URls);
    LoadPropertyDetailPage();
}

function FilterPropertyLinks99Acre()
{
    var Link = "";
    $("div.sT a.f14").each(function ()
    {
        Link = "http://www.99acres.com" + $.trim($(this).attr('href'));
        var PropID = Link.substring(Link.lastIndexOf('-')).replace("-", "").trim();
        URls.push({ Link: Link, PropID: PropID, Domain: "99" });
    });
    //alert(URls);
    console.log(URls);
    LoadPropertyDetailPage();
}

function FilterPropertyLinksMakaan()
{
    var Link = "";

    $("div.typefor a").each(function ()
    {
        Link = $.trim($(this).attr('href'));
        var a = Link.substring(0, Link.lastIndexOf('/'));
        var PropID = a.substring(a.lastIndexOf('/') + 1).split('-')[1].split('-')[0];
        URls.push({ Link: Link, PropID: PropID, Domain: "MK" });
    });
    console.log(URls);
    LoadPropertyDetailPage();
}


function LoadPropertyDetailPage()
{
    if (URls.length > 0)
    {
        chrome.extension.sendMessage({ Action: "TabCount" }, function (response) { });
    }
}

function DownloadPage()
{
    if (URls.length > 0)
    {

        var URL = URls[0];
        URls.splice(0, 1);

        console.log(URls.length + "," + URL.PropID);

        $.post("http://localhost:4566/PropertyMap/Data.aspx?Action=CheckIfPropertyAlreadyDownloaded&Data1=" + URL.Domain + "&Data2=" + URL.PropID, function (data)
        {
            var Lines = data.split('~')[1];

            console.log(data);
            if (Lines == "Not Found")
            {
                window.open(URL.Link);
            }
        });
    }
}


chrome.extension.onMessage.addListener(                                                         //88
  function (request, sender, sendResponse)
  {
      if (parseInt(request.Data) < 8)
      {
          DownloadPage();
      }

      sendResponse({});

      setTimeout("LoadPropertyDetailPage()", 500);
  });


function ProcessPropertyDetailsMakaan()
{
    //alert();
    str = "";
    var PropID = "", Price = "", Agent = "", Details = "";

    $("table").each(function (index)
    {
        var tdText = $.trim($("td:first", this).text());

        if (tdText.indexOf("Makaan ID") == 0)
        {
            PropID = GetCleanTable(this);
        }
        else if (tdText.indexOf("Rs.") == 0)
        {
            Price = GetCleanTable(this);
        }
        else if (tdText.indexOf("Name :") == 0)
        {
            Agent = GetCleanTable(this);
        }
        else if (tdText.indexOf("Floor Number") == 0)
        {
            Details = GetCleanTable(this);
        }
        str = PropID + "$" + Price + "$" + Agent + "$" + Details;
    });

    return str == "$$$" ? "" : str;
}

function ProcessPropertyDetailsIndiaProperty()
{
    var str = "";

    $("div.box1.padd5").each(function ()
    {
        $("p.paddb10").each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.mediumtxt").each(function ()
        {
            $("p.paddb5", $(this)).each(function (index)
            {
                if (index == 0)
                    str += $.trim($(this).text()) + "$";
                if (index == 1)
                    str += $.trim($(this).text()) + "$";
                if (index == 2)
                    str += $.trim($(this).text()) + "$";
            });
        });
        str += "^";
        $("div.boxBig.borderb").each(function ()
        {
            $("div.fleft", $(this)).each(function (index)
            {
                $("dl.dlSmall", $(this)).each(function (index)
                {
                    if (index == 0)
                        str += $.trim($(this).text()) + "$";
                    if (index == 1)
                        str += $.trim($(this).text()) + "$";
                    if (index == 2)
                        str += $.trim($(this).text()) + "$";
                    if (index == 3)
                        str += $.trim($(this).text()) + "$";
                    if (index == 4)
                        str += $.trim($(this).text()) + "$";
                    if (index == 5)
                        str += $.trim($(this).text()) + "$";
                    if (index == 6)
                        str += $.trim($(this).text()) + "$";
                });
            });
        });
        str += "^";
        $("div.textright.paddb10").each(function ()
        {

            str += $.trim($(this).text());


        });

        str += "^";
        $("div.fleft.divider250").each(function ()
        {

            $("dl.dlSmall", $(this)).each(function (index)
            {

                if (index == 0)
                    str += $.trim($(this).text()) + "$";
                if (index == 1)
                    str += $.trim($(this).text()) + "$";
                if (index == 2)
                    str += $.trim($(this).text()) + "$";
                if (index == 3)
                    str += $.trim($(this).text()) + "$";
                if (index == 4)
                    str += $.trim($(this).text()) + "$";
                if (index == 5)
                    str += $.trim($(this).text()) + "$";

            });
        });

        str += "^";
        $("div.paddt5.padd10").each(function ()
        {
            $("div.boldtxt.paddt10", $(this)).each(function ()
            {
                str += $.trim($(this).text());
            });
            str += "^";
            $("div.paddt10", $(this)).each(function (index)
            {
                if (index == 1)
                    str += $.trim($(this).text());
            });
            str += "^";
            $("div.paddt5", $(this)).each(function ()
            {
                str += $.trim($(this).text());
            });
            str += "^";

        });
    });
    str += "^";
    return str;
}

function ProcessPropertyDetailsMagicBrick()
{

    var str = "";

    $("table.propDetailWrapper").each(function ()
    {
        $("div.propId").each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.propertyBrifInfo h1", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.priceDetail", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("span.pricePerSqFt", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.propertyBrifInfoSecond", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.propertyBrifInfoThird", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";

        $("div.propertyBaseDetail", $(this)).each(function ()
        {
            $("table", $(this)).each(function ()
            {
                $("tr", $(this)).each(function ()
                {
                    $("td", $(this)).each(function ()
                    {
                        str += $.trim($(this).text()) + "$";
                    });
                    str += "@";
                });
            });

        });

        str += "^";
        $("div.agentComName", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.agentName", $(this)).each(function ()
        {
            str += $.trim($(this).text()) + "$";
        });

        $("div.companyAddress div.basicInfoAgent", $(this)).each(function (index)
        {
            if (index == 0)
                str += $.trim($(this).text());
        });
        str += "^";
        $("div.postedOnDate", $(this)).each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";
        $("div.hiddenContactInfo", $(this)).each(function ()
        {
            $("div.basicInfoAgent", $(this)).each(function ()
            {
                str += $.trim($(this).text()) + "$";
            });
        });
        str += "^";
    });
    str += $("div.formHeading").text();

    return str;
}

var ctr = 0;
function ProcessPropertyDetails99Acre()
{

    var sellertype;
    var a = $("div.f11.crv4_bdrgry1px div.prop_de_cont_new.b").text();
    if (a == "Send Email & SMS to this Dealer")
    {
        sellertype = "Dealer";

    }
    if (a == "Send Email & SMS to this Owner")
    {
        sellertype = "Owner";
    }
    if (a == "Send Email & SMS to this Builder")
    {
        sellertype = "Builder";

    }

    $("input.btnsprit.brown1_btn.mt5").each(function ()
    {
        $(this).click();

    });

    if (ctr == 0)
    {
        ctr = 1;
        setTimeout("FilterPropertyDetail()", 1000);
        return "";
    }

    var str = "";
    $("table.mb10.pdt5.detail").each(function ()
    {
        $("span.floatr.pdt10.f11").each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";


        $("div.roundcont.pdt9 h1").each(function ()
        {
            str += $.trim($(this).text());
        });
        str += "^";

        str += $("div#cssBox div.vp3").text();

        str += "^";
        $("div.sepl.f12.m10").each(function ()
        {

            $("div.f14.orange.ml_15").each(function ()
            {
                str += $.trim($(this).text());
            });

            str += "^";
            $("table", $(this)).each(function ()
            {

                $("tr", $(this)).each(function ()
                {
                    $("td", $(this)).each(function ()
                    {
                        str += $.trim($(this).text()) + "$"
                    });

                    str += "@";
                });
            });
        });

        str += "^";

        $("table.mt5.add_detail", $(this)).each(function ()
        {

            $("tr", $(this)).each(function ()
            {
                $("td", $(this)).each(function ()
                {
                    $("tr", $(this)).each(function ()
                    {
                        $("td", $(this)).each(function ()
                        {
                            str += $.trim($(this).text()) + "$"
                        });

                        str += "@";
                    });
                });

            });
        });

        str += "^";


        $("div.m5.f12", $(this)).each(function ()  //filter name and company name
        {
            $("table", $(this)).each(function (index)
            {
                if (index == 0)
                {
                    $("tr", $(this)).each(function (index)
                    {
                        if (index == 0)
                        {
                            $("td", $(this)).each(function ()
                            {
                                str += ($(this).text());
                            });
                        }
                    });
                }
            });
        });

        str += "^";
        if (sellertype != "Owner")
        {
            $("div#ctv").each(function (index)   //filter contact no
            {
                if (index == 1)
                {
                    $("table tr", $(this)).each(function ()
                    {
                        str += ($(this).text()) + "$";
                    });
                }
            });
        }
        else
        {
            $("div.m5.f11").each(function ()
            {
                str += $('input[name=mob]').val();
            });
        }
        str += "^";

        var PageURL = window.location.pathname;
        str += PageURL.substring(PageURL.lastIndexOf('-') + 1).split('&')[0];
        str += "^";
        str += sellertype;
        str += "^";
        str += $("td.lp10.f12").text();

    });


    return str;
}

function ProcessProjectDetailsMB()      // filter Project Deatail By MAgicbricks
{
    var Data = "";
    var ProjectName = "", Builder = "", Price = "", Link = "", Address = "", PossessionDate = "";
    $("div.project").each(function ()
    {
        Link = $("div.projectImage a img").attr('src');
        var FullName = $("div.projectName", this).text();
        ProjectName = FullName.split("by")[0];
        Builder = FullName.split("by")[1];
        PriceRange = $("div.priceRange", this).text();
        Address = $("div.address", this).text().split("in")[1];
        PossessionDate = Address.split(":")[1];
        Data += Link + "^" + ProjectName + "^" + Builder + "^" + PriceRange + "^" + Address + "^" + PossessionDate;
        Data += "~";
    });
    if (Data != "")
    {
        //alert(Data);
        $.post("http://localhost:4566/PropertyMap/Data.aspx?Action=UpdateCurrentProject", { PropertyData: Data }, function (data)
        {
            //setTimeout("window.close()", 100);
        });
    }
    //console.log(str);
}

function FilterSchools()     // Filter Landmarks from JustDial
{
    var Data = "";
    //alert();
    var ctr = 0;
    $("div.innerBG").each(function ()
    {

        var SchoolName = $(".comTitle_white span.Ctitle a", $(this)).text();
        if (SchoolName == "")
            SchoolName = $(".comTitle span.Ctitle a", $(this)).text();
        var Address = $(".logoDesc", $(this)).text().split("|")[0];
        var Lat = $("#lat" + ctr).val();
        var Lng = $("#lng" + ctr).val();
        var ContNo = $(".logoDesc p", $(this)).eq(0).text().split(':')[1].split('|')[0];
        //var Addreass = $("#dispads" + ctr).val();
        console.log(Lat + "^" + Lng + "^" + Address + "^" + SchoolName + "^" + ContNo);
        Data += Lat + "^" + Lng + "^" + Address + "^" + SchoolName + "^" + ContNo;
        ctr++;
        Data += "~";
    });
    if (Data != "")
    {
        //alert(Data);
        $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateAminityData", { AminityData: Data }, function (data)
        {
            //alert("Saved");
        });

    }
}

function FilterServicesFromJD()
{
    var Data = "";

    $("section.jgbg").each(function ()
    {
        var Address = "";
        var SocName = $("span.jcn a", $(this)).text().trim();
        Address = $("section.jcar section.jbc section p:eq(0)", $(this)).text().split('|')[0].trim();
        var Contact = $("section.jcar section.jbc section p:eq(1)", $(this)).text().split(':')[1].split('|')[0].trim();
        var City = Address.replace(/.*,\s*/, '').split('-')[0];
        //alert(City);

        //if ($(".jrcl3", $(this)).text() == "")
        //    Address = $(".jrcl2", $(this)).text();
        Data += SocName + "^" + Address + "^" + Contact + "^" + City;
        Data += "~";

        //Data+="|" + City+"|";

    });
    //console.log(Data);
    if (Data != "")
    {
        $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateServicesData", { ServicesData: Data }, function (data)
        {
            //alert(data);
        });
    }
}

function FilterDealersJd()
{
    var Data = "";

    $("section.jgbg").each(function ()
    {
        var Address = "";
        var SocName = $("span.jcn a", $(this)).text().trim();
        Address = $("section.jcar section.jbc section p:eq(0)", $(this)).text().split('|')[0].trim();
        var Contact = $("section.jcar section.jbc section p:eq(1)", $(this)).text().split(':')[1].split('|')[0].trim();
        var City = Address.replace(/.*,\s*/, '').split('-')[0];
       
        Data += SocName + "^" + Address + "^" + Contact + "^" + City;
        Data += "~";
        
    });

    console.log(Data);
    if (Data != "")
    {
        $.post(basePath+"Data.aspx?Action=UpdateAgentData", { ServicesData: Data }, function (data)
        {
        });
    }
}


var link = [];

function FilterProjectsFromMB()
{
    $("div.projectDetailLeft").each(function (index)
    {
        var text = [];
        var Posession = "";
        var BHKInfo = "";

        $("div", $(this)).each(function ()
        {
            if ($(this).text().indexOf("by:") > -1)
            {
                var temp = [];

                text = $(this).text().split(':');

                alert(text[0] + "-" + text[1]);

                if (text[0] == "Possession by")
                {
                    alert('dj');
                    Posession = text[1];
                }
                //alert(Posession);
            }
        });

        if ($("div.projectDetailHead", $(this)).children("b").children("a").prop("href") == undefined)
        {
            $("table tr", $(this)).each(function (index)
            {
                if ($(this).find('th:first').text() == "Property Type")
                {
                    BHKInfo = GetCleanTable($(this).parent());
                }
            });
        }

        //link.push($("div.projectDetailHead", $(this)).children("b").children("a").prop("href"));
        var hyperlink = $("div.projectDetailHead", $(this)).children("b").children("a").prop("href");
        if (!hyperlink)
        {
            hyperlink = $("div.projectDetailHead", $(this)).children("b").text();
        }

        var ProjName = $("div.projectDetailHead", $(this)).children("b").text().split("by")[0];
        var BuilderName = $("div.projectDetailHead", $(this)).children("b").text().split("by")[1].trim();
        var City = $('#project_bar_city').find(":selected").text();
        var Location = $("div.projectDetHead", $(this)).text().split("in")[1].trim();
        var PriceRange = $("div.projectsRates", $(this)).text().trim();

        var Data = hyperlink + "^" + ProjName + "^" + BuilderName + "^" + City + "^" + PriceRange + "^" + Location;
        //alert(Data);

        $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateProjectData", { ProjectData: Data }, function (data)
        {
            console.log("Project-" + data)
            var f = data.split('~');
            if (f.length > 1)
            {
                var ProjID = f[1];
                FillProjectDetail(hyperlink, ProjID, BHKInfo);
            }
        })

    });
    console.log("done");
}

function FillProjectDetail(hyperlink, ProjID, bhkInfo)
{
    console.log(hyperlink.trim(), ProjID);

    if (bhkInfo)
    {
        $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateProjectDetail", { ProjectDetailData: bhkInfo, ProjID: ProjID }, function (data)
        {
            //console.log(data);
            console.log(ProjID + '-a');
        });

        return;
    }
    else
    {
        $.ajax({
            url: hyperlink, async: false, cache: false, success: function (data)
            {
                var str = "";
                var Data = "";
                var loc = "";
                var poss = "";
                var BSP = "";
                var BHKInfo = "";

                $("li", $(data)).each(function ()
                {
                    if ($(this).text().indexOf(":") > -1)
                    {
                        var text = $(this).text().split(':')[0];
                        //alert(text);
                        switch (text)
                        {
                            case "BSP":
                                BSP = $(this).text().split(':')[1]
                                break;
                            case "Location":
                                loc = $(this).text().split(':')[1];
                                break;
                            case "Possession":
                                {
                                    poss = $(this).text().split(':')[1];
                                }
                        }
                    }
                });

                if (bhkInfo != " ")
                {
                    $("table tr", $(data)).each(function (index)
                    {
                        if ($(this).find('th:first').text() == "Type")
                        {

                            BHKInfo = GetCleanTable($(this).parent());
                        }
                    });
                }
                else
                {
                    BHKInfo = bhkInfo;
                }

                //console.log("BSP:", BSP);
                //console.log("Location:", loc);
                //console.log("Possetion:", poss);
                //console.log("BHKInfo:", BHKInfo);
                //Data = BSP + "#" + loc + "#" + poss + "@" + BHKInfo;

                Data = BHKInfo;

                //console.log(Data);
                $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateProjectDetail", { ProjectDetailData: Data, ProjID: ProjID }, function (data)
                {
                    //console.log(data);
                    console.log(ProjID + '-b');
                });
            }
            //});
        });
    }
}

var a;
function FilterData(divData)
{

}

function FillForm(City, SubCity)
{
    $("#bar_city").val(City);
    $("#prop_localityName").val(SubCity);
    $("#inputListings1").prop('checked', false);
    $("#inputListings2").prop('checked', true);
    $("#inputListings3").prop('checked', false);
    $("input.searchPropertyBtn").prop('checked', false);
}

function Process(data)
{
    var str = "";
    $("table").each(function (index)
    {
        $("input:radio", this).each(function ()
        {
            str += $(this).val() + "~";
        });
    });

    return str;
}

function htmlEncode(value)
{
    return $('<div/>').text(value).html();
}

function htmlDecode(value)
{
    return $('<div/>').html(value).text();
}

function GetCleanTable(obj, replacetxt, replacewith)
{
    console.log(obj);
    var str = "";
    $("tr", $(obj)).each(function (index)
    {
        $("th", $(this)).each(function (index)
        {
            str += $(this).text() + "^";
        });

        $("td", $(this)).each(function (index)
        {
            str += $(this).text() + "^";
        });

        str += "~";
    });
    //alert(str);
    return str;
}

function FilterProjectsURLAllCheckDeals()
{
    var URls = "";
    $("div.propResultProjDetail p.L3 a").each(function (index)
    {
        URls += $(this).attr("href") + "~";
    });

    $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateProjectURL", { Portal: "AllCheckDeals", URL: URls }, function (data)
    {
        console.log("Project-" + data)
    })
    console.log(URls);
}

function DownloadProjectsURLAllCheckDeals()
{
    $.get('http://localhost:4566/PropertyMap/Data/PortalURL/AllCheckDeals.txt', function (data)
    {
        var UrlS = data.split('\n');
        //console.log(UrlS);

        $(UrlS).each(function (index)
        {
            //if (index < 130)
            //return;
            //alert(PROJ_LATITUDE);
            //if ($(UrlS[index]).text().search('http') > 0)
            //alert(UrlS[index]);

            $.ajax({
                url: this, async: false, cache: false, success: function (data)
                {
                    var Name = "";
                    var PriceRange = "";
                    var Location = "";
                    var Sizes = "";
                    var Plans = "";
                    var Possession = "";
                    var BHKInfo = "";
                    var SourceURL = "";

                    SourceURL = this.url;
                    console.log(SourceURL);

                    Name = $("div.projectName div.h1heading", $(data)).text();
                    PriceRange = $("div.projectName div.price", $(data)).text();

                    $("div.projectDiscrip p", $(data)).each(function (index)
                    {
                        if ($(this).text().indexOf("Location:") > -1)
                        {
                            Location = $(this).text().split(':')[1];
                            //alert(Location);
                        }
                        else if ($(this).text().indexOf("Sizes:") > -1)
                        {
                            Sizes = $(this).text().split(':')[1];
                            //alert(Sizes);
                        }
                        else if ($(this).text().indexOf("Plans:") > -1)
                        {
                            Plans = $(this).text().split(':')[1];
                            //alert(Plans);
                        }
                        else if ($(this).text().indexOf("Possession By:") > -1)
                        {
                            Possession = $(this).text().split(':')[1];
                            //alert(Possession);
                        }
                    });

                    $("table.propTypesListGrid", $(data)).each(function (index)
                    {
                        BHKInfo = GetCleanTable($(this));
                        //console.log(Name+"!!"+BHKInfo);
                    });

                    var amenities = "";
                    $("div.amenitiesLeft div", $(data)).each(function (index)
                    {
                        var obj = $(this).text();
                        $("span", $(this)).each(function ()
                        {
                            if ($(this).attr('class') == undefined)
                            {
                                amenities += obj + "$";
                            }
                        });
                    });

                    $("div.amenitiesRight div", $(data)).each(function (index)
                    {
                        var obj = $(this).text();
                        $("span", $(this)).each(function ()
                        {
                            if ($(this).attr('class') == undefined)
                            {
                                amenities += obj + "$";
                            }
                        });
                    });
                    console.log(amenities);

                    data = data.toString();
                    //console.log(data);
                    var Startindex = data.indexOf("<script type='text/javascript'>");
                    var Endindex = data.indexOf("Popgal Js");

                    var ProjectData = data.toString().substring(Startindex, Endindex);


                    var Lat = ProjectData.substring(ProjectData.indexOf("PROJ_LATITUDE") + 13, ProjectData.indexOf("PROJ_LONGITUDE"));
                    var Lng = ProjectData.substring(ProjectData.indexOf("PROJ_LONGITUDE") + 14, ProjectData.indexOf("</script>"));

                    Lat = Lat.replace('=', '').replace('\'', '').replace('\'', '').replace(';', '');
                    Lng = Lng.replace('=', '').replace('\'', '').replace('\'', '').replace(';', '');
                    console.log(Lat);
                    console.log(Lng);

                    var ProjecrDetail = $("p#aboutContent", $(data)).text();
                    var BuilderName = ($("h3.seoHot", $(data)).text()).replace("About", "").trim();
                    //alert(BuilderName);
                    var BuilderDetail = $("p#aboutContentB", $(data)).text();

                    var ProjStr = Name + "^" + PriceRange + "^" + Location + "^" + Sizes + "^" + Plans + "^" + Possession + "^" + Lat + "^" + Lng + "^" + amenities + "^" + ProjecrDetail + "^" + BuilderName + "^" + BuilderDetail;

                    $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateProjectAllChk", { ProjectData: ProjStr, BhkDetail: BHKInfo, DataSourceURL: SourceURL }, function (data)
                    {
                        console.log("Project-" + data);
                    })
                }
            });
        });
    });
}

function FilterBuilderUrlCF()                                                                       //filter builders link and save to text file
{
    var URls = "";
    $("div.pad10 table tr td").each(
        function ()
        {
            if ($(this).text() != "")
                URls += $("a", $(this)).prop("href") + "~";
        })
    //alert(URls);
    $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateBuilderURL", { Portal: "CommonFloor", URL: URls }, function (data)
    {
        alert("done!");
        //console.log("Project-" + data)
    })
}

function DownloadBuilderCF()                                                                        //Download builder Information from Commonfloor
{
    var LastIndex = parseInt(LoadFromLocalStorage("LastIndex", "0"));
    //alert(LastIndex);

    $.get('http://localhost:4566/PropertyMap/Data.aspx?Action=GetNextRecord&Data1=CommonFloor.txt&Data2=' + LastIndex, function (data)
    {
        console.log(LastIndex + "-" +  data);
        if (data == "")
            return;

        $.ajax({
            url: data, cache: false, success: function (data)
            {
                var Name = $("h1", data).text();
                var imageURL = $("div#builder_info_panel img", data).prop("src");
                var Descrip = $("div#container>div>div:eq(1)>div:eq(0)", data).text();
                var Address = $("div.text11px_grey>div:first", data).text().replace("Address:", "").trim();
                var Contact = $("div.text11px_grey>div:eq(1)", data).text().replace("Contact:", "").trim();
                var Source = (this.url).split('?')[0];

                //if (Descrip.indexOf("<br>") > 0);
                //{
                //    console.log(Descrip);
                //    Descrip.replace("<br>", "");
                //    console.log("Sol-");
                //    console.log(Descrip);
                //}
                //if (Descrip.indexOf(';') > 0)
                //{
                //    Descrip.replace(";", "");
                //}

                Builderstr = Name + "^" + Address + "^" + Contact + "^" + Descrip + "^" + imageURL + "^" + Source;

                var ctr = 1;
                $.post("http://localhost:4566/Propertymap/Data.aspx?Action=UpdateBuilderCF", { BuilderData: htmlEncode(Builderstr)}, function (data)
                {
                    console.log(data);
                    setTimeout("DownloadBuilderCF()", 10);
                })
            },
            error: function (e)
            {
                //console.log(e.status);
                if(e.status==404)
                setTimeout("DownloadBuilderCF()", 10);
            }
        })
    })

    SaveInLocalStorage("LastIndex", LastIndex + 1);
}

function DownloadPriceTrendMB()                                     //Save pricetrend links in local  storage
{
    try
    {
        if (LoadFromLocalStorage("URLS") == undefined)
        {
            SaveInLocalStorage("URLS", []);
        }

        var URLS = LoadFromLocalStorage("URLS").split(',');

        //console.log(URLS);

        $("div#viewTrendsTable table").each(function (index)
        {
            $("tr", $(this)).each(function (index)
            {
                if (index > 1)
                {
                    $("td", $(this)).each(function ()
                    {
                        var l = ($("a", $(this)).prop("href")).trim();
                        for (var i = 0; i < URLS.length; i++)
                        {
                            if (URLS[i] == l)
                                return;
                        }

                        //if (($("a", $(this)).prop("href")).trim() == "http://www.magicbricks.com/Property-Rates-Trends/ALL-RESIDENTIAL-rates-in-Noida#")
                        //    return;
                        var s=($("a", $(this)).prop("href")).trim();
                        if(s.indexOf("ALL-RESIDENTIAL")>0)
                            return;
                        else
                        {
                            console.log(($("a", $(this)).prop("href")).trim());
                            URLS.push(($("a", $(this)).prop("href")).trim());
                        }
                    });
                }
            });
        });

        SaveInLocalStorage("URLS", URLS);
        SaveInLocalStorage("LastIndex", 0);
        //alert("links saved in Local Storage")
    }
    catch (err)
    {
        alert("?????????????Data not Saved!-" + err);
    }
}

function ProcessMagicBricksPriceTrends()                                        //process price trend data & post to data page 
{
    var heading = $("div#localityProfileMsg h2").text();

    var cityLoc="";
    if (heading.indexOf("in") > 0)
        cityLoc = heading.split("in")[1];
    else
    {
        cityLoc = heading.split("of")[1];
    }

    var city = cityLoc.split(',')[1].trim();
    var subcity = cityLoc.split(',')[0].trim();
    var City_Subcity = city + "+" + subcity;
    
    //function mytest()      //tampermonkey script to write data in div
    //{
    //    try
    //    {
    //        var obj = document.getElementById("locality-expert-reviewWrapper").innerHTML = unsafeWindow.lowerRange + "~" + unsafeWindow.upperRange + "~" + unsafeWindow.quartrValues;
    //    }
    //    catch (e) { }
    //    //setTimeout(mytest, 1000);
    //}
    //mytest();

    var data = $("#locality-expert-reviewWrapper").text();  //rea data of div written by tampermonkey
    //alert(data);
    var temp = data.split('~');

    var lowerRange = temp[0];
    var upperRange = temp[1];
    var quartrValues = temp[2];

    $.post("http://localhost:38592/Data.aspx?Action=UpdatePriceTrend", { LowerRange: lowerRange, UpperRange: upperRange, QuarterValues: quartrValues, CitySubcity: City_Subcity }, function (data)
    {
        var URLS = LoadFromLocalStorage("URLS", "").split(',');
        var LastIndex = parseInt(LoadFromLocalStorage("LastIndex", "0"));

        if (LastIndex < URLS.length)
        {
            var NewURL = URLS[LastIndex];
            location.href = NewURL;
            SaveInLocalStorage("LastIndex", LastIndex + 1);
        }
    });
}

function FilterMBAgents()
{
    $("div.agentSearchMainContent").each(function (index)
    {
        
        var id = $("div.contactProperty", $(this)).attr("id").split('#')[1];
        console.log(id);

        if (index == 0)
        {
           console.log( $("#tt2" + id + " a", $(this)).text());
            $("#tt2" + id + " a", $(this)).trigger("click");
        }
        return;

        //$("#tt2" + id + " a", $(this)).each(function ()
        //{
        //    $(this).click();
        //});
    //alert(id);
        if (id == "undefined") return;
        var temp = "dd2" + id;
        //$("#" + temp + "").show();
        //contactNo#965975  
        //tabs_content_property

        alert($("#contactNo#" + id + " span").text());

        //alert($("#" + id + "div.tabs_content_property ").text());
        //alert($("#subDataDiv#" + id + "div.cdetailAgent sapn").text());
        //alert(temp);
        //$(+id+")
    });
}

//---------------Local Storage--------------------//
function SaveInLocalStorage(Key, val)
{
    if (typeof (localStorage) != 'undefined')
    {
        window.localStorage.removeItem(Key);
        window.localStorage.setItem(Key, val);
        return true;
    }
    else
    {
        alert("Your browser does not support local storage, please upgrade to latest browser");
        return false;
    }
}

function LoadFromLocalStorage(Key, DefaultValue)
{
    var valoutput;
    if (typeof (window.localStorage) != 'undefined')
    {
        valoutput = window.localStorage.getItem(Key);
    }

    else
    {
        throw "window.localStorage, not defined";
    }

    if (DefaultValue && !valoutput)
        return DefaultValue;
    else
        return valoutput;
}
//---------------Local Storage--------------------//