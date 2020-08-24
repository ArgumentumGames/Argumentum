export class PolyFills {
    constructor () {
        this.findPolyfill();
    }

    /*
        This checks if Array.find() is supportet, if not, it uses a polyfill.
        This is only used for IE11, if 2sic dropps support for IE11 you can safely remove this code.
    */
    findPolyfill = () => {
        if (!Array.prototype.find) {
            Array.prototype.find = function(predicate: any) {
              'use strict';
              if (this == null) {
                throw new TypeError('Array.prototype.find called on null or undefined');
              }
              if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
              }
              var list = Object(this);
              var length = list.length >>> 0;
              var thisArg = arguments[1];
              var value;
          
              for (var i = 0; i < length; i++) {
                value = list[i];
                if (predicate.call(thisArg, value, i, list)) {
                  return value;
                }
              }
              return undefined;
            };
        }
    }
}