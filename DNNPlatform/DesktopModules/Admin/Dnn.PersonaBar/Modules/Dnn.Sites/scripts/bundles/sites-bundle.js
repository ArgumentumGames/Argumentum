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
/******/ 	return __webpack_require__(__webpack_require__.s = 30);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(31);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(27);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();


var resx = {
  get: function get(key) {
    var moduleName = "Sites";
    return utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].getResx(moduleName, key);
  }
};
var _default = resx;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(resx, "resx", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\localization\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\localization\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 4 */
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
  module.exports = __webpack_require__(36)();
}


/***/ }),
/* 5 */
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
/* 6 */
/***/ (function(module, exports) {

module.exports = window.dnn.Sites.CommonActions;

/***/ }),
/* 7 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 8 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 9 */
/***/ (function(module, exports) {

module.exports = window.dnn.Sites.CommonReducers;

/***/ }),
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();

var applicationSettings = {
  init: function init(applicationSettings) {
    if (!applicationSettings) {
      this.applicationSettings = {};
    }

    this.applicationSettings = applicationSettings;
  },
  applicationSettings: null
};
var _default = applicationSettings;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(applicationSettings, "applicationSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\utils\\applicationSettings.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\utils\\applicationSettings.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.Sites.CommonComponents;

/***/ }),
/* 12 */
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
/* 13 */
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

var	fixUrls = __webpack_require__(34);

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
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var utils_applicationSettings__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(10);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();


var Sites = {
  init: function init(initCallback) {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;
    if (typeof window.dnn[initCallback] === "function") {
      var options = window.dnn[initCallback]();
      utils_applicationSettings__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options);
    } // window.dnn[initCallback] = null;
    // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production


    __webpack_require__(32);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
var _default = Sites;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Sites, "Sites", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 15 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 16 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(8);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(15);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(18);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(19);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(20);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 18 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(8);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(9);
/* harmony import */ var dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();



var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  visiblePanel: Object(dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__["visiblePanel"])(),
  portal: Object(dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__["portal"])(),
  exportPortal: Object(dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__["exportPortal"])(),
  pagination: Object(dnn_sites_common_reducers__WEBPACK_IMPORTED_MODULE_1__["pagination"])()
});
var _default = rootReducer;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 20 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(21);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(22);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(23);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 21 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 22 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 23 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(35);
} else {}

/***/ }),
/* 25 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(7);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _PortalList__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(26);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(6);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(11);
/* harmony import */ var dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _CreatePortal__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(29);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
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
    var _this;

    _classCallCheck(this, App);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(App).call(this));
    _this.state = {
      editMode: false,
      portalBeingExported: {}
    };
    return _this;
  }

  _createClass(App, [{
    key: "onAddNewSite",
    value: function onAddNewSite(event) {
      event.preventDefault();
      this.navigateMap(1);
    }
  }, {
    key: "onEditSite",
    value: function onEditSite() {}
  }, {
    key: "onExportPortal",
    value: function onExportPortal(portalBeingExported) {
      var props = this.props;
      props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__["CommonExportPortalActions"].setPortalBeingExported(portalBeingExported, this.navigateMap.bind(this, 2)));
    }
  }, {
    key: "navigateMap",
    value: function navigateMap(page) {
      var props = this.props;
      props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__["CommonVisiblePanelActions"].selectPanel(page));
    }
  }, {
    key: "cancelExport",
    value: function cancelExport(event) {
      if (event !== undefined) event.preventDefault();
      this.setState({
        portalBeingExported: {}
      });
      this.navigateMap(0);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "sites-Root"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["PersonaBarPage"], {
          isOpen: props.selectedPage === 0 || props.selectedPage === 2
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_PortalList__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"], {
          onAddNewSite: this.onAddNewSite.bind(this),
          onExportPortal: this.onExportPortal.bind(this)
        })), props.selectedPage === 1 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["PersonaBarPage"], {
          isOpen: props.selectedPage === 1
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_CreatePortal__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
          onCancel: this.navigateMap.bind(this, 0)
        })), props.selectedPage === 2 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["PersonaBarPage"], {
          isOpen: props.selectedPage === 2
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_6__["ExportPortal"], {
          onCancel: this.cancelExport.bind(this)
        })))
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
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

App.PropTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  selectedPage: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  selectedPageVisibleIndex: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number
};

function mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex,
    portalBeingExported: state.exportPortal.portalBeingExported
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(App);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 26 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(7);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(11);
/* harmony import */ var dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(3);
/* harmony import */ var utils_applicationSettings__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(10);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(6);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(28);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_8__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
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











