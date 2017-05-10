//
//  GTSPhotoLibraryTools.mm
//  Unity-iPhone
//
//  Created by 默司 on 2017/5/10.
//
//

#include <UIKit/UIKit.h>
#include <Photos/Photos.h>

extern "C" {

    void RequestPermissionC(char *callbackObj, char *callbackFunc) {
        NSString *obj = [NSString stringWithCString:callbackObj encoding:NSUTF8StringEncoding];
        NSString *func = [NSString stringWithCString:callbackFunc encoding:NSUTF8StringEncoding];

        if (PHPhotoLibrary.authorizationStatus == PHAuthorizationStatusAuthorized) {
            return UnitySendMessage([obj UTF8String], [func UTF8String], "");
        }

        if (PHPhotoLibrary.authorizationStatus == PHAuthorizationStatusNotDetermined) {
            [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
                if (status == PHAuthorizationStatusAuthorized) {
                    UnitySendMessage([obj UTF8String], [func UTF8String], "");
                }
            }];
        } else {
            UIAlertController *c = [UIAlertController alertControllerWithTitle:@"相簿權限" message:@"相簿存取已被停用" preferredStyle:UIAlertControllerStyleAlert];

//            UIAlertActionStyleCancel
//            UIAlertActionStyleDefault
            [c addAction:[UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleCancel handler:nil]];
            [c addAction:[UIAlertAction actionWithTitle:@"前往設定" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
                [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
            }]];

            [UIApplication.sharedApplication.keyWindow.rootViewController presentViewController:c animated:true completion:nil];
        }
    }

    void SaveImageToPhotoLibraryC(char* imagePath) {
        NSString* path = [NSString stringWithCString:imagePath encoding:kCFStringEncodingUTF8];

        UIImage* photo = [UIImage imageWithData:[NSData dataWithContentsOfFile:path] scale:1];

        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            UIImageWriteToSavedPhotosAlbum(photo, nil, nil, nil);

			NSFileManager *manager = [NSFileManager defaultManager];
            [manager removeItemAtPath:path error:nil];
        });
    }

}
