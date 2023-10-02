using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableSubCategory : IDatabaseEntity
{
    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = null!;

    public int SubCategoryMainId { get; set; }

    public virtual TableCategory SubCategoryMain { get; set; } = null!;

    public virtual ICollection<TableFollow> TableFollows { get; } = new List<TableFollow>();

    public virtual ICollection<TableTopic> TableTopics { get; } = new List<TableTopic>();
}
