using EntityLayer.Concrete;

namespace Voice_Form.Models
{
    public class TopicAndCommentsModel
    {
        public TopicWithUserModel topic { get; set; }
        public List<CommentWithUserModel> comments { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string OrderKey { get; set; }
    }
}
