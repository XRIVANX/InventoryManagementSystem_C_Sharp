
ALTER PROCEDURE sp_PostPhysicalCount
    @CountID INT, @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Status nvarchar(10);
        SELECT @Status=Status FROM PhysicalCount WITH (UPDLOCK) WHERE CountID=@CountID;
        IF @Status IS NULL THROW 53003, 'Physical count not found.', 1;
        IF @Status <> 'OPEN'
            THROW 53001, 'Only an OPEN count can be posted.', 1;

        IF EXISTS (SELECT 1 FROM PhysicalCountLine WHERE CountID = @CountID AND CountedQty IS NULL)
            THROW 53002, 'Every line must have a counted quantity before posting.', 1;

        IF NOT EXISTS (SELECT 1 FROM PhysicalCountLine WHERE CountID = @CountID AND Variance <> 0)
        BEGIN
            UPDATE PhysicalCount SET Status='POSTED', PostedBy=@UserID, PostedAt=GETDATE()
            WHERE CountID = @CountID;
            COMMIT TRANSACTION;
            RETURN;   -- no variance, nothing to adjust
        END

        DECLARE @No NVARCHAR(30), @wh INT, @txn INT;
        EXEC sp_NextDocNo 'ADJ', @No OUTPUT;
        SELECT @wh = WarehouseID FROM PhysicalCount WHERE CountID = @CountID;

        INSERT INTO StockTransaction (TransactionNo, TransactionType, Reason, Remarks, CreatedBy)
        VALUES (@No, 'ADJUST', 'Physical count variance',
                'Auto-generated from count ' + (SELECT CountNo FROM PhysicalCount WHERE CountID=@CountID),
                @UserID);
        SET @txn = SCOPE_IDENTITY();

        INSERT INTO StockTransactionLine (TransactionID, ProductID, BatchID, ToWarehouseID, Quantity, UnitCost)
        SELECT @txn, l.ProductID, l.BatchID, @wh, l.Variance, p.AverageCost
        FROM PhysicalCountLine l
        JOIN Products p ON p.ProductID = l.ProductID
        WHERE l.CountID = @CountID AND l.Variance <> 0;

        EXEC sp_PostTransaction @txn, @UserID;

        UPDATE PhysicalCount
           SET Status='POSTED', PostedBy=@UserID, PostedAt=GETDATE(), AdjustmentTxnID=@txn
        WHERE CountID = @CountID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
