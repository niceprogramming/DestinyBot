FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src
COPY . .
RUN dotnet publish DestinyBot.sln -c Debug -o /app

FROM microsoft/dotnet:2.2-runtime
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "DestinyBot.dll"]