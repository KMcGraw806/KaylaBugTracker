﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaylaBugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        #region Parents/Children
        public int TicketId { get; set; }
        public virtual Tickets Tickets { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion

        #region Actual Properties
        public string Notes { get; set; }

        public DateTime Created { get; set; }
    #endregion
}
}