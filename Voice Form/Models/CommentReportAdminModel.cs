namespace Voice_Form.Models
{
    public class CommentReportAdminModel
    {
        public List<TableReportComment> reports { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
