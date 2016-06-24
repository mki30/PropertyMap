using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

public class DBCheck
{
    //CheckDatabase
    //CheckTable

    public static void RunSQLFile(DatabaseCE db, string data)
    {
        string[] Commands = data.Split(new string[] { "GO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        IDbCommand cmd = new SqlCeCommand();
        cmd.Connection = db.myconnection;
        foreach (string s in Commands)
        {
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
    }

    public static void CheckTable(DatabaseCE db, string TableName, Dictionary<string, string> Fields, string[] PrimaryKeys)
    {
        //fields to be added to all table
       
        Fields.Add("LUDate", "[datetime]");
        Fields.Add("LUBy", "[nvarchar](50)");

        //create table
        string SQL = "CREATE TABLE [" + TableName + "] (";
        string PK = " PRIMARY KEY (";
        foreach (string s in PrimaryKeys)
        {
            SQL += " [" + s + "] " + Fields[s] + ",";
            PK += " [" + s + "] " + ",";
        }

        PK = PK.Substring(0, PK.Length - 1) + ") ";
        SQL = SQL + PK + ") ";

        string err = db.RunQuery(SQL);

        //check for fields
        foreach (string s in Fields.Keys)
        {
            string Err = db.RunQuery("ALTER TABLE [" + TableName + "] ADD [" + s + "] " + Fields[s]);
        }
    }

    public static Boolean UpdateBlogDBStructure(DatabaseCE db, int Counter)                //for Blog.sdf Structure Update
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();
        switch (Counter)
        {
            case 1:
                fields.Add("ID", "[int]");
                fields.Add("Title", "[nvarchar](100)");
                fields.Add("SortData", "[nvarchar](1000)");
                fields.Add("Data", "[image]");
                fields.Add("DataSize", "[int]");
                fields.Add("PostDate", "[datetime]");
                fields.Add("CityID", "[int]");
                fields.Add("SubcityID", "[int]");
                fields.Add("UrlName", "[nvarchar](150)");
                fields.Add("ReferenceUrl", "[nvarchar](150)");
                fields.Add("VisitCount", "[int]");
                fields.Add("Offline", "[int]");
                fields.Add("IsDelete", "[int]");
                CheckTable(db, "Post", fields, new string[] { "ID" });
                break;
            //case 2:
            //    fields.Add("PostID", "[int]");
            //    fields.Add("CompanyID", "[int]");
            //    CheckTable(db, "PostCompanyLinks", fields, new string[] { "PostID", "CompanyID" });
            //    break;
            default:
                return false;
        }
        return true;
    }

    public static Boolean UpdateDBStructure(DatabaseCE db, int Counter)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        switch (Counter)
        {
            case 1://Society
                fields.Add("ID", "[int]");
                fields.Add("SocietyName", "[nvarchar](100)");
                fields.Add("URLName", "[nvarchar](255)");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                fields.Add("Address", "[nvarchar](255)");
                fields.Add("City", "[nvarchar](100)");
                fields.Add("CityID", "[int]");
                fields.Add("SubCity", "[nvarchar](200)");
                fields.Add("SubCityID", "[int]");
                fields.Add("Area", "[nvarchar](100)");
                fields.Add("AreaID", "[int]");
                fields.Add("State", "[nvarchar](100)");
                fields.Add("Country", "[nvarchar](100)");
                fields.Add("Pin", "[nvarchar](10)");
                fields.Add("PowerBackup", "[int]");
                fields.Add("BuiltYear", "[int]");
                fields.Add("DistanceAirport", "[int]");
                fields.Add("DistaneRailStation", "[int]");
                fields.Add("DistanceBusStation", "[int]");
                fields.Add("DistanceHospital", "[int]");
                fields.Add("DistanceSchool", "[int]");
                fields.Add("DistanceShopping", "[int]");
                fields.Add("Total1BHK", "[int]");
                fields.Add("Total2BHK", "[int]");
                fields.Add("Total3BHK", "[int]");
                fields.Add("Total4BHK", "[int]");
                fields.Add("Total5BHK", "[int]");
                fields.Add("ForSale1BHK", "[int]");
                fields.Add("ForSale2BHK", "[int]");
                fields.Add("ForSale3BHK", "[int]");
                fields.Add("ForSale4BHK", "[int]");
                fields.Add("ForSale5BHK", "[int]");
                fields.Add("ForRent1BHK", "[int]");
                fields.Add("ForRent2BHK", "[int]");
                fields.Add("ForRent3BHK", "[int]");
                fields.Add("ForRent4BHK", "[int]");
                fields.Add("ForRent5BHK", "[int]");
                fields.Add("ForSaleAgent", "[int]");
                fields.Add("ForSaleOwner", "[int]");
                fields.Add("ForRentAgent", "[int]");
                fields.Add("ForRentOwner", "[int]");
                fields.Add("Amenities", "[nvarchar](100)");
                fields.Add("Description", "[nvarchar](4000)");
                fields.Add("StartDate", "[datetime]");
                fields.Add("EndDate", "[datetime]");
                fields.Add("VedioLink", "[nvarchar](1000)");
                fields.Add("BuilderID", "[int]");
                fields.Add("PolyPoints", "[nvarchar](3000)");
                fields.Add("Deleted", "[int]");
                fields.Add("Verified", "[int]");
                fields.Add("UseType", "[int]");
                fields.Add("PropertyType", "[int]");
                fields.Add("URL", "[nvarchar](250)");
                fields.Add("BrochureURL", "[nvarchar](250)");
                fields.Add("PricelistURL", "[nvarchar](250)");
                fields.Add("DataSourceURL", "[nvarchar](300)");
                fields.Add("Town", "[nvarchar](100)");
                fields.Add("Status", "[int]"); //1-Announced,2-Launched,3-Ongoing,4-Completed
                fields.Add("Impression", "[int]");
                fields.Add("AvgPos", "[int]");
                CheckTable(db, "Society", fields, new string[] { "ID" });
                break;

            case 2://Apartment
                fields.Add("ID", "[int]");
                fields.Add("SocietyID", "[int]");
                fields.Add("ApartmentTypeID", "[int]");
                fields.Add("ApartmentNumber", "[int]");
                fields.Add("Block", "[nvarchar](100)");
                fields.Add("Floor", "[int]");
                fields.Add("Facing", "[nvarchar](100)");
                fields.Add("DirectionalFacing", "[nvarchar](100)");
                fields.Add("Lifts", "[int]");
                fields.Add("Parking1", "[int]");
                fields.Add("Parking2", "[int]");
                fields.Add("ServantRoom", "[int]");
                fields.Add("PowerBackup", "[int]");
                CheckTable(db, "Apartment", fields, new string[] { "ID" });
                break;

            case 3://Availability
                fields.Add("ID", "[int]");
                fields.Add("AvailabilityType", "[int]"); //Rent-0,Sale-1
                fields.Add("SocietyID", "[int]");
                fields.Add("SellerType", "[int]");
                fields.Add("SellerID", "[int]");
                fields.Add("SellerName", "[nvarchar](100)");
                fields.Add("SellerMobile1", "[nvarchar](100)");
                fields.Add("SellerMobile2", "[nvarchar](100)");
                fields.Add("SellerMobile3", "[nvarchar](100)");
                fields.Add("SellerPhone", "[nvarchar](100)");
                fields.Add("SellerEmail", "[nvarchar](100)");
                fields.Add("BHK", "[int]");
                fields.Add("Bathroom", "[int]");
                fields.Add("Balcony", "[int]");
                fields.Add("Area", "[int]");
                fields.Add("CarpetArea", "[int]");
                fields.Add("Amount", "[int]");
                fields.Add("DateAvailableFrom", "[datetime]");
                fields.Add("FloorNo", "[int]");
                fields.Add("TotalFloors", "[int]");
                fields.Add("DirectionalFacing", "[nvarchar](100)");
                fields.Add("Facing", "[nvarchar](100)");
                fields.Add("AgeOfConstruction", "[nvarchar](100)");
                fields.Add("TransactionType", "[nvarchar](100)");
                fields.Add("OwnershipType", "[nvarchar](100)");
                fields.Add("AvailableUnits", "[int]");
                fields.Add("AdditionalRoom", "[nvarchar](100)");
                fields.Add("PlotArea", "[int]");
                fields.Add("SocietyLocation", "[nvarchar](200)");
                fields.Add("PostedOnDate", "[datetime]");
                fields.Add("DataSource", "[nvarchar](100)");
                fields.Add("DataSourceID", "[nvarchar](100)");
                fields.Add("Furnishing", "[nvarchar](100)");
                fields.Add("SocietyName", "[nvarchar](500)");
                fields.Add("Company", "[nvarchar](500)");
                fields.Add("Address", "[nvarchar](500)");
                fields.Add("Owner", "[nvarchar](200)"); //ownwer name or other detail for the agents
                fields.Add("ApartmentTypeID", "[int]");
                fields.Add("Description", "[nvarchar](500)");
                fields.Add("URL", "[nvarchar](300)");
                fields.Add("RentFor", "[int]");
                fields.Add("Deleted", "[int]");
                CheckTable(db, "Availability", fields, new string[] { "ID" });
                break;

            case 4://Apartment Type
                fields.Add("ID", "[int]");
                fields.Add("TypeName", "[nvarchar](50)");
                fields.Add("SocietyID", "[int]");
                fields.Add("Bedroom", "[int]");
                fields.Add("Bathroom", "[int]");
                fields.Add("Balcony", "[int]");
                fields.Add("Area", "[int]");
                fields.Add("SuperArea", "[int]");
                fields.Add("PlotArea", "[int]");
                fields.Add("Unit", "[int]");
                fields.Add("Description", "[nvarchar](200)");
                fields.Add("OtherFeatures", "[nvarchar](50)");
                fields.Add("BSP", "[int]");
                fields.Add("BSP_Installments", "[int]");
                fields.Add("MaintenanceDep", "[int]");
                fields.Add("PowerBackupDep", "[int]");
                fields.Add("ParkingDep", "[int]");
                fields.Add("ClubDep", "[int]");
                fields.Add("PLC", "[float]");
                fields.Add("UseType", "[int]");
                fields.Add("PropertyType", "[int]");
                fields.Add("Notes", "[nvarchar](1000)");

                CheckTable(db, "ApartmentType", fields, new string[] { "ID" });
                break;

            case 5://Agent
                fields.Add("ID", "[int]");
                fields.Add("AgentName", "[nvarchar](50)");
                fields.Add("URLName", "[nvarchar](100)");
                fields.Add("AgentCompany", "[nvarchar](100)");
                fields.Add("PhoneNo1", "[nvarchar](150)");
                fields.Add("PhoneNo2", "[nvarchar](50)");
                fields.Add("Mobile1", "[nvarchar](50)");
                fields.Add("Mobile2", "[nvarchar](50)");
                fields.Add("Mobile3", "[nvarchar](50)");
                fields.Add("OperatingIn", "[nvarchar](100)");
                fields.Add("OperatingSince", "[int]");
                fields.Add("Address", "[nvarchar](300)");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                fields.Add("DealsIn", "[nvarchar](100)");
                fields.Add("PropertiesHandled", "[nvarchar](100)");
                fields.Add("EmailID", "[nvarchar](100)");
                fields.Add("URL", "[nvarchar](300)");
                fields.Add("UserType", "[int]");                        //0-Agent,1-Ownwer,2-Builder
                fields.Add("City", "[int]");
                fields.Add("UserID", "[nvarchar](100)");
                fields.Add("Password", "[nvarchar](100)");
                fields.Add("SourceURL", "[nvarchar](500)");
                fields.Add("Varified", "[int]");
                fields.Add("Deleted", "[int]");
                fields.Add("BuilderDescription", "[nvarchar](4000)");
                CheckTable(db, "Agent", fields, new string[] { "ID" });
                break;

            //case 6://Builder
            //    fields.Add("ID", "[int]");
            //    fields.Add("BuilderName", "[nvarchar](50)");
            //    fields.Add("ContactNo", "[nvarchar](50)");
            //    fields.Add("EmailID", "[nvarchar](200)");
            //    fields.Add("Address", "[nvarchar](500)");
            //    CheckTable(db, "Builder", fields, new string[] { "ID" });
            //    break;

            //case 7://Owner
            //    fields.Add("ID", "[int]");
            //    fields.Add("OwnerName", "[nvarchar](50)");
            //    fields.Add("ContactNo", "[nvarchar](50)");
            //    fields.Add("EmailID", "[nvarchar](200)");
            //    fields.Add("Address", "[nvarchar](500)");
            //    CheckTable(db, "Owner", fields, new string[] { "ID" });
            //    break;

            //case 8://Property
            //    fields.Add("ID", "[int]");
            //    fields.Add("PropertyName", "[nvarchar](100)");
            //    fields.Add("OwnerID", "[int]");
            //    fields.Add("PropertyFor", "[nvarchar](100)");
            //    fields.Add("UseFor", "[nvarchar](100)");
            //    fields.Add("Location", "[nvarchar](100)");
            //    fields.Add("City", "[nvarchar](100)");
            //    fields.Add("Type", "[nvarchar](100)");
            //    fields.Add("FloorNo", "[int]");
            //    fields.Add("Area", "[int]");
            //    fields.Add("Bedroomss", "[int]");
            //    fields.Add("Price", "[int]");
            //    fields.Add("Description", "[nvarchar](100)");
            //    fields.Add("PossessionStatus", "[int]");
            //    CheckTable(db, "Property", fields, new string[] { "ID" });
            //    break;

            //case 9://User
            //    fields.Add("ID", "[int]");
            //    fields.Add("UserType", "[int]");
            //    fields.Add("Name", "[nvarchar](100)");
            //    fields.Add("CompanyName", "[nvarchar](100)");
            //    fields.Add("PhoneNo1", "[nvarchar](50)");
            //    fields.Add("PhoneNo2", "[nvarchar](50)");
            //    fields.Add("Mobile1", "[nvarchar](50)");
            //    fields.Add("Mobile2", "[nvarchar](50)");
            //    fields.Add("Mobile3", "[nvarchar](50)");
            //    fields.Add("City", "[nvarchar](100)");
            //    fields.Add("Address", "[nvarchar](500)");
            //    fields.Add("Lat", "[float]");
            //    fields.Add("Lng", "[float]");
            //    fields.Add("OperatingIn", "[nvarchar](100)");
            //    fields.Add("OperatingSince", "[int]");
            //    fields.Add("DealsIn", "[nvarchar](500)");
            //    fields.Add("PropertiesHandled", "[nvarchar](100)");
            //    fields.Add("EmailID", "[nvarchar](100)");
            //    fields.Add("URL", "[nvarchar](300)");
            //    fields.Add("UserID", "[nvarchar](100)");
            //    fields.Add("Password", "[nvarchar](100)");
            //    CheckTable(db, "User", fields, new string[] { "ID" });
            //    break;

            case 6://Agent Client
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("PhoneNo", "[nvarchar](50)");
                fields.Add("MobileNo", "[nvarchar](50)");
                fields.Add("EmailID", "[nvarchar](100)");
                fields.Add("City", "[nvarchar](100)");
                fields.Add("Address", "[nvarchar](500)");
                fields.Add("AgentID", "[int]");
                CheckTable(db, "AgentClient", fields, new string[] { "ID" });
                break;

            case 7://AsignClient
                fields.Add("ID", "[int]");
                fields.Add("AgentID", "[int]");
                fields.Add("AvlID", "[int]");
                fields.Add("ClientID", "[int]");
                fields.Add("Description", "[nvarchar](500)");
                CheckTable(db, "AsignClient", fields, new string[] { "ID" });
                break;

            //case 12://Block
            //    fields.Add("ID", "[int]");
            //    fields.Add("BlockName", "[nvarchar](100)");
            //    CheckTable(db, "Block", fields, new string[] { "ID" });
            //    break;

            case 8://PriceList
                fields.Add("ID", "[int]");
                fields.Add("SocietyID", "[int]");
                fields.Add("ApartmentTypeID", "[int]");
                fields.Add("BSP", "[int]");
                fields.Add("BSP_Installments", "[int]");
                fields.Add("MaintenanceDep", "[int]");
                fields.Add("PowerBackupDep", "[int]");
                fields.Add("ParkingDep", "[int]");
                fields.Add("PLC", "[float]");
                fields.Add("ServiceTax_BSP", "[float]");
                fields.Add("ServiceTax_PLC", "[float]");
                fields.Add("ServiceTax_PowBak", "[float]");
                fields.Add("ServiceTax_Parking", "[float]");
                fields.Add("Notes", "[nvarchar](1000)");
                CheckTable(db, "PriceList", fields, new string[] { "ID" });
                break;

            case 9://LandMarks
                fields.Add("ID", "[int]");
                fields.Add("LandmarkType", "[nvarchar](100)");
                fields.Add("LandmarkName", "[nvarchar](100)");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                fields.Add("Address", "[nvarchar](500)");
                fields.Add("ContNo", "[nvarchar](100)");
                fields.Add("Website", "[nvarchar](100)");
                CheckTable(db, "Landmark", fields, new string[] { "ID" });
                break;
            case 10:   //PriceTrend
                fields.Add("ID", "[int]");
                fields.Add("City", "[nvarchar](100)");
                fields.Add("Subcity", "[nvarchar](100)");
                fields.Add("Type", "[nvarchar](50)");//apt-rent,apt-sale,com-rent,com-sale,plot-sale
                fields.Add("Min", "[int]");
                fields.Add("Max", "[int]");
                fields.Add("PriceDate", "[datetime]");
                fields.Add("Quarter", "[int]");
                fields.Add("Year", "[int]");
                fields.Add("CityID", "[int]");
                fields.Add("SubCityID", "[int]");
                CheckTable(db, "PriceTrend", fields, new string[] { "ID" });
                break;

            case 11:     //Distances
                fields.Add("ID", "[int]");
                fields.Add("SocietyID", "[int]");
                fields.Add("LandmarkID", "[int]");
                fields.Add("Type", "[int]"); //  0 -  Airports, 1- Railway Stations,..
                fields.Add("Code", "[nvarchar](10)"); //Code of Airport and Railway Station
                fields.Add("RoadDistance", "[float]");
                fields.Add("Time", "[int]");
                fields.Add("DrivingInstructions", "[image]");
                fields.Add("DataSize", "[int]");
                fields.Add("LatLng", "[image]");
                fields.Add("LatLngDataSize", "[int]");
                CheckTable(db, "Distance", fields, new string[] { "ID" });
                break;
            case 12:    // Services
                fields.Add("ID", "[int]");
                fields.Add("Type", "[int]"); // 0-packers and movers,1-
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("Contact", "[nvarchar](20)");
                fields.Add("Address", "[nvarchar](300)");
                fields.Add("City", "[nvarchar](50)");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                CheckTable(db, "Service", fields, new string[] { "ID" });
                break;
            case 13:    // City
                fields.Add("ID", "[int]");
                fields.Add("ParentID", "[int]");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("SortName", "[nvarchar](100)");
                fields.Add("CityGroup", "[int]");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                fields.Add("PolyPoints", "[nvarchar](4000)");
                fields.Add("ChildCount", "[int]");
                fields.Add("ChildSocietyCount", "[int]");
                fields.Add("Verified", "[int]");
                fields.Add("Polygon", "[image]");
                fields.Add("PolygonDataSize", "[int]");
                fields.Add("WikimapiaID", "[int]");
                fields.Add("OSMID", "[int]");
                fields.Add("UrlName", "[nvarchar](300)");
                CheckTable(db, "City", fields, new string[] { "ID" });
                break;
            //case 19:    //ProjData 
            //    fields.Add("ID", "[int]");
            //    fields.Add("URL", "[nvarchar](1000)");
            //    fields.Add("Name", "[nvarchar](100)");
            //    fields.Add("BSP", "[nvarchar](100)");
            //    fields.Add("Location", "[nvarchar](100)");
            //    fields.Add("City", "[nvarchar](100)");
            //    fields.Add("Builder", "[nvarchar](100)");
            //    fields.Add("Possession", "[nvarchar](100)");
            //    fields.Add("PriceRange", "[nvarchar](100)");
            //    CheckTable(db, "ProjData", fields, new string[] { "ID" });
            //    break;
            //case 20:                                         //ProjPriceDetail 
            //    fields.Add("ID", "[int]");
            //    fields.Add("ProjID", "[int]");
            //    fields.Add("BHK", "[nvarchar](100)");
            //    fields.Add("Area", "[nvarchar](100)");
            //    fields.Add("Price", "[nvarchar](100)");
            //    CheckTable(db, "ProjPriceDetail", fields, new string[] { "ID" });
            //    break;
            case 14:                                           //FloorInfo
                fields.Add("AptTypeID", "[int]");
                fields.Add("FloorName", "[nvarchar](25)");
                fields.Add("FloorNo", "[int]");
                fields.Add("BuiltupArea", "[int]");
                fields.Add("TotalArea", "[int]");
                fields.Add("Terrace", "[int]");
                fields.Add("Lawn", "[int]");
                CheckTable(db, "FloorInfo", fields, new string[] { "AptTypeID", "FloorNo" });
                break;
            case 15:                                                //ClientLinking
                fields.Add("AgentID", "[int]");
                fields.Add("ClientID", "[int]");
                CheckTable(db, "ClientLinking", fields, new string[] { "AgentID", "ClientID" });
                break;
            //case 23:                                                //AgentLinking
            //    fields.Add("AgentID1", "[int]");
            //    fields.Add("AgentID2", "[int]");
            //    CheckTable(db, "AgentLinking", fields, new string[] { "AgentID1", "AgentID2" });
            //    break;

            case 16:                                                //Project Questions
                fields.Add("ID", "[int]");
                fields.Add("ProjectID", "[int]");
                fields.Add("Subject", "[nvarchar](100)");
                fields.Add("Question", "[nvarchar](1000)");
                fields.Add("Answer", "[nvarchar](4000)");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("Email", "[nvarchar](200)");
                fields.Add("PageUrl", "[nvarchar](300)");
                fields.Add("Approved", "[int]");
                CheckTable(db, "ProjectQuestion", fields, new string[] { "ID" });
                break;

            case 17:                                                //Project Questions
                fields.Add("ID", "[int]");
                fields.Add("CityID", "[int]");
                fields.Add("SubcityID", "[int]");
                fields.Add("Header", "[nvarchar](300)");
                fields.Add("Details", "[image]");
                fields.Add("DetailsSize", "[int]");
                CheckTable(db, "NewsArticle", fields, new string[] { "ID" });
                break;

            case 18://ImagesDetails (a link table for Article,Projects,Products)
                fields.Add("ID", "[int]");
                fields.Add("ReferenceID", "[int]");//id for project, builder 
                fields.Add("ImageReferenceType", "[int]");//reference type eg,(project Image,builder image)
                fields.Add("ImageID", "[nvarchar](20)");//1_5.jpg
                fields.Add("Tags", "[nvarchar](100)");
                fields.Add("Sequence", "[int]");
                fields.Add("Width", "[float]");
                fields.Add("Height", "[float]");
                fields.Add("SizeKb", "[float]");
                fields.Add("Lat", "[float]");
                fields.Add("Lng", "[float]");
                fields.Add("Format", "[nvarchar](5)");
                fields.Add("Data", "[image]");
                fields.Add("DataSize", "[int]");
                fields.Add("UrlName", "[nvarchar](300)");
                fields.Add("Tagline", "[nvarchar](300)");
                fields.Add("Courtesy", "[nvarchar](100)");
                fields.Add("Reference", "[nvarchar](500)");
                CheckTable(db, "ImagesDetails", fields, new string[] { "ID" });
                break;

            case 19:                                                //Project Questions
                fields.Add("ID", "[int]");
                fields.Add("ProjectID", "[int]");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("Value", "[nvarchar](100)");
                fields.Add("Type", "[int]");
                CheckTable(db, "ProjectPricing", fields, new string[] { "ID" });
                break;
            default:
                return false;
        }
        return true;
    }
}

