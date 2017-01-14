using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Core;
using WebSite.Core.Repositories;
using WebSite.Persistence.Repositories;

namespace WebSite.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebSiteDBEntities _context;

        public UnitOfWork(WebSiteDBEntities context)
        {
            _context = context;
            Login = new LoginRepository(_context);
            // Teacher = new TeacherRepository(_context); 
        }
        public ILoginRepository Login { get; private set; }
        //public IsomeIntefrace asdasd {get; private set;}

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}