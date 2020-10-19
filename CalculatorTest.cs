using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

//Nuget Package: Microsoft.WinAppDriver.Appium.WebDriver
//Version: 1.0.1-Preview (This is a prerelease version of Microsoft.WinAppDriver.Appium.WebDriver.)
//Description: This is a temporary Selenium WebDriver extension for Appium that implements Actions API functionality for Windows Application Driver (WinAppDriver).
//             This is for preview purposes only.
//Dependencies:
//          1.Castle.Core.4.2.1
//          2.Newtonsoft.Json.10.0.3
//          3.Selenium.WebDriver.3.8.0
//          4.Selenium.Support.3.8.0
//Note: Do not upgrade the above dependencies

namespace WinAppDriverDemo
{
    [TestClass]
    public class CalculatorTest : CalculatorSession
    {
        private static WindowsElement calculatorResult;
        private static WindowsElement header;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session to launch a Calculator window
            Setup(context);

            // Identify calculator mode by locating the header
            try
            {
                header = session.FindElementByAccessibilityId("Header");
            }
            catch
            {
                header = session.FindElementByAccessibilityId("ContentPresenter");
            }

            // Locate the calculatorResult element
            calculatorResult = session.FindElementByAccessibilityId("CalculatorResults");

            Assert.IsNotNull(calculatorResult);
        }

        [TestInitialize]
        public void Clear()
        {
            session.FindElementByName("Clear").Click();
            Assert.AreEqual("0", GetCalculatorResultText());
        }

        [TestCleanup]
        public void ClearEntry()
        {
            session.FindElementByAccessibilityId("clearEntryButton").Click();
            Assert.AreEqual("0", GetCalculatorResultText());
        }

        [TestMethod]
        public void ProgrammerCalculatorTestMethod()
        {
            // Ensure that calculator is in standard mode
            if (!header.Text.Equals("Programmer", StringComparison.OrdinalIgnoreCase))
            {
                session.FindElementByAccessibilityId("TogglePaneButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Programmer Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(header.Text.Equals("Programmer", StringComparison.OrdinalIgnoreCase));
            }

            // Find the buttons by their accessibility ids using XPath and click them
            // NOTE: Do not use them for RadioButton
            session.FindElementByAccessibilityId("hexButton").Click();
            session.FindElementByName("A").Click();
            session.FindElementByName("Plus").Click();
            session.FindElementByName("One").Click();
            session.FindElementByName("Four").Click();
            session.FindElementByName("Equals").Click();
            session.FindElementByAccessibilityId("decimalButton").Click();

            Assert.AreEqual("30", GetCalculatorResultText());
        }

        [TestMethod]
        public void ScientificCalculatorTestMethod()
        {
            // Ensure that calculator is in standard mode
            if (!header.Text.Equals("Scientific", StringComparison.OrdinalIgnoreCase))
            {
                session.FindElementByAccessibilityId("TogglePaneButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Scientific Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(header.Text.Equals("Scientific", StringComparison.OrdinalIgnoreCase));
            }

            // To calculate value of Pi, Find the buttons by their names using XPath and click them.
            session.FindElementByXPath("//Button[@Name='Pi']").Click();

            Decimal expectedResult = Math.Round(Convert.ToDecimal(Math.PI), 9);
            Decimal actualResult = Math.Round(Convert.ToDecimal(GetCalculatorResultText()), 9);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void StandardCalculatorTestMethod()
        {
            // Ensure that calculator is in standard mode
            if (!header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                session.FindElementByAccessibilityId("TogglePaneButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Standard Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase));
            }

            // To calculate value of Pi, Find the buttons by their accessibility ids and click them.
            session.FindElementByAccessibilityId("num2Button").Click();
            session.FindElementByAccessibilityId("num2Button").Click();
            session.FindElementByAccessibilityId("divideButton").Click();
            session.FindElementByAccessibilityId("num7Button").Click();
            session.FindElementByAccessibilityId("equalButton").Click();

            Decimal expectedResult = Math.Round(Convert.ToDecimal(Math.PI), 2);
            Decimal actualResult = Math.Round(Convert.ToDecimal(GetCalculatorResultText()), 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private string GetCalculatorResultText()
        {
            return calculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }
    }
}