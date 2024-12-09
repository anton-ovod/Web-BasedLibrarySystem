# Software Requirements Specification (SRS)

## Web-based Library System

### 1. Introduction
#### 1.1 Purpose
The purpose of this document is to outline the software requirements 
for a web-based Library System. 
The system is designed to enable library members to search for and reserve books online, 
while borrowing and returning will be handled in person. 
Librarians will use the system to manage the library's book catalog and facilitate member interactions, 
and administrators will oversee user accounts and system configurations.


#### 1.2 Scope
The Library System is a web-based application designed to modernize library operations and improve user experience. Its primary functionalities include:
- **For Members**: Searching for books, reserving available titles online, and accessing personal borrowing history. Actual borrowing and returning of books are conducted at the library.
- **For Librarians**: Managing the book catalog, tracking borrowing and returning records, and generating reports to streamline daily operations.
- **For Administrators**: Overseeing user account management, system settings, and generating advanced analytics reports to optimize library workflows.

#### 1.3 Definitions, Acronyms, and Abbreviations
- **LMS**: Library Management System
- **Member**: A user who can borrow and reserve books.
- **Librarian**: A user responsible for managing library operations.
- **Administrator**: A user responsible for system configuration and user management.

#### 1.4 References
- [IEEE Standard 830-1998](https://ieeexplore.ieee.org/document/73053): Recommended Practice for Software Requirements Specifications.

### 2. Overall Description
#### 2.1 Product Perspective
The Library Management System will replace manual record-keeping with a digital system to improve efficiency and accuracy. It will be a web-based application accessible via browsers.

#### 2.2 Product Features
- Book catalog management.
- Member account management.
- Borrowing, returning, and reserving workflows.
- Notifications for due dates and overdue books.
- Reporting capabilities for administrators.

#### 2.3 User Characteristics
- **Library Members**: General users with basic computer skills.
- **Librarians**: Moderately experienced users familiar with library workflows.
- **Administrators**: Technically proficient users managing the system.

#### 2.4 Constraints
- The system must comply with data protection regulations.
- It should be accessible from standard web browsers.
- Performance should remain optimal with up to 1,000 concurrent users.

#### 2.5 Assumptions and Dependencies
- Members will have internet access to use the system.
- The library will provide required hardware and infrastructure.

### 3. Functional Requirements
#### 3.1 Member Features
- Search for books by title, author, or category.
- View book availability.
- Borrow and return books.
- Reserve unavailable books.
- View borrowing history.

#### 3.2 Librarian Features
- Add, update, or remove books from the catalog.
- Manage borrowing and returning processes.
- Notify members about overdue books.
- Generate inventory and transaction reports.

#### 3.3 Administrator Features
- Create, update, and delete user accounts.
- Configure system policies (e.g., loan durations, fines).
- Monitor system performance and usage.
- Generate comprehensive reports.

### 4. Non-Functional Requirements
#### 4.1 Performance
- The system should support up to 1,000 concurrent users without performance degradation.

#### 4.2 Usability
- The interface should be user-friendly and intuitive.

#### 4.3 Security
- Role-based access control to restrict unauthorized actions.
- Encrypted storage of sensitive data, such as passwords.

#### 4.4 Availability
- The system should have 99.9% uptime.

### 5. System Models
#### 5.1 Use Case Diagram
_(To be added: A UML diagram showing interactions among members, librarians, and administrators.)_

#### 5.2 Data Flow Diagram
_(To be added: A visual representation of data flows within the system.)_

### 6. Appendices
#### 6.1 Glossary
- **Book Catalog**: A database of all books available in the library.
- **Reservation**: A request to hold a book for future borrowing.

#### 6.2 Contact Information
For queries related to this document:
- Author: Anton Ovod
- Email: [Your Email Address]
- Institution: [Your Institution Name]

---
This document is subject to revision as the project progresses.
