using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
public class OpenTelemetry
{
    public string GrpcCollectorReceiverUrl { get; set; } = null!;
}
