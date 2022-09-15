using System.Web;
using System.Web.Mvc;

namespace _1851010095_NguyenHongPhat_ASPNET
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
