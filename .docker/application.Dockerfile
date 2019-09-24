FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime

# System.Drawing native dependencies
RUN apk update && \
    apk add libgdiplus -u --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/

WORKDIR /app
COPY --from=build /app/Sfira/out ./

ENTRYPOINT ["dotnet", "Sfira.dll"]
