using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class NotificationBL : ManagerRepository<TableNotification, NotificationDAL>
    {
        NotificationDAL notification_dal = new NotificationDAL();
        public void SendNotif(int sub_id, string cat_name, string title, int topic_id)
        { 
            notification_dal.SendNotif(sub_id, cat_name, title, topic_id);
        }

        public int GetActiveNotifNum(int id)
        {
          return notification_dal.GetActiveNotifNum(id);
        }

        public List<TableNotification> GetNotifs(int id, string notif_key)
        {
           return notification_dal.GetNotifs(id,notif_key);
        }

        public void ReadNotif(int notif_id)
        {
            notification_dal.ReadNotif(notif_id);
        }
    }
}
