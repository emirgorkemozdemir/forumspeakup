using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableStat : IDatabaseEntity
{
    public int StatId { get; set; }

    public DateTime? StatDate { get; set; }

    public int StatBestTopic { get; set; }

    public int StatDailyPageVisits { get; set; }

    public virtual TableTopic StatBestTopicNavigation { get; set; } = null!;
}
