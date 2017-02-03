using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;


namespace AdSitesGrabber.Extensions
{

    public static class IWebElementExtension
    {

        /// <summary>
        /// Поиск элемента по CSS-селектору.
        /// </summary>
        /// <param name="context">Контекст поиска.</param>
        /// <param name="cssSelector">Селектор.</param>
        /// <returns>Элемент.</returns>
        public static String ToStringExt(this IWebElement context)
        {
            return (
                         "--------------------------------------------"
                + "\n" + context.GetAttribute("outerHTML")/*.Replace("\n", " ").Replace("\t", " ")*/
                + "\n" + "--------------------------------------------"
            );
        }

    }
}
