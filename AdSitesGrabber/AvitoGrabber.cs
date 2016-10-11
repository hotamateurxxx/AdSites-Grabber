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
    /// Граббер сайта Avito.
    /// </summary>
    class AvitoGrabber 
        : Grabber
    {

        #region Properties

        /// <summary>
        /// Значение по-умолчанию отправного адреса, с которого начинается работа граббера.
        /// </summary>
        protected override string defaultStartUrl
        {
            get 
            {
                return "http://avito.ru/";
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="locationName">Имя места, для которого выбираются объявления.</param>
        /// <param name="startUrl">Отправной адрес, с которого начинается работа граббера.</param>
        /// <param name="driverManager">Мэнеджер веб-драйверов.</param>
        public AvitoGrabber(string locationName = null, string startUrl = null, IWebDriverManager driverManager = null)
            : base(locationName, startUrl, driverManager)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        public override void Execute()
        {
            // Создаем драйвер
            IWebDriver driver = driverManager.OccupyDriver(this);
            // Загружаем отправную страницу
            driver.Navigate().GoToUrl(startUrl);
            // Выбираем город
            selectLocation(driver);
            // Обрабатываем объявления на текущей странице
            processPageAdverts(driver);
            // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
            for (int idx = 0; idx < adverts.Count; idx++)
            {
                adverts[idx] = new AvitoAdvertOnPage(adverts[idx] as AvitoAdvertOnList, driver);
            }
            // Сейчас 
            driverManager.ReleaseDriver(driver);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Выбор географического места (города) объявлений.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной главной страницей.</param>
        /// <exception cref="Exception">Если указанное место не найдено.</exception>
        protected void selectLocation(IWebDriver driver)
        {
            try
            {
                IWebElement link = driver.FindElement(By.LinkText(locationName));
                link.Click();
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Заданное место не найдено", e);
            }
        }

        /// <summary>
        /// Обработка объявлений на текущей странице.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной страницей со списком объявлений.</param>
        protected void processPageAdverts(IWebDriver driver)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> divs = driver.FindElements(By.CssSelector(".catalog-list div.item_table"));
            if (divs.Count == 0)
                throw new Exception("На странице не найдено объявлений.");
            foreach (IWebElement div in divs)
            {
                Advert advert = new AvitoAdvertOnList(div);
                adverts.Add(advert);
            }
        }

        #endregion

    }

}
