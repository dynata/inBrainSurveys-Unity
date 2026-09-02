
# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and `InBrainSurveys` adheres to [Semantic Versioning](http://semver.org/).

## [3.1.0](https://github.com/inbrainai/unitysdk/releases/tag/v.3.1.0) - 2026-09-02

### Added
- Native Offers Support

### Changed
- Added redirect to `missing session` error
- Android native dependency updated to `com.github.inbrainai:sdk-android:3.1.3`.
- iOS native dependency updated to `InBrainSurveys 3.0.3`.

## [3.0.1](https://github.com/inbrainai/unitysdk/releases/tag/v.3.0.1) - 2026-08-12

### Changed
- Android native dependency updated to `com.github.inbrainai:sdk-android:3.1.2`.
- iOS native dependency updated to `InBrainSurveys 3.0.2` and shipped as `InBrainSurveys.xcframework` (replacing the device-only `.framework`).

### Fixed
- Error `missing session`
- White screen after `Forfeit reward` 

## [3.0.0](https://github.com/inbrainai/unitysdk/releases/tag/v.3.0.0) - 2026-05-26

### Added
- **New Wall Function:**
    - `InBrainWallOption` enum (`ALL`, `SURVEYS`, `OFFERS`)
    - `OpenWall(InBrainWallOption wallOption = InBrainWallOption.ALL)` function to `InBrain`
- **New Survey Function:**
    - `OpenSurvey(string surveyId, string searchId, bool offersEnabled = true)` function to `InBrain`

### Changed
- Android native dependency updated to `com.github.inbrainai:sdk-android:3.1.0`.
- iOS native framework updated to `InBrainSurveys 3.0.0`.
- Demo scene and demo script updated to align with current SDK flow.

### Deprecated
- `ShowSurveys()` - use `OpenWall(InBrainWallOption wallOption = InBrainWallOption.ALL)` instead
- `ShowSurvey(string surveyId, string searchId)` - use `OpenSurvey(string surveyId, string searchId, bool offersEnabled = true)` instead

### Removed
- Embedded Play Services Resolver binaries and bundled Android `.aar/.jar` artifacts in favor of EDM-based dependency resolution.

## [2.1.0](https://github.com/inbrainai/unitysdk/releases/tag/v.2.1.0) - 2023-10-20

### Added
- New `isProfilerSurvey` property for `InBrainSurvey` entity.

### Changed
- Android and iOS libraries updated in order to improve plugin stability and performance on both platforms.
- Existing `start` and `end` properties of `InBrainCurrencySale` entity have been deprecated.

## [2.0.1](https://github.com/inbrainai/unitysdk/releases/tag/v.2.0.1) - 2023-04-04

### Removed
- Redundant bridging header for iOS.
---

## [2.0.0](https://github.com/inbrainai/unitysdk/releases/tag/v.2.0.0) - 2023-02-14

### Added
- New `Init` method for extra InBrain SDK initialization flexibility.
- New `GetCurrencySale` method for fetching info regarding active currency sales.
- New `GetSurveysWithFilter` method for fetching active surveys list matching certain criteria.
- New `SetSessionId` method for tracking user sessions.
- New properties for `InBrainSurvey` entity.
---

### Changed
- Android and iOS libraries updated in order to improve plugin stability and performance on both platforms.
- Callback for handling WebView dismiss result.
- Existing `SetAppUserId` method doesn't initialize InBrain SDK anymore.
- Existing `SetLanguage` method has been deprecated.
- Existing `SetCustomData` method has been deprecated. 
- Existing `GetSurveys` methods have been deprecated.
---

### Removed
- `ShowSurvey` method which takes a single `surveyId` input parameter.
---

## [1.5.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.5.0) - 2022-03-08

### Added
- New `CheckSurveysAvailability` method for checking if there are any surveys available.
- New `SetCustomData` method for adding custom tracking and demographic data to inBrain session.
---

### Changed
- Android and iOS libraries updated in order to unify plugin behavior on both platforms.
- Native surveys methods now accept `placementId` parameter.
---

## [1.4.2](https://github.com/inbrainai/unitysdk/releases/tag/v.1.4.2) - 2021-05-31

### Changed
- Android and iOS libraries updated in order to add landscape orientation support for surveys wall.
---

## [1.4.1](https://github.com/inbrainai/unitysdk/releases/tag/v.1.4.1) - 2020-01-26

### Fixed
- Internal color conversion method for iOS.
---

## [1.4.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.4.0) - 2020-01-22

### Added
- New `GetSurveys` method for fetching available surveys list.
- New `ShowSurvey` for presenting specified survey in a web view.
---

## [1.3.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.3.0) - 2021-01-21

### Added
- New surveys wall UI customization methods - `SetStatusBarConfiguration` and `SetToolbarConfiguration`.
---

## [1.2.2](https://github.com/inbrainai/unitysdk/releases/tag/v.1.2.2) - 2020-08-05

### Fixed
- Build issues on with IL2CPP configuration for Android.
---

## [1.2.1](https://github.com/inbrainai/unitysdk/releases/tag/v.1.2.1) - 2020-08-04

### Fixed
- Plugin importing issues on Windows.
---

## [1.2.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.2.0) - 2020-08-03

### Added
- New `SetLanguage` method for changing surveys wall display language.
---

## [1.1.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.1.0) - 2020-07-17

### Changed
- Android and iOS libraries updated in order to unify plugin behavior on both platforms.
---

## [1.0.1](https://github.com/inbrainai/unitysdk/releases/tag/v.1.0.1) - 2020-02-18

### Changed
- Android library updated.
---

## [1.0.0](https://github.com/inbrainai/unitysdk/releases/tag/v.1.0.0) - 2020-02-05

### Added
- Basic InBrain functionality - surveys wall presentation, rewards fetching and confirmation.
---

