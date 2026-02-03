#Setup for the image and container:
    #Write: 
    #1. "docker build -f "Dockerfile" -t <tag_name> .
    #2. "docker run -p 8080:8080 <tag_name>"
#Read the documentation in this file, and on this website: "https://www.roundthecode.com/dotnet-tutorials/how-do-you-write-dockerfile-asp-net-core-app"

#First define the SDK to build the app.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

#This doesn't refer to a specific path in the repository. It is the name of the directory inside the docker image.
WORKDIR /source

# This copies everything from the folder you referred to when running `docker build`
# into the image’s current working directory (/source).
# `./ .` means “copy everything here” - and if it was written like: `./src` it would copy only that folder.
COPY ./ .

#Change the WORKDIR, to Chirp.Web.
WORKDIR /source/src/Chirp.Web

#Now we can call the necessary dotnet logic - restore, build and publish.
#The '-o + path' refers to where docker will put the "output" of those actions.
#This becomes relevant when we want docker to refer to the outputs later (especially the dll file from the publish)
#(Again these paths only exist within the docker image)
RUN dotnet restore "./Chirp.Web.csproj" 
RUN dotnet build "./Chirp.Web.csproj" -o /app/build
RUN dotnet publish "./Chirp.Web.csproj" -o /app/publish


#Defines the SDK for running the app.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS run

#Now we define the WORKDIR to be, the local path where Docker writes it's output (look at build & publish)
WORKDIR /app

#Now we copy the dll, that we got from building.
COPY --from=build /app/publish .

#Define the default executable that docker will run (dotnet Chirp.Web.dll)
ENTRYPOINT ["dotnet", "Chirp.Web.dll"]