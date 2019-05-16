
@set UIEXENAME=FDUI.exe
@set LGCEXENAME=FDLGC.exe
@set MmfServerEXENAME=MmfEventDispatchServer.exe
@set DebugWinEXENAME=DebugWin.exe
@set CIMEXENAME=FDCIM.exe
@set KGSMemoryIOServerEXENAME=KGSMemoryIOServer.exe
@set HIRATAEXENAME=WFAHirataAPI.exe



@set SysLogFileFolder=%LogFolderPath%Sys\
@set SysLogFile=%SysLogFileFolder%%date:~0,4%%date:~5,2%%date:~8,2%.log

@if not exist %SysLogFileFolder% (
	@mkdir %SysLogFileFolder%
	@echo "MD %SysLogFileFolder%"
)
@if not exist %SysLogFile% (
	@type nul > %SysLogFile%
)
@echo ###STOP### >> %SysLogFile%
@echo %date% %time% >> %SysLogFile%

@echo SysLogFile : %SysLogFile% >> %SysLogFile%

taskkill /f /im %UIEXENAME%
taskkill /f /im %LGCEXENAME%
taskkill /f /im %CIMEXENAME%
taskkill /f /im %MmfServerEXENAME%
taskkill /f /im %DebugWinEXENAME%
taskkill /f /im %KGSMemoryIOServerEXENAME%
taskkill /f /im %HIRATAEXENAME%
