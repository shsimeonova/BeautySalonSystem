Beauty Salon System is a simple appointment and offers management system for beauty salon administrators and clients.\

The application has admin and user interface.

Users can:
- View offers
- Request appointments for offers
- Receive emails for appointments confirmation (implemented with rabbitmq)
- View confirmed appointments in their profile

Admins can:
- Add and delete products
- Add and delete offers
- Confirm appointments

User authentication is implemented via IS4. All microservices use single sign on.
The application will start with 2 users - admin and _simona_ (passwords visible on the login form)

Some products and offers will be seeded to. 

You can replace email sftp credentials in the config, so do other properties you find fit for your env.