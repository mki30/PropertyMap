var units = new Array("Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen");
var tens = new Array("Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety");

function num(it)
{
    var theword = "";
    var started;
    if (it > 999) return "Lots";
    if (it == 0) return units[0];
    for (var i = 9; i >= 1; i--)
    {
        if (it >= i * 100)
        {
            theword += units[i];
            started = 1;
            theword += " hundred";
            if (it != i * 100) theword += " and ";
            it -= i * 100;
            i = 0;
        }
    };

    for (var i = 9; i >= 2; i--)
    {
        if (it >= i * 10)
        {
            theword += (started ? tens[i - 2].toLowerCase() : tens[i - 2]);
            started = 1;
            if (it != i * 10) theword += "-";
            it -= i * 10;
            i = 0
        }
    };

    for (var i = 1; i < 20; i++)
    {
        if (it == i)
        {
            theword += (started ? units[i].toLowerCase() : units[i]);
        }
    };
    return theword;
}