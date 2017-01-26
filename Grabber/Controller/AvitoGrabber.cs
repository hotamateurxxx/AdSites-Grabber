using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using NHibernate;
using NHibernate.Cfg;

using AdSitesGrabber.Model;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Граббер сайта Avito.
    /// </summary>
    class AvitoGrabber : Grabber
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public AvitoGrabber(string region, string url)
            : base(region, url)
        {
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        /// <param name="execParams">Параметры выполнения граббера.</param>
        public override void Execute(ExecuteParams execParams)
        {
            using (IWebManager webManager = WebManager.GetInstance())
            {
                // Общий индекс выполнения
                int idx = 0;
                // Создаем драйвер
                IWebDriver driver = webManager.OccupyDriver(this);
                // Загружаем отправную страницу
                driver.Navigate().GoToUrl(Url);
                // Выбираем город
                selectLocation(driver);
                // Обрабатываем объявления на текущей странице
                processPageAdverts(driver);
                // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
                for (; idx < Math.Min(execParams.Count, adverts.Count); idx++)
                {
                    try
                    {
                        adverts[idx] = new AvitoAdvertOnPage(adverts[idx] as AvitoAdvertOnList, driver);
                        Logger.Events.Info("Добавлено объявление со страницы:\n" + adverts[idx]);
                    }
                    catch (Exception e)
                    {
                        Logger.Warns.Error("Ошибка добавления объявения со страницы:\n" + adverts[idx].Url, e);
                    }
                }
                // Освобождение драйвера 
                webManager.ReleaseDriver(driver);
            }
        }

        /// <summary>
        /// Выбор географического места (города) объявлений.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной главной страницей.</param>
        /// <exception cref="Exception">Если указанное место не найдено.</exception>
        protected void selectLocation(IWebDriver driver)
        {
            try
            {
                IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                IWebElement link = wait.Until(drv => drv.FindElement(By.LinkText(Region)));
                link.Click();
            }
            catch (NoSuchElementException e)
            {
                Logger.Warns.Error("Заданное место не найдено", e);
                throw e;
            }
        }

        /// <summary>
        /// Обработка объявлений на текущей странице.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной страницей со списком объявлений.</param>
        protected void processPageAdverts(IWebDriver driver)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> divs;
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            divs = wait.Until(drv => drv.FindElements(By.CssSelector(".catalog-list div.item_table"));
            if (divs.Count == 0)
                throw new Exception("На странице не найдено объявлений.");
            foreach (IWebElement div in divs)
            {
                try
                {
                    Advert advert = new AvitoAdvertOnList(div);
                    advert.Location.Region = Region;
                    adverts.Add(advert);
                    Logger.Events.Info("Объявление со списка добавлено:\n" + advert);
                }
                catch (Exception e) 
                {
                    Logger.Warns.Error("Ошибка добавления объявления со списка:\n\n" + div.ToString(), e);
                }
            }
        }

    }

}
