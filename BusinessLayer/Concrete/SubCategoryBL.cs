using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class SubCategoryBL : ManagerRepository<TableSubCategory, SubCategoryDAL>
    {
        SubCategoryDAL subCategoryDAL = new SubCategoryDAL();
        public string GetSubNameByID(int? id)
        {
            return subCategoryDAL.GetSubNameByID(id);
        }
    }
}
