using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.WindowsService._9M
{
    public partial class VtuWorker_NineMobile : ServiceBase
    {
        public VtuWorker_NineMobile()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
