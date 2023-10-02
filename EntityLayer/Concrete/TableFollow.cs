using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableFollow : IDatabaseEntity
{
    public int FollowId { get; set; }

    public int FollowerId { get; set; }

    public int FollowingSubId { get; set; }

    public bool? FollowActive { get; set; }

    public virtual TableUser Follower { get; set; } = null!;

    public virtual TableSubCategory FollowingSub { get; set; } = null!;
}
