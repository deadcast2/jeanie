# Jeanie Reservation System

A web-based reservation management system built with ASP.NET MVC that allows organizations to manage time-slot based appointments.

![.NET](https://img.shields.io/badge/.NET-4.7.2-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## Overview

Jeanie Reservation System is a comprehensive solution for managing time-slot based appointments and reservations. Originally developed for a university research lab, this system allows administrators to:

- Set up and manage available time slots
- Block dates/times that are unavailable
- Configure daily reservation limits
- Email participants with confirmation and reminder emails
- Track and manage reservations

For participants, the system offers:

- Easy-to-use interface for booking appointments
- Email notifications and reminders
- Calendar integration (Google, Apple, Outlook)
- Ability to confirm or cancel reservations

## Features

- **Admin Panel**: Manage reservations, block dates, and configure system settings
- **Email Integration**: SendGrid-powered email notifications for reservation confirmation, reminders, and updates
- **Calendar Integration**: Export reservations to Google Calendar, Apple Calendar, and Outlook
- **Time Slot Management**: Configure available time slots and daily limits
- **Responsive Design**: Mobile-friendly interface for both administrators and participants
- **User Authentication**: Secure admin access with ASP.NET Identity
- **Form Validation**: Client and server-side validation for all user inputs

## Screenshots

*[Screenshots would be added here]*

## Installation

### Prerequisites

- Visual Studio 2019 or newer
- SQL Server (Express edition or higher)
- .NET Framework 4.7.2
- IIS Express (for development) or IIS (for production)

### Setup Steps

1. Clone the repository:
   ```
   git clone https://github.com/deadcast2/jeanie-reservation-system.git
   ```

2. Open the solution file `jeanie.sln` in Visual Studio.

3. Restore NuGet packages:
   ```
   Update-Package -reinstall
   ```

4. Configure the connection string in `Web.config` to point to your SQL Server instance:
   ```xml
   <connectionStrings>
     <add name="JeanieContext" connectionString="Server=YOUR_SERVER\SQLEXPRESS;Database=jeanie;Trusted_Connection=True;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

5. Configure environment variables for SendGrid integration:
   - `SENDGRID_API_KEY`: Your SendGrid API key
   - `DEFAULT_EMAIL`: The default "from" email address for notifications

6. Run Entity Framework migrations to create the database:
   ```
   Update-Database
   ```

7. The default admin account is:
   - Email: admin@example.com
   - Password: Temp123!
   
   **Important**: Change this password immediately after first login!

8. Execute the formatted_reservation_view.sql to generate the view needed for reservations

## Configuration

In the `Web.config` file, you can adjust the following settings:

```xml
<appSettings>
    <add key="HoursNotice" value="24" />
    <add key="HoursInAdvance" value="72" />
    <add key="StartHour" value="9" />
    <add key="EndHour" value="20.5" />
    <add key="TimeSlotSize" value="3" />
    <add key="TimeIncreament" value="0.5" />
</appSettings>
```

- `HoursNotice`: Hours before reservation to send reminder email
- `HoursInAdvance`: Minimum hours in advance for making a reservation
- `StartHour`: First hour of the day for available time slots (24-hour format)
- `EndHour`: Last hour of the day for available time slots (24-hour format)
- `TimeSlotSize`: Duration of each time slot in hours
- `TimeIncreament`: Increments between time slots in hours

## Customization

### Email Templates

Email templates can be customized through the admin interface. Navigate to Settings → Email Template to modify:

- Email subject line
- Email body content

The system supports the following placeholders:
- `$name`: Replaced with the participant's first name
- `$link`: Replaced with a link to the reservation

### Attachments

You can customize the attachments sent with confirmation emails by replacing the files in:
```
/Content/attachments/
```

## Security Features

- ASP.NET Identity for authentication and authorization
- CSRF protection with anti-forgery tokens
- SQL injection protection with parameterized queries
- Strong password requirements
- Account lockout after failed login attempts
- Secure cookies

## Deployment

### IIS Deployment

1. Publish the application from Visual Studio:
   - Right-click the project → Publish
   - Choose "Folder" as the publish target
   - Configure the publish location

2. In IIS Manager:
   - Create a new website or application
   - Set the physical path to the published folder
   - Configure the application pool to use .NET Framework 4.7.2
   - Set up bindings as needed

3. Set environment variables on the server:
   - `SENDGRID_API_KEY`
   - `DEFAULT_EMAIL`

### AWS/Azure Deployment

Detailed instructions for cloud deployment are available in the [Deployment Guide](DEPLOYMENT.md).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [Bootstrap](https://getbootstrap.com/) - Frontend framework
- [jQuery](https://jquery.com/) - JavaScript library
- [SendGrid](https://sendgrid.com/) - Email service
- [FullCalendar](https://fullcalendar.io/) - Calendar display
- [Summernote](https://summernote.org/) - WYSIWYG editor