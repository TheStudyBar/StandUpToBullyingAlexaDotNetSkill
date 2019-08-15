#!/bin/sh

dotnet build
dotnet publish
cd bin/Debug/netcoreapp2.1
rm -f skill.zip
cd publish
zip ../skill.zip *

exit 0
