$(document).ready(function ()
{
    CalculatePrice();

    $("input").bind("keyup", function ()
    {
        //if (("#IntrestRate").val > 20)
        //{
        //    alert("qfsf");
        //}
        CalculatePrice();
    });

    //$("#tableFloorPlans input").bind("focus", function () {
    //    $(this).select();
    //});

    //$("input").bind("click", function () {
    //    $(this).select();
    //});

    //$('#ddlAmtperc').change(function ()
    //{
    //    CalculatePrice();
    //});
});

function fmt(val)
{
    return Globalize.format(val, "n", "en-IN")
}

var LastTotal = 0;
function CalculatePrice()
{
    var Area = GetNumber("#txtArea");
    var BSP = GetNumber("#BSPRate");
    var BSPPrice = BSP * Area;
    var RegistryRate = GetDecimal("#txtRegistryRate");
    var RegistryValue = parseInt(BSPPrice * RegistryRate / 100);
    var PLCRate = GetDecimal("#PLCRate");
    var PLCValue = parseInt(Area * PLCRate);
    var PowerBackDeposit = GetNumber("#PowerBack");
    var ParkingDeposit = GetNumber("#Parking");

    $("#txtTotalBSP").val(BSPPrice);
    $("#txtTotalPLC").val(PLCValue);

    var Total = BSPPrice
    + GetNumber("#MaintenanceDeposit")
    + PowerBackDeposit
    + GetNumber("#Parking")
    + GetNumber("#ClubFee")
    + PLCValue;

    if (BSP == 0)
    {
        Total = $("#txtAmount").val().replace(',', "");
    }
    else
    {
        $("#txtAmount").val(Total);
    }

    $("#txtTotal").val(fmt(Total));

    //if (LastTotal == Total)
    //    return;

    LastTotal = Total
 
    var P = (Total);
    var r = GetDecimal("#IntrestRate") / 12 / 100;
    var n = GetDecimal("#Year") * 12;

    //console.log(P + "-" + r + "-" + n);
    var EMI = parseInt((P * r) * (Math.pow((1 + r), n) / (Math.pow((1 + r), n) - 1)));

    $(".EMI").html(fmt(EMI));
    $("#txtEMI").val(fmt(EMI));

    var TotalIntrest = (EMI * n - P);
    $(".totalintrest").html(fmt(TotalIntrest));
    var TotalPayMent = parseInt(P) + parseInt(TotalIntrest);

    $(".totalPayment").html(fmt(TotalPayMent));

    var pincPerc = parseInt((P / TotalPayMent) * 100);
    var interPerc = parseInt((TotalIntrest / TotalPayMent) * 100);
    DrawPI(pincPerc, interPerc);                                             //Call PI Chart Draw

    year = [];
    psYear = [];
    psPrincipal = [];
    psInterest = [];
    psBalance = [];
    var a = [],
        c = [],
        b = [];
    for (i = 0; i < n / 12; i++) psYear[i] = i + 1;
    c[0] = P * r;
    a[0] = EMI - c[0];
    b[0] = P - a[0];
    for (i = 1; i < n; i++) c[i] = b[i - 1] * r, a[i] = EMI - c[i], b[i] = b[i - 1] - a[i];
    for (i = b[n - 1] = 0; i < n / 12; i++)
    {
        a = n / 12 * 12 - i * 12 >= 12 ? 12 : n / 12 * 12 - i * 12;
        for (j = psInterest[i] = 0; j < a; j++) psInterest[i] += c[i * 12 + j];
        psInterest[i] = Math.round(psInterest[i]);
        psPrincipal[i] = Math.round(EMI * a - psInterest[i]);
        psBalance[i] = Math.round(b[(i + 1) * 12 - (12 - a) - 1]);
        year[i] = "year" + (i + 1);
    }

    DrawBarGraph(psInterest, psPrincipal);                          //Draw Bar Graph
    DisplayPaymentScheduleTable(TotalIntrest);                      //Display amortization
}

function DisplayPaymentScheduleTable(TotalIntrest)
{

    var a = "<table class='amortization table table-hover table-striped table-condensed'><tr><th>Year</th><th>Principal Amount</th><th>Interest Amount</th><th>Balance Amount</th></tr>";
    for (i = 0; i < psYear.length; i++)
    {
        a += "<tr><td>" + psYear[i]
            + '</td><td class="currency">Rs. ' + fmt(psPrincipal[i])
            + '</td><td class="currency">Rs. ' + fmt(psInterest[i])
            + '</td><td class="currency">Rs. ' + fmt(psBalance[i]) + "</td></tr>";
    }
    a += "<tr><td><td><td><b>Total Interest Rs. " + fmt(TotalIntrest) + "</b><td>";
    a += "</table>";
    $("#amortization").html(a)
}

function DrawBarGraph(psInterest, psPrincipal)
{
    var chart1; // globally available
    $(document).ready(function ()
    {
        chart1 = new Highcharts.Chart({
            chart: {
                renderTo: 'container',
                type: 'column'
            },
            title: {
                text: 'EMI Chart'
            },
            xAxis: {
                title: {
                    text: ''
                },
                categories: psYear,
                labels: {
                    rotation: -60,
                    align: "right",
                    style: {
                        font: "normal 9px Verdana, sans-serif"
                    },
                    formatter: function ()
                    {
                        return "Year " + this.value
                    }
                }
            },
            yAxis: {
                title: {
                    text: 'Intrest+Principal'
                }, labels: {
                    formatter: function ()
                    {
                        return "Rs. " + Globalize.format(this.value, "n", "en-IN")
                    }
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: false,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            legend: {
                align: "right",
                x: -10,
                verticalAlign: "top",
                y: 20,
                floating: true,
                backgroundColor: Highcharts.theme && Highcharts.theme.legendBackgroundColorSolid || "white",
                shadow: false
            },
            series: [{
                name: 'Principal',
                data: psPrincipal
            }, {
                name: 'Interest',                data: psInterest
            }]
        });
    });
}

function DrawPI(pincPerc, interPerc)
{
    //return;
    var chart;
    $(document).ready(function ()
    {
        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'emipiechart',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            title: {
                text: ''
            },

            tooltip: {
                formatter: function ()
                {
                    return "<b>" + this.point.name + "</b>: " + this.y + " %"
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    showInLegend: true,
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        formatter: function ()
                        {
                            return '<b>' + this.point.name + '</b>: ' + Math.round(this.percentage) + ' %';
                        }
                    }
                }
            },
            legend: {
                align: "right",
                x: -10,
                verticalAlign: "top",
                y: 20,
                floating: true,
                backgroundColor: Highcharts.theme && Highcharts.theme.legendBackgroundColorSolid || "white",
                shadow: false
            },
            series: [{
                type: 'pie',
                name: 'Price Share',
                data: [
                     {
                         
                         name: 'Principal',
                         y: pincPerc,
                         sliced: true,
                         selected: true
                     },
                      ['Interest', interPerc]
                ]
            }]
        });
    });
}

function GetNumber(id)
{
    var val = parseInt($(id).val(), 10);
    return isNaN(val) ? 0 : val;
}

function GetDecimal(id)
{
    var val = parseFloat($(id).val(), 10);
    return isNaN(val) ? 0 : val;
}
