using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using AdSitesGrabber.Model;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрастный граббер сайта.
    /// </summary>
    abstract class Grabber : IDisposable
    {

        /// <summary>
        /// Параметры запуска граббера.
        /// </summary>
        public class ExecuteParams {

            /// <summary>
            /// Количество загружаемых объявлений.
            /// </summary>
            public int Count;

        }

        /// <summary>
        /// Отправной адрес, с которого начинается работа граббера.
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Имя места, для которого выбираются объявления.
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Объявления.
        /// </summary>
        protected List<Advert> adverts;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public Grabber(string region, string url)
        {
            adverts = new List<Advert>();
            Region = region;
            Url = url;
        }

        /// <summary>
        /// Освобождение.
        /// </summary>
        public virtual void Dispose()
        {
            // do nothing yet
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string advertsStr = "";
            foreach (Advert advert in adverts)
            {
                advertsStr += "\n\n" + advert;
            }
            advertsStr = advertsStr.Replace("\n", "\n\t");

            return Url + "\n" + Region + "\n" + "Объявления:" + advertsStr;
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        /// <param name="execParams">Параметры выполнения граббера.</param>
        abstract public void Execute(ExecuteParams execParams);


        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="by">Критерий выбора элемента.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(IWebDriver driver, By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            return wait.Until(drv => driver.FindElement(by));
        }


        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(IWebDriver driver, String cssSelector)
        {
            return waitElement(driver, By.CssSelector(cssSelector));
        }

    }

}
