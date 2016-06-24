/// <reference path="jquery-1.8.0.min.js" />
//------------High Chart Draw------------------
var chart;
var options;

$(document).ready(function ()
{
    options = {
        chart: { renderTo: 'container', type: 'line', marginRight: null, marginBottom: null },
        title: { text: 'Property Price Trend', x: -20 },//center
        subtitle: { text: 'Source: propertymap.info', x: -20 },
        xAxis: { categories: [], title: { text: 'Quarters' } },
        yAxis: { title: { text: 'Price/Sq Ft (Rs.)' } },
        
        tooltip: {
            formatter: function ()
            {
                return '<b>' + this.series.name + '</b><br/>' +
                this.x + ': ' + this.y + 'Rs.';
            },
        },
        plotOptions: {
            line: {
                enableMouseTracking: true
            }
        },
        legend: { layout: 'horizontal', align: 'center', verticalAlign: 'top', x: -10, y: 100, borderWidth: 1 },
        series: []
    };

    var ctr=1;
    $("#lblCheckBox input[type=checkbox]").click(function ()
    {
        if (ctr <= 4)
        {
            if ($(this).prop("checked"))
            {
                ctr++;
                ShowTrend(this);
            }
            else
            {
                ctr--;
                var SeriesID = $(this).attr("data-index");
                $(TrendData).each(function ()
                {
                    if (this.SeriesID == SeriesID)
                        this.Visible = false;
                });
                DisplayTrend();
            }
        }
        //else
        //{
        //    $(this).prop('checked', false);
        //    alert("maximum 5 selections are allowed");
        //}
    });

    //Load all the selected cities price trends
    $("#lblCheckBox input[type=checkbox]").each(function ()
    {
        if ($(this).prop("checked"))
            ShowTrend(this);
    });

    LoadPriceTrend(SubCityUrlName, SubCity, 0);
});

var TrendData = new Array();

function ShowTrend(obj)
{
    LoadPriceTrend($(obj).attr("data-urlname"),$(obj).attr("data-name"), $(obj).attr("data-index"));
}

function LoadPriceTrend(SubCityURLName, SubCityName, SeriesID)
{
    $("[data-name='" + SubCityName + "']").prop('checked', true);

    var Found = false;

    // check if the price trends has been already downloaded
    $(TrendData).each(function (Index)
    {
        if (this.SeriesID == SeriesID)
        {
            Found = true;
            this.Visible = true;
        }
    });

    if (!Found)
    {
        $.post(BasePath + "Data.aspx?Action=GetPriceTrend&Data1=" + SubCityURLName, function (Data)
        {
            var PriceTrend = JSON.parse(Data);
            //console.log(PriceTrend);
            var MaxRate = new Array();
            var MinRate = new Array();
            var AvgRate = new Array();
            var Categories = new Array();

            $(PriceTrend).each(function (index)
            {
                MaxRate.push(this.Max);
                MinRate.push(this.Min);
                AvgRate.push((this.Max + this.Min) / 2);
                Categories.push(this.Quarter);
            });

            TrendData.push({ SeriesID: SeriesID, Data: AvgRate, Visible: true, SubCity: SubCityName, Categ: Categories });
            //console.log(TrendData);
            DisplayTrend();
        });
    }
    else
        DisplayTrend();
}

function DisplayTrend()
{
    options.series = [];

    var x = [];
    $(TrendData).each(function (Index)
    {
        if (this.Visible)
        {
            if (this.Categ.length > x.length)
                x = this.Categ;
            options.series.push({ name: this.SubCity, data: this.Data });
        }
    });

    options.xAxis.categories = x;
    chart = new Highcharts.Chart(options);
}
