using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using AdSitesGrabber.Extensions;

// Псевдонимы
using IWebElements = System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement>;

namespace AdSitesGrabber.Controller.Avito
{

    /// <summary>
    /// Граббер объявления на странице.
    /// </summary>
    class AvitoAdvertOnPageGrabber : AvitoAdvertGrabber
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Вэб-драйвер.</param>
        public AvitoAdvertOnPageGrabber(IWebDriver driver) : 
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
                // Ждем окончания загрузки JS и jQuery
                Driver.WaitForJSandJQueryToLoad();
                IWebElement body = Driver.FindElement("body");
                ParseCategories(body, ref advert);
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
        /// Разбор категорий.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseCategories(IWebElement container, ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(container.FindElement("select#category"));
            String categoryTitle = select.SelectedOption.Text.Trim();
            // Создаем новую категорию
            Category category = new Category();
            // Добавляем новый элемент категории
            category.Items.Add(categoryTitle);
            // Добавляем категорию в список
            advert.Categories.Add(category);
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
        /// Разбор тела.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseBody(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement body = container.FindElement("div.item-view");
            ParseTitle(body, ref advert);
            ParsePrice(body, ref advert);
            //ParseLocation(body, ref advert);
            ParseText(body, ref advert);
            ParseSubtitle(body, ref advert);
            ParseSeller(body, ref advert);
            ParseActions(body, ref advert);
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
            catch (WebDriverTimeoutException)
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
            IWebElement popupContent = Driver.FindElement("div.js-item-phone-popup-content");

            // Телефон продавца картинкой
            advert.Contact.Phone.Src = waitAttrValue("div.item-phone-big-number img", "src", popupContent);

            // Раздел информации о продавце
            IWebElement divSeller = popupContent.FindElement("div.seller-info");

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
                catch (NoSuchElementException e)
                {
                    throw e;
                }
            }
            
            IWebElements divProps = divSeller.FindElements(By.CssSelector("seller-info-prop"));
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
        /// Разбор раздела информации о продавце (под кнопкой "Показать телефон").
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseSeller(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = waitElement("div.seller-info", container);
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
        /// Разбор места.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseLocation(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = container.FindElement("#map span[itemprop=name]");
            advert.Location.Region = elem.Text;
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseText(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = container.FindElement("div.item-description");
            advert.Text = elem.GetAttribute("textContent");
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
