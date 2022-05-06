/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "http://localhost:8080/dist/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 38);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(39);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 1 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = function(originalModule) {
	if (!originalModule.webpackPolyfill) {
		var module = Object.create(originalModule);
		// module.parent = undefined by default
		if (!module.children) module.children = [];
		Object.defineProperty(module, "loaded", {
			enumerable: true,
			get: function() {
				return module.l;
			}
		});
		Object.defineProperty(module, "id", {
			enumerable: true,
			get: function() {
				return module.i;
			}
		});
		Object.defineProperty(module, "exports", {
			enumerable: true
		});
		module.webpackPolyfill = 1;
	}
	return module;
};


/***/ }),
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(17);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(18);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "c", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _licensing__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(19);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _licensing__WEBPACK_IMPORTED_MODULE_2__["a"]; });






/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();


var resx = {
  get: function get(key) {
    var moduleName = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName;
    return _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.getResx(moduleName, key);
  }
};
var _default = resx;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(resx, "resx", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\resources\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\resources\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (false) { var throwOnDirectAccess, ReactIs; } else {
  // By explicitly using `prop-types` you are opting into new production behavior.
  // http://fb.me/prop-types-in-prod
  module.exports = __webpack_require__(44)();
}


/***/ }),
/* 6 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

var utils = {
  init: function init(utilities) {
    if (!utilities) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = utilities;
  },
  utilities: null
};
var _default = utils;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 8 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 9 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(20);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(21);
/* harmony import */ var _licensing__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(22);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _licensing__WEBPACK_IMPORTED_MODULE_2__["a"]; });






/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/*
  MIT License http://www.opensource.org/licenses/mit-license.php
  Author Tobias Koppers @sokra
*/
// css base code, injected by the css-loader
module.exports = function (useSourceMap) {
  var list = []; // return the list of modules as css string

  list.toString = function toString() {
    return this.map(function (item) {
      var content = cssWithMappingToString(item, useSourceMap);

      if (item[2]) {
        return '@media ' + item[2] + '{' + content + '}';
      } else {
        return content;
      }
    }).join('');
  }; // import a list of modules into the list


  list.i = function (modules, mediaQuery) {
    if (typeof modules === 'string') {
      modules = [[null, modules, '']];
    }

    var alreadyImportedModules = {};

    for (var i = 0; i < this.length; i++) {
      var id = this[i][0];

      if (id != null) {
        alreadyImportedModules[id] = true;
      }
    }

    for (i = 0; i < modules.length; i++) {
      var item = modules[i]; // skip already imported module
      // this implementation is not 100% perfect for weird media query combinations
      // when a module is imported multiple times with different media queries.
      // I hope this will never occur (Hey this way we have smaller bundles)

      if (item[0] == null || !alreadyImportedModules[item[0]]) {
        if (mediaQuery && !item[2]) {
          item[2] = mediaQuery;
        } else if (mediaQuery) {
          item[2] = '(' + item[2] + ') and (' + mediaQuery + ')';
        }

        list.push(item);
      }
    }
  };

  return list;
};

function cssWithMappingToString(item, useSourceMap) {
  var content = item[1] || '';
  var cssMapping = item[3];

  if (!cssMapping) {
    return content;
  }

  if (useSourceMap && typeof btoa === 'function') {
    var sourceMapping = toComment(cssMapping);
    var sourceURLs = cssMapping.sources.map(function (source) {
      return '/*# sourceURL=' + cssMapping.sourceRoot + source + ' */';
    });
    return [content].concat(sourceURLs).concat([sourceMapping]).join('\n');
  }

  return [content].join('\n');
} // Adapted from convert-source-map (MIT)


function toComment(sourceMap) {
  // eslint-disable-next-line no-undef
  var base64 = btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap))));
  var data = 'sourceMappingURL=data:application/json;charset=utf-8;base64,' + base64;
  return '/*# ' + data + ' */';
}

