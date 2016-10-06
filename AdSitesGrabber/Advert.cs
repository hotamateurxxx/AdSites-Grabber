﻿using System;
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
    /// Объявление на элементе веб-страницы.
    /// </summary>
    public interface IAdvertOnElement
    {

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="element">Элемент страницы, содержащий объявление.</param>
        void ParseElement(IWebElement element);

    }

    /// <summary>
    /// Объявление на странице объявления.
    /// </summary>
    public interface IAdvertOnPage 
        : IAdvertOnElement
    {

        /// <summary>
        /// Разбор страницы с объявлением.
        /// </summary>
        /// <param name="driver">Драйвер с загруженной страницей, содержащей объявление.</param>
        void ParsePage(IWebDriver driver);

    }

    /// <summary>
    /// Объявление на странице списка объявлений.
    /// </summary>
    public interface IAdvertOnList
        : IAdvertOnElement
    {
    }

    /// <summary>
    /// Объявление.
    /// </summary>
    abstract class Advert 
        : IAdvertOnElement
    {

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        protected string url;

        /// <summary>
        /// Заголовок.
        /// </summary>
        /// <remarks>По сути, наименование товара.</remarks>
        protected string title;

        /// <summary>
        /// Цена строкой.
        /// </summary>
        /// <remarks>Пока как строка, потом будем заморачиваться с валютами.</remarks>
        protected string priceStr;

        /// <summary>
        /// Время обновления строкой.
        /// </summary>
        protected string updateTimeStr;

        /// <summary>
        /// Время обновления.
        /// </summary>
        protected DateTime updateTime;

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        protected string titleImgUrl;

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        protected int photosCount;

        /// <summary>
        /// Место.
        /// </summary>
        protected string location;

        /// <summary>
        /// Текст.
        /// </summary>
        /// <remarks>Без HTML.</remarks>
        protected string text;

        /// <summary>
        /// Текст с HTML.
        /// </summary>
        /// <remarks>Текст с HTML-форматированием.</remarks>
        protected string htmlText;

        /// <summary>
        /// Категории.
        /// </summary>
        /// <remarks>Двойной список используется для того, чтобы можно было использовать и композицию и пересечение элементов.</remarks>
        protected List<List<string>> categories;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Advert()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">Элемент.</param>
        public Advert(IWebElement element)
            : this()
        {
            ParseElement(element);
        }

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="element">Элемент страницы, содержащий объявление.</param>
        abstract public void ParseElement(IWebElement element);

    }

}
