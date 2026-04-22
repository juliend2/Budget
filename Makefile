build: clean
	cd Budget.Web && dotnet build

clean:
	cd Budget.Web && dotnet clean
	cd Budget.Web && rm -rf bin/ obj/

watch:
	cd Budget.Web && dotnet watch

publish:
	cd Budget.Web && dotnet publish -c Release -o ./publish
