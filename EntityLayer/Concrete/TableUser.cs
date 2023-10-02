using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableUser : IDatabaseEntity
{ 
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public DateTime UserCreationDate { get; set; }

    public string UserMail { get; set; } = null!;

    public string? UserIpAdress { get; set; }

    public string? UserPp { get; set; }

    public string? UserBio { get; set; }

    public bool? UserActive { get; set; }

    public int UserPoint { get; set; }

    public byte UserWarning { get; set; }

    public virtual ICollection<TableCommentLike> TableCommentLikes { get; } = new List<TableCommentLike>();

    public virtual ICollection<TableComment> TableComments { get; } = new List<TableComment>();

    public virtual ICollection<TableFollow> TableFollows { get; } = new List<TableFollow>();

    public virtual ICollection<TableLike> TableLikes { get; } = new List<TableLike>();

    public virtual ICollection<TableNotification> TableNotifications { get; } = new List<TableNotification>();

    public virtual ICollection<TableReportComment> TableReportComments { get; } = new List<TableReportComment>();

    public virtual ICollection<TableReport> TableReports { get; } = new List<TableReport>();

    public virtual ICollection<TableTopic> TableTopics { get; } = new List<TableTopic>();
}
