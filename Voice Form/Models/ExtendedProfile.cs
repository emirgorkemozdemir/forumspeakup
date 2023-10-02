namespace Voice_Form.Models
{
    public class ExtendedProfile
    {
        public TableUser User { get; set; }

        public List<TableTopic> shared_list { get; set; }

        public List<TableTopic> liked_list { get; set; }
    }
}
