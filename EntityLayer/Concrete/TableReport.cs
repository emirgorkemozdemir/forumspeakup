using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableReport : IDatabaseEntity
{
    public int ReportId { get; set; }

    public int Reporter { get; set; }

    public int Reporting { get; set; }

    public bool? ReportActive { get; set; }

    public DateTime? ReportDate { get; set; }

    public virtual TableUser ReporterNavigation { get; set; } = null!;

    public virtual TableTopic ReportingNavigation { get; set; } = null!;
}
