private class ColumnData
{

	public string ColumnName {get;set;}
	public string DataType {get;set;}
	public bool IsNullable {get;set;}

}
var tableNames = new [] {"", "", "", ""};
var tableNameInList = string.Join(",",tableNames.Select(s => $"'{s}'"));
var columnQuery = $"SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME IN ({tableNameInList}) ORDER BY ORDINAL_POSITION";
var tableAndColumnInfo = new Dictionary<string, List<ColumnData>>();
using (SqlConnection conn = new SqlConnection(connStr))
{
	conn.Open();
	var cmd = new SqlCommand(columnQuery, conn);
	var rdr = cmd.ExecuteReader();

	while (rdr.Read())
	{
		var tableName = Convert.ToString(rdr["TABLE_NAME"]);
		if (tableAndColumnInfo.ContainsKey(tableName) == false)
		{
			tableAndColumnInfo.Add(tableName, new List<ColumnData>());
		}
		var columnData = new ColumnData();
		columnData.ColumnName = Convert.ToString(rdr["COLUMN_NAME"]);
		columnData.DataType = Convert.ToString(rdr["DATA_TYPE"]);
		var isNullable = Convert.ToString(rdr["IS_NULLABLE"]);
		if (isNullable.Equals("YES", StringComparison.OrdinalIgnoreCase))
		{
			columnData.IsNullable = true;
		}
									
		tableAndColumnInfo[tableName].Add(columnData);
	} //end while

	rdr.Close();
}

var getProcOutput = new System.Text.StringBuilder();

foreach (var tableName in tableAndColumnInfo.Keys)
{
    getProcOutput.AppendLine("GO\n");
    getProcOutput.AppendLine($"\n\nCREATE PROCEDURE {tableName}Get");
    getProcOutput.AppendLine($"/*");
    getProcOutput.AppendLine($" cl");
    getProcOutput.AppendLine($" --------");
    getProcOutput.AppendLine($" {DateTime.Now.ToShortDateString()} {Environment.UserName} - generated");
    getProcOutput.AppendLine($"*/");
    getProcOutput.AppendLine($"AS\n\n");
    getProcOutput.AppendLine($"\tSELECT");

    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        getProcOutput.AppendLine($"\t\t{comma} [{pp.ColumnName}]");
    }

    getProcOutput.AppendLine($"\tFROM {tableName}");
}

var saveProcOutput = new System.Text.StringBuilder();

foreach (var tableName in tableAndColumnInfo.Keys)
{
    saveProcOutput.AppendLine("GO\n");
    saveProcOutput.AppendLine($"\n\nCREATE PROCEDURE {tableName}Save");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t{comma} @{pp.ColumnName}  {pp.DataType}");
    }
    saveProcOutput.AppendLine($"/*");
    saveProcOutput.AppendLine($" cl");
    saveProcOutput.AppendLine($" --------");
    saveProcOutput.AppendLine($" {DateTime.Now.ToShortDateString()} {Environment.UserName} - generated");
    saveProcOutput.AppendLine($"*/");
    saveProcOutput.AppendLine($"AS");
    saveProcOutput.AppendLine($"");
    saveProcOutput.AppendLine($"SET XACT_ABORT ON ");
    saveProcOutput.AppendLine($"BEGIN TRANSACTION \n");
    saveProcOutput.AppendLine($"\tBEGIN TRY\n");
    saveProcOutput.AppendLine($"\t\tMERGE {tableName} AS existing");
    saveProcOutput.AppendLine($"\t\tUSING (VALUES ( ");
    saveProcOutput.AppendLine($"\t\t\t");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t\t\t{comma} @{pp.ColumnName}");
    }
    saveProcOutput.AppendLine("\t\t)) AS new(");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t\t\t{comma} {pp.ColumnName}");
    }
    saveProcOutput.AppendLine("\t\t)");
    saveProcOutput.AppendLine("\t\tON existing.Id = new.Id");
    saveProcOutput.AppendLine("\t\tWHEN MATCHED THEN");
    saveProcOutput.AppendLine("\t\t\tUPDATE SET");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t\t\t{comma} existing.{pp.ColumnName} = new.{pp.ColumnName}");
    }
    saveProcOutput.AppendLine("\t\tWHEN NOT MATCHED THEN");

    saveProcOutput.AppendLine("\t\t\tINSERT (");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t\t\t\t{comma} {pp.ColumnName}");
    }
    saveProcOutput.AppendLine("\t\t\t )");
    saveProcOutput.AppendLine("\t\t\tVALUES (");
    for (var x = 0; x < tableAndColumnInfo[tableName].Count; x++)
    {
        var pp = tableAndColumnInfo[tableName][x];
        var comma = x == 0 ? "" : ",";
        saveProcOutput.AppendLine($"\t\t\t{comma} new.{pp.ColumnName}");
    }
    saveProcOutput.AppendLine("\t\t\t );");

    saveProcOutput.AppendLine($"END TRY");
    saveProcOutput.AppendLine($"BEGIN CATCH");
    saveProcOutput.AppendLine($"	IF (XACT_STATE() = -1)");
    saveProcOutput.AppendLine($"	BEGIN ");
    saveProcOutput.AppendLine($"		ROLLBACK TRANSACTION");
    saveProcOutput.AppendLine($"	END");
    saveProcOutput.AppendLine($"	EXECUTE [dbo].[CET_GetErrorInfoAndRaiseError]");
    saveProcOutput.AppendLine($"END CATCH");
    saveProcOutput.AppendLine($"");
    saveProcOutput.AppendLine($"IF (XACT_STATE() = 1)");
    saveProcOutput.AppendLine($"BEGIN ");
    saveProcOutput.AppendLine($"	COMMIT TRANSACTION");
    saveProcOutput.AppendLine($"END    ");

}