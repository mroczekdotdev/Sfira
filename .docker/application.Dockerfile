FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime

# System.Drawing native dependencies
RUN apk update && \
    apk add libgdiplus -u --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Sfira.dll"]
