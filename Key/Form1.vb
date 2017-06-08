Imports System.Management
Imports System
Imports System.Text
Imports System.Security.Cryptography


Public Class Form1
    Public kk As String = ""
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim hw As New clsComputerInfo

        Dim hdd As String
        Dim cpu As String
        Dim mb As String
        Dim mac As String

        cpu = hw.GetProcessorId()
        hdd = hw.GetVolumeSerial("C")
        mb = hw.GetMotherBoardID()
        mac = hw.GetMACAddress()


        MsgBox(cpu & "   " & hdd & "   " & mb & "   " & mac)


        Dim hwid As String = Strings.UCase(hw.getMD5Hash(cpu & hdd & mb))

        ' MessageBox.Show(Strings.UCase(hwid))

        TextBox1.Text = hwid
        kk = hwid
        CheckLicense()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
    Public Sub CheckLicense()
        If kk = "08F6ADA116C6946AC2D8B168B6F78FC1" Then
            MsgBox("License correct")
        Else
            MsgBox("License did not match")
        End If
    End Sub
End Class


Public Class clsComputerInfo

    Friend Function GetProcessorId() As String
        Dim strProcessorId As String = String.Empty
        Dim query As New SelectQuery("Win32_processor")
        Dim search As New ManagementObjectSearcher(query)
        Dim info As ManagementObject

        For Each info In search.Get()
            strProcessorId = info("processorId").ToString()
        Next
        Return strProcessorId

    End Function

    Friend Function GetMACAddress() As String

        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim MACAddress As String = String.Empty
        For Each mo As ManagementObject In moc

            If (MACAddress.Equals(String.Empty)) Then
                If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()

                mo.Dispose()
            End If
            MACAddress = MACAddress.Replace(":", String.Empty)

        Next
        Return MACAddress
    End Function

    Friend Function GetVolumeSerial(Optional ByVal strDriveLetter As String = "C") As String

        Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", strDriveLetter))
        disk.Get()
        Return disk("VolumeSerialNumber").ToString()
    End Function

    Friend Function GetMotherBoardID() As String

        Dim strMotherBoardID As String = String.Empty
        Dim query As New SelectQuery("Win32_BaseBoard")
        Dim search As New ManagementObjectSearcher(query)
        Dim info As ManagementObject
        For Each info In search.Get()

            strMotherBoardID = info("SerialNumber").ToString()

        Next
        Return strMotherBoardID

    End Function



    Friend Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult
    End Function
End Class