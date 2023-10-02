namespace Voice_Form.Models
{
    public class ReportAdminModel
    {
        public List<TableReport> reports { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
