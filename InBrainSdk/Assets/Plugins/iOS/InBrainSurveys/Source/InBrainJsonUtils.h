//
//  InBrainJsonUtils.h
//  Unity-iPhone
//
//  Created by Ivan Tustanivsky on 14/01/20.
//

#import <Foundation/Foundation.h>
#import "InBrainSurveys/InBrainSurveys-Swift.h"

@interface InBrainJsonUtils : NSObject

+ (NSString *)serializeArray:(NSMutableArray *)array;
+ (NSString *)serializeDictionary:(NSDictionary *)dictionary;
+ (NSString *)serializeRewards:(NSArray<InBrainReward *> *)rewards;
+ (NSString *)serializeSurveys:(NSArray<InBrainNativeSurvey *> *)surveys;
+ (NSString *)serializeOffers:(NSArray<InBrainNativeOffer *> *)offers;
+ (NSString *)serializeCurrencySale:(InBrainCurrencySale *)currencySale;
+ (NSString *)serializeRewardsViewDismissedResult:(NSArray<InBrainSurveyReward *> *)rewards byWebView:(BOOL)status;

+ (NSArray *)deserializeArray:(NSString *)jsonArray;
+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic;
+ (NSArray<NSNumber *> *)deserializeNumbersArray:(NSArray *)numbers;

@end
