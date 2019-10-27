Imports System.Windows.Media.Animation

Public Class NotificationBanner
    Public Shake As Storyboard

    Public Sub ShakeIcon()
        Shake = Me.FindResource("NotificationActiveLooping")
        Shake.Begin()
    End Sub

    Public Sub Notify(ByVal NotificationHeader As String, ByVal NotificationBody As String)
        ShakeIcon()
        LBLNotificationHeader.Content = NotificationHeader
        LBLNotificationBody.Text = NotificationBody
    End Sub
End Class
