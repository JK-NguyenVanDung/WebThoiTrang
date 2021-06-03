﻿using System.Web.Mvc;

namespace WebThoiTrang.Areas.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            [Authorize(Roles = "Nhân Viên")]

            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index",controller="Dashboard", id = UrlParameter.Optional }
            );
        }
    }
}