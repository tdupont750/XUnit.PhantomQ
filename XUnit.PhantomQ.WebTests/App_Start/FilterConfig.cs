using System.Web;
using System.Web.Mvc;

namespace XUnit.PhantomQ.WebTests
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}