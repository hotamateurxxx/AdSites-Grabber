using System;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрактный граббер страницы.
    /// </summary>
    class PageGrabber : IDisposable
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
