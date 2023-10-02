using EntityLayer.Concrete;

namespace Voice_Form.Models
{
    public class ProfileAllSharingsMode
    {
        public List<TableTopic> Topics { get; set; }

        public int Userid { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
