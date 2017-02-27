using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Исключение разбора строки.
    /// </summary>
    public class ParseException : Exception
    {

        /// <summary>
        /// Входящая строка.
        /// </summary>
        public String InputString { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Текст исключения.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        /// <param name="inputString">Входящая строка.</param>
        public ParseException(String message, Exception innerException, String inputString) :
            base(message, innerException)
        {
            InputString = inputString;
        }

    }
}
