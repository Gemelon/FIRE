# Copilot Instructions

## Repository Guidelines
- The project should be prepared for MIT licensing, with a copyright notice '(C) 2026 by Thomas Stoll' included where appropriate in source headers or documentation.

## Code Documentation
- All documentation and code comments must be written in English.

## Path Generation
- The user should decide whether a counter is used; a counter is only activated when the template contains placeholders such as `{Counter:D3}`.
- For photo and video collections, counter values must be persisted so deleted numbers are not reused and the image sequence remains stable.

## Application Structure
- The console app serves as a test/quick-start client for the API and should not play a role in UI applications; UI applications should directly integrate the FIRE library.
