 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;

namespace TaskManagementLibrary.Data.DBManager
{
    public class CreateTaskDataManager : TaskManagementDataManager, ICreateTaskDataManager
    {
        public CreateTaskDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void CreateTask(CreateTaskRequest request, IUsecaseCallbackBasecase<CreateTaskResponse> response)
        {
            var id = DBhandler.AddTask(request.task);
            ZResponse<CreateTaskResponse> zResponse = new ZResponse<CreateTaskResponse>();
            CreateTaskResponse createTaskResponse = new CreateTaskResponse();
            createTaskResponse.Data = DBhandler.GetTask(id);
            zResponse.Response = "Task added successfully";
            zResponse.Data = createTaskResponse;
            response.OnResponseSuccess(zResponse);
        } 
    }
}
