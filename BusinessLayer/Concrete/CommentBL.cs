using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class CommentBL : ManagerRepository<TableComment, CommentDAL>
    {
        CommentDAL comment_dal = new CommentDAL();
        public string LikeComment(int comment_id, int user_id)
        {
            return comment_dal.LikeComment(comment_id, user_id);
        }

        public int? GetCommentLikeNumber(int comment_id)
        {
            return comment_dal.GetCommentLikeNumber(comment_id);
        }

        public int GetCommenterIDByCommentID(int comment_id)
        {
            return comment_dal.GetCommenterIDByCommentID(comment_id);
        }

        public bool CheckIfUserCanPostCommentToday(int? user_id)
        {
            return comment_dal.CheckIfUserCanPostCommentToday(user_id);
        }
    }
}
