using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Cfg;

namespace NHibernateTests
{
    class Program
    {
        static void Main(string[] args)
        {

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession currentSession = sessionFactory.OpenSession();

            Item item = new Item();
            item.Name = "Item1";

            ITransaction tx = currentSession.BeginTransaction();
            currentSession.Save(item);
            tx.Commit();

            currentSession.Close();

            Console.WriteLine("Done.");
            Console.ReadLine();

        }
    }
}
