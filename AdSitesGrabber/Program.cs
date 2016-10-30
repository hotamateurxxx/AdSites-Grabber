using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Engine;
using NHibernate.Mapping;

using log4net;
using log4net.Config;

using AdSitesGrabber.Controller;
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

            /*

            Configuration configuration = new Configuration().Configure();
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            ISession currentSession = sessionFactory.OpenSession();
            ITransaction tx = currentSession.BeginTransaction();

            Category category;
            
            category = new Category();
            category.Items.Add("Недвижимость");
            category.Items.Add("Квартиры");
            category.Items.Add("Новостройки");
            currentSession.Save(category);

            category = new Category();
            category.Items.Add("Недвижимость");
            category.Items.Add("Квартиры");
            category.Items.Add("Вторичка");
            currentSession.Save(category);

            category = new Category();
            category.Items.Add("Недвижимость");
            category.Items.Add("Частные дома");
            currentSession.Save(category);

            category = new Category();
            category.Items.Add("Недвижимость");
            category.Items.Add("Земельные участки");
            currentSession.Save(category);

            tx.Commit();
            currentSession.Close();

            Console.WriteLine("Done.");
            Console.ReadLine();
            */

            
            using (WebDriverManager manager = WebDriverManager.GetInstance())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                manager.FactoryDriver = WebDriverManager.DriverType.PhantomJS;
                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();
            }

        }

    }
}
