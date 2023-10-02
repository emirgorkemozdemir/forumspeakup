using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableComment : IDatabaseEntity
{
    public int CommentId { get; set; }

    public string CommentContent { get; set; } = null!;

    public string CommentVoiceLink { get; set; } = null!;

    public int? CommentLike { get; set; }

    public int CommentTopicId { get; set; }

    public DateTime CommentDate { get; set; }

    public string CommentSharerIp { get; set; } = null!;

    public bool? CommentActive { get; set; }

    public int CommentSenderId { get; set; }

    public virtual TableUser CommentSender { get; set; } = null!;

    public virtual ICollection<TableCommentLike> TableCommentLikes { get; } = new List<TableCommentLike>();

    public virtual ICollection<TableReportComment> TableReportComments { get; } = new List<TableReportComment>();
}
