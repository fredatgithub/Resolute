# New Year's Resolution Tracker - Implementation Plan

## Overview

A .NET 10.0 console application that helps users track and stay accountable to their New Year's resolutions through scheduled reminders and progress tracking.

## Core Features

### 1. Resolution Management

- Add new resolutions with description, target date, and reminder settings
- Edit existing resolutions
- Mark resolutions as complete
- Delete resolutions
- View all resolutions with status

### 2. Reminder System

- **Interval-based reminders**: Check in every N days
- **Date-based reminders**: Specific dates for milestones
- **Smart scheduling**: Calculate next reminder date based on last check-in
- Display upcoming reminders on app launch

### 3. Progress Tracking

- Record check-ins with notes
- Track completion status
- View history of check-ins per resolution
- Statistics: completion rate, streak tracking

## Data Model

### Resolution Structure (JSON)

```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "category": "string",
  "createdDate": "datetime",
  "targetDate": "datetime?",
  "isCompleted": "bool",
  "completedDate": "datetime?",
  "reminderSettings": {
    "type": "interval | specific-dates | both",
    "intervalDays": "int?",
    "specificDates": ["datetime[]"]
  },
  "checkIns": [
    {
      "date": "datetime",
      "status": "on-track | struggling | completed",
      "notes": "string"
    }
  ]
}
```

### File Structure

- `resolutions.json` - Main data file with all resolutions
- `settings.json` - User preferences (notification preferences, etc.)

## Application Architecture

### Menu System

1. **Main Menu**
   - View all resolutions
   - Add new resolution
   - Check in on resolution
   - View upcoming reminders
   - Statistics & reports
   - Settings
   - Exit

2. **Resolution Details View**
   - Show full resolution info
   - Progress history
   - Actions: Check-in, Edit, Complete, Delete

### Key Components

1. **ResolutionManager**
   - CRUD operations for resolutions
   - Load/save JSON data

2. **ReminderService**
   - Calculate next reminder dates
   - Check for due reminders
   - Display reminder notifications

3. **CheckInService**
   - Record progress check-ins
   - Update resolution status
   - Calculate next reminder after check-in

4. **StatisticsService**
   - Calculate completion rates
   - Track streaks
   - Generate reports

5. **JsonDataService**
   - Handle file I/O
   - Serialize/deserialize data
   - Ensure data integrity

## User Workflow Examples

### Adding a Resolution

1. User selects "Add new resolution"
2. Enter title, description, category
3. Set target completion date (optional)
4. Configure reminders:
   - Choose interval (e.g., check in every 7 days)
   - Or add specific milestone dates
5. Save and confirm

### Daily Launch Experience

1. App shows welcome message
2. Display any resolutions needing check-in today
3. Show upcoming reminders (next 7 days)
4. Main menu for other actions

### Check-in Process

1. User selects resolution to check in on
2. App shows last check-in and days since
3. User provides status update (on track/struggling/completed)
4. Add optional notes
5. App calculates and shows next reminder date

## Implementation Phases

### ✅ Phase 0: Setup and Dependencies

- Add TimeWarp.Nuru NuGet package (version 3.x latest prerelease (3.0.0-beta.23))
  - Source: <https://feed.nuget.org/packages/TimeWarp.Nuru>
  - This will provide enhanced console UI capabilities for the application
- Verify package installation and basic setup

### ✅ Phase 1: Core Data Layer

- Create data models (classes)
- Implement JSON serialization/deserialization
- Basic file I/O operations
- CRUD for resolutions

### ✅ Phase 2: Basic UI & Management

- Console menu system
- Add/view/edit resolutions
- Basic data validation
- Error handling

### ✅ Phase 3: Reminder System

- Reminder calculation logic
- Date/interval handling
- Display due reminders on launch
- Manual check-in functionality

### ✅ Phase 4: Progress Tracking

- Check-in recording
- Status updates
- View history

### ✅ Phase 5: Statistics & Polish

- Completion rate calculations
- Streak tracking
- Reports and summaries
- Settings/preferences
- Input validation refinements
- User experience improvements
- **TimeWarp.Nuru** (v2.1.0-beta.1) - Enhanced console UI framework (PRIMARY)
- `System.Text.Json` - For JSON operations (built-in)
- `System.IO` - File operations

## Technical Considerations

### Libraries/Packages

- `System.Text.Json` - For JSON operations (built-in)
- `System.IO` - File operations
- Consider: Spectre.Console for enhanced console UI (optional)

### Error Handling

- File not found scenarios
- Invalid JSON recovery
- Date validation
- Graceful degradation

### Data Persistence

- Auto-save after each operation
- Backup mechanism for data safety
- Migration strategy if data model changes

### Testing Strategy

- Unit tests for business logic
- Test data generation
- Edge case handling (leap years, date boundaries, etc.)

## Future Enhancements (Post-MVP)

- Export to CSV/PDF reports
- Categories and tags
- Recurring resolutions
- Shared resolutions (team goals)
- Integration with calendar apps
- Web/mobile companion app
- Motivational quotes on check-ins
- Achievement badges/gamification
