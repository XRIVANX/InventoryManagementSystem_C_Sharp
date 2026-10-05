# Run with Windows PowerShell (the SAP SDK requires .NET Framework).
param([string]$AssemblyDirectory)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
if (-not $AssemblyDirectory) { $AssemblyDirectory = Join-Path $projectRoot 'bin\PrintVerification' }
[void][Reflection.Assembly]::LoadFrom((Join-Path $AssemblyDirectory 'CrystalDecisions.CrystalReports.Engine.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $AssemblyDirectory 'CrystalDecisions.ReportAppServer.DataSetConversion.dll'))
$data = New-Object System.Data.DataSet 'InventoryReports'
$rows = New-Object System.Data.DataTable 'ExpiringItems'
foreach ($name in @('Warehouse','SKU','Product','Batch No')) { [void]$rows.Columns.Add($name,[string]) }
[void]$rows.Columns.Add('Expiry Date',[datetime])
[void]$rows.Columns.Add('Days Remaining',[int])
foreach ($name in @('Qty On Hand','Avg Cost','At-Risk Value')) { [void]$rows.Columns.Add($name,[decimal]) }
$data.Tables.Add($rows)
$context = New-Object System.Data.DataTable 'ReportContext'
[void]$context.Columns.Add('Title',[string]); [void]$context.Columns.Add('Filters',[string]); [void]$context.Columns.Add('GeneratedAt',[datetime])
$data.Tables.Add($context)
$data.WriteXmlSchema((Join-Path $projectRoot 'Reports\ExpiringItems.xsd'))
$report = New-Object CrystalDecisions.CrystalReports.Engine.ReportDocument
try {
    $report.Load((Join-Path $projectRoot 'Reports\InventoryValuation.rpt'))
    $client = $report.ReportClientDocument
    $definition = $client.ReportDefController.ReportDefinition
    $objects = $client.ReportDefController.ReportObjectController
    $heading = $definition.PageHeaderArea.Sections.Item(0)
    $details = $definition.DetailArea.Sections.Item(0)
    $footer = $definition.ReportFooterArea.Sections.Item(0)
    $labelPrototype = $heading.ReportObjects.Item(0).Clone($true)
    $fieldPrototype = $details.ReportObjects.Item(0).Clone($true)
    $summaryPrototype = $footer.ReportObjects.Item(0).Clone($true)
    foreach ($section in @($heading,$details,$footer)) {
        while ($section.ReportObjects.Count -gt 0) { $objects.Remove($section.ReportObjects.Item(0)) }
    }
    $client.DataDefController.FormulaFieldController.Remove(0)
    Write-Output 'Replacing the valuation data source...'
    $client.DatabaseController.RemoveTable('InventoryValuation')
    Write-Output 'Adding ExpiringItems data schema...'
    $sourceData = New-Object System.Data.DataSet 'ExpirySource'
    $sourceData.Tables.Add($rows.Copy())
    $client.DatabaseController.AddDataSource([CrystalDecisions.ReportAppServer.DataSetConversion.DataSetConverter]::Convert($sourceData))
    Write-Output 'Building report columns...'
    $dateFormula = New-Object CrystalDecisions.ReportAppServer.DataDefModel.FormulaFieldClass
    $dateFormula.Name = 'ExpiryDateDisplay'
    $dateFormula.Text = 'ToText({ExpiringItems.Expiry Date}, "yyyy-MM-dd")'
    [void]$client.DataDefController.FormulaFieldController.Add($dateFormula)
    $table = $client.DatabaseController.Database.Tables.Item(1)
    $weights = @(13,10,21,11,10,8,9,8,10)
    $left = 0
    for ($i=0; $i -lt $rows.Columns.Count; $i++) {
        $width = [int](14400*$weights[$i]/100)
        $label = $labelPrototype.Clone($true)
        $label.Name = 'ExpiryHeading' + $i
        $label.Paragraphs.Item(0).ParagraphElements.Item(0).Text = $rows.Columns[$i].ColumnName
        $label.Left = $left; $label.Width = $width-80
        $objects.Add($label,$heading,-1)
        $field = $fieldPrototype.Clone($true)
        $source = $table.DataFields.Item($i)
        $field.Name = 'ExpiryValue' + $i
        $field.DataSourceName = $source.FormulaForm
        $field.FieldValueType = $source.Type
        if ($i -eq 4) {
            $field.DataSourceName = '{@ExpiryDateDisplay}'
            $field.FieldValueType = $fieldPrototype.FieldValueType
        }
        $field.Left = $left; $field.Width = $width-80
        $objects.Add($field,$details,-1)
        $left += $width
    }
    $formula = New-Object CrystalDecisions.ReportAppServer.DataDefModel.FormulaFieldClass
    $formula.Name = 'ReportSummary'
    $formula.Text = '"Total quantity: " + ToText(Sum({ExpiringItems.Qty On Hand}), 2) + "     |     At-risk value: PHP " + ToText(Sum({ExpiringItems.At-Risk Value}), 2)'
    [void]$client.DataDefController.FormulaFieldController.Add($formula)
    $objects.Add($summaryPrototype,$footer,-1)
    $report.ReportDefinition.ReportObjects['Label1'].Text = 'INVENTORYSYSTEM  /  EXPIRING ITEMS'
    foreach ($object in $report.ReportDefinition.Sections['Section3'].ReportObjects) {
        $object.ObjectFormat.EnableCanGrow = $true
    }
    $report.SaveAs((Join-Path $projectRoot 'Reports\ExpiringItems.rpt'))
    Write-Output 'Created ExpiringItems.rpt and matching XSD using the SAP SDK.'
} finally { $report.Close(); $report.Dispose() }
