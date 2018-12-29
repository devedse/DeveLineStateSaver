call "%~dp0Cleanup.cmd"

robocopy "%~dp0..\DeveLineStateSaver\bin\Release\netstandard2.0" "%~dp0Output" /E /xf *.pdb

7za a -mm=Deflate -mfb=258 -mpass=15 "%~dp0DeveLineStateSaver.zip" "%~dp0Output\*"
7za a -t7z -m0=LZMA2 -mmt=on -mx9 -md=1536m -mfb=273 -ms=on -mqs=on -sccUTF-8 "%~dp0DeveLineStateSaver.7z" "%~dp0Output\*"