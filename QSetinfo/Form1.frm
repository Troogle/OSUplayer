VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   2316
   ClientLeft      =   108
   ClientTop       =   456
   ClientWidth     =   3624
   LinkTopic       =   "Form1"
   ScaleHeight     =   2316
   ScaleWidth      =   3624
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  '´°¿ÚÈ±Ê¡
   Visible         =   0   'False
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
On Error GoTo 1
App.TaskVisible = False
Set o = CreateObject("QQCPHelper.CPAdder")
If Command <> "" Then
Dim num As Long
num = InStr(Command, " ")
Dim strin As String
strin = Mid(Command, num + 1)
num = Val(Left(Command, num - 1))
o.PutRSInfo num, 65542, strin, "Developed by Troogle"
Else
1: t = MsgBox("Developed by Troogle", , "Wrong Usage")
End If
Unload Me
End Sub
