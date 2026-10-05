using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using InventorySystem.Data;
using InventorySystem.Forms.Trans;

// Existing-record checks are read-only; removal checks use a rolled-back fixture.
internal static class SavedDraftsVerification
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static readonly Assembly App = typeof(FrmStockIn).Assembly;

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            Session.RoleName = "Manager";
            VerifyRestore(new FrmStockIn(), "IN");
            VerifyRestore(new FrmStockOut(), "OUT");
            VerifyRestore(new FrmTransfer(), "TRANSFER");
            VerifyRestore(new FrmAdjustment(), "ADJUST");
            VerifyGridRefresh();
            if (Array.IndexOf(args, "--database") >= 0) VerifyDatabase();
            if (Array.IndexOf(args, "--remove-database") >= 0) VerifyRemoval();
            Console.WriteLine("PASS: Saved Drafts verification.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static object Field(Form form, string name) => form.GetType().GetField(name, Private).GetValue(form);
    private static void Call(Form form, string name, params object[] args) => form.GetType().GetMethod(name, Private).Invoke(form, args);
    private static void Assert(bool value, string message) { if (!value) throw new Exception(message); }

    private static void Bind(Form form, string control, string idColumn, string nameColumn)
    {
        var combo = (ComboBox)Field(form, control);
        var table = new DataTable();
        table.Columns.Add(idColumn, typeof(int)); table.Columns.Add(nameColumn, typeof(string));
        table.Rows.Add(1, "Active entry");
        combo.DisplayMember = nameColumn; combo.ValueMember = idColumn; combo.DataSource = table;
        combo.SelectedIndex = -1;
    }

    private static void VerifyRestore(Form form, string type)
    {
        using (form)
        {
            form.BindingContext = new BindingContext();
            Call(form, "InitLinesTable");
            if (type == "TRANSFER")
            {
                Bind(form, "cboFromWarehouse", "WarehouseID", "WarehouseName");
                Bind(form, "cboToWarehouse", "WarehouseID", "WarehouseName");
            }
            else Bind(form, "cboWarehouse", "WarehouseID", "WarehouseName");
            if (type == "IN") Bind(form, "cboSupplier", "SupplierID", "SupplierName");

            var header = new DataTable();
            header.Columns.Add("TransactionID", typeof(int)); header.Columns.Add("TransactionNo");
            header.Columns.Add("TransactionDate", typeof(DateTime)); header.Columns.Add("ReferenceNo");
            header.Columns.Add("Remarks"); header.Columns.Add("SupplierID", typeof(int));
            header.Columns.Add("SupplierName"); header.Columns.Add("Reason");
            header.Rows.Add(42, "DRAFT-42", new DateTime(2026, 9, 20), "REF-42", "Saved remarks", 7, "Inactive supplier", "Custom reason");
            var lines = new DataTable();
            lines.Columns.Add("ProductID", typeof(int)); lines.Columns.Add("SKU"); lines.Columns.Add("ProductName");
            lines.Columns.Add("BatchID", typeof(int)); lines.Columns.Add("BatchNo");
            lines.Columns.Add("Quantity", typeof(decimal)); lines.Columns.Add("UnitCost", typeof(decimal));
            lines.Columns.Add("FromWarehouseID", typeof(int)); lines.Columns.Add("FromWarehouseName");
            lines.Columns.Add("ToWarehouseID", typeof(int)); lines.Columns.Add("ToWarehouseName");
            lines.Rows.Add(11, "SKU-11", "First product", DBNull.Value, "-", type == "ADJUST" ? -3m : 3m, 5m, 8, "Saved source", 9, "Saved destination");
            lines.Rows.Add(12, "SKU-12", "Second product", 6, "BATCH-6", 2m, 10m, 8, "Saved source", 9, "Saved destination");
            var draftType = App.GetType("InventorySystem.Services.SavedDraft");
            var draft = Activator.CreateInstance(draftType);
            draftType.GetProperty("Header").SetValue(draft, header.Rows[0]);
            draftType.GetProperty("Lines").SetValue(draft, lines);
            Call(form, "RestoreSavedDraft", draft);

            Assert((int)form.GetType().GetProperty("TransactionId").GetValue(form) == 42, type + ": original ID restored");
            Assert(((Label)Field(form, "lblTxnNo")).Text == "DRAFT-42", type + ": document number restored");
            Assert(((TextBox)Field(form, "txtRemarks")).Text == "Saved remarks", type + ": remarks restored");
            Assert(((TextBox)Field(form, "txtReferenceNo")).Text == "REF-42", type + ": reference restored");
            Assert(((DateTimePicker)Field(form, "dtpDate")).Value == new DateTime(2026, 9, 20), type + ": date restored");
            var restored = (DataTable)Field(form, "_lines");
            Assert(restored.Rows.Count == 2 && restored.Rows[0].IsNull("BatchID") && (int)restored.Rows[1]["BatchID"] == 6,
                type + ": batch and non-batch lines restored");
            Assert((decimal)restored.Rows[0]["Quantity"] == (type == "ADJUST" ? -3m : 3m), type + ": quantity sign preserved");
            Assert((decimal)restored.Rows[0]["LineTotal"] == (type == "ADJUST" ? -15m : 15m), type + ": totals computed");
            string warehouse = type == "TRANSFER" ? "cboFromWarehouse" : "cboWarehouse";
            Assert(Convert.ToInt32(((ComboBox)Field(form, warehouse)).SelectedValue) == (type == "IN" || type == "ADJUST" ? 9 : 8),
                type + ": saved warehouse selected, including inactive entries");
            if (type == "TRANSFER") Assert(Convert.ToInt32(((ComboBox)Field(form, "cboToWarehouse")).SelectedValue) == 9, "Transfer destination restored");
            if (type == "IN") Assert(Convert.ToInt32(((ComboBox)Field(form, "cboSupplier")).SelectedValue) == 7, "Saved supplier restored");
            if (type == "OUT" || type == "ADJUST") Assert(((ComboBox)Field(form, "cboReason")).Text == "Custom reason", type + ": saved reason restored");
            var changes = Field(form, "_draftChanges");
            var dirty = changes.GetType().GetProperty("IsDirty");
            Assert(!(bool)dirty.GetValue(changes), type + ": restored draft starts saved");
            Assert(((Button)Field(form, "btnSaveDraft")).Enabled && ((Button)Field(form, "btnPost")).Enabled,
                type + ": saved draft can be edited and posted");
            ((TextBox)Field(form, "txtRemarks")).Text = "Edited remarks";
            Assert((bool)dirty.GetValue(changes), type + ": edits require resaving");
            Console.WriteLine("PASS: " + type + " restores header, warehouses, lines, totals, and edit state.");
        }
    }

    private static void VerifyGridRefresh()
    {
        using (var list = new FrmSavedDrafts())
        {
            list.BindingContext = new BindingContext();
            var handle = list.Handle;
            var grid = (DataGridView)Field(list, "grid");
            var gridHandle = grid.Handle;
            var data = new DataTable();
            data.Columns.Add("TransactionID", typeof(int)); data.Columns.Add("TransactionType");
            data.Columns.Add("Document No"); data.Columns.Add("Date", typeof(DateTime));
            data.Rows.Add(1, "OUT", "DRAFT-1", DateTime.Today);
            data.Rows.Add(2, "OUT", "DRAFT-2", DateTime.Today);
            Call(list, "DisplayDrafts", data);
            grid.CurrentCell = grid.Rows[0].Cells["Document No"];
            Application.DoEvents();
            var remove = (Button)Field(list, "remove");
            Assert(remove.Enabled, "Selection enables removal after the grid settles");
            bool checkedSelectionEvent = false;
            grid.CurrentCellChanged += (s, e) =>
            {
                if (grid.CurrentRow == null)
                {
                    checkedSelectionEvent = true;
                    Assert(remove.Enabled, "Button state must not change inside current-cell events");
                }
            };
            grid.CurrentCell = null;
            Assert(checkedSelectionEvent, "Selection-clearing regression exercised");
            Assert(remove.Enabled, "Selection action update is deferred");
            Application.DoEvents();
            Assert(!remove.Enabled, "No selection disables removal after event processing");
            var remaining = data.Copy(); remaining.Rows.RemoveAt(0);
            Call(list, "DisplayDrafts", remaining);
            grid.CurrentCell = grid.Rows[0].Cells["Document No"];
            Application.DoEvents();
            Assert(grid.Rows.Count == 1 && remove.Enabled, "Removing a draft leaves the next draft selectable");
            Call(list, "DisplayDrafts", data.Clone());
            Application.DoEvents();
            Assert(grid.Rows.Count == 0 && !remove.Enabled, "Removing the last draft disables removal safely");
            Console.WriteLine("PASS: list refresh defers button state changes and handles removal of the last draft.");
        }
    }

    private static object Service(string name, params object[] args) => App.GetType("InventorySystem.Services.DraftService").GetMethod(name).Invoke(null, args);

    private static void VerifyDatabase()
    {
        var all = (DataTable)Service("Search", "", "");
        using (var list = new FrmSavedDrafts())
        {
            list.BindingContext = new BindingContext();
            typeof(Form).GetMethod("OnLoad", Private).Invoke(list, new object[] { EventArgs.Empty });
            var grid = (DataGridView)Field(list, "grid");
            Assert(((DataTable)grid.DataSource).Rows.Count == all.Rows.Count, "Saved Drafts screen lists all accessible drafts");
            if (all.Rows.Count > 0)
            {
                int requestedId = 0; string requestedType = null;
                list.OpenDraftRequested += (id, type) => { requestedId = id; requestedType = type; };
                grid.CurrentCell = grid.Rows[0].Cells["Document No"];
                Assert(((Button)Field(list, "remove")).Enabled, "Selected draft enables removal");
                Call(list, "OpenSelectedDraft");
                Assert(requestedId == Convert.ToInt32(all.Rows[0]["TransactionID"]) && requestedType == all.Rows[0]["TransactionType"].ToString(),
                    "Open Draft passes the selected document to its editor");
            }
            ((TextBox)Field(list, "search")).Text = "NO-SUCH-DRAFT-VERIFICATION";
            list.RefreshDrafts();
            Assert(((DataTable)grid.DataSource).Rows.Count == 0 && !((Button)Field(list, "open")).Enabled,
                "Empty search disables Open Draft");
            Assert(!((Button)Field(list, "remove")).Enabled, "Empty search disables Remove Draft");
        }
        foreach (DataRow row in all.Rows)
        {
            int id = Convert.ToInt32(row["TransactionID"]);
            string type = row["TransactionType"].ToString();
            var draft = Service("Load", id, type);
            var header = (DataRow)draft.GetType().GetProperty("Header").GetValue(draft);
            Assert(header["Status"].ToString() == "DRAFT", "Only drafts can reopen");
            var byNumber = (DataTable)Service("Search", row["Document No"].ToString(), type);
            Assert(byNumber.Select("TransactionID=" + id).Length == 1, "Search and type filter find original draft");
            using (var form = CreateEditor(type, id))
            {
                form.BindingContext = new BindingContext();
                Call(form, form.GetType().Name + "_Load", null, EventArgs.Empty);
                Assert((int)form.GetType().GetProperty("TransactionId").GetValue(form) == id, "Existing draft reopened in editor");
                Assert(((DataTable)Field(form, "_lines")).Rows.Count > 0, "Existing draft lines loaded");
            }
        }
        ExpectRejected(-1, "IN");
        if (all.Rows.Count > 0)
        {
            var row = all.Rows[0];
            ExpectRejected(Convert.ToInt32(row["TransactionID"]), row["TransactionType"].ToString() == "IN" ? "OUT" : "IN");
        }
        var posted = DbHelper.GetData("SELECT TOP 1 TransactionID, TransactionType FROM StockTransaction WHERE Status='POSTED'");
        if (posted.Rows.Count > 0) ExpectRejected(Convert.ToInt32(posted.Rows[0]["TransactionID"]), posted.Rows[0]["TransactionType"].ToString());
        Session.RoleName = "Staff";
        Assert(((DataTable)Service("Search", "", "")).Select("TransactionType='ADJUST'").Length == 0, "Staff cannot browse adjustments");
        Console.WriteLine("PASS: read-only database search and reopen checks (" + all.Rows.Count + " existing drafts), unavailable/wrong-type/posted rejection, and staff filtering.");
    }

    private static void VerifyRemoval()
    {
        using (var cn = DbHelper.GetConnection())
        {
            cn.Open();
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    Session.RoleName = "Manager";
                    int id;
                    using (var cmd = new SqlCommand(@"
                        INSERT INTO StockTransaction (TransactionNo, TransactionType, TransactionDate, Status, CreatedBy)
                        SELECT TOP 1 @no, 'OUT', GETDATE(), 'DRAFT', CreatedBy FROM StockTransaction;
                        SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@no", "VERIFY-" + Guid.NewGuid().ToString("N").Substring(0, 20));
                        var result = cmd.ExecuteScalar();
                        Assert(result != DBNull.Value, "Removal fixture needs an existing transaction creator");
                        id = Convert.ToInt32(result);
                    }
                    ExecuteFixture(cn, tx, id, @"
                        INSERT INTO StockTransactionLine (TransactionID, ProductID, BatchID, FromWarehouseID, ToWarehouseID, Quantity, UnitCost)
                        SELECT TOP 1 @id, ProductID, BatchID, FromWarehouseID, ToWarehouseID, Quantity, UnitCost
                        FROM StockTransactionLine WHERE TransactionID<>@id;");
                    Assert(FixtureCount(cn, tx, id, "StockTransactionLine") == 1, "Removal fixture contains an item");
                    RemoveFixture(cn, tx, -1, true);
                    ExecuteFixture(cn, tx, id, "UPDATE StockTransaction SET Status='POSTED' WHERE TransactionID=@id");
                    RemoveFixture(cn, tx, id, true);
                    Assert(FixtureCount(cn, tx, id, "StockTransactionLine") == 1, "Posted transaction items preserved");
                    ExecuteFixture(cn, tx, id, "UPDATE StockTransaction SET Status='DRAFT', TransactionType='ADJUST' WHERE TransactionID=@id");
                    Session.RoleName = "Staff";
                    RemoveFixture(cn, tx, id, true);
                    Assert(FixtureCount(cn, tx, id, "StockTransaction") == 1, "Staff cannot remove adjustment drafts");
                    Session.RoleName = "Manager";
                    RemoveFixture(cn, tx, id, false);
                    Assert(FixtureCount(cn, tx, id, "StockTransaction") == 0 && FixtureCount(cn, tx, id, "StockTransactionLine") == 0,
                        "Draft header and items removed together");
                    RemoveFixture(cn, tx, id, true);
                    Console.WriteLine("PASS: removal deletes draft and items, rejects missing/posted drafts, and enforces adjustment permissions; fixture rolled back.");
                }
                finally { tx.Rollback(); }
            }
        }
    }

    private static void ExecuteFixture(SqlConnection cn, SqlTransaction tx, int id, string sql)
    {
        using (var cmd = new SqlCommand(sql, cn, tx))
        { cmd.Parameters.AddWithValue("@id", id); cmd.ExecuteNonQuery(); }
    }

    private static int FixtureCount(SqlConnection cn, SqlTransaction tx, int id, string table)
    {
        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM " + table + " WHERE TransactionID=@id", cn, tx))
        { cmd.Parameters.AddWithValue("@id", id); return Convert.ToInt32(cmd.ExecuteScalar()); }
    }

    private static void RemoveFixture(SqlConnection cn, SqlTransaction tx, int id, bool expectRejection)
    {
        try
        {
            App.GetType("InventorySystem.Services.DraftService").GetMethod("RemoveInTransaction", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { cn, tx, id });
        }
        catch (TargetInvocationException ex)
        { if (expectRejection && ex.InnerException is InvalidOperationException) return; throw; }
        Assert(!expectRejection, "Removal must reject this transaction");
    }

    private static Form CreateEditor(string type, int id)
    {
        switch (type)
        {
            case "IN": return new FrmStockIn(id);
            case "OUT": return new FrmStockOut(id);
            case "TRANSFER": return new FrmTransfer(id);
            case "ADJUST": return new FrmAdjustment(id);
            default: throw new Exception("Unsupported type " + type);
        }
    }

    private static void ExpectRejected(int id, string type)
    {
        try { Service("Load", id, type); }
        catch (TargetInvocationException ex) { if (ex.InnerException is InvalidOperationException) return; throw; }
        throw new Exception("Unavailable, posted, or mismatched draft was accepted");
    }
}
