using AdSitesGrabber.Extensions;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;

// Псевдонимы
using IWebElements = System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement>;

namespace AdSitesGrabber.Controller.Avito
{

    /// <summary>
    /// Граббер объявления на странице.
    /// </summary>
    public class AvitoAdvertOnPageParser : AvitoAdvertParser
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Вэб-драйвер.</param>
        public AvitoAdvertOnPageParser(IWebDriver driver) : 
            base(driver)
        {
        }

        /// <summary>
        /// Разбор объявления по ссылке.
        /// </summary>
        /// <param name="url">Ссылка на объявление.</param>
        /// <returns>Объявление.</returns>
        public AvitoAdvert Parse(String url)
        {
            try
            {
                AvitoAdvert advert = new AvitoAdvert();
                advert.Url = url;
                Driver.Navigate().GoToUrl(advert.Url);
                Driver.WaitForJSandJQueryToLoad();
                ParseSearchForm(ref advert);
                IWebElement body = Driver.FindElement("body");
                ParseBody(body, ref advert);
                return advert;
            }
            catch (Exception e)
            {
                Logger.Warns.Error("\n\nОшибка анализа объявения по адресу:\n" + url, e);
                throw e;
            }
        }

        /// <summary>
        /// Разбор формы поиска.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseSearchForm(ref AvitoAdvert advert)
        {
            IWebElement form = Driver.FindElement("form#search_form");
            ParseSearchForm_Categories(form, ref advert);
            ParseSearchForm_Region(form, ref advert);
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="form">Форма.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseSearchForm_Categories(IWebElement form, ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(form.FindElement("select#category"));
            String categoryTitle = select.SelectedOption.Text.Trim();
            // Создаем новую категорию
            Category category = new Category();
            // Добавляем новый элемент категории
            category.Items.Add(categoryTitle);
            // Добавляем категорию в список
            advert.Categories.Add(category);
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="form">Форма.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseSearchForm_Region(IWebElement form, ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(form.FindElement("select#region"));
            String regionTitle = select.SelectedOption.Text.Trim();
            advert.Location.Region = regionTitle;
        }

        /// <summary>
        /// Разбор тела.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseBody(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement body = container.FindElement("div.item-view");
            ParseTitle(body, ref advert);
            ParseSubtitle(body, ref advert);
            ParsePrice(body, ref advert);

            // Разбор раздела контактов
            IWebElement divContacts = body.FindElement("div.item-view-contacts");
            ParseSeller(divContacts, ref advert);
            ParseActions(divContacts, ref advert);

            // Разбор текста объявления
            try
            {
                ParseText(body, ref advert);
            }
            catch
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор заголовка.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseTitle(IWebElement container, ref AvitoAdvert advert)
        {
            try
            {
                IWebElement h1 = container.FindElement("div.title-info-main .title-info-title-text");
                advert.Title = h1.GetAttribute("textContent");
            }
            catch (WebDriverTimeoutException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Разбор навигационной цепочки.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseBreadcrumbs(IWebElement container, ref AvitoAdvert advert)
        {
            String cssSelector = ".b-catalog-breadcrumbs .breadcrumb-link";
            waitElement(cssSelector, container);
            IWebElements links = container.FindElements(By.CssSelector(cssSelector));
        }

        /// <summary>
        /// Разбор раздела действий (области с кнопкой "Показать телефон").
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseActions(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = container.FindElement("div.item-actions");
            IWebElement elemPhone = elem.FindElement("div.item-phone-number");
            IWebElement button = elemPhone.FindElement("button");
            button.Click();
            try
            {
                ParsePopupPhone(ref advert);
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор всплывающего окна с телефоном.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParsePopupPhone(ref AvitoAdvert advert)
        {
            // Содержимое всплывающего окна
            IWebElement container = Driver.FindElement("div.js-item-phone-popup-content");

            // Телефон продавца картинкой
            advert.Contact.Phone.Src = waitAttrValue("div.item-phone-big-number img", "src", container);

            // Разбор информации о продавце со всплывающего окна
            ParseSeller(container, ref advert);

        }

        /// <summary>
        /// Разбор раздела информации о продавце (во всплывающем окне или на правой части страницы).
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseSeller(IWebElement container, ref AvitoAdvert advert)
        {

            // Раздел информации о продавце
            IWebElement divSeller = container.FindElement("div.seller-info");

            // Ссылка на профиль продавца (ее может не быть)
            try
            {
                advert.Contact.Url = divSeller.FindAttrValue("div.seller-info-avatar a", "href").Trim();
            }
            catch (NoSuchElementException)
            {
                try
                {
                    advert.Contact.Url = divSeller.FindAttrValue("div.seller-info-name a", "href").Trim();
                }
                catch (NoSuchElementException)
                {
                    // Do nothing
                }
            }

            IWebElements divProps = divSeller.FindElements("div.seller-info-prop");
            foreach (IWebElement divProp in divProps)
            {
                String label = divProp.FindAttrValue("div.seller-info-label", "textContent").Trim();
                String value;
                switch (label)
                {
                    case "Компания":
                    case "Магазин":
                    case "Агентство":
                    case "Работодатель":
                        value = divProp.FindAttrValue("div.seller-info-name", "textContent").Trim();
                        advert.Contact.Company = value;
                        break;

                    case "Контактное лицо":
                        value = divProp.FindAttrValue("div.seller-info-value", "textContent").Trim();
                        advert.Contact.Person = value;
                        break;

                    case "Адрес":
                        value = divProp.FindAttrValue("div.seller-info-value", "textContent").Trim();
                        advert.Contact.Address = value;
                        break;
                }
            }

        }

        /// <summary>
        /// Разбор идентификатора объявления.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseSubtitle(IWebElement container, ref AvitoAdvert advert)
        {
            // <div class="title-info-metadata-item">№ 896393562, размещено сегодня в 13:43  </div>
            IWebElement elem = container.FindElement("div.title-info-metadata-item");
            String elemText = elem.GetAttribute("textContent").Trim();
            Match match = Regex.Match(elemText, "№\\s+(\\d+),\\s+размещено\\s+(.+)");
            advert.Id = ExtractId(match.Groups[1].Value);
            advert.UpdateTimeStr = match.Groups[2].Value;
            advert.UpdateTime = ExtractDateTime(advert.UpdateTimeStr);
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseText(IWebElement container, ref AvitoAdvert advert)
        {
            //IWebElement elem = container.FindElement("div.item-description");
            IWebElement elem = container.FindElement("div.item-view-main");
            string text = elem.GetAttribute("innerText");
            advert.Text = Regex.Replace(text, @"(^\s+$[\r\n]*)|(^\s+)|(\s+$)", "", RegexOptions.Multiline);
            advert.HtmlText = elem.GetAttribute("outerHTML");
        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParsePrice(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = container.FindElement("#price-value");
            try
            {
                String textContext = elem.GetAttribute("textContent");
                advert.Price.RawValue = textContext.Trim();

                // Ищем единицу измерения рубль
                try
                {
                    IWebElement span = elem.FindElement("span.font_arial-rub");
                    String numbersOnly = Regex.Replace(textContext, "\\D", "");
                    advert.Price.Value = Convert.ToDecimal(numbersOnly);
                    advert.Price.Unit = "рубль";
                }
                catch (NoSuchElementException)
                {
                    // Do nothing
                }
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора цены:\n" + advert.Price.RawValue + "\n" + elem.GetAttribute("outerHTML"), e);
                throw e;
            }
        }


    }
}
