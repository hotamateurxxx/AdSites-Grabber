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
    public class Location
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Регион.
        /// </summary>
        public virtual String Region { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public virtual String City { get; set; }

        /// <summary>
        /// Район.
        /// </summary>
        public virtual String District { get; set; }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override String ToString()
        {
            return
            (
                         "Регион: " + Region
                + "\n" + "Город: " + District
                + "\n" + "Район: " + District
            );
        }

    }
}
