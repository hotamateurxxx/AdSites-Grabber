using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber
{

    /// <summary>
    /// Менеджер веб-драйверов.
    /// </summary>
    interface IWebDriverManager 
    {

        /// <summary>
        /// Получить свободный драйвер.
        /// </summary>
        /// <param name="owner">Будущий владелец драйвера.</param>>
        /// <returns>Готовый к работе веб-драйвер</returns>
        IWebDriver GetFreeDriver(Object owner);

        /// <summary>
        /// Освободить ранее полученный веб-драйвер.
        /// </summary>
        /// <param name="driver">Ранее полученный веб-драйвер.</param>
        void ReleaseDriver(IWebDriver driver);

    }


    class WebDriverRecord
        : IWebDriver;
    {

        public IWebDriver driver;
        public Object owner;

    }

    /// <summary>
    /// Менеджер веб-драйверов.
    /// </summary>
    class WebDriverManager
        : IWebDriverManager
    {

        /// <summary>
        /// Используемые веб-драйвера.
        /// </summary>
        protected List<WebDriverRecord> driversRecords;

        /// <summary>
        /// Путь к исполняемому файлу браузера FireFox.
        /// </summary>
        public string firefoxBinPath;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WebDriverManager()
        {
            driversRecords = new List<WebDriverRecord>();
        }

        /// <summary>
        /// Получить свободный драйвер.
        /// </summary>
        /// <param name="owner">Будущий владелец драйвера.</param>>
        /// <returns>Готовый к работе веб-драйвер</returns>
        public virtual IWebDriver GetFreeDriver(Object owner)
        {
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.owner == null)
                {
                    driverRec.owner = owner;
                    return driverRec.driver;
                }
            }

            WebDriverRecord drvRec = new WebDriverRecord();
            return drvRec.driver;

        }

        /// <summary>
        /// Освободить ранее полученный веб-драйвер.
        /// </summary>
        /// <param name="driver">Ранее полученный веб-драйвер.</param>
        public virtual void ReleaseDriver(IWebDriver driver)
        {
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.driver == driver)
                {
                    driverRec.owner = null;
                }
            }
        }

    }

}
