using System.Web;
using System.Web.Mvc;

namespace MultiPlatfomPlatformGame.WebEditorASP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
