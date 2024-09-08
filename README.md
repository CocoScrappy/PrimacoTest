# Dmitryberg Terminal

## Overview

Dmitryberg Terminal is a simple web application that allows users to search for financial data using ticker symbols. 
It utilizes a C# for the backend and a JS/React frontend.

## Setup Instructions

### Backend

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/dmitryberg-terminal.git
   cd dmitryberg-terminal/backend
2. **Install dependencies**

    dotnet restore

3. **Use dbCreateScript to create a Database**

4. **Create .env file, obtain token for ALPHA_VANTAGE_API and connection string**

5. **Run the backend**

    dotnet run

### Frontend

1. **Navigate to backend**
    cd ../frontend/dmitryberg-terminal

2. **Install dependencies**

    npm install

3. **Run the frontend**

    npm start


### Database 
Ensure your database is set up correctly by using the provided dbCreateScript.sql. This script creates the necessary tables for the application, including User and SearchHistory.

### Additional Details
Technologies Used: C#, React, SQL Server, Alpha Vantage API
Frontend Framework: React
Database: MS SQL
