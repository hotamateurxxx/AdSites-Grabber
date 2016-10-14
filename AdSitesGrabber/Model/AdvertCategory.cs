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
    class Category : List<string>
    {

        #region Public Methods

        /// <summary>
        /// Конструктор
        /// </summary>
        public Category()
            : base()
        {
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string result = "";
            foreach (string tag in this)
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
