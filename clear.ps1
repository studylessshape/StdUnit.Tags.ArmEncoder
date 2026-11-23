Get-ChildItem -Force -Recurse -Filter ".vs"  | Remove-Item -Recurse -Force
Get-ChildItem -Force -Recurse -Filter "bin"  | Remove-Item -Recurse -Force
Get-ChildItem -Force -Recurse -Filter "obj"  | Remove-Item -Recurse -Force
Get-ChildItem -Force -Recurse -Filter "paket-files"  | Remove-Item -Recurse -Force