using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Исключение разбора строки.
    /// </summary>
    [Serializable]
    public class ParseException : Exception, ISerializable
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

        /// <summary>
        /// При переопределении в производном классе задает объект SerializationInfo со сведениями об исключении.
        /// </summary>
        /// <param name="info">Объект SerializationInfo, содержащий сериализованные данные объекта о созданном исключении.</param>
        /// <param name="context">Объект StreamingContext, содержащий контекстные сведения об источнике или назначении.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("InputString", InputString);
            base.GetObjectData(info, context);
        }

    }
}
