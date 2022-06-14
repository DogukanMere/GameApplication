# INTO TO DARKNESS
## Passion Project - 2022 - ASP.NET

**This project is created for an imaginary video game which I named "Into the Darkness"**

### Running the Project:
- Verify that the project contains an App Data folder => right-click solution => View in File Explorer
- Tools => Nuget Package Manager => Package Manage Console => type: Update-Database
- Make sure the database was created: go through steps [View => SQL Server Object Explorer => MSSQLLocalDb => and relevant database]

### Main Components:
**This is my passion project, and it was influenced by video games in general.**
**For this project, there are three sections.**
1. Creatures
2. Races
3. Dungeons

### Logic:
- Each creature is a member of a race
- Many creatures can share the same race
- Dungeons can contain a variety of monsters
- Creatures can reside in multiple dungeons

### Features:
**Creature:**
1. After adding a new creature, you can upload an image representing the creature (Admin only)
2. A creature can be assign to more than a dungeon or remove from any dungeon. (Admin only)
3. When a new creature added, you can choose listed races which are in the system already (Admin only)
3. Based on creature powers, a bar will appear under power rate which represents how dangerous the creature is.
4. It is possible to determine the race each creature belongs to.

**Race:**
1. If you visit a race, you can check which creatures belong to that specific race.
2. You can check if a race is offensive or not which is given with green and red color. While green is showing that a race is inoffensive, red does opposite.

**Dungeon:**
1. When you visit a specific dungeon, you can see which creatures might be encountered in that dungeon which will let users to find any creature easily.
2. Each dungeon has location information, so users know where to find the entrance for all dungeons.

### Admin Features:
**CRUD(create, read, delete, update) functionality is applied to this project to be able to manage all content**

**Adding a race, dungeon, creature:**
- Once you are on "Races"|"Dungeons" or "Creature" tab, you can add a new race|dungeon|creature simply clicking "New (Race | Dungeon | Creature)"
- It will ask you for filling up a form which requires some information, and click "Create" to add this to the system
- You can view this (race | dungeon | creature) in relevant tab showing available ones

**Updating a race, dungeon, creature:**
- Once you are on a specific race, dungeon or creature, you can edit information by clicking "Update" button 
- After changing information, click update button

**Deleting a race, dungeon, creature:**
- Any species, race, or dungeon can be visited, and a delete button will show on the page, just below the update button.



