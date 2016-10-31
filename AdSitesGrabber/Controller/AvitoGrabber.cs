﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <param name="webManager">Мэнеджер веб-драйверов.</param>
        public AvitoGrabber(string locationName = null, string startUrl = null)
            : base(locationName, startUrl)
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
                // Создаем драйвер
                IWebDriver driver = webManager.OccupyDriver(this);
                // Загружаем отправную страницу
                driver.Navigate().GoToUrl(startUrl);
                // Выбираем город
                selectLocation(driver);
                // Обрабатываем объявления на текущей странице
                processPageAdverts(driver);
                // Когда закончили читать объявления в списках - заходим по ссылке на каждое объявление и дочитываем его
                for (int idx = 0; idx < adverts.Count; idx++)
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
                IWebElement link = driver.FindElement(By.LinkText(locationName));
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
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> divs = driver.FindElements(By.CssSelector(".catalog-list div.item_table"));
            if (divs.Count == 0)
                throw new Exception("На странице не найдено объявлений.");
            foreach (IWebElement div in divs)
            {
                try
                {
                    Advert advert = new AvitoAdvertOnList(div);
                    advert.Location.Region = locationName;
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
