//
//  KCMoviePlayerOverlayView.h
//  cocos2d-ios
//
//  Created by Dennis Qin on 3/25/13.
//
//

#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>
#import "KCVideoRangeSlider.h"
#import "KCVideoPlayerSlider.h"
#import "KCGradientButton.h"
#import <Kamcord.h>

@interface KCMoviePlayerOverlayView : UIView

- (id)initWithFrame:(CGRect)frame videoUrl:(NSURL *)videoUrl parentViewController:(UIViewController<KCVideoRangeSliderDelegate> *)parentViewController trimEnabled:(bool)trimEnabled;
- (void)setupTrimInterface;
- (void)updateViewForPlayBackState:(MPMoviePlaybackState)playBackState;
- (void)updateCurrentTime:(NSTimeInterval)time;
- (void)updateSliderLabels;
- (void)hidePlayMarker;

@property (nonatomic) bool trimActive;

@end
