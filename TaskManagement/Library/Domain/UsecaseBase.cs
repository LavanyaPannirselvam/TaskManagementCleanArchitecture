using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain
{
    public abstract class UsecaseBase<T>
    {
        public CancellationTokenSource Source { get; }
        private readonly CancellationToken _token;
        private IPresenterCallbackBasecase<T> _responseCallback;

        public UsecaseBase() { }
        protected UsecaseBase(IPresenterCallbackBasecase<T> responseCallback, CancellationTokenSource CtsSource)
        {
            _responseCallback = responseCallback;
            Source = CtsSource;
            _token = Source.Token;
        }

        public void Execute()
        {
            if (GetIfAvailableInCache())
            {
                return;
            }
            Task.Run(delegate ()
            {
                try
                {
                    Action();
                }
                catch(BaseException ex) 
                {
                    //Debug.WriteLine(ex.ToString()); 
                    _responseCallback.OnError(ex);
                }
            }, _token);
        }

        public abstract void Action();
        public virtual bool GetIfAvailableInCache()
        {
            return false;
        }
    }
}
