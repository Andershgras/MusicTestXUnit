using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

public class UiTests
{
    [Fact]
    public void Test_Data_Loads_In_Table()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12));

        try
        {
            driver.Navigate().GoToUrl("http://localhost:5500");

            // Click Search button
            wait.Until(d => d.FindElement(By.XPath("//button[text()='Search']"))).Click();

            // Wait for table rows
            wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")).Count > 0);

            Assert.True(driver.FindElements(By.CssSelector("table tbody tr")).Count > 0);
        }
        finally
        {
            driver.Quit();
        }
    }

    [Fact]
    public void Test_Login_Saves_Token()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        try
        {
            driver.Navigate().GoToUrl("http://localhost:5500");

            ((IJavaScriptExecutor)driver).ExecuteScript("localStorage.clear();");
            driver.Navigate().Refresh();

            wait.Until(d => d.FindElement(By.CssSelector("input[placeholder='Username']"))).SendKeys("admin");
            driver.FindElement(By.CssSelector("input[type='password']")).SendKeys("1234");
            driver.FindElement(By.XPath("//button[text()='Login']")).Click();

            // Wait for success message
            wait.Until(d => d.FindElement(By.CssSelector("div.alert-success")));

            var token = ((IJavaScriptExecutor)driver).ExecuteScript("return localStorage.getItem('token');");
            Assert.NotNull(token);
            Assert.NotEmpty(token.ToString());
        }
        finally
        {
            driver.Quit();
        }
    }

    [Fact]
    public void Test_Records_Load_After_Login()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        try
        {
            driver.Navigate().GoToUrl("http://localhost:5500");

            ((IJavaScriptExecutor)driver).ExecuteScript("localStorage.clear();");
            driver.Navigate().Refresh();

            // Login
            wait.Until(d => d.FindElement(By.CssSelector("input[placeholder='Username']"))).SendKeys("admin");
            driver.FindElement(By.CssSelector("input[type='password']")).SendKeys("1234");
            driver.FindElement(By.XPath("//button[text()='Login']")).Click();

            // Login() auto-calls getRecords(), just wait for rows
            wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")).Count > 0);

            Assert.True(driver.FindElements(By.CssSelector("table tbody tr")).Count > 0);
        }
        finally
        {
            driver.Quit();
        }
    }
}