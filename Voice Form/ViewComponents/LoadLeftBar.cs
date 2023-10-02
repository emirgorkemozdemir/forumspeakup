using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voice_Form.Models;

namespace Voice_Form.ViewComponents
{
    public class LoadLeftBar : ViewComponent
    {
        TopicBL topic_manager = new TopicBL();
        public IViewComponentResult Invoke()
        {
            List<TableTopic> my_list = new List<TableTopic>();
            if (HttpContext.Session.GetString("TopicKey") == "trends")
            {
                my_list = topic_manager.GetTrends();
            }
            else if (HttpContext.Session.GetString("TopicKey") == "latest")
            {
                my_list = topic_manager.GetLatest();
            }
            else
            {
                my_list = topic_manager.GetLatest();
            }
            return View(my_list);
        }
    }
}
