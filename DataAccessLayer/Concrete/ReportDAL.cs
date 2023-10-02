using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class ReportDAL : EfRepositoryBase<TableReport, VoiceFormContext>
    {
        public string ReportPost(int user_id, int reporting_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_report_record = my_context.TableReports.Where(r => r.Reporter == user_id && r.Reporting == reporting_id && r.ReportActive == true).FirstOrDefault();
                if (my_report_record !=null)
                {
                    if (my_report_record.ReportActive == false)
                    {
                        my_report_record.ReportActive = true;
                        my_context.SaveChanges();
                        return "already_reported";
                    }
                    else
                    {
                        my_report_record.ReportActive = false;
                        my_context.SaveChanges();
                        return "already_reported";
                    }
                }
                else
                {
                    TableReport new_report = new TableReport();
                    new_report.Reporter =user_id;
                    new_report.Reporting = reporting_id;
                    new_report.ReportActive = true;
                    my_context.TableReports.Add(new_report);
                    my_context.SaveChanges();
                    return "reported_first_time";
                }
            }

        }

        public string ReportComment(int user_id, int reporting_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_report_record = my_context.TableReportComments.Where(r => r.ReportCommentReporter == user_id && r.ReportCommentReporting == reporting_id && r.ReportCommentActive == true).FirstOrDefault();
                if (my_report_record != null)
                {
                    if (my_report_record.ReportCommentActive == false)
                    {
                        my_report_record.ReportCommentActive = true;
                        my_context.SaveChanges();
                        return "already_reported";
                    }
                    else
                    {
                        my_report_record.ReportCommentActive = false;
                        my_context.SaveChanges();
                        return "already_reported";
                    }
                }
                else
                {
                    TableReportComment new_report = new TableReportComment();
                    new_report.ReportCommentReporter = user_id;
                    new_report.ReportCommentReporting = reporting_id;
                    new_report.ReportCommentActive = true;
                    my_context.TableReportComments.Add(new_report);
                    my_context.SaveChanges();
                    return "reported_first_time";
                }
            }

        }

        public bool CheckIfUserCanReportPostToday(int? user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list_of_reports_today = my_context.TableReports.
                    Where(t => t.ReportDate > DateTime.Now.AddDays(-1) && (t.Reporter == user_id)).ToList();
                if (list_of_reports_today.Count() >= 15)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool CheckIfUserCanReportCommentToday(int? user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list_of_reports_today = my_context.TableReportComments.
                    Where(t => t.ReportCommentDate > DateTime.Now.AddDays(-1) && (t.ReportCommentReporter == user_id)).ToList();
                if (list_of_reports_today.Count() >= 15)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
