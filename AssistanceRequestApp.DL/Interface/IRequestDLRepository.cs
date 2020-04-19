namespace AssistanceRequestApp.DL.Interface
{
    using AssistanceRequestApp.Models.UserDefinedModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRequestDLRepository" />.
    /// </summary>
    public interface IRequestDLRepository
    {
        /// <summary>
        /// The CreateRequest.
        /// </summary>
        /// <param name="requestModel">The requestModel<see cref="CreateRequestModel"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        Task<string> CreateRequest(CreateRequestModel requestModel);

        /// <summary>
        /// The GetAllRequests.
        /// </summary>
        /// <returns>The <see cref="List{DetailRequestModel}"/>.</returns>
        List<DetailRequestModel> GetAllRequests();

        /// <summary>
        /// The DetailRequest.
        /// </summary>
        /// <param name="requestId">The requestId<see cref="int"/>.</param>
        /// <returns>The <see cref="DetailRequestModel"/>.</returns>
        DetailRequestModel DetailRequest(int requestId);

        /// <summary>
        /// The EditRequest.
        /// </summary>
        /// <param name="requestModel">The requestModel<see cref="EditRequestModel"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        Task<string> EditRequest(EditRequestModel requestModel);

        /// <summary>
        /// The DeleteRequest.
        /// </summary>
        /// <param name="requestId">The requestId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        Task<string> DeleteRequest(int requestId);
    }
}
