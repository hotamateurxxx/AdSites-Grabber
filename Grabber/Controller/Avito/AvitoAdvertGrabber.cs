using System;
using OpenQA.Selenium;
using AdSitesGrabber.Model;
using AdSitesGrabber.Model.Avito;

namespace AdSitesGrabber.Controller.Avito
{
    class AvitoAdvertGrabber : AdvertGrabber
    {

        public AvitoAdvertGrabber(IWebDriver driver) : 
            base(driver)
        {
        }

    }
}
