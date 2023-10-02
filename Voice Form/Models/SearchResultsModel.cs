namespace Voice_Form.Models
{
    public class SearchResultsModel
    {
        public List<TableTopic> Topics { get; set; }

        public string SearchParameter { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
