using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class SubCategoryDAL : EfRepositoryBase<TableSubCategory, VoiceFormContext>
    {
        public string GetSubNameByID(int? id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                return my_context.TableSubCategories.Find(id).SubCategoryName;
            }
        }
    }
}
