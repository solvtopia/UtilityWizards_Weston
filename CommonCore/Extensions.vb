Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Serialization
Imports Telerik.Web.UI
Imports System.Web

Public Module Extensions

    <Extension()> Public Function ToAge(ByVal dt As Date) As Integer
        Dim retVal As Integer = 0

        retVal = CInt(DateDiff(DateInterval.Year, dt, Now))
        If Now < New Date(Year(Now), Month(dt), Day(dt)) Then
            retVal -= 1
        End If

        Return retVal
    End Function

    <Extension()> Public Function FixPhoneNumber(ByVal s As String) As String
        Dim retVal As String = s

        retVal = retVal.Replace(".", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Trim

        Return retVal
    End Function

    <Extension()> Public Function UserPlatform(ByVal pg As System.Web.UI.Page) As Enums.UserPlatform
        Dim retVal As Enums.UserPlatform = Enums.UserPlatform.Unavailable

        If HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("iphone") Then
            retVal = Enums.UserPlatform.iPhone
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("ipod") Then
            retVal = Enums.UserPlatform.iPod
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("ipad") Then
            retVal = Enums.UserPlatform.iPad
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("android") And HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("mobile") Then
            retVal = Enums.UserPlatform.AndroidPhone
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("android") Then
            retVal = Enums.UserPlatform.AndroidTablet
        ElseIf HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").ToLower.Contains("winphone") Then
            retVal = Enums.UserPlatform.WindowsPhone
        Else retVal = Enums.UserPlatform.Desktop
        End If

        Return retVal
    End Function

    <Extension()> Public Function OnLocal(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        retVal = HttpContext.Current.Request.Url.ToString.ToLower.Contains("localhost")

        Return retVal
    End Function

    <Extension()> Public Function OnMobile(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidPhone, Enums.UserPlatform.AndroidTablet, Enums.UserPlatform.iPad, Enums.UserPlatform.iPhone, Enums.UserPlatform.iPod, Enums.UserPlatform.WindowsPhone
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Function OnPhone(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidPhone, Enums.UserPlatform.iPhone, Enums.UserPlatform.iPod, Enums.UserPlatform.WindowsPhone
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Function OnTablet(ByVal pg As System.Web.UI.Page) As Boolean
        Dim retVal As Boolean

        Select Case pg.UserPlatform
            Case Enums.UserPlatform.AndroidTablet, Enums.UserPlatform.iPad
                retVal = True
            Case Else
                retVal = False
        End Select

        Return retVal
    End Function

    <Extension()> Public Function GetUrlTitle(ByVal url As String) As String
        Dim retVal As String = ""

        Dim rq As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        Dim rs As System.Net.WebResponse = rq.GetResponse
        Dim sr As New System.IO.StreamReader(rs.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
        retVal = sr.ReadToEnd

        retVal = retVal.Substring(retVal.IndexOf("<title>") + 7)
        retVal = retVal.Substring(0, retVal.IndexOf("</title>"))

        Return retVal
    End Function

    <Extension()> Public Sub MsgBox(ByVal pg As System.Web.UI.Page, ByVal message As String, Optional ByVal scriptAfterAlert As String = "")
        Try
            message = Replace(message, "'", """")
            message = Replace(message, vbCrLf, "\r\n")
            message = Replace(message, vbLf, "\r\n")

            Dim myScript As String = "alert('" & message & "');" & scriptAfterAlert

            ' use the script manager to attach a new local script to popup a message box
            ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MsgBoxScript", myScript, True)

        Catch ex As Exception
            ex.WriteToErrorLog
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Sub OpenWindow(ByVal pg As System.Web.UI.Page, ByVal url As String, Optional ByVal height As Integer = 240, Optional ByVal width As Integer = 320, Optional ByVal scrollbars As Boolean = False, Optional ByVal addressbar As Boolean = False, Optional ByVal resizable As Boolean = False, Optional ByVal toolbar As Boolean = False, Optional ByVal status As Boolean = False, Optional ByVal directories As Boolean = False, Optional ByVal menubar As Boolean = False)
        Try
            Dim myScript, sScrollBars, sAddressBar, sResizable, sToolbar, sStatus, sDirectories, sMenuBar As String

            If scrollbars = True Then sScrollBars = "1" Else sScrollBars = "0"
            If addressbar = True Then sAddressBar = "1" Else sAddressBar = "0"
            If resizable = True Then sResizable = "1" Else sResizable = "0"
            If toolbar = True Then sToolbar = "1" Else sToolbar = "0"
            If status = True Then sStatus = "1" Else sStatus = "0"
            If directories = True Then sDirectories = "1" Else sDirectories = "0"
            If menubar = True Then sMenuBar = "1" Else sMenuBar = "0"

            'myScript = "window.open('" & url & "','_blank', 'height=" & height & ",width=" & width & ",scrollbars=" & sScrollBars & ",location=" & sAddressBar & ",resizable=1');"
            myScript = "var newwin = window.open('" & url & "', '_blank', 'scrollbars=" & sScrollBars & ", resizable=" & sResizable & ", width=" & width & ",height=" & height & ", location=" & sAddressBar & ", toolbar=" & sToolbar & ", status=" & sStatus & ",directories=" & sDirectories & ",menubar=" & sMenuBar & "'); if (newwin == null) { alert('\n Your web browser just prevented a pop-up window from opening.  Try holding down the control key and hit the button again.\n'); } else { newwin.focus(); }"

            ' use the script manager to attach a new local script to popup a message box
            ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MyScript", myScript, True)

        Catch ex As Exception
            ex.WriteToErrorLog
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Function FindInControl(ByVal ctrl As Control, ByVal id As String) As Control
        Dim retVal As Control = Nothing

        If Not ctrl Is Nothing Then
            For Each ctr As Control In ctrl.Controls
                Dim tmpID As String = ""
                If ctr.ID Is Nothing Then
                    Try
                        tmpID = ctr.ClientID
                    Catch ex As Exception
                        tmpID = ""
                    End Try
                Else : tmpID = ctr.ID
                End If

                If tmpID.ToLower = id.ToLower Then retVal = ctr : Exit For

                If ctr.HasControls Then
                    Dim tmp As Control = ctr.FindInControl(id)
                    If Not tmp Is Nothing Then retVal = tmp : Exit For
                Else
                End If
            Next
        End If

        Return retVal
    End Function

    <Extension()> Public Sub RunClientScript(ByVal pg As System.Web.UI.Page, ByVal script As String)
        Try
            script = Replace(script, "'", """")

            ' use the script manager to attach a new local script
            If pg IsNot Nothing Then
                ScriptManager.RegisterStartupScript(pg, pg.GetType(), "MyClientScript", script, True)
            End If

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    <Extension()> Public Function GetPostBackControl(ByVal pg As Page) As Control
        If Not pg.IsPostBack Then
            Return Nothing
        End If

        Dim control As Control = Nothing
        ' first we will check the "__EVENTTARGET" because if post back made by the controls
        ' which used "_doPostBack" function also available in Request.Form collection.
        Dim controlName As String = pg.Request.Params("__EVENTTARGET")
        If Not [String].IsNullOrEmpty(controlName) Then
            control = pg.FindControl(controlName)
        Else
            ' if __EVENTTARGET is null, the control is a button type and we need to
            ' iterate over the form collection to find it

            ' ReSharper disable TooWideLocalVariableScope
            Dim controlId As String
            Dim foundControl As Control
            ' ReSharper restore TooWideLocalVariableScope

            For Each ctl As String In pg.Request.Form
                ' handle ImageButton they having an additional "quasi-property" 
                ' in their Id which identifies mouse x and y coordinates
                If ctl.EndsWith(".x") OrElse ctl.EndsWith(".y") Then
                    controlId = ctl.Substring(0, ctl.Length - 2)
                    foundControl = pg.FindControl(controlId)
                Else
                    foundControl = pg.FindControl(ctl)
                End If

                If Not (TypeOf foundControl Is Button OrElse TypeOf foundControl Is ImageButton OrElse TypeOf foundControl Is LinkButton) Then
                    Continue For
                End If

                control = foundControl
                Exit For
            Next
        End If

        Return If(control Is Nothing, Nothing, control)
    End Function

    <Extension()> Public Function ToBoolean(ByVal value As Object) As Boolean
        Dim retVal As Boolean = False

        Try
            Select Case value.ToString.ToLower
                Case "a", "1", "y", "yes", "true", "t"
                    retVal = True
                Case "b", "0", "n", "no", "false", "f"
                    retVal = False
                Case Else
                    retVal = CBool(value)
            End Select
        Catch ex As Exception
            retVal = False
        End Try

        Return retVal
    End Function

    <Extension()> Public Function IsNullOrNothing(ByVal obj As Object, valueIfNull As Integer) As Integer
        Dim retVal As Integer = valueIfNull
        If IsDBNull(obj) OrElse obj Is Nothing Then retVal = valueIfNull Else retVal = obj.ToString.ToInteger
        Return retVal
    End Function
    <Extension()> Public Function IsNullOrNothing(ByVal obj As Object, valueIfNull As String) As String
        Dim retVal As String = valueIfNull
        If IsDBNull(obj) OrElse obj Is Nothing Then retVal = valueIfNull Else retVal = obj.ToString
        Return retVal
    End Function

    <Extension()> Public Function CheckString(ByVal obj As Object, ByVal ValueIfNothing As String) As String
        Return ToString(obj, ValueIfNothing)
    End Function

    <Extension()> Public Function ToString(ByVal obj As Object, ByVal ValueIfNothing As String) As String
        If obj Is Nothing Then
            obj = ValueIfNothing
        End If

        Return obj.ToString
    End Function

    <Extension()> Public Function ToDate(ByVal str As String, ByVal DefaultReturnValue As Date) As Date
        Try
            If IsDate(str) Then
                Return CDate(str)
            Else
                Return DefaultReturnValue
            End If

        Catch ex As Exception
            Return DefaultReturnValue
        End Try
    End Function

    <Extension()> Public Function ToDecimal(ByVal obj As Object) As Decimal
        Return obj.ToString.ToDecimal
    End Function
    <Extension()> Public Function ToDecimal(ByVal obj As String) As Decimal
        Return obj.ToDecimal(0)
    End Function
    <Extension()> Public Function ToDecimal(ByVal obj As Object, ByVal DefaultReturnValue As Decimal) As Decimal
        Return obj.ToString.ToDecimal(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToDecimal(ByVal str As String, ByVal DefaultReturnValue As Decimal) As Decimal
        Try
            'remove trailing "%"
            If str.EndsWith("%") Then
                str = str.TrimEnd(CChar("%"))
                str = CStr(str.ToDecimal / 100)
            End If

            Return CDec(str)

        Catch ex As Exception
            Return DefaultReturnValue
        End Try
    End Function

    <Extension()> Public Function ToDouble(ByVal obj As Object) As Decimal
        Return obj.ToString.ToDouble
    End Function
    <Extension()> Public Function ToDouble(ByVal str As String) As Decimal
        Return str.ToDouble(0)
    End Function
    <Extension()> Public Function ToDouble(ByVal obj As Object, ByVal DefaultReturnValue As Decimal) As Decimal
        Return obj.ToString.ToDouble(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToDouble(ByVal str As String, ByVal DefaultReturnValue As Decimal) As Decimal
        Try
            'remove trailing "%"
            If str.EndsWith("%") Then
                str = str.TrimEnd(CChar("%"))
                str = CStr(str.ToDecimal / 100)
            End If

            Return CDec(str)

        Catch ex As Exception
            Return DefaultReturnValue
        End Try
    End Function

    <Extension()> Public Function ToInteger(ByVal obj As Object) As Integer
        Return obj.ToString.ToInteger
    End Function
    <Extension()> Public Function ToInteger(ByVal str As String) As Integer
        Return str.ToInteger(0)
    End Function
    <Extension()> Public Function ToInteger(ByVal obj As Object, ByVal DefaultReturnValue As Integer) As Integer
        Return obj.ToString.ToInteger(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToInteger(ByVal str As String, ByVal DefaultReturnValue As Integer) As Integer
        Try
            Return CInt(str)
        Catch ex As Exception
            Return DefaultReturnValue
        End Try
    End Function

    <Extension()> Public Function ToLongInteger(ByVal obj As Object) As Long
        Return obj.ToString.ToLongInteger
    End Function
    <Extension()> Public Function ToLongInteger(ByVal str As String) As Long
        Return str.ToLongInteger(0)
    End Function
    <Extension()> Public Function ToLongInteger(ByVal obj As Object, ByVal DefaultReturnValue As Long) As Long
        Return obj.ToString.ToLongInteger(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToLongInteger(ByVal str As String, ByVal DefaultReturnValue As Long) As Long
        Try
            Return CLng(str)
        Catch ex As Exception
            Return DefaultReturnValue
        End Try
    End Function

    <Extension()> Public Function GetByID(ByVal mods As List(Of SystemModule), ByVal id As Integer) As SystemModule
        Dim retVal As New SystemModule

        Dim modFound As Boolean = False
        For Each m As SystemModule In mods
            If m.ID = id Then
                retVal = m
                modFound = True
                Exit For
            End If
        Next

        If Not modFound Then retVal = Nothing

        Return retVal
    End Function

    <Extension()> Function InList(ByVal items As RadComboBoxItemCollection, ByVal value As String) As Boolean
        Dim retVal As Boolean = False

        For Each item As RadComboBoxItem In items
            If item.Value.ToLower = value.ToLower Then
                retVal = True
                Exit For
            End If
        Next

        Return retVal
    End Function

    <Extension()> Function FindItem(ByVal items As List(Of NameValuePair), ByVal name As String) As NameValuePair
        Dim retVal As NameValuePair = New NameValuePair()

        For Each item As NameValuePair In items
            If item.Name.ToLower = name.ToLower Then
                retVal = item
                Exit For
            End If
        Next

        Return retVal
    End Function

    <Extension()> Function FindItemIndex(ByVal items As List(Of NameValuePair), ByVal name As String) As Integer
        Dim retval As Integer = -1

        For x As Integer = 0 To items.Count - 1
            Dim item As NameValuePair = items(x)
            If item.Name IsNot Nothing AndAlso item.Name.ToLower = name.ToLower Then
                retval = x
                Exit For
            End If
        Next

        Return retval
    End Function

    <Extension()> Public Function EvalCode(ByVal vbCode As String) As Object
        Dim codeDomProvider As CodeDomProvider = New VBCodeProvider

        ' add the references as parameters
        Dim cp As CompilerParameters = New CompilerParameters
        cp.ReferencedAssemblies.Add("system.dll")
        cp.ReferencedAssemblies.Add("system.xml.dll")
        cp.ReferencedAssemblies.Add("system.data.dll")
        cp.CompilerOptions = "/t:library"
        cp.GenerateInMemory = True

        ' setup the code
        Dim sb As StringBuilder = New StringBuilder("")
        sb.Append("Imports System" & vbCrLf)
        sb.Append("Imports System.Xml" & vbCrLf)
        sb.Append("Imports System.Data" & vbCrLf)
        sb.Append("Imports System.Data.SqlClient" & vbCrLf)
        sb.Append("Namespace NK5  " & vbCrLf)
        sb.Append("Class NK5Lib " & vbCrLf)
        sb.Append("public function  EvalCode() as Object " & vbCrLf)
        sb.Append(vbCode & vbCrLf)
        sb.Append("End Function " & vbCrLf)
        sb.Append("End Class " & vbCrLf)
        sb.Append("End Namespace" & vbCrLf)

        ' execute the code and capture the result
        Dim cr As CompilerResults = codeDomProvider.CompileAssemblyFromSource(cp, sb.ToString)
        Dim a As System.Reflection.Assembly = cr.CompiledAssembly
        Dim o As Object
        Dim mi As MethodInfo
        o = a.CreateInstance("NK5.NK5Lib")
        Dim t As Type = o.GetType()
        mi = t.GetMethod("EvalCode")
        Dim s As Object
        s = mi.Invoke(o, Nothing)

        ' return the result of the code
        Return s
    End Function

    <Extension()> Public Function EvalFormula(ByVal formula As String) As Double
        Dim retVal As Double = 0

        Dim dt As New DataTable
        retVal = CDbl(dt.Compute(formula, ""))

        Return retVal
    End Function

#Region "Object Extensions"

    <Extension()> Public Function SerializeToXml(ByVal o As Object) As String
        Dim sw As New StringWriter()
        Dim tw As XmlTextWriter = Nothing

        Try
            Dim serializer As New XmlSerializer(o.GetType)
            tw = New XmlTextWriter(sw)
            serializer.Serialize(tw, o)

        Catch ex As Exception
        Finally
            sw.Close()
            If tw IsNot Nothing Then
                tw.Close()
            End If
        End Try

        Return sw.ToString()
    End Function

    'Public Function DeserializeFromXML(ByVal xml As String, objectType As Type) As Object
    <Extension()> Public Function DeserializeFromXml(ByVal o As Object, ByVal xml As String) As Object
        Dim strReader As StringReader = Nothing
        Dim serializer As XmlSerializer = Nothing
        Dim xmlReader As XmlTextReader = Nothing
        Dim obj As Object = Nothing

        Try
            strReader = New StringReader(xml)
            serializer = New XmlSerializer(o.GetType)
            xmlReader = New XmlTextReader(strReader)
            obj = serializer.Deserialize(xmlReader)

        Catch exp As Exception
        Finally
            If xmlReader IsNot Nothing Then
                xmlReader.Close()
            End If
            If strReader IsNot Nothing Then
                strReader.Close()
            End If
        End Try

        Return obj
    End Function

#End Region

End Module
