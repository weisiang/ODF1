title FDCIM

for /F "tokens=1,2" %%i in ('tasklist /FI "WINDOWTITLE eq 系統管理員:  FDCIM" /fo table /nh') do set pid=%%j

echo "start %CIM%" >> %SysLogFile% & @start /wait %CIM% %pid% 
pause
@echo return code %errorlevel% >> %SysLogFile%
@goto %errorlevel%


:0
pause
@exit
::Common
:-1
#echo Program already run.
pause
@exit

:-10
@echo Environment Variables : Root LogFolderPath not defined.
pause
exit

:-11
@echo Environment Variables : Root ConfigFolderPath not defined.
pause
exit

:-12
@echo Sys/EqLayout.xml not found. Please check Sys config.
pause
exit

:-13
@echo Sys/MemoryIOClient.xml not found. Please check Sys config.
pause
exit

:-14
@echo Modules/logs.ini not found. Please check UI config.
pause
exit

:-15
@echo Config/Sys/timechartAdd.xml not found. Please check config.
pause
exit

:-16
@echo Config/Sys/Account.ini not found. Please check config.
pause
exit

:-17
@echo Config/Module/system.ini not found. Please check config.
pause
exit

::UI
:-20
@echo Config/UI/LayoutPos.xml not found. Please check config.
pause
exit

::LGC
:-30
@echo Config/LGC/PPID.xml not found. Please check config.
pause
exit
:-31
@echo Config/LGC/StatusRecord.ini not found. Please check config.
pause
exit

exit
