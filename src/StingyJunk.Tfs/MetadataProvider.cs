namespace StingyJunk.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public class MetadataProvider : TfsConnectedBase
    {
        /// <summary>
        ///     Creates an instance and builds the known elements collection immediately 
        /// </summary>
        /// <param name="baseUrl"></param>
        public MetadataProvider(string baseUrl)
        {
            RebuildTfsKnownElementsAsync(baseUrl).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Creates an instance by reading the local configuration file
        /// </summary>
        public MetadataProvider()
        {
            //not sure what this does. Don't think I want it in here. Would be nice id log4net picked up
            // the ambient logger already (maybe it does?)

            var settingsBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("TfsConfiguration.json", false, false)
                .AddJsonFile("TfsConfiguration.json.secret", true);

            var builtConfig = settingsBuilder.Build();

            LoadTfsConfiguration(builtConfig);
        }

        public async Task<List<WorkItemField>> GetFieldsAsync()
        {
            var knownProjectRefs = TfsKnownElements.KnownProjectReferences;
            if (knownProjectRefs == null)
            {
                throw new ArgumentException($"{nameof(knownProjectRefs)} returned a null result");
            }
            var firstKnownProjectRef = knownProjectRefs.FirstOrDefault();
            if (firstKnownProjectRef.Value == null)
            {
                throw new ArgumentException($"{nameof(firstKnownProjectRef)} returned no items");
            }
            var mostLikelyProject = TfsKnownElements.GetMostLikelyProject(firstKnownProjectRef.Value.Name);
            var witClient = GetWorkItemTrackingHttpClient(mostLikelyProject);
            return await witClient.GetFieldsAsync();
        }

        public async Task<Dictionary<string, List<WorkItemType>>> GetWorkItemTypesAsync()
        {
            var returnValue = new Dictionary<string, List<WorkItemType>>();

            foreach (var project in TfsKnownElements.KnownProjectReferences)
            {
                var collectionName = TfsKnownElements.KnownProjectToCollectionMap[project.Value.Name];
                var collection = TfsKnownElements.KnownProjectCollectionReferences[collectionName];
                var witClient = GetWorkItemTrackingHttpClient(new MostLikelyProjectInfo
                {
                    TeamProjectRef = project.Value,
                    TeamProjectCollectionRef = collection
                });
                var wits = await witClient.GetWorkItemTypesAsync(project.Value.Name);
                returnValue.Add(project.Value.Name, wits);
            }

            return returnValue;
        }
    }
}