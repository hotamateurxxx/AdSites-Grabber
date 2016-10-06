using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber
{

    /// <summary>
    /// Предпросмотр объявления на Avito (что видно в списке обявлений).
    /// </summary>    
    class AvitoAdvertOnList 
        : Advert, IAdvertOnList
    {

        /// <summary>
        /// Id объявления.
        /// </summary>
        protected UInt64 id;

        /// <summary>
        /// Тип объявления.
        /// </summary>
        protected int dataType;

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
                    updateTime = DateTime.Parse(updateTimeStr);
                }
                catch (FormatException e)
                {
                    // Do nothing
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
            catch (NoSuchElementException e)
            {
                // Do nothing
            }

            // Количество фотографий
            try
            {
                IWebElement i = element.FindElement(By.CssSelector(".b-photo .photo-icons i"));
                photosCount = Convert.ToInt16(i.Text);
            }
            catch (NoSuchElementException e)
            {
                // Do nothing
            }

        }

    }
}
