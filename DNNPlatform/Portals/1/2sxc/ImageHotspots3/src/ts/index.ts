
require('../scss/_style.scss');

import { activateAdmin } from './admin';
import { initFancybox } from './lib-2sxc-fancybox';
import { FancyboxOptions } from './lib-2sxc-fancybox-options';

const winAny = (window as any);
winAny.appHotspot3 ??= {};
winAny.appHotspot3.activateAdmin ??= activateAdmin;
winAny.appHotspot3.initFancybox ??= initFancyboxCustom;

function initFancyboxCustom({ attribute, options } : { attribute: string, options: FancyboxOptions }) {
    let customOptions = {
        ...options,
        on: {  
            done: (fancybox: any, slide: any) => {
              const imgWidth = document.querySelector(`${slide.src} img`).getBoundingClientRect().width
              let fancyboxText: HTMLElement = document.querySelector(`${slide.src} .hotspot3-fancybox-text`)
              let hotspotContent: HTMLElement = document.querySelector(`${slide.src}.hotspot3-fancybox-content`)
              if(!fancyboxText.hasAttribute('style')) fancyboxText.style.maxWidth = `${Math.floor(imgWidth)}px`
              hotspotContent.style.opacity = "1"
            }
          }
    }
    initFancybox({ attribute, options: customOptions })
}