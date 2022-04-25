using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocoaDI
{
    public class BotService : IBotService
    {
        private readonly IOptions<MyConfig> _config;
        private IWebDriver _driver;

        public BotService(IOptions<MyConfig> config)
        {
            _config = config;

            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");

            _driver = new ChromeDriver(_config.Value.CaminhoChromeDriver, chromeOptions);
        }
        protected void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(5);
            _driver.Navigate().GoToUrl(
                _config.Value.UrlTangerino);
        }

        protected async Task ClickColaborador()
        {
            IWebElement searchBtn = _driver.FindElement(By.XPath(@"/html/body/div[1]/div[3]/fieldset/form/ul/li[2]"));
            Actions actionProvider = new Actions(_driver);
            // Perform click-and-hold action on the element
            actionProvider.Click(searchBtn).Build().Perform();
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        public void BaterPonto()
        {
            try
            {
                CarregarPagina();
                ClickColaborador().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _driver.Dispose();
        }

    }
}
