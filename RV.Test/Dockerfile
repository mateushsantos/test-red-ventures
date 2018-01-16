FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore RV.Test

COPY . ./
RUN dotnet publish RV.Test -c Release -o out

FROM microsoft/aspnetcore:2.0
WORKDIR /app

COPY --from=build-env /app/RV.Test/out ./

ENTRYPOINT ["dotnet", "RV.Test.Web.dll"]