﻿namespace NServiceBus.Router.Deduplication.Outbox
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    class SessionState
    {
        public long Lo { get; }
        public long Hi { get; }
        public string Table { get; }

        public long EpochSize => Hi - Lo;

        public SessionState(long lo, long hi, string table)
        {
            Lo = lo;
            Hi = hi;
            Table = table;
        }

        public bool Matches(long sequence)
        {
            return sequence >= Lo && sequence < Hi;
        }

        public Task CreateConstraint(SqlConnection conn, SqlTransaction trans)
        {
            var table = new OutboxTable(Table);
            return table.CreateConstraint(Lo, Hi, conn, trans);
        }

        public Task DropConstraint(SqlConnection conn, SqlTransaction trans)
        {
            var table = new OutboxTable(Table);
            return table.DropConstraint(Lo, Hi, conn, trans);
        }
    }
}