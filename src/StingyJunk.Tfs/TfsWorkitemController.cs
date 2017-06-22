namespace StingyJunk.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Config;
    using Microsoft.Extensions.Configuration;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    //https://www.visualstudio.com/en-us/docs/integrate/get-started/client-libraries/samples
    /// <summary>
    ///     Provides ways to operate with Team Foundation Server workitems
    /// </summary>
    public class TfsWorkitemController : TfsConnectedBase
    {
        #region "props and fields"

        #endregion //#region "props and fields"

        #region "public interface members"

        /// <summary>
        ///     Initializes a new instance of the <see cref="TfsWorkitemController"/> class.
        /// </summary>
        /// <param name="configRoot">The configuration root.</param>
        public TfsWorkitemController(IConfigurationRoot configRoot)
        {
            LoadTfsConfiguration(configRoot);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TfsWorkitemController"/> class.
        /// </summary>
        /// <param name="systemInstances">The TFS configuration.</param>
        public TfsWorkitemController(SystemInstances systemInstances)
        {
            LoadTfsConfiguration(systemInstances);
        }

        #endregion //#region "public interface members"

        #region "workitem handling"

        /// <summary>
        /// Gets the single workitem detail by identifier asynchronous.
        /// </summary>
        /// <param name="workItemId">The work item identifier.</param>
        /// <param name="projectHint">The project hint.</param>
        /// <returns></returns>
        public async Task<WorkItem> GetSingleWorkitemDetailByIdAsync(int workItemId, string projectHint)
        {
            //assumes workitem id follows the workitem token.

            var mostLikelyTeamProject = TfsKnownElements.GetMostLikelyProject(projectHint);
            var witClient = GetWorkItemTrackingHttpClient(mostLikelyTeamProject);
            var workitem = await witClient.GetWorkItemAsync(workItemId);
            return workitem;
        }

        /// <summary>
        /// Gets the workitem details by wiql query asynchronous.
        /// </summary>
        /// <param name="wiql">The wiql.</param>
        /// <param name="projectHint">The project hint.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Learn to query, scrub!</exception>
        public async Task<List<WorkItem>> GetWorkitemDetailsByWiqlQueryAsync(string wiql, string projectHint)
        {
            if (string.IsNullOrWhiteSpace(wiql))
            {
                throw new ArgumentException("Learn to query, scrub!");
            }

            var mostLikelyTeamProject = TfsKnownElements.GetMostLikelyProject(projectHint);
            var witClient = GetWorkItemTrackingHttpClient(mostLikelyTeamProject);

            var resultOfOnlyIdValues = await witClient.QueryByWiqlAsync(new Wiql {Query = wiql}, mostLikelyTeamProject.TeamProjectRef.Name);
            if (resultOfOnlyIdValues.WorkItems.Any() == false)
            {
                return new List<WorkItem>();
            }

            var workitems = await witClient.GetWorkItemsAsync(resultOfOnlyIdValues.WorkItems.Select(w => w.Id));

            return workitems;
        }


        /*
                private async Task<WorkItem> CreateWorkItem(string requestor, string requestedWork, string customerName,
                    string channelHint)
                {
                    var mostLikelyTeamProject = TfsKnownElements.GetMostLikelyProject(channelHint);
        
                    var connection = new VssConnection(new Uri(TfsKnownElements.BaseUrl), new VssCredentials());
                    var witClient = connection.GetClient<WorkItemTrackingHttpClient>();
                    var document = new JsonPatchDocument
                                   {
                                       new JsonPatchOperation
                                       {
                                           From = "Title",
                                           Path = "/fields/System.Title",
                                           Operation = Operation.Add
                                       }
                                   };
        
                    var type = "Product Backlog Item";
                    return await witClient.CreateWorkItemAsync(document, mostLikelyTeamProject.TeamProjectRef.Name, type);
                }
                */

        #endregion //#region "workitem handling"

        #region "utils"

        #endregion //#region "utils"
    }
}