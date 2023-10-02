using EntityLayer.Abstract;
using System;
using System.Collections.Generic;

namespace Voice_Form.Models;

public partial class TableNotification : IDatabaseEntity
{
    public int NotificationId { get; set; }

    public string NotificationContent { get; set; } = null!;

    public int NotificationOwner { get; set; }


    public bool? NotificationActive { get; set; }
    public DateTime? NotificationDate { get; set; }

    public virtual TableUser NotificationOwnerNavigation { get; set; } = null!;
}
