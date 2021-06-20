using Cassandra;
using System.Collections.Generic;

namespace Common
{
    public class CassDbContext
    {
        private static ISession _session;

        public static ISession GetSession()
        {
            if (_session != null) return _session;

            var cluster = Cluster.Builder()
                                    .AddContactPoints("127.0.0.1")
                                    .WithDefaultKeyspace("messaging")
                                    .Build();


            // CREATE KEYSPACE IF NOT EXISTS messaging WITH replication = { 'class': 'SimpleStrategy', 'replication_factor' : 1};
            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists(new Dictionary<string, string>
            {
                { "class", "SimpleStrategy" },
                { "replication_factor", "1"}
            });

            return _session = session;
        }

    }
}
