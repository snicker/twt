@echo off
del /S /Q .\deployment\twt\*.*
rd .\deployment\twt
md deployment
md .\deployment\twt
copy .\bin\ReleaseMerged\*.* .\deployment\twt
copy .\lib\*.txt .\deployment\twt
.\lib\7za.exe a .\deployment\twt.zip .\deployment\twt\*.*