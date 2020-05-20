namespace AssistanceRequestApp.DL
{
    using AssistanceRequestApp.DL.Context;
    using AssistanceRequestApp.DL.Interface;
    using AssistanceRequestApp.Models.DBModels;
    using AssistanceRequestApp.Models.UserDefinedModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RequestDLRepository" />.
    /// </summary>
    public class RequestDLRepository : IRequestDLRepository
    {
        /// <summary>
        /// Defines the context.
        /// </summary>
        private readonly AssistanceRequestAppDBContext context;

        /// <summary>
        /// Defines the requestEntity.
        /// </summary>
        private readonly DbSet<Request> requestEntity;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<RequestDLRepository> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestDLRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="AssistanceRequestAppDBContext"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{RequestDLRepository}"/>.</param>
        public RequestDLRepository(AssistanceRequestAppDBContext context, ILogger<RequestDLRepository> logger)
        {
            this.context = context;
            requestEntity = context.Set<Request>();
            this.logger = logger;
        }

        /// <summary>
        /// The CreateRequest.
        /// </summary>
        /// <param name="requestModel">The requestModel<see cref="CreateRequestModel"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> CreateRequest(CreateRequestModel requestModel)
        {
            string result = string.Empty;
            try
            {
                Request requestDb = new Request { Name = requestModel.Name, EmployeeId = requestModel.EmployeeId, EmailId = requestModel.EmailId, Phone = requestModel.Phone, ApplicationName = requestModel.ApplicationName, NatureofRequest = requestModel.NatureofRequest, RelatedEnvironment = requestModel.RelatedEnvironment, DescriptionofRequest = requestModel.DescriptionofRequest, CreatedDate = DateTime.Now, IPAddress = requestModel.IPAddress, Status = requestModel.Status, IsActive = true };
                context.Requests.Add(requestDb);
                //context.Entry(requestModel).State = EntityState.Added;
                await context.SaveChangesAsync();
                result = Convert.ToString(requestDb.Id);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in CreateRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// The GetAllRequests.
        /// </summary>
        /// <returns>The <see cref="List{DetailRequestModel}"/>.</returns>
        public List<DetailRequestModel> GetAllRequests()
        {
            List<DetailRequestModel> lstDetailRequestModel = new List<DetailRequestModel>();
            try
            {
                lstDetailRequestModel = (from requestModel in context.Requests
                                         where requestModel.IsActive.Equals(true)
                                         orderby requestModel.Id descending
                                         select new DetailRequestModel
                                         {
                                             Id = requestModel.Id,
                                             CreatedDate = requestModel.CreatedDate,
                                             EmailId = requestModel.EmailId,
                                             ModifiedDate = requestModel.ModifiedDate,
                                             IPAddress = requestModel.IPAddress,
                                             Name = requestModel.Name,
                                             EmployeeId = requestModel.EmployeeId,
                                             Phone = requestModel.Phone,
                                             ApplicationName = requestModel.ApplicationName,
                                             NatureofRequest = requestModel.NatureofRequest,
                                             RelatedEnvironment = requestModel.RelatedEnvironment,
                                             DescriptionofRequest = requestModel.DescriptionofRequest,
                                             AssignedTo = requestModel.AssignedTo,
                                             Status = requestModel.Status,
                                             DateAssigned = requestModel.DateAssigned,
                                             EstimatedHours = requestModel.EstimatedHours,
                                             DateCompleted = requestModel.DateCompleted,
                                             ActualHours = requestModel.ActualHours,
                                             ResolutionComments = requestModel.ResolutionComments,
                                             IsActive = requestModel.IsActive
                                         }).ToList();
                return lstDetailRequestModel;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in GetAllRequests " + ex.Message);
                logger.LogError(ex.StackTrace);
                return new List<DetailRequestModel>();
            }
        }

        /// <summary>
        /// The DetailRequest.
        /// </summary>
        /// <param name="requestId">The requestId<see cref="int"/>.</param>
        /// <returns>The <see cref="DetailRequestModel"/>.</returns>
        public DetailRequestModel DetailRequest(int requestId)
        {
            DetailRequestModel detailRequestModel = new DetailRequestModel();
            try
            {
                detailRequestModel = (from requestModel in context.Requests
                                      where requestModel.Id.Equals(requestId) &&
                                      requestModel.IsActive.Equals(true)
                                      orderby requestModel.Id descending
                                      select new DetailRequestModel
                                      {
                                          Id = requestModel.Id,
                                          CreatedDate = requestModel.CreatedDate,
                                          ModifiedDate = requestModel.ModifiedDate,
                                          IPAddress = requestModel.IPAddress,
                                          Name = requestModel.Name,
                                          EmployeeId = requestModel.EmployeeId,
                                          EmailId = requestModel.EmailId,
                                          Phone = requestModel.Phone,
                                          ApplicationName = requestModel.ApplicationName,
                                          NatureofRequest = requestModel.NatureofRequest,
                                          RelatedEnvironment = requestModel.RelatedEnvironment,
                                          DescriptionofRequest = requestModel.DescriptionofRequest,
                                          AssignedTo = requestModel.AssignedTo,
                                          Status = requestModel.Status,
                                          DateAssigned = requestModel.DateAssigned,
                                          EstimatedHours = requestModel.EstimatedHours,
                                          DateCompleted = requestModel.DateCompleted,
                                          ActualHours = requestModel.ActualHours,
                                          ResolutionComments = requestModel.ResolutionComments,
                                          IsActive = requestModel.IsActive
                                      }).FirstOrDefault();
                return detailRequestModel;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in DetailRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return new DetailRequestModel();
        }

        /// <summary>
        /// The EditRequest.
        /// </summary>
        /// <param name="requestModel">The requestModel<see cref="EditRequestModel"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> EditRequest(EditRequestModel requestModel)
        {
            string result = string.Empty;
            try
            {
                Request requestDb = context.Requests.Find(requestModel.Id);
                requestDb.Id = requestModel.Id;
                //requestDb.CreatedDate = requestModel.CreatedDate;
                requestDb.ModifiedDate = DateTime.Now;
                requestDb.IPAddress = requestModel.IPAddress;
                requestDb.Name = requestModel.Name;
                requestDb.EmployeeId = requestModel.EmployeeId;
                requestDb.EmailId = requestModel.EmailId;
                requestDb.Phone = requestModel.Phone;
                requestDb.ApplicationName = requestModel.ApplicationName;
                requestDb.NatureofRequest = requestModel.NatureofRequest;
                requestDb.RelatedEnvironment = requestModel.RelatedEnvironment;
                requestDb.DescriptionofRequest = requestModel.DescriptionofRequest;
                requestDb.AssignedTo = requestModel.AssignedTo;
                requestDb.Status = requestModel.Status;
                requestDb.DateAssigned = requestModel.DateAssigned;
                requestDb.EstimatedHours = requestModel.EstimatedHours;
                requestDb.DateCompleted = requestModel.DateCompleted;
                requestDb.ActualHours = requestModel.ActualHours;
                requestDb.ResolutionComments = requestModel.ResolutionComments;
                context.Requests.Update(requestDb);
                await context.SaveChangesAsync();
                if (requestDb.Id < 1)
                    result = "Error in CreateRequest";
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in EditRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// The DeleteRequest.
        /// </summary>
        /// <param name="requestId">The requestId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> DeleteRequest(int requestId)
        {
            string result = string.Empty;
            try
            {
                Request requestDb = context.Requests.Find(requestId);
                requestDb.IsActive = false;
                context.Requests.Update(requestDb);
                //context.Requests.Remove(requestDb);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in DeleteRequest " + ex.Message);
                logger.LogError(ex.StackTrace);
                result = ex.Message;
            }
            return result;
        }
    }
}
