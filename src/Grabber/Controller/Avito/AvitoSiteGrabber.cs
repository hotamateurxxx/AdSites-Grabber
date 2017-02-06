using AdSitesGrabber.Extensions;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using OpenQA.Selenium;
using System;

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
                // Ждем окончания загрузки JS и jQuery
                driver.WaitForJSandJQueryToLoad();
                // Обрабатываем объявления на текущей странице
                processPageAdverts(driver);
                int count = Math.Min(execParams.Count, adverts.Count);
                using (AvitoAdvertOnPageParser grabber = new AvitoAdvertOnPageParser(driver))
                {
                    // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
                    for (; idx < count; idx++)
                    {
                        try
                        {
                            Advert advertOnList = adverts[idx];
                            Advert advertOnPage = grabber.Parse(advertOnList.Url);
                            adverts[idx] = advertOnPage;
                            String msg = String.Format("\n\nДобавлено объявление со страницы {0}/{1}:", idx + 1, count) + "\n" + adverts[idx];
                            Logger.Events.Info(msg);
                            Console.WriteLine(msg);
                        }
                        catch (Exception e)
                        {
                            Logger.Warns.Error(String.Format("\n\nОшибка добавления объявения со страницы {0}/{1}:", idx + 1, count) + "\n" + adverts[idx].Url, e);
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
            String cssSelector = ".catalog-list div.item_table";
            driver.WaitElement(cssSelector);
            IWebElements divs = driver.FindElements(cssSelector);

            using (AvitoAdvertOnListParser grabber = new AvitoAdvertOnListParser(driver))
            {
                for (int i = 0; i < divs.Count; i++)
                {
                    IWebElement div = divs[i];
                    try
                    {
                        AvitoAdvert advert = grabber.Parse(div);
                        advert.Location.Region = Region;
                        adverts.Add(advert);
                        String msg = String.Format("\n\nОбъявление со списка добавлено {0}/{1}:", i + 1, divs.Count) + "\n" + advert;
                        Logger.Events.Info(msg);
                        Console.WriteLine(msg);
                    }
                    catch (Exception e)
                    {
                        Logger.Warns.Error(String.Format("\n\nОшибка добавления объявления со списка {0}/{1}:", i + 1, divs.Count) + "\n" + div.ToStringExt(), e);
                        Console.ReadLine();
                    }
                }
            }
        }

    }

}
