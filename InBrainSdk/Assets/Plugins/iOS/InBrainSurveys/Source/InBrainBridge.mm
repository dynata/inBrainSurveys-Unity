//
//  InBrainBridge.cpp
//  InBrainSurveys_SDK_Swift
//
//  Created by Ivan Tustanivsky on 09/01/20.
//  Copyright © 2020 InBrain. All rights reserved.
//

#import "InBrainSurveys/InBrainSurveys-Swift.h"
#import "InBrainProxyViewController.h"
#import "InBrainFunctionDefs.h"
#import "InBrainUtils.h"
#import "InBrainJsonUtils.h"

#pragma mark - C interface

extern "C" {

    InBrainProxyViewController *inBrainView;

    void _ib_SetInBrain(char* clientId, char* secret, bool isS2S) {
            NSString* secretString = [InBrainUtils createNSStringFrom:secret];
            NSString* clientIdString = [InBrainUtils createNSStringFrom:clientId];
            
            inBrainView = [[InBrainProxyViewController alloc] init];
            inBrainView.modalPresentationStyle = UIModalPresentationFullScreen;
            
            [[InBrain shared] setInBrainWithApiClientID:clientIdString
                                              apiSecret:secretString
                                              isS2S:isS2S];
    }

    void _ib_SetInBrainWithUserId(char* clientId, char* secret, bool isS2S, char* userId) {
        NSString* secretString = [InBrainUtils createNSStringFrom:secret];
        NSString* clientIdString = [InBrainUtils createNSStringFrom:clientId];
        NSString* userIdString = [InBrainUtils createNSStringFrom:userId];
        
        inBrainView = [[InBrainProxyViewController alloc] init];
        inBrainView.modalPresentationStyle = UIModalPresentationFullScreen;
        
        [[InBrain shared] setInBrainWithApiClientID:clientIdString
                                          apiSecret:secretString
                                          isS2S:isS2S
                                          userID:userIdString];
    }

    void _ib_SetInBrainUserId(char* userId) {
        NSString* userIdString = [InBrainUtils createNSStringFrom:userId];
        [[InBrain shared] setWithUserID:userIdString];
    }

    void _ib_SetSessionId(char* sessionId) {
        NSString* sessionIdString = [InBrainUtils createNSStringFrom:sessionId];
        [[InBrain shared] setSessionID:sessionIdString];
    }

    void _ib_SetDataOptions(char* demographicDataJson) {
        NSMutableArray<NSDictionary<NSString*, id>*>* demographicData = [NSMutableArray new];
        if(demographicDataJson != nil) {
            NSDictionary* demographicDataDictionary = [InBrainJsonUtils deserializeDictionary:[InBrainUtils createNSStringFrom:demographicDataJson]];
            
            NSMutableDictionary *genderDict = [NSMutableDictionary dictionary];
            genderDict[@"gender"] = demographicDataDictionary[@"gender"];
            [demographicData addObject:genderDict];
            
            NSMutableDictionary *ageDict = [NSMutableDictionary dictionary];
            ageDict[@"age"] = demographicDataDictionary[@"age"];
            [demographicData addObject:ageDict];
        }
        
        [[InBrain shared] setDataOptions:demographicData];
    }
    
    void _ib_CheckSurveysAvailability(ActionBoolCallbackDelegate surveysAvailabilityCheckedCallback, void *surveysAvailabilityCheckedActionPtr) {
        [[InBrain shared] checkForAvailableSurveysWithCompletion:^(BOOL isAvailable, NSError* error) {
            surveysAvailabilityCheckedCallback(surveysAvailabilityCheckedActionPtr, isAvailable);
        }];
    }

    void _ib_ShowSurveys(int wallOption) {
        inBrainView.surveyId = @"";
        inBrainView.searchId = @"";
        
        inBrainView.wallOption = (InBrainWallOption)wallOption;
        
        [UnityGetGLViewController() presentViewController:inBrainView animated:NO completion:nil];
    }

    void _ib_ShowSurvey(char* id, char* searchId, bool offersEnabled) {
        NSString* surveyId = [InBrainUtils createNSStringFrom:id];
        NSString* srchId = [InBrainUtils createNSStringFrom:searchId];
        inBrainView.surveyId = surveyId;
        inBrainView.searchId = srchId;
        inBrainView.offersEnabled = offersEnabled;
        [UnityGetGLViewController() presentViewController:inBrainView animated:NO completion:nil];
    }

    void _ib_SetCallback(ActionStringCallbackDelegate rewardReceivedCallback,
                         void *rewardReceivedActionPtr,
                         ActionStringCallbackDelegate rewardViewDismissedCallback,
                         void *rewardViewDismissedActionPtr) {
        
        inBrainView.onRewardsReceived = ^(NSString* rewards) {
            rewardReceivedCallback(rewardReceivedActionPtr, [InBrainUtils createCStringFrom:rewards]);
        };
        
        inBrainView.onRewardsViewDismissed = ^(NSString* dismissedResult) {
            rewardViewDismissedCallback(rewardViewDismissedActionPtr, [InBrainUtils createCStringFrom:dismissedResult]);
        };
    }

    void _ib_RemoveCallback() {
        inBrainView.onRewardsReceived = nil;
        inBrainView.onRewardsViewDismissed = nil;
    }

    void _ib_GetRewards() {
        [[InBrain shared] getRewards];
    }

    void _ib_GetRewardsWithCallback(ActionStringCallbackDelegate rewardReceivedCallback, void *rewardReceivedActionPtr,
        ActionVoidCallbackDelegate failedToReceiveRewardsCallback, void *failedToReceiveRewardsActionPtr) {
        [[InBrain shared] getRewardsWithSuccess:^(NSArray<InBrainReward *> * _Nonnull rewardsArray) {
            NSString* rewards = [InBrainJsonUtils serializeRewards:rewardsArray];
            rewardReceivedCallback(rewardReceivedActionPtr, [InBrainUtils createCStringFrom:rewards]);
        } failed:^(NSError* error){
            failedToReceiveRewardsCallback(failedToReceiveRewardsActionPtr);
        }];
    }

    void _ib_ConfirmRewards(char* rewardsJson) {
        NSDictionary *rewardsDictionary = [InBrainJsonUtils deserializeDictionary:[InBrainUtils createNSStringFrom:rewardsJson]];
        NSArray<NSNumber *> *rewardsToConfirm = [InBrainJsonUtils deserializeNumbersArray:rewardsDictionary[@"ids"]];
        [[InBrain shared] confirmRewardsWithTxIdArray:rewardsToConfirm];
    }

    void _ib_SetNavigationBarConfig(char* title, int backgroundColor, int titleColor, int backButtonColor) {
        [[InBrain shared] setNavigationBarTitle:[InBrainUtils createNSStringFrom:title]];

        InBrainNavBarConfig* config = [[InBrainNavBarConfig alloc] initWithBackgroundColor:[InBrainUtils colorFrom:backgroundColor] buttonsColor:[InBrainUtils colorFrom:backButtonColor] titleColor:[InBrainUtils colorFrom:titleColor] isTranslucent:false hasShadow:false];

        [[InBrain shared] setNavigationBarConfig:config];
    }

    void _ib_SetStatusBarConfig(bool white, bool hide) {
        UIStatusBarStyle style;
        if (@available(iOS 13, *)) {
            style = white ? UIStatusBarStyleLightContent : UIStatusBarStyleDarkContent;
        } else {
            style = UIStatusBarStyleLightContent;
        }
        InBrainStatusBarConfig* config = [[InBrainStatusBarConfig alloc] 	initWithStatusBarStyle:style hideStatusBar:hide];
        [[InBrain shared] setStatusBarConfig:config];
    }

    void _ib_GetNativeSurveysWithFilterAndCallback(char* filterJson, ActionStringCallbackDelegate surveysReceivedCallback, void *surveysReceivedActionPtr,
        ActionVoidCallbackDelegate failedToReceiveSurveysCallback, void *failedToReceiveSurveysActionPtr) {
        NSDictionary* filterDataDictionary = [InBrainJsonUtils deserializeDictionary:[InBrainUtils createNSStringFrom:filterJson]];
        NSString* placeId = filterDataDictionary[@"placementId"];
        NSArray<NSNumber *> *categoryIds = [InBrainJsonUtils deserializeNumbersArray:filterDataDictionary[@"categoryIds"]];
        NSArray<NSNumber *> *excludedCategoryIds = [InBrainJsonUtils deserializeNumbersArray:filterDataDictionary[@"excludedCategoryIds"]];
        InBrainSurveyFilter* filter = [[InBrainSurveyFilter alloc] initWithPlacementId:placeId categoryIDs:categoryIds excludedCategoryIDs:excludedCategoryIds];
        [[InBrain shared] getNativeSurveysWithFilter:filter success:^(NSArray<InBrainNativeSurvey *> * _Nonnull surveysArray) {
            NSString* surveys = [InBrainJsonUtils serializeSurveys:surveysArray];
            surveysReceivedCallback(surveysReceivedActionPtr, [InBrainUtils createCStringFrom:surveys]);
        } failed:^(NSError* error) {
            failedToReceiveSurveysCallback(failedToReceiveSurveysActionPtr);
        }];
    }
    
    void _ib_GetCurrencySale(ActionStringCallbackDelegate currencySaleReceivedCallback, void *currencySaleReceivedActionPtr,
        ActionVoidCallbackDelegate failedToReceiveCurrencySaleCallback, void *failedToReceiveCurrencySaleActionPtr) {
        [[InBrain shared] getCurrencySaleWithSuccess:^(InBrainCurrencySale * _Nonnull currencySaleObject) {
            NSString* currencySale = [InBrainJsonUtils serializeCurrencySale:currencySaleObject];
            currencySaleReceivedCallback(currencySaleReceivedActionPtr, [InBrainUtils createCStringFrom:currencySale]);
        } failed:^(NSError* error) {
            failedToReceiveCurrencySaleCallback(failedToReceiveCurrencySaleActionPtr);
        }];
    }
}
