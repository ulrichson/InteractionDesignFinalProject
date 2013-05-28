//
//  KCNotificationScheduler.h
//
//
//  Created by Haitao Mao on 4/1/13.
//
//

#import "Kamcord.h"

@interface KCNotificationScheduler : NSObject

typedef enum
{
    KC_STANDARD_NOTIFICATION = 0,
    KC_UNITY_NOTIFICATION,
    KC_TEST_NOTIFICATION
} KC_NOTIFICATION_TYPE;

+ (KCNotificationScheduler *) sharedScheduler;
- (void)updateKamcordNotifications:(NSNotification *)notif;
- (void)handleKamcordNotification:(UILocalNotification *)notification
                 notificationType:(KC_NOTIFICATION_TYPE)notificationType;

// Hacky version for Unity because we can't actually pass the notification.
- (void)handleKamcordNotification;

@end
