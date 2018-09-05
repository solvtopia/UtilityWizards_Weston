Imports Amazon.S3
Imports Amazon.S3.Model
Imports Amazon.Runtime
Imports Amazon
Imports Amazon.S3.Util
Imports System.Collections.ObjectModel
Imports System.IO

Public Class AWSHelper
    Const AWS_ACCESS_KEY As String = "AKIAILO4RJHZWPMRIARA"
    Const AWS_SECRET_KEY As String = "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"
    Const AWS_BUCKET_NAME As String = "wpg-file-upload"

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

    Public Function ListingFiles(Optional foldername As String = "/") As ObservableCollection(Of String)
        Dim obsv As New ObservableCollection(Of String)
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
                        If Not foldername = "/" Then
                            If entry.Key.ToString.StartsWith(foldername) Then
                                Dim replacementstring As String = Replace(entry.Key, foldername, "")
                                If Not replacementstring = "" Then
                                    obsv.Add(replacementstring)
                                End If
                            End If
                        Else obsv.Add(Replace(entry.Key, foldername, ""))
                        End If
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
