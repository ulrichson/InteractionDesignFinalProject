//
//  KCMoviePlayerViewController.h
//
//
//  Created by Kevin Wang on 2/1/13.
//
//

#import <MediaPlayer/MediaPlayer.h>
#import "KCMoviePlayerOverlayView.h"
#import "KCVideoRangeSlider.h"

@interface KCMoviePlayerViewController : UIViewController <KCVideoRangeSliderDelegate, UIGestureRecognizerDelegate>

@property (nonatomic) bool trimEnabled;
@property (nonatomic, assign) int appId;
@property (nonatomic, copy) NSString * videoId;
@property (nonatomic, assign) NSURL * contentUrl;
@property (nonatomic, assign) int feedPosition;
@property (nonatomic, retain) MPMoviePlayerViewController * moviePlayerViewController;
@property (nonatomic, assign) MPMoviePlayerController * moviePlayer;
@property (nonatomic, retain) KCMoviePlayerOverlayView * moviePlayerOverlayView;

- (id)initWithContentURL:(NSURL *)contentUrl;

- (id)initWithContentURL:(NSURL *)contentUrl
                   appId:(int)appId
                 videoId:(NSString *)videoId
            feedPosition:(int)feedPosition;

- (void)   videoRange:(KCVideoRangeSlider *)videoRange
didChangeLeftPosition:(CGFloat)leftPosition
        rightPosition:(CGFloat)rightPosition
         playPosition:(CGFloat)playPosition
            trackType:(KCTrackType)trackType;

@end
