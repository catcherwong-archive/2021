using System;
using System.Collections.Generic;

namespace Common
{
    public class Message
    {
        public string InqId { get; set; }
        public long SendTime { get; set; }
        public string SenderId { get; set; }
        public sbyte SenderRole { get; set; }
        public sbyte MsgType { get; set; }
        public string MsgBody { get; set; }
        public string ExtInfo { get; set; }

        public static List<Message> BuildMessages(string inqId, int count)
        {
            var list = new List<Message>();

            var inqTime = DateTimeOffset.Now.AddDays(-new Random().Next(1, 500));

            var docId = Guid.NewGuid().ToString();
            var pId = Guid.NewGuid().ToString("N");

            for (int i = 0; i < count; i++)
            {
                var time = inqTime.AddSeconds(10 * (i + 1));
                var message = new Message
                {
                    InqId = inqId,
                    SendTime = time.ToUnixTimeMilliseconds(),
                    MsgType = (sbyte)1,
                    MsgBody = Tools.GenMessage(new Random().Next(5, 10)),
                    SenderId = i % 2 == 0 ? pId : docId,
                    SenderRole = (sbyte)(i % 2),
                };

                list.Add(message);
            }

            return list;
        }

        public static Message BuildMessage(Cassandra.Row row)
        {
            // 解析从 cassandra 中返回的行
            var inq_id = row.GetValue<string>("inq_id");
            var send_time = row.GetValue<long>("send_time");
            var msg_body = row.GetValue<string>("msg_body");
            var msg_type = row.GetValue<sbyte>("msg_type");
            var sender_id = row.GetValue<string>("sender_id");
            var sender_role = row.GetValue<sbyte>("sender_role");

            return new Message
            {
                InqId = inq_id,
                ExtInfo = string.Empty,
                MsgBody = msg_body,
                MsgType = msg_type,
                SenderId = sender_id,
                SenderRole = sender_role,
                SendTime = send_time,
            };
        }
    }
}
