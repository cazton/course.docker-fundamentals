# #############################################################################
# Movie Catalog
#

# #############################################################################
# BASE IMAGE
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base


WORKDIR /app


# vscode debugging support
WORKDIR /vsdbg
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg


# #############################################################################
# BUILDER IMAGE
FROM microsoft/dotnet:2.1-sdk AS builder
ENV NUGET_XMLDOC_MODE skip


# publish
COPY ./src /src
WORKDIR /src/MovieCatalog.Web
RUN dotnet publish -o /publish 

# #############################################################################
# PRODUCTION IMAGE
FROM base AS production

LABEL MAINTAINER Christopher Town (chris@cdinc.net)
ENV NUGET_XMLDOC_MODE skip
ENV ASPNETCORE_URLS=http://+:4000
EXPOSE 80

# gather published output into /app
WORKDIR /app
COPY --from=builder /publish .

# Kick off a container just to wait debugger to attach and run the app
ENTRYPOINT ["/bin/bash", "-c", "if [ \"$REMOTE_DEBUGGING\" = \"enabled\" ]; then sleep infinity; else dotnet MovieCatalog.Web.dll; fi"]

# #############################################################################
