using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Элемент категории.
    /// </summary>
    class CategoryItem
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Имя элемента.
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CategoryItem()
            : base()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя элемента.</param>
        public CategoryItem(string name) 
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            return Name;
        }

    }
}
