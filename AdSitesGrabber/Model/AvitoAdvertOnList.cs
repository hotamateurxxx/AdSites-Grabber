using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Объявление на Avito (каким оно видно в списке обявлений).
    /// </summary>    
    class AvitoAdvertOnList : Advert, IAdvertOnList
    {

        #region Declarations

        /// <summary>
        /// Id объявления.
        /// </summary>
        protected UInt64 id;

        /// <summary>
        /// Тип объявления.
        /// </summary>
        protected int dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AvitoAdvertOnList()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bodyElement">Элемент.</param>
        public AvitoAdvertOnList(IWebElement element)
            : base(element)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        public override void ParseElement(IWebElement bodyElement)
        {

            // Id
            ParseId(bodyElement);

            // Тип (2 - сверху, 1 - обычное)
            dataType = Convert.ToInt16(bodyElement.GetAttribute("data-type"));

            // Заголовок
            ParseTitle(bodyElement);

            // Ссылка на объявление
            ParseUrl(bodyElement);

            // Категории
            ParseCategories(bodyElement);

            // Цена
            ParsePrice(bodyElement);

            // Обновлено
            ParseUpdateTime(bodyElement);

            // Заглавная фотография
            ParseTitlePhoto(bodyElement);

            // Количество фотографий
            ParsePhotosCount(bodyElement);

        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return base.ToString() + "\n" + "Номер: " + id.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Разбор идентификатора объявления.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseId(IWebElement bodyElement)
        {
            id = Convert.ToUInt64(bodyElement.GetAttribute("id").Substring(1));
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        /// <remarks>Видимо, пока писал на Avito убрали отображение категорий в списке оъявлений.</remarks>
        private void ParseCategories(IWebElement bodyElement)
        {
            // Do nothing
        }

        /// <summary>
        /// Разбор заголовка объявления.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseTitle(IWebElement bodyElement)
        {
            try
            {
                IWebElement h3 = bodyElement.FindElement(By.CssSelector(".item-description-title"));
                title = h3.Text;
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Заголовок объявления найден", e);
            }
        }

        /// <summary>
        /// Разбор ссылки на страницу объявления.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseUrl(IWebElement bodyElement)
        {
            try
            {
                IWebElement a = bodyElement.FindElement(By.CssSelector(".item-description-title a"));
                url = a.GetAttribute("href");
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Ссылка на объявление не найдена", e);
            }
        }

        /// <summary>
        /// Разбор штампа времени.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseUpdateTime(IWebElement bodyElement)
        {
            try
            {
                IWebElement div = bodyElement.FindElement(By.CssSelector("div.clearfix div.date"));
                updateTimeStr = div.Text;

                // Готовим строку к разбору методом DateTime.Parse
                string timeStr = updateTimeStr;
                DateTime timeNow = new DateTime();
                timeNow = DateTime.Now;
                timeStr = Regex.Replace(timeStr, "Сегодня", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                timeNow = timeNow.AddDays(-1);
                timeStr = Regex.Replace(timeStr, "Вчера", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                // Разбираем строку методом DateTime.Parse
                updateTime = DateTime.Parse(timeStr);

            }
            catch (FormatException e)
            {
                throw new Exception("Не удается определить время последнего обновления", e);
            }
        }

        /// <summary>
        /// Цена.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParsePrice(IWebElement bodyElement)
        {
            try
            {
                IWebElement about = bodyElement.FindElement(By.CssSelector("div.about"));
                priceStr = about.Text;
            }
            finally
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParseTitlePhoto(IWebElement bodyElement)
        {
            try
            {
                IWebElement img = bodyElement.FindElement(By.CssSelector(".b-photo img.photo-count-show"));
                titleImgUrl = img.GetAttribute("src");
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        private void ParsePhotosCount(IWebElement bodyElement)
        {
            try
            {
                IWebElement i = bodyElement.FindElement(By.CssSelector(".b-photo .photo-icons i"));
                photosCount = Convert.ToInt16(i.Text);
            }
            catch (FormatException)
            {
                if (titleImgUrl != null)
                    photosCount = 1;
            }
            catch (NoSuchElementException)
            {
                // Нет фотографий
            }
        }

        #endregion

    }
}
