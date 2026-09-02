//
//  InBrainJsonUtils.m
//  Unity-iPhone
//
//  Created by Ivan Tustanivsky on 14/01/20.
//

#import "InBrainJsonUtils.h"

@implementation InBrainJsonUtils

+ (NSString *)serializeArray:(NSArray *)array {
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:array options:nil error:&error];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

+ (NSString *)serializeDictionary:(NSDictionary *)dictionary {
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:nil error:&error];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

+ (NSString *)serializeRewards:(NSArray<InBrainReward *> *)rewards {
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];

    NSMutableArray *rewardsArray = [NSMutableArray arrayWithCapacity:rewards.count];

    for (NSUInteger i = 0; i < rewards.count; ++i) {
        NSMutableDictionary *reward = [NSMutableDictionary dictionary];
        reward[@"transactionId"] = @(rewards[i].transactionId);
        reward[@"amount"] = @(rewards[i].amount);
        reward[@"currency"] = rewards[i].currency;
        reward[@"transactionType"] = @(rewards[i].transactionType);
        [rewardsArray addObject:reward];
    }

    dictionary[@"rewards"] = rewardsArray;

    return [self serializeDictionary:dictionary];
}

+ (NSString *)serializeSurveys:(NSArray<InBrainNativeSurvey *> *)surveys {
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];

    NSMutableArray *surveysArray = [NSMutableArray arrayWithCapacity:surveys.count];

    for (NSUInteger i = 0; i < surveys.count; ++i) {
        NSMutableDictionary *survey = [NSMutableDictionary dictionary];
        survey[@"id"] = surveys[i].id;
        survey[@"searchId"] = surveys[i].searchId;
        survey[@"rank"] = @(surveys[i].rank);
        survey[@"time"] = @(surveys[i].time);
        survey[@"value"] = @(surveys[i].value);
        survey[@"currencySale"] = @(surveys[i].currencySale);
        survey[@"multiplier"] = @(surveys[i].multiplier);
        survey[@"categories"] = surveys[i].categoryIds;
        survey[@"conversionLevel"] = @(surveys[i].conversionLevel);
        survey[@"isProfilerSurvey"] = @(surveys[i].isProfilerSurvey);
        [surveysArray addObject:survey];
    }
    
    dictionary[@"surveys"] = surveysArray;

    return [self serializeDictionary:dictionary];
}

+ (NSDateFormatter *)utcDateFormatter {
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
    [dateFormatter setDateFormat:@"yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"];
    return dateFormatter;
}

+ (NSDictionary *)currencySaleDictionary:(InBrainCurrencySale *)currencySale {
    if (currencySale == nil) {
        return nil;
    }

    NSDateFormatter *dateFormatter = [self utcDateFormatter];
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    dictionary[@"title"] = currencySale.title;
    dictionary[@"multiplier"] = @(currencySale.multiplier);
    dictionary[@"description"] = currencySale.description;
    dictionary[@"startOn"] = [dateFormatter stringFromDate:currencySale.startOn];
    dictionary[@"endOn"] = [dateFormatter stringFromDate:currencySale.endOn];
    return dictionary;
}

+ (NSDictionary *)promotionDictionary:(InBrainOfferPromotion *)promotion {
    if (promotion == nil) {
        return nil;
    }

    return @{
        @"multiplier": @(promotion.multiplier),
        @"originalReward": @(promotion.originalReward),
        @"originalRewardString": promotion.originalRewardString ?: @""
    };
}

+ (NSDictionary *)goalDictionary:(InBrainOfferGoal *)goal {
    NSMutableDictionary *goalDict = [NSMutableDictionary dictionary];
    goalDict[@"id"] = @(goal.id);
    goalDict[@"title"] = goal.title ?: @"";
    goalDict[@"goalDescription"] = goal.goalDescription ?: @"";
    goalDict[@"reward"] = @(goal.reward);
    goalDict[@"rewardString"] = goal.rewardString ?: @"";
    goalDict[@"isCompleted"] = @(goal.isCompleted);
    goalDict[@"sortOrder"] = @(goal.sortOrder);
    goalDict[@"attributionWindowMinutes"] = @(goal.attributionWindowMinutes);

    if (goal.completeBy) {
        goalDict[@"completeBy"] = [[self utcDateFormatter] stringFromDate:goal.completeBy];
    }
    if (goal.promotion) {
        goalDict[@"promotion"] = [self promotionDictionary:goal.promotion];
    }

    return goalDict;
}

+ (NSArray *)goalDictionaries:(NSArray<InBrainOfferGoal *> *)goals {
    if (goals == nil || goals.count == 0) {
        return @[];
    }

    NSMutableArray *result = [NSMutableArray arrayWithCapacity:goals.count];
    for (InBrainOfferGoal *goal in goals) {
        [result addObject:[self goalDictionary:goal]];
    }
    return result;
}

