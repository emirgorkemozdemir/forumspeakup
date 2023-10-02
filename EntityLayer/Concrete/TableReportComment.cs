using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableReportComment : IDatabaseEntity
{
    public int ReportCommentId { get; set; }

    public int ReportCommentReporter { get; set; }

    public int ReportCommentReporting { get; set; }

    public bool? ReportCommentActive { get; set; }

    public DateTime? ReportCommentDate { get; set; }

    public virtual TableUser ReportCommentReporterNavigation { get; set; } = null!;

    public virtual TableComment ReportCommentReportingNavigation { get; set; } = null!;
}
