using Cassandra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Init
{
    class Program
    {
        static readonly string CREATE_TABLE_SQL = @"
                CREATE TABLE IF NOT EXISTS messages(
                    inq_id text,
                    send_time bigint,
                    sender_id text,
                    sender_role tinyint,
                    msg_type tinyint,
                    msg_body text,
                    ext_info text,
                    PRIMARY KEY (inq_id, send_time)
                ) WITH CLUSTERING ORDER BY (send_time ASC)";

        static readonly string INSERT_SQL = @"
INSERT INTO messages(inq_id, send_time, sender_id, sender_role, msg_type, msg_body, ext_info)
VALUES (?, ?, ?, ?, ?, ?, ?)
";

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var session = Common.CassDbContext.GetSession();

            session.Execute(CREATE_TABLE_SQL);

            InitRandomData(session, 100000);

            Console.WriteLine("hello cassandra");
            Console.ReadKey();
        }


        static void InitRandomData(ISession session, int inqCount = 100)
        {

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 8
            };

            Parallel.For(0, inqCount, options, async x =>
            {
                // 问诊Id
                var inqId = Guid.NewGuid().ToString("N");

                // 会话数量
                var msgCount = new Random().Next(6, 20);

                var messages = Common.Message.BuildMessages(inqId, msgCount);

                var batch = new BatchStatement();

                foreach (var item in messages)
                {
                    batch.Add(session.Prepare(INSERT_SQL).Bind(item.InqId, item.SendTime, item.SenderId, item.SenderRole, item.MsgType, item.MsgBody, ""));
                }

                await session.ExecuteAsync(batch);

                Console.WriteLine(x);
            });
        }
    }
}
