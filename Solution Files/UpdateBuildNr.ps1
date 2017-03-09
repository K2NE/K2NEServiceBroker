$newVersion = get-date -Format "yyyyMMddHHmm"

$AssemInfo = split-path -Path $PSScriptRoot -parent
$AssemInfo += "\K2Field.K2NE.ServiceBroker\Properties\AssemblyInfo.cs"
	
$TmpFile = $AssemInfo + ".tmp"
get-content $AssemInfo | %{$_ -replace 'AssemblyFileVersion\(\"([0-9]\.[0-9]\.[0-9]+)\.([0-9]*)\"\)',"AssemblyFileVersion(""`$1.$newVersion"")" } > $TmpFile


Move-item $TmpFile $AssemInfo -force
Write-host "updated assembly file"