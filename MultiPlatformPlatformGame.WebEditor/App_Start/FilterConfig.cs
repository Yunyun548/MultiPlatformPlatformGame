using System.Web;
using System.Web.Mvc;

namespace MultiPlatformPlatformGame.WebEditor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
