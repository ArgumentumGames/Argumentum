let BeerSlider = require('../../node_modules/beerslider');
require('../scss/_style.scss');

const winAny = (window as any);
winAny.appImageCompare2 ??= {};
winAny.appImageCompare2.init ??= init;

export function init({ domId } : { domId: string }) {
    new BeerSlider(document.getElementById(domId));
}
