# RDW Techday Registration Site

This site is a RDW specific implementation of a congress administration system and offers the following features:
* Entering and displaying the Tracks, Sessions, Rooms, Timeslots
* Registration of RDW emloyees through self service and registration of non-RDW through an invite by e-mail
* Guarding the number of attendants per room as defined by the room capacity
* Guarding of the total number of attendants as defined by the venue capacity
* Automated printing of badges for the event organisation, attendants and speakers. The badges of attendants also contain their selected sessions so it is easy to check if they are registered for a session
* Administration pages to check delegates, attendance to sessions and the overall state of the registrations

## How to setup
* Clone the project to your Visual Studio 
* Copy the ```secrets.json.example``` to ```secrets.json``` and fill in the right values for the parameters. 
* Run, you can log in with the default account you set up in the secrets.json. This is the default admin account. You can use this to add more admins (after they registered) and do all the other stuff necessary.

## The configuration data
The app has a number of Models that need to be filled before you can release it to the public. The app will initialize the database with a dataset that worked well in the past and fits the default venue that we use.
The Models in the app are:
* **Tijdvak** A Tijdvak (Timeslot) defines a Timerange in which (a part of) of a session can be given. Timeslot can be combined in Session (e.g. a workshop that needs wo timeslots). You need to give the list of timeslots the right values in the beginning. Timeslot is used by a lot of other classes.  Changing the timeslot, basically means reentering all your data and reregistering all the attendees. So be warned. 
* **Ruimte** A Ruimte (space) is a location or room in which a sessie is being given.
* **Track** A track is a series of sessions that fit together (So a track on software with lectures on software, a track on Infrastructure with infra sessions etc) 
* **Sessie** A Sessie (Session) defines a session in a certain track in a certain Ruimte spanning one or more Tijdvakken
* **ApplicationUser** is a user of the system can be either an Admin or a Normal uses defined on which role is assigned. It Derives from Identity.IdentityUser. 
   Here is some fiddly stuff. When the user is created. the ApplicationUserTijdVakken for that user will also be created. These AUT classes are used
   to track whcih timeslots are empty/used for joining sessions. A more elgant solution here is most likely possible. This was an effective soluton for the
   current scope of features. The Drawback of this is that you cannot add or delete Tijdvakken once users have been registered!!
* **ApplicationUserTijdvak** The timeslots used bij a user and which links them to a session. Read the explanation above.
* **SessieTijdvak** The timeslots needed for a session
* **Maxima** This model stores the allowed maximum number of users (internal and external) is implemented as a singleton and will only store 1 record in the database.


