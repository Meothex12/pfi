using System;
using System.Collections.Generic;
using System.Linq;
using pfi.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;

namespace pfi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            migrateCoutriesDataToDataBase();
        }

        public void migrateCoutriesDataToDataBase()
        {
            Database1Entities db = new Database1Entities();
            Country country = db.Countries.FirstOrDefault();
            if (country == null)
            {
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/Countries.txt"));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] tokens = line.Split(';');
                    Country newCountry = new Country();
                    newCountry.Name = tokens[0];
                    db.Countries.Add(newCountry);
                    db.SaveChanges();
                }

                sr.Dispose();
            }
            db.Dispose();
        }
    }
}
