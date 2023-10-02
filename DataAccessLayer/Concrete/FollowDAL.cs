using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class FollowDAL : EfRepositoryBase<TableFollow, VoiceFormContext>
    {
        public string FollowTopic(int follower_id, int following_sub)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_follow_record = my_context.TableFollows.Where(f => f.FollowerId == follower_id && f.FollowingSubId == following_sub).FirstOrDefault();
                if (my_follow_record != null)
                {
                    if (my_follow_record.FollowActive ==false)
                    {
                        my_follow_record.FollowActive = true;
                        my_context.SaveChanges();
                        return "followed_back";
                    }
                    else
                    {
                        my_follow_record.FollowActive = false;
                        my_context.SaveChanges();
                        return "unfollowed_back";
                    }
                }
                else
                {
                    TableFollow new_follow = new TableFollow();
                    new_follow.FollowerId = follower_id;
                    new_follow.FollowingSubId = following_sub;
                    new_follow.FollowActive = true;
                    my_context.TableFollows.Add(new_follow);
                    my_context.SaveChanges();
                    return "followed_first_time";
                }
            }
        }
    }
}
