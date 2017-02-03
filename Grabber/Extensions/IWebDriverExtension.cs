using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdSitesGrabber.Extensions
{
    public static class IWebDriverExtension
    {

        /// <summary>
        /// Таймаут ожидания веб-элемента после загрузки текста страницы в мс.
        /// </summary>
        public static int WaitTimeout { get; set; }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="by">Критерий выбора элемента.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        public static IWebElement WaitElement(this IWebDriver driver, By by, ISearchContext context = null)
        {
            context = context ?? driver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(WaitTimeout));
            return wait.Until(drv => context.FindElement(by));
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        public static IWebElement WaitElement(this IWebDriver driver, String cssSelector, ISearchContext context = null)
        {
            return driver.WaitElement(By.CssSelector(cssSelector), context);
        }

        /// <summary>
        /// Ждать окончания загрузки JavaScript и jQuery.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <returns>Завершена ли загрузка.</returns>
        public static Boolean WaitForJSandJQueryToLoad(this IWebDriver driver)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(WaitTimeout));

            // wait for jQuery to load
            ExpectedConditions<Boolean> jQueryLoad = new ExpectedConditions<Boolean>() {
              @Override
              public Boolean apply(WebDriver driver) {
                try {
                  return ((Long)((JavascriptExecutor)getDriver()).executeScript("return jQuery.active") == 0);
                }
                catch (Exception e) {
                  // no jQuery present
                  return true;
                }
              }
            };

            // wait for Javascript to load
            ExpectedConditions<Boolean> jsLoad = new ExpectedConditions<Boolean>() {
              @Override
              public Boolean apply(WebDriver driver) {
                return ((JavascriptExecutor)getDriver()).executeScript("return document.readyState")
                .toString().equals("complete");
              }
            };

          return wait.until(jQueryLoad) && wait.until(jsLoad);
        
        }

    }
}
