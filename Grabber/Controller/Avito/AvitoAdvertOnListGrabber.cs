using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
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
            ParseId(container, advert);

            // Тип (2 - сверху, 1 - обычное)
            advert.DataType = Convert.ToInt16(container.GetAttribute("data-type"));

            // Заголовок
            ParseTitle(container, advert);

            // Ссылка на объявление
            ParseUrl(container, advert);

            // Категории
            ParseCategories(container, advert);

            // Цена
            ParsePrice(container, advert);

            // Обновлено
            ParseUpdateTime(container, advert);

            // Заглавная фотография
            ParseTitlePhoto(container, advert);

            // Количество фотографий
            ParsePhotosCount(container, advert);

            return advert;

        }

        /// <summary>
        /// Разбор идентификатора объявления.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseId(IWebElement container, AvitoAdvert advert)
        {
            advert.Id = Convert.ToUInt64(container.GetAttribute("id").Substring(1));
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        /// <remarks>Видимо, пока писал на Avito убрали отображение категорий в списке оъявлений.</remarks>
        private void ParseCategories(IWebElement container, AvitoAdvert advert)
        {
            // Do nothing
        }

        /// <summary>
        /// Разбор заголовка объявления.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseTitle(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement h3 = waitElement(".item-description-title");
                advert.Title = h3.Text;
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Заголовок объявления найден", e);
            }
        }

        /// <summary>
        /// Разбор ссылки на страницу объявления.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseUrl(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement a = waitElement(".item-description-title a");
                advert.Url = a.GetAttribute("href");
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Ссылка на объявление не найдена", e);
            }
        }

        /// <summary>
        /// Разбор штампа времени.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParseUpdateTime(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement div = waitElement("div.clearfix div.date");
                advert.UpdateTimeStr = div.Text;

                // Готовим строку к разбору методом DateTime.Parse
                string timeStr = advert.UpdateTimeStr;
                DateTime timeNow = new DateTime();
                timeNow = DateTime.Now;
                timeStr = Regex.Replace(timeStr, "Сегодня", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                timeNow = timeNow.AddDays(-1);
                timeStr = Regex.Replace(timeStr, "Вчера", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                // Разбираем строку методом DateTime.Parse
                advert.UpdateTime = DateTime.Parse(timeStr);

            }
            catch (FormatException e)
            {
                throw new Exception("Не удается определить время последнего обновления", e);
            }
        }

        /// <summary>
        /// Цена.
        /// </summary>
        /// <param name="advert">Объявление.</param>
        private void ParsePrice(IWebElement container, AvitoAdvert advert)
        {
            try
            {
                IWebElement about = waitElement("div.about");
                advert.Price.RawValue = about.Text;
            }
            finally
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
                IWebElement img = waitElement(".b-photo img.photo-count-show");
                advert.Media.TitleImgUrl = img.GetAttribute("src");
            }
            catch (NoSuchElementException)
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
                IWebElement i = waitElement(".b-photo .photo-icons i");
                advert.Media.PhotosCount = Convert.ToInt16(i.Text);
            }
            catch (FormatException)
            {
                if (advert.Media.TitleImgUrl != null)
                    advert.Media.PhotosCount = 1;
            }
            catch (NoSuchElementException)
            {
                // Нет фотографий
            }
        }

    }
}
