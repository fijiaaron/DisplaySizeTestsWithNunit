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

		private void log(String message)
		{
			TestContext.Out.WriteLine(message);
		}

		[SetUp]
		public void Setup()
		{
			log("starting driver");

			var options = new ChromeOptions();
			if (Environment.GetEnvironmentVariable("IS_TESTERY") == "true")
			{
				options.AddArguments(Environment.GetEnvironmentVariable("TESTERY_CHROME_ARGS").Split(';'));
			}
			driver = new ChromeDriver(options);

			log(driver.GetType().Name);

			log("opening url");
			driver.Url = "https://whatismyviewport.com/";
			log(driver.Url);
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


		public (int x, int y) getViewportDimensions()
		{
			var js = (IJavaScriptExecutor)driver;

			var x = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));
			var y = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));

			log("viewportWidth: " + x);
			log("viewportHeight: " + y);

			return (x, y);
		}

		public (int w, int h) getScreenDimensions(IWebDriver driver)
		{
			var js = (IJavaScriptExecutor)driver;

			var w = Convert.ToInt32(js.ExecuteScript("return window.screen.width"));
			var h = Convert.ToInt32(js.ExecuteScript("return window.screen.height"));

			log("screen width: " + w);
			log("screen height: " + h);

			return (w, h);
		}

		public (int w, int h) getViewportDimensions(IWebDriver driver)
		{
			var js = (IJavaScriptExecutor)driver;

			var w = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));
			var h = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));

			log("viewport width: " + w);
			log("viewport height: " + h);

			return (w, h);
		}


		[TearDown]
		public void TearDown()
		{
			Pause(2);
			driver.Quit();
		}

		private void Pause(float seconds)
		{
			int milliseconds = Convert.ToInt32(seconds * 1000);
			System.Threading.Thread.Sleep(milliseconds);
		}
	}
}