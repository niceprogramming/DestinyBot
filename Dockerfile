FROM microsoft/dotnet:2.1-sdk AS build-env
COPY . .
RUN dotnet publish DestinyBot.sln -c Release -o /app

FROM microsoft/dotnet:2.1-runtime-alpine
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "DestinyBot.dll"]