using OpenQA.Selenium;

namespace AdSitesGrabber.Extensions
{

    /// <summary>
    /// Расширение интерфейса OpenQA.Selenium.IWebElement.
    /// </summary>
    public static class IWebElementExtension
    {

        /// <summary>
        /// Поиск элемента по CSS-селектору.
        /// </summary>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Элемент.</returns>
        public static string ToStringExt(this IWebElement context)
        {
            return (
                         "--------------------------------------------"
                + "\n" + context.GetAttribute("outerHTML")/*.Replace("\n", " ").Replace("\t", " ")*/
                + "\n" + "--------------------------------------------"
            );
        }

    }
}
