# Warehouse Management System

## Version: 2.0  
**Creation Date:** 14.10.2023  
**Last Modified:** 25.01.2024  

**Authors:**  
- [Alan Kowańdy](https://github.com/alankowandy)  
- [Julia Banach](https://github.com/juliaban)
- Mariusz Kotecki  

---

## General Information
### Purpose
A system to manage warehouse operations for a company producing shop and high-storage racks.

### Key Features
- Track inventory levels
- Notify for reorders (BANF)
- Store detailed product information
- Manage employee and product databases
- Support for receiving and dispatching goods

### Target Audience
Companies with their own warehouses and production facilities for items like shop racks.

### Business Benefits
- Improved warehouse efficiency
- Reduced human errors
- Simplified ordering process
- Integrated data for time-saving and efficient work

### Project Background
This project was created for a software engineering course at Politechnika Poznańska.

---

## Actors
- **Warehouse Worker:** Manages inventory intake and dispatch.
- **Purchasing Employee:** Tracks inventory, analyzes usage, and plans orders.
- **Production Manager:** Plans production orders.
- **System Administrator:** Manages users and materials.

---

## Functional Requirements
- **Receiving Materials:** Verify and add materials to inventory.
- **Removing Materials:** Verify and remove materials from inventory.
- **Generating Orders:** Analyze inventory and create orders.
- **Material Need Analysis (BANF):** Generate list of materials to order.
- **Planning Production Orders:** Generate and manage production orders.
- **User Management:** Add, edit, or remove system users.
- **Material Management:** Add, edit, or remove materials.
- **Product Management:** Add or remove products.

---

## Non-Functional Requirements
- **Usability:** Simple and intuitive UI, minimal training required.
- **Reliability:** Thoroughly tested, updated weekly.
- **Performance:** Server-dependent, supports multiple simultaneous logins.
- **Security:** User identification and encrypted data compliant with EU directives.

---

## Screenshots
<p>
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/cee5a89c-c118-4238-aaec-0e7003ee1a62" alt="" width="200">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/19a67e96-244d-444e-927d-702a93c0882e" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/c5983f4d-203e-4ce2-81c3-1dbe4eb21f0f" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/5dd4f690-485a-480d-94b9-f05003de42ef" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/3f6d25cc-626f-470c-8d5b-f861b4dc5116" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/36c315ec-3f3f-4d77-837b-c29d7bb38a23" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/2dd9cf05-ac8e-43b1-b355-60a61d0e16c2" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/5983a005-6dd1-4bf0-ae37-ad93ae7e01a8" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/f4c3cc58-cc51-46f0-a78b-b471b42e05ae" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/150c19a0-c5de-4e3f-9c6f-eab1459235e5" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/954420da-041d-48dd-ae2a-194a9b83e5e8" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/6da55c73-4736-4509-ab24-7de1fdba8911" alt="" width="400">
    <img src="https://github.com/alankowandy/software-engineering-put-course/assets/100705149/4e5b4bb8-d9c4-4d9e-8726-28d808cd375e" alt="" width="400">
</p>

---

## How to Run the Program
1. **Pre-requisites:**
   - Visual Studio 2019
   - .NET Framework
   - SQLite

2. **Setup:**
   - Clone the repository:
     ```sh
     git clone https://github.com/yourusername/warehouse-management-system.git
     ```
   - Open the solution file (`.sln`) in Visual Studio 2019.

3. **Build and Run:**
   - Build the solution using Visual Studio.
   - Run the application from Visual Studio.

4. **Login:**
   - Use the following credentials to test the program:
     - **Admin:** `admin` / `admin`
     - **User:** `user` / `user`

---

## Database
This program uses SQLite as its database. The SQLite instance is provided within the project.

---

## License
This project is licensed under the MIT License. See the [LICENSE](path/to/license) file for details.

---
