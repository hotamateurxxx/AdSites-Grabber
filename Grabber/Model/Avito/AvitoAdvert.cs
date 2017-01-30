using OpenQA.Selenium;
using System;


namespace AdSitesGrabber.Model.Avito
{
    
    /// <summary>
    /// Объявление с сайта Avito.
    /// </summary>
    class AvitoAdvert : Advert
    {

        /// <summary>
        /// Id объявления.
        /// </summary>
        public UInt64 Id { get; set; }

        /// <summary>
        /// Тип объявления.
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return base.ToString() + "\n" + "Номер: " + Id.ToString();
        }

    }
}
