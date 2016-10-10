using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber
{

    /// <summary>
    /// Объявление на Avito (каким оно видно в списке обявлений).
    /// </summary>    
    class AvitoAdvertOnList 
        : Advert, IAdvertOnList
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
        /// <param name="element">Элемент.</param>
        public AvitoAdvertOnList(IWebElement element)
            : base(element)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="element"></param>
        public override void ParseElement(IWebElement element)
        {

            // Id
            id = Convert.ToUInt64(element.GetAttribute("id").Substring(1));

            // Тип (2 - сверху, 1 - обычное)
            dataType = Convert.ToInt16(element.GetAttribute("data-type"));

            // Заголовок и ссылка
            try
            {
                IWebElement h3 = element.FindElement(By.CssSelector(".item-description-title"));
                title = h3.Text;

                // Ссылка на объявление
                try
                {
                    IWebElement a = h3.FindElement(By.CssSelector("a"));
                    url = a.GetAttribute("href");
                }
                catch (NoSuchElementException e)
                {
                    throw new Exception("Ссылка на объявление не найдена", e);
                }
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Заголовок объявления найден", e);
            }

            // Цена
            try
            {
                IWebElement about = element.FindElement(By.CssSelector("div.about"));
                priceStr = about.Text;
            }
            finally
            {
                // Do nothing
            }

            // Категории, Обновлено
            List<List<string>> categories = new List<List<string>>();
            try
            {
                IWebElement data = element.FindElement(By.CssSelector("div.data"));
                priceStr = data.Text;

                // Категории
                try
                {
                    IWebElement p = element.FindElement(By.CssSelector("p"));
                    /// <remark>
                    /// Здесь надо сказать что совсем правильно было бы не брать весь текст параграфа, а потом разбивать 
                    /// его по делителю. Надо бы обойти каждый текстовый узел в отдельности, но средствами Selenuim это 
                    /// делается только через выполнение JS.
                    /// </remark>
                    string[] tags = p.Text.Split('|');
                    List<string> category = new List<string>();
                    foreach (string tag in tags)
                    {
                        category.Add(tag);
                    }
                    categories.Add(category);
                }
                finally
                {
                    // Do nothing
                }

                // Обновлено
                try
                {
                    IWebElement div = element.FindElement(By.CssSelector("div.clearfix div.date"));
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
                catch (FormatException)
                {
                    throw;
                }
            }
            finally
            {
                // Do nothing
            }

            // 

            // Заглавная фотография
            try
            {
                IWebElement img = element.FindElement(By.CssSelector(".b-photo img.photo-count-show"));
                titleImgUrl = img.GetAttribute("src");
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }

            // Количество фотографий
            try
            {
                IWebElement i = element.FindElement(By.CssSelector(".b-photo .photo-icons i"));
                photosCount = Convert.ToInt16(i.Text);
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }

        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return base.ToString() + "\n" + "id " + id.ToString();
        }

        #endregion

    }
}
