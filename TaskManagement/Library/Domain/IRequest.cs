using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain
{
    public interface IRequest
    {
        CancellationTokenSource CtsSource { get; set; }
    }
}
