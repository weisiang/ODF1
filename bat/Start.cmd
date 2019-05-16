
::---------AP need read these environment variables------------
@set LogFolderPath=%cd%\..\Logs\
@set ConfigFolderPath=%cd%\..\config\
@set BatFolder=%cd%
::-------------------------------------------------------------
@set UIEXENAME=FDUI.exe
@set LGCEXENAME=FDLGC.exe
@set MmfServerEXENAME=MmfEventDispatchServer.exe
@set DebugWinEXENAME=DebugWin.exe
@set KGSMemoryIOServerEXENAME=KGSMemoryIOServer.exe
@set CIMEXENAME=FDCIM.exe
@set HIRATAAPIPATH=D:\Hirata_API\API_exe\WFAHirataAPI.exe
@set HIRATAEXENAME=WFAHirataAPI.exe

@set SysLogFileFolder=%LogFolderPath%Sys\
@set SysLogFile=%SysLogFileFolder%%date:~0,4%%date:~5,2%%date:~8,2%.log

@echo LogFolderPath : %LogFolderPath%
@echo ConfigFolderPath : %ConfigFolderPath%
@echo SysLogFile : %SysLogFile%

@if not exist %SysLogFileFolder% (
	@mkdir %SysLogFileFolder%
	@echo "MD %SysLogFileFolder%"
)
@if not exist %SysLogFile% (
	@type nul > %SysLogFile%
)
@echo ###START### >> %SysLogFile%
@echo %date% %time% >> %SysLogFile%

@echo LogFolderPath : %LogFolderPath% >> %SysLogFile%
@echo ConfigFolderPath : %ConfigFolderPath% >> %SysLogFile%
@echo SysLogFile : %SysLogFile% >> %SysLogFile%

@cd ..
@for /F "tokens=*" %%a in ('dir /s /b "obj"') do rmdir /q /s %%a
timeout /t 1
cd %BatFolder%

 where /r %cd%\.. %UIEXENAME% > ui.tmp 
 <ui.tmp (set /p FDUI=)

 where /r %cd%\.. %LGCEXENAME% > lgc.tmp 
 <lgc.tmp (set /p LGC=)

 where /r %cd%\.. %CIMEXENAME% > cim.tmp 
 <cim.tmp (set /p CIM=)

 where /r %cd%\.. %MmfServerEXENAME% > MmfEventDispatchServer.tmp 
 <MmfEventDispatchServer.tmp (set /p MmfServer=)
 
 where /r %cd%\.. %DebugWinEXENAME% > DebugWin.tmp 
 <DebugWin.tmp (set /p DebugWin=)

 where /r %cd%\.. %KGSMemoryIOServerEXENAME% > KGSMemoryIOServerEXENAME.tmp 
 <KGSMemoryIOServerEXENAME.tmp (set /p KGSMemoryIOServer=)

 
taskkill /f /im %UIEXENAME%
taskkill /f /im %LGCEXENAME%
taskkill /f /im %CIMEXENAME%
taskkill /f /im %MmfServerEXENAME%
taskkill /f /im %DebugWinEXENAME%
taskkill /f /im %HIRATAEXENAME%

timeout /t 3
::@rmdir /q /s %CIM%\..\obj
@start %HIRATAAPIPATH% & echo "start %HIRATAAPIPATH%" >> %SysLogFile%
timeout /t 3
@start %DebugWin% & echo "start %DebugWin%" >> %SysLogFile%
@start %KGSMemoryIOServer% & echo "start %KGSMemoryIOServer%" >> %SysLogFile%

cd %cd%\..\Logs
@start %MmfServer% & echo "start %MmfServer%" >> %SysLogFile%
cd %BatFolder%
timeout /t 3
@start UI.cmd & echo "start UI.cmd" >> %SysLogFile%
::@start LGC.cmd & echo "start LGC.cmd" >> %SysLogFile%
::@start CIM.cmd & echo "start CIM.cmd" >> %SysLogFile%

