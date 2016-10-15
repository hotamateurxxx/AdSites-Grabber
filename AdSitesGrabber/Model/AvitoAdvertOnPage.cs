﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using AdSitesGrabber.Controller;

namespace AdSitesGrabber.Model
{
 
    /// <summary>
    /// Объявление на Avito (каким оно видно на странице объявления).
    /// </summary>
    class AvitoAdvertOnPage : AvitoAdvertOnList, IAdvertOnPage
    {

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bodyElement">Элемент.</param>
        public AvitoAdvertOnPage(IWebElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Апгрейд объявления со списка объявлением со страницы.
        /// </summary>
        /// <param name="advert">Объявление на странице списка.</param>
        /// <param name="driver">Веб-драйвер с загруженной страницей.</param>
        public AvitoAdvertOnPage(AvitoAdvertOnList advert, IWebDriver driver)
            : base()
        {
            url = advert.Url;
            updateTime = advert.UpdateTime;
            ParsePage(driver);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Загрузка и разбор страницы с обявлением.
        /// </summary>
        /// <param name="driver">Веб-драйвер с загруженной страницей.</param>
        public void ParsePage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(url);
            IWebElement body = driver.FindElement(By.TagName("body"));
            ParseElement(body);
        }

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        public override void ParseElement(IWebElement bodyElement)
        {
            ParseTitle(bodyElement);
            ParseCategories(bodyElement);
            ParseBody(bodyElement);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Разбор заголовка.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseTitle(IWebElement bodyElement)
        {
            IWebElement h1 = bodyElement.FindElement(By.CssSelector("h1.h1[itemprop=name]"));
            title = h1.Text;
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        private void ParseCategories(IWebElement bodyElement)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> links = bodyElement.FindElements(By.CssSelector(".b-catalog-breadcrumbs .breadcrumb-link"));
            // Создаем новую категорию
            Category category = new Category();
            foreach (IWebElement link in links)
            {
                // Добавляем новый элемент категории
                category.Add(link.Text);
            }
            // Добавляем категорию в список
            categories.Add(category);
        }

        /// <summary>
        /// Разбор тела.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseBody(IWebElement bodyElement)
        {
            IWebElement div = bodyElement.FindElement(By.CssSelector("div.g_92"));
            ParsePrice(div);
            ParseLocation(div);
            ParseText(div);
            ParseId(div);
            //ParseUpdateTime(div);
        }

        /// <summary>
        /// Разбор штампа времени.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseUpdateTime(IWebElement bodyElement)
        {
            throw new Exception("Метод пока не реализован.");
        }

        /// <summary>
        /// Разбор идентификатора объявления.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseId(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector("#item_id"));
                id = Convert.ToUInt64(elem.Text);
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор места.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseLocation(IWebElement bodyElement)
        {
            IWebElement elem = bodyElement.FindElement(By.CssSelector("#map span[itemprop=name]"));
            location = elem.Text;
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseText(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector("#desc_text > p"));
                text = elem.Text;
                htmlText = elem.ToString();
            }
            catch (NoSuchElementException)
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector(".description.description-expanded"));
                text = elem.Text;
                htmlText = elem.ToString();
            }

        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParsePrice(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector(".description_price .p_i_price span[itemprop=price]"));
                priceStr = elem.Text;

                if (Regex.Match(priceStr, "руб.").Success)
                {
                    priceValue = Convert.ToDecimal(Regex.Replace(priceStr, "руб.", ""));
                    priceUnit = "руб.";
                }
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора цены:\n" + priceStr, e);
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        #endregion

    }
}