﻿# PlayerManagement  
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/C0C11EVSE0)
<br>
---
[![SonarQube Cloud](https://sonarcloud.io/images/project_badges/sonarcloud-highlight.svg)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=TomasiDeveloping_PlayerManagement)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
<br>
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=bugs)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=TomasiDeveloping_PlayerManagement&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=TomasiDeveloping_PlayerManagement)

## Changelog  

All notable changes to this project are documented here.  
This project is currently in the **Beta Phase**.  

---

### **[0.11.0]** - *2025-06-19*  
#### ✨ Added  
- **Squad Management per Player:** You can now add up to **four squads per player**, each with:  
  - **Power** (in millions)  
  - **Position**  
  - **Type**: Tank, Aircraft, Missile, or Mixed  
- **Visual Enhancements:** Squad types are displayed with custom icons and total power is summarized at the top of the squad card.

🛠️ **Fixed**  
- (N/A)

---

### **[0.10.0]** - *2025-06-02*  
#### ✨ Added  
- **Team Selection for Desert Storm:** You can now assign **Team A** or **Team B** when creating a Desert Storm entry.  
- **Team Display in Table:** The selected team is now also shown in the Desert Storm overview table.

🛠️ **Fixed**  
- (N/A)

---

### **[0.9.1]** - *2025-04-24*  
#### ✨ Added  
- **Number Formatting Input Mask:** Numbers are now automatically formatted for better readability (e.g., `250000` → `250.000`).  

🛠️ **Fixed**  
- Fixed an issue where the selected category was not saved correctly when updating a custom event.

---

### **[0.9.0]** - *2025-04-23*  
#### ✨ Added  
- **Event Categories:** Custom events can now be assigned to specific categories.  
- **Category-Based Leaderboards:** Each category can have its own leaderboard, depending on the event type (e.g., participation-only, points-only, or a combination).  

🛠️ **Fixed**  
- Minor display issues in the event and leaderboard sections have been resolved.

---

### **[0.8.3]** - *2025-04-10*  
#### ♻️ Changed  
- **NuGet Packages:** Updated all dependencies, including Serilog and Seq, to their latest stable versions.  
- **Logging Setup:** Cleaned up and reorganized the Serilog configuration for improved clarity and maintainability.  

🛠️ **Fixed**  
- (N/A)

---


### **[0.8.0]** - *2025-03-11*  
#### ✨ Added  
- **Feedback Page:** Users can now submit feedback, including bug reports and feature requests.  
- **GitHub Integration:** Feedback is automatically created as a GitHub issue, providing a direct link to track progress.  
- **Screenshot Upload:** Users can attach a screenshot to better illustrate issues.  
- **Success Message:** After submission, the form hides, and a success message with the GitHub issue link is displayed.  

🛠️ **Fixed**  
- (N/A)

---

### **[0.7.0]** - *2025-02-06*  
#### ✨ Added  
- **MVP Page**: A new page has been introduced where players can be loaded and a list of MVPs is displayed based on a calculation formula.
- **Filter Options**: You can now display the entire alliance, only R4/R5 members, or just players without R5/R4.
- **API Key**: A new tab has been added under the alliance section, allowing users to generate an API key to access endpoints and integrate them into their own systems.
- **MVP List Endpoint**: The API endpoint for fetching the MVP list is now available.

#### 🛠️ Fixed  
- *(N/A)*

---

### **[0.6.1]** - *2025-01-28*  
#### ✨ Added  
- *(N/A)*  

#### 🛠️ Fixed  
- **VS Table**: Resolved a bug that caused incorrect data display.

---

### **[0.6.0-beta]** - *2025-01-28*  
#### ✨ Added  
- **Pagination**: Implemented and adjusted for all tables.  
- **Zombie Siege**: Expanded the table to display all waves survived by the entire alliance.  

#### 🛠️ Fixed  
- *(N/A)* 

---

### **[0.5.1-beta]** - *2025-01-27*  
#### ✨ Added  
- *(N/A)*

#### 🛠️ Fixed  
- *Zombie siege**: Smile customised for exactly 20 waves

---

### **[0.5.0-beta]** - *2025-01-21*  
#### ✨ Added  
- **Player VS Duel**: In the player detail view, the VS points can now be viewed as a bar chart.

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.4.1-beta]** - *2025-01-21*  
#### ✨ Added  
- **Excel Import**: Players can now be imported via Excel.  

#### 🛠️ Fixed  
- **Week Pipe Logic**: Corrected calculation logic for weekly processing.  

---

### **[0.4.0-beta]** - *2025-01-20*  
#### ✨ Added  
- **Player Dismissal Page**: A new GUI page was added for dismissing players.  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.6-beta]** - *2025-01-16*  
#### ✨ Added  
- **Player Dismissal Function**: Core dismissal functionality implemented.  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.5-beta]** - *2025-01-09*  
#### ✨ Added  
- *(N/A)*  

#### 🛠️ Fixed  
- **MVP Formula**: Corrected MVP calculation formula.  

---

### **[0.3.4-beta]** - *2025-01-07*  
#### ✨ Added  
- **Alliance MVP Calculation**: Implemented MVP calculation for alliances with API endpoint support.  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.3-beta]** - *2024-12-17*  
#### ✨ Added  
- **Custom Event**: Introduced custom event functionality.  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.2-beta]** - *2024-12-03*  
#### ✨ Added  
- **Event Progress**: "In Progress" status added to events.  
- **League Details in VS Duel**: Added league tiers (e.g., Silver, Gold, Diamond).  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.1-beta]** - *2024-11-28*  
#### ✨ Added  
- **Zombie Siege Event**: Introduced a new Zombie Siege event.  

#### 🛠️ Fixed  
- *(N/A)*  

---

### **[0.3.0-beta]** - *2024-11-26*  
#### ✨ Added  
- **Initial Beta Release**: Core features introduced in the first beta release.  

#### 🛠️ Fixed  
- *(N/A in the initial release)*  
