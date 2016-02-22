namespace pluralsight_challenge.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Dispatcher;
    using StructureMap;

    /// <summary>
    /// Structure map dependency scope for WebApi
    /// </summary>
    public class StructureMapDependencyScope : IDependencyScope
    {
        /// <summary>
        /// Instance of the container
        /// </summary>
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyScope" /> class.
        /// </summary>
        /// <param name="container">Instance of the container</param>
        public StructureMapDependencyScope(IContainer container)
        {
            _container = container;
        }
 
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>The retrieved service.</returns>
        public object GetService(Type serviceType)
        {
            return _container.TryGetInstance(serviceType);
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>The retrieved collection of services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _container = null;
        }
    }

    /// <summary>
    /// Class to support StructureMap in WebApi controllers
    /// </summary>
    public class StructureMapWebApiDependencyResolver : StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator
    {
        /// <summary>
        /// Instance of the container
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapWebApiDependencyResolver" /> class.
        /// </summary>
        /// <param name="container">Instance of the container</param>
        public StructureMapWebApiDependencyResolver(IContainer container)
            : base(container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;

            _container.Inject(typeof(IHttpControllerActivator), this);
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(_container.GetNestedContainer());
        }
        
        /// <summary>
        /// Creates an System.Web.Http.Controllers.IHttpController object.
        /// </summary>
        /// <param name="request">The message request</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>An System.Web.Http.Controllers.IHttpController object.</returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return _container.GetNestedContainer().GetInstance(controllerType) as IHttpController;
        }
    }
}