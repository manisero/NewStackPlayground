Param(
  [string]$dumpPath
)

$scriptFolderPath = $PSScriptRoot
$dbName = "Schema_dump"

if ([string]::IsNullOrEmpty($dumpPath))
{
    $dumpPath = "$scriptFolderPath/dump.sql"
}

Write-Host "Creating database '$dbName'..." -ForegroundColor Yellow
createdb $dbName

Write-Host "Running DbMigrator..." -ForegroundColor Yellow
dotnet run --project "$scriptFolderPath/../src/NewStackPlayground.DbMigrator" -- $dbName

Write-Host "Creating dump in '$dumpPath'..." -ForegroundColor Yellow
pg_dump --no-owner --no-privileges --no-acl --schema-only -f $dumpPath $dbName

Write-Host "Dropping database '$dbName'..." -ForegroundColor Yellow
dropdb $dbName
