
@echo off
 IF NOT EXIST "Uninstall-RunMeAsAdministor.bat" ( 
 echo You must run this file from the directory in which it is located. Run Command Propmt as Administrator and navigate to this folder.
 GOTO EXIT
 )
:NOWINDIR
set /p UserInputPath= Enter the location of your DNN installation (eg. c:\DNN\):
@set "var=%UserInputPath%"


IF Not %var:~-1%==\ set var=%var%\
 IF NOT EXIST %var% ( 
	echo Path does now exist. Please try again.
 GOTO NOWINDIR
 )

 echo %var%
 echo removing symbolic links...
 
 IF NOT EXIST %var%App_Code ( 
	echo No App_Code folder
	GOTO AFTERAPPCODE
 )
 cd App_Code
 FOR /d %%G in (*) DO   rd %var%App_Code\%%G
  FOR %%G in (*) DO   del %var%App_Code\%%G

:AFTERAPPCODE  
REM SETUP DESKTOP MODULES
cd ..\DesktopModules\

rd %var%DesktopModules\AgapeConnect
rd %var%DesktopModules\AgapeFR
rd %var%DesktopModules\AgapeEurope
rd %var%DesktopModules\AgapeUK

REM SETUP FRANCE PORTAL SPECIFIC RADEDITOR CONFIG FILES
cd ..\DesktopModules\Admin\RadEditorProvider\ConfigFile
del %var%DesktopModules\Admin\RadEditorProvider\ConfigFile\ConfigFile.PortalId.0.xml
cd ..\ToolsFile
del %var%DesktopModules\Admin\RadEditorProvider\ToolsFile\ToolsFile.PortalId.0.xml
cd ..\App_LocalResources
FOR %%G in (*.*.fr-FR.resx) DO del %var%DesktopModules\Admin\RadEditorProvider\App_LocalResources\%%G
cd ..\..\..

REM SETUP FRANCE PORTAL SPECIFIC DNNSharp RESOURCE FILES
cd DnnSharp\ActionForm\App_LocalResources
del %var%DesktopModules\DnnSharp\ActionForm\App_LocalResources\Form.fr-FR.resx
del %var%DesktopModules\DnnSharp\ActionForm\App_LocalResources\Reports.fr-FR.resx
cd ..\..\..

REM SETUP APP_WebRefereces
cd ..\App_WebReferences
IF NOT EXIST %var%App_WebReferences ( 
	echo No App_WebReferences folder
	GOTO AFTERAPPWEBREF 
)
FOR /d %%G in (*) DO  rd %var%App_WebReferences\%%G

:AFTERAPPWEBREF		
REM SETUP PORTAL DIRECTORY
cd ..\Portals\_default\Skins
FOR /d %%G in (*) DO  rd %var%Portals\_default\Skins\%%G
cd ..\Containers
FOR /d %%G in (*) DO  rd %var%Portals\_default\Containers\%%G
cd ..
FOR %%G in (*) DO  del %var%Portals\_default\%%G

cd ../0
IF NOT EXIST %var%Portals\0 ( 
	echo No \Portals\0 folder
	GOTO AFTERPORTAL
)
FOR %%G in (*) DO  del %var%Portals\0\%%G
FOR /d %%G in (*) DO  rd %var%Portals\0\%%G

:AFTERPORTAL
REM SETUP js Directory
cd ../../js
FOR %%G in (*) DO  del %var%js\%%G
FOR /d %%G in (*) DO  rd %var%js\%%G


REM SETUP bin Directory
cd ../bin
FOR %%G in (*) DO  del %var%bin\%%G
FOR /d %%G in (*) DO  rd %var%bin\%%G
cd ..

REM SETUP the sso and Scripts Directory
rd %var%sso
rd %var%Scripts

REM SETUP the Site Analytics file
del %var%SiteAnalytics.config

echo complete. Press any key to exit.
pause > nul

 :EXIT