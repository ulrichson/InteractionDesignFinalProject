#ifndef _KAMCORD_IPHONE_GLESSUPPORT_H_
#define _KAMCORD_IPHONE_GLESSUPPORT_H_

#import "iPhone_GlesSupport.h"

#ifdef __cplusplus
extern "C" {
#endif
    void KamcordInitUnity();
    void ForceOrientationCheck();
    void KamcordDisable();
#ifdef __cplusplus
}
#endif

#define CreateSurfaceGLES CreateSurfaceGLES_Kamcord
#define DestroySurfaceGLES DestroySurfaceGLES_Kamcord
#define PreparePresentSurfaceGLES PreparePresentSurfaceGLES_Kamcord
#define AfterPresentSurfaceGLES AfterPresentSurfaceGLES_Kamcord
#define CreateSurfaceMultisampleBuffersGLES CreateSurfaceMultisampleBuffersGLES_Kamcord
#define DestroySurfaceMultisampleBuffersGLES DestroySurfaceMultisampleBuffersGLES_Kamcord

void CreateSurfaceGLES_Kamcord(struct EAGLSurfaceDesc * surface);
void DestroySurfaceGLES_Kamcord(struct EAGLSurfaceDesc* surface);
void CreateSurfaceMultisampleBuffersGLES_Kamcord(struct EAGLSurfaceDesc* surface);
void DestroySurfaceMultisampleBuffersGLES_Kamcord(struct EAGLSurfaceDesc* surface);
void PreparePresentSurfaceGLES_Kamcord(struct EAGLSurfaceDesc* surface);
void AfterPresentSurfaceGLES_Kamcord(struct EAGLSurfaceDesc* surface);

#endif
