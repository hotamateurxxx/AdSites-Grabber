using System;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрактный граббер страницы.
    /// </summary>
    public class PageParser : IDisposable
    {

        /// <summary>
        /// Является ли объект уже освобожденным.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Освобождение.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Совобождение ресурсов.
        /// </summary>
        /// <param name="disposing">Освобождение и управляемых и машинных ресурсов.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                _disposed = true;
            }
        }

    }
}
