using System.Web;
using System.Web.Mvc;

namespace Clientes2S_Backend_Auth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
