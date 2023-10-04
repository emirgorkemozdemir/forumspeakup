using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class UserDAL : EfRepositoryBase<TableUser, VoiceFormContext>
    {
        public string UserRegisterExistingCheckDAL(TableUser my_user)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                string return_text = "";
                var selected_userMail = my_context.TableUsers.FirstOrDefault(i => i.UserMail == my_user.UserMail);
                var selected_userName = my_context.TableUsers.FirstOrDefault(i => i.UserName == my_user.UserName);

                if (selected_userMail != null)
                {
                    return_text = "Girdiğiniz mail adresi kullanılmaktadır.";
                }
                else if (selected_userName != null)
                {
                    return_text = "Girdiğiniz kullanıcı adı kullanılmaktadır.";
                }

                return return_text;
            }
        }

        public TableUser UserLoginDAL(TableUser my_user)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {

                var selected_user = my_context.TableUsers.FirstOrDefault(i => i.UserName == my_user.UserName && i.UserPassword == my_user.UserPassword);
                if (selected_user==null)
                {
                    return my_user;
                }
                else
                {
                    if (selected_user.UserActive == true)
                    {
                        return selected_user;
                    }
                    else
                    {
                        selected_user.UserActive = true;
                        my_context.SaveChanges();
                        return selected_user;
                    }
                }   
            }
        }

        public bool UserCheckIPAdress(string ip_adress)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var list_of_accounts = my_context.TableUsers.Where(u => u.UserIpAdress == ip_adress).ToList();
                if (list_of_accounts.Count()>=3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void ChangePassword(string mail, string new_password)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_user = my_context.TableUsers.FirstOrDefault(a => a.UserMail == mail);
                selected_user.UserPassword = new_password;
                my_context.SaveChanges();
            }

        }

        public TableUser IsUserAdmin(TableUser my_user)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
               // user admin code is hidden
            }
        }

        public TableUser GetMyProfile(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var user = my_context.TableUsers.Find(id);
                return user;
            }
        }

        public void ChangePP(int id, string pp_name)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_user = my_context.TableUsers.Find(id);
                selected_user.UserPp = pp_name;
                my_context.SaveChanges();
            }
        }

        public void ChangeBio(int id, string bio)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_user = my_context.TableUsers.Find(id);
                selected_user.UserBio = bio;
                my_context.SaveChanges();
            }
        }

        public string GetUsernameByID(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var user = my_context.TableUsers.Find(id);
                return user.UserName;
            }
        }

        public string GetBioByID(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var user = my_context.TableUsers.Find(id);
                return user.UserBio;
            }
        }

        public void AddUserPoint(int user_id, int user_point)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_user = my_context.TableUsers.Find(user_id);
                my_user.UserPoint += user_point;
                my_context.SaveChanges();
            }
        }

        public List<TableTopic> GetUsersLastLiked(int user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                List<TableTopic> list = new List<TableTopic>();
                var liked_topics = my_context.TableLikes.Where(l=>l.LikeUser == user_id).Take(10).ToList();
                foreach (var topic_like in liked_topics)
                {
                    list.Add(my_context.TableTopics.Find(topic_like.LikeTopic));
                }

                return list;
            }
        }

        public List<TableTopic> GetUsersLastShared(int user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_topics = my_context.TableTopics.Where(t => t.TopicSharerId == user_id).OrderByDescending(t=>t.TopicDate).Take(10).ToList();
                return selected_topics;
            }
        }

        public List<TableTopic> GetUsersAllShared(int user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var selected_topics = my_context.TableTopics.Where(t => t.TopicSharerId == user_id).OrderByDescending(t => t.TopicDate).ToList();
                return selected_topics;
            }
        }

        public TableUser GetUserByID(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_user = my_context.TableUsers.Find(id);
                return my_user;
            }
        }

        public void DeactivateUser(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_user = my_context.TableUsers.Find(id);
                my_user.UserActive = false;
                my_context.SaveChanges();
            }
        }

    }
}
