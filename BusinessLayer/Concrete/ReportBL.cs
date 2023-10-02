using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class ReportBL : ManagerRepository<TableReport, ReportDAL>
    {
        ReportDAL report_dal = new ReportDAL();
        public string ReportPost(int user_id, int reporting_id)
        {
            return report_dal.ReportPost(user_id, reporting_id);
        }

        public string ReportComment(int user_id, int reporting_id)
        {
            return report_dal.ReportComment(user_id, reporting_id);
        }

        public bool CheckIfUserCanReportPostToday(int? user_id)
        {
            return report_dal.CheckIfUserCanReportPostToday(user_id);
        }

        public bool CheckIfUserCanReportCommentToday(int? user_id)
        {
            return report_dal.CheckIfUserCanReportCommentToday(user_id);
        }

    }
}
