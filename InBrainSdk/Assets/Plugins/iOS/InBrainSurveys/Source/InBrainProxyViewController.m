//
//  InBrainProxyViewController.m
//  Unity-iPhone
//
//  Created by Ivan Tustanivsky on 13/01/20.
//

#import "InBrainProxyViewController.h"
#import "InBrainJsonUtils.h"

@interface InBrainProxyViewController ()

@end

@implementation InBrainProxyViewController

InBrain* inBrain;
bool isOpened = false;

- (InBrainProxyViewController*)init {
    self = [super init];
    inBrain = [InBrain shared];
    inBrain.inBrainDelegate = self;
    return self;
}

- (void)viewDidAppear:(BOOL)animated {
    [super viewDidAppear:animated];
    
    if (!isOpened) {
        if (_shouldOpenOffer) {
            [inBrain openOfferWithId:_offerId success:^{} failed:^(NSError * error) {
                NSLog(@"Failed to open inBrain offer: %@", error.localizedDescription);
            }];
        }
        else if([_surveyId length] == 0) {
            [inBrain openWallWith:_wallOption from:self];
        }
        else {
            [inBrain showNativeSurveyWithId:_surveyId searchId:_searchId offersEnabled:_offersEnabled from:self];
        }
        isOpened = true;
    }
}

- (void)didReceiveInBrainRewardsWithRewardsArray:(NSArray<InBrainReward*>* _Nonnull)rewardsArray {
    NSString* rewards = [InBrainJsonUtils serializeRewards:rewardsArray];
    
    if (_onRewardsReceived != nil) {
        _onRewardsReceived(rewards);
    }
}

- (void)didFailToReceiveRewardsWithError:(NSError * _Nonnull)error {

}

- (void)surveysClosedByWebView:(BOOL)byWebView completedSurvey:(BOOL)completedSurvey rewards:(NSArray<InBrainSurveyReward *> * _Nullable)rewards {
    [self dismissViewControllerAnimated:NO completion:nil];
    isOpened = false;
    
    NSString* webViewDismissedResult = [InBrainJsonUtils serializeRewardsViewDismissedResult:rewards byWebView:byWebView];
    
    if (_onRewardsViewDismissed) {
        _onRewardsViewDismissed(webViewDismissedResult);
    }
}

@end
