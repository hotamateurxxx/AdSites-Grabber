using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace AdSitesGrabber
{

    #region Interfaces

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
        IWebDriver OccupyDriver(Object owner);

        /// <summary>
        /// Освободить ранее полученный веб-драйвер.
        /// </summary>
        /// <param name="driver">Ранее полученный веб-драйвер.</param>
        void ReleaseDriver(IWebDriver driver);

    }

    #endregion

    /// <summary>
    /// Запись с информацией о веб-драйвере.
    /// </summary>
    class WebDriverRecord
    {

        public IWebDriver driver;
        public Object owner;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Драйвер.</param>
        /// <param name="owner">Владелец.</param>
        public WebDriverRecord(IWebDriver driver, Object owner)
        {
            this.driver = driver;
            this.owner = owner;
        }

    }

    /// <summary>
    /// Менеджер веб-драйверов.
    /// 
    /// Нужен для того, чтобы во всех местах проекта использовались единые методы работы с веб-драйвером.
    /// </summary>
    class WebDriverManager
        : IWebDriverManager
    {

        #region Static Declarations

        /// <summary>
        /// Используемые веб-драйвера.
        /// </summary>
        private static IWebDriverManager _instance;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Создать если экземпляр не создан. Вернуть ссылку на экземпляр.
        /// </summary>
        /// <returns>Ссылка на экземпляр.</returns>
        public static IWebDriverManager GetInstance()
        {
            _instance = _instance ?? new WebDriverManager();
            return _instance;
        }

        #endregion

        #region Protected Declarations

        /// <summary>
        /// Используемые веб-драйвера.
        /// </summary>
        protected List<WebDriverRecord> driversRecords;

        #endregion

        #region Properties

        public string FirefoxBinPath
        {
            get
            {
                return firefoxBinPath;
            }

            set
            {
                firefoxBinPath = value;
            }
        }

        #endregion

        #region Public Declarations

        /// <summary>
        /// Путь к исполняемому файлу браузера FireFox.
        /// </summary>
        protected string firefoxBinPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public WebDriverManager()
        {
            driversRecords = new List<WebDriverRecord>();
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~WebDriverManager()
        {
            // Закрываем открытые окна
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.driver is IWebDriver)
                {
                    driverRec.driver.Close();
                }
            }
            // Надо дать время на взаимодействие с браузером, а то не успеет закрыть
            Thread.Sleep(1000);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Получить свободный драйвер.
        /// </summary>
        /// <param name="owner">Будущий владелец драйвера.</param>>
        /// <returns>Готовый к работе веб-драйвер</returns>
        public virtual IWebDriver OccupyDriver(Object owner)
        {
            // Ищем первый драйвер без владельца
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.owner == null)
                {
                    // Записываем владельца и возвращаем драйвер
                    driverRec.owner = owner;
                    return driverRec.driver;
                }
            }

            // Создаем новый драйвер
            IWebDriver driver = CreateDriver();
            // Добавляем новую запись о драйвере
            WebDriverRecord drvRec = new WebDriverRecord(driver, owner);
            driversRecords.Add(drvRec);
            // Возвращаем драйвер
            return driver;
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
                    return;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Создание нового веб-драйвера.
        /// </summary>
        /// <returns>Новый веб-драйвер.</returns>
        protected virtual IWebDriver CreateDriver()
        {
            if (firefoxBinPath == null)
            {
                FirefoxDriver driver = new FirefoxDriver();
                return driver;
            }
            else
            {
                FirefoxBinary binary = new FirefoxBinary(firefoxBinPath);
                FirefoxProfile profile = new FirefoxProfile();
                return new FirefoxDriver(binary, profile);
            }
        }

        #endregion

    }

}
