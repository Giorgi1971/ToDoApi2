using System;
namespace TodoApp.Api.Db.Entity
{
    public enum SendEmailRequestStatus
    {
        New, // =1 ,
        Sent, // =2,
        Failed // = 3  ასეც შეიძლება.
    }

    public class SendEmailRequestEntity
    {
        public long Id { get; set; }
        public string ToAddress { get; set; }
        public string Body { get; set; }
        public SendEmailRequestStatus Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? SentAt { get; set; }
    }
}

