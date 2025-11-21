# Contract Monthly Claim System — Part 3 (Final POE)

## Login Credentials

| Role | Email | Password | Description |
|------|-------|---------|------------|
| Lecturer | lecturer1@college.com | pass123 | Submit monthly claims, upload documents, view submission history |
| Programme Coordinator | pc1@college.com | pass123 | Verify pending claims submitted by lecturers |
| Academic Manager | am1@college.com | pass123 | Approve claims verified by Programme Coordinators |
| HR (Admin) | hr1@college.com | pass123 | Manage users, set hourly rates, update roles, generate reports |

## Lecturer Feedback from Part 2 and Implementation

| Feedback | Implementation |
|----------|---------------|
| Lecturers should not set their own hourly rate | HR module added to define hourly rates; Lecturers’ rates are now automatically pulled from the database. |
| Files stored without validation | File uploads now validated for type (PDF, DOCX, XLSX) and size; securely stored under `/wwwroot/uploads/`. |
| Generic error messages for file issues | Detailed feedback provided for missing, oversized, or invalid files. |
| Missing functionality testing | Unit tests added for claim submission, including valid/invalid inputs and file validation. |
| Managers approving unverified claims | Workflow updated: Coordinators verify claims before Managers approve; approved/rejected claims move to separate views. |
| Approved claims still shown in Pending | Approved and rejected claims are now filtered out from Pending Claims view. |

## New Features

- **Session-Based Login:** Secure authentication for each role without using Identity  
- **Database Integration:** SQL Server database for persistent user and claim data  
- **Role-Based Access Control:** Pages and functionality restricted by user role  
- **Claim Workflow Automation:** Claims automatically move through Pending → Verified → Approved  
- **Auto Payment Calculation:** Lecturer payments calculated as hours × hourly rate  
- **File Upload Validation:** Checks for supported formats and size limits  
- **HR Dashboard:** HR can manage users, roles, and hourly rates  
- **Report Generation:** HR can view and generate summary reports of approved claims  

## Video Demonstration

- Shows all roles and workflows  
- Demonstrates login access control  
- Includes database operations and unit tests
- Video Link:

## Technologies

- ASP.NET Core MVC  
- Entity Framework Core with SQL Server (LocalDB)  
- Session-based authentication  
- C#, Razor Views, LINQ  
- Bootstrap for UI styling  

## Version Control

At least 10 descriptive commits, including:

- Database integration  
- Session-based login  
- HR module and CRUD operations  
- Claim verification workflow  
- File validation and error handling  
- Approved/rejected claim filtering  
- Unit tests  
- Report generation  
- UI styling updates  
- Updated README/documentation

## Access the Application

Try the live web application here: https://localhost:7036/

## Conclusion

Part 3 automates the claim process for all roles, centralizes HR management, enforces proper workflow, validates file uploads, and secures sessions, fully meeting the POE Part 3 requirements.
