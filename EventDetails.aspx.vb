Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Web.Services
Imports GlobalProcedureFunction
Imports Newtonsoft.Json

Partial Class EventDetails
    Inherits System.Web.UI.Page
    Public Shared con As String = ConfigurationManager.ConnectionStrings("con").ConnectionString
    Public Shared Property JsonConvert As Object
    Public Shared Property GlobalProcedureFunction As Object

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

    End Sub

    <WebMethod()>
    Shared Function loadExpo(ByVal code As String) As String
        Dim JSONString As String = String.Empty
        Dim cmd = "select *, " &
                    "Day(fromdate) As 'FDay',LEFT(DATENAME(month, fromdate), 3) AS 'FMonth',year(fromdate) as 'FYear', " &
                     "right(convert(varchar(32),fromdate,100),8) as 'fromTime', " &
                    "right(convert(varchar(32),todate,100),8) as 'toTime', " &
                    "Day(todate) As 'TDay',LEFT(DATENAME(month, todate), 3) AS 'TMonth',year(todate) as 'TYear', " &
                    "(case when todate < DATEADD(HOUR, 8, CONVERT(varchar(20),GETUTCDATE(),120))  then 'Expired' else 'Valid' end) as 'Validity' " &
                    "From tb_expo  where expocode = @code"

        Dim sqlcom As New SqlCommand(cmd)
        sqlcom.Parameters.AddWithValue("@code", code)
        'MsgBox(ViewCommand(sqlcom))
        Dim n As DataTable = SelectParamDBDT(sqlcom, con)
        JSONString = JsonConvert.SerializeObject(n)
        Return JSONString
    End Function

    Private Shared Function SelectParamDBDT(sqlcom As SqlCommand, con As String) As DataTable
        Throw New NotImplementedException()
    End Function

    <WebMethod()>
    Shared Function loadEvent(ByVal code As String) As String
        Dim JSONString As String = String.Empty
        Dim cmd = "select *,FORMAT(fromdate, 'MMM d h:mm tt') AS fromdate_,FORMAT(todate, 'MMM d h:mm tt') AS todate_ from tb_event where expocode = @code and not DeleteInd = 'x' order by fromdate"

        Dim sqlcom As New SqlCommand(cmd)
        sqlcom.Parameters.AddWithValue("@code", code)

        Dim n As DataTable = SelectParamDBDT(sqlcom, con)
        JSONString = JsonConvert.SerializeObject(n)
        Return JSONString
    End Function
    <WebMethod()>
    Shared Function register(ByVal business_type As String,
                            eventcode As String,
                            nameval As String,
                            websiteval As String,
                            emailval As String,
                            phoneval As String
   ) As String

        Dim result As String = String.Empty

        Dim cmd As String = ""

        cmd = "insert into tb_event_customer (BusinessType,Eventcode,CompanyName,email, Website,Phoneno) " &
            "values(@BusinessType,@Eventcode,@CompanyName,@email, @Website,@Phoneno)"

        Dim sqlcom As New SqlCommand(cmd)
        sqlcom.Parameters.AddWithValue("@BusinessType", business_type)
        sqlcom.Parameters.AddWithValue("@Eventcode", eventcode)
        sqlcom.Parameters.AddWithValue("@CompanyName", nameval)
        sqlcom.Parameters.AddWithValue("@Website", websiteval)
        sqlcom.Parameters.AddWithValue("@email", emailval)
        sqlcom.Parameters.AddWithValue("@Phoneno", phoneval)


        result = GlobalProcedureFunction.InsertUpdateParam(sqlcom, con)

        If result = "success" Then
            sendEmailToMerchant(emailval, nameval, eventcode)
        End If

        Return result
    End Function
    Shared Function sendEmailToMerchant(ByVal merchantEmail As String, memberName As String, expoid As String) As String
        Dim result As String

        Dim cmd = "select  *,case when FORMAT(CAST(fromdate AS DATETIME), 'dd MMM yyyy') = FORMAT(CAST(todate AS DATETIME), 'dd MMM yyyy') then " &
                    "'on ' + FORMAT(CAST(fromdate AS DATETIME), 'dd MMM yyyy') else " &
                    "'from ' + FORMAT(CAST(fromdate AS DATETIME), 'dd MMM yyyy') + ' to ' + FORMAT(CAST(todate AS DATETIME), 'dd MMM yyyy') end as 'expodt'," &
                    " CASE WHEN CAST(fromdate AS DATETIME) = CAST(todate AS DATETIME) THEN  " &
                     "'at ' + FORMAT(CAST(fromdate AS DATETIME), 'hh:mm tt') ELSE  " &
                     "'from ' + FORMAT(CAST(fromdate AS DATETIME), 'hh:mm tt') + ' to ' + FORMAT(CAST(todate AS DATETIME), 'hh:mm tt') END AS 'expotm'" &
                    " from tb_expo where expocode = @expoid"

        Dim sqlcom As New SqlCommand(cmd)

        sqlcom.Parameters.AddWithValue("@expoid", expoid)

        Dim dt As DataTable = SelectParamDBDT(sqlcom, con)

        If dt.Rows.Count > 0 Then

            Try
                Dim mail As New System.Net.Mail.MailMessage()

                mail.To.Add(merchantEmail)
                mail.From = New MailAddress("admin@ezyshare.online", "EzyShare")

                mail.Subject = "Confirmation: Registration for EVENT " & dt.Rows(0)("exponame")

                'MsgBox(pdf)


                mail.SubjectEncoding = Encoding.UTF8
                mail.Body = ""
                mail.BodyEncoding = Encoding.UTF8
                mail.IsBodyHtml = True
                mail.Priority = MailPriority.Normal '<div class="col-md-12 col-sm-12 col-xs-12 col-sm-12 col-xs-12">

                Dim html = "<p>Dear " & memberName & ",</p>" &
                    "<p>Congratulations! Your registration for the EVENT " & dt.Rows(0)("exponame") & " has been successfully processed. We look forward to welcoming you " & dt.Rows(0)("expodt") & " at " & dt.Rows(0)("venue") & ".</p>" &
                "<p></p>" &
                "<p>Event Details:</p>" &
                "<p>-Date :  " & dt.Rows(0)("expodt") & "</p>" &
                "<p>-Time :  " & dt.Rows(0)("expotm") & "</p>" &
                "<p>-Venue: " & dt.Rows(0)("venue") & ".</p>" &
                "<p></p>" &
                "<p>Please arrive on time to ensure a smooth and productive experience. Keep an eye on your email for any updates or specific instructions leading up to the event.</p>" &
                "<p></p>" &
                "<p>Thank you for registering for the EVENT " & dt.Rows(0)("exponame") & ". We are excited to have you join us and look forward to seeing you there!</p>" &
                "<p></p>" &
                "<p>Best regards,</p>" &
                    "<p></p>" &
                     "<p>Admin</p>" &
                     "<p>016-2323737</p>" &
                     "<p>admin@ezyshare.online</p>" &
                     "<p>NSI SOLUTIONS SDN BHD (1290872-K)</p>" &
                     "<p>Suite 22.06-22.10 & 22.15-22.18,</p>" &
                     "<p>Level 22, Wisma Zelan, No. 1,</p>" &
                     "<p>Jalan Tasik Permaisuri 2, Bandar Sri Permaisuri,</p>" &
                     "<p>56000 Cheras, Kuala Lumpur, Malaysia.</p>" &
                       "<p></p>" &
                    "<p> * This is a system generated email, do not reply to this email.</p>"

                Dim emailcontent = html

                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(emailcontent)
                htmlView.ContentType = New System.Net.Mime.ContentType("text/html")
                mail.AlternateViews.Add(htmlView)

                Dim client As New SmtpClient()
                client.UseDefaultCredentials = False
                client.Credentials = New System.Net.NetworkCredential("merchantcare@ezyshare.online", "157#Hge!89~P")
                client.Port = 587
                client.Host = "mail.ezyshare.online"
                client.EnableSsl = False

                client.Send(mail)
                result = "success"

            Catch ex As Exception
                result = ex.Message
            End Try
        End If




        Return result
    End Function
    <WebMethod()>
    Shared Function getPostExpoImg(ByVal expocode As String) As String

        Dim JSONString As String = String.Empty
        Dim cmd As String

        cmd = "select * from tb_postexpo a ,tb_expo b where a.expocode = b.expocode  and a.expocode = @expocode and publish = '1' "


        Dim sqlcom As New SqlCommand(cmd)
        sqlcom.Parameters.AddWithValue("@expocode", expocode)


        'MsgBox(ViewCommand(sqlcom))

        Dim n As DataTable = GlobalProcedureFunction.SelectParamDBDT(sqlcom, con)

        JSONString = JsonConvert.SerializeObject(n)

        Return JSONString
    End Function
End Class
