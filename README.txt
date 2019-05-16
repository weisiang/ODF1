1. FD project moudle naming UI / LGC / CIM only.
2. module output exe file naming FD+ $module name , like FDUI / FDLGC / FDCIM.
	if not , the bat file can't open correctly.
3. this project use environment variabls , so programer have to execut bat/SetEnvi.cmd to develop
		3.1 close the VS.
		3.2 execute SetEnvi.cmd.
		3.2 open VS.
4. Every module has own config.
	2.1 logs.ini (module logs setting.)
	2.2 system.ini (only set module name.)
	2.3 else ( if module need can add any config file.)
	
	2.4 system config folder has:
		2.4.1 EqLayout.xml
		2.4.2 MemoryIOClient.xml
	
5. Release to user side , please copy bat/Start.cmd and Stop.cmd to desktop.

6. if project use Memory server / debugwin , please use Start.cmd to open the software.

7. Every modul's MMF controller class have to use 
	7.1  MmfObjectNamespace = "CommonData." + $projectName ; (like CommonData.HIRATA).
	7.2  cv_Assembly = Assembly.LoadFrom(SysUtils.ExtractFileDir(SysUtils.GetExeName()) + "\\CommonData.dll");

8.Every New project need add a folder at CommonData dll , and change the name space. 

9. Module's main form has the code  "CommonData.HIRATA.CommonStaticData.KillTerminal(args);"
	programer can use "Environment.Exit($return_code);" to exit the module before the kill terminal command.
	And programer have to add some define at bat file.
	Like:
	:0
	pause
	@exit

	:-1
	pause
	@exit

	:-10
	@echo LogPath not found.
	pause

	:-11
	@echo ConfigPath not found.
	pause
	



	