/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {

/*
	MIT License http://www.opensource.org/licenses/mit-license.php
	Author Tobias Koppers @sokra
*/

var stylesInDom = {};

var	memoize = function (fn) {
	var memo;

	return function () {
		if (typeof memo === "undefined") memo = fn.apply(this, arguments);
		return memo;
	};
};

var isOldIE = memoize(function () {
	// Test for IE <= 9 as proposed by Browserhacks
	// @see http://browserhacks.com/#hack-e71d8692f65334173fee715c222cb805
	// Tests for existence of standard globals is to allow style-loader
	// to operate correctly into non-standard environments
	// @see https://github.com/webpack-contrib/style-loader/issues/177
	return window && document && document.all && !window.atob;
});

var getTarget = function (target, parent) {
  if (parent){
    return parent.querySelector(target);
  }
  return document.querySelector(target);
};

var getElement = (function (fn) {
	var memo = {};

	return function(target, parent) {
                // If passing function in options, then use it for resolve "head" element.
                // Useful for Shadow Root style i.e
                // {
                //   insertInto: function () { return document.querySelector("#foo").shadowRoot }
                // }
                if (typeof target === 'function') {
                        return target();
                }
                if (typeof memo[target] === "undefined") {
			var styleTarget = getTarget.call(this, target, parent);
			// Special case to return head of iframe instead of iframe itself
			if (window.HTMLIFrameElement && styleTarget instanceof window.HTMLIFrameElement) {
				try {
					// This will throw an exception if access to iframe is blocked
					// due to cross-origin restrictions
					styleTarget = styleTarget.contentDocument.head;
				} catch(e) {
					styleTarget = null;
				}
			}
			memo[target] = styleTarget;
		}
		return memo[target]
	};
})();

var singleton = null;
var	singletonCounter = 0;
var	stylesInsertedAtTop = [];

var	fixUrls = __webpack_require__(42);

module.exports = function(list, options) {
	if (typeof DEBUG !== "undefined" && DEBUG) {
		if (typeof document !== "object") throw new Error("The style-loader cannot be used in a non-browser environment");
	}

	options = options || {};

	options.attrs = typeof options.attrs === "object" ? options.attrs : {};

	// Force single-tag solution on IE6-9, which has a hard limit on the # of <style>
	// tags it will allow on a page
	if (!options.singleton && typeof options.singleton !== "boolean") options.singleton = isOldIE();

	// By default, add <style> tags to the <head> element
        if (!options.insertInto) options.insertInto = "head";

	// By default, add <style> tags to the bottom of the target
	if (!options.insertAt) options.insertAt = "bottom";

	var styles = listToStyles(list, options);

	addStylesToDom(styles, options);

	return function update (newList) {
		var mayRemove = [];

		for (var i = 0; i < styles.length; i++) {
			var item = styles[i];
			var domStyle = stylesInDom[item.id];

			domStyle.refs--;
			mayRemove.push(domStyle);
		}

		if(newList) {
			var newStyles = listToStyles(newList, options);
			addStylesToDom(newStyles, options);
		}

		for (var i = 0; i < mayRemove.length; i++) {
			var domStyle = mayRemove[i];

			if(domStyle.refs === 0) {
				for (var j = 0; j < domStyle.parts.length; j++) domStyle.parts[j]();

				delete stylesInDom[domStyle.id];
			}
		}
	};
};

function addStylesToDom (styles, options) {
	for (var i = 0; i < styles.length; i++) {
		var item = styles[i];
		var domStyle = stylesInDom[item.id];

		if(domStyle) {
			domStyle.refs++;

			for(var j = 0; j < domStyle.parts.length; j++) {
				domStyle.parts[j](item.parts[j]);
			}

			for(; j < item.parts.length; j++) {
				domStyle.parts.push(addStyle(item.parts[j], options));
			}
		} else {
			var parts = [];

			for(var j = 0; j < item.parts.length; j++) {
				parts.push(addStyle(item.parts[j], options));
			}

			stylesInDom[item.id] = {id: item.id, refs: 1, parts: parts};
		}
	}
}

function listToStyles (list, options) {
	var styles = [];
	var newStyles = {};

	for (var i = 0; i < list.length; i++) {
		var item = list[i];
		var id = options.base ? item[0] + options.base : item[0];
		var css = item[1];
		var media = item[2];
		var sourceMap = item[3];
		var part = {css: css, media: media, sourceMap: sourceMap};

		if(!newStyles[id]) styles.push(newStyles[id] = {id: id, parts: [part]});
		else newStyles[id].parts.push(part);
	}

	return styles;
}

function insertStyleElement (options, style) {
	var target = getElement(options.insertInto)

	if (!target) {
		throw new Error("Couldn't find a style target. This probably means that the value for the 'insertInto' parameter is invalid.");
	}

	var lastStyleElementInsertedAtTop = stylesInsertedAtTop[stylesInsertedAtTop.length - 1];

	if (options.insertAt === "top") {
		if (!lastStyleElementInsertedAtTop) {
			target.insertBefore(style, target.firstChild);
		} else if (lastStyleElementInsertedAtTop.nextSibling) {
			target.insertBefore(style, lastStyleElementInsertedAtTop.nextSibling);
		} else {
			target.appendChild(style);
		}
		stylesInsertedAtTop.push(style);
	} else if (options.insertAt === "bottom") {
		target.appendChild(style);
	} else if (typeof options.insertAt === "object" && options.insertAt.before) {
		var nextSibling = getElement(options.insertAt.before, target);
		target.insertBefore(style, nextSibling);
	} else {
		throw new Error("[Style Loader]\n\n Invalid value for parameter 'insertAt' ('options.insertAt') found.\n Must be 'top', 'bottom', or Object.\n (https://github.com/webpack-contrib/style-loader#insertat)\n");
	}
}

function removeStyleElement (style) {
	if (style.parentNode === null) return false;
	style.parentNode.removeChild(style);

	var idx = stylesInsertedAtTop.indexOf(style);
	if(idx >= 0) {
		stylesInsertedAtTop.splice(idx, 1);
	}
}

function createStyleElement (options) {
	var style = document.createElement("style");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}

	if(options.attrs.nonce === undefined) {
		var nonce = getNonce();
		if (nonce) {
			options.attrs.nonce = nonce;
		}
	}

	addAttrs(style, options.attrs);
	insertStyleElement(options, style);

	return style;
}

function createLinkElement (options) {
	var link = document.createElement("link");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}
	options.attrs.rel = "stylesheet";

	addAttrs(link, options.attrs);
	insertStyleElement(options, link);

	return link;
}

function addAttrs (el, attrs) {
	Object.keys(attrs).forEach(function (key) {
		el.setAttribute(key, attrs[key]);
	});
}

function getNonce() {
	if (false) {}

	return __webpack_require__.nc;
}

function addStyle (obj, options) {
	var style, update, remove, result;

	// If a transform function was defined, run it on the css
	if (options.transform && obj.css) {
	    result = typeof options.transform === 'function'
		 ? options.transform(obj.css) 
		 : options.transform.default(obj.css);

	    if (result) {
	    	// If transform returns a value, use that instead of the original css.
	    	// This allows running runtime transformations on the css.
	    	obj.css = result;
	    } else {
	    	// If the transform function returns a falsy value, don't add this css.
	    	// This allows conditional loading of css
	    	return function() {
	    		// noop
	    	};
	    }
	}

	if (options.singleton) {
		var styleIndex = singletonCounter++;

		style = singleton || (singleton = createStyleElement(options));

		update = applyToSingletonTag.bind(null, style, styleIndex, false);
		remove = applyToSingletonTag.bind(null, style, styleIndex, true);

	} else if (
		obj.sourceMap &&
		typeof URL === "function" &&
		typeof URL.createObjectURL === "function" &&
		typeof URL.revokeObjectURL === "function" &&
		typeof Blob === "function" &&
		typeof btoa === "function"
	) {
		style = createLinkElement(options);
		update = updateLink.bind(null, style, options);
		remove = function () {
			removeStyleElement(style);

			if(style.href) URL.revokeObjectURL(style.href);
		};
	} else {
		style = createStyleElement(options);
		update = applyToTag.bind(null, style);
		remove = function () {
			removeStyleElement(style);
		};
	}

	update(obj);

	return function updateStyle (newObj) {
		if (newObj) {
			if (
				newObj.css === obj.css &&
				newObj.media === obj.media &&
				newObj.sourceMap === obj.sourceMap
			) {
				return;
			}

			update(obj = newObj);
		} else {
			remove();
		}
	};
}

var replaceText = (function () {
	var textStore = [];

	return function (index, replacement) {
		textStore[index] = replacement;

		return textStore.filter(Boolean).join('\n');
	};
})();

function applyToSingletonTag (style, index, remove, obj) {
	var css = remove ? "" : obj.css;

	if (style.styleSheet) {
		style.styleSheet.cssText = replaceText(index, css);
	} else {
		var cssNode = document.createTextNode(css);
		var childNodes = style.childNodes;

		if (childNodes[index]) style.removeChild(childNodes[index]);

		if (childNodes.length) {
			style.insertBefore(cssNode, childNodes[index]);
		} else {
			style.appendChild(cssNode);
		}
	}
}

function applyToTag (style, obj) {
	var css = obj.css;
	var media = obj.media;

	if(media) {
		style.setAttribute("media", media)
	}

	if(style.styleSheet) {
		style.styleSheet.cssText = css;
	} else {
		while(style.firstChild) {
			style.removeChild(style.firstChild);
		}

		style.appendChild(document.createTextNode(css));
	}
}

