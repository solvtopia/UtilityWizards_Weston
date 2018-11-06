Imports Amazon.S3
Imports Amazon.S3.Model
Imports Amazon.Runtime
Imports Amazon
Imports Amazon.S3.Util
Imports System.Collections.ObjectModel
Imports System.IO

Public Class AWSHelper
    Const AWS_ACCESS_KEY As String = "AKIA5BE3HIKCUD2Y6UFY"
    Const AWS_SECRET_KEY As String = "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"
    Const AWS_BUCKET_NAME As String = "wpg-file-upload"

    Public Structure AwsFileInfo
        Sub New(ByVal s3object As S3Object, ByVal folder As String)
            If Not folder = "/" Then
                If s3object.Key.ToString.StartsWith(folder) Then
                    Dim replacementstring As String = Replace(s3object.Key, folder, "")
                    If Not replacementstring = "" Then
                        Me.Name = replacementstring
                    End If
                End If
            Else Me.Name = s3object.Key.Replace(folder, "")
            End If

            Me.FullName = s3object.Key
            Me.LastModified = s3object.LastModified
            Me.Size = s3object.Size
            Me.Directory = folder

            Dim ext() As String = Me.Name.Split("."c)
            Me.Extension = ext(ext.Length - 1)

            Dim s3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.USEast2)

            Me.PutUrlExpires = Now.AddDays(7)
            Dim request As GetPreSignedUrlRequest
            request = New GetPreSignedUrlRequest With {
                .BucketName = AWS_BUCKET_NAME,
                .Key = Me.FullName,
                .Verb = HttpVerb.PUT,
                .Expires = Me.PutUrlExpires
            }
            Me.PutUrl = s3Client.GetPreSignedURL(request)

            Me.GetUrlExpires = Now.AddDays(7)
            request = New GetPreSignedUrlRequest With {
                .BucketName = AWS_BUCKET_NAME,
                .Key = Me.FullName,
                .Verb = HttpVerb.GET,
                .Expires = Me.GetUrlExpires
            }
            Me.GetUrl = s3Client.GetPreSignedURL(request)
        End Sub

        Public Name As String
        Public FullName As String
        Public Directory As String
        Public LastModified As Date
        Public Size As Long
        Public Extension As String
        Public PutUrl As String
        Public PutUrlExpires As Date
        Public GetUrl As String
        Public GetUrlExpires As Date
    End Structure

    Private Property s3Client As IAmazonS3

    'Usage for This Class
    '
    '       CreateABucket("<bucketname>") returns: empty string if successful/ error message if failed
    '         Bucket name should conform with DNS requirements: 
    '         - Should not contain uppercase characters
    '         - Should not contain underscores (_)
    '         - Should be between 3 and 63 characters long
    '         - Should not end with a dash
    '         - Cannot contain two, adjacent periods
    '         - Cannot contain dashes next to periods (e.g., "my-.bucket.com" and "my.-bucket" are invalid)
    '       CreateFolder("<bucketname>", "<foldername>")  returns: empty string if successful/ error message if failed
    '         Follow same rules as above
    '       AddFileToFolder("<filename>", "<bucketname>", "<foldername>") returns: empty string if successful/ error message if failed
    '       ListingFiles("<bucketname>", "<foldername>") return observable collection(of String) - empty collection if errors
    '       DeleteFileFromFolder("<bucketname>", "<foldername>, <filename>) returns: empty string if successful/ error message if failed
    '       GetFileFromFolder("<bucketname>", "<foldername>", <filename>, <target>) returns: empty string if successful/ error message if failed

    Sub New()
        Try
            s3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.USEast2)

        Catch ex As Exception

        End Try
    End Sub

    '''<summary>
    '''       CreateABucket("<bucketname>") returns: empty string if successful/ error message if failed
    '''         Bucket name should conform with DNS requirements: 
    '''         - Should not contain uppercase characters
    '''         - Should not contain underscores (_)
    '''         - Should be between 3 and 63 characters long
    '''         - Should not end with a dash
    '''         - Cannot contain two, adjacent periods
    '''         - Cannot contain dashes next to periods (e.g., "my-.bucket.com" and "my.-bucket" are invalid)
    ''' </summary>
    Public Function CreateABucket() As String  'parameter : client As IAmazonS3
        Dim retVal As String = ""
        Try
            Try
                Dim putRequest1 As PutBucketRequest = New PutBucketRequest() With {.BucketName = AWS_BUCKET_NAME, .UseClientRegion = True}
                Dim response1 As PutBucketResponse = s3Client.PutBucket(putRequest1)

            Catch amazonS3Exception As AmazonS3Exception
                If amazonS3Exception.ErrorCode = "BucketAlreadyOwnedByYou" Then
                    retVal = "Bucket already exists"
                Else
                    If (Not IsNothing(amazonS3Exception.ErrorCode) And amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")) Or amazonS3Exception.ErrorCode.Equals("InvalidSecurity") Then
                        retVal = "Check the provided AWS Credentials."
                    Else
                        retVal = String.Format("Error occurred. Message:'{0}' when writing an object", amazonS3Exception.Message)
                    End If
                End If
            End Try

        Catch ex As Exception
            retVal = ""
        End Try

        Return retVal
    End Function

    Public Function CreateFolder(folderName() As String) As String
        Dim retVal As String = ""
        Try
            Try
                Dim folderKey As String = ""
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, AWS_BUCKET_NAME) Then
                    retVal = "Bucket does not exist"
                Else
                    For i = 0 To folderName.Length - 1
                        folderKey += folderName(i) & "/"

                    Next
                    ' folderKey = folderKey & "/"    'end the folder name with "/"
                    Dim request As PutObjectRequest = New PutObjectRequest()
                    request.BucketName = AWS_BUCKET_NAME
                    request.StorageClass = S3StorageClass.Standard
                    request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.None
                    ' request.CannedACL = S3CannedACL.BucketOwnerFullControl
                    request.Key = folderKey
                    request.ContentBody = String.Empty
                    s3Client.PutObject(request)
                End If

            Catch ex As Exception
                retVal = ex.Message
            End Try

        Catch ex As AmazonS3Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function

    Public Function AddFileToFolder(FileName As String, folderName As String) As String
        Dim retVal As String = ""
        Try
            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, AWS_BUCKET_NAME) Then
                    Dim fname() As String = folderName.Split("/")
                    CreateFolder(fname)
                Else
                    Dim path As String = FileName
                    Dim file As FileInfo = New FileInfo(path)

                    Dim key As String = String.Format("{0}/{1}", folderName, file.Name)
                    Dim por As PutObjectRequest = New PutObjectRequest()
                    por.BucketName = AWS_BUCKET_NAME
                    por.StorageClass = S3StorageClass.Standard
                    por.ServerSideEncryptionMethod = ServerSideEncryptionMethod.None
                    por.CannedACL = S3CannedACL.PublicRead
                    por.Key = key
                    por.InputStream = file.OpenRead()
                    s3Client.PutObject(por)
                End If

            Catch ex As Exception
                retVal = ex.Message
            End Try

        Catch ex As AmazonS3Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function

    Public Function ListFiles(Optional foldername As String = "/") As List(Of AwsFileInfo)
        Dim obsv As New List(Of AwsFileInfo)
        Dim delimiter As String = "/"
        If Not foldername.EndsWith(delimiter) Then
            foldername = String.Format("{0}{1}", foldername, delimiter)
        End If
        Try
            Try
                Dim request As New ListObjectsRequest() With {.BucketName = AWS_BUCKET_NAME}
                Do
                    Dim response As ListObjectsResponse = s3Client.ListObjects(request)
                    For Each entry As S3Object In response.S3Objects
                        obsv.Add(New AwsFileInfo(entry, foldername))
                    Next
                    If (response.IsTruncated) Then
                        request.Marker = response.NextMarker
                    Else request = Nothing
                    End If
                Loop Until IsNothing(request)

            Catch ex As AmazonS3Exception

            End Try

        Catch ex As Exception

        End Try

        Return obsv
    End Function

    Public Function DeleteFileFromFolder(foldername As String, keyname As String) As String
        Dim retVal As String = ""
        Try
            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, AWS_BUCKET_NAME) Then
                    retVal = "Bucket does not exist"
                Else
                    foldername = foldername.Replace("\", "/")
                    Dim dor As DeleteObjectRequest = New DeleteObjectRequest() With {.BucketName = AWS_BUCKET_NAME, .Key = String.Format("{0}/{1}", foldername, keyname)}
                    s3Client.DeleteObject(dor)
                End If

            Catch ex As AmazonS3Exception
                retVal = ex.Message
            End Try

        Catch ex As Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function

    Public Function DeleteFolder(foldername As String) As Boolean
        Try
            If Not foldername.EndsWith("/") Then
                foldername = foldername & "/"
            End If

            Dim dor As DeleteObjectRequest = New DeleteObjectRequest() With {.BucketName = AWS_BUCKET_NAME, .Key = foldername}
            s3Client.DeleteObject(dor)

            Return True

        Catch ex As AmazonS3Exception
            Return False
        End Try
    End Function

    Public Function GetFileFromFolder(folderName As String, FileName As String, target As String) As String
        Dim retVal As String = ""
        Try
            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, AWS_BUCKET_NAME) Then
                    retVal = "Bucket does not exist"
                Else
                    Dim request As ListObjectsRequest = New ListObjectsRequest() With {.BucketName = AWS_BUCKET_NAME}
                    Do
                        Dim response As ListObjectsResponse = s3Client.ListObjects(request)
                        For i As Integer = 1 To response.S3Objects.Count - 1
                            Dim entry As S3Object = response.S3Objects(i)
                            If Replace(entry.Key, folderName & "/", "") = FileName Then
                                Dim objRequest As GetObjectRequest = New GetObjectRequest() With {.BucketName = AWS_BUCKET_NAME, .Key = entry.Key}
                                Dim objResponse As GetObjectResponse = s3Client.GetObject(objRequest)
                                objResponse.WriteResponseStreamToFile(target & FileName)
                                Exit Do
                            End If
                        Next
                        If (response.IsTruncated) Then
                            request.Marker = response.NextMarker
                        Else
                            request = Nothing
                        End If
                    Loop Until IsNothing(request)
                End If

            Catch ex As AmazonS3Exception
                retVal = ex.Message
            End Try

        Catch ex As Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function

    Public Function OpenFile(folderName As String, FileName As String) As String
        Dim retVal As String = DownloadFile(folderName, FileName)
        If retVal = "" Then
            Dim target As String = Path.GetTempPath()
            System.Diagnostics.Process.Start(target & FileName)
        End If
        Return retVal
    End Function

    Public Function CopyingObject(folderFile As String, destinationfolder As String) As String
        Dim retVal As String = ""
        Try
            Using s3Client
                Dim request As CopyObjectRequest = New CopyObjectRequest
                request.SourceBucket = AWS_BUCKET_NAME
                request.SourceKey = folderFile.Replace("\", "/")
                request.DestinationBucket = AWS_BUCKET_NAME
                request.DestinationKey = destinationfolder.Replace("\", "/")

                Dim response As CopyObjectResponse = s3Client.CopyObject(request)
            End Using

        Catch s3Exception As AmazonS3Exception
            retVal = s3Exception.Message
        End Try

        Return retVal
    End Function

    Public Function RenameObject(folderFile As String, destinationfolder As String) As String
        Dim retVal As String = ""

        Try
            'Dim copyRequest As CopyObjectRequest = New CopyObjectRequest
            'copyRequest.SourceBucket = AWS_BUCKET_NAME
            'copyRequest.SourceKey = folderFile.rep
            'copyRequest.WithSourceBucket("SourceBucket").WithSourceKey("SourceKey").WithDestinationBucket("DestinationBucket").WithDestinationKey("DestinationKey").WithCannedACL(S3CannedACL.PublicRead)
            'S3.CopyObject(copyRequest)
            'Dim deleteRequest As DeleteObjectRequest = New DeleteObjectRequest().WithBucketName("SourceBucket").WithKey("SourceKey")
            's3.DeleteObject(deleteRequest)

        Catch s3Exception As AmazonS3Exception
            retVal = s3Exception.Message
        End Try

        Return retVal
    End Function

    Public Function DownloadFile(folderName As String, FileName As String) As String
        Return DownloadFile(folderName, FileName, "")
    End Function
    Public Function DownloadFile(folderName As String, FileName As String, DestFolder As String) As String
        If DestFolder = "" Then DestFolder = Path.GetTempPath
        Dim target As String = DestFolder
        If Not My.Computer.FileSystem.DirectoryExists(target) Then My.Computer.FileSystem.CreateDirectory(target)

        Dim retVal As String = ""
        If folderName = "" Then folderName = "/"
        folderName = folderName.Replace("\", "/")

        Try
            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, AWS_BUCKET_NAME) Then
                    retVal = "Bucket does not exist"
                Else
                    Dim request As ListObjectsRequest = New ListObjectsRequest() With {.BucketName = AWS_BUCKET_NAME}
                    Do
                        Dim response As ListObjectsResponse = s3Client.ListObjects(request)
                        For i As Integer = 1 To response.S3Objects.Count - 1
                            Dim entry As S3Object = response.S3Objects(i)
                            If Replace(entry.Key, folderName & "/", "") = FileName Then
                                Dim objRequest As GetObjectRequest = New GetObjectRequest() With {.BucketName = AWS_BUCKET_NAME, .Key = entry.Key}
                                Dim objResponse As GetObjectResponse = s3Client.GetObject(objRequest)
                                objResponse.WriteResponseStreamToFile(My.Computer.FileSystem.CombinePath(target, FileName))
                                Exit Do
                            End If
                        Next
                        If (response.IsTruncated) Then
                            request.Marker = response.NextMarker
                        Else
                            request = Nothing
                        End If
                    Loop Until IsNothing(request)
                End If

            Catch ex As AmazonS3Exception
                retVal = ex.Message
            End Try

        Catch ex As Exception
            retVal = ex.Message
        End Try

        Return retVal
    End Function
End Class
