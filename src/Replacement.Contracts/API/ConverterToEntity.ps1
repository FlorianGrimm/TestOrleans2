[string] $scriptlocation = $global:MyInvocation.MyCommand.Definition
Write-Host "scriptlocation $scriptlocation"

[string] $projectDirectory = [System.IO.Path]::GetDirectoryName([System.IO.Path]::GetDirectoryName($scriptlocation))
Write-Host "projectDirectory $projectDirectory"

$csproj = "$projectDirectory\Replacement.Contracts.csproj"
dotnet build -p:ExtraDefineConstants=NOConverterToAPI -p:OutDir=bin\Debug\NOConverterToAPI\ $csproj 

[string] $outputlocation=$scriptlocation.Substring(0, $scriptlocation.Length-4)+".cs"
Write-Host "outputlocation $outputlocation"

[string] $assemblylocation=[System.IO.Path]::Combine($projectDirector, "bin\Debug\NOConverterToAPI\Replacement.Contracts.dll")
Write-Host "assemblylocation $assemblylocation"

[System.Reflection.Assembly] $assembly = [System.Reflection.Assembly]::LoadFrom($assemblylocation)

# 
[System.Type[]] $types = $assembly.GetTypes()

$typesEntity = $types | Where-Object {$_.Namespace -eq "Replacement.Contracts.Entity"}
$typesEntity = $typesEntity | Where-Object {$_.Name.EndsWith("Entity") }
if ($false){
    Write-Host "typesEntity"
    $typesEntity | ForEach-Object {
        [System.Type] $type = $_     
        Write-Host $type.FullName
    }
}

$typesAPI = $types | Where-Object {$_.Namespace -eq "Replacement.Contracts.API"}
$typesAPI = $typesAPI | Where-Object {$_.Name.EndsWith("API") }
if ($false){
    Write-Host "typesAPI"
    $typesAPI | ForEach-Object {
        [System.Type] $type = $_     
        Write-Host $type.FullName
    }
}

[System.Text.StringBuilder] $output = [System.Text.StringBuilder]::new()


$output.AppendLine("#if NOConverterToAPI") | Out-Null
$output.AppendLine("#else") | Out-Null
$output.AppendLine("namespace Replacement.Contracts.API;") | Out-Null
$output.AppendLine("public static partial class ConverterToEntity {") | Out-Null
foreach ($typeEntity in $typesEntity) {
    [string] $nameEntity = $typeEntity.Name
    [string] $nameAPI = $nameEntity.Substring(0, $nameEntity.Length-6) + "API"

    [System.Type] $typeAPI = $typesAPI | Where-Object {$_.Name -eq $nameAPI} | Select-Object -First 1

    if ($null -ne $typeAPI) {
        #$typeEntity.GetProperties
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
            [PSCustomObject[]]$propertiesEntityAPI = $propertiesEntity | ForEach-Object {
                $propertyEntity = $_
                $propertyAPI = $propertiesAPI | Where-Object {$_.Name -eq $propertyEntity.Name } | Select-Object -First 1
                if ($null -ne $propertyAPI) {
                    [string] $propertyName = $propertyEntity.Name
                    [PSCustomObject]@{
                        PropertyName   = $propertyName
                        PropertyEntity = $propertyEntity
                        PropertyAPI    = $propertyAPI
                    }
                }
            }
            [int] $lastIdx = $propertiesEntityAPI.Length-1
            for ([int] $idx=0;$idx -le $lastIdx; $idx++){
                [PSCustomObject] $propertyEntityAPI = $propertiesEntityAPI[$idx]
                #[string] $suffix
                #(if ($idx -eq $lastIdx) { $suffix="" } else { $suffix="," })
                [string] $suffix = if ($idx -eq $lastIdx) { "" } else { "," }
                
                [string]$propertyName=$propertyEntityAPI.PropertyName
                $output.AppendLine("                $($propertyName): that.$($propertyName)$($suffix)") | Out-Null
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


<#
[System.Type] $typeIDataOperationRelated = [Replacement.Contracts.Entity.IDataOperationRelated]
$typesDataOperationRelated = $types | Where-Object {
    [System.Type] $type = $_ 
    if ($null -ne ($type.GetInterfaces() | Where-Object {$_ -eq $typeIDataOperationRelated})) {
        $type        
    }
}

$typesDataOperationRelated| ForEach-Object {
    [System.Type] $type = $_ 
    
    Write-Host $type.FullName
}
#>

# G:\github\FlorianGrimm\TestOrleans2\src\Replacement.Contracts\bin\Debug\net6.0\Replacement.Contracts.dll

#