+ (NSString *)serializeOffers:(NSArray<InBrainNativeOffer *> *)offers {
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    NSMutableArray *offersArray = [NSMutableArray arrayWithCapacity:offers.count];
    NSDateFormatter *dateFormatter = [self utcDateFormatter];

    for (NSUInteger i = 0; i < offers.count; ++i) {
        InBrainNativeOffer *nativeOffer = offers[i];
        NSMutableDictionary *offer = [NSMutableDictionary dictionary];
        offer[@"id"] = @(nativeOffer.id);
        offer[@"title"] = nativeOffer.title ?: @"";
        offer[@"reward"] = @(nativeOffer.reward);
        offer[@"rewardString"] = nativeOffer.rewardString ?: @"";
        offer[@"featuredRank"] = @(nativeOffer.featuredRank);
        offer[@"attributionWindowMinutes"] = @(nativeOffer.attributionWindowMinutes);
        offer[@"offerDescription"] = nativeOffer.offerDescription ?: @[];
        offer[@"instructions"] = nativeOffer.instructions ?: @[];
        offer[@"requirements"] = nativeOffer.requirements ?: @[];
        offer[@"tags"] = nativeOffer.tags ?: @[];
        offer[@"categories"] = nativeOffer.categories ?: @[];
        offer[@"standardGoals"] = [self goalDictionaries:nativeOffer.standardGoals];
        offer[@"purchaseGoals"] = [self goalDictionaries:nativeOffer.purchaseGoals];

        if (nativeOffer.thumbnailUrl) {
            offer[@"thumbnailUrl"] = nativeOffer.thumbnailUrl;
        }
        if (nativeOffer.heroImageUrl) {
            offer[@"heroImageUrl"] = nativeOffer.heroImageUrl;
        }
        if (nativeOffer.attemptedAt) {
            offer[@"attemptedAt"] = [dateFormatter stringFromDate:nativeOffer.attemptedAt];
        }
        if (nativeOffer.completeBy) {
            offer[@"completeBy"] = [dateFormatter stringFromDate:nativeOffer.completeBy];
        }
        if (nativeOffer.promotion) {
            offer[@"promotion"] = [self promotionDictionary:nativeOffer.promotion];
        }
        if (nativeOffer.campaignCurrencySale) {
            offer[@"campaignCurrencySale"] = [self currencySaleDictionary:nativeOffer.campaignCurrencySale];
        }

        [offersArray addObject:offer];
    }

    dictionary[@"offers"] = offersArray;
    return [self serializeDictionary:dictionary];
}

+ (NSString *)serializeCurrencySale:(InBrainCurrencySale *)currencySale {
    NSDictionary *dictionary = [self currencySaleDictionary:currencySale];
    return dictionary != nil ? [self serializeDictionary:dictionary] : @"{}";
}

+ (NSString *)serializeRewardsViewDismissedResult:(NSArray<InBrainSurveyReward *> *)rewards byWebView:(BOOL)status {
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];

    NSMutableArray *rewardsArray = [NSMutableArray arrayWithCapacity:rewards.count];

    for (NSUInteger i = 0; i < rewards.count; ++i) {
        NSMutableDictionary *reward = [NSMutableDictionary dictionary];
        reward[@"surveyId"] = rewards[i].surveyId;
        reward[@"placementId"] = rewards[i].placementId;
        reward[@"categories"] = rewards[i].categoryIds;
        reward[@"userReward"] = @(rewards[i].userReward);
        reward[@"outcomeType"] = @(rewards[i].outcomeType);
        [rewardsArray addObject:reward];
    }

    dictionary[@"rewards"] = rewardsArray;
    dictionary[@"byWebView"] = [NSNumber numberWithBool:status];

    return [self serializeDictionary:dictionary];
}

+ (NSArray *)deserializeArray:(NSString *)jsonArray {
    NSError *e = nil;
    NSArray *array = [NSJSONSerialization JSONObjectWithData:[jsonArray dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (array != nil) {
        NSMutableArray *prunedArr = [NSMutableArray array];
        [array enumerateObjectsUsingBlock:^(id obj, NSUInteger idx, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedArr[idx] = obj;
            }
        }];
        return prunedArr;
    }
    return array;
}

+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic {
    NSError *e = nil;
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonDic dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (dictionary != nil) {
        NSMutableDictionary *prunedDict = [NSMutableDictionary dictionary];
        [dictionary enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedDict[key] = obj;
            }
        }];
        return prunedDict;
    }
    return dictionary;
}

+ (NSArray<NSNumber *> *)deserializeNumbersArray:(NSArray *)numbers {
    NSMutableArray<NSNumber *> *result = [NSMutableArray new];

    for (NSNumber *n in numbers) {
        [result addObject:n];
    }

    return result;
}

@end
