using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Локация объявления.
    /// </summary>
    class Location
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Регион.
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Район.
        /// </summary>
        public virtual string District { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return
            (
                         "Регион: " + Region
                + "\n" + "Район: " + District
                + "\n" + "Адрес: " + Address
            );
        }

    }
}
