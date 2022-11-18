### Using
-Move the first files to a permanent location, if you delete files program cant work! </br>
-Open with visual studio and change firebase auth and url on **FirebaseHelper** page with yours.</br>
-Start on debug mode</br>
-Test program, open your firebase realtime data base and first change **Timer** to **6000** after, change **Shutdown** property to **true**</br>
-If program is working change **Cancel** property to **True** its canceling shutdown timer.</br>
-Stop debuging and close program.</br>
-Open file location **"@\RemoteShutdownPC\bin\Debug\"** select **RemoteShutdownPC.exe** and create a shortcut.</br>
-After copy shortcut to **"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup"**.</br>
-Done now program is starting on every startup, you can test change firebase **Shutdown** property to **True** your pc will shutdown automaticly.</br>
