using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class CommentDAL : EfRepositoryBase<TableComment, VoiceFormContext>
    {
        public string LikeComment(int comment_id, int user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_like_record = my_context.TableCommentLikes.Where(l => l.CommentLiked == comment_id && l.CommentLiker == user_id).FirstOrDefault();
                if (my_like_record != null)
                {
                    if (my_like_record.CommentLikeActive==false)
                    {
                        var selected_comment = my_context.TableComments.Find(comment_id);
                        selected_comment.CommentLike = selected_comment.CommentLike + 1;
                        my_like_record.CommentLikeActive = true;
                        //var deleting_entry = my_context.TableCommentLikes.Entry(my_like_record);
                        //deleting_entry.State = EntityState.Deleted;
                        my_context.SaveChanges();
                        return "user_already_liked";
                    }
                    else
                    {
                        var selected_comment = my_context.TableComments.Find(comment_id);
                        selected_comment.CommentLike = selected_comment.CommentLike - 1;
                        my_like_record.CommentLikeActive = false;
                        //var deleting_entry = my_context.TableCommentLikes.Entry(my_like_record);
                        //deleting_entry.State = EntityState.Deleted;
                        my_context.SaveChanges();
                        return "user_already_liked";
                    }
                   
                }
                else
                {
                    var selected_comment = my_context.TableComments.Find(comment_id);
                    selected_comment.CommentLike = selected_comment.CommentLike + 1;
                    TableCommentLike new_comment_like = new TableCommentLike();
                    new_comment_like.CommentLiker = user_id;
                    new_comment_like.CommentLiked = comment_id;
                    my_context.TableCommentLikes.Add(new_comment_like);
                    my_context.SaveChanges();
                    return "user_liked_it";
                }
            }
        }

        public int? GetCommentLikeNumber(int comment_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var like_num = my_context.TableComments.Find(comment_id).CommentLike;
                return like_num;
            }
        }

        public int GetCommenterIDByCommentID(int comment_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var commenter = my_context.TableComments.Find(comment_id).CommentSenderId;
                return commenter;
            }
        }

        public bool CheckIfUserCanPostCommentToday(int? user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var user = my_context.TableUsers.Find(user_id);
                var list_of_topics_today = my_context.TableComments.
                    Where(t => t.CommentDate > DateTime.Now.AddDays(-1) && (t.CommentSenderId == user_id || t.CommentSharerIp == user.UserIpAdress)).ToList();
                if (list_of_topics_today.Count() >= 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
