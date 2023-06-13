using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IDeleteProjectDataManager
    {
        void DeleteProject(DeleteProjectRequest request, IUsecaseCallbackBasecase<bool> response);
    }


    public class DeleteProjectRequest : IRequest
    {
        public int projectId;
        public CancellationTokenSource CtsSource { get ; set ; }

        public DeleteProjectRequest(int id,CancellationTokenSource cts) 
        {
            projectId = id;
            CtsSource = cts;
        }
    }


    public interface IPresenterDeleteProjectCallback : IPresenterCallbackBasecase<bool>
    { }


    public class DeleteProject : UsecaseBase<bool>
    {
        private IDeleteProjectDataManager _projectDeletionDataManager;
        private DeleteProjectRequest _deleteProjectRequest;
        private IPresenterDeleteProjectCallback _callback;
        
        public DeleteProject( DeleteProjectRequest deleteProjectRequest, IPresenterDeleteProjectCallback callback)
        {
            _projectDeletionDataManager = ServiceProvider.GetInstance().Services.GetService<IDeleteProjectDataManager>();
            _deleteProjectRequest = deleteProjectRequest;
            _callback = callback;
        }
        
        public override void Action()
        {
            this._projectDeletionDataManager.DeleteProject(_deleteProjectRequest, new DeleteProjectCallback(this));
        }
        
        
        public class DeleteProjectCallback : IUsecaseCallbackBasecase<bool>
        {
            private DeleteProject _projectDeletion;
            
            public DeleteProjectCallback(DeleteProject projectDeletion)
            {
                _projectDeletion = projectDeletion;
            }

            public void OnResponseError(BException response)
            {
                _projectDeletion._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                _projectDeletion._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _projectDeletion._callback.OnSuccessAsync(response);
            }
        }
        
        
        public class DeleteProjectResponse : ZResponse<bool>
        {

        }
    }
}