var PortalList =
/*#__PURE__*/
function (_Component) {
  _inherits(PortalList, _Component);

  function PortalList() {
    _classCallCheck(this, PortalList);

    return _possibleConstructorReturn(this, _getPrototypeOf(PortalList).apply(this, arguments));
  }

  _createClass(PortalList, [{
    key: "componentWillMount",
    value: function componentWillMount() {
      var props = this.props;
      props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7__["CommonPortalListActions"].loadPortals({
        portalGroupId: props.pagination.portalGroupId,
        filter: props.pagination.filter,
        pageIndex: props.pagination.pageIndex,
        pageSize: props.pagination.pageSize
      }));
    }
  }, {
    key: "loadMore",
    value: function loadMore(event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7__["CommonPaginationActions"].loadMore(function () {
        props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_7__["CommonPortalListActions"].loadPortals({
          portalGroupId: props.pagination.portalGroupId,
          filter: props.pagination.filter,
          pageIndex: props.pagination.pageIndex + 1,
          pageSize: props.pagination.pageSize
        }, true));
      }));
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: _style_less__WEBPACK_IMPORTED_MODULE_8___default.a.sitesPortalList
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Sites")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          onClick: props.onAddNewSite.bind(this),
          size: "large"
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AddNewSite"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageBody"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(dnn_sites_common_components__WEBPACK_IMPORTED_MODULE_4__["ListView"], {
          onAddNewSite: props.onAddNewSite.bind(this),
          onExportPortal: props.onExportPortal.bind(this),
          culture: utils_applicationSettings__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].applicationSettings.cultureCode
        }), props.portals.length < props.totalCount &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "load-more-button"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          onClick: this.loadMore.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("LoadMore.Button")))))
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

  return PortalList;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

PortalList.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  onAddNewSite: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onEditSite: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onExportPortal: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  portals: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array
};

