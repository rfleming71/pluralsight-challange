namespace pluralsight_challenge
{
    using System.Web.Mvc;

    /// <summary>
    /// Class for setting up global filters
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters to the configuration
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
