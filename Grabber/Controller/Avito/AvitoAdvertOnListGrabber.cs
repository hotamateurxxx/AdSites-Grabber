using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;

namespace AdSitesGrabber.Controller.Avito
{

    class AvitoAdvertOnListGrabber : AvitoAdvertGrabber
    {

        public AvitoAdvertOnListGrabber(IWebDriver driver) : 
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

            // Id
            advert.Id = ExtractId(container.GetAttribute("id").Substring(1));

            // Тип (2 - сверху, 1 - обычное)
            advert.DataType = Convert.ToInt16(container.GetAttribute("data-type"));

            // Регион списка
            ParseRegion(ref advert);

            // Раздел описания
            ParseDesc(container, ref advert);

            // Заголовок
            ParseTitle(container, advert);

            // Ссылка на объявление
            ParseUrl(container, advert);

            // Цена
            ParsePrice(container, advert);

            // Заглавная фотография
            ParseTitlePhoto(container, advert);

            // Количество фотографий
            ParsePhotosCount(container, advert);

            return advert;

        }

        /// <summary>
        /// Разбор региона.
        /// </summary>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        private void ParseRegion(ref AvitoAdvert advert)
        {
            SelectElement select = new SelectElement(waitElement("select#region"));
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
            /*
    <div class="description">
 
    <h3 class="title item-description-title"> <a class="item-description-title-link" href="/izhevsk/planshety_i_elektronnye_knigi/romoss_sense_6_20000_mah_novyy_910493586" title="Romoss Sense 6 20000 mAh новый в Ижевске">
    Romoss Sense 6 20000 mAh новый
    </a>
     </h3> <div class="about">
    2 500 руб.       </div>
  
    <div class="data">
         <p>Планшеты и электронные книги</p>
    <p>р-н Первомайский</p>
     <div class="clearfix ">
    <div class="date c-2">
    Сегодня 14:43
    </div>
    </div> </div> </div>
            */
            IWebElement descElement = waitElement(".description", container);
            ParseCategories(descElement, ref advert);
            ParseUpdateTime(descElement, ref advert);
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        /// <remarks>Видимо, пока писал на Avito убрали отображение категорий в списке оъявлений.</remarks>
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
        /// <param name="advert">Объявление.</param>
        private void ParseUpdateTime(IWebElement container, ref AvitoAdvert advert)
        {
            IWebElement div = waitElement("div.clearfix div.date", container);
            String inputStr = div.GetAttribute("innerText");
            advert.UpdateTimeStr = inputStr;
            advert.UpdateTime = ExtractDateTime(inputStr);
        }

        /// <summary>
        /// Разбор заголовка объявления.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseTitle(IWebElement container, AvitoAdvert advert)
        {
            IWebElement h3 = waitElement(".item-description-title", container);
            advert.Title = h3.GetAttribute("innerText");
        }

        /// <summary>
        /// Разбор ссылки на страницу объявления.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseUrl(IWebElement container, AvitoAdvert advert)
        {
            IWebElement a = waitElement(".item-description-title a", container);
            advert.Url = a.GetAttribute("href");
        }

        /// <summary>
        /// Цена.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParsePrice(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement about = waitElement("div.about", container);
                String textContent = about.GetAttribute("innerText");
                advert.Price.RawValue = textContent.Trim();
            }
            catch (WebDriverTimeoutException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseTitlePhoto(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement img = waitElement(".b-photo img.photo-count-show", container);
                advert.Media.TitleImgUrl = img.GetAttribute("src");
            }
            catch (WebDriverTimeoutException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParsePhotosCount(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement i = waitElement(".b-photo .photo-icons i", container);
                String innerText = i.GetAttribute("innerText");
                advert.Media.PhotosCount = Convert.ToInt16(innerText);
            }
            catch (WebDriverTimeoutException)
            {
                advert.Media.PhotosCount = (advert.Media.TitleImgUrl != null) ? 1 : 0;
            }
            catch (FormatException)
            {
                advert.Media.PhotosCount = (advert.Media.TitleImgUrl != null) ? 1 : 0;
            }
        }

    }
}
