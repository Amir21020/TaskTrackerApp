namespace TaskTrackerApp.Domain.Enums;

public enum PermissionType
{
    CreateProject = 1,
    DeleteProject = 2,
    ViewProject = 3,

    UploadSchemes = 10,
    ApproveSchemes = 11, 
    RejectSchemes = 12,

    CreateTask = 20,
    ExecuteTask = 21,
    UploadExecutionPhotos = 22,
    ViewReports = 30
}
