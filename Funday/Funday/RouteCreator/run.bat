
@echo off

set /p Json="Enter Json file: "
node index.js   -J "%Json%"
