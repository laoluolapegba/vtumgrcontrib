using System;

namespace Chams.Vtumanager.Fulfillment.NineMobile.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
