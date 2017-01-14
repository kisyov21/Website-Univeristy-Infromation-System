using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;
using WebSite.Core.Repositories;
using WebSite.Persistence.Repositories;

namespace WebSite.Persistence.Repositories
{
    public class LoginRepository : Repository<tblLogin>, ILoginRepository
    {
        public LoginRepository(WebSiteDBEntities context) : base(context)
        {
        }

        public void Add(Login entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Login> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Login> Find(Expression<Func<Login, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void GetByUsernameAndPassword(Login user)
        {
            throw new NotImplementedException();
        }


        public void Remove(Login entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Login> entities)
        {
            throw new NotImplementedException();
        }

        Login IRepository<Login>.Get(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Login> IRepository<Login>.GetAll()
        {
            throw new NotImplementedException();
        }

        public WebSiteDBEntities WebSiteDBEntities
        {
            get { return Context as WebSiteDBEntities; }
        }
    }
}