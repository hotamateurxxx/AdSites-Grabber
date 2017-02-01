using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;

namespace AdSitesGrabber.Controller.Avito
{
    abstract class AvitoAdvertGrabber : AdvertGrabber
    {

        public AvitoAdvertGrabber(IWebDriver driver) : 
            base(driver)
        {
        }

        /// <summary>
        /// Извлечение штампа времени из строки.
        /// </summary>
        /// <param name="inputStr">Входящая строка.</param>
        /// <returns>Штамп времени.</returns>
        protected static DateTime ExtractDateTime(String inputStr)
        {
            try
            {
                inputStr = Regex.Replace(inputStr, "\\s+в\\s+", " ", RegexOptions.IgnoreCase);
                DateTime timeNow = new DateTime();
                timeNow = DateTime.Now;
                inputStr = Regex.Replace(inputStr, "Сегодня", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                timeNow = timeNow.AddDays(-1);
                inputStr = Regex.Replace(inputStr, "Вчера", timeNow.ToShortDateString(), RegexOptions.IgnoreCase);
                return DateTime.Parse(inputStr);
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора штампа времени объявления:\n" + inputStr, e);
                throw e;
            }
        }

        /// <summary>
        /// Извлечение идентификатора объявления из строки.
        /// </summary>
        /// <param name="inputStr">Входящая строка.</param>
        /// <returns>Идентификатор объявления.</returns>
        protected static UInt64 ExtractId(String inputStr)
        {
            try
            {
                return Convert.ToUInt64(inputStr);
            }
            catch (FormatException e)
            {
                Logger.Warns.Error("Ошибка разбора номера объявления:\n" + inputStr, e);
                throw e;
            }
        }

    }
}
