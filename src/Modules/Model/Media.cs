using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Model
{

    /// <summary>
    /// Медиа-содержимое объявления.
    /// </summary>
    public class Media
    {

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        public string TitleImgUrl { get; set; }

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        public int PhotosCount { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Media()
        {
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string urls = "Заглавная: " + TitleImgUrl;
            return (
                (PhotosCount == 0) ? "Нет фотографий" : (
                    PhotosCount.ToString() + " фотографий: " + ("\n" + urls).Replace("\n", "\n\t")
                )
            );
        }


    }
}
