using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using PropertyListModel;
using System.IO;

public partial class Default : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
       base.Page_Load(sender, e);

        if (Request.Browser.IsMobileDevice && Data1 == "")
        {
            Response.Clear();
            Response.WriteFile("mobile.htm");
            Response.End();
            return;
        }

        if (Action.Length > 0 && Action == "city")
        {
            Response.StatusCode = 301;
            Response.Redirect("/" + Data1);
            return;
        }

        Title = "India Property Map" + Global.AppTitle;

        if (Request.QueryString["reset"] != null)    //Refresh Global Data
        {
            Global.LoadGlobalData();
            return;
        }

        Society project = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == Data1);
        if (project != null && Data1.Length > 0)
        {
            Agent agent = Global.AgentList.Values.FirstOrDefault(m => m.URLName != null && m.URLName.ToLower() == Data2);

            if (Request.Browser.IsMobileDevice)    //Mobile 
            {
                Response.Clear();
                string txt = File.ReadAllText(Server.MapPath("~/mobile.htm"));

                if (!string.IsNullOrWhiteSpace(Data1))
                {
                    Society society = Global.ProjectList.Values.FirstOrDefault(m => m.URLName.ToLower() == Data1.Trim().ToLower());
                    if (society != null)
                    {
                        txt = txt.Replace(@"[PROJECT_TITLE]", "<title>" + society.SocietyName + "</title>").Replace("Loading Project Detail...", society.ProjectDetailMobile()).Replace("//customscript", "isChangePage=true; changeTo='#Details';");
                        txt = txt.Replace("//BasePath", "var BasePath='" + Global.GetRootPathVirtual + "/';");
                    }
                }
                Response.Write(txt);
                Response.End();
            }
            if (Data2 == "")
            {
                ProjectDetail.InitCtl(this, project, agent);
                ProjectDetail.Visible = true;
                MetaDescription = ProjectDetail.MetaDescription;
                MetaKeywords = ProjectDetail.MetaKeywords;
                Title = ProjectDetail.Title;
            }
            if (Data2 == "floor-plans")
            {
                FloorPlans.InitCtl(this, project, agent);
                FloorPlans.Visible = true;
                MetaDescription = FloorPlans.MetaDescription;
                MetaKeywords = FloorPlans.MetaKeywords;
                Title = FloorPlans.Title;
            }
        }
        else
        {
            Data1 = Data1.ToLower();
            Agent agent = Global.AgentList.Values.FirstOrDefault(m => m.URLName != null && m.URLName.ToLower() == Data1);

            if (agent != null)
            {
                if (Data2 == "edit")
                {
                    AgentEdit.Visible = true;
                    AgentEdit.InitCtl(this, agent);
                }
                else
                {
                    AgentMicrosite.Visible = true;
                    AgentMicrosite.InitCtrl(agent, Data2);
                    Title = AgentMicrosite.Title;
                    MetaDescription = PropList.MetaDescription;
                    MetaKeywords = PropList.MetaKeywords;
                }
            }

            else
            {
                agent = Global.BuilderList.Values.FirstOrDefault(m => m.URLName != null && m.URLName.ToLower() == Data1);
                if (agent != null)
                {
                    if (Request.Browser.IsMobileDevice)  // Mobile
                    {
                        Response.Clear();
                        string txt = File.ReadAllText(Server.MapPath("~/mobile.htm"));
                        //if (!string.IsNullOrWhiteSpace(d))
                        //{
                        //    Agent builder = Global.BuilderList.Values.FirstOrDefault(m => m.AgentName.ToLower() == d.Trim().ToLower() && m.UserType == 2);
                        //    if (builder != null)
                        //    {
                        txt = txt.Replace("<title>Property Map</title>", "<title>" + agent.AgentName + "</title>").Replace("Loading Builder Detail...", agent.BuilderDetailMobile()).Replace("//customscript", "isChangePage=true; changeTo='#BuilderDetails';");
                        //}
                        //}
                        Response.Write(txt);
                        Response.End();
                    }

                    Agent builder = Global.BuilderList.Values.FirstOrDefault(m => m.URLName != null && m.URLName.ToLower() == Data1);
                    BuilderControl.Visible = true;
                    BuilderControl.InitCtl(this, builder);
                    Title = BuilderControl.Title;
                    MetaDescription = BuilderControl.MetaDescription;
                    MetaKeywords = BuilderControl.MetaKeywords;
                }
                else
                {
                    if (Data1.Length == 0)
                        Data1 = "noida";
                    City city = Global.CityList.Values.FirstOrDefault(m => m.UrlName != null && m.UrlName.ToLower() == Data1);

                    if (city != null)
                    {
                        PropList.Visible = true;
                        PropList.InitCtlPropList(city);
                        MetaDescription = PropList.MetaDescription;
                        MetaKeywords = PropList.MetaKeywords;
                    }
                    else
                    {
                        Data1 = Data1.ToLower();
                        Availability Avl = Global.AvailabilityList.Values.FirstOrDefault(m => m.URL == Data1);
                        if (Avl != null)
                        {
                            AvlDetail.Visible = true;
                            AvlDetail.InitCtl(this, Avl);
                            Title = AvlDetail.Title;
                            MetaDescription = AvlDetail.MetaDescription;
                        }
                        else
                        {
                            Response.Redirect("/pagenotfound");
                        }
                    }
                }
            }
        }
    }
}