using AdSitesGrabber.Extensions;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdSitesGrabber.Controller.Avito
{

    /// <summary>
    /// Граббер объявления со списка на странице.
    /// </summary>
    public class AvitoAdvertOnListParser : AvitoAdvertParser
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Вэб-драйвер.</param>
        public AvitoAdvertOnListParser(IWebDriver driver) : 
            base(driver)
        {
        }

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <returns>Объявление в контейнере.</returns>
        public AvitoAdvert Parse(IWebElement container)
        {

            AvitoAdvert advert = new AvitoAdvert();

            // Id
            advert.Id = ExtractId(container.GetAttribute("id").Substring(1));

            // Тип (2 - сверху, 1 - обычное)
            advert.DataType = Convert.ToInt16(container.GetAttribute("data-type"));

            // Регион списка
            ParseRegion(ref advert);

            // Раздел описания
            ParseDesc(container, ref advert);

            // Заголовок
            ParseTitle(container, ref advert);

            // Ссылка на объявление
            ParseUrl(container, ref advert);

            // Цена
            try
            {
                ParsePrice(container, ref advert);
            }
            catch (Exception)
            {
                // Do nothing
            }

            // Заглавная фотография
            try
            {
                ParseTitlePhoto(container, ref advert);
            }
            catch (Exception)
            {
                // Do nothing
            }

            // Количество фотографий
            try
            {
                ParsePhotosCount(container, ref advert);
            }
            catch (Exception)
            {
                advert.Media.PhotosCount = (advert.Media.TitleImgUrl != null) ? 1 : 0;
            }

            return advert;

        }

        /// <summary>
        /// Разбор региона.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseRegion(ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(Driver.FindElement("select#region"));
            String regionTitle = select.SelectedOption.Text.Trim();
            advert.Location.Region = regionTitle;
        }

        /// <summary>
        /// Анализ блока описания.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseDesc(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement descElement = container.FindElement(".description");
            ParseCategories(descElement, ref advert);
            ParseUpdateTime(descElement, ref advert);
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseCategories(IWebElement container, ref AvitoAdvert advert)
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            String jsText = "$('#i" + advert.Id.ToString() + " .data p').first().prop('firstChild').textContent";
            jsText = "return " + jsText + ";";
            String categoryTitle = ((String) js.ExecuteScript(jsText)).Trim();
            // Создаем новую категорию
            Category category = new Category();
            // Добавляем новый элемент категории
            category.Items.Add(categoryTitle);
            // Добавляем категорию в список
            advert.Categories.Add(category);
        }

        /// <summary>
        /// Разбор штампа времени.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseUpdateTime(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement div = container.FindElement("div.clearfix div.date");
            String inputStr = div.GetAttribute("innerText");
            advert.UpdateTimeStr = inputStr;
            advert.UpdateTime = ExtractDateTime(inputStr);
        }

        /// <summary>
        /// Разбор заголовка объявления.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseTitle(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement h3 = container.FindElement(".item-description-title");
            advert.Title = h3.GetAttribute("innerText");
        }

        /// <summary>
        /// Разбор ссылки на страницу объявления.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseUrl(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement a = container.FindElement(".item-description-title a");
            advert.Url = a.GetAttribute("href");
        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParsePrice(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement about = container.FindElement("div.about");
            String textContent = about.GetAttribute("innerText");
            advert.Price.RawValue = textContent.Trim();
        }

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParseTitlePhoto(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement img = container.FindElement(".b-photo img.photo-count-show");
            advert.Media.TitleImgUrl = img.GetAttribute("src");
        }

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        /// <param name="container">Контейнер объявления.</param>
        /// <param name="advert">Объявление.</param>
        private void ParsePhotosCount(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement i = container.FindElement(".b-photo .photo-icons i");
            String innerText = i.GetAttribute("innerText");
            advert.Media.PhotosCount = Convert.ToInt16(innerText);
        }

    }
}
