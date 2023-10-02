using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace EntityLayer.Concrete
{
    public class CommentWithUserModel
    {
        public TableComment Comment { get; set; }
        public TableUser User { get; set; }
    }
}
