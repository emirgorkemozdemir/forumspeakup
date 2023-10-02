using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class AdminBL : ManagerRepository<TableReport, AdminDAL>
    {
        AdminDAL admin_dal = new AdminDAL();
        public List<TableReport> ReportedTopics()
        {
          return admin_dal.ReportedTopics();
        }

        public List<TableReportComment> ReportedComments()
        {
            return admin_dal.ReportedComments();
        }

        public List<TableReport> ReportedDeactiveTopics()
        {
            return admin_dal.ReportedDeactiveTopics();
        }

        public List<TableReportComment> ReportedDeactiveComments()
        {
            return admin_dal.ReportedDeactiveComments();
        }

        public void DeactiveReportedTopic(int id)
        {
            admin_dal.DeactiveReportedTopic(id);
        }

        public void DeactiveReportedComment(int id)
        {
            admin_dal.DeactiveReportedComment(id);
        }

        public void DeactiveTopic(int id)
        {
            admin_dal.DeactiveTopic(id);
        }

        public void DeactiveComment(int id)
        {
            admin_dal.DeactiveComment(id);
        }

        public void ActivateTopic(int id)
        {
            admin_dal.ActivateTopic(id);
        }

        public void ActivateComment(int id)
        {
            admin_dal.ActivateComment(id);
        }

        public TableComment GetSingleComment(int id)
        {
            return admin_dal.GetSingleComment(id);
        }
    }
}
