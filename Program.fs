open Farmer
open Farmer.Builders
open Farmer.ContainerGroup
open Settings

let fileShareName = "minecraft-share"
let fileShareSize = 10<Gb>
let volumeName = "minecraft-storage"
let containerName = "minecraft-container"
let dockerImage = "itzg/minecraft-server"
let containerMemory = 4.0<Gb>
let containerCpuCount = 2
let containerGroupName = "minecraft-container-group"
let resourceGroupName = "minecraft"

let storage = storageAccount {
    name storageAccountName
    sku Storage.Standard_LRS
    add_file_share_with_quota fileShareName fileShareSize
    }

let mineServerContainer = 
    containerInstance {
        name containerName
        image dockerImage
        add_public_ports [ Settings.minecraftPort ]
        
        memory containerMemory
        cpu_cores containerCpuCount
        env_vars [
            env_var "EULA" "TRUE"
            env_var "OPS" Settings.adminMinecraftAccount
            env_var "TZ" Settings.timeZone
            env_var "ENABLE_RCON" "false"
            env_var "TYPE" "BUKKIT"
            env_var "MEMORY" (sprintf "%fG" (containerMemory/1.0<Gb>))
        ]
        add_volume_mount volumeName "/data"
    }

let mineServer = containerGroup {
    name containerGroupName
    operating_system Linux
    public_dns Settings.publicDnsName [ TCP, Settings.minecraftPort ]
    restart_policy AlwaysRestart
    add_instances [ mineServerContainer ]
    add_volumes [
        volume_mount.azureFile volumeName fileShareName storage.Name.ResourceName.Value
    ]
}

let deployment = arm {
    location Settings.location
    add_resources [ storage; mineServer ]
}

// printf "Generating ARM template..."
// deployment |> Writer.quickWrite "arm/mineServer"
// printfn "all done! Template written to output.json"

// Alternatively, deploy your resource group directly to Azure here.
deployment
|> Deploy.execute resourceGroupName Deploy.NoParameters
|> printfn "%A"