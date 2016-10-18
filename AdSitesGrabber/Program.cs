using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NHibernate;
using NHibernate.Cfg;

using AdSitesGrabber.Controller;

namespace AdSitesGrabber
{
    class Program
    {

        static void Main(string[] args)
        {

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession currentSession = sessionFactory.OpenSession();

            Item item = new Item();
            item.Name = "Some Name";

            ITransaction tx = currentSession.BeginTransaction();
            currentSession.Save(item);
            tx.Commit();
            currentSession.Close();

            Console.WriteLine("Done.");
            Console.ReadLine();

            using (WebDriverManager manager = new WebDriverManager())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();
            }

        }

    }
}
