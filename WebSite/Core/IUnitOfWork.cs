using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Core.Repositories;

namespace WebSite.Core
{
    public interface IUnitOfWork : IDisposable //expouses repositories 
    {
        ILoginRepository Login { get; }
        int Complete();
    }
}