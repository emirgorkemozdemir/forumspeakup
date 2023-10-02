using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableTopic : IDatabaseEntity
{
    public int TopicId { get; set; }

    public string TopicTitle { get; set; } = null!;

    public string? TopicContent { get; set; }

    public string? TopicVoiceLink { get; set; }

    public int? TopicLikes { get; set; }

    public int? TopicDislikes { get; set; }

    public int TopicCategory { get; set; }

    public int? TopicSubCategory { get; set; }

    public DateTime TopicDate { get; set; }

    public string TopicSharerIp { get; set; } = null!;

    public bool? TopicActive { get; set; }

    public int? TopicSharerId { get; set; }

    public virtual ICollection<TableLike> TableLikes { get; } = new List<TableLike>();

    public virtual ICollection<TableReport> TableReports { get; } = new List<TableReport>();

    public virtual ICollection<TableStat> TableStats { get; } = new List<TableStat>();

    public virtual TableCategory TopicCategoryNavigation { get; set; } = null!;

    public virtual TableUser? TopicSharer { get; set; }

    public virtual TableSubCategory? TopicSubCategoryNavigation { get; set; }
}
