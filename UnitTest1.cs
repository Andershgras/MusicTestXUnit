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

        driver.Navigate().GoToUrl("http://localhost:5500");

        // Wait until table rows appear (max 10 sec)
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // 1. Find button
        var button = wait.Until(d => d.FindElement(By.TagName("button")));

        // 2. Click button
        button.Click();


        wait.Until(d => d.FindElements(By.TagName("tr")).Count > 0);

        var rows = driver.FindElements(By.TagName("tr"));

        Assert.True(rows.Count > 0);

        driver.Quit();
    }
}