using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class FollowBL : ManagerRepository<TableFollow, FollowDAL>
    {
        FollowDAL follow_manager = new FollowDAL();
        public string FollowTopic(int follower_id, int following_sub)
        {
            return follow_manager.FollowTopic(follower_id, following_sub);
        }
    }
}
