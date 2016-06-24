var SelectedProperty = [];

function LoadPropertyList()
{
    var Data = LoadFromLocalStorage("PL");
    if (Data == null)
        SelectedProperty.length = 0;
    else
        SelectedProperty = $.parseJSON(Data);
}

function AddToMyList(SocID, ProjName, AptID,BHKValue,SuperArea)  //Add List
{
  
    //console.log(SocID+"-"+ProjName+"-"+AptID+"-"+BHKValue+"-"+SuperArea);
    if (GetIndex(SocID, AptID)==-1)
    {
        //console.log("SelectedProperty.length : " + SelectedProperty.length);
        SelectedProperty[SelectedProperty.length] = { SocietyID: SocID, ApartmentID: AptID, ProjectName: ProjName, BHK: BHKValue,SupArea:SuperArea };
        SaveInLocalStorage("PL", JSON.stringify(SelectedProperty));
        //console.log(SelectedProperty);
    }
}

function DeleteList(SocietyID, AptID)
{
    alert(SocietyID+"-"+AptID);
    var index = GetIndex(SocietyID, AptID);
    //console.log(index);
    if (index != -1)
    {
        SelectedProperty.splice(index, 1);
        SaveInLocalStorage("PL", JSON.stringify(SelectedProperty));
    }
}

function GetIndex(SocietyID, AptID)
{
    var Found = -1;
    for(var i=0;i<SelectedProperty.length;i++)
    {
        if (SelectedProperty[i].SocietyID == SocietyID && SelectedProperty[i].ApartmentID == AptID)
        {
            Found = i;
            break;
        }
    }
    return Found;
}