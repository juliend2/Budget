clean:
	cd Budget.Web && dotnet clean
	cd Budget.Web && rm -rf bin/ obj/
	cd Budget.Web && dotnet build

watch:
	cd Budget.Web && dotnet watch