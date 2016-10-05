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
    class AvitoGrabber : Grabber
    {

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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="locationName">Имя места, для которого выбираются объявления.</param>
        /// <param name="startUrl">Отправной адрес, с которого начинается работа граббера.</param>
        public AvitoGrabber(string locationName = null, string startUrl = null) 
            : base(locationName, startUrl)
        {
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        public override void Execute()
        {
            // Создаем драйвер
            driver = new FirefoxDriver();
            // Загружаем отправную страницу
            driver.Navigate().GoToUrl(startUrl);
            // Выбираем город
            selectLocation();
            // Обрабатываем объявления на текущей странице
            processPageAdverts();
            driver.Close();
        }

        /// <summary>
        /// Выбор географического места (города) объявлений.
        /// </summary>
        /// <exception cref="Exception">Если указанное место не найдено.</exception>
        protected void selectLocation()
        {
            try
            {
                IWebElement link = driver.FindElement(By.LinkText(locationName));
                link.Click();
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                throw new Exception("Заданное место не найдено", e);
            }
        }

        /// <summary>
        /// Обработка объявлений на текущей странице.
        /// </summary>
        protected void processPageAdverts()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> divs = driver.FindElements(By.CssSelector(".catalog-list div.item-table"));
            if (divs.Count == 0)
                throw new Exception("На странице не найдено объявлений.");
            foreach (IWebElement div in divs)
            {
                // id объявления
                int id = Convert.ToInt16(div.GetAttribute("id").Substring(1));
                
                // Тип объявления (2 - сверху, 1 - обычное)
                string dataType = div.GetAttribute("data-type");

                // Заголовок объявления (наименование товара)
                string title;
                string url;
                try
                {
                    IWebElement h3 = div.FindElement(By.CssSelector(".item-description-title"));
                    title = h3.Text;

                    // Ссылка на объявление
                    try
                    {
                        IWebElement a = h3.FindElement(By.CssSelector("a"));
                        url = a.GetAttribute("href");
                    }
                    catch (OpenQA.Selenium.NoSuchElementException e)
                    {
                        throw new Exception("Ссылка на объявление не найдена", e);
                    }
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    throw new Exception("Заголовок объявления найден", e);
                }

                // Цена
                string priceStr;
                try
                {
                    IWebElement about = div.FindElement(By.CssSelector("div.about"));
                    priceStr = about.Text;
                }
                finally
                {
                    // Do nothing
                }

                // Категории, Обновлено
                List<List<string>> categories = new List<List<string>>();
                string updatedStr;
                DateTime updated;
                try
                {
                    IWebElement data = div.FindElement(By.CssSelector("div.data"));
                    priceStr = data.Text;

                    // Категории
                    try
                    {
                        IWebElement p = div.FindElement(By.CssSelector("p"));
                        /// <remark>
                        /// Здесь надо сказать что совсем правильно было бы не брать весь текст параграфа, а потом разбивать 
                        /// его по делителю. Надо бы обойти каждый текстовый узел в отдельности, но средствами Selenuim это 
                        /// делается только через выполнение JS.
                        /// </remark>
                        string[] tags = p.Text.Split('|');
                        List<string> category = new List<string>();
                        foreach (string tag in tags)
                        {
                            category.Add(tag);
                        }
                        categories.Add(category);
                    }
                    finally
                    {
                        // Do nothing
                    }

                    // Обновлено
                    try
                    {
                        IWebElement element = div.FindElement(By.CssSelector("div.clearfix div.date"));
                        updatedStr = element.Text;
                        updated = DateTime.Parse(updatedStr);
                    }
                    finally
                    {
                        // Do nothing
                    }
                }
                finally
                {
                    // Do nothing
                }

                // 

                // Заглавная фотография
                string imgUrl;
                try
                {
                    IWebElement img = div.FindElement(By.CssSelector(".b-photo img.photo-count-show"));
                    imgUrl = img.GetAttribute("src");
                }
                finally
                {
                    // Do nothing
                }

                // Количество фотографий
                int photoCount;
                try
                {
                    IWebElement i = div.FindElement(By.CssSelector(".b-photo .photo-icons i"));
                    photoCount = Convert.ToInt16(i.Text);
                }
                finally
                {
                    // Do nothing
                }

            }
            /*
            // Список ссылок на объявления
            List<string> urls = new List<string>();
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> links = driver.FindElements(By.CssSelector(".catalog-list a.item-description-title-link"));
            foreach (IWebElement link in links)
            {
                string url = link.GetAttribute("href");
                urls.Add(url);
            }
             * */
            /*
                AvitoAdvert advert = new AvitoAdvert(url, null);
                advert.Execute();
            */

        }

    }
}
