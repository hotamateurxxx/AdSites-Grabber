using System;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрактный граббер страницы.
    /// </summary>
    public class PageParser : IDisposable
    {

        /// <summary>
        /// Освобождение.
        /// </summary>
        public virtual void Dispose()
        {
            // do nothing yet
        }

    }
}