function updateLink (link, options, obj) {
	var css = obj.css;
	var sourceMap = obj.sourceMap;

	/*
		If convertToAbsoluteUrls isn't defined, but sourcemaps are enabled
		and there is no publicPath defined then lets turn convertToAbsoluteUrls
		on by default.  Otherwise default to the convertToAbsoluteUrls option
		directly
	*/
	var autoFixUrls = options.convertToAbsoluteUrls === undefined && sourceMap;

	if (options.convertToAbsoluteUrls || autoFixUrls) {
		css = fixUrls(css);
	}

	if (sourceMap) {
		// http://stackoverflow.com/a/26603875
		css += "\n/*# sourceMappingURL=data:application/json;base64," + btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap)))) + " */";
	}

	var blob = new Blob([css], { type: "text/css" });

	var oldSrc = link.href;

	link.href = URL.createObjectURL(blob);

	if(oldSrc) URL.revokeObjectURL(oldSrc);
}


/***/ }),
/* 13 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(46);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();


var boilerPlate = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;        
    var options = window.dnn.initLicensing();
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options.utility);
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName = options.moduleName; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(40);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
var _default = boilerPlate;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(boilerPlate, "boilerPlate", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 15 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 16 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



var ApplicationService =
/*#__PURE__*/
function () {
  function ApplicationService() {
    _classCallCheck(this, ApplicationService);
  }

  _createClass(ApplicationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getProduct",
    value: function getProduct(callback) {
      var sf = this.getServiceFramework("Licensing");
      sf.get("GetProduct", {}, callback);
    }
  }, {
    key: "getServerInfo",
    value: function getServerInfo(callback) {
      var sf = this.getServiceFramework("ServerSummary");
      sf.get("GetServerInfo", {}, callback);
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return ApplicationService;
}();

var applicationService = new ApplicationService();
var _default = applicationService;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\services\\applicationService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  LOAD_MORE: "LOAD_MORE",
  LOAD_TAB_DATA: "LOAD_TAB_DATA"
};
var _default = paginationActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 18 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
var _default = paginationActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

var licensingActionTypes = {
  RETRIEVED_LICENSING_PRODUCT: "RETRIEVED_LICENSING_PRODUCT",
  RETRIEVED_SERVER_INFO: "RETRIEVED_SERVER_INFO"
};
var _default = licensingActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(licensingActionTypes, "licensingActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\licensing.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\constants\\actionTypes\\licensing.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 20 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();


var paginationActions = {
  loadTab: function loadTab(index) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "b"].LOAD_TAB_DATA,
        payload: {
          index: index
        }
      });
    };
  }
};
var _default = paginationActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActions, "paginationActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 21 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();


var visiblePanelActions = {
  selectPanel: function selectPanel(panel, selectedPageVisibleIndex) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "c"].SELECT_PANEL,
        payload: {
          selectedPage: panel,
          selectedPageVisibleIndex: selectedPageVisibleIndex
        }
      });
    };
  }
};
var _default = visiblePanelActions;
/* unused harmony default export */ var _unused_webpack_default_export = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanelActions, "visiblePanelActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 22 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(16);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();



var licensingActions = {
  getProduct: function getProduct(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getProduct(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* licensing */ "a"].RETRIEVED_LICENSING_PRODUCT,
          data: {
            productName: data.Results.Name
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getServerInfo: function getServerInfo(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getServerInfo(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* licensing */ "a"].RETRIEVED_SERVER_INFO,
          data: {
            productVersion: data.ProductVersion
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  }
};
var _default = licensingActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(licensingActions, "licensingActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\licensing.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\actions\\licensing.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 23 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 24 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(8);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(15);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(25);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(26);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(30);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();






/* eslint-disable no-undef */

var IS_PRODUCTION = "production" === "production";
function configureStore(initialState) {
  var store = Object(redux__WEBPACK_IMPORTED_MODULE_0__["createStore"])(_reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"], initialState, Object(redux__WEBPACK_IMPORTED_MODULE_0__["compose"])(IS_PRODUCTION ? Object(redux__WEBPACK_IMPORTED_MODULE_0__["applyMiddleware"])(redux_thunk__WEBPACK_IMPORTED_MODULE_1___default.a) : Object(redux__WEBPACK_IMPORTED_MODULE_0__["applyMiddleware"])(redux_thunk__WEBPACK_IMPORTED_MODULE_1___default.a, redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default()()), _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].instrument()));
  return store;
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 25 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 26 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(8);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _paginationReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(27);
/* harmony import */ var _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(28);
/* harmony import */ var _licensingReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(29);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();





var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  pagination: _paginationReducer__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"],
  visiblePanel: _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"],
  licensing: _licensingReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"]
});
var _default = rootReducer;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 27 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "b"].LOAD_TAB_DATA:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        tabIndex: action.payload.index
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(pagination, "pagination", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\reducers\\paginationReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 28 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return visiblePanel; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0,
    selectedPageVisibleIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "c"].SELECT_PANEL:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedPage: action.payload.selectedPage,
        selectedPageVisibleIndex: action.payload.selectedPageVisibleIndex
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanel, "visiblePanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\reducers\\visiblePanelReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 29 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return licensingSettings; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function licensingSettings() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* licensing */ "a"].RETRIEVED_LICENSING_PRODUCT:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        productName: action.data.productName
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* licensing */ "a"].RETRIEVED_SERVER_INFO:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        productVersion: action.data.productVersion
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(licensingSettings, "licensingSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\reducers\\licensingReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(31);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(32);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(33);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();






var _default = Object(redux_devtools__WEBPACK_IMPORTED_MODULE_1__["createDevTools"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default.a, {
  toggleVisibilityKey: "ctrl-h",
  changePositionKey: "ctrl-q"
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default.a, null)));

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 31 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 32 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 33 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(43);
} else {}

/***/ }),
/* 35 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(5);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(6);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(9);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _body__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(36);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(4);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var App =
/*#__PURE__*/
function (_Component) {
  _inherits(App, _Component);

  function App() {
    _classCallCheck(this, App);

    return _possibleConstructorReturn(this, _getPrototypeOf(App).call(this));
  }

  _createClass(App, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "licensing-app"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPage"], {
          isOpen: props.selectedPage === 0
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("nav_Licensing")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_body__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], null)))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return App;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

App.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  selectedPage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number
};

function mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(App);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 36 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* unused harmony export Body */
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(5);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(6);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(9);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(10);
/* harmony import */ var _platform__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(37);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(51);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_6__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var Body =
/*#__PURE__*/
function (_Component) {
  _inherits(Body, _Component);

  function Body() {
    var _this;

    _classCallCheck(this, Body);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Body).call(this));
    _this.handleSelect = _this.handleSelect.bind(_assertThisInitialized(_this));
    return _this;
  }

  _createClass(Body, [{
    key: "handleSelect",
    value: function handleSelect(index) {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* pagination */ "b"].loadTab(index)); //index acts as scopeTypeId
    }
  }, {
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* licensing */ "a"].getProduct());
    }
    /*eslint no-mixed-spaces-and-tabs: "error"*/

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageBody"], null, this.props.productName === "DNNCORP.CE" &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_platform__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], null))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return Body;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);
Body.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  productName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.index,
    productName: state.licensing.productName
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(Body);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Body, "Body", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\body\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 37 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(5);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(6);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(10);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(13);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(4);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








/*eslint-disable quotes*/

var dnnTechnologyEditorialIcon = __webpack_require__(47).default;

var githubIcon = __webpack_require__(48).default;

var dnnIcon = __webpack_require__(49).default;

var docsIcon = __webpack_require__(50).default;

var Platform =
/*#__PURE__*/
function (_Component) {
  _inherits(Platform, _Component);

  function Platform() {
    _classCallCheck(this, Platform);

    return _possibleConstructorReturn(this, _getPrototypeOf(Platform).call(this));
  }

  _createClass(Platform, [{
    key: "renderVersion",
    value: function renderVersion() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "intro-header-row"
        }, this.props.productVersion)
      );
    }
  }, {
    key: "onGitHubClick",
    value: function onGitHubClick() {
      window.open("https://github.com/dnnsoftware/Dnn.Platform", "_blank");
    }
  }, {
    key: "onCommunityClick",
    value: function onCommunityClick() {
      window.open("https://dnncommunity.org", "_blank");
    }
  }, {
    key: "onDocsClick",
    value: function onDocsClick() {
      window.open("https://dnndocs.com", "_blank");
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderLinks",
    value: function renderLinks() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "links-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-docs-wrapper",
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Docs.Header"),
          onClick: this.onDocsClick.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-docs"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "docs-icon",
          dangerouslySetInnerHTML: {
            __html: docsIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-docs-header"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Docs.Header")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-docs-desc"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Docs")))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-community-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-community",
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Community.Header"),
          onClick: this.onCommunityClick.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "dnn-icon",
          dangerouslySetInnerHTML: {
            __html: dnnIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-community-header"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Community.Header")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-community-desc"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Community")))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-github-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-github",
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GitHub.Header"),
          onClick: this.onGitHubClick.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "github-icon",
          dangerouslySetInnerHTML: {
            __html: githubIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-github-header"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GitHub.Header")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "link-github-desc"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GitHub")))))
      );
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* licensing */ "a"].getServerInfo());
    }
    /*eslint no-mixed-spaces-and-tabs: "error"*/

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: _style_less__WEBPACK_IMPORTED_MODULE_4___default.a.licensingPlatform
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, this.renderVersion(),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "intro"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "dnn-technology-editorial-icon",
          dangerouslySetInnerHTML: {
            __html: dnnTechnologyEditorialIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "intro-header"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Intro.Header")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "intro-body"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Intro")))), this.renderLinks())
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return Platform;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

