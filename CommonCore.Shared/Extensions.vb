﻿Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
Imports Telerik.Web.UI

Public Module Extensions

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

        Try
            retVal = retVal.Replace(".", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Trim

        Catch ex As Exception
        End Try

        Return retVal
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

    <Extension()> Public Function ToDouble(ByVal obj As Object) As Double
        Return obj.ToString.ToDouble
    End Function
    <Extension()> Public Function ToDouble(ByVal str As String) As Double
        Dim DefaultReturnValue As Double = If(IsNumeric(str), CDbl(str), 0)
        Return str.ToDouble(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToDouble(ByVal obj As Object, ByVal DefaultReturnValue As Double) As Double
        Return obj.ToString.ToDouble(DefaultReturnValue)
    End Function
    <Extension()> Public Function ToDouble(ByVal str As String, ByVal DefaultReturnValue As Double) As Double
        Try
            'remove trailing "%"
            If str.EndsWith("%") Then
                str = str.TrimEnd(CChar("%"))
                str = CStr(str.ToDouble / 100)
            End If

            Return CDbl(str)

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

        Try
            formula = formula.ToUpper().Replace("IIF(", "IF(").Replace("IF(", "IIF(")

            Using dt As DataTable = New DataTable()
                retVal = dt.Compute(formula, Nothing).ToString.ToDouble
            End Using

        Catch ex As Exception
            retVal = 0
        End Try

        Return retVal
    End Function

    <Extension()> Public Function TestFormula(ByVal formula As String) As String
        Dim retVal As String = "Success"

        Try
            formula = formula.ToUpper().Replace("IIF(", "IF(").Replace("IF(", "IIF(")

            Using dt As DataTable = New DataTable()
                dt.Compute(formula, Nothing)
            End Using

        Catch ex As Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function

    <Extension()> Public Function GetFieldNamesFromFormula(ByVal formula As String) As List(Of String)
        Return GetFieldNamesFromFormula(formula, New List(Of String))
    End Function
    <Extension()> Public Function GetFieldNamesFromFormula(ByVal formula As String, ByVal fieldList As List(Of String)) As List(Of String)
        Dim retVal As New List(Of String)

        Dim fieldName As String = ""
        Dim fieldStarted As Boolean = False
        For Each c As Char In formula
            If c = "[" Then fieldStarted = True
            If fieldStarted Then fieldName &= c
            If c = "]" Then
                Dim fieldToAdd As String = fieldName.Replace("[", "").Replace("]", "")
                If Not retVal.Contains(fieldToAdd) And Not fieldList.Contains(fieldToAdd) Then retVal.Add(fieldToAdd)
                fieldName = ""
                fieldStarted = False
            End If
        Next

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
