Imports Microsoft.VisualBasic
Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Namespace AgapeEncryption


    Public Class AgapeEncrypt
        'Change these values in your own implementation

        Public Shared Function Decrypt(ByVal val As String) As String
            Dim SharedKey As Byte() = {204, 3, 86, 175, 154, 132, 65, 229, 87, 43, 89, 195, 132, 41, 77, 61}
            Dim sharedvector As Byte() = {152, 198, 32, 65, 99, 111, 234, 132, 76, 48, 32, 78, 165, 84, 32, 5}

            Dim tdes As New TripleDESCryptoServiceProvider()
            Dim toDecrypt As Byte() = Convert.FromBase64String(val)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, tdes.CreateDecryptor(SharedKey, sharedvector), CryptoStreamMode.Write)
            cs.Write(toDecrypt, 0, toDecrypt.Length)
            cs.FlushFinalBlock()
            Return Encoding.UTF8.GetString(ms.ToArray())
        End Function

        Public Shared Function Encrypt(ByVal val As String) As String
            Dim SharedKey As Byte() = {204, 3, 86, 175, 154, 132, 65, 229, 87, 43, 89, 195, 132, 41, 77, 61}
            Dim sharedvector As Byte() = {152, 198, 32, 65, 99, 111, 234, 132, 76, 48, 32, 78, 165, 84, 32, 5}

            Dim tdes As New TripleDESCryptoServiceProvider()
            Dim toEncrypt As Byte() = Encoding.UTF8.GetBytes(val)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, tdes.CreateEncryptor(SharedKey, sharedvector), CryptoStreamMode.Write)

            cs.Write(toEncrypt, 0, toEncrypt.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        End Function
    End Class



End Namespace

