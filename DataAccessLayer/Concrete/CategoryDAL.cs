using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class CategoryDAL : EfRepositoryBase<TableCategory, VoiceFormContext>
    {
        public List<SubNormalCategory> GetAllCategories()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                List<SubNormalCategory> my_list = new List<SubNormalCategory>();
                var categories = my_context.TableCategories.ToList();
                foreach (var category in categories)
                {
                    SubNormalCategory entity = new SubNormalCategory();
                    entity.SubList = my_context.TableSubCategories.Where(s => s.SubCategoryMainId == category.CategoryId).ToList();
                    entity.MainCategoryName = category.CategoryName;
                    my_list.Add(entity);
                }
                return my_list;
            }
        }
        public List<SubNormalCategory> GetPopularCategories()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                List<SubNormalCategory> my_list = new List<SubNormalCategory>();
                var categories = my_context.TableCategories.OrderByDescending(c => c.CategoryCount).Take(10).ToList();
                foreach (var category in categories)
                {
                    SubNormalCategory entity = new SubNormalCategory();
                    entity.SubList = my_context.TableSubCategories.Where(s => s.SubCategoryMainId == category.CategoryId).ToList();
                    entity.MainCategoryName = category.CategoryName;
                    my_list.Add(entity);
                }
                return my_list;
            }
        }
    }
}
