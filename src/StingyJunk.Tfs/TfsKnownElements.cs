namespace StingyJunk.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.TeamFoundation.Core.WebApi;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    ///     The collection of known tfs team collections, projects, and other cached info
    /// </summary>
    public class TfsKnownElements
    {
        /// <summary>
        ///     Gets the base URL.
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        ///     Gets or sets the known project collection references.
        /// </summary>
        public Dictionary<string, TeamProjectCollectionReference> KnownProjectCollectionReferences { get; }

        /// <summary>
        ///     Gets or sets the known project collections.
        /// </summary>
        public Dictionary<string, TeamProjectCollection> KnownProjectCollections { get; }

        /// <summary>
        ///     Gets or sets the known project references.
        /// </summary>
        public Dictionary<string, TeamProjectReference> KnownProjectReferences { get; }

        /// <summary>
        ///     Gets or sets the known project to collection map.
        /// </summary>
        public Dictionary<string, string> KnownProjectToCollectionMap { get; }

        /// <summary>
        ///     Gets the is all present.
        /// </summary>
        public bool IsAllPresent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BaseUrl) == false)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TfsKnownElements"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public TfsKnownElements(string baseUrl)
        {
            BaseUrl = baseUrl;
            KnownProjectCollectionReferences = new Dictionary<string, TeamProjectCollectionReference>(StringComparer.OrdinalIgnoreCase);
            KnownProjectCollections = new Dictionary<string, TeamProjectCollection>(StringComparer.OrdinalIgnoreCase);
            KnownProjectReferences = new Dictionary<string, TeamProjectReference>(StringComparer.OrdinalIgnoreCase);
            KnownProjectToCollectionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Clears the known properties.
        /// </summary>
        public void ClearKnownProperties()
        {
            KnownProjectCollectionReferences.Clear();
            KnownProjectReferences.Clear();
            KnownProjectToCollectionMap.Clear();
        }

        /// <summary>
        /// Builds the working collection URL.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns></returns>
        public string BuildWorkingCollectionUrl(string collectionName)
        {
            var collection = KnownProjectCollections[collectionName];
            var refLink = collection.Links.Links["web"] as ReferenceLink;
            Debug.Assert(refLink != null, "refLink != null");
            return refLink.Href;
        }


        /// <summary>
        /// Gets the most likely project.
        /// </summary>
        /// <param name="projectHint">The project hint.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not locate a most likely project</exception>
        public MostLikelyProjectInfo GetMostLikelyProject(string projectHint)
        {
            TeamProjectReference locatedProjectReference = null;

            foreach (var project in KnownProjectReferences)
            {
                if (project.Value.Name.Equals(projectHint, StringComparison.OrdinalIgnoreCase)
                    || project.Value.Description?.IndexOf(projectHint, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    locatedProjectReference = project.Value;
                    break;
                }
            }

            if (locatedProjectReference == null)
            {
                throw new InvalidOperationException("Could not locate a most likely project");
            }

            var collectionName = KnownProjectToCollectionMap[locatedProjectReference.Name];
            var locatedCollectionReference = KnownProjectCollectionReferences[collectionName];
            return new MostLikelyProjectInfo
            {
                TeamProjectCollectionRef = locatedCollectionReference,
                TeamProjectRef = locatedProjectReference
            };
        }
    }
}