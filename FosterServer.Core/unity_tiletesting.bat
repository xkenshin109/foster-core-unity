:: Compile Script will transfer .dlls to the project
:: Read config options
@echo off
setLocal enableDelayedExpansion
for /f %%a in (FosterCompileSettings.config) do (
	for /f "tokens=1,2,3 delims=| " %%b in ("%%a") do (
		echo %%b
		if "%%b" == "setting" (
			echo i am setting
		)
		echo %%c
		echo %%d
	)
	echo %%a
)
:: List Directory for all Unity Projects
:: Wait for User Input to select from List
:: Compile FosterCore Project 
echo ***Compiling FosterCore Solution***
msbuild ../FosterServerUdp/FosterServerUdp.sln
echo ***Compiling Project Completed
:: Verify CustomLibrary Folder exists

:: Create CustomLibrary Folder
:: Transfer .dlls 


pause
goto end

:handleSetting
	echo Setting baby
	
:handleDll:
	echo DLL baby
	
:end
	exit