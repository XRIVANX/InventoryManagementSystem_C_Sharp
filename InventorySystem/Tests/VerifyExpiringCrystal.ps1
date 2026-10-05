# Run with Windows PowerShell. Uses synthetic data; no database or printer writes.
param([string]$AssemblyDirectory)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
if (-not $AssemblyDirectory) { $AssemblyDirectory = Join-Path $projectRoot 'bin\CrystalExpiryVerification' }
[void][Reflection.Assembly]::LoadFrom((Join-Path $AssemblyDirectory 'InventorySystem.exe'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $AssemblyDirectory 'CrystalDecisions.CrystalReports.Engine.dll'))
$data = New-Object System.Data.DataSet
$data.ReadXmlSchema((Join-Path $projectRoot 'Reports\ExpiringItems.xsd'))
$rows = $data.Tables['ExpiringItems']
for ($i=0; $i -lt 85; $i++) {
    [void]$rows.Rows.Add('Main Warehouse','SKU-'+$i,'Batch tracked product with a longer descriptive name '+$i,
        'BATCH-'+$i,[datetime]'2026-10-20',14,[decimal]2,[decimal]15,[decimal]30)
}
[void]$data.Tables['ReportContext'].Rows.Add('Expiring Items','Expiring in 30 Days | Main Warehouse | All Products',[datetime]::Now)
$helper = [InventorySystem.Forms.Inquiry.FrmExpiringInquiry].Assembly.GetType('InventorySystem.Forms.Reports.CrystalReportWindow')
$mapped = $helper.GetMethod('CreateData').Invoke($null,@([InventorySystem.Services.InventoryReport]::ExpiringItems,$rows,'Expiring Items','Applied filter snapshot'))
if ($mapped.Tables[0].TableName -ne 'ExpiringItems' -or $mapped.Tables[0].Rows.Count -ne 85 -or $mapped.Tables['ReportContext'].Rows[0]['Filters'] -ne 'Applied filter snapshot') { throw 'Crystal dataset mapping failed.' }
$mapped.Dispose()
$report = New-Object CrystalDecisions.CrystalReports.Engine.ReportDocument
try {
    $report.Load((Join-Path $projectRoot 'Reports\ExpiringItems.rpt'))
    foreach ($table in $report.Database.Tables) { $table.SetDataSource($data.Tables[$table.Name]) }
    $report.ExportToDisk([CrystalDecisions.Shared.ExportFormatType]::PortableDocFormat,(Join-Path $AssemblyDirectory 'CrystalExpiryPreview.pdf'))
    Write-Output 'PASS: Crystal template loaded, mapped dataset and applied filters verified, and 85 synthetic rows exported.'
} finally { $report.Close(); $report.Dispose(); $data.Dispose() }
