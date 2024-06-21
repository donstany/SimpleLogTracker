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

---------------------------------------------------------------------------------------------------

# Да се ползва DDD за архитектура на Аpplcation-а

## Задача:

### 1т. База данни с 3 таблици – User, Project, TimeLog.

- За User да се пази следната информация: име, фамилия, e-mail
- Project да пази Id и име на проекта
- За TimeLog да се пази потребител, проект, дата и часове (float)

### 2т. Stored Procedure за инициализация на базата данни, която да прави следното:

- При всяко стартиране да изтрива съдържанието на таблиците User, Project, TimeLog
- Генерира 100 записа в таблицата Users с произволни имена и e-mail адреси на базата на:
  - Име: John, Gringo, Mark, Lisa, Maria, Sonya, Philip, Jose, Lorenzo, George, Justin
  - Фамилия: Johnson, Lamas, Jackson, Brown, Mason, Rodriguez, Roberts, Thomas, Rose, McDonalds
  - Domain: hotmail.com, gmail.com, live.com
  - За всеки генериран запис трябва да се вземе произволно име, произволно презиме, а за e-mail адреса име.фамилия@произволен домейн.

- Генерира 3 проекта: My own, Free Time, Work
- За всеки запис в User генерира произволен брой записи (1-20) в таблицата TimeLog с произволен проект и произволен брой Hours (0.25-8.00) на потребител.
- Записите в рамките на ден да не надвишават 8 работни часа за потребител.

### 3т. Потребителския интерфейс трябва да бъде една страница, която да е разделена на две колони по 50%

- Лявата колона да съдържа Grid със Users разделени по 10 на страница. Използвай SQL Pagination.
  - Добави сортиране на таблицата и филтър с дати от … до.

- Дясната колона да съдържа bar chart (Google Charts), по 1 бар за TOP 10 Users с най-голям брой часове за избрания период, размера на bar-a е сумата часове по потребител или проект. (Радио бутон да избере потребител или проект)
  - Добави бутон Compare за всеки user в таблицата с users, който с асинхронна заявка към сървъра (ajax) да извлича данните за часове на избрания user и да ги отразява в chart-a като ниво (линия) в червен цвят за сравнение с ТOP 10.
  - Данните за часове на избрания потребител да не се зареждат при зареждане на таблицата, а при натискане на бутона Compare.

### 4т. Да се добави бутон, който стартира процедурата за инициализация на базата и презарежда страницата.
