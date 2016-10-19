using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NHibernate;
using NHibernate.Cfg;

using log4net;
using log4net.Config;

using AdSitesGrabber.Controller;
using AdSitesGrabber.Tests;
using AdSitesGrabber.Model;

namespace AdSitesGrabber
{
    class Program
    {

        static void Main(string[] args)
        {

            // Запускаем конфигурацию log4net напрямую, потому что непонятно что будет отрабатывать раньше:
            // мой вызов LogManager или какая-то из инструкций NHibernate.
            XmlConfigurator.Configure();

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession currentSession = sessionFactory.OpenSession();
            ITransaction tx = currentSession.BeginTransaction();

            Random random = new Random();
            for (int idx = 0; idx < 10; idx++)
            {
                Category category = new Category();
                category.Tags.Add(random.Next().ToString());
                category.Tags.Add(random.Next().ToString());
                category.Tags.Add(random.Next().ToString());
                currentSession.Save(category);

            }

            tx.Commit();
            currentSession.Close();

            Console.WriteLine("Done.");
            Console.ReadLine();

            /*
            using (WebDriverManager manager = new WebDriverManager())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();
            }
            */

        }

    }
}
