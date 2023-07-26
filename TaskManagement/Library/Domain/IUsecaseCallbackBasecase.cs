using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain
{
    public interface IUsecaseCallbackBasecase<R>
    {
        void OnResponseError(BaseException response);
        void OnResponseFailure(ZResponse<R> response);
        void OnResponseSuccess(ZResponse<R> response);
    }
}
