call "%~dp0Cleanup.cmd"

robocopy "%~dp0..\DeveLineStateSaver\bin\Release\netstandard2.0" "%~dp0Output" /E /xf *.pdb

rem "%~dp0\7z_x64_1805\7za.exe" a -mm=Deflate -mfb=258 -mpass=15 "%~dp0DeveLineStateSaver.zip" "%~dp0Output\*"
rem "%~dp0\7z_x64_1805\7za.exe" a -t7z -m0=LZMA2 -mmt=on -mx9 -md=1536m -mfb=273 -ms=on -mqs=on -sccUTF-8 "%~dp0DeveLineStateSaver.7z" "%~dp0Output\*"
7zip_x64.exe a -mm=Deflate -mfb=258 -mpass=15 "%~dp0DeveLineStateSaver.zip" "%~dp0Output\*"
7zip_x64.exe a -t7z -m0=LZMA2 -mmt=on -mx9 -md=1536m -mfb=273 -ms=on -mqs=on -sccUTF-8 "%~dp0DeveLineStateSaver.7z" "%~dp0Output\*"