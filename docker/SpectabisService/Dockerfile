FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0-core-tools

WORKDIR /app

COPY *.sln .
COPY ./src ./src
COPY ./Tests ./Tests

RUN dotnet restore -r linux-x64

RUN dotnet publish src/SpectabisService \
	--no-restore \
	-c Release -r linux-x64 --self-contained \
	--output /out/SpectabisService

RUN dotnet publish Tests/Integration/ServiceTests \
	--no-restore \
	-c Release -r linux-x64 --self-contained \
	--output /out/ServiceTests