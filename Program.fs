open Farmer
open Farmer.Builders
open Farmer.ContainerGroup

let adminMinecraftAccount = ""
let publicDnsName = "mineserver12345" // Will be mineserver12345.westeurope.azurecontainer.io
let storageAccountName = "minecraftstorage12345"
let fileShareName = "minecraft-share"
let volumeName = "minecraft-storage"
let minecraftPort = 25565us

let storage = storageAccount {
    name storageAccountName
    sku Storage.Standard_LRS
    add_file_share_with_quota fileShareName 10<Gb>
    }

let mineServerContainer = 
    containerInstance {
        name "minecraft-container"
        image "itzg/minecraft-server"
        add_public_ports [ minecraftPort ]
        
        memory 4.0<Gb>
        cpu_cores 2
        env_vars [
            env_var "EULA" "TRUE"
            env_var "OPS" adminMinecraftAccount
            env_var "TZ" "Europe/Stockholm"
        ]
        add_volume_mount volumeName "/data"

    }

let mineServer = containerGroup {
    name "minecraft-container-group"
    operating_system Linux
    public_dns publicDnsName [ TCP, minecraftPort ]
    restart_policy AlwaysRestart
    add_instances [ mineServerContainer ]
    add_volumes [
        volume_mount.azureFile volumeName fileShareName storage.Name.ResourceName.Value
    ]
}

let deployment = arm {
    location Location.WestEurope
    add_resources [ storage; mineServer ]
}

// printf "Generating ARM template..."
// deployment |> Writer.quickWrite "arm/mineServer"
// printfn "all done! Template written to output.json"

// Alternatively, deploy your resource group directly to Azure here.
deployment
|> Deploy.execute "minecraft" Deploy.NoParameters
|> printfn "%A"
