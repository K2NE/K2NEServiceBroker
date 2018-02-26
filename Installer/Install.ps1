#If a GUID is found that is the same, it will "overwrite" the service type.
$guid = [guid]"1cf912ee-0d6d-43a8-b8b5-9514ac30113e" 
$brokerName = "K2Field.K2NE.ServiceBroker.K2NEServiceBroker"
$brokerSystemName = $brokerName
$brokerDLL = "K2Field.K2NE.ServiceBroker.dll"
$displayName = $brokerName -replace '\.',' '
$brokerDescription = "The K2NE Service Broker. For more information, see https://github.com/K2NE/K2NEServiceBroker"

Function GetK2Version([string]$machine = $env:computername) {
    $registryKeyLocation = "SOFTWARE\SourceCode\Installer\"
    $registryKeyName = "Version"

	Write-Debug "Getting K2 version from $machine "
    
    $reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine, $machine)
    $regKey= $reg.OpenSubKey($registryKeyLocation)
    $installVer = $regKey.GetValue($registryKeyName)
    return [Version]$installVer.Major
}

Function StartK2Service() {
	Write-Host  "Starting K2 blackpearl service"
	$job = Start-Job -ScriptBlock {
		Get-Service -DisplayName 'K2 blackpearl Server' | where-object {$_.Status -ne "Running"} | Start-Service
	}
	Wait-Job $job
	Receive-Job $job
}

Function StopK2Service() {
    Write-Host "Stopping K2 blackpearl service"

	$job = Start-Job -ScriptBlock {
		Get-Service -DisplayName 'K2 blackpearl Server' | where-object {$_.Status -eq "Running"} | Stop-Service -Confirm:$false -Force 
		Stop-Process -ProcessName "K2HostServer"  -force -ErrorAction SilentlyContinue -Confirm:$false
	}
	Wait-Job $job
	Receive-Job $job
}

Function StartK2FiveService() {
	Write-Host  "Starting K2 Server service"
	$job = Start-Job -ScriptBlock {
		Get-Service -DisplayName 'K2 Server' | where-object {$_.Status -ne "Running"} | Start-Service
	}
	Wait-Job $job
	Receive-Job $job
}

Function StopK2FiveService() {
    Write-Host "Stopping K2 Server service"

	$job = Start-Job -ScriptBlock {
		Get-Service -DisplayName 'K2 Server' | where-object {$_.Status -eq "Running"} | Stop-Service -Confirm:$false -Force 
		Stop-Process -ProcessName "K2HostServer"  -force -ErrorAction SilentlyContinue -Confirm:$false
	}
	Wait-Job $job
	Receive-Job $job
}

Function GetK2InstallPath([string]$machine = $env:computername) {
    $registryKeyLocation = "SOFTWARE\SourceCode\BlackPearl\blackpearl Core\"
    $registryKeyName = "InstallDir"

	Write-Debug "Getting K2 install path from $machine "
    
    $reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine, $machine)
    $regKey= $reg.OpenSubKey($registryKeyLocation)
    $installDir = $regKey.GetValue($registryKeyName)
    return $installDir
}

Function GetK2ConnectionString([string]$k2Server = "localhost", [int]$port = 5555) {
    Write-Debug "Creating connectionstring for machine '$k2Server' and port '$port'";
	Add-Type -AssemblyName ('SourceCode.HostClientAPI, Version=4.0.0.0, Culture=neutral, PublicKeyToken=16a2c5aaaa1b130d')
		
    $connString = New-Object -TypeName "SourceCode.Hosting.Client.BaseAPI.SCConnectionStringBuilder";
    $connString.Integrated = $true;
    $connString.IsPrimaryLogin = $true;
    $connString.Host = $k2Server;
    $connString.Port = $port;

    return $connString.ConnectionString; 
}

Function RegisterServiceType([string]$k2ConnectionString, [string]$k2Server, [string]$ServiceTypeDLL, [guid]$guid, [string]$systemName, [string]$displayName, [string]$description = "") {
    # Get Paths for local environment and for the remote machine, we might run this installer from a simple windows 7 host, while we deploy to a server that has a different drive...
    $k2Path = GetK2InstallPath
    $remK2Path = GetK2InstallPath -machine $k2Server
    $smoManServiceAssembly = Join-Path $k2Path "bin\SourceCode.SmartObjects.Services.Management.dll"
    $serviceBrokerAssembly = Join-Path $remK2Path "ServiceBroker\$($ServiceTypeDLL)"
	$className = "K2Field.K2NE.ServiceBroker.K2NEServiceBroker";
	
    Write-Debug "Adding/Updating ServiceType $serviceBrokerAssembly with guid $guid using $k2ConnectionString"
	Write-Debug  "ServiceBrokerAssembly: $serviceBrokerAssembly"
    
    Add-Type -Path $smoManServiceAssembly #We load this assembly locally, but we connect to the remote host.
    $smoManService = New-Object SourceCode.SmartObjects.Services.Management.ServiceManagementServer

    #Create connection and capture output (methods return a bool)
    $tmpOut = $smoManService.CreateConnection()
    $tmpOut = $smoManService.Connection.Open($k2ConnectionString);
    Write-Debug "Connected to K2 host server"

    # Check if we need to update or register a new one...
    if ([string]::IsNullOrEmpty($smoManService.GetServiceType($guid)) ) {
        Write-Debug "Registering new service type..."
        $tmpOut = $smoManService.RegisterServiceType($guid, $systemName, $displayName, $description, $serviceBrokerAssembly, $className);
        write-debug "Registered new service type..."
    } else {
        Write-Debug "Updating service type..."
        $tmpOut = $smoManService.UpdateServiceType($guid, $systemName, $displayName, $description, $serviceBrokerAssembly, $className);
        Write-Debug "Updated service type..."
    }
    $smoManService.Connection.Close();
    write-host "Deployed service-type"
}

$targetPath = GetK2InstallPath
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

if (GetK2Version -ge 5) {
    StopK2FiveService -server localhost 
}
else {
    StopK2Service -server localhost 
}


Copy-Item $scriptPath\* -Include K2Field.K2NE.ServiceBroker.dll -Destination "$targetPath\ServiceBroker"

if (GetK2Version -ge 5) {
    StartK2FiveService -server localhost 
}
else {
    StartK2Service -server localhost 
}


$k2constring = GetK2ConnectionString 
RegisterServiceType -k2ConnectionString $k2constring -ServiceTypeDLL $brokerDLL -guid $guid -systemName $brokerSystemName -displayName $displayName -description $brokerDescription
