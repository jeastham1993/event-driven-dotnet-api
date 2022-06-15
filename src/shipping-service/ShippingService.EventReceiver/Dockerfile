FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
COPY ShippingService.EventReceiver.csproj .
RUN dotnet restore --runtime linux-musl-x64 ShippingService.EventReceiver.csproj

COPY . .
RUN dotnet publish -c Release -r linux-musl-x64 --self-contained -o out ShippingService.EventReceiver.csproj

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out/* /app/
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["/app/ShippingService.EventReceiver"]