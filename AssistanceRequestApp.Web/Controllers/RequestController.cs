namespace AssistanceRequestApp.Web.Controllers
{
    using AssistanceRequestApp.DL.Interface;
    using AssistanceRequestApp.Models.UserDefinedModels;
    using AssistanceRequestApp.Web.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RequestController" />.
    /// </summary>
    public class RequestController : Controller
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<RequestController> logger;

        /// <summary>
        /// Defines the requestDLRepository.
        /// </summary>
        private readonly IRequestDLRepository requestDLRepository;

        /// <summary>
        /// Defines the lstDetailRequestModels.
        /// </summary>
        private readonly List<DetailRequestModel> lstDetailRequestModels = new List<DetailRequestModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{RequestController}"/>.</param>
        /// <param name="context">The context<see cref="IRequestDLRepository"/>.</param>
        public RequestController(ILogger<RequestController> logger, IRequestDLRepository context)
        {
            try
            {
                this.logger = logger;
                this.requestDLRepository = context;
                lstDetailRequestModels = requestDLRepository.GetAllRequests();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in RequestController " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <param name="search">The search<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public IActionResult Index(string search)
        {
            try
            {
                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        return PartialView("_RequestIndexGrid", lstDetailRequestModels.Where(e =>
                        //e.Id.ToString().ToLower().Contains(search) ||
                        e.Name.ToString().ToLower().Contains(search) ||
                        e.EmployeeId.ToString().ToLower().Contains(search) ||
                        e.EmailId.ToString().ToLower().Contains(search) ||
                        e.ApplicationName.ToString().ToLower().Contains(search) ||
                        e.DescriptionofRequest.ToString().ToLower().Contains(search)
                        //e.AssignedTo.ToString().ToLower().Contains(search)
                        //e.Status.ToString().Contains(search)
                        ).ToList());
                    }
                    else
                        return PartialView("_RequestIndexGrid", lstDetailRequestModels);
                }
                return View(lstDetailRequestModels);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in Index " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return View(lstDetailRequestModels);
        }

        /// <summary>
        /// The GetDetailRequestModel.
        /// </summary>
        /// <param name="requestId">The requestId<see cref="int"/>.</param>
        /// <returns>The <see cref="DetailRequestModel"/>.</returns>
        private DetailRequestModel GetDetailRequestModel(int requestId)
        {
            DetailRequestModel detailRequestModel = new DetailRequestModel();
            try
            {
                detailRequestModel = requestDLRepository.DetailRequest(requestId);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in GetDetailRequestModel " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return detailRequestModel;
        }

        /// <summary>
        /// The CreateEditRequest.
        /// </summary>
        /// <param name="id">The id<see cref="int?"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public IActionResult CreateEditRequest(int? id)
        {
            EditRequestModel editRequestModel = new EditRequestModel();
            try
            {
                if (id.HasValue && id != 0)
                {
                    int requestId = Convert.ToInt32(id);
                    DetailRequestModel detailRequestModel = GetDetailRequestModel(requestId);

                    editRequestModel.Id = detailRequestModel.Id;
                    editRequestModel.CreatedDate = detailRequestModel.CreatedDate;
                    editRequestModel.ModifiedDate = detailRequestModel.ModifiedDate;
                    editRequestModel.IPAddress = Common.ClientIPAddressDetails.GetClientIP();
                    editRequestModel.Name = detailRequestModel.Name;
                    editRequestModel.EmployeeId = detailRequestModel.EmployeeId;
                    editRequestModel.EmailId = detailRequestModel.EmailId;
                    editRequestModel.Phone = detailRequestModel.Phone;
                    editRequestModel.ApplicationName = detailRequestModel.ApplicationName;
                    editRequestModel.NatureofRequest = detailRequestModel.NatureofRequest;
                    editRequestModel.RelatedEnvironment = detailRequestModel.RelatedEnvironment;
                    editRequestModel.DescriptionofRequest = detailRequestModel.DescriptionofRequest;
                    editRequestModel.AssignedTo = detailRequestModel.AssignedTo;
                    editRequestModel.Status = detailRequestModel.Status;
                    editRequestModel.DateAssigned = detailRequestModel.DateAssigned;
                    editRequestModel.EstimatedHours = detailRequestModel.EstimatedHours;
                    editRequestModel.DateCompleted = detailRequestModel.DateCompleted;
                    editRequestModel.ActualHours = detailRequestModel.ActualHours;
                    editRequestModel.ResolutionComments = detailRequestModel.ResolutionComments;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in CreateEditRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return View(editRequestModel);
        }

        /// <summary>
        /// The CreateEditRequest.
        /// </summary>
        /// <param name="editRequestModel">The editRequestModel<see cref="EditRequestModel"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditRequest(EditRequestModel editRequestModel)
        {
            string result = string.Empty;
            try
            {
                if (editRequestModel.Id == 0)
                {
                    ModelState.Remove("AssignedTo");
                    ModelState.Remove("Status");
                    ModelState.Remove("DateCompleted");
                    ModelState.Remove("ResolutionComments");
                }
                else
                {
                    if (!editRequestModel.Status.Equals("Closed"))
                    {
                        ModelState.Remove("DateCompleted");
                        ModelState.Remove("ResolutionComments");
                    }
                }
                if (ModelState.IsValid)
                {
                    if (editRequestModel.Id == 0)
                    {
                        CreateRequestModel addRequestModel = new CreateRequestModel { Name = editRequestModel.Name, EmployeeId = editRequestModel.EmployeeId, EmailId = editRequestModel.EmailId, Phone = editRequestModel.Phone, ApplicationName = editRequestModel.ApplicationName, NatureofRequest = editRequestModel.NatureofRequest, RelatedEnvironment = editRequestModel.RelatedEnvironment, DescriptionofRequest = "[" + DateTime.Now.ToString("MMMM-dd-yy") + "] " + editRequestModel.DescriptionofRequest, IPAddress = Common.ClientIPAddressDetails.GetClientIP(), Status = "Submitted" };
                        result = await requestDLRepository.CreateRequest(addRequestModel);
                        if (string.IsNullOrWhiteSpace(result))
                            return RedirectToAction("index");
                    }
                    else
                    {
                        result = await requestDLRepository.EditRequest(editRequestModel);
                        if (string.IsNullOrWhiteSpace(result))
                            return RedirectToAction("index");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in CreateEditRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return View(editRequestModel);
        }

        /// <summary>
        /// The DetailRequest.
        /// </summary>
        /// <param name="id">The id<see cref="int?"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult DetailRequest(int? id)
        {
            DetailRequestModel detailRequestModel = new DetailRequestModel();
            try
            {
                if (id.HasValue && id != 0)
                {
                    int requestId = Convert.ToInt32(id);
                    detailRequestModel = GetDetailRequestModel(requestId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in DetailRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return View(detailRequestModel);
        }

        /// <summary>
        /// The DeleteRequest.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult DeleteRequest(int id)
        {
            DetailRequestModel detailRequestModel = new DetailRequestModel();
            try
            {
                if (id != 0)
                {
                    int requestId = Convert.ToInt32(id);
                    detailRequestModel = GetDetailRequestModel(requestId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in DeleteRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return View(detailRequestModel);
        }

        /// <summary>
        /// The DeleteRequest.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <param name="collection">The collection<see cref="IFormCollection"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteRequest(int id, IFormCollection collection)
        {
            try
            {
                if (id != 0)
                {
                    string result = await requestDLRepository.DeleteRequest(id);
                    if (string.IsNullOrWhiteSpace(result))
                        return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in DeleteRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
                return View();
            }
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
