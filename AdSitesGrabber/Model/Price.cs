using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Цена объявления.
    /// </summary>
    class Price
    {

        /// <summary>
        /// Цена строкой.
        /// </summary>
        public string RawValue { get; set; }

        /// <summary>
        /// Наименование валюты в цене.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Количество валюты в цене.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Price()
        {
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return (
                Unit == null ? RawValue : Value + " " + Unit
            );
        }

    }
}
