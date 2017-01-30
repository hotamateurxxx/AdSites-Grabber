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
        public AvitoAdvert Parse(IWebElement container)
        {
            AvitoAdvert advert = new AvitoAdvert();

            ParseTitle(container, ref advert);
            ParseCategories(container, ref advert);
            ParseBody(container, ref advert);

            return advert;
        }

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <returns>Объявление в контейнере.</returns>
        public AvitoAdvert Parse(IWebElement container, ref Advert advertOnList)
        {
            AvitoAdvert advert = Parse(container);
            advert.Url = advertOnList.Url;
            advert.UpdateTime = advertOnList.UpdateTime;
            return advert;
        }

        /// <summary>
        /// Разбор заголовка.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseTitle(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement h1 = waitElement("h1.h1[itemprop=name]", container);
            advert.Title = h1.Text;
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        private void ParseCategories(IWebElement container, ref AvitoAdvert advert)
        {
            String cssSelector = ".b-catalog-breadcrumbs .breadcrumb-link";
            waitElement(cssSelector, container);
            IWebElements links = container.FindElements(By.CssSelector(cssSelector));
            // Создаем новую категорию
            Category category = new Category();
            foreach (IWebElement link in links)
            {
                // Добавляем новый элемент категории
                category.Items.Add(link.Text);
            }
            // Добавляем категорию в список
            advert.Categories.Add(category);
        }

        /// <summary>
        /// Разбор тела.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseBody(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement div = waitElement("div.g_92", container);
            ParsePrice(div, ref advert);
            ParseLocation(div, ref advert);
            ParseText(div, ref advert);
            ParseId(div, ref advert);
            //ParseUpdateTime(div, ref advert);
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
            catch (NoSuchElementException)
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
            try
            {
                IWebElement elem = waitElement("#desc_text > p", container);
                advert.Text = elem.Text;
                advert.HtmlText = elem.ToString();
            }
            catch (NoSuchElementException)
            {
                IWebElement elem = waitElement(".description.description-expanded", container);
                advert.Text = elem.Text;
                advert.HtmlText = elem.ToString();
            }

        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParsePrice(IWebElement container, ref AvitoAdvert advert)
        {
            try
            {
                IWebElement elem = waitElement(".description_Price .p_i_Price span[itemprop=Price]", container);
                advert.Price.RawValue = elem.Text;

                if (Regex.Match(advert.Price.RawValue, "руб.").Success)
                {
                    advert.Price.Value = Convert.ToDecimal(Regex.Replace(advert.Price.RawValue, "руб.", ""));
                    advert.Price.Unit = "руб.";
                }
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора цены:\n" + advert.Price.RawValue, e);
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }


    }
}
