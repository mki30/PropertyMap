function QueryString()
{
    // PROPERTIES
    this.arg = new Array;
    this.status = false;

    // METHODS
    this.clear = Clear;
    this.getQueryString = Get;
    this.getAll = GetAll;
    this.getStatus = GetStatus;
    this.read = Read;

    // FUNCTIONS

    // Clears the array, this.arg, of all query string data
    function Clear()
    {
        this.arg = new Array;
    }

    // Returns a named value from the query string
    function Get(sName)
    {
        return this.arg[sName];
    }

    // Return all data as an associative array
    function GetAll()
    {
        return this.arg;
    }

    function GetStatus()
    {
        return this.status;
    }

    // Reads the query string into an array named this.arg
    function Read(sUrl)
    {
        var aArgsTemp, aTemp, sQuery;
        // You can pass in a URL query string
        if (sUrl)
        {
            sQuery = sUrl.substr(sUrl.lastIndexOf("?") + 1, sUrl.length);
        }
        // Or read it from the browser location
        else
        {
            sQuery = window.location.search.substr(1, window.location.search.length);
        }
        // Check that query string exists and contains data
        // If not (length < 1) then return
        if (sQuery.length < 1) { return; }
        // Else set this.status to true and proceed
        else { this.status = true; }
        //
        aArgsTemp = sQuery.split("&");
        for (var i = 0; i < aArgsTemp.length; i++)
        {
            aTemp = aArgsTemp[i].split("=");
            this.arg[aTemp[0]] = aTemp[1];
        }
    }
}

