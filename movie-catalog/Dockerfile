# #############################################################################
# Movie Catalog
#

FROM microsoft/dotnet:2.1-sdk 

LABEL MAINTAINER Christopher Town (chris@cdinc.net)
ENV NUGET_XMLDOC_MODE skip
ENV ASPNETCORE_URLS=http://+:4000

# publish
COPY ./src /src
WORKDIR /src/MovieCatalog.Web
RUN dotnet publish -o /app 

# run
# EXPOSE 4000
WORKDIR /app
ENTRYPOINT [ "dotnet", "MovieCatalog.Web.dll" ]

# #############################################################################
