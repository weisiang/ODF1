
for /f "tokens=*" %%a in ('where /r %cd%\.. licence.dat' ) do del /f /q %%a
 