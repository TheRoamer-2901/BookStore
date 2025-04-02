@echo off
SET TestResultsFileProjectName=TestReport
SET DLLToTestRelativePath=bin\Debug\net8.0\BookStore.Test.dll
SET Filters=+[BookStore.Service*]* +[BookStore.Persistence*]* -[BookStore.Test*]*

REM Set the correct paths for the tools
SET OpenCoverPath="C:\Users\%USERNAME%\.nuget\packages\opencover\4.7.1221\tools\OpenCover.Console.exe"
SET NUnitConsolePath="C:\Users\%USERNAME%\.nuget\packages\nunit.consolerunner\3.19.2\tools\nunit3-console.exe"
SET ReportGeneratorPath="C:\Users\%USERNAME%\.nuget\packages\reportgenerator\5.4.5\tools\net8.0\ReportGenerator.exe"

REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

REM Remove any previous test execution files
IF EXIST "%~dp0%TestResultsFileProjectName%.trx" del "%~dp0%TestResultsFileProjectName%.trx%"

REM Remove previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"

REM Run the tests and generate coverage
call :RunOpenCoverUnitTestMetrics

REM Generate the report output based on the test results
if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)

REM Launch the report
if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%


:RunOpenCoverUnitTestMetrics
%OpenCoverPath% ^
-register:user ^
-target:%NUnitConsolePath% ^
-targetargs:"--noheader \"%~dp0%DLLToTestRelativePath%\"" ^
-filter:"%Filters%" ^
-mergebyhash ^
-skipautoprops ^
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" ^
-output:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml"
exit /b %errorlevel%


:RunReportGeneratorOutput
%ReportGeneratorPath% ^
-reports:"%~dp0GeneratedReports\%TestResultsFileProjectName%.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%


:RunLaunchReport
start "report" "%~dp0GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%
