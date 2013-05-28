//
//  KCVideoPlayerSlider.h
//  cocos2d-ios
//
//  Created by Dennis Qin on 4/12/13.
//
//

#import <UIKit/UIKit.h>

@interface KCVideoPlayerSlider : UIView

- (id)initWithFrame:(CGRect)frame duration:(float)duration labelWidth:(int)labelWidth;
- (void)updateCurrentTime:(NSTimeInterval)time;
- (void)updateLabels;

@property (nonatomic, retain) UISlider * playSlider;
@property (nonatomic, retain) UILabel * timeElapsed;
@property (nonatomic, retain) UILabel * timeRemaining;

@end
