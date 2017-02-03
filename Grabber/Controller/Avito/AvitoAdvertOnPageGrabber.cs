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
    class AvitoAdvertOnPageGrabber : AvitoAdvertGrabber
    {

        public AvitoAdvertOnPageGrabber(IWebDriver driver) : 
            base(driver)
        {
        }

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <returns>Объявление в контейнере.</returns>
        public AvitoAdvert Parse(String url)
        {
            try
            {
                AvitoAdvert advert = new AvitoAdvert();
                advert.Url = url;
                Driver.Navigate().GoToUrl(advert.Url);
                IWebElement body = waitElement("body");
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
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseTitle(IWebElement container, ref AvitoAdvert advert)
        {
            try
            {
                IWebElement h1 = waitElement("div.title-info-main .title-info-title-text", container);
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
        /// <param name="body">Элемент с телом объявления.</param>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        private void ParseCategories(IWebElement body, ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(waitElement("select#category", body));
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
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        private void ParseBreadcrumbs(IWebElement body, ref AvitoAdvert advert)
        {
            String cssSelector = ".b-catalog-breadcrumbs .breadcrumb-link";
            waitElement(cssSelector, body);
            IWebElements links = body.FindElements(By.CssSelector(cssSelector));
        }

        /// <summary>
        /// Разбор тела.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseBody(IWebElement body, ref AvitoAdvert advert)
        {
            IWebElement container = waitElement("div.item-view", body);
            ParseTitle(container, ref advert);
            ParsePrice(container, ref advert);
            //ParseLocation(container, ref advert);
            ParseText(container, ref advert);
            ParseSubtitle(container, ref advert);
            ParseSeller(container, ref advert);
            ParseActions(container, ref advert);
        }

        /// <summary>
        /// Разбор раздела действий (области с кнопкой "Показать телефон").
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseActions(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = waitElement("div.item-actions", container);
            IWebElement elemPhone = waitElement("div.item-phone-number", elem);
            IWebElement button = waitElement("button", elemPhone);
            button.Click();
            ParsePopupPhone(ref advert);
        }

        /// <summary>
        /// Разбор всплывающего окна с телефоном.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParsePopupPhone(ref AvitoAdvert advert)
        {
            // Содержимое всплывающего окна
            IWebElement popupContent = waitElement("div.js-item-phone-popup-content");

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
            try
            {
                // <div class="title-info-metadata-item">№ 896393562, размещено сегодня в 13:43  </div>
                IWebElement elem = waitElement("div.title-info-metadata-item", container);
                String elemText = elem.GetAttribute("textContent").Trim();
                Match match = Regex.Match(elemText, "№\\s+(\\d+),\\s+размещено\\s+(.+)");
                advert.Id = ExtractId(match.Groups[1].Value);
                advert.UpdateTimeStr = match.Groups[2].Value;
                advert.UpdateTime = ExtractDateTime(advert.UpdateTimeStr);
            }
            catch (WebDriverTimeoutException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор места.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseLocation(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = waitElement("#map span[itemprop=name]", container);
            advert.Location.Region = elem.Text;
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseText(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = waitElement("div.item-description", container);
            advert.Text = elem.GetAttribute("textContent");
            advert.HtmlText = elem.GetAttribute("outerHTML");
        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParsePrice(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement elem = waitElement("#price-value", container);
            try
            {
                String textContext = elem.GetAttribute("textContent");
                advert.Price.RawValue = textContext.Trim();

                // Ищем единицу измерения рубль
                try
                {
                    IWebElement span = waitElement("span.font_arial-rub", elem);
                    String numbersOnly = Regex.Replace(textContext, "\\D", "");
                    advert.Price.Value = Convert.ToDecimal(numbersOnly);
                    advert.Price.Unit = "рубль";
                }
                catch (WebDriverTimeoutException)
                {
                    // Do nothing
                }
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора цены:\n" + advert.Price.RawValue + "\n" + elem.GetAttribute("outerHTML"), e);
                Console.ReadLine();
            }
            catch (WebDriverTimeoutException)
            {
                // Do nothing
            }
        }


    }
}
