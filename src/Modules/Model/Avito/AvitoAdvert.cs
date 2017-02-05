using System;

namespace AdSitesGrabber.Model.Avito
{
    
    /// <summary>
    /// Объявление с сайта Avito.
    /// </summary>
    public class AvitoAdvert : Advert
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
            return base.ToString() + "\n" + "Идентификатор: " + Id.ToString();
        }

    }
}
