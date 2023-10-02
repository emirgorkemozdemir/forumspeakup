using EntityLayer.Abstract;

namespace Voice_Form.Models
{
    public class SubNormalCategory
    {
        public string MainCategoryName { get; set; }
        public List<TableSubCategory> SubList { get; set; }
    }
}
