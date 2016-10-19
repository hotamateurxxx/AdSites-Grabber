using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NHibernate;
using NHibernate.Cfg;

using AdSitesGrabber.Controller;
using AdSitesGrabber.Tests;

namespace AdSitesGrabber
{
    class Program
    {

        static void Main(string[] args)
        {

            Random random = new Random();
            Item item;

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession currentSession = sessionFactory.OpenSession();
            ITransaction tx = currentSession.BeginTransaction();
            for (int idx = 0; idx < 10; idx++)
            {
                item = new Item();
                item.Name = random.Next().ToString();
                currentSession.Save(item);
            }
            tx.Commit();
            currentSession.Close();

            using (WebDriverManager manager = new WebDriverManager())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();
            }

        }

    }
}
