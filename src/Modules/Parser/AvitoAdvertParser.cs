using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;
using AdSitesGrabber.Extensions;
using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;

namespace AdSitesGrabber.Controller.Avito
{

    /// <summary>
    /// Парсер объявления.
    /// </summary>
    public abstract class AvitoAdvertParser : AdvertParser
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Вэб-драйвер.</param>
        public AvitoAdvertParser(IWebDriver driver) : 
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
                throw new ParseException("Ошибка разбора штампа времени объявления", e, inputStr);
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
                throw new ParseException("Ошибка разбора номера объявления", e, inputStr);
            }
        }

        /// <summary>
        /// Проверка страницы на ошибку "Не найдено".
        /// </summary>
        /// <returns>Является ли данная страница вариацией "Не найдено" или нет.</returns>
        protected Boolean ParseNotFound()
        {
            try
            {
                IWebElement header = Driver.FindElement(".nulus .nulus__header");
                String headerText = header.GetAttribute("textContent");
                Match match = Regex.Match(headerText, "По вашему запросу ничего не найдено");
                if (match.Success == false)
                    return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

    }
}
