using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

using (IWebDriver driver = new ChromeDriver())
{
    var p = 1;
    var _url = "https://www.kinopoisk.ru/lists/movies/top250/?page=";
    var url = _url + p;

    driver.Navigate().GoToUrl(url);

    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

    try
    {
        wait.Until(ExpectedConditions.ElementExists(By.CssSelector("main")));
        bool perehod = true;
        string filePath = "movies.txt";
        List<string> movieInfoList = new List<string>();
        do {
            Console.WriteLine("start");
            var titleNode = driver.FindElements(By.CssSelector(".styles_root__ti07r"));
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".styles_root__ti07r")));
           
            int count = 0;

            if (titleNode != null)
            {
                //Thread.Sleep(5000);
                
                foreach (var el in titleNode)
                {
                    var title = el.FindElement(By.CssSelector(".styles_mainTitle__IFQyZ.styles_activeMovieTittle__kJdJj")).Text;

                    var title_eng_el = el.FindElements(By.CssSelector(".desktop-list-main-info_secondaryTitle__ighTt"));
                    string title_eng = string.Join(", ", title_eng_el.Select(element => element.Text));

                    var year_dur = el.FindElement(By.CssSelector(".desktop-list-main-info_secondaryText__M_aus")).Text;

                    var rate = el.FindElement(By.XPath(".//span[@class='styles_kinopoiskValuePositive__vOb2E styles_kinopoiskValue__9qXjg styles_top250Type__mPloU']")).Text;

                    string movieInfo = $"Топ {movieInfoList.Count + 1} : {title}, {title_eng}{year_dur}, рейтинг: {rate}";

                    movieInfoList.Add(movieInfo);
                    count++;
                }
                Console.Clear();

                foreach (var el in movieInfoList)
                {
                    Console.WriteLine(el);

                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var el in movieInfoList)
                    {
                        writer.WriteLine(el);
                    }
                }
                Console.WriteLine("\n");
                Console.WriteLine("Movies titles saved to : " + filePath);
                Console.WriteLine("Total movies: " + movieInfoList.Count + "\n\n");

                try
                {
                    var nextPageBtn = driver.FindElement(By.XPath("//a[contains(@class,'styles_end__aEsmB styles_start__UvE6T')]"));
                    var hasNextPage = nextPageBtn.Enabled;
                    if (hasNextPage == true)
                    {
                        p++;
                        url = _url + p;
                        Thread.Sleep(2000);
                        //nextPageBtn.Click();
                        driver.Navigate().GoToUrl(url);
                    }
                }
                catch (NoSuchElementException)
                {
                    perehod = false;
                }
            }
            else
            {
                Console.WriteLine("Title not found.");
            }      
        }
        while (perehod == true);
    }

    catch (WebDriverTimeoutException)
    {
        Console.Clear();
        Console.WriteLine("filmi ne zagruzhautsya((");
    }

}
