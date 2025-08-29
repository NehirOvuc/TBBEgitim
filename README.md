# TBBEgitim - Education Management System

This project is an ASP.NET MVC web application designed to manage and automate educations.  
It provides features for handling courses, applications, reports, and automated backup tasks.

## Features

- View educations with details.

- See reports about courses, applications, and participant data.

- Admin Portal
  - Add users
  - See the backup records

- Automatic Backup System
  Integrated with **Quartz.NET** for scheduling:
  - Daily automatic backups of passwords (`sifreler.txt`)  
  - Backup logs stored in `App_Data/backup-log.txt`  
  - Backup files stored in `App_Data/Backup/`

## Technologies Used

- ASP.NET MVC
- Entity Framework
- Quartz.NET (for scheduling jobs)
- SQL Server
- jQuery & DataTables (for interactive tables)
