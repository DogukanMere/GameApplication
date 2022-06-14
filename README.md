# INTO TO DARKNESS
## Passion Project - 2022 - ASP.NET

**This project is created for an imaginary video game which I named "Into the Darkness"**

#### Running the Project:
- Verify that the project contains an App Data folder => right-click solution => View in File Explorer
- Tools => Nuget Package Manager => Package Manage Console => type: Update-Database
- Make sure the database was created: go through steps [View => SQL Server Object Explorer => MSSQLLocalDb => and relevant database]

#### Main Components
**This is my passion project, and it was influenced by video games in general.**
**For this project, there are three sections.**
1. **Creatures**
2. **Races**
3. **Dungeons**

##### Logic:
- **Each creature is a member of a race**

- **Many creatures can share the same race**

- **Dungeons can contain a variety of monsters**

- **Creatures can reside in multiple dungeons**

#### Admin Features
**CRUD(create, read, delete, update) functionality is applied to this project to be able to manage all content**

**Adding a race, dungeon, creature:**
- Once you are on "Races"|"Dungeons" or "Creature" tab, you can add a new race|dungeon|creature simply clicking "New (Race|Dungeon|Creature)"
- It will ask you for filling up a form which requires some information, and click "Create" to add this to the system
- You can view this (race|dungeon|creature) in relevant tab showing available ones

**Updating a race, dungeon, creature:**
- Once you are on a specific race, dungeon or creature, you can edit information by clicking "Update" button 
- It will ask you for filling up a form which requires "Dungeon Name - Dungeon Location", and click "Create Dungeon" to add this dungeon to the system
- You can view this dungeon in Dungeons tab showing dungeons available

