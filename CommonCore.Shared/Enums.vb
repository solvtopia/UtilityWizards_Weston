Public Class Enums
    Public Enum ProjectName
        API
        Builder
        _811Processor
        MobileApp
        CommonCore
        CommonCoreShared
        FileProcessor
    End Enum

    Public Enum FileSort
        Name
        Size
    End Enum
    Public Enum ScrapeType
        ReturnAll
        KeepTags
        RemoveAll
    End Enum
    Public Enum DBName
        ' common databases
        UtilityWizards
        UtilityWizards_Sandbox

        ' project specific databases
    End Enum

    Public Enum SystemModuleStatus
        Open = 0
        Pending = 1
        Completed = 2
    End Enum

    Public Enum SystemModuleType
        Folder
        [Module]
    End Enum

    Public Enum SystemModulePriority
        Normal
        Emergency
        High
    End Enum

    Public Enum TransactionType
        Unavailable
        [New]
        Existing
    End Enum

    Public Enum SystemQuestionType
        CheckBox
        DropDownList
        TextBox
        MemoField
        NumericTextBox
        CurrencyTextBox
    End Enum

    Public Enum SystemQuestionTextBoxSize
        XSmall = 50
        Small = 100
        Medium = 200
        Large = 300
    End Enum

    Public Enum SystemQuestionDropDownSize
        Auto = 0
        Small = 100
        Medium = 200
        Large = 300
    End Enum

    Public Enum SystemQuestionBindingType
        UserInput = 0
        MasterFeed = 1
        Formula = 2
    End Enum

    Public Enum SystemQuestionLocation
        TopLeft
        TopMiddle
        TopRight
        FullPage
        BottomLeft
        BottomMiddle
        BottomRight
    End Enum

    Public Enum SystemUserPermissions
        User
        Administrator
        SystemAdministrator
        Solvtopia
        Technician
        Supervisor
    End Enum

    Public Enum IconSize
        Small
        Large
    End Enum

    Public Enum SystemMode
        Live
        Demo
    End Enum

    Public Enum LogEntry
        QueryRecord
        AddRecord
        EditRecord
        DeleteRecord

        UserLogin
        ChatSession

        UnDeleteRecord
    End Enum

    Public Enum CreditCardType
        Visa
        MasterCard
        Discover
        Amex
        DinersClub
        enRoute
        JCB
        Invalid
    End Enum

    Public Enum ValidationErrorType
        Warning
        [Error]
    End Enum

    Public Enum UserPlatform
        Unavailable = 100

        iPhone = 0
        iPod = 1
        iPad = 2

        AndroidPhone = 3
        AndroidTablet = 4

        WindowsPhone = 5

        Desktop = 6

        Api = 7
    End Enum

    Public Enum NotificationType
        Custom

        NormalWorkOrderCreated
        EmergencyWorkOrderCreated
        WorkOrderAssigned
    End Enum

    Public Enum InformationPopupButtons
        YesNo
        OkCancel
        OkOnly
    End Enum

    Public Enum InformationPopupType
        DeleteFolder
        DeleteModule
        ErrorMessage
        InformationOnly
        MoveModule
        DeleteRecord
        DeleteUser
        DeleteReport
    End Enum

    Public Enum InformationPopupIcon
        [Error]
        Information
        Question
    End Enum

#Region "Control Enums"

    Public Enum NumericFormat
        Currency
        Custom
        Number
        Percent
    End Enum

    Public Enum CasingOption
        Normal
        UPPER
        lower
    End Enum

    Public Enum SaveFormat
        Bmp
        Emf
        Exif
        Gif
        Icon
        Jpeg
        MemoryBmp
        Png
        Tiff
        Wmf
    End Enum

    Public Enum GradientDirection
        Vertical
        Horizontal
        ForwardDiagonal
        BackwardDiagonal
    End Enum

    Public Enum GradientColors
        Two
        Three
    End Enum

#End Region

#Region "API Enums"

    Public Enum ApiResultCode
        success = 0
        failed = 1
    End Enum

#End Region

End Class
