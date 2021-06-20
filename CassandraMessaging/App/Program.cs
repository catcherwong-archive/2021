using System;

namespace App
{
    class Program
    {
        static readonly string GET_MSG_SQL = @"
SELECT * FROM messages WHERE inq_id = ?
";

        static void Main(string[] args)
        {
            var session = Common.CassDbContext.GetSession();

            // 问诊Id
            var inqId = "ca271601d20b4a5a9a006835213bc521";

            var stmt = session.Prepare(GET_MSG_SQL).Bind(inqId);

            var rowset = session.Execute(stmt);

            Console.WriteLine("患者\t\t\t\t\t医生");

            foreach (var row in rowset)
            {
                var msg = Common.Message.BuildMessage(row);

                if (msg.SenderRole == 0)
                {
                    Console.WriteLine($"{msg.MsgBody}\t\t\t\t\t");
                }
                else
                {
                    Console.WriteLine($"\t\t\t\t\t{msg.MsgBody}");
                }
            }

            Console.ReadKey();
        }
    }
}