function mapStateToProps(state) {
  return {
    portals: state.portal.portals,
    totalCount: state.portal.totalCount,
    viewMode: state.viewMode,
    pagination: state.pagination
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(PortalList);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(PortalList, "PortalList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\PortalList\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\PortalList\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\PortalList\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 27 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

var utils = _extends({}, window.dnn.utility); //make copy, in case of accidental change.


var _default = utils;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(38);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 29 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(7);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(6);
/* harmony import */ var dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(3);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(39);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_6__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance"); }

function _iterableToArrayLimit(arr, i) { var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }








var emptyNewPortal = {
  SiteTemplate: "",
  SiteName: "",
  SiteAlias: "",
  SiteDescription: "",
  SiteKeywords: "",
  IsChildSite: false,
  HomeDirectory: "Portals/[PortalID]",
  UseCurrentUserAsAdmin: true,
  Firstname: "",
  Lastname: "",
  Username: "",
  Email: "",
  Password: "",
  PasswordConfirm: ""
};

function extractDomain(url) {
  var domain, port; //find & remove protocol (http, ftp, etc.) and get domain

  if (url.indexOf("://") > -1) {
    domain = url.split("/")[2];
  } else {
    domain = url.split("/")[0];
  }

  var _domain$split = domain.split(":");

  var _domain$split2 = _slicedToArray(_domain$split, 2);

  domain = _domain$split2[0];
  port = _domain$split2[1];
  return port !== undefined && port !== 80 && port !== 443 ? "".concat(domain, ":").concat(port) : domain;
}

function validateEmail(email) {
  var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(email);
}

function getUniqueId() {
  return Math.random() * Date.now();
}

var CreatePortal =
/*#__PURE__*/
function (_Component) {
  _inherits(CreatePortal, _Component);

  function CreatePortal() {
    var _this;

    _classCallCheck(this, CreatePortal);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(CreatePortal).call(this));
    _this.state = {
      defaultTemplate: "",
      newPortal: _extends({}, emptyNewPortal),
      error: {
        SiteName: true,
        SiteAlias: true,
        Firstname: false,
        Lastname: false,
        Username: false,
        Email: false,
        Password: false,
        PasswordConfirm: false
      },
      triedToSave: false
    };
    return _this;
  }

  _createClass(CreatePortal, [{
    key: "resetNewPortal",
    value: function resetNewPortal() {
      this.setState({
        newPortal: _extends({}, emptyNewPortal)
      });
    }
  }, {
    key: "componentWillMount",
    value: function componentWillMount() {
      var _this2 = this;

      var props = this.props,
          state = this.state;
      props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__["CommonPortalListActions"].getPortalTemplates(function (data) {
        var newPortal = state.newPortal;
        newPortal.SiteTemplate = data.Results.DefaultTemplate;

        _this2.setState({
          defaultTemplate: data.Results.DefaultTemplate,
          newPortal: newPortal
        });
      }));
    }
  }, {
    key: "resolveSiteUrl",
    value: function resolveSiteUrl(isChildSite) {
      var rootDomain = extractDomain(window.location.href);

      if (isChildSite) {
        var _this$state = this.state,
            newPortal = _this$state.newPortal,
            error = _this$state.error;

        if (newPortal.SiteAlias !== "" && newPortal.SiteAlias.indexOf(rootDomain) === -1) {
          newPortal.SiteAlias = rootDomain + "/" + newPortal.SiteAlias;
        } else {
          newPortal.SiteAlias = rootDomain + "/" + newPortal.SiteName.replace(/[^a-zA-Z0-9-_]/g, "");
        }

        error.SiteAlias = this.resolveSiteAliasError(newPortal.SiteAlias, isChildSite);
        this.setState({
          newPortal: newPortal,
          error: error
        });
      } else {
        var _newPortal = this.state.newPortal;
        _newPortal.SiteAlias = _newPortal.SiteAlias.replace(rootDomain + "/", "");
        this.setState({
          newPortal: _newPortal
        });
      }
    }
  }, {
    key: "resolveSiteAliasError",
    value: function resolveSiteAliasError(value, isChildSite) {
      var rootDomain = extractDomain(window.location.href);
      var subUrl = value.replace(rootDomain + "/", "");

      if (!isChildSite) {
        var regex = /[^\/a-z0-9-.:]/i;
        return value === "" || regex.test(value) || value.indexOf(" ") > 0;
      } else {
        var _regex = /[^\/a-z0-9_-]/i;

        if (_regex.test(subUrl) && subUrl !== "" || value.indexOf(rootDomain + "/") === -1 || subUrl === "" || value.indexOf(" ") > 0) {
          return true;
        }

        return false;
      }
    }
  }, {
    key: "resolvePasswordError",
    value: function resolvePasswordError(value) {
      if (value !== this.state.newPortal.Password || value === "" || value.length < 7) {
        return true;
      }

      return false;
    }
  }, {
    key: "resolveEmailError",
    value: function resolveEmailError(value) {
      return !validateEmail(value);
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = _typeof(event) === "object" ? event.target.value : event;
      var _this$state2 = this.state,
          newPortal = _this$state2.newPortal,
          error = _this$state2.error;

      switch (key) {
        case "IsChildSite":
          //Convert radio button's return of string to boolean.
          newPortal[key] = value === "true";
          this.resolveSiteUrl(value === "true");
          break;

        case "PasswordConfirm":
          newPortal[key] = value;
          error[key] = this.resolvePasswordError(value);
          break;

        case "Email":
          newPortal[key] = value;
          error[key] = this.resolveEmailError(value);
          break;

        case "Password":
        case "Firstname":
        case "Lastname":
        case "Username":
        case "SiteName":
          newPortal[key] = value;
          error[key] = value === "";
          break;

        case "SiteAlias":
          newPortal[key] = value;
          error[key] = this.resolveSiteAliasError(value, newPortal.IsChildSite);
          break;

        case "UseCurrentUserAsAdmin":
          newPortal[key] = value;

          if (value === true) {
            error.Firstname = false;
            error.Lastname = false;
            error.Username = false;
            error.Email = false;
            error.Password = false;
            error.PasswordConfirm = false;
          } else {
            error.Firstname = newPortal.Firstname === "";
            error.Lastname = newPortal.Lastname === "";
            error.Username = newPortal.Username === "";
            error.Email = this.resolveEmailError(newPortal.Email);
            error.Password = newPortal.Password === "";
            error.PasswordConfirm = this.resolvePasswordError(newPortal.PasswordConfirm);
          }

          break;

        default:
          newPortal[key] = value;
          break;
      }

      this.setState({
        newPortal: newPortal,
        error: error
      });
    }
  }, {
    key: "onSelect",
    value: function onSelect(option) {
      var newPortal = this.state.newPortal;
      newPortal.SiteTemplate = option.value;
      this.setState({
        newPortal: newPortal
      });
    }
  }, {
    key: "checkForError",
    value: function checkForError(newPortal, key) {
      if (newPortal[key] === "") {
        return true;
      }
    }
  }, {
    key: "setCreatingPortal",
    value: function setCreatingPortal(creatingPortal) {
      this.setState({
        creatingPortal: creatingPortal
      });
    }
  }, {
    key: "createPortal",
    value: function createPortal() {
      var _this3 = this;

      var props = this.props,
          state = this.state;
      var triedToSave = state.triedToSave;
      triedToSave = true;
      this.setState({
        triedToSave: triedToSave
      }, function () {
        var withError = false;
        Object.keys(state.error).forEach(function (errorKey) {
          if (state.error[errorKey]) {
            withError = true;
          }
        });

        if (withError) {
          return;
        }

        _this3.setCreatingPortal(true);

        props.dispatch(dnn_sites_common_actions__WEBPACK_IMPORTED_MODULE_4__["CommonPortalListActions"].createPortal(state.newPortal, function () {
          _this3.setCreatingPortal(false);

          _this3.resetNewPortal();

          props.onCancel();
        }, function () {
          //active the button after error message disappear
          setTimeout(function () {
            _this3.setCreatingPortal(false);
          }, 5200);
        }));
      });
    }
  }, {
    key: "onCancel",
    value: function onCancel(event) {
      if (event) {
        event.preventDefault();
      }

      this.props.onCancel();
      this.resetNewPortal();
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var templateOptions = props.portalTemplates.map(function (template) {
        return {
          label: template.Name,
          value: template.Value
        };
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "create-portal"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AddNewSite.Header")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("BackToSites"),
            onClick: props.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "create-site-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Title.Label") + "*",
          inputId: "add-new-site-title",
          value: state.newPortal.SiteName,
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SiteTitleRequired.Error"),
          onChange: this.onChange.bind(this, "SiteName"),
          error: state.error.SiteName && state.triedToSave
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["MultiLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Description"),
          inputId: "add-new-site-description",
          value: state.newPortal.SiteDescription,
          className: "portal-description",
          onChange: this.onChange.bind(this, "SiteDescription"),
          error: false
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "site-thumbnails-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["DropdownWithError"], {
          options: templateOptions,
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SiteTemplate.Label"),
          value: state.newPortal.SiteTemplate,
          defaultDropdownValue: state.defaultTemplate,
          onSelect: this.onSelect.bind(this)
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "site-type-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 55
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["RadioButtons"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SiteType.Label"),
          onChange: this.onChange.bind(this, "IsChildSite"),
          buttonGroup: "siteType",
          value: state.newPortal.IsChildSite,
          options: [{
            label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Domain"),
            value: false
          }, {
            label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Directory"),
            value: true
          }]
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 45
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Directory"),
          className: "home-directory",
          inputId: "home-directory",
          value: state.newPortal.HomeDirectory,
          onChange: this.onChange.bind(this, "HomeDirectory"),
          labelType: "inline",
          error: false
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SiteUrl.Label"),
          inputId: "site-url",
          value: state.newPortal.SiteAlias,
          onChange: this.onChange.bind(this, "SiteAlias"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SiteAliasRequired.Error"),
          error: state.error.SiteAlias && state.triedToSave
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "user-as-admin"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Switch"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AssignCurrentUserAsAdmin.Label"),
          onText: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOff"),
          value: state.newPortal.UseCurrentUserAsAdmin,
          onChange: this.onChange.bind(this, "UseCurrentUserAsAdmin")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Collapsible"], {
          style: {
            clear: "both"
          },
          isOpened: !this.state.newPortal.UseCurrentUserAsAdmin
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminUserName.Label"),
          inputId: "admin-user-name",
          value: state.newPortal.Username,
          onChange: this.onChange.bind(this, "Username"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("UsernameRequired.Error"),
          error: state.error.Username && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminFirstName.Label"),
          inputId: "admin-first-name",
          value: state.newPortal.Firstname,
          onChange: this.onChange.bind(this, "Firstname"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("FirstNameRequired.Error"),
          error: state.error.Firstname && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminLastName.Label"),
          inputId: "admin-last-name",
          value: state.newPortal.Lastname,
          onChange: this.onChange.bind(this, "Lastname"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("LastNameRequired.Error"),
          error: state.error.Lastname && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminEmail.Label"),
          inputId: "admin-email",
          value: state.newPortal.Email,
          onChange: this.onChange.bind(this, "Email"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("EmailRequired.Error"),
          error: state.error.Email && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminPassword.Label"),
          inputId: "admin-password",
          value: state.newPortal.Password,
          type: "password",
          onChange: this.onChange.bind(this, "Password"),
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("PasswordRequired.Error"),
          error: state.error.Password && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreateSite_AdminPasswordConfirm.Label"),
          inputId: "admin-password-confirm",
          type: "password",
          value: state.newPortal.PasswordConfirm,
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("PasswordConfirmRequired.Error"),
          onChange: this.onChange.bind(this, "PasswordConfirm"),
          error: state.error.PasswordConfirm && state.triedToSave && !state.newPortal.UseCurrentUserAsAdmin,
          autoComplete: getUniqueId()
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          style: {
            clear: "both"
          }
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: "site-action-buttons"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "secondary",
          onClick: this.onCancel.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("cmdCancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          disabled: this.state.creatingPortal,
          onClick: this.createPortal.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("cmdCreateSite"))))))
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

  return CreatePortal;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

CreatePortal.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func
};

function mapStateToProps(state) {
  return {
    portalTemplates: state.exportPortal.templates
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(CreatePortal);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(emptyNewPortal, "emptyNewPortal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(extractDomain, "extractDomain", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(validateEmail, "validateEmail", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(getUniqueId, "getUniqueId", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(CreatePortal, "CreatePortal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\components\\CreatePortal\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(16);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(7);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(14);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(17);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(24);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
  enterModule && enterModule(module);
})();







var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
var appContainer = document.getElementById("sites-container");
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init("initSites");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_2__["Provider"], {
  store: store
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_containers_Root__WEBPACK_IMPORTED_MODULE_5___default.a, null)), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(33);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, ".sites-app * {\n  box-sizing: border-box;\n}\n", ""]);



/***/ }),
/* 34 */
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
/* 35 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(25);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).enterModule;
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
          className: "sites-app personaBar-mainContainer"
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Sites.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(2)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(37);

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
/* 37 */
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
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "._2wEPNkcZQiSOPhkaZSbca2 .dnn-persona-bar-page-body .persona-bar-page-body {\n  background: transparent;\n  border: none;\n}\n._2wEPNkcZQiSOPhkaZSbca2 .load-more-button {\n  text-align: center;\n}\n", ""]);

// Exports
exports.locals = {
	"sitesPortalList": "_2wEPNkcZQiSOPhkaZSbca2"
};

/***/ }),
/* 39 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(40);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.create-portal {\n  float: left;\n}\n.create-portal .create-site-container {\n  padding: 50px 35px;\n}\n.create-portal .create-site-container .dnn-single-line-input-with-error,\n.create-portal .create-site-container .dnn-multi-line-input-with-error {\n  width: 100%;\n}\n.create-portal .create-site-container .portal-description .dnn-ui-common-multi-line-input {\n  height: 75px;\n  overflow: auto;\n}\n.create-portal .create-site-container .site-template-box {\n  margin-top: 25px;\n}\n.create-portal .create-site-container .site-template-box .dnn-label {\n  display: block;\n  width: 100%;\n}\n.create-portal .create-site-container .site-thumbnails-container {\n  width: 100%;\n}\n.create-portal .create-site-container .site-thumbnails-container .dnn-dropdown-with-error {\n  width: 100%;\n  margin-top: 25px;\n  margin-bottom: 35px;\n}\n.create-portal .create-site-container .site-type-container {\n  margin-top: 25px;\n}\n.create-portal .create-site-container .site-type-container .dnn-radio-buttons {\n  padding-top: 6px;\n}\n.create-portal .create-site-container .site-type-container .dnn-radio-buttons > ul {\n  display: table;\n}\n.create-portal .create-site-container .site-type-container .dnn-radio-buttons ul li label {\n  float: left;\n}\n.create-portal .create-site-container .site-type-container .home-directory .dnn-label {\n  width: 40%;\n  margin-right: 0;\n}\n.create-portal .create-site-container .site-type-container .home-directory .dnn-label > label {\n  font-size: 13px;\n  float: right;\n  margin-right: 25px;\n}\n.create-portal .create-site-container .site-type-container .home-directory .input-tooltip-container {\n  width: 60%;\n}\n.create-portal .create-site-container .site-type-container #home-directory {\n  width: 100%;\n}\n.create-portal .create-site-container .user-as-admin .dnn-switch-container {\n  width: 100%;\n}\n.create-portal .create-site-container .user-as-admin .dnn-switch-container .dnn-switch {\n  float: right;\n}\n.create-portal .create-site-container .site-action-buttons {\n  text-align: center;\n  margin-top: 25px;\n}\n.create-portal .create-site-container .site-action-buttons .dnn-ui-common-button {\n  margin-right: 10px;\n}\n.create-portal .create-site-container .dnn-grid-system.with-right-border {\n  padding: 25px 0;\n}\n.create-portal .create-site-container .dnn-grid-system.with-right-border > .dnn-grid-cell:last-child {\n  padding-left: 15px;\n}\n.create-portal .create-site-container .dnn-grid-system.with-right-border > .dnn-grid-cell:first-child {\n  padding-right: 15px;\n  border-right: 1px solid #C8C8C8;\n}\n", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=sites-bundle.js.map