Platform.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  productVersion: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    productVersion: state.licensing.productVersion
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(Platform);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(dnnTechnologyEditorialIcon, "dnnTechnologyEditorialIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(githubIcon, "githubIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(dnnIcon, "dnnIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(docsIcon, "docsIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(Platform, "Platform", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\components\\platform\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 38 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(23);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(6);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(14);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(24);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(34);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();







var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init();
var appContainer = document.getElementById("licensing-container");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_2__["Provider"], {
  store: store
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_containers_Root__WEBPACK_IMPORTED_MODULE_5___default.a, null)), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 39 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(1));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(41);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 42 */
/***/ (function(module, exports) {


/**
 * When source maps are enabled, `style-loader` uses a link element with a data-uri to
 * embed the css on the page. This breaks all relative urls because now they are relative to a
 * bundle instead of the current page.
 *
 * One solution is to only use full urls, but that may be impossible.
 *
 * Instead, this function "fixes" the relative urls to be absolute according to the current page location.
 *
 * A rudimentary test suite is located at `test/fixUrls.js` and can be run via the `npm test` command.
 *
 */

module.exports = function (css) {
  // get current location
  var location = typeof window !== "undefined" && window.location;

  if (!location) {
    throw new Error("fixUrls requires window.location");
  }

	// blank or null?
	if (!css || typeof css !== "string") {
	  return css;
  }

  var baseUrl = location.protocol + "//" + location.host;
  var currentDir = baseUrl + location.pathname.replace(/\/[^\/]*$/, "/");

	// convert each url(...)
	/*
	This regular expression is just a way to recursively match brackets within
	a string.

	 /url\s*\(  = Match on the word "url" with any whitespace after it and then a parens
	   (  = Start a capturing group
	     (?:  = Start a non-capturing group
	         [^)(]  = Match anything that isn't a parentheses
	         |  = OR
	         \(  = Match a start parentheses
	             (?:  = Start another non-capturing groups
	                 [^)(]+  = Match anything that isn't a parentheses
	                 |  = OR
	                 \(  = Match a start parentheses
	                     [^)(]*  = Match anything that isn't a parentheses
	                 \)  = Match a end parentheses
	             )  = End Group
              *\) = Match anything and then a close parens
          )  = Close non-capturing group
          *  = Match anything
       )  = Close capturing group
	 \)  = Match a close parens

	 /gi  = Get all matches, not the first.  Be case insensitive.
	 */
	var fixedCss = css.replace(/url\s*\(((?:[^)(]|\((?:[^)(]+|\([^)(]*\))*\))*)\)/gi, function(fullMatch, origUrl) {
		// strip quotes (if they exist)
		var unquotedOrigUrl = origUrl
			.trim()
			.replace(/^"(.*)"$/, function(o, $1){ return $1; })
			.replace(/^'(.*)'$/, function(o, $1){ return $1; });

		// already a full url? no change
		if (/^(#|data:|http:\/\/|https:\/\/|file:\/\/\/|\s*$)/i.test(unquotedOrigUrl)) {
		  return fullMatch;
		}

		// convert the url to a full url
		var newUrl;

		if (unquotedOrigUrl.indexOf("//") === 0) {
		  	//TODO: should we add protocol?
			newUrl = unquotedOrigUrl;
		} else if (unquotedOrigUrl.indexOf("/") === 0) {
			// path should be relative to the base url
			newUrl = baseUrl + unquotedOrigUrl; // already starts with '/'
		} else {
			// path should be relative to current directory
			newUrl = currentDir + unquotedOrigUrl.replace(/^\.\//, ""); // Strip leading './'
		}

		// send back the fixed url(...)
		return "url(" + JSON.stringify(newUrl) + ")";
	});

	// send back the fixed css
	return fixedCss;
};


/***/ }),
/* 43 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(35);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }




var Root =
/*#__PURE__*/
function (_Component) {
  _inherits(Root, _Component);

  function Root() {
    _classCallCheck(this, Root);

    return _possibleConstructorReturn(this, _getPrototypeOf(Root).call(this));
  }

  _createClass(Root, [{
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "licensing-Root"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_App__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"], null))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return Root;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

var _default = Root;
/* harmony default export */ __webpack_exports__["default"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Licensing.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(0)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(2)(module)))

/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(45);

function emptyFunction() {}
function emptyFunctionWithReset() {}
emptyFunctionWithReset.resetWarningCache = emptyFunction;

module.exports = function() {
  function shim(props, propName, componentName, location, propFullName, secret) {
    if (secret === ReactPropTypesSecret) {
      // It is still safe when called from React.
      return;
    }
    var err = new Error(
      'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
      'Use PropTypes.checkPropTypes() to call them. ' +
      'Read more at http://fb.me/use-check-prop-types'
    );
    err.name = 'Invariant Violation';
    throw err;
  };
  shim.isRequired = shim;
  function getShim() {
    return shim;
  };
  // Important!
  // Keep this list in sync with production version in `./factoryWithTypeCheckers.js`.
  var ReactPropTypes = {
    array: shim,
    bool: shim,
    func: shim,
    number: shim,
    object: shim,
    string: shim,
    symbol: shim,

    any: shim,
    arrayOf: getShim,
    element: shim,
    elementType: shim,
    instanceOf: getShim,
    node: shim,
    objectOf: getShim,
    oneOf: getShim,
    oneOfType: getShim,
    shape: getShim,
    exact: getShim,

    checkPropTypes: emptyFunctionWithReset,
    resetWarningCache: emptyFunction
  };

  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2wR2bEK-gcpZDleHSljFSX {\n  border-bottom: none;\n}\n._2wR2bEK-gcpZDleHSljFSX .intro-header-row {\n  padding: 10px;\n  float: right;\n  overflow: hidden;\n  color: #959695;\n  border-left: 1px solid #C8C8C8;\n  border-bottom: 1px solid #C8C8C8;\n}\n._2wR2bEK-gcpZDleHSljFSX .intro {\n  text-align: center;\n  padding: 50px 0;\n  border: 1px solid #C8C8C8;\n}\n._2wR2bEK-gcpZDleHSljFSX .intro .intro-header {\n  margin: 0 0 15px 0;\n  font-size: 25px;\n  color: #4B4E4F;\n}\n._2wR2bEK-gcpZDleHSljFSX .intro .intro-body {\n  font-size: 15px;\n  color: #959695;\n  margin: 0 50px;\n}\n._2wR2bEK-gcpZDleHSljFSX .intro .dnn-technology-editorial-icon {\n  width: 250px;\n  float: right;\n  margin: 0 20px;\n  /*> svg {\n                width: 250px;\n            }*/\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper {\n  float: left;\n  margin-top: 30px;\n  border: 1.5px solid #C8C8C8;\n  font-weight: bolder;\n  display: flex;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper {\n  float: left;\n  width: 265px;\n  margin: 25px 0;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github {\n  height: 100%;\n  cursor: pointer;\n  float: left;\n  margin: 0 20px;\n  text-align: center;\n  border: 1px solid #fafafa;\n  min-height: 290px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github:hover {\n  border: 1px solid #21A3DA;\n  background-color: #FBFCFC;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github .link-github-header {\n  margin: 10px 0 20px 0;\n  color: #4B4E4F;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github .link-github-desc {\n  margin: 0 25px 20px 25px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github .github-icon {\n  margin-top: 10px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-github-wrapper .link-github .github-icon > svg {\n  width: 80px;\n  height: 80px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper {\n  border-right: 1px solid #C8C8C8;\n  float: left;\n  width: 264px;\n  margin: 25px 0;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper .link-community {\n  height: 100%;\n  cursor: pointer;\n  float: left;\n  margin: 0 20px;\n  text-align: center;\n  border: 1px solid #fafafa;\n  min-height: 290px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper .link-community:hover {\n  border: 1px solid #21A3DA;\n  background-color: #FBFCFC;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper .link-community .link-community-header {\n  margin: -10px 0 20px 0;\n  color: #4B4E4F;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper .link-community .link-community-desc {\n  margin: 0 25px 20px 25px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-community-wrapper .link-community .dnn-icon > svg {\n  width: 110px;\n  height: 110px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper {\n  border-right: 1px solid #C8C8C8;\n  float: left;\n  width: 265px;\n  margin: 25px 0;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs {\n  height: 100%;\n  cursor: pointer;\n  float: left;\n  margin: 0 20px;\n  text-align: center;\n  border: 1px solid #fafafa;\n  min-height: 290px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs:hover {\n  border: 1px solid #21A3DA;\n  background-color: #FBFCFC;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs .link-docs-header {\n  margin: 10px 0 20px 0;\n  color: #4B4E4F;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs .link-docs-desc {\n  margin: 0 25px 20px 25px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs .docs-icon {\n  margin-top: 20px;\n}\n._2wR2bEK-gcpZDleHSljFSX .links-wrapper .link-docs-wrapper .link-docs .docs-icon > svg {\n  height: 69px;\n}\n", ""]);

// Exports
exports.locals = {
	"licensingPlatform": "_2wR2bEK-gcpZDleHSljFSX"
};

/***/ }),
/* 47 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 118.9 118.9\"><title>DNN Technology Editorial</title><path d=\"M20.62,59.45A38.83,38.83,0,1,1,59.45,98.28,38.83,38.83,0,0,1,20.62,59.45\" fill=\"#e6e7e8\"/><path d=\"M71.79,51V35.5a25.18,25.18,0,0,0-8.46-1.44H37.94V67.92H54.86V51Z\" fill=\"#ef3e42\"/><path d=\"M71.79,51H54.86V67.92h8.47a8.46,8.46,0,0,0,8.46-8.46Z\" fill=\"#fff\"/><path d=\"M71.79,35.5V51H87.28A25.46,25.46,0,0,0,71.79,35.5M37.94,84.85H54.87V67.92H37.94Z\" fill=\"#472a2b\"/><path d=\"M87.28,51H71.79v8.47a8.46,8.46,0,0,1-8.46,8.46H54.86V84.85h8.47A25.42,25.42,0,0,0,87.28,51\" fill=\"#00a4e4\"/><path d=\"M48.69,106.57l-.85-.19-.33,1.48.93.21c.65.14,1.09,0,1.2-.55s-.22-.79-1-1M72.61,9.93l-1.55,2.2,1.78.47ZM12.41,43.74a2.1,2.1,0,1,0,1.49-3.92,2.1,2.1,0,1,0-1.49,3.92M93.65,25.55c.43-.44.21-.93-.34-1.45l-.71-.69-1.17,1.22.72.68c.41.39,1,.75,1.5.24m-5.77-8.72L85.7,18.4l1.54,1Zm7.62,6.45c.34-.36.22-.79-.32-1.3l-.63-.6-1,1.09.69.66c.48.46,1,.52,1.31.15M16.66,31.81c-.6-.37-1,.09-1.36.63l-.41.66,1.48.91.39-.64c.34-.57.5-1.19-.1-1.56M59.45,0A59.45,59.45,0,1,0,118.9,59.45,59.45,59.45,0,0,0,59.45,0m47.67,35.7-1.31.74-1.58-2.82L103,34.3l1.5,2.66-1.24.69L101.78,35l-1.33.75,1.67,3-1.32.74L98.29,35l6.41-3.6Zm-5.6-8.8-4.6,3.54,1.73,2.25-1.22.94L94.61,30l5.82-4.48ZM96.4,21.2c.82.79,1.53,2,.5,3.09a1.61,1.61,0,0,1-2,.31v0a1.68,1.68,0,0,1-.1,2.27c-1.11,1.16-2.49.58-3.5-.39l-2.11-2,5.08-5.3Zm-8.12-6.47,1.43,1L88.16,23.5l-1.64-1.1.33-1.52L84.49,19.3l-1.25.9-1.61-1.07Zm-8.91-4.65,1.63.73L78.63,16.1l2.6,1.17-.63,1.41-4.23-1.89ZM72.31,7.82,74,8.26,75,16.15l-1.91-.51L73,14.1l-2.75-.72-.89,1.25-1.87-.49Zm-12.21,2a4,4,0,0,1,6.95-2.17L65.74,8.71a1.84,1.84,0,0,0-1.41-.8A2.11,2.11,0,0,0,62,10a2.07,2.07,0,0,0,1.88,2.43,1.89,1.89,0,0,0,1.61-.6l1.13,1.26a3.64,3.64,0,0,1-3,1A3.77,3.77,0,0,1,60.1,9.79m-5.4.92c-1-.23-1.94-.63-2.05-2-.13-1.62,1.29-2.42,2.65-2.53a3.68,3.68,0,0,1,2.47.7l-1,1.28a1.83,1.83,0,0,0-1.36-.52c-.46,0-1,.24-1,.82s.48.61,1.19.77c1,.23,2.34.55,2.46,2,.14,1.69-1.15,2.56-2.6,2.67a3.86,3.86,0,0,1-2.83-.83l1.06-1.27a2.2,2.2,0,0,0,1.65.65c.54,0,1-.3,1-.88s-.62-.69-1.61-.92M24.54,19.3l5.33,1.81,0,0L26.8,17.42l1.33-1.09L32.82,22l-1.55,1.28-5.35-1.81,0,0L29,25.15l-1.32,1.1L23,20.59Zm-4,3.79,1.15,1L19.6,26.52l1,.9,2-2.33,1.08.92-2,2.33,1.16,1,2.21-2.61,1.16,1L23,31.62l-5.6-4.77Zm-6.39,8.47c.84-1.37,2-2.14,3.43-1.26S18.83,32.68,18,34l-.46.75,2.36,1.45L19,37.71l-6.26-3.85ZM9.56,40.42a3.74,3.74,0,0,1,5-2.38,3.81,3.81,0,0,1,2.22,5.14,3.8,3.8,0,0,1-5.07,2.34,3.74,3.74,0,0,1-2.17-5.1m-2.62,33,7-2.2,1.48,4.72L14,76.36l-1-3.08-1.32.41.92,2.91L11.27,77l-.91-2.91-1.46.45,1,3.26-1.44.46ZM12.66,87.2l-1-2,1.6-2.6-3-.22L9.3,80.54l4.62.54,2-3.67,1,1.91-1.42,2.25,2.65.18.93,1.84-4.14-.51ZM16,92l-1-1.44,4.72-3.43-1.22-1.68,1.22-.89,3.49,4.8L22,90.26l-1.22-1.68Zm7.54,6.2-1.09,1.05-3.57-3.67,5.26-5.12L27.62,94l-1.08,1-2.25-2.31-1,1,2.13,2.19-1,1L22.28,94.7l-1.09,1.06Zm5,5.89L29,98.47h0l-2.8,3.9-1.39-1,4.33-5.94,1.64,1.19-.45,5.62,0,0,2.79-3.9,1.39,1-4.33,5.94Zm9,.06c.75.74,1.71,1.69,1,3-.75,1.52-2.31,1.59-3.61.94a3.86,3.86,0,0,1-2-2.18l1.56-.54a2.2,2.2,0,0,0,1.08,1.41c.48.24,1,.27,1.26-.26s-.17-.91-.89-1.62-1.34-1.54-.74-2.73c.73-1.45,2.35-1.4,3.57-.79a3.67,3.67,0,0,1,1.75,1.89l-1.54.56a1.77,1.77,0,0,0-.89-1.15c-.41-.21-1-.32-1.25.2s.1.77.63,1.27m4.33,6.64-1.68-.61,2.49-6.91,1.68.6Zm9.5-3.14a1.6,1.6,0,0,1-1.52,1.28v0a1.7,1.7,0,0,1,1.09,2c-.35,1.56-1.84,1.78-3.2,1.47l-2.86-.64,1.61-7.17,2.86.64c1.11.25,2.35.94,2,2.39M58,113.75l-4.61-.34.54-7.33,1.78.13L55.26,112l2.83.21ZM18.91,59.45A40.55,40.55,0,1,1,59.45,100,40.54,40.54,0,0,1,18.91,59.45m41.56,54.3L60,106.42l4.93-.3.09,1.5-3.22.2.08,1.37L65,109l.09,1.42-3.05.18.09,1.53,3.41-.21.09,1.51ZM91.3,103.61a3.85,3.85,0,0,1-2.85.76l.25-1.64a2.19,2.19,0,0,0,1.74-.3c.44-.32.7-.77.35-1.25s-.88-.26-1.85,0a2.19,2.19,0,0,1-2.76-.6c-1-1.32-.16-2.73.95-3.53a3.67,3.67,0,0,1,2.48-.69L89.4,98A1.79,1.79,0,0,0,88,98.3c-.37.27-.74.73-.4,1.2s.73.26,1.42,0c1-.33,2.29-.74,3.15.45,1,1.37.34,2.78-.84,3.63m4.55-3.85-5.17-5.23,3.52-3.48,1.06,1.08L93,94.39l1,1,2.17-2.15,1,1-2.17,2.15L96,97.48l2.44-2.4,1.06,1.07Zm8-8.75a3.87,3.87,0,1,1-3.72-6.18l.25,1.68a1.85,1.85,0,0,0-1.42.8,2.12,2.12,0,0,0,.56,3,2.07,2.07,0,0,0,3-.35,1.88,1.88,0,0,0,.32-1.7l1.67-.3a3.63,3.63,0,0,1-.71,3m4.51-7.56a2.87,2.87,0,0,1-4,1.5l-4.11-2,.78-1.58,4,2a1.39,1.39,0,1,0,1.21-2.48l-4-1.95.78-1.59,4.11,2a2.9,2.9,0,0,1,1.27,4.15m3.56-10-3.24.72-.17.57,2.8.82-.49,1.67-7.06-2.05.79-2.69c.39-1.36,1.3-2.55,2.91-2.08a2,2,0,0,1,1.48,2l3.56-1Zm-6.21-5.74.61-4.9,1.5.18-.4,3.21,1.37.17.37-3,1.41.17-.37,3,1.51.19.42-3.39,1.51.19L113,68.57ZM107,72.05c-.65-.19-1,.4-1.11.91l-.27.91,1.65.48.23-.81c.16-.55.22-1.28-.5-1.49m-58.8,37.3-1-.22-.36,1.64,1,.22c.55.13,1.25.12,1.4-.58s-.3-.89-1-1.06\" fill=\"#d1d3d4\"/><path d=\"M6.27,53.47l-.56-.57a.4.4,0,0,0-.56,0,.4.4,0,0,0,0,.57l.56.56a.4.4,0,0,0,.56,0,.39.39,0,0,0,0-.56\" fill=\"#bcbec0\"/><path d=\"M4.2,56.89H3.4a.4.4,0,1,0,0,.79h.8a.4.4,0,0,0,0-.79\" fill=\"#bcbec0\"/><path d=\"M15.34,57.68h-.8a.4.4,0,0,0,0,.8h.8a.4.4,0,0,0,0-.8\" fill=\"#bcbec0\"/><path d=\"M14.15,53.47a.39.39,0,0,0-.56,0L13,54a.39.39,0,0,0,0,.56.4.4,0,0,0,.56,0l.56-.56a.39.39,0,0,0,0-.56\" fill=\"#bcbec0\"/><path d=\"M9.77,52.91a.39.39,0,0,0,.28-.12.4.4,0,0,0,.12-.28v-.8a.4.4,0,0,0-.77-.15l0,.16v.79a.4.4,0,0,0,.4.4\" fill=\"#bcbec0\"/><path d=\"M9.37,54.5a3.18,3.18,0,0,0-1.6,5.93v1.23H11V60.43A3.18,3.18,0,0,0,9.37,54.5\" fill=\"#bcbec0\"/><rect x=\"7.77\" y=\"62.35\" width=\"3.18\" height=\"0.8\" fill=\"#bcbec0\"/><path d=\"M7.77,64.54h.82l0,.1c0,.38.36.39.8.39s.79,0,.79-.39l0-.1H11v-.8H7.77Z\" fill=\"#bcbec0\"/><path d=\"M112,47.83v-1a3.35,3.35,0,0,0-6.7,0v1h-.75v6.71h8.2V47.83Zm-2.71,5.24h-1.48l.38-2a.74.74,0,1,1,1.1-.64.74.74,0,0,1-.37.64Zm1.22-5.24h-3.72v-1a1.86,1.86,0,0,1,3.72,0Z\" fill=\"#bcbec0\"/><path d=\"M80.66,108.62a1.39,1.39,0,0,1-2,2,1.33,1.33,0,0,1-.37-.7l-1.58,1.59-1.61-1.61a1.38,1.38,0,0,0,.63-2.31,1.37,1.37,0,0,0-2,0,1.35,1.35,0,0,0-.36.62l-1.6-1.6L73.38,105a1.55,1.55,0,0,1-.55-.33,1.39,1.39,0,1,1,2-2,1.55,1.55,0,0,1,.33.55l1.63-1.63,1.6,1.6A1.39,1.39,0,1,0,80,105l1.6,1.61L80,108.25a1.33,1.33,0,0,1,.7.37\" fill=\"#bcbec0\"/><path d=\"M40.39,13a1.12,1.12,0,1,0,1.12-1.12A1.12,1.12,0,0,0,40.39,13\" fill=\"#bcbec0\"/><path d=\"M42.49,9.62h1.3L41.51,7.34,39.23,9.62h1.29v1.85a1.77,1.77,0,0,1,1-.29,1.74,1.74,0,0,1,1,.29Zm-2,6.83H39.23l2.28,2.28,2.28-2.28h-1.3V14.61a1.81,1.81,0,0,1-1,.28,1.85,1.85,0,0,1-1-.28Z\" fill=\"#bcbec0\"/><path d=\"M43.36,13a1.82,1.82,0,0,0-.28-1h1.84V10.76L47.2,13l-2.28,2.27V14H43.08a1.81,1.81,0,0,0,.28-1\" fill=\"#bcbec0\"/><path d=\"M35.81,13l2.28-2.28v1.29h1.85a1.82,1.82,0,0,0,0,2H38.09v1.29Z\" fill=\"#bcbec0\"/></svg>");

/***/ }),
/* 48 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg role=\"img\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><title>GitHub icon</title><path d=\"M12 .297c-6.63 0-12 5.373-12 12 0 5.303 3.438 9.8 8.205 11.385.6.113.82-.258.82-.577 0-.285-.01-1.04-.015-2.04-3.338.724-4.042-1.61-4.042-1.61C4.422 18.07 3.633 17.7 3.633 17.7c-1.087-.744.084-.729.084-.729 1.205.084 1.838 1.236 1.838 1.236 1.07 1.835 2.809 1.305 3.495.998.108-.776.417-1.305.76-1.605-2.665-.3-5.466-1.332-5.466-5.93 0-1.31.465-2.38 1.235-3.22-.135-.303-.54-1.523.105-3.176 0 0 1.005-.322 3.3 1.23.96-.267 1.98-.399 3-.405 1.02.006 2.04.138 3 .405 2.28-1.552 3.285-1.23 3.285-1.23.645 1.653.24 2.873.12 3.176.765.84 1.23 1.91 1.23 3.22 0 4.61-2.805 5.625-5.475 5.92.42.36.81 1.096.81 2.22 0 1.606-.015 2.896-.015 3.286 0 .315.21.69.825.57C20.565 22.092 24 17.592 24 12.297c0-6.627-5.373-12-12-12\"/></svg>");

/***/ }),
/* 49 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 81 31.95\"><title>DNN Logo</title><path d=\"M45,19.5H42.63V14.74H45a2.38,2.38,0,0,1,0,4.76m0-8.85H38.42V23.59H45a6.47,6.47,0,0,0,0-12.94\" fill=\"#472a2b\"/><path d=\"M66.1,17.06a6.48,6.48,0,0,0-6.48-6.41H53.15V23.59h4.21V14.86h2.26a2.26,2.26,0,0,1,2.27,2.26v6.47H66.1V17.06Z\" fill=\"#472a2b\"/><path d=\"M81,17.06a6.47,6.47,0,0,0-6.47-6.41H68.05V23.59h4.21V14.86h2.27a2.26,2.26,0,0,1,2.26,2.26v6.47H81V17.06Z\" fill=\"#472a2b\"/><path d=\"M21.3,10.65V.91A16,16,0,0,0,16,0H0V21.3H10.65V10.65Z\" fill=\"#ef3e42\"/><path d=\"M21.3.91v9.74H31A16,16,0,0,0,21.3.91M0,32H10.65V21.3H0Z\" fill=\"#472a2b\"/><path d=\"M31,10.65H21.3V16A5.33,5.33,0,0,1,16,21.3H10.65V32H16A16,16,0,0,0,31,10.65\" fill=\"#00a4e4\"/></svg>");

/***/ }),
/* 50 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"131\" height=\"50\" viewBox=\"0 0 131 50\">\r\n  <image y=\"4\" width=\"131\" height=\"43\" xlink:href=\"data:img/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIMAAAArCAYAAABb0jvlAAAGFUlEQVR4nO2ce4gVVRzHP3fdHppliyuZpVlSGWRZRmWQlX9EVhoFlmUPUAsikiIoi7CiouxlL1R6QWGpmWZgmtrDHprYQy1QsTQfhaUZaVtm2+7Gqe/E8ZzZu3fu3Dszi/OB5c78zpx5fuec3/n9zmyhMHVLC8lxOTAHOA34NMHj5pRADfBbgjeqWX9Lgc+BS70tclKjJuEDNwJNWh4AzAYWq6XISZmkxRDQbC2fA3wG3OVtlZMoaYkhjAflT+SkRJbEYLgEWAZ08Epyqk7WxGA4A/jYs+ZUnSyKwTAQmOZZc6pKVsVgGAFc71njUQAOA3oAXZO+oKyTZTEYntPDqxQdgdXAD8CsdC8te2RdDIaXPEv5mJbhENU+KMmLaA+0BzFcCJzsWcujxYq4/pHkRbQHahM+x076jSrCl4H+njV9OoS0MC0RQ/zmXpwKHK317xSqL4UjgH5AF2AHsBL4uUi9AXqxDgYagK+B5UFhHDFsBb6P8GBrVMewRE12qXTQBe+Mcb7VwITRF0kA6JrM8i5gHbAQeBb4vZVjjwfGAD0d+2bgeeABr8Z/nALcDwxx7v+fwJvAIxJGwHnAYxKdyzq9bFNM1nKXlBKVO4GHs/BEItBJAq4DPlIoPA6DgA/bqP+jsrV27KSgnMwgb+u9Mfs+17ENA97yttybFRKqCfufGZIhNq1CZ8fWJ07LsJ9nKQ0zpHsqYstQ0AU+6pWky9/W0b8CZgIHAscrmmruUXcJ7yi98YZXHCHMsEY3l2lYjcRq3trrtN7TEUKD3vjV2r8J6e8BrrDyP09Y288FRqsFqQcGA9cAdwMb4oih3LrGmx/pWdvmSmCi8wCyxFKnWe8FLAD6an2CruFY4Gpru1v0cgTMVD8ePMRrtd9v1DUEbNPbv8Wpa9hk2Y60lhtUD3VlG4AXgsI0RhONTtYyCn1SON9S6eJst9kR/RD9DrZsKx0hBExUS4NT52LLNs4RAhLBJsc201oeIb/t6ZDup10MLW16e5Zs86Xl9AZiqbfOeGGRs3/HWu7q/Bo+8GqEczswzyox3dbNqr8WuBc4gHYohu6eJfsU843KLSNC62omE10EXOUIDPk298jnqE9DDOV2EYSM6bPO6VbEc4d+t1nnfEGR8x9iLf/k7IMSRiIu07RP07reKoc84BhgUhpiKLTxRhSj3HpJ8KtzDOP5T7XWg777PcvWTw6ky23AiZYtqPO2ZZsQkmw7ATjb2xscZy0bn+JJxRzesOzDk45AGg6P8VCzHEI2afc7NLTsqyHi/iprUFwGefAm3zJK6xM1Kpir9aFq0gNM8GmjlsdrdIEyr2tVf4NGLw+p6zdDy9e13Rjtw8QaJmny0E6NMnpYx5mfhhiGepbS2RqjbjWwZ2T1byVkvl7XbLccozXEDN7ika0Mt0184gZr3bzVw61Wpl6xBZfJEkMvCQGJdaCWm5xzNyO8G9PoJsZ6ltLZGKNuNWhS+LnFCkk368EvUd9smug1Icc2ff59reQStsuxC4uQmqb9LAkljNcsUW7TsHSWglEBthDmq0vaFOcjGtPk3ORZizNDodly+EVzG+IEnSodjjbdQDfH1iIxlNqlddRUPztRtUxRwrYwPsJJSieY+/NFSJwhoJuSVL0VMDRO6Sp1Mf9ijNN1k6KIwvT5n3jW4kyJIQR0g7IWffxLE2XisFt5isVl7GNNK61OGKa1eTfE/j+1CpFWizoNn8ZJwXFYUMXz3OdBYphtzTMolYKclDlKnNRZLUvQf3aWV+1mx8plegX2UdC5Gg71SvdxamN877hUYhjmlVSeeU6wplyalb+vU9+cY1GrWTnlzGcIhkrbQ5yoSjO+QvvbLc+5xvrmM0ekEWeIykJ5yZWiMX/44bSHRNUoz5JTFbIuhrEVGLrllEiWxWCc02c8a07VyKoYVuT/1SV5siiGVVZCJSdBsiaGRfrQY49XklN1siQGMwP4/Hz8nx5ZiDMs18yeqImvnAqTphiMk/g48KpXkpMKtWWGorHm30UJRa9XRNFM0HjfK81JFSOGFzVvLypLtP1kawawTZPyHmYSxbf6YKTU3HtO0gD/AL7PJv78QeuAAAAAAElFTkSuQmCC\"/>\r\n</svg>\r\n");

/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(52);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 52 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#licensing-container .dnn-persona-bar-page-body .persona-bar-page-body {\n  border: none;\n  background-color: #FBFCFC;\n}\n", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=licensing-bundle.js.map