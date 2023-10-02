using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class UserBL : ManagerRepository<TableUser, UserDAL>
    {
        UserDAL user_dal = new UserDAL();

        public string UserRegisterExistingCheckBL(TableUser my_user)
        {
            return user_dal.UserRegisterExistingCheckDAL(my_user);
        }

        public TableUser UserLoginBL(TableUser my_user)
        {
            return user_dal.UserLoginDAL(my_user);
        }

        public void ChangePasswordBL(string mail_adress, string new_password)
        {
            user_dal.ChangePassword(mail_adress, new_password);
        }

        public TableUser IsUserAdminBL(TableUser my_user)
        {
            return user_dal.IsUserAdmin(my_user);
        }

        public bool UserCheckIPAdress(string ip_adress)
        {
            return user_dal.UserCheckIPAdress(ip_adress);
        }

        public TableUser GetMyProfile(int id)
        {
            return user_dal.GetMyProfile(id);
        }

        public void ChangePP(int id, string pp_name)
        {
            user_dal.ChangePP(id, pp_name);
        }

        public string GetUsernameByID(int id)
        {
            return user_dal.GetUsernameByID(id);
        }

        public void ChangeBio(int id, string bio)
        {
            user_dal.ChangeBio(id, bio);
        }

        public string GetBioByID(int id)
        {
            return user_dal.GetBioByID(id);
        }

        public void AddUserPoint(int user_id, int user_point)
        {
            user_dal.AddUserPoint(user_id, user_point);
        }

        public List<TableTopic> GetUsersLastLiked(int user_id)
        {
            return user_dal.GetUsersLastLiked(user_id);
        }

        public List<TableTopic> GetUsersLastShared(int user_id)
        {
            return user_dal.GetUsersLastShared(user_id);
        }

        public List<TableTopic> GetUsersAllShared(int user_id)
        {
            return user_dal.GetUsersAllShared(user_id);
        }

        public TableUser GetUserByID(int id)
        {
            return user_dal.GetUserByID(id);
        }

        public void DeactivateUser(int id)
        {
            user_dal.DeactivateUser(id);
        }
    }
}
