import Swiper from 'swiper';
import 'swiper/css/swiper.css';
require('../scss/_styles.scss');

(window as any).appSwiperInit = function appSwiperInit(moduleID: string, autoplay: string, speed: string, effectDefaults: any, fallback: any) {
  var configured = {
    autoplay: (autoplay === 'True'),
    speed: speed
  };
  var merged = Object.assign(fallback, effectDefaults, configured);
  var mySwiper = new Swiper (`.swiper-${moduleID}`, merged);
}