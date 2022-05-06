import Swiper, { Navigation, Pagination, Parallax, EffectCoverflow, EffectCube, EffectFade, EffectFlip, Autoplay } from 'swiper';

interface SwiperOptions {
  autoplay: string;
  speed: string;
  effectDefaults: any;
  fallback: any; // see effect fallback in custom app settings
}

function initAppSwiper({ moduleId, options } : { moduleId: string, options: SwiperOptions }) {
  let isSingleSlide = document.querySelectorAll(`.swiper-${moduleId} .swiper-slide:not(.swiper-slide-duplicate)`).length == 1;
  let configured = getSwiperConfiguration(isSingleSlide, options, moduleId);
  let merged = Object.assign(options.fallback, options.effectDefaults, configured);
  
  new Swiper (`.swiper-${moduleId}`, merged);
}

function getSwiperConfiguration(isSingleSlide: boolean, options: SwiperOptions, moduleId: string) {
  if (isSingleSlide) {
    return {
      autoplay: false,
      loop: false,      
      modules: [Navigation, Pagination, Parallax, EffectCoverflow, EffectCube, EffectFade, EffectFlip, Autoplay]
    };
  }

  return {
    autoplay: options.autoplay,
    speed: options.speed,
    modules: [Navigation, Pagination, Parallax, EffectCoverflow, EffectCube, EffectFade, EffectFlip, Autoplay],
    navigation: {
      nextEl: `.swiper-button-next-${moduleId}`,
      prevEl: `.swiper-button-prev-${moduleId}`,
    }
  }
}

// so it can be called from the HTML when content re-initializes dynamically
const winAny = (window as any);
winAny.appSwiper2 ??= {};
winAny.appSwiper2.init ??= initAppSwiper;
