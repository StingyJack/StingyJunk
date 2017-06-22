namespace StingyJunk.Tfs
{
    public static class WiqlQueries
    {
        //https://www.visualstudio.com/en-us/docs/work/guidance/work-item-field

        public static string GetQueryForAnyInProgressWork()
        {
            //just getting the ID's because the query result is for workitem references and you have to 
            //go get the actual workitems anyway.
            return "SELECT [System.Id]"
                   //+ " [System.WorkItemType]" 
                   //+ " , [System.Title]" 
                   //+ " , [System.State]" 
                   //+ " , [System.AssignedTo]" 
                   //+ " , [System.IterationPath] " 
                   + " FROM WorkItems "
                   + "WHERE [System.State] IN ('New','Approved','Committed', 'InProgress', 'ToDo') "
                    + " AND [System.AssignedTo] != ''";
        }
        /*
            Select [System.WorkItemType]
                ,[System.Title]
                ,[System.State]
                ,[Microsoft.VSTS.Scheduling.Effort]
                ,[System.IterationPath] 
            FROM WorkItemLinks 
            WHERE Source.[System.WorkItemType] 
                    IN GROUP 'Microsoft.RequirementCategory' 
                AND Target.[System.WorkItemType] 
                    IN GROUP 'Microsoft.RequirementCategory' 
                AND Target.[System.State] 
                    IN ('New','Approved','Committed') 
                AND [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward' 
            ORDER BY [Microsoft.VSTS.Common.BacklogPriority] ASC
                ,[System.Id] ASC MODE (Recursive, ReturnMatchingChildren)"
         
         */
    }
}
