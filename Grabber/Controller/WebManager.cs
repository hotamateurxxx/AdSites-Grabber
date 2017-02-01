using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;


namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Менеджер веб-драйверов.
    /// </summary>
    public class WebManager : IWebManager
    {

        /// <summary>
        /// Используемый тип драйвера.
        /// </summary>
        public enum DriverType { Firefox, PhantomJS }

        /// <summary>
        /// Запись с информацией о веб-драйвере.
        /// </summary>
        protected class WebDriverRecord
        {

            /// <summary>
            /// Сам веб-драйвер.
            /// </summary>
            public IWebDriver driver;

            /// <summary>
            /// Пользующийся этим веб-драйвером владелец.
            /// </summary>
            public Object owner;

            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="driver">Драйвер.</param>
            /// <param name="owner">Владелец.</param>
            public WebDriverRecord(IWebDriver driver, Object owner)
            {
                this.driver = driver;
                this.owner = owner;
            }

        }

        /// <summary>
        /// Инстанция.
        /// </summary>
        private static WebManager _instance;

        /// <summary>
        /// Используемые веб-драйвера.
        /// </summary>
        protected List<WebDriverRecord> driversRecords;

        /// <summary>
        /// Используемый тип драйвера.
        /// </summary>
        public DriverType FactoryDriver { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу браузера.
        /// </summary>
        public string BrowserPath { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        private WebManager()
        {
            driversRecords = new List<WebDriverRecord>();
            _instance = this;
        }

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~WebManager()
        {
            // Закрытие драйверов
            CloseDrivers();
            // Надо дать время на взаимодействие с браузером, а то не успеет закрыть
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Создать если экземпляр не создан. Вернуть ссылку на экземпляр.
        /// </summary>
        /// <returns>Ссылка на экземпляр.</returns>
        public static WebManager GetInstance()
        {
            _instance = _instance ?? new WebManager();
            return _instance;
        }

        /// <summary>
        /// Получить свободный драйвер.
        /// </summary>
        /// <param name="owner">Будущий владелец драйвера.</param>>
        /// <returns>Готовый к работе веб-драйвер</returns>
        public virtual IWebDriver OccupyDriver(Object owner)
        {
            // Ищем первый драйвер без владельца
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.owner == null)
                {
                    // Записываем владельца и возвращаем драйвер
                    driverRec.owner = owner;
                    return driverRec.driver;
                }
            }

            // Создаем новый драйвер
            IWebDriver driver = CreateDriver();
            // Добавляем новую запись о драйвере
            WebDriverRecord drvRec = new WebDriverRecord(driver, owner);
            driversRecords.Add(drvRec);
            // Возвращаем драйвер
            return driver;
        }

        /// <summary>
        /// Освободить ранее полученный веб-драйвер.
        /// </summary>
        /// <param name="driver">Ранее полученный веб-драйвер.</param>
        public virtual void ReleaseDriver(IWebDriver driver)
        {
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.driver == driver)
                {
                    driverRec.owner = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Освобождение.
        /// </summary>
        public virtual void Dispose()
        {
            // Закрытие драйверов
            CloseDrivers();
            // Говорим не вызывать метод завершения объекта
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Для каждого драйвера: закрытие окна, освобождение и удаление из списка.
        /// </summary>
        protected virtual void CloseDrivers()
        {
            // Закрываем окна
            foreach (WebDriverRecord driverRec in driversRecords)
            {
                if (driverRec.driver is IWebDriver)
                {
                    driverRec.driver.Quit();
                    // Надо дать время на взаимодействие с браузером, а то не успеет закрыть
                    Thread.Sleep(1000);
                    driverRec.driver.Dispose();
                    driverRec.driver = null;
                }
            }
            driversRecords.RemoveAll(x => x.driver == null);
        }

        /// <summary>
        /// Создание нового веб-драйвера.
        /// </summary>
        /// <returns>Новый веб-драйвер.</returns>
        protected virtual IWebDriver CreateDriver()
        {
            IWebDriver driver;
            switch (FactoryDriver)
            {

                case DriverType.Firefox:
                    FirefoxOptions options = new FirefoxOptions();
                    if (BrowserPath != null)
                        options.BrowserExecutableLocation = BrowserPath;
                    driver = new FirefoxDriver(options);
                    driver.Manage().Window.Maximize();
                    break;

                case DriverType.PhantomJS:
                    var driverService = PhantomJSDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    driver = new PhantomJSDriver(driverService);
                    break;
                
                default:
                    throw new Exception("Неизвестный фабричный тип веб-драйвера.");

            }
            // Надеюсь этого не потребуется если будет адекватное решение с Fluent Wait
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            return driver;
        }

    }

}
