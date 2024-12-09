# Software Requirements Specification

## Web-based Library System

### 1. Introduction
#### 1.1 Purpose
The purpose of this document is to outline the software requirements 
for a web-based Library System. 
The system is designed to enable library members to search for and reserve books online, 
while borrowing and returning will be handled in person. 
Librarians will use the system to manage the library's book catalog 
and facilitate member interactions.

#### 1.2 Definitions, Acronyms, and Abbreviations
- **WLS**: Web-based Library System
- **Reader**: A user who can search, reserve, borrow, and prolong books, report issues with borrowed books, and request to donate books.
- **Librarian**: A user responsible for managing library inventory, member interactions, and resolving account issues.

#### 1.3 Scope
The Library System is a web-based application designed to modernize library operations and improve user experience. 
Its primary functionalities include:

- **For Readers**: Searching for books, reserving available titles online, 
    prolonging borrowed books a limited number of times, 
    reporting lost or destroyed books and receiving guidance from librarians, 
    requesting to donate books to the library, 
    and accessing personal borrowing history. 
    Actual borrowing and returning of books are conducted at the library.

- **For Librarians**: Managing the book catalog, 
    tracking borrowing and returning records, 
    generating reports to streamline daily operations, 
    approving or rejecting book donation requests, 
    responding to reports of lost or destroyed books, 
    and resolving member account issues.

### 2. Overall Description

#### 2.1 Product Perspective
The WLS is a standalone web-based system 
aimed at improving the efficiency of library operations. 
It will replace traditional manual processes 
with a more automated and interactive system. 

#### 2.2 Product Features
- **Book Search**: Readers can search for books by title, author, genre, or ISBN.

- **Online Reservations**: Readers can reserve books available in the library.

- **Borrowing History**: Readers can view their borrowing history.

- **Prolonging Borrowed Books**: Readers can extend their borrowing period within set limits.

- **Report Issues**: Readers can report lost or destroyed borrowed books and receive guidance from librarians.

- **Book Donations**: Readers can request to donate books to the library.

- **Catalog Managment**: Librarians can add, update and remove books from the library catalog.

- **Borrowing and Returning Management**: Librarians can track records of borrowed and returned books.

- **Reports**: Librarians can generate reports such as overdue books, borrowing trends, etc.

#### 2.3 User Characteristics

- **Readers**:  Typically familiar with browsing and searching for information online. 
  They require minimal training and should be able to use features 
  like book search, reservations, and reporting lost or damaged books intuitively.
  Additionally, they may need occasional guidance from librarians 
  for more complex interactions, such as resolving issues with borrowed books.

- **Librarians**: Proficient in library management processes and basic computer operations. 
  They will require training to manage advanced system features like generating reports, 
  catalog updates, and responding to user-reported issues such as lost or damaged books.

#### 2.4 Constraints
- The system must comply with data protection regulations (e.g., GDPR).
- Compatible with commonly used web browsers (e.g., Chrome, Firefox, Safari).
- Limited to existing library infrastructure for hardware integration (e.g., barcode scanners).
- Maximum response time for critical operations (e.g., search and reservations) should not exceed 2 seconds.

#### 2.5 Assumptions and Dependencies
- Readers and Librarians have access to a stable internet connection.
- Hardware will be provided by the library.
- The library database is available and operational for integration.

### 3. Functional Requirements
#### 3.1 Reader Features
- Ability to search for books by title, author, genre, or ISBN.

- Reserve a book if it is available.

- View personal borrowing history.

- Prolong the borrowing period for a book, subject to library policy.

- Report lost or damaged books.

- Submit requests to donate books to the library.

#### 3.2 Librarian Features
- Add, update, and remove books from the catalog.

- Manage borrowing and returning records.

- Approve or reject book reservation requests.

- Respond to lost or damaged book reports.

- Approve or reject book donation requests.

- Generate reports on library statistics, such as overdue books and popular titles.

### 4. Non-Functional Requirements
#### 4.1 Performance
- The system should handle up to 500 simultaneous user sessions without performance degradation.

- Search results should load within 2 seconds for up to 1 million book records.

#### 4.2 Usability
- The interface must be intuitive and user-friendly for both Readers and Librarians.

- Provide tooltips and help documentation for all features.

#### 4.3 Security
- Implement role-based access control (RBAC) to separate Reader and Librarian functionalities.

- All sensitive data must be encrypted during storage and transmission.

- Include CAPTCHA for user registration to prevent automated sign-ups.

#### 4.4 Availability
- Ensure 99.9% system uptime, excluding scheduled maintanace.

- Data backups must occur daily, with the ability to restore 2 hours.

### 5. System Models
#### 5.1 Use Case Diagram
_(To be added: A UML diagram showing interactions among members, librarians.)_

#### 5.2 Data Flow Diagram
_(To be added: A visual representation of data flows within the system.)_


#### 6.2 Contact Information
For queries related to this document:
- Author: Anton Ovod
- Email: antovod36@gmail.com
- Institution: Lublin University of Technology

---
This document is subject to revision as the project progresses.
