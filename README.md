# InBrain Surveys Unity SDK

InBrain Surveys Unity SDK allows to present native web view with surveys from within your Unity app and receive rewards for their completion.

## Supported platforms

- iOS
- Android

## Requirements

Unity 2018.4.14f1+

## Installation

Download latest version of InBrain Surveys Unity SDK (.unitypackage). In Unity editor right click inside Project pane and select Import package -> Custom package. Select file you've downloaded and confirm. Now InBrain Surveys Unity SDK should be ready to use.

## Configuration

There are some prerequisites for InBrain Surveys Unity SDK:
- API ClientID (Provided in inBrain.ai dashboard)
- API Secret (Provided in inBrain.ai dashboard)
- Server to Server (flag indicating whether your app using S2S callbacks for inBrain rewards)

In Unity editor go to Window -> InBrain -> Edit Settings and enter those values in corresponding input fields.

## Usage

### Setup

In order to access SDK functionality InBrain singleton object required. One will be instantiated automatically after referencing its 'Instance' method for the first time.
Also, provide unique user ID before calling any other methods to specify which player should receive rewards.

```
InBrain.Instance.Init();
InBrain.Instance.SetAppUserId(appUserId);
```

### Callbacks

Set global callback that will be triggered every time when new rewards received or surveys web view dismissed.

```
InBrain.Instance.AddCallback(rewards => {
	// process rewards...
}, result => { 
	// handle web view dismissal result... 
});
```

Optionally you can specify whether to confirm received rewards automatically (by default they are not).

### Check surveys availability

Check if there are any surveys available before presenting inBrain experience:

```
InBrain.Instance.CheckSurveysAvailability(flag =>
{
	// handle result...
});
```

### Show surveys

Present surveys web view (wall) with the following call:

```
InBrain.Instance.ShowSurveys();
```

Optionally specify wall option:

```
InBrain.Instance.ShowSurveys(InBrainWallOption wallOption);
```

Where `InBrainWallOption` is an enum with the following options: `ALL`, `OFFERS` and `SURVEYS`. By default, the value is set to `ALL`.

### Get rewards

There are two ways to receive rewards that user earned during surveys completion:

- use global callback (described above)
- request rewards manually with GetRewards method

```
// Global callback will be triggered
InBrain.Instance.GetRewards();

// Provide callbacks as parameters
InBrain.Instance.GetRewards(rewards => {
	// process rewards...
}, () => { 
	// process error... 
});

```

### Confirm rewards

Processed rewards should be confirmed in order to avoid receiving them repeatedly.

```
InBrain.Instance.ConfirmRewards(rewardsList);
```

This call should always be made following rewards data processing.

## Customization

SDK provides additional means that allow surveys wall UI customization.

### Status bar

```
var statusBarConfig = new InBrainStatusBarConfig
{
	StatusBarColor = Color.green,
	LightStatusBarIcons = false,
	HideStatusBarIos = false
};
InBrain.Instance.SetStatusBarConfig(statusBarConfig);
```

### Toolbar

```
var statusBarConfig = new InBrainStatusBarConfig
{
	StatusBarColor = Color.green,
	LightStatusBarIcons = false,
	HideStatusBarIos = false
};
InBrain.Instance.SetStatusBarConfig(statusBarConfig);
```

### Native surveys

There is a possibility to open only one specific survey in a web view. In order to that list of available surveys should be requested first by calling a `GetSurveys` method.

```
InBrain.Instance.GetSurveys(OnSurveysFetched);
```

Each recieved survey has its own `id` and `searchId` values that should be passed to `ShowSurvey` method.

```
InBrain.Instance.ShowSurvey(surveyId, searchId);
```

Optionally specify whether to enable offers:

```
InBrain.Instance.ShowSurvey(surveyId, searchId, bool offersEnabled);
```

By default, offersEnabled value is set to `true`.

Alternatively one can fetch list of available surveys that metch certain criteria (i.e. survey category) by calling the `GetSurveysWithFilter` method instead.

```
var filter = new InBrainSurveyFilter("SURVEY_PLACEMENT_ID", new List<InBrainSurveyCategory> { InBrainSurveyCategory.SocialResearch });
InBrain.Instance.GetSurveysWithFilter(filter, OnSurveysFetched);
```

### Currency sales

The plugin provides an API allowing to get active currency sale. This can be done with method `GetCurrencySale`.

```
InBrain.Instance.GetCurrencySale(sale => {
	// process currency sale...
});
```
