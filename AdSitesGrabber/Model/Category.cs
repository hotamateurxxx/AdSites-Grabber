using System;
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

        public virtual Guid RecId { get; set; }
        public virtual List<string> Tags { get; set; }

        #region Public Methods

        /// <summary>
        /// Конструктор
        /// </summary>
        public Category()
        {
            Tags = new List<string>();
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string result = "";
            foreach (string tag in Tags)
            {
                result +=
                (
                    ((result == "") ? "" : " | ") 
                    + tag
                );
            }
            return result;
        }

        #endregion

    }

}
