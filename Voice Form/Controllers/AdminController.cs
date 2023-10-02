using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Voice_Form.Models;

namespace Voice_Form.Controllers
{
    public class AdminController : Controller
    {
        AdminBL admin_manager = new AdminBL();
        public IActionResult AdminMainPage()
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Main");
            }  
        }

        public IActionResult ReportedTopics(int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                var list = admin_manager.ReportedTopics();
                ReportAdminModel model = new ReportAdminModel();
                model.reports = list;
                model.TotalPages = model.reports.Count() % 10 == 0 ? model.TotalPages = model.reports.Count() / 10
    : (model.reports.Count() / 10) + 1;
                int skip = (page - 1) * pageSize;
                int take = pageSize;
                model.reports = model.reports.Skip(skip).Take(take).ToList();
                model.CurrentPage = page;
                return View(model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }          
        }

        public IActionResult DeactiveTopic(int topic_id,int report_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.DeactiveTopic(topic_id);
                admin_manager.DeactiveReportedTopic(report_id);
                return RedirectToAction("ReportedTopics");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }   
        }

        public IActionResult DeactiveJustTopic(int topic_id, int sender_id)
        {
            if (Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")) == sender_id)
            {
                admin_manager.DeactiveTopic(topic_id);
                return RedirectToAction("index","Main");
            }
            else
            {
                return RedirectToAction("Logout", "User");
            }
          
        }

        public IActionResult DeactiveReport(int report_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.DeactiveReportedTopic(report_id);
                return RedirectToAction("ReportedTopics");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }         
        }

        public IActionResult ReportedDeactiveTopics(int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                var list = admin_manager.ReportedDeactiveTopics();
                ReportAdminModel model = new ReportAdminModel();
                model.reports = list;
                model.TotalPages = model.reports.Count() % 10 == 0 ? model.TotalPages = model.reports.Count() / 10
    : (model.reports.Count() / 10) + 1;
                int skip = (page - 1) * pageSize;
                int take = pageSize;
                model.reports = model.reports.Skip(skip).Take(take).ToList();
                model.CurrentPage = page;
                return View(model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            } 
        }

        public IActionResult ActivateTopic(int topic_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.ActivateTopic(topic_id);
                return RedirectToAction("ReportedDeactiveTopics");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }  
        }

        public IActionResult ReportedComments(int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                var list = admin_manager.ReportedComments();
                CommentReportAdminModel model = new CommentReportAdminModel();
                model.reports = list;
                model.TotalPages = model.reports.Count() % 10 == 0 ? model.TotalPages = model.reports.Count() / 10
    : (model.reports.Count() / 10) + 1;
                int skip = (page - 1) * pageSize;
                int take = pageSize;
                model.reports = model.reports.Skip(skip).Take(take).ToList();
                model.CurrentPage = page;
                return View(model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }  
        }

        public IActionResult DeactiveComment(int comment_id, int report_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.DeactiveTopic(comment_id);
                admin_manager.DeactiveReportedTopic(report_id);
                return RedirectToAction("ReportedTopics");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        public IActionResult DeactiveCommentReport(int comment_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.DeactiveReportedTopic(comment_id);
                return RedirectToAction("ReportedTopics");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }    
        }

        public IActionResult OpenSingleCommentAdmin(int comment_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                var comment = admin_manager.GetSingleComment(comment_id);
                return View(comment);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }  
        }

        public IActionResult ReportedDeactiveComments(int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                var list = admin_manager.ReportedDeactiveComments();
                CommentReportAdminModel model = new CommentReportAdminModel();
                model.reports = list;
                model.TotalPages = model.reports.Count() % 10 == 0 ? model.TotalPages = model.reports.Count() / 10
    : (model.reports.Count() / 10) + 1;
                int skip = (page - 1) * pageSize;
                int take = pageSize;
                model.reports = model.reports.Skip(skip).Take(take).ToList();
                model.CurrentPage = page;
                return View(model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }   
        }

        public IActionResult ActivateComment(int comment_id)
        {
            if (HttpContext.Session.GetString("IsAdminLogged") == "yes")
            {
                admin_manager.ActivateComment(comment_id);
                return RedirectToAction("ReportedDeactiveComments");
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }
    }
}
