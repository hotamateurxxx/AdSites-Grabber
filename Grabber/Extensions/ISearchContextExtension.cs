﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdSitesGrabber.Extensions
{

    /// <summary>
    /// Расширение интерфейса OpenQA.Selenium.ISearchContext.
    /// </summary>
    public static class ISearchContextExtension
    {

        /// <summary>
        /// Поиск элемента по CSS-селектору.
        /// </summary>
        /// <param name="context">Контекст поиска.</param>
        /// <param name="cssSelector">Селектор.</param>
        /// <returns>Элемент.</returns>
        public static IWebElement FindElement(this ISearchContext context, String cssSelector)
        {
            return context.FindElement(By.CssSelector(cssSelector));
        }

        /// <summary>
        /// Значение атрибута элемента по CSS-селектору.
        /// </summary>
        /// <param name="context">Контекст поиска.</param>
        /// <param name="cssSelector">Селектор.</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <returns>Значение атрибута.</returns>
        public static String FindAttrValue(this ISearchContext context, String cssSelector, String attrName)
        {
            return FindElement(context, cssSelector).GetAttribute(attrName);
        }

    }
}
