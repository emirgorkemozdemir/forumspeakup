using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Voice_Form.Models;

namespace Voice_Form.ViewComponents
{
    public class LMessageBox : ViewComponent
    {
        NotificationBL notification_manager = new NotificationBL();
        public IViewComponentResult Invoke(int user_id)
        {
            return View(notification_manager.GetActiveNotifNum(user_id));
        }
    }
}
