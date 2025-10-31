# ReportingSystem
# 🧾 Reporting System API Documentation

**Version:** v1  
**Purpose:** This API provides endpoints for managing a reporting system (governorates, departments, employees, users, report types, reports, and images).

## 🔐 General Notes

- Most endpoints require a **JWT Bearer Token** in the header:
  ```http
  Authorization: Bearer <your_token_here>

  Data format: JSON

Image upload: multipart/form-data
Response codes:
✅ 200 – Success

🆕 201 – Created

⚠️ 400 – Bad Request

🔒 401 – Unauthorized

❌ 404 – Not Found



⚙️ Security Scheme

Type: Bearer Token

Location: Header

Name: Authorization

Format:

Authorization: Bearer eyJhbGciOi...






---

👤 Users API

| Method | Endpoint          | Description          | Auth Required |
| ------ | ----------------- | -------------------- | ------------- |
| POST   | `/Register`       | Register a new user  | ❌            |
| POST   | `/Login`          | User login           | ❌            |
| GET    | `/Profile`        | Get user profile     | ✅            |
| PUT    | `/ChangePassword` | Change user password | ✅            |
| DELETE | `/DeleteAccount`  | Delete user account  | ✅            |


🧑‍💼 Employees API

| Method | Endpoint                                     | Description              | Auth Required |
| ------ | -------------------------------------------- | ------------------------ | ------------- |
| POST   | `/api/Employees/CreateEmployee`              | Create a new employee    | ✅            |
| POST   | `/api/Employees/CreateAdmin`                 | Create a new admin       | ✅            |
| POST   | `/api/Employees/Login`                       | Employee login           | ❌            |
| GET    | `/api/Employees/Profile`                     | Get employee profile     | ✅            |
| PUT    | `/api/Employees/ChangePassword/{employeeId}` | Change employee password | ✅            |
| DELETE | `/api/Employees/DeleteEmployee/{employeeId}` | Delete employee          | ✅            |




🏢 Departments API


| Method | Endpoint                                              | Description                    | Auth Required |
| ------ | ----------------------------------------------------- | ------------------------------ | ------------- |
| GET    | `/api/Departments`                                    | Get all departments            | ❌            |
| POST   | `/api/Departments`                                    | Create a new department        | ❌            |
| GET    | `/api/Departments/{Id}`                               | Get department by ID           | ❌            |
| PUT    | `/api/Departments/{Id}`                               | Update department              | ✅            |
| DELETE | `/api/Departments/{Id}`                               | Delete department              | ✅            |
| GET    | `/api/Departments/GetByGovernorateId/{governorateId}` | Get departments by governorate | ❌            |


🗺 Governorates API

| Method | Endpoint                 | Description           | Auth Required |
| ------ | ------------------------ | --------------------- | ------------- |
| GET    | `/api/Governorates`      | Get all governorates  | ❌            |
| GET    | `/api/Governorates/{Id}` | Get governorate by ID | ❌            |


🧾 Reports API

| Method | Endpoint                                                 | Description                | Auth Required |
| ------ | -------------------------------------------------------- | -------------------------- | ------------- |
| GET    | `/api/Reports`                                           | Get all reports            | ✅            |
| POST   | `/api/Reports`                                           | Create a new report        | ✅            |
| GET    | `/api/Reports/{ReportId}`                                | Get report by ID           | ✅            |
| PUT    | `/api/Reports/{ReportId}`                                | Update report              | ✅            |
| DELETE | `/api/Reports/{ReportId}`                                | Delete report              | ✅            |
| GET    | `/api/Reports/GetReportsByGovernorateId/{GovernorateId}` | Get reports by governorate | ✅            |
| GET    | `/api/Reports/GetReportsByDepartmentId/{DepartmentId}`   | Get reports by department  | ✅            |
| GET    | `/api/Reports/GetReportsByReportTypeId/{ReportTypeId}`   | Get reports by type        | ✅            |
| GET    | `/api/Reports/GetReportsForUser`                         | Get reports by user        | ✅            |
| GET    | `/api/Reports/GetReportsForEmployee`                     | Get employee reports       | ✅            |
| POST   | `/api/Reports/UpdateReportStatus/{reportId}`             | Update report status       | ✅            |
| POST   | `/api/Reports/RejectReport/{reportId}`                   | Reject a report            | ✅            |


🧩 Report Types API

| Method | Endpoint                                                            | Description                     | Auth Required  |
| ------ | ------------------------------------------------------------------- | ------------------------------- | -------------  |
| GET    | `/api/ReportTypes`                                                  | Get all report types            | ✅             |
| POST   | `/api/ReportTypes`                                                  | Create a new report type        | ✅             |
| GET    | `/api/ReportTypes/{Id}`                                             | Get report type by ID           | ✅             |
| PUT    | `/api/ReportTypes/{Id}`                                             | Update report type              | ✅             |
| DELETE | `/api/ReportTypes/{Id}`                                             | Delete report type              | ✅             |
| GET    | `/api/ReportTypes/GetAllReportTypesByDepartmentId/{departmentId}`   | Get report types by department  | ✅             |
| GET    | `/api/ReportTypes/GetAllReportTypesByGovernorateId/{governorateId}` | Get report types by governorate | ✅             |




🖼 Images API


| Method | Endpoint                                     | Description                         | Auth Required |
| ------ | -------------------------------------------- | ----------------------------------- | ------------- |
| GET    | `/api/Images`                                | Get all images                      | ✅             |
| POST   | `/api/Images/{reportId}`                     | Upload an image (form-data: `file`) | ✅             |
| GET    | `/api/Images/{imageId}`                      | Get image by ID                     | ✅             |
| DELETE | `/api/Images/{imageId}`                      | Delete image                        | ✅             |
| GET    | `/api/Images/GetImagesByReportId/{reportId}` | Get images by report                | ✅             |


🔄 Report Updates API


| Method | Endpoint                                                       | Description             | Auth Required |
| ------ | -------------------------------------------------------------- | ----------------------- | ------------- |
| GET    | `/api/ReportUpdates`                                           | Get all report updates  | ✅             |
| GET    | `/api/ReportUpdates/GetReportUpdatesByEmployeeId/{employeeId}` | Get updates by employee | ✅             |
| GET    | `/api/ReportUpdates/GetReportUpdatesByReportId/{reportId}`     | Get updates by report   | ✅             |
| GET    | `/api/ReportUpdates/GetReportUpdatesById/{Id}`                 | Get update by ID        | ✅             |


