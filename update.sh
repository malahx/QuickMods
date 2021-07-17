#!/bin/bash

a=`find . -name \*csproj -print`


for i in $a; do
echo $i
	sed -i 's/\.\.\\\.\.\\lib\\Managed/r:\\KSP_1.3.1_dev\\KSP_x64_Data\\Managed/g' $i
done
