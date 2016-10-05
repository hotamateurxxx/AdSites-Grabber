using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SiteGrabber
{
    class AvitoPreviewAdvert : Advert
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="url">Адрес объявления.</param>
        /// <param name="driver">Драйвер.</param>
        public AvitoPreviewAdvert(string url, IWebDriver driver)
            : base(url, driver)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="url">Адрес объявления.</param>
        /// <param name="driver">Драйвер.</param>
        public AvitoPreviewAdvert(string url, IWebDriver driver)
            : base(url, driver)
        {
        }

    }
}
