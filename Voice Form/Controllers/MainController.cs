using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Voice_Form.Extra_Classes;
using Voice_Form.Models;

namespace Voice_Form.Controllers
{
    public class MainController : Controller
    {
        CategoryBL category_manager = new CategoryBL();
        TopicBL topic_manager = new TopicBL();
        CommentBL comment_manager = new CommentBL();
        SubCategoryBL sub_category_manager = new SubCategoryBL();
        FollowBL follow_manager = new FollowBL();
        UserBL user_manager = new UserBL();
        ReportBL report_manager = new ReportBL();
        NotificationBL notification_manager = new NotificationBL();


        private readonly IWebHostEnvironment _hostEnvironment;
        public MainController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }

        public List<SelectListItem> returnMainCategories()
        {
            var category_list = category_manager.List();
            List<SelectListItem> select_list = new List<SelectListItem>();
            foreach (var category in category_list)
            {
                select_list.Add(new SelectListItem(text: category.CategoryName.ToString(), value: category.CategoryId.ToString()));
            }

            return select_list;
        }

        [HttpPost]
        public JsonResult GetSubCategories(int main_id)
        {
            var sub_list = sub_category_manager.List();
            var new_sub_list = sub_list.Where(m => m.SubCategoryMainId == main_id).ToList();
            var jsonlist = Json(new_sub_list);
            return jsonlist;
        }

        public IActionResult index(bool keepOpenPage = false)
        {
            if (keepOpenPage == true)
            {
                string previousUrl = Request.Headers["Referer"].ToString();
                return Redirect(previousUrl);
            }
            else
            {
                var my_list = category_manager.GetPopularCategories();
                return View(my_list);
            }
        
        }

        public IActionResult SetLeftBarKey(string load_topics_key = "latest")
        {
            HttpContext.Session.SetString("TopicKey", load_topics_key);
            return RedirectToAction("index", new { keepOpenPage = true });
        }

        public IActionResult LoadAllCategories()
        {
            var mylist = category_manager.GetAllCategories();
            return View(mylist);
        }

        [HttpGet]
        public IActionResult OpenTopic(int topic_id, int page = 1, int pageSize = 10, string order_key ="suggested")
        {

            TopicAndCommentsModel model = new TopicAndCommentsModel();
            if (order_key=="suggested")
            {
                model= topic_manager.getTopicAndComments(topic_id);
                model.OrderKey = "suggested";
            }
            else if (order_key=="descending")
            {
                model = topic_manager.getTopicAndCommentsOrderDescendingDate(topic_id);
                model.OrderKey = "descending";
            }
            else if (order_key == "ascending")
            {
                model = topic_manager.getTopicAndCommentsOrderAscendingDate(topic_id);
                model.OrderKey = "ascending";
            }
            else if (order_key == "liked")
            {
                model = topic_manager.getTopicAndCommentsOrderMostLiked(topic_id);
                model.OrderKey = "liked";
            }

            model.TotalPages = model.comments.Count() % 10 == 0 ? model.TotalPages = model.comments.Count() / 10
    : (model.comments.Count() / 10) + 1;
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            model.comments = model.comments.Skip(skip).Take(take).ToList();
            model.CurrentPage = page;
            return View(model);
        }

