﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Категория объявления.
    /// </summary>
    class Category
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Упорядоченный список элементов, составляющий категорию.
        /// </summary>
        public virtual IList<CategoryItem> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Category()
        {
            Items = new List<CategoryItem>();
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string result = "";
            foreach (CategoryItem item in Items)
            {
                result +=
                (
                    ((result == "") ? "" : " | ") 
                    + item
                );
            }
            return result;
        }

    }

}
