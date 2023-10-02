using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class CategoryBL : ManagerRepository<TableCategory, CategoryDAL>
    {
        CategoryDAL category_dal = new CategoryDAL();
        public List<SubNormalCategory> GetPopularCategories()
        {
            return category_dal.GetPopularCategories();
        }

        public List<SubNormalCategory> GetAllCategories()
        {
            return category_dal.GetAllCategories();
        }
    }
}
