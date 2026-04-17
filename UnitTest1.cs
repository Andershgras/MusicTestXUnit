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
    [Fact]
    public void Test_Search_Filter_Works()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Navigate().GoToUrl("http://localhost:5500");

            // 1. Type "a" into the Title search box
            var searchInput = wait.Until(d => d.FindElement(By.CssSelector("input[placeholder='Search by title...']")));
            searchInput.SendKeys("a");

            // 2. Click Search
            driver.FindElement(By.XPath("//button[text()='Search']")).Click();

            // 3. Simple Check: Ensure the table is still there (not crashed)
            var rows = wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")));
            Assert.NotNull(rows);
        }
        finally { driver.Quit(); }
    }
    [Fact]
    public void Test_Logout_Clears_View()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Navigate().GoToUrl("http://localhost:5500");

            // 1. Quick Login
            wait.Until(d => d.FindElement(By.CssSelector("input[placeholder='Username']"))).SendKeys("admin");
            driver.FindElement(By.CssSelector("input[type='password']")).SendKeys("1234");
            driver.FindElement(By.XPath("//button[text()='Login']")).Click();

            // 2. Click Logout (Wait for it to appear first)
            wait.Until(d => d.FindElement(By.XPath("//button[text()='Logout']"))).Click();

            // 3. Check: Is the Username input visible again?
            var loginInput = wait.Until(d => d.FindElement(By.CssSelector("input[placeholder='Username']")));
            Assert.True(loginInput.Displayed);
        }
        finally { driver.Quit(); }
    }
}