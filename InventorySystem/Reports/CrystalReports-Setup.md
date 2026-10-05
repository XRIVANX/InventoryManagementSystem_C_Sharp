# SAP Crystal Reports

Crystal Reports is enabled by default. Five genuine SAP templates are included: InventoryValuation.rpt, StockCard.rpt, LowStock.rpt, Movement.rpt, and ExpiringItems.rpt. Each uses the matching XSD schema and receives filtered data from the application; no database credentials or business records are saved in the templates.

## Use the reports

Run the Debug / Any CPU configuration on this computer. Open Reports from the top menu, choose a report and its filters, select Generate report, then Crystal preview. The SAP viewer supports export and printing. Changed filters require generating the report again.

## Runtime and builds

Insights → Expiring Items → Print opens the Crystal viewer with the displayed rows and their applied expiration, warehouse, and product filters. The ExpiringItems report includes batches, expiry dates, days remaining, quantities, average costs, and total at-risk value. Use the viewer toolbar to print or export. An empty result disables Print.

This computer has SAP's 64-bit runtime installed. Every Crystal-enabled build explicitly targets x64 with Prefer32Bit=false, including legacy configurations labeled x86. Prefer Debug / Any CPU in Visual Studio. Stop any existing debugging session before rebuilding so the older executable can be replaced.

The project copies all RPT and XSD files into the output Reports folder. Install the matching SAP runtime on computers where the application is deployed. If SDK references do not resolve, set CrystalAssemblyDirectory to the directory containing the Engine, Shared and Windows.Forms assemblies. EnableCrystalReports=false remains available for builds without the SDK.

## Verification

All four templates loaded using SAP ReportDocument, rendered page 1 in the actual CrystalReportViewer, and exported successfully from live InventoryDB data. SAP row counts matched the filtered data: valuation 5, stock card 2, low stock 3, movement 17. Landscape exports were rendered and visually checked for clipping. Valuation totaled 1,080.00 and the selected stock card closed at 10.00 during verification.

Database verification also passed stock-in/out, transfer, signed adjustment, batch balance, average cost, physical count and invalid posting checks. Temporary test records were rolled back. The physical-count procedure rejects a missing count through the included migration.

The reports preserve chronological stock-card order and signed movement adjustments. Low-stock reports include products with zero stock. Valuation uses current average cost. Report totals depend on the selected filters and current database records.


