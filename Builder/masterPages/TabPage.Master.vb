Imports Telerik.Web.UI

Public Class TabPage
    Inherits builderMasterPage

#Region "Events"

    Public Event NewModuleClicked(ByVal folderId As Integer)
    Public Event FolderClicked(ByVal editId As Integer)

#End Region

#Region "Properties"

    Public ReadOnly Property DeviceId As String
        Get
            Return Request.QueryString("deviceid")
        End Get
    End Property
    Public ReadOnly Property UserId As Integer
        Get
            Return Request.QueryString("uid").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If

        If App.CurrentClient.Approved = Enums.SystemMode.Demo Then
            Dim daysToShow As Long = DateDiff(DateInterval.Day, App.CurrentClient.DemoStartDate, Now.Date)
            If daysToShow < 0 Then daysToShow = App.CurrentClient.DemoDuration
        End If
    End Sub
End Class