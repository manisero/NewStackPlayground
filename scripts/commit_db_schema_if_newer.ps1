$scriptFolderPath = $PSScriptRoot
$dbName = "Schema_dump"

$dumpFolderPath = "$scriptFolderPath/../db_schema"
$dumpFilePath = "$dumpFolderPath/schema.sql"
$dbMigrationsFolderPath = "$scriptFolderPath/../src/NewStackPlayground.DbMigrations"

if (Test-Path $dumpFilePath)
{
    $schemaDumpTs = (Get-ItemProperty $dumpFilePath).LastWriteTimeUtc

    $migrationItems = @(Get-Item $dbMigrationsFolderPath) + (Get-ChildItem $dbMigrationsFolderPath -Recurse |? { $_.FullName -NotMatch "\\(bin|obj)" })
    $migrationTs = ($migrationItems | measure LastWriteTimeUtc -Maximum).Maximum

    if ($schemaDumpTs -gt $migrationTs)
    {
        Write-Host "Db schema is up to date." -ForegroundColor Yellow
        exit
    }
}

Write-Host "Exporting db schema to '$dumpFilePath'..." -ForegroundColor Yellow
[System.IO.Directory]::CreateDirectory($dumpFolderPath) | Out-Null
& "$scriptFolderPath/dump_db_schema.ps1" $dumpFilePath

Write-Host "Staging db schema for git commit..." -ForegroundColor Yellow
git add $dumpFilePath
