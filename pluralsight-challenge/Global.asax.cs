
namespace pluralsight_challenge
{
    using System.Web.Http;
    using System.Web.Mvc;
    using pluralsight_challenge.App_Start;
    using StructureMap;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            Container container = new Container(new DependencyRegistry());
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }
    }
}
