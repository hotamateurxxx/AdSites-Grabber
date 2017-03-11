using OpenQA.Selenium;
using System;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Менеджер веб-драйверов.
    /// </summary>
    public interface IWebManager : IDisposable
    {

        /// <summary>
        /// Получить свободный драйвер.
        /// </summary>
        /// <param name="owner">Будущий владелец драйвера.</param>>
        /// <returns>Готовый к работе веб-драйвер</returns>
        IWebDriver OccupyDriver(Object owner);

        /// <summary>
        /// Освободить ранее полученный веб-драйвер.
        /// </summary>
        /// <param name="driver">Ранее полученный веб-драйвер.</param>
        void ReleaseDriver(IWebDriver driver);

    }

}
