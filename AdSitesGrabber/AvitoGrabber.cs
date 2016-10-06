﻿using System;
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
            // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
            for (int idx = 0; idx < adverts.Count; idx++)
            {
                adverts[idx] = new AvitoAdvertOnPage(adverts[idx] as AvitoAdvertOnList, driver);
            }
            // Сейчас 
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
            catch (NoSuchElementException e)
            {
                throw new Exception("Заданное место не найдено", e);
            }
        }

        /// <summary>
        /// Обработка объявлений на текущей странице.
        /// </summary>
        protected void processPageAdverts()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> divs = driver.FindElements(By.CssSelector(".catalog-list div.item_table"));
            if (divs.Count == 0)
                throw new Exception("На странице не найдено объявлений.");
            foreach (IWebElement div in divs)
            {
                Advert advert = new AvitoAdvertOnList(div);
                adverts.Add(advert);
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
                AvitoAdvertOnPage advert = new AvitoAdvertOnPage(url, null);
                advert.Execute();
            */

        }

    }
}
