using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;

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
                try
                {
                    ParseCategories(body, ref advert);
                    ParseBody(body, ref advert);
                }
                catch (Exception e)
                {
                    Logger.Warns.Error("\n\nОшибка анализа объявения:\n" + body.GetAttribute("outerHTML"), e);
                    throw e;
                }
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
            IWebElement select = waitElement("select#category", body);
            String categoryTitle = select.GetAttribute("innerText");
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
            //ParseId(container, ref advert);
            //ParseUpdateTime(container, ref advert);
        }

        /// <summary>
        /// Разбор штампа времени.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseUpdateTime(IWebElement container, ref AvitoAdvert advert)
        {
            throw new Exception("Метод пока не реализован.");
        }

        /// <summary>
        /// Разбор идентификатора объявления.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseId(IWebElement container, ref AvitoAdvert advert)
        {
            try
            {
                IWebElement elem = waitElement("#item_id", container);
                advert.Id = Convert.ToUInt64(elem.Text);
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
            try
            {
                IWebElement elem = waitElement("#price-value", container);
                String textContext = elem.GetAttribute("textContent");
                advert.Price.RawValue = Regex.Replace(textContext, "\\D", "");
                advert.Price.Value = Convert.ToDecimal(advert.Price.RawValue);

                // Ищем единицу измерения рубль
                try
                {
                    IWebElement span = waitElement("span.font_arial-rub", elem);
                    advert.Price.Unit = "рубль";
                }
                catch (WebDriverTimeoutException)
                {
                    // Do nothing
                }
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора цены:\n" + advert.Price.RawValue, e);
            }
            catch (WebDriverTimeoutException)
            {
                // Do nothing
            }
        }


    }
}