        [HttpPost]
        public IActionResult OpenTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LikeTopic(string topic_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged")=="yes")
            {
                var message = topic_manager.LikeTopic(Convert.ToInt32(topic_id), Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                if (message == "user_liked_it")
                {
                    var poster = topic_manager.GetPosterIDByTopicID(Convert.ToInt32(topic_id));
                    user_manager.AddUserPoint((int)poster, 1);
                }
                var like_num = topic_manager.GetTopicLikeNumber(Convert.ToInt32(topic_id));
                return Json(like_num);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
         
        }

        [HttpPost]
        public IActionResult LikeComment(string comment_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                var message = comment_manager.LikeComment(Convert.ToInt32(comment_id), Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                if (message == "user_liked_it")
                {
                    var commenter = comment_manager.GetCommenterIDByCommentID(Convert.ToInt32(comment_id));
                    user_manager.AddUserPoint(commenter, 1);
                }
                var like_num = comment_manager.GetCommentLikeNumber(Convert.ToInt32(comment_id));
                return Json(like_num);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }   
        }

        [HttpGet]
        public IActionResult CreatePostWithAudio()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                ViewBag.Categories = returnMainCategories();
                return View();
            }
            else
            {
                return RedirectToAction("index", "Main");
            }      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePostWithAudio(TableTopic new_topic,string hiddenaudiotext)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                if (topic_manager.CheckIfUserCanPostToday(new_topic.TopicSharerId))
                {
                    ViewBag.Categories = returnMainCategories();
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var filePath = Path.Combine(wwwRootPath + "/extra/badwords.json");
                    if (String.IsNullOrEmpty(hiddenaudiotext))
                    {
                        ViewBag.words_validation = "Ses kaydı boş bırakılamaz. Ses kaydı olmadan post atmak için aşağıdaki seçeneği seçin.";
                        return View();
                    }
                    else if (BadWords.CheckBadWords(new_topic.TopicContent, filePath))
                    {
                        ViewBag.words_validation = "Küfür, hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else if (BadWords.CheckBadWords(new_topic.TopicTitle, filePath))
                    {
                        ViewBag.words_validation = "Küfür, hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else if (new_topic.TopicVoiceLink == null)
                    {
                        ViewBag.words_validation = "Ses kaydı boş bırakılamaz. Ses kaydı olmadan post atmak için aşağıdaki seçeneği seçin.";
                        return View();
                    }
                    else if (hiddenaudiotext.Contains("*"))
                    {
                        ViewBag.words_validation = "Küfür,hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else
                    {
                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
                        new_topic.TopicSharerIp = remoteIpAddress.ToString();
                        user_manager.AddUserPoint(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), 5);
                        string sub_cat_name = sub_category_manager.GetSubNameByID(new_topic.TopicSubCategory);
                        topic_manager.Add(new_topic);
                        int topid = new_topic.TopicId;
                        notification_manager.SendNotif((int)new_topic.TopicSubCategory, sub_cat_name, new_topic.TopicTitle, topid);
                        return RedirectToAction("OpenTopic", new { topic_id = topid });
                    }
                }
                else
                {
                    ViewBag.Categories = returnMainCategories();
                    ViewBag.words_validation = "Günlük paylaşım sınırını aştınız";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            }   
        }

        [HttpPost]
        public async Task<IActionResult> UploadAudio(string mytext)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                string badwordsroot = _hostEnvironment.WebRootPath;
                var badwordspath = Path.Combine(badwordsroot + "/extra/badwords.json");
                var audio = Request.Form.Files[0];
                if (audio == null || audio.Length == 0)
                {
                    return BadRequest("No audio file uploaded.");
                }
                else if (audio.Length > 15000000)
                {
                    return BadRequest("Dosya boyutu çok büyük");
                }
                else if (BadWords.CheckBadWords(mytext, badwordspath))
                {
                    return BadRequest("Küfür tespit edildi");
                }
                else if (mytext.Contains("*"))
                {
                    return BadRequest("Küfür tespit edildi");
                }
                else
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var filePath = Path.Combine(wwwRootPath + "/audios/", audio.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await audio.CopyToAsync(stream);
                    }
                    return Ok(filePath.ToString());
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            } 
        }

