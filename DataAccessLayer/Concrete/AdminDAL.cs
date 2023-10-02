using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class AdminDAL : EfRepositoryBase<TableReport, VoiceFormContext>
    {
        public List<TableReport> ReportedTopics()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableReports.Where(r => r.ReportActive == true && r.ReportingNavigation.TopicActive==true)
                    .OrderByDescending(r=>r.ReportDate).ToList();
                return list;    
            }
        }

        public List<TableReportComment> ReportedComments()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableReportComments.Where(r => r.ReportCommentActive == true && r.ReportCommentReportingNavigation.CommentActive==true)
                    .OrderByDescending(r=>r.ReportCommentDate).ToList();
                return list;
            }
        }

        public List<TableReport> ReportedDeactiveTopics()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableReports.Where(r => r.ReportActive == false).OrderByDescending(r=>r.ReportingNavigation.TopicActive)
                    .ThenByDescending(r => r.ReportDate).ToList();
                return list;
            }
        }

        public List<TableReportComment> ReportedDeactiveComments()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableReportComments.Where(r => r.ReportCommentActive == false).OrderByDescending(r=>r.ReportCommentReportingNavigation.CommentActive)
                    .ThenByDescending(r => r.ReportCommentDate).ToList();
                return list;
            }
        }

        public void DeactiveReportedTopic(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var reported_topic = my_context.TableReports.Find(id);
                reported_topic.ReportActive = false;
                my_context.SaveChanges();
            }
        }

        public void DeactiveReportedComment(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var reported_comment = my_context.TableReportComments.Find(id);
                reported_comment.ReportCommentActive = false;
                my_context.SaveChanges();
            }
        }

        public void DeactiveTopic(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var topic = my_context.TableTopics.Find(id);
                topic.TopicActive = false;
                my_context.SaveChanges();
            }
        }

        public void DeactiveComment(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var comment = my_context.TableComments.Find(id);
                comment.CommentActive = false;
                my_context.SaveChanges();
            }
        }

        public void ActivateTopic(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var topic = my_context.TableTopics.Find(id);
                topic.TopicActive = true;
                my_context.SaveChanges();
            }
        }

        public void ActivateComment(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var comment = my_context.TableComments.Find(id);
                comment.CommentActive = true;
                my_context.SaveChanges();
            }
        }

        public TableComment GetSingleComment(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var comment = my_context.TableComments.Find(id);
                return comment;
            }
        }

    }
}
