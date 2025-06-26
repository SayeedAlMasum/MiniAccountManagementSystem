CREATE TABLE ChartOfAccounts (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    ParentId INT NULL
);

CREATE TABLE Vouchers (
    Id INT PRIMARY KEY IDENTITY,
    Date DATE,
    ReferenceNo NVARCHAR(50),
    VoucherType NVARCHAR(20)
);

CREATE TABLE VoucherEntries (
    Id INT PRIMARY KEY IDENTITY,
    VoucherId INT,
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
);

-- Stored Procedure
CREATE PROCEDURE sp_SaveVoucher
    @Date DATE,
    @ReferenceNo NVARCHAR(50),
    @VoucherType NVARCHAR(20),
    @Entries dbo.VoucherEntryType READONLY
AS
BEGIN
    DECLARE @VoucherId INT
    INSERT INTO Vouchers (Date, ReferenceNo, VoucherType)
    VALUES (@Date, @ReferenceNo, @VoucherType)

    SET @VoucherId = SCOPE_IDENTITY()

    INSERT INTO VoucherEntries (VoucherId, AccountId, Debit, Credit)
    SELECT @VoucherId, AccountId, Debit, Credit FROM @Entries
END

-- Table Type
CREATE TYPE VoucherEntryType AS TABLE
(
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
)
