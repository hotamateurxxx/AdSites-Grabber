﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{
    class Contact
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public virtual Phone Phone { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public virtual String Address { get; set; }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override String ToString()
        {
            return
            (
                         "Телефон: " + Phone
                + "\n" + "Адрес: " + Address
            );
        }

    }
}