        [HttpGet]
        public IActionResult CreatePostWithoutAudio()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                ViewBag.Categories = returnMainCategories();
                return View();
            }
            else
            {
                return RedirectToAction("index", "Main");
            }         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePostWithoutAudio(TableTopic new_topic, string hiddenaudiotext)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                if (topic_manager.CheckIfUserCanPostToday(new_topic.TopicSharerId))
                {
                    ViewBag.Categories = returnMainCategories();
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var filePath = Path.Combine(wwwRootPath + "/extra/badwords.json");

                    if (BadWords.CheckBadWords(new_topic.TopicContent, filePath))
                    {
                        ViewBag.words_validation = "Küfür, hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else if (BadWords.CheckBadWords(new_topic.TopicTitle, filePath))
                    {
                        ViewBag.words_validation = "Küfür, hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else
                    {
                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
                        new_topic.TopicSharerIp = remoteIpAddress.ToString();
                        topic_manager.Add(new_topic);
                        user_manager.AddUserPoint(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), 5);
                        int topid = new_topic.TopicId;
                        string sub_cat_name = sub_category_manager.GetSubNameByID((int)new_topic.TopicSubCategory);
                        notification_manager.SendNotif((int)new_topic.TopicSubCategory, sub_cat_name, new_topic.TopicTitle, topid);
                        return RedirectToAction("OpenTopic", new { topic_id = topid });
                    }
                }
                else
                {
                    ViewBag.words_validation = "Günlük paylaşım sınırını aştınız";
                    ViewBag.Categories = returnMainCategories();
                    return View();   
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendComment(string topicid , string commentcontent ,string hiddenaudiotext,string hiddenvoicelink)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                if (comment_manager.CheckIfUserCanPostCommentToday(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID"))))
                {
                    ViewBag.Categories = returnMainCategories();
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var filePath = Path.Combine(wwwRootPath + "/extra/badwords.json");
                    if (BadWords.CheckBadWords(commentcontent, filePath))
                    {
                        ViewBag.words_validation = "Küfür, hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else if (hiddenaudiotext.Contains("*"))
                    {
                        ViewBag.words_validation = "Küfür,hakaret içeriği tespit edildi.";
                        return View();
                    }
                    else
                    {
                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
                        TableComment new_comment = new TableComment();
                        new_comment.CommentContent = commentcontent;
                        new_comment.CommentVoiceLink = hiddenvoicelink;
                        new_comment.CommentSharerIp = remoteIpAddress.ToString();
                        new_comment.CommentTopicId = Convert.ToInt32(topicid);
                        comment_manager.Add(new_comment);
                        return RedirectToAction("index", new { keepOpenPage = true });
                    }
                }
                else
                {
                    ViewBag.words_validation = "Günlük yorum sınırı aşıldı";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
          
        }

      
        public IActionResult Search(string searchParam, int page = 1, int pageSize = 10)
        {
            SearchResultsModel model = new SearchResultsModel();
            model.SearchParameter = searchParam;
            model.Topics = topic_manager.SearchTopics(searchParam);
            model.TotalPages = model.Topics.Count() % 10 == 0 ? model.TotalPages = model.Topics.Count() / 10
  : (model.Topics.Count() / 10) + 1;
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            model.Topics = model.Topics.Skip(skip).Take(take).ToList();
            model.CurrentPage = page;
            return View(model);
        }

        public IActionResult FollowCategory(string subcat_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                follow_manager.FollowTopic(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), Convert.ToInt32(subcat_id));
                return RedirectToAction("index", new { keepOpenPage = true });
            }
            else
            {
                return RedirectToAction("index", "Main");
            }      
        }

        [HttpGet]
        public IActionResult ReportPost(int user_id,string reporting_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                if (report_manager.CheckIfUserCanReportPostToday(user_id))
                {
                    report_manager.ReportPost(user_id, Convert.ToInt32(reporting_id));
                    return RedirectToAction("index", new { keepOpenPage = true });
                }
                else
                {
                    return RedirectToAction("index", new { keepOpenPage = true });
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            }     
        }

        [HttpGet]
        public IActionResult ReportComment(int user_id, string reporting_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                if (report_manager.CheckIfUserCanReportCommentToday(user_id))
                {
                    report_manager.ReportComment(user_id, Convert.ToInt32(reporting_id));
                    return RedirectToAction("index", new { keepOpenPage = true });
                }
                else
                {
                    return RedirectToAction("index", new { keepOpenPage = true });
                }
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        [HttpGet]
        public IActionResult MessageBox(string notif_key = "active")
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                List<TableNotification> list = notification_manager.GetNotifs(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), notif_key);
                NotifMessageBoxModel model = new NotifMessageBoxModel();
                model.NotifKey = notif_key;
                model.Notifs = list;
                return View(model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        [HttpGet]
        public IActionResult ReadNotif(int notif_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                notification_manager.ReadNotif(notif_id);
                return RedirectToAction("index", new { keepOpenPage = true });
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        [HttpGet]
        public IActionResult ReadNotifAndRedirectToOpenTopic(int notif_id,int topic_id)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                notification_manager.ReadNotif(notif_id);
                return RedirectToAction("OpenTopic", new { topic_id = topic_id });
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        public IActionResult GetPopularTopicsOfCategory(int subcat_id)
        {
            var my_list = topic_manager.GetCategoriesPopularTopics(subcat_id);
            ViewBag.subcat = subcat_id;
            return View(my_list);
        }

        public IActionResult GetLatestByCategory(int subcat_id)
        {
            var my_list = topic_manager.GetLatestByCategory(subcat_id);
            return View(my_list);
        }
    }
}
