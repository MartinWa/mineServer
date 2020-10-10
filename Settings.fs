module Settings
open Farmer

let adminMinecraftAccount = "sss" // Minecraft user that will be OP.
let publicDnsName = "xxx" // Will be xxx.westeurope.azurecontainer.io. Must be unique in Azure.
let storageAccountName = "yyy" // Must be unique in Azure. Not a good error if it is not. 
let minecraftPort = 25565us // Default 25565 is best
let location = Location.WestEurope // What Azure data center to deploy to
let timeZone = "Europe/London" // Server timezone