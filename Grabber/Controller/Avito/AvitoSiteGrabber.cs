using System;
using OpenQA.Selenium;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using AdSitesGrabber.Extensions;

// Псевдонимы
using IWebElements = System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement>;


namespace AdSitesGrabber.Controller.Avito
{

    /// <summary>
    /// Граббер сайта Avito.
    /// </summary>
    class AvitoSiteGrabber : SiteGrabber
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public AvitoSiteGrabber(string region, string url)
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
                using (AvitoAdvertOnPageGrabber grabber = new AvitoAdvertOnPageGrabber(driver))
                {
                    // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
                    for (; idx < Math.Min(execParams.Count, adverts.Count); idx++)
                    {
                        try
                        {
                            Advert advertOnList = adverts[idx];
                            Advert advertOnPage = grabber.Parse(advertOnList.Url);
                            adverts[idx] = advertOnPage;
                            Logger.Events.Info("\n\nДобавлено объявление со страницы:\n" + adverts[idx]);
                        }
                        catch (Exception e)
                        {
                            Logger.Warns.Error("\n\nОшибка добавления объявения со страницы:\n" + adverts[idx].Url, e);
                            Console.ReadLine();
                        }
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
                IWebElement link = driver.WaitElement(By.LinkText(Region));
                link.Click();
            }
            catch (WebDriverTimeoutException e)
            {
                throw new Exception("Заданное место не найдено.", e);
            }
        }


        /// <summary>
        /// Обработка объявлений на текущей странице.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной страницей со списком объявлений.</param>
        protected void processPageAdverts(IWebDriver driver)
        {
            // Ждем загрузки jQuery
            IWebElement element = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('body')[0]");
            Console.WriteLine("element.id == " + element.GetAttribute("id"));

            String cssSelector = ".catalog-list div.item_table";
            try
            {
                driver.WaitElement(By.CssSelector(cssSelector));
            }
            catch (WebDriverTimeoutException e)
            {
                throw new Exception("На странице не найдено объявлений.", e);
            }

            IWebElements divs = driver.FindElements(By.CssSelector(cssSelector));
            using (AvitoAdvertOnListGrabber grabber = new AvitoAdvertOnListGrabber(driver))
            {
                //foreach (IWebElement div in divs)
                for (int i = 0; i < divs.Count; i++)
                {
                    IWebElement div = divs[i];
                    try
                    {
                        AvitoAdvert advert = grabber.Parse(div);
                        advert.Location.Region = Region;
                        adverts.Add(advert);
                        Logger.Events.Info(String.Format("\n\nОбъявление со списка добавлено {0}/{1}:", i, divs.Count) + "\n" + advert);
                    }
                    catch (Exception e)
                    {
                        Logger.Warns.Error(String.Format("\n\nОшибка добавления объявления со списка {0}/{1}:", i, divs.Count) + "\n" + div.ToStringExt(), e);
                        Console.ReadLine();
                    }
                }
            }
        }

    }

}
