using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Объявление.
    /// </summary>
    abstract class Advert
    {

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        /// <remarks>По сути, наименование товара.</remarks>
        public string Title { get; set; }

        /// <summary>
        /// Текст.
        /// </summary>
        /// <remarks>Без HTML.</remarks>
        public string Text {get; set; }

        /// <summary>
        /// Текст с HTML.
        /// </summary>
        /// <remarks>Текст с HTML-форматированием.</remarks>
        public string HtmlText { get; set; }

        /// <summary>
        /// Время обновления строкой.
        /// </summary>
        public string UpdateTimeStr { get; set; }

        /// <summary>
        /// Обновлено.
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Категории.
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Место.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public Price Price { get; set; }

        /// <summary>
        /// Медиа-содержимое.
        /// </summary>
        public Media Media { get; set; }

        /// <summary>
        /// Контакт.
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Advert()
        {
            Categories = new List<Category>();
            Location = new Location();
            Price = new Price();
            Media = new Media();
            Contact = new Contact();
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string CategoriesStr = "";
            foreach (Category category in Categories)
            {
                CategoriesStr +=
                (
                    ((CategoriesStr == "") ? "" : "|") 
                    + category.ToString()
                );
            }

            return
            (
                         "Ссылка: " + Url
                + "\n" + "Локация: " + ("\n" + Location).Replace("\n", "\n\t")
                + "\n" + "Категории: " + CategoriesStr
                + "\n" + "Заголовок: " + Title 
                + "\n" + "Цена: " + Price
                + "\n" + "Контакт: " + ("\n" + Contact).Replace("\n", "\n\t")
                + "\n" + "Обновлено: " + UpdateTime.ToString()
            );
        }

    }

}
