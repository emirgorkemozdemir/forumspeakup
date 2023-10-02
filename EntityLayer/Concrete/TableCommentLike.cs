using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableCommentLike : IDatabaseEntity
{
    public int CommentLikeId { get; set; }

    public int CommentLiker { get; set; }

    public int CommentLiked { get; set; }

    public bool? CommentLikeActive { get; set; }

    public virtual TableComment CommentLikedNavigation { get; set; } = null!;

    public virtual TableUser CommentLikerNavigation { get; set; } = null!;
}
