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
            var newTask = request.task;
            var id = DbHandler.AddTask(newTask);
            newTask = DbHandler.GetTask(id);
            ZResponse<CreateTaskResponse> zResponse = new ZResponse<CreateTaskResponse>();
            CreateTaskResponse createTaskResponse = new CreateTaskResponse();
            createTaskResponse.NewTask = newTask;
            zResponse.Response = "Task added successfully";
            zResponse.Data = createTaskResponse;
            response.OnResponseSuccess(zResponse);
        } 
        //event to be created and handled
    }
}
