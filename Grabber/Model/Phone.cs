using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{
    class Phone
    {

        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public virtual Guid RecId { get; set; }

        /// <summary>
        /// Ссылка на телефон.
        /// Адрес картинки или сама картинка бинарной строкой. По сути, значение указываемое в атрибуте "src".
        /// </summary>
        public virtual String Src { get; set; }

        /// <summary>
        /// Непосредственно сам номер телефона.
        /// </summary>
        public virtual String Number { get; set; }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override String ToString()
        {
            return
            (
                         "Изображение: " + Src
                + "\n" + "Номер: " + Number
            );
        }

    }
}
