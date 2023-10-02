using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableCategory : IDatabaseEntity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? CategoryCount { get; set; }

    public virtual ICollection<TableSubCategory> TableSubCategories { get; } = new List<TableSubCategory>();

    public virtual ICollection<TableTopic> TableTopics { get; } = new List<TableTopic>();
}
