# Farmer deployment of a Minecraft server to Azure Containers

## Description
This is a [Farmer](https://compositionalit.github.io/farmer/) library deployment of a [itzg/minecraft-server](https://hub.docker.com/r/itzg/minecraft-server) docker image to [Azure Container Services](https://azure.microsoft.com/product-categories/containers/)

## Preparations
1. Get an [Azure Account](azure.microsoft.com/account/free)
1. Get your _tenant id_ and _subscription id_ ([here](https://aster.cloud/2019/07/30/how-to-retrieve-subscription-id-resource-group-id-tenant-id-client-id-and-client-secret-in-azure/) is a guide)
1. Change the variables in Settings.fs
1. Make sure you have the latest [dotnet core](https://dotnet.microsoft.com/download) and [azure cli](https://docs.microsoft.com/sv-se/cli/azure/install-azure-cli) installed

## How to deploy
1. Login to your temant with `az login -t _tenant id_`
1. Set your account with `az account set --subscription _subscription id_`
1. Deploy the resources  with `dotnet run`