//
//  InBrainObjProxyViewController.h
//  Unity-iPhone
//
//  Created by Ivan Tustanivsky on 13/01/20.
//

#import "InBrainSurveys/InBrainSurveys-Swift.h"

NS_ASSUME_NONNULL_BEGIN

@interface InBrainProxyViewController : UIViewController<InBrainDelegate>

@property(nonatomic, copy, nullable) void (^onRewardsReceived)(NSString* rewards);
@property(nonatomic, copy, nullable) void (^onRewardsViewDismissed)(NSString* dismissedResult);

@property(nonatomic, copy) NSString* surveyId;
@property(nonatomic, copy) NSString* searchId;

@property(nonatomic, assign) InBrainWallOption wallOption;
@property(nonatomic, assign) BOOL offersEnabled;

@end

NS_ASSUME_NONNULL_END
