namespace StingyJunk.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bases;
    using Config;
    using Microsoft.Extensions.Configuration;
    using Microsoft.TeamFoundation.Core.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.Common;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    ///     Provides base Tfs Connection and cached config info
    /// </summary>
    /// <seealso cref="LoggableBase" />
    public abstract class TfsConnectedBase : LoggableBase
    {
        #region "props and fields"

        /// <summary>
        ///     Gets or sets the TFS known elements.
        /// </summary>
        /// <value>
        /// The TFS known elements.
        /// </value>
        protected TfsKnownElements TfsKnownElements { get; set; }

        #endregion //#region "props and fields"

        /// <summary>
        /// Loads the TFS configuration.
        /// </summary>
        /// <param name="configRoot">The configuration root.</param>
        /// <exception cref="System.ArgumentException"></exception>
        protected void LoadTfsConfiguration(IConfigurationRoot configRoot)
        {
            var tfsConfig = new SystemInstances();
            configRoot.Bind(tfsConfig);
            LoadTfsConfiguration(tfsConfig);
        }

        protected void LoadTfsConfiguration(SystemInstances tfsConfig)
        {
            if (tfsConfig.IsValid == false)
            {
                throw new ArgumentException($"Tfs configuration is not valid: {tfsConfig.Instances?.Count} instances");
            }

            var onlySupportsOneTfsInstance = tfsConfig.Instances.First();
            var baseUrl = onlySupportsOneTfsInstance.InstanceUrl;

            if (onlySupportsOneTfsInstance.AutoLoadInstanceMetaData)
            {
                LogInfo("Auto loading Tfs instance metadata...");
                RebuildTfsKnownElementsAsync(baseUrl).GetAwaiter().GetResult();
                LogInfo("Auto load of Tfs instance metadata complete");
            }
            else
            {
                LogInfo("Auto load of Tfs instance metadata was skipped");
                TfsKnownElements = new TfsKnownElements(baseUrl);
            }
        }

        #region "Connection members"

        /// <summary>
        /// Gets the server connection.
        /// </summary>
        /// <returns></returns>
        protected virtual VssConnection GetServerConnection()
        {
            var serverConnection = new VssConnection(new Uri(TfsKnownElements.BaseUrl), new VssCredentials());
            return serverConnection;
        }

        /// <summary>
        /// Gets the team project collections asynchronous.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TeamProjectCollectionReference>> GetTeamProjectCollectionsAsync()
        {
            var serverConnection = GetServerConnection();
            var collectionClient = serverConnection.GetClient<ProjectCollectionHttpClient>();
            return await collectionClient.GetProjectCollections();
        }

        /// <summary>
        /// Gets the team project collection asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        protected virtual async Task<TeamProjectCollection> GetTeamProjectCollectionAsync(string id)
        {
            var serverConnection = GetServerConnection();
            var collectionClient = serverConnection.GetClient<ProjectCollectionHttpClient>();
            return await collectionClient.GetProjectCollection(id);
        }

        /// <summary>
        /// Gets the project collection connection.
        /// </summary>
        /// <param name="collectionUrl">The collection URL.</param>
        /// <returns></returns>
        protected virtual VssConnection GetProjectCollectionConnection(string collectionUrl)
        {
            var serverConnection = new VssConnection(new Uri(collectionUrl), new VssCredentials());
            return serverConnection;
        }

        /// <summary>
        /// Gets the team projects asynchronous.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TeamProjectReference>> GetTeamProjectsAsync(string collectionName)
        {
            var collectionUrl = TfsKnownElements.BuildWorkingCollectionUrl(collectionName);
            var serverConnection = GetProjectCollectionConnection(collectionUrl);
            var projectClient = serverConnection.GetClient<ProjectHttpClient>();
            return await projectClient.GetProjects(ProjectState.All, 100, 0);
        }

        /// <summary>
        /// Gets the workitem fields.
        /// </summary>
        /// <param name="mostLikelyProjectInfo">The most likely project information.</param>
        /// <returns></returns>
        protected virtual async Task<List<WorkItemField>> GetWorkitemFields(MostLikelyProjectInfo mostLikelyProjectInfo)
        {
            var witClient = GetWorkItemTrackingHttpClient(mostLikelyProjectInfo);
            return await witClient.GetFieldsAsync();
        }

        #endregion //#region "Connection members"

        /// <summary>
        /// Rebuilds the TFS known elements asynchronous.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns></returns>
        protected async Task<bool> RebuildTfsKnownElementsAsync(string baseUrl)
        {
            if (TfsKnownElements == null)
            {
                TfsKnownElements = new TfsKnownElements(baseUrl);
            }

            TfsKnownElements.ClearKnownProperties();

            var projectCollections = await GetTeamProjectCollectionsAsync();

            foreach (var projectCollectionReference in projectCollections)
            {
                var projectCollection = await GetTeamProjectCollectionAsync(projectCollectionReference.Id.ToString());
                TfsKnownElements.KnownProjectCollectionReferences.Add(projectCollectionReference.Name, projectCollectionReference);
                TfsKnownElements.KnownProjectCollections.Add(projectCollection.Name, projectCollection);

                foreach (var projectReference in await GetTeamProjectsAsync(projectCollectionReference.Name))
                {
                    TfsKnownElements.KnownProjectReferences.Add(projectReference.Name, projectReference);
                    TfsKnownElements.KnownProjectToCollectionMap.Add(projectReference.Name, projectCollectionReference.Name);
                }
            }

            return true;
        }


        #region "client members"

        /// <summary>
        /// Gets the work item tracking HTTP client.
        /// </summary>
        /// <param name="mostLikelyConnectionInfo">The most likely connection information.</param>
        /// <returns></returns>
        protected virtual WorkItemTrackingHttpClient GetWorkItemTrackingHttpClient(MostLikelyProjectInfo mostLikelyConnectionInfo)
        {
            var collectionUrl = TfsKnownElements.BuildWorkingCollectionUrl(mostLikelyConnectionInfo.TeamProjectCollectionRef.Name);
            var connection = GetProjectCollectionConnection(collectionUrl);

            var witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            return witClient;
        }

        /// <summary>
        /// Gets the work item tracking HTTP client.
        /// </summary>
        /// <param name="projectHint">The project hint.</param>
        /// <returns></returns>
        protected virtual WorkItemTrackingHttpClient GetWorkItemTrackingHttpClient(string projectHint)
        {
            var mostLikelyTeamProject = TfsKnownElements.GetMostLikelyProject(projectHint);
            return GetWorkItemTrackingHttpClient(mostLikelyTeamProject);
        }

        #endregion //#region "client members"
    }
}