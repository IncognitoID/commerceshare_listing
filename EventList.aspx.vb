

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports GlobalProcedureFunction
Imports Newtonsoft.Json

Partial Class EventList
    Inherits System.Web.UI.Page
    Public Shared con As String = ConfigurationManager.ConnectionStrings("con").ConnectionString
    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

    End Sub

    <WebMethod()>
    Shared Function loadExpo(ByVal code As String, viewmore As String) As String
        Dim JSONString As String = String.Empty

        Dim cmd = ""
        If viewmore = "yes" Then
            cmd = "select *, " &
                    "Day(fromdate) As 'FDay',LEFT(DATENAME(month, fromdate), 3) AS 'FMonth',year(fromdate) as 'FYear', " &
                    "Day(todate) As 'TDay',LEFT(DATENAME(month, todate), 3) AS 'TMonth',year(todate) as 'TYear', " &
                    "right(convert(varchar(32),fromdate,100),8) as 'fromTime', " &
                    "right(convert(varchar(32),todate,100),8) as 'toTime', " &
                    "(case when todate < DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120))  then 'Expired' else 'Valid' end) as 'Validity'   " &
                    "From tb_expo  where not deleteind = 'x' order by fromdate desc"
        Else
            cmd = "select top 10 *, " &
                    "Day(fromdate) As 'FDay',LEFT(DATENAME(month, fromdate), 3) AS 'FMonth',year(fromdate) as 'FYear', " &
                    "Day(todate) As 'TDay',LEFT(DATENAME(month, todate), 3) AS 'TMonth',year(todate) as 'TYear', " &
                    "right(convert(varchar(32),fromdate,100),8) as 'fromTime', " &
                    "right(convert(varchar(32),todate,100),8) as 'toTime', " &
                    "(case when todate < DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120))  then 'Expired' else 'Valid' end) as 'Validity'   " &
                    "From tb_expo  where not deleteind = 'x' order by fromdate desc"
        End If


        'MsgBox(cmd)
        Dim sqlcom As New SqlCommand(cmd)
        'sqlcom.Parameters.AddWithValue("@code", code)

        Dim n As DataTable = SelectParamDBDT(sqlcom, con)
        JSONString = JsonConvert.SerializeObject(n)
        Return JSONString
    End Function



End Class
