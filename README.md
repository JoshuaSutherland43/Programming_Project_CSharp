# Contact Monthly Claim System 🧭

In the creation of any System there are many key aspects that need to be thoroughly planned out and executed to ensure the overall success of the System in its entirety. 
These aspects range from the design of the System and its integrated user interface to the database structure, that will be used to store the variety of data being handled by the System, and finally clear understanding and recording of constraints for which the System needs to confine to throughout. 
In this document, I will cover these aspects in respect to a Contract Monthly Claim System (MCCS) designed to aid Independent Contractor (IC) lecturers. 

I have created a fully functional Monthly Claim application along with a database to accompany it.

## Explanation: 📊
The following application allows for lecturers to submmit monthly claims, this claim contains lecturer documents, hours of work and additional notes and information that the lecturer may have.
The claim will then be sent to the data base allowing for claim data to be stored securely by the use of tokens. The information of personal claims can be tracked in the lecturer claim tracker.
Here lecturers will also be able to view pending and accepted/ rejected claims (essentially a history and tracking page).

Admins will be allowed to change claim status as well as reject and approve claims, admins can also view and edit specific user information in the admin page.

All these features and functionality allow for a seemless experience that will allow for lecturers to comfortably and reliably issue a claim.

## GUI/ UI: 📺
Home Screen: 🏠
When designing the user interface my main goal was to create a comprehensible and simplistic approach to the environment for lecturers to interact with. When greeted with the home page, a text box is displayed perfectly centered in the screen welcoming the lecturer to submit a claim then and there. This approach allows the lecturer to easily and efficiently submit a claim without having to jump through hoops. 

Underneath the welcome box is a link for the user to sign up. This allows for the user to quickly and easily create an account and get closer to submitting a claim. 
![Screenshot 2024-10-18 224346](https://github.com/user-attachments/assets/a49b639e-1115-4e41-90b5-9d03f48461bb)

 

The home page also has a menu in the corner for lecturers to navigate to other tabs within the System, such as the Claim Tracker and login/ sign-up pages. For Admin a prompt will appear for admin to be directed to the Claim Acceptance page where admin can deny and accept lecturer claims. 





Login Page: 🧀
When the user interacts with the Login and Sign-Up pages, clear text boxes are displayed to the user that indicate the type of information needing to be entered. The buttons and textboxes offer responsive feedback. On either page the user may select to login or sign-up if their account is nonexistent or if it has been established. Documents may also be uploaded to the signup page as for the lecturers CV and Certificates. 

Login: 
![Screenshot 2024-10-18 224223](https://github.com/user-attachments/assets/0c7f17f3-172b-49dd-a91e-e7ae3ce7ab31)

 

Sign-up: 
![Screenshot 2024-10-18 224246](https://github.com/user-attachments/assets/297aeed2-ecdf-44a9-9873-56aca927b010)

 


Claim Tracking Page: 🐦
Finally, we have the Claim Tracking page, this page allows the user to see how far their claim is in the acceptance process. The user is clearly indicated by the flag, and this helps create a clear understanding of what stage the user is currently in. 

There are textboxes below the tracker to indicate to the user what each stage is and this will help the Lecturer know what went wrong wherever they are along the process. 
![Screenshot 2024-10-18 224423](https://github.com/user-attachments/assets/f3291e51-cb3c-4f36-bb18-3daa37f1cd0f)





Admin Page: ✔️
For the constant traffic of File submissions and retrieval I decided to incorporate an Azure File Share Storage as this will allow the database to retrieve and send large amount of data in the form of files. For my Lecturer information I decided to make use of Azure’s Table Storage service to keep track of all lecturer and administrative data. 

Image of the Admin page displaying both table data and a button to retrieve file information: 

![Screenshot 2024-10-18 224324](https://github.com/user-attachments/assets/d5cbeb40-0942-418f-bc2b-51137f47e39a)



## Databases: ✈️
SQL:
![sql](https://github.com/user-attachments/assets/7c78cba3-7a45-449c-afb5-32b447d05d76)


## Unit Tests: 🦄

### HomeClaim Test:
![home 1](https://github.com/user-attachments/assets/889e0d5b-7add-4dba-9f67-35645e9664ee)
![home 2](https://github.com/user-attachments/assets/331d2123-a4c7-4eaa-a87a-5e90253450e2)



### TrackingClaim Test:
![track](https://github.com/user-attachments/assets/d34e7173-ecf2-4781-a664-17a2e77510bf)



### Success Message: 🥇
![Screenshot 2024-10-18 225344](https://github.com/user-attachments/assets/a09fa5ab-dd48-4e97-ba0c-4a032dd34528)

#### Note*
The one of the unit test wont work in automation due to it being on a local database



## Video: 📷
https://youtu.be/Ne8LLMO6U3w?si=W0mgRlm0vGiZaECw


## Conclusion 🍎
With the following data implemented, I hope to implement more functionality in the next part.
Thank You!!
