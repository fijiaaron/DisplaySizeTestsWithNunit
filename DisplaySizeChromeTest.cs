using NUnit.Framework;

namespace DisplaySizeTestsWithNunit
{
	using NUnit.Framework;
	using OpenQA.Selenium;
	using OpenQA.Selenium.Chrome;
	using System;

	public class DisplaySizeChromeTest
	{
		IWebDriver driver;

		[SetUp]
		public void Setup()
		{
			Log("starting driver");
			driver = GetDriverInstance();

			Log("opening url");
			driver.Url = "https://whatismyviewport.com/";
			Log(driver.Url);
		}

		[Test]
		public void CheckUnmodifiedDimensions()
		{
			(int viewportWidth, int viewportHeight) = getViewportDimensions(driver);
			(int screenWidth, int screenHeight) = getScreenDimensions(driver);

			Assert.That(viewportWidth, Is.GreaterThanOrEqualTo(1280));
			Assert.That(screenWidth, Is.GreaterThanOrEqualTo(1280));
			Assert.That(screenWidth, Is.GreaterThan(viewportWidth));
		}

		[Test]
		public void CheckMaximizedDimensions()
		{
			driver.Manage().Window.Maximize();

			(int viewportWidth, int viewportHeight) = getViewportDimensions(driver);
			(int screenWidth, int screenHeight) = getScreenDimensions(driver);

			Assert.That(viewportWidth, Is.GreaterThanOrEqualTo(1280));
			Assert.That(screenWidth, Is.GreaterThanOrEqualTo(1280));
			Assert.That(screenWidth, Is.EqualTo(viewportWidth));
		}


		private (int x, int y) getViewportDimensions(IWebDriver driver)
		{
			var js = (IJavaScriptExecutor)driver;

			var x = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));
			var y = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));

			Log("viewportWidth: " + x);
			Log("viewportHeight: " + y);

			return (x, y);
		}

		private (int x, int y) getScreenDimensions(IWebDriver driver)
		{
			var js = (IJavaScriptExecutor)driver;

			var x = Convert.ToInt32(js.ExecuteScript("return window.screen.width"));
			var y = Convert.ToInt32(js.ExecuteScript("return window.screen.height"));

			Log("screen width: " + x);
			Log("screen height: " + y);

			return (x, y);
		}

		[TearDown]
		public void TearDown()
		{
			Pause(2);
			driver.Quit();
		}

		private IWebDriver GetDriverInstance()
		{
			var options = new ChromeOptions();
			if (Environment.GetEnvironmentVariable("IS_TESTERY") == "true")
			{
				options.AddArguments(Environment.GetEnvironmentVariable("TESTERY_CHROME_ARGS").Split(';'));
			}

			IWebDriver driver = new ChromeDriver(options);
			Log(driver.GetType().Name);

			return driver;
		}

		private void Log(String message)
		{
			TestContext.Out.WriteLine(message);
		}

		private void Pause(float seconds)
		{
			int milliseconds = Convert.ToInt32(seconds * 1000);
			System.Threading.Thread.Sleep(milliseconds);
		}
	}
}