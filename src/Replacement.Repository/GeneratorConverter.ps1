[string] $namecsproj = "Replacement.Contracts.csproj"

[string] $scriptlocation = $global:MyInvocation.MyCommand.Definition
Write-Host "scriptlocation $scriptlocation"

[string] $projectRepositoryDirectory = [System.IO.Path]::GetDirectoryName($scriptlocation)
Write-Host "projectRepositoryDirectory $projectRepositoryDirectory"

[string] $projectContractsDirectory = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($projectRepositoryDirectory), "Replacement.Contracts")
Write-Host "projectContractsDirectory $projectContractsDirectory"

[string] $fullNameContractscsproj = "$($projectContractsDirectory)\$($namecsproj)"
dotnet build -p:ExtraDefineConstants=NOConverterToAPI -p:OutDir=bin\Debug\NOConverterToAPI\ $fullNameContractscsproj
if (-not $?) {
    Write-Host "stopping"
} else {

    [string] $assemblylocation=[System.IO.Path]::Combine($projectContractsDirectory, "bin\Debug\NOConverterToAPI\Replacement.Contracts.dll")
    Write-Host "assemblylocation $assemblylocation"
#
    [System.Reflection.Assembly] $assembly = [System.Reflection.Assembly]::LoadFrom($assemblylocation)
    [System.Type[]] $types = $assembly.GetTypes()


    [System.Type] $IDataEntity = [Replacement.Contracts.Entity.IDataEntity]
    [System.Type] $IOperationRelatedEntity = [Replacement.Contracts.Entity.IOperationRelatedEntity]
    [System.Type] $IHistoryEntity = [Replacement.Contracts.Entity.IHistoryEntity]
    $typesEntity = $types | Where-Object {$_.IsClass -and $_.Namespace -eq "Replacement.Contracts.Entity"}
    
    $typesEntity = $typesEntity | Where-Object {$_.GetInterfaces().Contains($IDataEntity) -or $_.GetInterfaces().Contains($IOperationRelatedEntity) -or $_.GetInterfaces().Contains($IHistoryEntity) }
    if ($false) {
        Write-Host "typesEntity"
        $typesEntity | ForEach-Object {
            [System.Type] $type = $_     
            Write-Host $type.FullName
        }
    }

    [System.Type] $IDataAPI = [Replacement.Contracts.API.IDataAPI]
    [System.Type] $IOperationRelatedAPI = [Replacement.Contracts.API.IOperationRelatedAPI]
    [System.Type] $IHistoryAPI = [Replacement.Contracts.API.IHistoryAPI]
    $typesAPI = $types | Where-Object {$_.IsClass -and $_.Namespace -eq "Replacement.Contracts.API"}
    $typesAPI = $typesAPI | Where-Object { $_.GetInterfaces().Contains($IDataAPI) -or $_.GetInterfaces().Contains($IOperationRelatedAPI) -or $_.GetInterfaces().Contains($IHistoryAPI) }
    #$typesAPI = $typesAPI | Where-Object {$_.Name.EndsWith("API") }
    if ($false) {
        Write-Host "typesAPI"
        $typesAPI | ForEach-Object {
            [System.Type] $type = $_     
            Write-Host $type.FullName
        }
    }

    [System.Collections.Hashtable] $hsTypeAPI = New-Object "System.Collections.Hashtable"
    [System.Collections.Hashtable] $hsTypeEntity = New-Object "System.Collections.Hashtable"
    [System.Collections.Hashtable] $hsTypeEntityToAPI = New-Object "System.Collections.Hashtable"
    [System.Collections.Hashtable] $hsTypeAPIToEntity = New-Object "System.Collections.Hashtable"
    foreach($typeEntity in $typesEntity ){
        $hsTypeEntity.Add($typeEntity.Name, $typeEntity)
    }
    foreach($typeAPI in $typesAPI){
        $hsTypeAPI.Add($typeAPI.Name, $typeAPI)
        $typeEntity=$hsTypeEntity[$typeAPI.Name+"Entity"]
        if ($null -ne $typeEntity){
            $hsTypeEntityToAPI[$typeEntity]=$typeAPI
            $hsTypeAPIToEntity[$typeAPI]=$typeEntity
        }
    }

    $hsMappingProperty=New-Object "System.Collections.Hashtable"
    #$hsMappingProperty["DataVersion"] = "EntityVersion"
    $hsMappingProperty["EntityVersion"] = "DataVersion"
    #
    function mapProperties() {
        param(
            [System.Reflection.PropertyInfo[]] $propertiesAPI,
            [System.Reflection.PropertyInfo[]] $propertiesEntity
        )

        [PSCustomObject[]]$propertiesEntityAPI = $propertiesEntity | ForEach-Object {
            $propertyEntity = $_
            [string] $propertyNameAPI = $hsMappingProperty[$propertyEntity.Name]
            $propertyAPI = $propertiesAPI | Where-Object { ($_.Name -eq $propertyEntity.Name) -or ($_.Name -eq $propertyNameAPI)  } | Select-Object -First 1
            #
            if ($null -ne $propertyAPI) {
                [string] $ConvertToAPI = if ($propertyEntity.Name -eq "EntityVersion") { "DataVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToDataVersion(that.EntityVersion)" } else { "" }
                [string] $ConvertToEntity = if ($propertyEntity.Name -eq "EntityVersion") { "EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)" } else { "" }
                [PSCustomObject]@{
                    PropertyEntity  = $propertyEntity
                    PropertyAPI     = $propertyAPI
                    ConvertToAPI    = $ConvertToAPI
                    ConvertToEntity = $ConvertToEntity
                }
            }
        }
        $propertiesEntityAPI
    }


    #
    function buildConverterToEntity() {
        Write-Host "buildConverterToEntity"
        [string] $outputlocation=[System.IO.Path]::Combine($projectRepositoryDirectory, "Extensions\ConverterToEntity.cs")
        #
        Write-Host "outputlocation $outputlocation"
        #
            
        [System.Text.StringBuilder] $output = [System.Text.StringBuilder]::new()
        $output.AppendLine("#if NOConverterToAPI") | Out-Null
        $output.AppendLine("#else") | Out-Null
        $output.AppendLine("namespace Replacement.Contracts.API;") | Out-Null
        $output.AppendLine("public static partial class ConverterToEntity {") | Out-Null
        foreach ($typeEntity in $typesEntity) {
            [string] $nameEntity = $typeEntity.Name
            [System.Type] $typeAPI = $hsTypeEntityToAPI[$typeEntity]
            if ($null -ne $typeAPI) {
                [string] $nameAPI = $typeAPI.Name
                
                [System.Reflection.PropertyInfo[]] $propertiesEntity = $typeEntity.GetProperties()
                [System.Reflection.PropertyInfo[]] $propertiesAPI = $typeAPI.GetProperties()    
                if (($null -ne $propertiesEntity) -and ($null -ne $propertiesAPI)){
                    #
                    $output.AppendLine('    [return: NotNullIfNotNull("that")]') | Out-Null
                    $output.AppendLine("    public static $($nameEntity)? To$($nameEntity)(this $($nameAPI)? that) {") | Out-Null
                    $output.AppendLine("        if (that is null) {") | Out-Null
                    $output.AppendLine("            return default;") | Out-Null
                    $output.AppendLine("        } else {") | Out-Null
                    $output.AppendLine("            return new $($nameEntity)(") | Out-Null
                    
                    [PSCustomObject[]]$propertiesMapped = mapProperties $propertiesAPI $propertiesEntity
                    [int] $lastIdx = $propertiesMapped.Length-1
                    for ([int] $idx=0;$idx -le $lastIdx; $idx++){
                        [PSCustomObject] $propertyMapped = $propertiesMapped[$idx]
                        #[string] $suffix
                        #(if ($idx -eq $lastIdx) { $suffix="" } else { $suffix="," })
                        [string] $suffix = if ($idx -eq $lastIdx) { "" } else { "," }
                        [string] $ConvertToEntity = $propertyMapped.ConvertToEntity

                        if (-not [System.String]::IsNullOrEmpty($ConvertToEntity)){
                            $output.AppendLine("                $($ConvertToEntity)$($suffix)") | Out-Null
                        } else {
                            $output.AppendLine("                $($propertyMapped.PropertyEntity.Name): that.$($propertyMapped.PropertyAPI.Name)$($suffix)") | Out-Null
                        }
                    }
                    $output.AppendLine("                );") | Out-Null
                    $output.AppendLine("        }") | Out-Null
                    $output.AppendLine("    }") | Out-Null
                    $output.AppendLine("") | Out-Null
                    $output.AppendLine("    public static List<$($nameEntity)> ToList$($nameEntity)(this IEnumerable<$($nameAPI)> that) {") | Out-Null
                    $output.AppendLine("        var result = new List<$($nameEntity)>();") | Out-Null
                    $output.AppendLine("        foreach (var item in that) { ") | Out-Null
                    $output.AppendLine("            result.Add(item.To$($nameEntity)());") | Out-Null
                    $output.AppendLine("        }") | Out-Null
                    $output.AppendLine("        return result;") | Out-Null
                    $output.AppendLine("    }") | Out-Null
                    $output.AppendLine("") | Out-Null

                }
            }
        }
        $output.AppendLine("}") | Out-Null
        $output.AppendLine("#endif") | Out-Null
        if($false){
            Write-Host "Output"
            Write-Host ($output.ToString())
        }

        [string] $oldContent=""
        if ([System.IO.File]::Exists($outputlocation)){
            $oldContent = [System.IO.File]::ReadAllText($outputlocation)
        }
        [string] $newContent=$output.ToString()
        if ($newContent -ne $oldContent) {
            [System.IO.File]::WriteAllText($outputlocation, $newContent) | Out-Null
        }

    }
    #
    function buildConverterToAPI() {
        Write-Host "buildConverterAPI"
        #[string] $outputlocation=[System.IO.Path]::Combine($projectContractsDirectory, "Entity\ConverterToAPI.cs")
        [string] $outputlocation=[System.IO.Path]::Combine($projectRepositoryDirectory, "Extensions\ConverterToAPI.cs")
        #
        Write-Host "outputlocation $outputlocation"
        #
        [System.Text.StringBuilder] $output = [System.Text.StringBuilder]::new()

        $output.AppendLine("#if NOConverterToAPI") | Out-Null
        $output.AppendLine("#else") | Out-Null
        $output.AppendLine("namespace Replacement.Contracts.Entity;") | Out-Null
        $output.AppendLine("public static partial class ConverterToAPI {") | Out-Null
        foreach ($typeEntity in $typesEntity) {
            [string] $nameEntity = $typeEntity.Name
            [System.Type] $typeAPI = $hsTypeEntityToAPI[$typeEntity]
            if ($null -ne $typeAPI) {
                [string] $nameAPI = $typeAPI.Name

                [System.Reflection.PropertyInfo[]] $propertiesEntity = $typeEntity.GetProperties()
                [System.Reflection.PropertyInfo[]] $propertiesAPI = $typeAPI.GetProperties()    
                if (($null -ne $propertiesEntity) -and ($null -ne $propertiesAPI)){
                    #
                    $output.AppendLine('    [return: NotNullIfNotNull("that")]') | Out-Null
                    $output.AppendLine("    public static $($nameAPI)? To$($nameAPI)(this $($nameEntity)? that) {") | Out-Null
                    $output.AppendLine("        if (that is null) {") | Out-Null
                    $output.AppendLine("            return default;") | Out-Null
                    $output.AppendLine("        } else {") | Out-Null
                    $output.AppendLine("            return new $($nameAPI)(") | Out-Null
                    
                    [PSCustomObject[]] $propertiesMapped = mapProperties $propertiesAPI $propertiesEntity

                    [int] $lastIdx = $propertiesMapped.Length-1
                    for ([int] $idx=0;$idx -le $lastIdx; $idx++){
                        [PSCustomObject] $propertyMapped = $propertiesMapped[$idx]
                        #[string] $suffix
                        #(if ($idx -eq $lastIdx) { $suffix="" } else { $suffix="," })
                        [string] $suffix = if ($idx -eq $lastIdx) { "" } else { "," }

                        [string] $ConvertToAPI = $propertyMapped.ConvertToAPI
                        if (-not [System.String]::IsNullOrEmpty($ConvertToAPI)){
                            $output.AppendLine("                $($ConvertToAPI)$($suffix)") | Out-Null
                        } else {
                            $output.AppendLine("                $($propertyMapped.PropertyAPI.Name): that.$($propertyMapped.PropertyEntity.Name)$($suffix)") | Out-Null
                        }
                        
                    }
                    $output.AppendLine("                );") | Out-Null
                    $output.AppendLine("        }") | Out-Null
                    $output.AppendLine("    }") | Out-Null
                    $output.AppendLine("") | Out-Null
                    $output.AppendLine("    public static List<$($nameAPI)> ToList$($nameAPI)(this IEnumerable<$($nameEntity)> that) {") | Out-Null
                    $output.AppendLine("        var result = new List<$($nameAPI)>();") | Out-Null
                    $output.AppendLine("        foreach (var item in that) { ") | Out-Null
                    $output.AppendLine("            result.Add(item.To$($nameAPI)());") | Out-Null
                    $output.AppendLine("        }") | Out-Null
                    $output.AppendLine("        return result;") | Out-Null
                    $output.AppendLine("    }") | Out-Null
                    $output.AppendLine("") | Out-Null
                }
            }
        }
        $output.AppendLine("}") | Out-Null
        $output.AppendLine("#endif") | Out-Null
        if($false){
            Write-Host "Output"
            Write-Host ($output.ToString())
        }

        [string] $oldContent=""
        if ([System.IO.File]::Exists($outputlocation)){
            $oldContent = [System.IO.File]::ReadAllText($outputlocation)
        }
        [string] $newContent=$output.ToString()
        if ($newContent -ne $oldContent) {
            [System.IO.File]::WriteAllText($outputlocation, $newContent) | Out-Null
        }

        #
    }
    #
    buildConverterToAPI
    buildConverterToEntity
    #
}