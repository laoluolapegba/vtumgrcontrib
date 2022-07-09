using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.QueService
{
    public interface IAMQService
    {
        Task<bool> SubmitTopupOrder(RechargeRequest rechargeRequest);
    }
}
