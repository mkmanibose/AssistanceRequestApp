namespace AssistanceRequestApp.Web.Controllers
{
    using AssistanceRequestApp.DL.Interface;
    using AssistanceRequestApp.Models.UserDefinedModels;
    using AssistanceRequestApp.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="HomeController" />.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Defines the requestDLRepository.
        /// </summary>
        private readonly IRequestDLRepository requestDLRepository;

        /// <summary>
        /// Defines the lstDetailRequestModels.
        /// </summary>
        private List<DetailRequestModel> lstDetailRequestModels = new List<DetailRequestModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{HomeController}"/>.</param>
        /// <param name="context">The context<see cref="IRequestDLRepository"/>.</param>
        public HomeController(ILogger<HomeController> logger, IRequestDLRepository context)
        {
            try
            {
                this.logger = logger;
                this.requestDLRepository = context;
                lstDetailRequestModels = requestDLRepository.GetAllRequests();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in HomeController " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index()
        {
            return View(lstDetailRequestModels);
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
        /// The RelatedEnvironmentPieChart.
        /// </summary>
        /// <returns>The <see cref="JsonResult"/>.</returns>
        [HttpGet]
        public JsonResult RelatedEnvironmentPieChart()
        {
            List<RelatedEnvironmentModel> lstRelatedEnvironmentModels = new List<RelatedEnvironmentModel>();
            try
            {
                lstDetailRequestModels = requestDLRepository.GetAllRequests();
                List<string> lstSearch = new List<string>();
                lstSearch = lstDetailRequestModels.Select(s => s.RelatedEnvironment.ToString()).Distinct().ToList();
                foreach (string item in lstSearch)
                {
                    lstRelatedEnvironmentModels.Add(new RelatedEnvironmentModel { Environment = item, NoCount = lstDetailRequestModels.Where(s => s.RelatedEnvironment.Equals(item)).Count() });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in RelatedEnvironmentPieChart " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return Json(lstRelatedEnvironmentModels);
        }

        /// <summary>
        /// The NatureofRequestColumnChart.
        /// </summary>
        /// <returns>The <see cref="JsonResult"/>.</returns>
        [HttpGet]
        public JsonResult NatureofRequestColumnChart()
        {
            List<NatureofRequestModel> lstNatureofRequestModel = new List<NatureofRequestModel>();
            try
            {
                lstDetailRequestModels = requestDLRepository.GetAllRequests();
                List<string> lstSearch = new List<string>();
                lstSearch = lstDetailRequestModels.Select(s => s.NatureofRequest.ToString()).Distinct().ToList();
                foreach (string item in lstSearch)
                {
                    lstNatureofRequestModel.Add(new NatureofRequestModel { NatureRequest = item, NoCount = lstDetailRequestModels.Where(s => s.NatureofRequest.Equals(item)).Count() });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in NatureofRequestColumnChart " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return Json(lstNatureofRequestModel);
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
