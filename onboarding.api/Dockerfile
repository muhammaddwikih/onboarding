#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster as build
WORKDIR /app
EXPOSE 80
EXPOSE 443

# copy csproj and restore as distinct layers
COPY *.sln .
COPY onboarding.api/*.csproj ./onboarding.api/
COPY onboarding.bll/*.csproj ./onboarding.bll/
COPY onboarding.bll.Test/*.csproj ./onboarding.bll.Test/
COPY onboarding.dal/*.csproj ./onboarding.dal/
COPY External/*.csproj ./External/
COPY Scheduler/*.csproj ./Scheduler/

#restore dependencies
RUN dotnet restore

# copy everything else and build app
COPY onboarding.api/ ./onboarding.api/
COPY onboarding.bll/ ./onboarding.bll/
COPY onboarding.bll.Test/ ./onboarding.bll.Test/
COPY onboarding.dal/ ./onboarding.dal/
COPY External/ ./External/
COPY Scheduler/ ./Scheduler/

#
WORKDIR /app/onboarding.api
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
WORKDIR /app 
#
COPY --from=build /app/onboarding.api/out ./
ENTRYPOINT ["dotnet", "onboarding.api.dll"]