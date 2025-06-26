//VoucherService.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniAccountManagementSystem.Models;
using System.Collections.Generic;
using System.Data;

public class VoucherService
{
    private readonly IConfiguration _config;

    public VoucherService(IConfiguration config)
    {
        _config = config;
    }

    private string ConnectionString => _config.GetConnectionString("DefaultConnection");

    // Save voucher using stored procedure and table-valued parameter
    public void SaveVoucher(VoucherModel voucher)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand("sp_SaveVoucher", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Date", voucher.Date);
        cmd.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
        cmd.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);

        var tvp = new DataTable();
        tvp.Columns.Add("AccountId", typeof(int));
        tvp.Columns.Add("Debit", typeof(decimal));
        tvp.Columns.Add("Credit", typeof(decimal));

        foreach (var entry in voucher.Entries)
        {
            tvp.Rows.Add(entry.AccountId, entry.Debit, entry.Credit);
        }

        var tvpParam = new SqlParameter("@Entries", SqlDbType.Structured)
        {
            TypeName = "VoucherEntryType",
            Value = tvp
        };

        cmd.Parameters.Add(tvpParam);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    // Fetch all vouchers with their associated entries
    public List<VoucherWithEntries> GetAllVouchers()
    {
        var result = new List<VoucherWithEntries>();

        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        string sql = @"
        SELECT v.Id, v.ReferenceNo, v.VoucherType, v.Date,
               e.AccountId, e.Debit, e.Credit,
               a.Name AS AccountName
        FROM Vouchers v
        INNER JOIN VoucherEntries e ON v.Id = e.VoucherId
        INNER JOIN ChartOfAccounts a ON e.AccountId = a.Id
        ORDER BY v.Id DESC, e.Id";

        using var cmd = new SqlCommand(sql, conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        Dictionary<int, VoucherWithEntries> dict = new();

        while (reader.Read())
        {
            int voucherId = (int)reader["Id"];

            if (!dict.ContainsKey(voucherId))
            {
                dict[voucherId] = new VoucherWithEntries
                {
                    Id = voucherId,
                    ReferenceNo = reader["ReferenceNo"].ToString() ?? "",
                    VoucherType = reader["VoucherType"].ToString() ?? "",
                    Date = (DateTime)reader["Date"],
                    Entries = new List<VoucherEntryDisplay>()
                };
            }

            dict[voucherId].Entries.Add(new VoucherEntryDisplay
            {
                AccountId = (int)reader["AccountId"],
                AccountName = reader["AccountName"].ToString() ?? "",
                Debit = (decimal)reader["Debit"],
                Credit = (decimal)reader["Credit"]
            });
        }

        return dict.Values.ToList();
    }
}
