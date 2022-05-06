import { initFancybox } from "./lib-2sxc-fancybox";

var winAny = window as any;
winAny.appGalleryV7 ??= {};
winAny.appGalleryV7.init ??= initFancybox;