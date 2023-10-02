using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableLike : IDatabaseEntity
{
    public int LikeId { get; set; }

    public int LikeUser { get; set; }

    public int LikeTopic { get; set; }

    public bool? LikeActive { get; set; }

    public virtual TableTopic LikeTopicNavigation { get; set; } = null!;

    public virtual TableUser LikeUserNavigation { get; set; } = null!;
}
