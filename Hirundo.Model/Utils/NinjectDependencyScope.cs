﻿namespace Hirundo.Model.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using Ninject;
    using Ninject.Syntax;

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolutionRoot;

        public NinjectDependencyScope(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public object GetService(Type serviceType)
        {
            return this.resolutionRoot.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.resolutionRoot.GetAll(serviceType);
        }

        public void Dispose()
        {
            if (this.resolutionRoot != null && this.resolutionRoot is IDisposable)
            {
                ((IDisposable)this.resolutionRoot).Dispose();
            }

            this.resolutionRoot = null;
        }
    }
}
