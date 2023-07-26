using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain
{
    public interface IPresenterCallbackBasecase<R>
    {
        void OnSuccessAsync(ZResponse<R> response);
        void OnError(BaseException errorMessage);
        void OnFailure(ZResponse<R> response);
    }
}
