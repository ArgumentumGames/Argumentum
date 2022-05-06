import { showEncryptedMails } from './mail-obfuscator';
import { activateYouTubeInline } from './youtube-preview';
import { activeGoogleMaps } from './lib-2sxc-google-maps';
import { initFancybox } from './lib-2sxc-fancybox';

// so it can be called from the HTML when content re-initializes dynamically
const winAny = (window as any);
winAny.appContent = winAny.appContent || {};
winAny.appContent.activeGoogleMaps = winAny.appContent.activeGoogleMaps || activeGoogleMaps;
winAny.appContent.activateYouTubeInline = winAny.appContent.activateYouTubeInline || activateYouTubeInline;
winAny.appContent.showEncryptedMails = winAny.appContent.showEncryptedMails || showEncryptedMails;
winAny.appContent.initFancybox = initFancybox;
