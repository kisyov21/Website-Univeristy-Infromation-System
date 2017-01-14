using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebSite.Core.Repositories
{
    public interface ILoginRepository : IRepository<Login>
    {
        void GetByUsernameAndPassword(Login user);
    }   
}