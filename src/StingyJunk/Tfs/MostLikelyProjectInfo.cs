namespace StingyJunk.Tfs
{
    using Microsoft.TeamFoundation.Core.WebApi;

    public class MostLikelyProjectInfo
    {
        public TeamProjectReference TeamProjectRef { get; set; }
        public TeamProjectCollectionReference TeamProjectCollectionRef { get; set; }
    }
}