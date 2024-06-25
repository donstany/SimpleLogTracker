# Use DDD for Application Architecture

## Task:

### 1. Database with 3 tables – User, Project, TimeLog.

- For the User table, store the following information: first name, last name, e-mail
- The Project table should store the Id and name of the project
- For the TimeLog table, store user, project, date, and hours (float)

### 2. Stored Procedure for database initialization, which performs the following:

- Deletes the content of the User, Project, and TimeLog tables upon each start
- Generates 100 records in the Users table with random names and e-mail addresses based on:
  - First Names: John, Gringo, Mark, Lisa, Maria, Sonya, Philip, Jose, Lorenzo, George, Justin
  - Last Names: Johnson, Lamas, Jackson, Brown, Mason, Rodriguez, Roberts, Thomas, Rose, McDonalds
  - Domains: hotmail.com, gmail.com, live.com
  - For each generated record, a random first name, random last name, and an e-mail address in the format first.last@randomdomain.com should be selected.

- Generates 3 projects: My own, Free Time, Work
- For each record in the User table, generates a random number of records (1-20) in the TimeLog table with random projects and a random number of hours (0.25-8.00) per user.
- The total hours logged per day should not exceed 8 working hours for each user.

### 3. The user interface should be a single page, divided into two columns of 50% each.

- The left column should contain a Grid with Users, divided into pages of 10 users each. Use SQL Pagination.
  - Add sorting to the table and a date filter from ... to.

- The right column should contain a bar chart (Google Charts) with one bar for the TOP 10 Users with the most hours for the selected period, where the bar size represents the sum of hours per user or project. (Radio button to select user or project)
  - Add a Compare button for each user in the users table, which asynchronously requests the hours data for the selected user from the server (via AJAX) and displays it in the chart as a level (line) in red for comparison with the TOP 10.
  - The hours data for the selected user should not be loaded when the table is initially displayed, but only when the Compare button is clicked.

### 4. Add a button that starts the database initialization procedure and reloads the page.
