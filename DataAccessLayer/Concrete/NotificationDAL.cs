using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class NotificationDAL : EfRepositoryBase<TableNotification, VoiceFormContext>
    {
        public void SendNotif(int sub_id, string cat_name, string title, int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_followlist = my_context.TableFollows.Where(f => f.FollowingSubId == sub_id).ToList();
                List<TableUser> user_list = new List<TableUser>();
                foreach (var follows in selected_followlist)
                {
                    var user = my_context.TableUsers.Find(follows.FollowerId);
                    user_list.Add(user);
                }

                foreach (var user in user_list)
                {
                    TableNotification new_not = new TableNotification();
                    new_not.NotificationContent = $"{topic_id}|{cat_name}|{title}";
                    new_not.NotificationOwner = user.UserId;
                    new_not.NotificationActive = true;
                    my_context.TableNotifications.Add(new_not);
                    my_context.SaveChanges();
                }
            }
        }

        public int GetActiveNotifNum(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableNotifications.Where(n => n.NotificationOwner == id && n.NotificationActive == true).ToList();
                return list.Count();
            }
        }

        public List<TableNotification> GetNotifs(int id, string notif_key)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                if (notif_key == "passive")
                {
                    var list = my_context.TableNotifications.Where(n => n.NotificationOwner == id && n.NotificationActive == false).ToList();
                    return list;
                }
                else
                {
                    var list = my_context.TableNotifications.Where(n => n.NotificationOwner == id && n.NotificationActive == true).ToList();
                    return list;
                }
              
            }
        }

        public List<TableNotification> GetPassiveNotifs(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list = my_context.TableNotifications.Where(n => n.NotificationOwner == id && n.NotificationActive == false).ToList();
                return list;
            }
        }

        public void ReadNotif(int notif_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_notif = my_context.TableNotifications.Find(notif_id);
                selected_notif.NotificationActive = false;
                my_context.SaveChanges();
            }
        }
    }
}
