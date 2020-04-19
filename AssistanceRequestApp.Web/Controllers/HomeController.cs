namespace AssistanceRequestApp.Web.Controllers
{
    using AssistanceRequestApp.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="HomeController" />.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{HomeController}"/>.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The Privacy.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
