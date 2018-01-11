FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore RV.Test

COPY . ./
RUN dotnet publish RV.Test -c Release -o out

FROM microsoft/aspnetcore:2.0
WORKDIR /app

COPY --from=build-env /app/RV.Test/out ./

RUN apt-get update && apt-get install -y dos2unix

COPY --from=build-env /app/RV.Test/entrypoint.sh ./

RUN dos2unix ./entrypoint.sh && apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "RV.Test.Web.dll"]