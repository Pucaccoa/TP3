﻿using System.Web;
using System.Web.Mvc;

namespace _420_476_TP3_Pucacco_Alex
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
