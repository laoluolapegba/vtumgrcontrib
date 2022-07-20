using System;
using System.Collections.Generic;
using System.Text;

namespace Chams.Vtumanager.Provisioning.Services.AirtelPretups
{
    public enum PretupsErrorCodes
    {
        RequestSuccessfullyProcessed = 200,
        TranscationStatusisUnderprocess = 205,
        TransactionStatusisFail = 206,
        RequestsuccessfulmessagesenttoC2SReceiver = 207
    }
}
