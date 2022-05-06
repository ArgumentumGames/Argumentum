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
/******/ 	return __webpack_require__(__webpack_require__.s = 42);
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

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (false) { var throwOnDirectAccess, ReactIs; } else {
  // By explicitly using `prop-types` you are opting into new production behavior.
  // http://fb.me/prop-types-in-prod
  module.exports = __webpack_require__(26)();
}


/***/ }),
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// CONCATENATED MODULE: ./src/constants/actionTypes/pagination.js
var paginationActionTypes = {
  LOAD_MORE: "LOAD_MORE",
  LOAD_TAB_DATA: "LOAD_TAB_DATA"
};
/* harmony default export */ var pagination = (paginationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/visiblePanel.js
var visiblePanel_paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
/* harmony default export */ var visiblePanel = (visiblePanel_paginationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/seo.js
var seoActionTypes = {
  RETRIEVED_SEO_GENERAL_SETTINGS: "RETRIEVED_SEO_GENERAL_SETTINGS",
  UPDATED_SEO_GENERAL_SETTINGS: "UPDATED_SEO_GENERAL_SETTINGS",
  SEO_GENERAL_SETTINS_CLIENT_MODIFIED: "SEO_GENERAL_SETTINS_CLIENT_MODIFIED",
  RETRIEVED_SEO_REGEX_SETTINGS: "RETRIEVED_SEO_REGEX_SETTINGS",
  UPDATED_SEO_REGEX_SETTINGS: "UPDATED_SEO_REGEX_SETTINGS",
  SEO_REGEX_SETTINS_CLIENT_MODIFIED: "SEO_REGEX_SETTINS_CLIENT_MODIFIED",
  TESTED_SEO_PAGE_URL: "TESTED_SEO_PAGE_URL",
  TESTED_SEO_URL_REWRITING: "TESTED_SEO_URL_REWRITING",
  CLEARED_SEO_TEST_PAGE_URL_RESULTS: "CLEARED_SEO_TEST_PAGE_URL_RESULTS",
  CLEARED_SEO_TEST_URL_REWRITING_RESULTS: "CLEARED_SEO_TEST_URL_REWRITING_RESULTS",
  RETRIEVED_SEO_SITEMAP_SETTINGS: "RETRIEVED_SEO_SITEMAP_SETTINGS",
  UPDATED_SEO_SITEMAP_SETTINGS: "UPDATED_SEO_SITEMAP_SETTINGS",
  SEO_SITEMAP_SETTINS_CLIENT_MODIFIED: "SEO_SITEMAP_SETTINS_CLIENT_MODIFIED",
  CLEARED_SEO_SITEMAP_CACHE: "CLEARED_SEO_SITEMAP_CACHE",
  RETRIEVED_SEO_SITEMAP_PROVIDERS: "RETRIEVED_SEO_SITEMAP_PROVIDERS",
  UPDATED_SEO_SITEMAP_PROVIDER: "UPDATED_SEO_SITEMAP_PROVIDER",
  CREATED_SEO_SITEMAP_VERIFICATION: "CREATED_SEO_SITEMAP_VERIFICATION",
  RETRIEVED_SEO_EXTENSION_URL_PROVIDERS: "RETRIEVED_SEO_EXTENSION_URL_PROVIDERS",
  UPDATED_SEO_EXTENSION_URL_PROVIDER: "UPDATED_SEO_EXTENSION_URL_PROVIDER"
};
/* harmony default export */ var seo = (seoActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/index.js
/* concated harmony reexport pagination */__webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* concated harmony reexport visiblePanel */__webpack_require__.d(__webpack_exports__, "c", function() { return visiblePanel; });
/* concated harmony reexport seo */__webpack_require__.d(__webpack_exports__, "b", function() { return seo; });





/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
var utils = {
  init: function init(utilities) {
    if (!utilities) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = utilities;
  },
  utilities: null
};
/* harmony default export */ __webpack_exports__["a"] = (utils);

/***/ }),
/* 5 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 6 */
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
/* 7 */
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

var	fixUrls = __webpack_require__(25);

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
/* 8 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(28);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 10 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(29);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 11 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(32);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(33);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 13 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(36);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 14 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(37);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(38);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 16 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 17 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 18 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 19 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 20 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 21 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(41);
} else {}

/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(24);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 25 */
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
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(27);

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
/* 27 */
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
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2S6c92w5a0_XA9eIu0odQw {\n  margin: 30px 30px;\n}\n._2S6c92w5a0_XA9eIu0odQw * {\n  box-sizing: border-box;\n}\n._2S6c92w5a0_XA9eIu0odQw .columnTitle {\n  font-weight: bolder;\n  padding: 0 0 25px 0;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .dnn-label.block label {\n  max-width: 90%;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .generalSettings-row_switch {\n  border-bottom: 1px solid #C8C8C8;\n  margin: 20px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 20px 0;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .generalSettings-row_switch .dnn-switch-container {\n  float: right;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .generalSettings-row-options .dnn-label {\n  padding-top: 0;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .generalSettings-row-options .dnn-radio-buttons {\n  margin-left: -15px;\n  padding: 7px 0 0 0;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .generalSettings-row-options .dnn-radio-buttons label {\n  margin-bottom: 8px;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .sectionLabel {\n  border-top: 1px solid #C8C8C8;\n  padding-top: 20px;\n  margin-top: 20px;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .sectionLabel label {\n  color: #000;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-ui-common-input-group .dnn-dropdown {\n  width: 100%;\n}\n._2S6c92w5a0_XA9eIu0odQw .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 30px 0;\n  float: left;\n}\n._2S6c92w5a0_XA9eIu0odQw .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-grid-system .left-column {\n  padding-right: 30px;\n  border-right: 1px solid #C8C8C8;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-grid-system .left-column .dnn-dropdown {\n  width: 100%;\n}\n._2S6c92w5a0_XA9eIu0odQw .dnn-grid-system .right-column {\n  padding-left: 30px;\n}\n", ""]);

// Exports
exports.locals = {
	"generalSettings": "_2S6c92w5a0_XA9eIu0odQw"
};

/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3jlHbc3md3Yk2L2iMDcyLa {\n  margin: 30px 30px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .columnTitle {\n  font-weight: bolder;\n  padding: 0 0 10px 0;\n  border-bottom: 1px solid #C8C8C8;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper {\n  border: 1px solid #C8C8C8;\n  float: left;\n  width: 100%;\n  margin-top: 20px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group {\n  padding: 15px 15px;\n  width: 708px;\n  background-color: #EFF0F0;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .generalSettings-row_switch {\n  margin: 10px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 10px 0;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .generalSettings-row_switch .dnn-switch-container {\n  float: right;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .generalSettings-row-options .dnn-label {\n  padding-top: 0;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .generalSettings-row-options .dnn-radio-buttons {\n  margin-left: -15px;\n  padding: 7px 0 0 0;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .generalSettings-row-options .dnn-radio-buttons label {\n  margin-bottom: 8px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .sectionLabel {\n  border-top: 1px solid #C8C8C8;\n  padding-top: 20px;\n  margin-top: 20px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .sectionLabel label {\n  color: #000;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .dnn-dropdown {\n  width: 681px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .dnn-file-upload {\n  width: 313px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .groupWrapper .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 30px 0;\n  float: left;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .dnn-grid-system .left-column {\n  padding-right: 30px;\n  border-right: 1px solid #C8C8C8;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .dnn-grid-system .left-column .dnn-dropdown {\n  width: 281px;\n}\n._3jlHbc3md3Yk2L2iMDcyLa .dnn-grid-system .right-column {\n  padding-left: 30px;\n}\n", ""]);

// Exports
exports.locals = {
	"regexSettings": "_3jlHbc3md3Yk2L2iMDcyLa"
};

/***/ }),
/* 30 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(31);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-component-providers {\n  display: block;\n  float: left;\n  width: 100%;\n  cursor: auto;\n}\n.collapsible-component-providers:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n.collapsible-component-providers div.collapsible-providers {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 10px 0;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-name {\n  width: 40%;\n  padding-left: 15px;\n  float: left;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-enabled {\n  width: 20%;\n  float: left;\n  text-align: center;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-enabled .item-row-enabled-display {\n  color: #959695;\n  margin: 0 0 0 -15px;\n  display: table;\n  margin: 0 auto;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-enabled .item-row-enabled-display .enabled-icon > svg {\n  padding: 0 15px 0 15px;\n  width: 46px;\n  float: left;\n  height: 16px;\n  fill: #959695;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-priority {\n  width: 30%;\n  float: left;\n  text-align: center;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-editButton {\n  width: 6%;\n  float: left;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-editButton .edit-icon {\n  cursor: pointer;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-editButton .edit-icon > svg {\n  width: 16px;\n  float: right;\n  height: 16px;\n  fill: #C8C8C8;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-editButton .edit-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-editButton .edit-icon-active > svg {\n  width: 16px;\n  float: right;\n  height: 16px;\n  fill: #1E88C3;\n}\n.collapsible-component-providers div.collapsible-providers .row .item-row-wrapper {\n  padding: 0 5px 0 5px;\n}\n", ""]);



/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2oRyyGMIc8mZAHJKJRoEuB {\n  padding: 0 20px;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group {\n  width: 100%;\n  padding: 0 0 10px 0;\n  margin: 0 0 10px 0;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group:first-child {\n  margin: 0 0 20px 0;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_switch .dnn-switch-container {\n  float: right;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_dropdown {\n  float: left;\n  width: 100%;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_dropdown .dnn-dropdown {\n  width: 120px;\n  float: right;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_dropdown .dnn-dropdown .collapsible-label {\n  width: auto;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_dropdown .dnn-label {\n  width: 180px;\n  margin-top: 8px;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .providerSettings-row_dropdown .dnn-label label {\n  float: left;\n  font-weight: bolder;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 99.5%;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .dnn-radio-buttons {\n  float: left;\n  width: 300px;\n  margin: 9px 0 0 30px;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .dnn-radio-buttons label {\n  font-size: 14px;\n  float: left;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .dnn-ui-common-input-group .dnn-radio-buttons li {\n  margin: 0 20px 0 0;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .buttons-box-secondary {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 0 0 20px 0;\n}\n._2oRyyGMIc8mZAHJKJRoEuB .buttons-box-secondary .dnn-ui-common-button {\n  margin: 5px;\n}\n", ""]);

// Exports
exports.locals = {
	"providerSettingEditor": "_2oRyyGMIc8mZAHJKJRoEuB"
};

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2tZyRPklojzNSWF011aJSz {\n  margin: 45px 30px;\n}\n._2tZyRPklojzNSWF011aJSz * {\n  box-sizing: border-box;\n}\n._2tZyRPklojzNSWF011aJSz .columnTitle {\n  font-weight: bolder;\n  padding: 0 0 25px 0;\n}\n._2tZyRPklojzNSWF011aJSz .columnTitle2 {\n  float: left;\n  width: 100%;\n  font-weight: bolder;\n  border-bottom: 1px solid #C8C8C8;\n  padding: 45px 0 15px 0;\n}\n._2tZyRPklojzNSWF011aJSz .columnTitle3 {\n  float: left;\n  width: 100%;\n  font-weight: bolder;\n  padding: 45px 0 20px 0;\n}\n._2tZyRPklojzNSWF011aJSz .provider-items-grid {\n  border: solid 1px #C8C8C8;\n  margin: 15px 0;\n  float: left;\n  width: 735px;\n}\n._2tZyRPklojzNSWF011aJSz .provider-items-grid .header-row {\n  border-bottom: 1px solid #C8C8C8;\n  padding: 10px 0 10px 0;\n  width: 100%;\n  float: left;\n  overflow: hidden;\n  text-transform: uppercase;\n}\n._2tZyRPklojzNSWF011aJSz .provider-items-grid .header-row .header-Name {\n  width: 40%;\n  float: left;\n  font-weight: bolder;\n  padding-left: 15px;\n}\n._2tZyRPklojzNSWF011aJSz .provider-items-grid .header-row .header-Enabled {\n  width: 20%;\n  float: left;\n  font-weight: bolder;\n  text-align: center;\n}\n._2tZyRPklojzNSWF011aJSz .provider-items-grid .header-row .header-Priority {\n  width: 30%;\n  float: left;\n  font-weight: bolder;\n  text-align: center;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group {\n  padding: 0 0 15px 0;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .dnn-label.inline {\n  max-width: 70%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .dnn-label.inline label {\n  max-width: 85%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapUrl {\n  float: left;\n  width: 100%;\n  background: #E5E7E6;\n  border: #E5E7E6;\n  padding: 8px;\n  box-sizing: border-box;\n  height: 34px;\n  margin-bottom: 3px;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapSettings-row_switch {\n  margin: 10px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 10px 0;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapSettings-row_switch .dnn-switch-container {\n  float: right;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapSettings-row-options .dnn-label {\n  padding-top: 0;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapSettings-row-options .dnn-radio-buttons {\n  margin-left: -15px;\n  padding: 7px 0 0 0;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sitemapSettings-row-options .dnn-radio-buttons label {\n  margin-bottom: 8px;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sectionLabel {\n  border-top: 1px solid #C8C8C8;\n  padding-top: 20px;\n  margin-top: 20px;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .sectionLabel label {\n  color: #000;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .dnn-dropdown {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .daysToCache {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .daysToCache .dnn-dropdown {\n  width: 62%;\n  box-sizing: border-box;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .daysToCache .dnn-dropdown .collapsible-label {\n  width: auto;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .daysToCache .clearCacheBtn {\n  margin-left: 10px;\n  float: right;\n  width: 35%;\n  overflow: hidden;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .daysToCache .dnn-ui-common-button {\n  padding: 0;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .searchEngineSubmit {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .searchEngineSubmit .dnn-dropdown {\n  width: 62% !important;\n  box-sizing: border-box;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .searchEngineSubmit .dnn-dropdown .collapsible-label {\n  width: auto;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .searchEngineSubmit .searchEngineSubmitBtn {\n  margin-left: 10px;\n  float: right;\n  width: 35%;\n  overflow: hidden;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .createVerification {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .createVerification .dnn-single-line-input-with-error {\n  width: 62% !important;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .createVerification .createVerificationBtn {\n  margin-left: 10px;\n  float: right;\n  width: 35%;\n  overflow: hidden;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group input {\n  width: auto;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 30px 0;\n  float: left;\n}\n._2tZyRPklojzNSWF011aJSz .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-grid-system .left-column {\n  padding-right: 30px;\n  border-right: 1px solid #C8C8C8;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-grid-system .left-column .dnn-dropdown {\n  width: 100%;\n}\n._2tZyRPklojzNSWF011aJSz .dnn-grid-system .right-column {\n  padding-left: 30px;\n}\n", ""]);

// Exports
exports.locals = {
	"sitemapSettings": "_2tZyRPklojzNSWF011aJSz"
};

/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(35);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-component-extention-url-providers {\n  display: block;\n  float: left;\n  width: 100%;\n  cursor: auto;\n}\n.collapsible-component-extention-url-providers:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 10px 0;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-name {\n  width: 60%;\n  padding-left: 15px;\n  float: left;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-enabled {\n  width: 30%;\n  float: left;\n  text-align: center;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-enabled .item-row-enabled-display {\n  color: #959695;\n  margin: 0 0 0 -15px;\n  display: table;\n  margin: 0 auto;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-enabled .item-row-enabled-display .enabled-icon > svg {\n  padding: 0 15px 0 15px;\n  width: 16px;\n  float: left;\n  height: 16px;\n  fill: #959695;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-enabled .checkbox {\n  margin-right: 0;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton {\n  width: 6%;\n  float: left;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton .edit-icon {\n  cursor: pointer;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton .edit-icon > svg {\n  width: 16px;\n  float: right;\n  height: 16px;\n  fill: #C8C8C8;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton .edit-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton .edit-icon-active {\n  cursor: pointer;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-editButton .edit-icon-active > svg {\n  width: 16px;\n  float: right;\n  height: 16px;\n  fill: #1E88C3;\n}\n.collapsible-component-extention-url-providers div.collapsible-providers .row .item-row-wrapper {\n  padding: 0 5px 0 5px;\n}\n", ""]);



/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3O1XFCuoZS5F8iEuplLmiW {\n  padding: 0 20px;\n}\n._3O1XFCuoZS5F8iEuplLmiW .edit-provider {\n  float: left;\n  padding: 0px;\n  width: 100%;\n  border: 0px;\n  min-height: 80px;\n  padding: 10px 25px;\n}\n", ""]);

// Exports
exports.locals = {
	"providerSettingEditor": "_3O1XFCuoZS5F8iEuplLmiW"
};

/***/ }),
/* 37 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._24Trf0riLTlrRpk3sDr8uY {\n  margin: 30px 30px;\n  float: left;\n}\n._24Trf0riLTlrRpk3sDr8uY * {\n  box-sizing: border-box;\n}\n._24Trf0riLTlrRpk3sDr8uY .columnTitle {\n  font-weight: bolder;\n  padding: 0 0 10px 0;\n  margin: 0 0 5px 0;\n  border-bottom: 1px solid #C8C8C8;\n}\n._24Trf0riLTlrRpk3sDr8uY .provider-items-grid {\n  border: solid 1px #C8C8C8;\n  margin: 15px 0;\n  float: left;\n  width: 735px;\n}\n._24Trf0riLTlrRpk3sDr8uY .provider-items-grid .header-row {\n  border-bottom: 1px solid #C8C8C8;\n  padding: 10px 0 10px 0;\n  width: 100%;\n  float: left;\n  overflow: hidden;\n  text-transform: uppercase;\n}\n._24Trf0riLTlrRpk3sDr8uY .provider-items-grid .header-row .header-Name {\n  width: 60%;\n  float: left;\n  font-weight: bolder;\n  padding-left: 15px;\n}\n._24Trf0riLTlrRpk3sDr8uY .provider-items-grid .header-row .header-Enabled {\n  width: 30%;\n  float: left;\n  font-weight: bolder;\n  text-align: center;\n}\n._24Trf0riLTlrRpk3sDr8uY .provider-items-grid .no-extension-url-providers {\n  float: left;\n  width: 100%;\n  text-align: center;\n  padding: 10px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionUrlProviders": "_24Trf0riLTlrRpk3sDr8uY"
};

/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.oibGKdDrTj_jlWCHvbe6L {\n  margin: 30px 30px;\n}\n.oibGKdDrTj_jlWCHvbe6L .columnTitleOne {\n  font-weight: bolder;\n  padding-bottom: 10px;\n  margin-bottom: 20px;\n  border-bottom: 1px solid #C8C8C8;\n}\n.oibGKdDrTj_jlWCHvbe6L .columnTitleTwo {\n  width: 100%;\n  float: left;\n  font-weight: bolder;\n  padding: 10px 0;\n  margin: 20px 0;\n  border-bottom: 1px solid #C8C8C8;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group {\n  padding: 0 0 15px 0;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .dnn-label.block label {\n  max-width: 90%;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .dnn-ui-common-multi-line-input {\n  height: 230px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .sectionLabel {\n  border-top: 1px solid #C8C8C8;\n  padding-top: 20px;\n  margin-top: 20px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .sectionLabel label {\n  color: #000;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .dnn-dropdown {\n  width: 681px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group .dnn-file-upload {\n  width: 313px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-ui-common-input-group input {\n  width: auto;\n}\n.oibGKdDrTj_jlWCHvbe6L .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n}\n.oibGKdDrTj_jlWCHvbe6L .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-grid-system {\n  border: 1px solid #C8C8C8;\n  background-color: #FBFCFC;\n  padding: 20px 20px 0 20px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-grid-system:last-child {\n  margin-bottom: 30px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-grid-system .left-column {\n  padding-right: 20px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-grid-system .left-column .dnn-dropdown {\n  width: 281px;\n}\n.oibGKdDrTj_jlWCHvbe6L .dnn-grid-system .right-column {\n  padding-left: 20px;\n}\n", ""]);

// Exports
exports.locals = {
	"testUrl": "oibGKdDrTj_jlWCHvbe6L"
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

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#seo-container .dnn-persona-bar-page-body .persona-bar-page-body .row-opened {\n  margin-top: -1px;\n  margin-bottom: -1px;\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3 !important;\n}\n", ""]);



/***/ }),
/* 41 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: D:/a/1/s/node_modules/prop-types/index.js
var prop_types = __webpack_require__(2);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(5);

// EXTERNAL MODULE: external "window.dnn.nodeModules.CommonComponents"
var external_window_dnn_nodeModules_CommonComponents_ = __webpack_require__(1);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 3 modules
var actionTypes = __webpack_require__(3);

// CONCATENATED MODULE: ./src/actions/pagination.js

var paginationActions = {
  loadTab: function loadTab(index) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["a" /* pagination */].LOAD_TAB_DATA,
        payload: {
          index: index
        }
      });
    };
  }
};
/* harmony default export */ var pagination = (paginationActions);
// CONCATENATED MODULE: ./src/actions/visiblePanel.js

var visiblePanelActions = {
  selectPanel: function selectPanel(panel, selectedPageVisibleIndex) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["c" /* visiblePanel */].SELECT_PANEL,
        payload: {
          selectedPage: panel,
          selectedPageVisibleIndex: selectedPageVisibleIndex
        }
      });
    };
  }
};
/* harmony default export */ var visiblePanel = (visiblePanelActions);
// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/services/applicationService.js
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



var applicationService_ApplicationService =
/*#__PURE__*/
function () {
  function ApplicationService() {
    _classCallCheck(this, ApplicationService);
  }

  _createClass(ApplicationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getGeneralSettings",
    value: function getGeneralSettings(callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("GetGeneralSettings", {}, callback);
    }
  }, {
    key: "updateGeneralSettings",
    value: function updateGeneralSettings(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("UpdateGeneralSettings", payload, callback, failureCallback);
    }
  }, {
    key: "getRegexSettings",
    value: function getRegexSettings(callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("GetRegexSettings", {}, callback);
    }
  }, {
    key: "updateRegexSettings",
    value: function updateRegexSettings(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("UpdateRegexSettings", payload, callback, failureCallback);
    }
  }, {
    key: "testUrl",
    value: function testUrl(pageId, queryString, customPageName, callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("TestUrl?pageId=" + pageId + "&queryString=" + encodeURIComponent(queryString) + "&customPageName=" + encodeURIComponent(customPageName), {}, callback);
    }
  }, {
    key: "testUrlRewrite",
    value: function testUrlRewrite(uri, callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("TestUrlRewrite?uri=" + uri, {}, callback);
    }
  }, {
    key: "getSitemapSettings",
    value: function getSitemapSettings(callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("GetSitemapSettings", {}, callback);
    }
  }, {
    key: "updateSitemapSettings",
    value: function updateSitemapSettings(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("UpdateSitemapSettings", payload, callback, failureCallback);
    }
  }, {
    key: "getSitemapProviders",
    value: function getSitemapProviders(callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("GetSitemapProviders", {}, callback);
    }
  }, {
    key: "updateSitemapProvider",
    value: function updateSitemapProvider(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("UpdateSitemapProvider", payload, callback, failureCallback);
    }
  }, {
    key: "createVerification",
    value: function createVerification(verification, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("CreateVerification?verification=" + verification, {}, callback, failureCallback);
    }
  }, {
    key: "clearCache",
    value: function clearCache(callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("ResetCache", {}, callback, failureCallback);
    }
  }, {
    key: "getExtensionUrlProviders",
    value: function getExtensionUrlProviders(callback) {
      var sf = this.getServiceFramework("SEO");
      sf.get("GetExtensionUrlProviders", {}, callback);
    }
  }, {
    key: "updateExtensionUrlProviderStatus",
    value: function updateExtensionUrlProviderStatus(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("SEO");
      sf.post("UpdateExtensionUrlProviderStatus", payload, callback, failureCallback);
    }
  }]);

  return ApplicationService;
}();

var applicationService = new applicationService_ApplicationService();
/* harmony default export */ var services_applicationService = (applicationService);
// CONCATENATED MODULE: ./src/actions/seo.js


var siteInfoActions = {
  getGeneralSettings: function getGeneralSettings(callback) {
    return function (dispatch) {
      services_applicationService.getGeneralSettings(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].RETRIEVED_SEO_GENERAL_SETTINGS,
          data: {
            generalSettings: data.Settings,
            replacementCharacterList: data.ReplacementCharacterList,
            deletedPageHandlingTypes: data.DeletedPageHandlingTypes,
            clientModified: false,
            adminRoleId: data.AdminRoleId
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateGeneralSettings: function updateGeneralSettings(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateGeneralSettings(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].UPDATED_SEO_GENERAL_SETTINGS,
          data: {
            clientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  generalSettingsClientModified: function generalSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* seo */].SEO_GENERAL_SETTINS_CLIENT_MODIFIED,
        data: {
          generalSettings: parameter,
          clientModified: true
        }
      });
    };
  },
  getRegexSettings: function getRegexSettings(callback) {
    return function (dispatch) {
      services_applicationService.getRegexSettings(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].RETRIEVED_SEO_REGEX_SETTINGS,
          data: {
            regexSettings: data.Settings,
            regexClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateRegexSettings: function updateRegexSettings(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateRegexSettings(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].UPDATED_SEO_REGEX_SETTINGS,
          data: {
            regexClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  regexSettingsClientModified: function regexSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* seo */].SEO_REGEX_SETTINS_CLIENT_MODIFIED,
        data: {
          regexSettings: parameter,
          regexClientModified: true
        }
      });
    };
  },
  testUrl: function testUrl(pageId, queryString, customPageName, callback) {
    return function (dispatch) {
      services_applicationService.testUrl(pageId, queryString, customPageName, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].TESTED_SEO_PAGE_URL,
          data: {
            urls: data.Urls.join("\n")
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  testUrlRewrite: function testUrlRewrite(uri, callback) {
    return function (dispatch) {
      services_applicationService.testUrlRewrite(uri, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].TESTED_SEO_URL_REWRITING,
          data: {
            rewritingResult: data.RewritingResult.rewritingResult,
            culture: data.RewritingResult.culture,
            identifiedPage: data.RewritingResult.identifiedPage,
            redirectionReason: data.RewritingResult.redirectionReason,
            redirectionResult: data.RewritingResult.redirectionResult,
            operationMessages: data.RewritingResult.operationMessages
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  clearUrlTestResults: function clearUrlTestResults() {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* seo */].CLEARED_SEO_TEST_PAGE_URL_RESULTS,
        data: {
          urls: ""
        }
      });
    };
  },
  clearUrlRewritingTestResults: function clearUrlRewritingTestResults() {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* seo */].CLEARED_SEO_TEST_URL_REWRITING_RESULTS,
        data: {
          rewritingResult: "",
          culture: "",
          identifiedPage: "",
          redirectionReason: "",
          redirectionResult: "",
          operationMessages: ""
        }
      });
    };
  },
  getSitemapSettings: function getSitemapSettings(callback) {
    return function (dispatch) {
      services_applicationService.getSitemapSettings(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].RETRIEVED_SEO_SITEMAP_SETTINGS,
          data: {
            sitemapSettings: data.Settings,
            searchEngineUrls: data.SearchEngineUrls,
            clientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateSitemapSettings: function updateSitemapSettings(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateSitemapSettings(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].UPDATED_SEO_SITEMAP_SETTINGS,
          data: {
            clientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  sitemapSettingsClientModified: function sitemapSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* seo */].SEO_SITEMAP_SETTINS_CLIENT_MODIFIED,
        data: {
          sitemapSettings: parameter,
          clientModified: true
        }
      });
    };
  },
  clearCache: function clearCache(callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.clearCache(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].CLEARED_SEO_SITEMAP_CACHE,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  getSitemapProviders: function getSitemapProviders(callback) {
    return function (dispatch) {
      services_applicationService.getSitemapProviders(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].RETRIEVED_SEO_SITEMAP_PROVIDERS,
          data: {
            providers: data.Providers
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateSitemapProvider: function updateSitemapProvider(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateSitemapProvider(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].UPDATED_SEO_SITEMAP_PROVIDER,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  getExtensionUrlProviders: function getExtensionUrlProviders(callback) {
    return function (dispatch) {
      services_applicationService.getExtensionUrlProviders(function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].RETRIEVED_SEO_EXTENSION_URL_PROVIDERS,
          data: {
            providers: data.Providers
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateExtensionUrlProviderStatus: function updateExtensionUrlProviderStatus(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateExtensionUrlProviderStatus(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].UPDATED_SEO_EXTENSION_URL_PROVIDER,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  createVerification: function createVerification(verification, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.createVerification(verification, function (data) {
        dispatch({
          type: actionTypes["b" /* seo */].CREATED_SEO_SITEMAP_VERIFICATION,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  }
};
/* harmony default export */ var seo = (siteInfoActions);
// CONCATENATED MODULE: ./src/actions/index.js




// EXTERNAL MODULE: ./src/components/generalSettings/style.less
var style = __webpack_require__(9);
var style_default = /*#__PURE__*/__webpack_require__.n(style);

// CONCATENATED MODULE: ./src/resources/index.jsx

var resx = {
  get: function get(key) {
    var moduleName = utils["a" /* default */].moduleName;
    return utils["a" /* default */].utilities.getResx(moduleName, key);
  }
};
/* harmony default export */ var resources = (resx);
// CONCATENATED MODULE: ./src/components/generalSettings/index.jsx
function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function generalSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function generalSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function generalSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) generalSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) generalSettings_defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }











var generalSettings_GeneralSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(GeneralSettingsPanelBody, _Component);

  function GeneralSettingsPanelBody() {
    var _this;

    generalSettings_classCallCheck(this, GeneralSettingsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(GeneralSettingsPanelBody).call(this));
    _this.state = {
      generalSettings: undefined,
      triedToSubmit: false
    };
    return _this;
  }

  generalSettings_createClass(GeneralSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.generalSettings) {
        this.setState({
          generalSettings: props.generalSettings
        });
        return;
      }

      props.dispatch(seo.getGeneralSettings(function (data) {
        _this2.setState({
          generalSettings: _extends({}, data.Settings)
        });
      }));
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      this.setState({
        generalSettings: _extends({}, props.generalSettings),
        triedToSubmit: false
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;

      var generalSettings = _extends({}, state.generalSettings);

      if (key === "ReplaceSpaceWith") {
        generalSettings[key] = event.value;
      } else {
        generalSettings[key] = _typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        generalSettings: generalSettings,
        triedToSubmit: false
      });
      props.dispatch(seo.generalSettingsClientModified(generalSettings));
    }
  }, {
    key: "keyValuePairsToOptions",
    value: function keyValuePairsToOptions(keyValuePairs) {
      var options = [];

      if (keyValuePairs !== undefined) {
        options = keyValuePairs.map(function (item) {
          return {
            label: item.Key,
            value: item.Value
          };
        });
      }

      return options;
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });
      props.dispatch(seo.updateGeneralSettings(state.generalSettings, function () {
        utils["a" /* default */].utilities.notify(resources.get("SettingsUpdateSuccess"));
      }, function () {
        utils["a" /* default */].utilities.notifyError(resources.get("SettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
        props.dispatch(seo.getGeneralSettings(function (data) {
          _this3.setState({
            generalSettings: _extends({}, data.Settings)
          });
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;

      if (state.generalSettings) {
        var columnOne =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "left-column",
          key: "columnOne"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "columnTitle"
        }, resources.get("UrlRewriter")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("ReplacementCharacter.Help"),
          label: resources.get("ReplacementCharacter")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: this.keyValuePairsToOptions(props.replacementCharacterList),
          value: state.generalSettings.ReplaceSpaceWith,
          onSelect: this.onSettingChange.bind(this, "ReplaceSpaceWith")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("enableSystemGeneratedUrlsLabel.Help"),
          label: resources.get("enableSystemGeneratedUrlsLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.EnableSystemGeneratedUrls,
          onChange: this.onSettingChange.bind(this, "EnableSystemGeneratedUrls")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("enableLowerCaseLabel.Help"),
          label: resources.get("enableLowerCaseLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.ForceLowerCase,
          onChange: this.onSettingChange.bind(this, "ForceLowerCase")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("autoAsciiConvertLabel.Help"),
          label: resources.get("autoAsciiConvertLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.AutoAsciiConvert,
          onChange: this.onSettingChange.bind(this, "AutoAsciiConvert")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("setDefaultSiteLanguageLabel.Help"),
          label: resources.get("setDefaultSiteLanguageLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.ForcePortalDefaultLanguage,
          onChange: this.onSettingChange.bind(this, "ForcePortalDefaultLanguage")
        }))));
        var columnTwo =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "right-column",
          key: "columnTwo"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "columnTitle"
        }, resources.get("UrlRedirects")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row-options"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("plDeletedPages.Help"),
          label: resources.get("plDeletedPages")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["RadioButtons"], {
          onChange: this.onSettingChange.bind(this, "DeletedTabHandlingType"),
          options: this.keyValuePairsToOptions(props.deletedPageHandlingTypes),
          buttonGroup: "deletedTabHandlingType",
          value: state.generalSettings.DeletedTabHandlingType
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("enable301RedirectsLabel.Help"),
          label: resources.get("enable301RedirectsLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.RedirectUnfriendly,
          onChange: this.onSettingChange.bind(this, "RedirectUnfriendly")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "generalSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("redirectOnWrongCaseLabel.Help"),
          label: resources.get("redirectOnWrongCaseLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.generalSettings.RedirectWrongCase,
          onChange: this.onSettingChange.bind(this, "RedirectWrongCase")
        }))));
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: style_default.a.generalSettings
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
            numberOfColumns: 2
          }, [columnOne, columnTwo]),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.clientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, resources.get("Cancel")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.clientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, resources.get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null)
      );
    }
  }]);

  return GeneralSettingsPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

generalSettings_GeneralSettingsPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  generalSettings: prop_types_default.a.object,
  deletedPageHandlingTypes: prop_types_default.a.array,
  replacementCharacterList: prop_types_default.a.array,
  clientModified: prop_types_default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    generalSettings: state.seo.generalSettings,
    deletedPageHandlingTypes: state.seo.deletedPageHandlingTypes,
    replacementCharacterList: state.seo.replacementCharacterList,
    clientModified: state.seo.clientModified
  };
}

/* harmony default export */ var components_generalSettings = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(mapStateToProps)(generalSettings_GeneralSettingsPanelBody));
// EXTERNAL MODULE: ./src/components/regexSettings/style.less
var regexSettings_style = __webpack_require__(10);
var regexSettings_style_default = /*#__PURE__*/__webpack_require__.n(regexSettings_style);

// CONCATENATED MODULE: ./src/components/regexSettings/index.jsx
function regexSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { regexSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { regexSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return regexSettings_typeof(obj); }

function regexSettings_extends() { regexSettings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return regexSettings_extends.apply(this, arguments); }

function regexSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function regexSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function regexSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) regexSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) regexSettings_defineProperties(Constructor, staticProps); return Constructor; }

function regexSettings_possibleConstructorReturn(self, call) { if (call && (regexSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return regexSettings_assertThisInitialized(self); }

function regexSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function regexSettings_getPrototypeOf(o) { regexSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return regexSettings_getPrototypeOf(o); }

function regexSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) regexSettings_setPrototypeOf(subClass, superClass); }

function regexSettings_setPrototypeOf(o, p) { regexSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return regexSettings_setPrototypeOf(o, p); }











var regexSettings_RegexSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  regexSettings_inherits(RegexSettingsPanelBody, _Component);

  function RegexSettingsPanelBody() {
    var _this;

    regexSettings_classCallCheck(this, RegexSettingsPanelBody);

    _this = regexSettings_possibleConstructorReturn(this, regexSettings_getPrototypeOf(RegexSettingsPanelBody).call(this));
    _this.state = {
      regexSettings: undefined,
      triedToSubmit: false,
      error: {
        IgnoreRegex: false,
        DoNotRewriteRegex: false,
        UseSiteUrlsRegex: false,
        DoNotRedirectRegex: false,
        DoNotRedirectSecureRegex: false,
        ForceLowerCaseRegex: false,
        NoFriendlyUrlRegex: false,
        DoNotIncludeInPathRegex: false,
        ValidExtensionlessUrlsRegex: false,
        RegexMatch: false
      }
    };
    return _this;
  }

  regexSettings_createClass(RegexSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.regexSettings) {
        this.setState({
          regexSettings: props.regexSettings
        });
        return;
      }

      props.dispatch(seo.getRegexSettings(function (data) {
        _this2.setState({
          regexSettings: regexSettings_extends({}, data.Settings)
        });
      }));
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      this.setState({
        regexSettings: regexSettings_extends({}, props.regexSettings),
        triedToSubmit: false
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;

      var regexSettings = regexSettings_extends({}, state.regexSettings);

      regexSettings[key] = regexSettings_typeof(event) === "object" ? event.target.value : event;
      this.setState({
        regexSettings: regexSettings,
        triedToSubmit: false
      });
      props.dispatch(seo.regexSettingsClientModified(regexSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      var _this3 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true,
        error: {
          IgnoreRegex: false,
          DoNotRewriteRegex: false,
          UseSiteUrlsRegex: false,
          DoNotRedirectRegex: false,
          DoNotRedirectSecureRegex: false,
          ForceLowerCaseRegex: false,
          NoFriendlyUrlRegex: false,
          DoNotIncludeInPathRegex: false,
          ValidExtensionlessUrlsRegex: false,
          RegexMatch: false
        }
      });
      props.dispatch(seo.updateRegexSettings(state.regexSettings, function () {
        utils["a" /* default */].utilities.notify(resources.get("SettingsUpdateSuccess"));
      }, function (error) {
        var errorMessage = JSON.parse(error.responseText);

        if (errorMessage.Errors) {
          _this3.handleValidationErrors(errorMessage.Errors);
        } else {
          utils["a" /* default */].utilities.notifyError(resources.get("SettingsError"));
        }
      }));
    }
  }, {
    key: "handleValidationErrors",
    value: function handleValidationErrors(errors) {
      var state = this.state;

      if (errors.filter(function (error) {
        return error.Key === "IgnoreRegex";
      }).length > 0) {
        state.error["IgnoreRegex"] = true;
      } else {
        state.error["IgnoreRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "DoNotRewriteRegex";
      }).length > 0) {
        state.error["DoNotRewriteRegex"] = true;
      } else {
        state.error["DoNotRewriteRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "UseSiteUrlsRegex";
      }).length > 0) {
        state.error["UseSiteUrlsRegex"] = true;
      } else {
        state.error["UseSiteUrlsRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "DoNotRedirectRegex";
      }).length > 0) {
        state.error["DoNotRedirectRegex"] = true;
      } else {
        state.error["DoNotRedirectRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "DoNotRedirectSecureRegex";
      }).length > 0) {
        state.error["DoNotRedirectSecureRegex"] = true;
      } else {
        state.error["DoNotRedirectSecureRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "ForceLowerCaseRegex";
      }).length > 0) {
        state.error["ForceLowerCaseRegex"] = true;
      } else {
        state.error["ForceLowerCaseRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "NoFriendlyUrlRegex";
      }).length > 0) {
        state.error["NoFriendlyUrlRegex"] = true;
      } else {
        state.error["NoFriendlyUrlRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "DoNotIncludeInPathRegex";
      }).length > 0) {
        state.error["DoNotIncludeInPathRegex"] = true;
      } else {
        state.error["DoNotIncludeInPathRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "ValidExtensionlessUrlsRegex";
      }).length > 0) {
        state.error["ValidExtensionlessUrlsRegex"] = true;
      } else {
        state.error["ValidExtensionlessUrlsRegex"] = false;
      }

      if (errors.filter(function (error) {
        return error.Key === "RegexMatch";
      }).length > 0) {
        state.error["RegexMatch"] = true;
      } else {
        state.error["RegexMatch"] = false;
      }

      this.setState({
        error: state.error
      });
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this4 = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
        props.dispatch(seo.getRegexSettings(function (data) {
          _this4.setState({
            regexSettings: regexSettings_extends({}, data.Settings)
          });
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;

      if (state.regexSettings) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: regexSettings_style_default.a.regexSettings
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "columnTitle"
          }, resources.get("RegularExpressions")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("ignoreRegExLabel.Help"),
            label: resources.get("ignoreRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.IgnoreRegex && this.state.triedToSubmit,
            errorMessage: resources.get("ignoreRegExInvalidPattern"),
            value: state.regexSettings.IgnoreRegex,
            onChange: this.onSettingChange.bind(this, "IgnoreRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("doNotRewriteRegExLabel.Help"),
            label: resources.get("doNotRewriteRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.DoNotRewriteRegex && this.state.triedToSubmit,
            errorMessage: resources.get("doNotRewriteRegExInvalidPattern"),
            value: state.regexSettings.DoNotRewriteRegex,
            onChange: this.onSettingChange.bind(this, "DoNotRewriteRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("siteUrlsOnlyRegExLabel.Help"),
            label: resources.get("siteUrlsOnlyRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.UseSiteUrlsRegex && this.state.triedToSubmit,
            errorMessage: resources.get("siteUrlsOnlyRegExInvalidPattern"),
            value: state.regexSettings.UseSiteUrlsRegex,
            onChange: this.onSettingChange.bind(this, "UseSiteUrlsRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("doNotRedirectUrlRegExLabel.Help"),
            label: resources.get("doNotRedirectUrlRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.DoNotRedirectRegex && this.state.triedToSubmit,
            errorMessage: resources.get("doNotRedirectUrlRegExInvalidPattern"),
            value: state.regexSettings.DoNotRedirectRegex,
            onChange: this.onSettingChange.bind(this, "DoNotRedirectRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("doNotRedirectHttpsUrlRegExLabel.Help"),
            label: resources.get("doNotRedirectHttpsUrlRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.DoNotRedirectSecureRegex && this.state.triedToSubmit,
            errorMessage: resources.get("doNotRedirectHttpsUrlRegExInvalidPattern"),
            value: state.regexSettings.DoNotRedirectSecureRegex,
            onChange: this.onSettingChange.bind(this, "DoNotRedirectSecureRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("preventLowerCaseUrlRegExLabel.Help"),
            label: resources.get("preventLowerCaseUrlRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.ForceLowerCaseRegex && this.state.triedToSubmit,
            errorMessage: resources.get("preventLowerCaseUrlRegExInvalidPattern"),
            value: state.regexSettings.ForceLowerCaseRegex,
            onChange: this.onSettingChange.bind(this, "ForceLowerCaseRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("doNotUseFriendlyUrlsRegExLabel.Help"),
            label: resources.get("doNotUseFriendlyUrlsRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.NoFriendlyUrlRegex && this.state.triedToSubmit,
            errorMessage: resources.get("doNotUseFriendlyUrlsRegExInvalidPattern"),
            value: state.regexSettings.NoFriendlyUrlRegex,
            onChange: this.onSettingChange.bind(this, "NoFriendlyUrlRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("keepInQueryStringRegExLabel.Help"),
            label: resources.get("keepInQueryStringRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.DoNotIncludeInPathRegex && this.state.triedToSubmit,
            errorMessage: resources.get("keepInQueryStringRegExInvalidPattern"),
            value: state.regexSettings.DoNotIncludeInPathRegex,
            onChange: this.onSettingChange.bind(this, "DoNotIncludeInPathRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("urlsWithNoExtensionRegExLabel.Help"),
            label: resources.get("urlsWithNoExtensionRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.ValidExtensionlessUrlsRegex && this.state.triedToSubmit,
            errorMessage: resources.get("urlsWithNoExtensionRegExInvalidPattern"),
            value: state.regexSettings.ValidExtensionlessUrlsRegex,
            onChange: this.onSettingChange.bind(this, "ValidExtensionlessUrlsRegex")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "groupWrapper"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
            tooltipMessage: resources.get("validFriendlyUrlRegExLabel.Help"),
            label: resources.get("validFriendlyUrlRegExLabel"),
            extra:
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
            inputStyle: {
              margin: "0"
            },
            withLabel: false,
            error: this.state.error.RegexMatch && this.state.triedToSubmit,
            errorMessage: resources.get("validFriendlyUrlRegExInvalidPattern"),
            value: state.regexSettings.RegexMatch,
            onChange: this.onSettingChange.bind(this, "RegexMatch")
          }))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.regexClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, resources.get("Cancel")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.regexClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, resources.get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null)
      );
    }
  }]);

  return RegexSettingsPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

regexSettings_RegexSettingsPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  regexSettings: prop_types_default.a.object,
  regexClientModified: prop_types_default.a.bool
};

function regexSettings_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    regexSettings: state.seo.regexSettings,
    regexClientModified: state.seo.regexClientModified
  };
}

/* harmony default export */ var components_regexSettings = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(regexSettings_mapStateToProps)(regexSettings_RegexSettingsPanelBody));
// EXTERNAL MODULE: ./src/components/sitemapSettings/providerRow/style.less
var providerRow_style = __webpack_require__(30);

// CONCATENATED MODULE: ./src/components/sitemapSettings/providerRow/index.jsx
function providerRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { providerRow_typeof = function _typeof(obj) { return typeof obj; }; } else { providerRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return providerRow_typeof(obj); }

function providerRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function providerRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function providerRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) providerRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) providerRow_defineProperties(Constructor, staticProps); return Constructor; }

function providerRow_possibleConstructorReturn(self, call) { if (call && (providerRow_typeof(call) === "object" || typeof call === "function")) { return call; } return providerRow_assertThisInitialized(self); }

function providerRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function providerRow_getPrototypeOf(o) { providerRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return providerRow_getPrototypeOf(o); }

function providerRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) providerRow_setPrototypeOf(subClass, superClass); }

function providerRow_setPrototypeOf(o, p) { providerRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return providerRow_setPrototypeOf(o, p); }







var providerRow_ProviderRow =
/*#__PURE__*/
function (_Component) {
  providerRow_inherits(ProviderRow, _Component);

  function ProviderRow() {
    providerRow_classCallCheck(this, ProviderRow);

    return providerRow_possibleConstructorReturn(this, providerRow_getPrototypeOf(ProviderRow).call(this));
  }

  providerRow_createClass(ProviderRow, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var opened = this.props.openId !== "" && this.props.name === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      if (this.props.openId !== "" && this.props.name === this.props.openId) {//this.props.Collapse();
      } else {
        this.props.OpenCollapse(this.props.name);
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getEnabledDisplay",
    value: function getEnabledDisplay() {
      var props = this.props;

      if (props.enabled) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "item-row-enabled-display"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "enabled-icon",
            dangerouslySetInnerHTML: {
              __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].CheckMarkIcon
            }
          }))
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", null, "\xA0")
        );
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var opened = this.props.openId !== "" && this.props.name === this.props.openId;

      if (props.visible) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "collapsible-component-providers" + (opened ? " row-opened" : "")
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "collapsible-providers " + !opened
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "row"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-name"
          }, props.name),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-enabled"
          }, this.getEnabledDisplay()),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-priority"
          }, props.overridePriority ? props.priority : resources.get("None")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-editButton"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: opened ? "edit-icon-active" : "edit-icon",
            dangerouslySetInnerHTML: {
              __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
            },
            onClick: this.toggle.bind(this)
          })))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
            accordion: true,
            isOpened: opened,
            style: {
              float: "left",
              overflow: "inherit"
            },
            fixedHeight: 160
          }, opened && props.children))
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", null)
        );
      }
    }
  }]);

  return ProviderRow;
}(external_window_dnn_nodeModules_React_["Component"]);

providerRow_ProviderRow.propTypes = {
  name: prop_types_default.a.string,
  enabled: prop_types_default.a.bool,
  priority: prop_types_default.a.number,
  overridePriority: prop_types_default.a.bool,
  OpenCollapse: prop_types_default.a.func,
  Collapse: prop_types_default.a.func,
  id: prop_types_default.a.string,
  openId: prop_types_default.a.string,
  visible: prop_types_default.a.bool
};
providerRow_ProviderRow.defaultProps = {
  collapsed: true,
  visible: true
};
/* harmony default export */ var providerRow = (providerRow_ProviderRow);
// EXTERNAL MODULE: ./src/components/sitemapSettings/providerEditor/style.less
var providerEditor_style = __webpack_require__(11);
var providerEditor_style_default = /*#__PURE__*/__webpack_require__.n(providerEditor_style);

// CONCATENATED MODULE: ./src/components/sitemapSettings/providerEditor/index.jsx
function providerEditor_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { providerEditor_typeof = function _typeof(obj) { return typeof obj; }; } else { providerEditor_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return providerEditor_typeof(obj); }

function providerEditor_extends() { providerEditor_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return providerEditor_extends.apply(this, arguments); }

function providerEditor_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function providerEditor_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function providerEditor_createClass(Constructor, protoProps, staticProps) { if (protoProps) providerEditor_defineProperties(Constructor.prototype, protoProps); if (staticProps) providerEditor_defineProperties(Constructor, staticProps); return Constructor; }

function providerEditor_possibleConstructorReturn(self, call) { if (call && (providerEditor_typeof(call) === "object" || typeof call === "function")) { return call; } return providerEditor_assertThisInitialized(self); }

function providerEditor_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function providerEditor_getPrototypeOf(o) { providerEditor_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return providerEditor_getPrototypeOf(o); }

function providerEditor_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) providerEditor_setPrototypeOf(subClass, superClass); }

function providerEditor_setPrototypeOf(o, p) { providerEditor_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return providerEditor_setPrototypeOf(o, p); }







var priorityOptions = [];

var providerEditor_ProviderEditor =
/*#__PURE__*/
function (_Component) {
  providerEditor_inherits(ProviderEditor, _Component);

  function ProviderEditor() {
    var _this;

    providerEditor_classCallCheck(this, ProviderEditor);

    _this = providerEditor_possibleConstructorReturn(this, providerEditor_getPrototypeOf(ProviderEditor).call(this));
    _this.state = {
      settings: {}
    };
    return _this;
  }

  providerEditor_createClass(ProviderEditor, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;
      priorityOptions = [];
      priorityOptions.push({
        "value": -1,
        "label": resources.get("None")
      });
      priorityOptions.push({
        "value": 1,
        "label": "1"
      });
      priorityOptions.push({
        "value": 0.9,
        "label": "0.9"
      });
      priorityOptions.push({
        "value": 0.8,
        "label": "0.8"
      });
      priorityOptions.push({
        "value": 0.7,
        "label": "0.7"
      });
      priorityOptions.push({
        "value": 0.6,
        "label": "0.6"
      });
      priorityOptions.push({
        "value": 0.5,
        "label": "0.5"
      });
      priorityOptions.push({
        "value": 0.4,
        "label": "0.4"
      });
      priorityOptions.push({
        "value": 0.3,
        "label": "0.3"
      });
      priorityOptions.push({
        "value": 0.2,
        "label": "0.2"
      });
      priorityOptions.push({
        "value": 0.1,
        "label": "0.1"
      });
      priorityOptions.push({
        "value": 0,
        "label": "0"
      });
      this.setState({
        settings: providerEditor_extends({}, props.settings)
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var settings = this.state.settings;

      if (key === "Priority") {
        settings[key] = parseFloat(event.value);

        if (parseFloat(event.value) !== -1) {
          settings["OverridePriority"] = true;
        } else {
          settings["OverridePriority"] = false;
        }
      } else {
        settings[key] = providerEditor_typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        settings: settings
      });
    }
  }, {
    key: "onUpdateItem",
    value: function onUpdateItem(event) {
      event.preventDefault();
      var state = this.state;
      this.props.onUpdate(state.settings);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;

      if (state.settings) {
        var columnOne =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "left-column"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "providerSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("enableSitemapProvider.Help"),
          label: resources.get("enableSitemapProvider")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.settings.Enabled,
          onChange: this.onSettingChange.bind(this, "Enabled")
        }))));
        var columnTwo =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "right-column"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "providerSettings-row_dropdown"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("overridePriority.Help"),
          label: resources.get("overridePriority")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: priorityOptions,
          value: !state.settings.OverridePriority ? -1 : state.settings.Priority,
          onSelect: this.onSettingChange.bind(this, "Priority")
        }))));
        /* eslint-disable react/no-danger */

        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: providerEditor_style_default.a.providerSettingEditor
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            style: {
              height: "15px"
            }
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
            numberOfColumns: 2
          }, [columnOne, columnTwo]),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "buttons-box-secondary"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "secondary",
            onClick: this.props.Collapse.bind(this)
          }, resources.get("Cancel")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "primary",
            onClick: this.onUpdateItem.bind(this)
          }, resources.get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null)
      );
    }
  }]);

  return ProviderEditor;
}(external_window_dnn_nodeModules_React_["Component"]);

providerEditor_ProviderEditor.propTypes = {
  settings: prop_types_default.a.object,
  Collapse: prop_types_default.a.func,
  onUpdate: prop_types_default.a.func
};
/* harmony default export */ var providerEditor = (providerEditor_ProviderEditor);
// EXTERNAL MODULE: ./src/components/sitemapSettings/style.less
var sitemapSettings_style = __webpack_require__(12);
var sitemapSettings_style_default = /*#__PURE__*/__webpack_require__.n(sitemapSettings_style);

// CONCATENATED MODULE: ./src/components/sitemapSettings/index.jsx
function sitemapSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { sitemapSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { sitemapSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return sitemapSettings_typeof(obj); }

function sitemapSettings_extends() { sitemapSettings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return sitemapSettings_extends.apply(this, arguments); }

function sitemapSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function sitemapSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function sitemapSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) sitemapSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) sitemapSettings_defineProperties(Constructor, staticProps); return Constructor; }

function sitemapSettings_possibleConstructorReturn(self, call) { if (call && (sitemapSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return sitemapSettings_assertThisInitialized(self); }

function sitemapSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function sitemapSettings_getPrototypeOf(o) { sitemapSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return sitemapSettings_getPrototypeOf(o); }

function sitemapSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) sitemapSettings_setPrototypeOf(subClass, superClass); }

function sitemapSettings_setPrototypeOf(o, p) { sitemapSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return sitemapSettings_setPrototypeOf(o, p); }












var daysToCacheOptions = [];
var sitemapSettings_priorityOptions = [];
var tableFields = [];
var re = /[^].html$/;

var sitemapSettings_SitemapSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  sitemapSettings_inherits(SitemapSettingsPanelBody, _Component);

  function SitemapSettingsPanelBody() {
    var _this;

    sitemapSettings_classCallCheck(this, SitemapSettingsPanelBody);

    _this = sitemapSettings_possibleConstructorReturn(this, sitemapSettings_getPrototypeOf(SitemapSettingsPanelBody).call(this));
    _this.state = {
      sitemapSettings: undefined,
      searchEngine: "Google",
      verification: "",
      triedToSubmit: false,
      triedToCreate: false,
      openId: "",
      error: {
        verificationValidity: true
      }
    };
    return _this;
  }

  sitemapSettings_createClass(SitemapSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.sitemapSettings) {
        this.setState({
          sitemapSettings: props.sitemapSettings
        });
        return;
      }

      daysToCacheOptions = [];
      daysToCacheOptions.push({
        "value": 0,
        "label": resources.get("DisableCaching")
      });
      daysToCacheOptions.push({
        "value": 1,
        "label": resources.get("1Day")
      });
      daysToCacheOptions.push({
        "value": 2,
        "label": resources.get("2Days")
      });
      daysToCacheOptions.push({
        "value": 3,
        "label": resources.get("3Days")
      });
      daysToCacheOptions.push({
        "value": 4,
        "label": resources.get("4Days")
      });
      daysToCacheOptions.push({
        "value": 5,
        "label": resources.get("5Days")
      });
      daysToCacheOptions.push({
        "value": 6,
        "label": resources.get("6Days")
      });
      daysToCacheOptions.push({
        "value": 7,
        "label": resources.get("7Days")
      });
      sitemapSettings_priorityOptions = [];
      sitemapSettings_priorityOptions.push({
        "value": 1,
        "label": "1"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.9,
        "label": "0.9"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.8,
        "label": "0.8"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.7,
        "label": "0.7"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.6,
        "label": "0.6"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.5,
        "label": "0.5"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.4,
        "label": "0.4"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.3,
        "label": "0.3"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.2,
        "label": "0.2"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0.1,
        "label": "0.1"
      });
      sitemapSettings_priorityOptions.push({
        "value": 0,
        "label": "0"
      });
      tableFields = [];
      tableFields.push({
        "name": resources.get("Name.Header"),
        "id": "Name"
      });
      tableFields.push({
        "name": resources.get("Enabled.Header"),
        "id": "Enabled"
      });
      tableFields.push({
        "name": resources.get("Priority.Header"),
        "id": "Priority"
      });
      props.dispatch(seo.getSitemapSettings(function (data) {
        _this2.setState({
          sitemapSettings: sitemapSettings_extends({}, data.Settings)
        });
      }));
      props.dispatch(seo.getSitemapProviders(function (data) {
        _this2.setState({
          sitemapProviders: sitemapSettings_extends({}, data.Providers)
        });
      }));
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      this.setState({
        sitemapSettings: sitemapSettings_extends({}, props.sitemapSettings),
        triedToSubmit: false,
        triedToCreate: false
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;

      var sitemapSettings = sitemapSettings_extends({}, state.sitemapSettings);

      if (key === "SitemapExcludePriority" || key === "SitemapCacheDays" || key === "SitemapMinPriority") {
        sitemapSettings[key] = event.value;
      } else {
        sitemapSettings[key] = sitemapSettings_typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        sitemapSettings: sitemapSettings,
        triedToSubmit: false
      });
      props.dispatch(seo.sitemapSettingsClientModified(sitemapSettings));
    }
  }, {
    key: "searchEngineListToOptions",
    value: function searchEngineListToOptions(list) {
      var options = [];

      if (list !== undefined) {
        options = list.map(function (item) {
          return {
            label: item.Key,
            value: item.Key
          };
        });
      }

      return options;
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });
      props.dispatch(seo.updateSitemapSettings(state.sitemapSettings, function () {
        utils["a" /* default */].utilities.notify(resources.get("SettingsUpdateSuccess"));
      }, function () {
        utils["a" /* default */].utilities.notifyError(resources.get("SettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
        props.dispatch(seo.getSitemapSettings(function (data) {
          _this3.setState({
            sitemapSettings: sitemapSettings_extends({}, data.Settings)
          });
        }));
      });
    }
  }, {
    key: "onClearCache",
    value: function onClearCache() {
      var props = this.props;
      props.dispatch(seo.clearCache());
    }
  }, {
    key: "onUpdateProvider",
    value: function onUpdateProvider(settings) {
      var _this4 = this;

      var props = this.props;
      props.dispatch(seo.updateSitemapProvider(settings, function () {
        utils["a" /* default */].utilities.notify(resources.get("SettingsUpdateSuccess"));
        props.dispatch(seo.getSitemapProviders(function (data) {
          _this4.setState({
            sitemapProviders: sitemapSettings_extends({}, data.Providers)
          });
        }));

        _this4.collapse();
      }, function () {
        utils["a" /* default */].utilities.notifyError(resources.get("SettingsError"));
      }));
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id) {
      var _this5 = this;

      setTimeout(function () {
        _this5.setState({
          openId: id
        });
      }, this.timeout);
    }
  }, {
    key: "collapse",
    value: function collapse() {
      if (this.state.openId !== "") {
        this.setState({
          openId: ""
        });
      }
    }
  }, {
    key: "toggle",
    value: function toggle(openId) {
      if (openId !== "") {
        this.uncollapse(openId);
      } else {
        this.collapse();
      }
    }
  }, {
    key: "renderProvidersHeader",
    value: function renderProvidersHeader() {
      var tableHeaders = tableFields.map(function (field) {
        var className = "provider-items header-" + field.id;
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: className,
            key: "header-" + field.id
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedProviders",
    value: function renderedProviders() {
      var _this6 = this;

      if (this.props.providers) {
        return this.props.providers.map(function (item, index) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(providerRow, {
              name: item.Name,
              enabled: item.Enabled,
              priority: item.Priority,
              overridePriority: item.OverridePriority,
              index: index,
              key: "provider-" + index,
              closeOnClick: true,
              openId: _this6.state.openId,
              OpenCollapse: _this6.toggle.bind(_this6),
              Collapse: _this6.collapse.bind(_this6)
            },
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(providerEditor, {
              settings: item,
              Collapse: _this6.collapse.bind(_this6),
              onUpdate: _this6.onUpdateProvider.bind(_this6),
              openId: _this6.state.openId
            }))
          );
        });
      }
    }
  }, {
    key: "onSearchEngineChange",
    value: function onSearchEngineChange(event) {
      this.setState({
        searchEngine: event.value
      });
    }
  }, {
    key: "onSearchEngineSubmit",
    value: function onSearchEngineSubmit() {
      var state = this.state,
          props = this.props;
      var url = props.searchEngineUrls.filter(function (item) {
        return item.Key === state.searchEngine;
      })[0].Value;
      window.open(url, "_blank");
    }
  }, {
    key: "onVerificationChange",
    value: function onVerificationChange(event) {
      var state = this.state;
      var verification = sitemapSettings_typeof(event) === "object" ? event.target.value : event;

      if (verification === "" || !re.test(verification)) {
        state.error["verificationValidity"] = true;
      } else if (verification !== "" && re.test(verification)) {
        state.error["verificationValidity"] = false;
      }

      this.setState({
        verification: verification,
        error: state.error,
        triedToCreate: false
      });
    }
  }, {
    key: "onCreateVerification",
    value: function onCreateVerification() {
      var state = this.state,
          props = this.props;
      this.setState({
        triedToCreate: true
      });

      if (state.error.verificationValidity) {
        return;
      }

      props.dispatch(seo.createVerification(state.verification));
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;

      if (state.sitemapSettings) {
        var columnOne =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "left-column",
          key: "columnOne"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("sitemapUrlLabel.Help"),
          label: resources.get("sitemapUrlLabel")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "sitemapUrl"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("a", {
          href: state.sitemapSettings.SitemapUrl,
          target: "_blank",
          rel: "noopener noreferrer"
        }, state.sitemapSettings.SitemapUrl))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("lblExcludePriority.Help"),
          label: resources.get("lblExcludePriority")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: sitemapSettings_priorityOptions,
          value: state.sitemapSettings.SitemapExcludePriority,
          onSelect: this.onSettingChange.bind(this, "SitemapExcludePriority")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "sitemapSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("lblIncludeHidden.Help"),
          label: resources.get("lblIncludeHidden")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.sitemapSettings.SitemapIncludeHidden,
          onChange: this.onSettingChange.bind(this, "SitemapIncludeHidden")
        }))));
        var columnTwo =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "right-column",
          key: "columnTwo"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("lblCache.Help"),
          label: resources.get("lblCache")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "daysToCache"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: daysToCacheOptions,
          value: state.sitemapSettings.SitemapCacheDays,
          onSelect: this.onSettingChange.bind(this, "SitemapCacheDays")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          className: "clearCacheBtn",
          type: "secondary",
          onClick: this.onClearCache.bind(this)
        }, resources.get("lnkResetCache")))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("lblMinPagePriority.Help"),
          label: resources.get("lblMinPagePriority")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: sitemapSettings_priorityOptions,
          value: state.sitemapSettings.SitemapMinPriority,
          onSelect: this.onSettingChange.bind(this, "SitemapMinPriority")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "sitemapSettings-row_switch"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          labelType: "inline",
          tooltipMessage: resources.get("lblLevelPriority.Help"),
          label: resources.get("lblLevelPriority")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: state.sitemapSettings.SitemapLevelMode,
          onChange: this.onSettingChange.bind(this, "SitemapLevelMode")
        }))));
        var columnThree =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "left-column",
          key: "columnThree"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("lblSearchEngine.Help"),
          label: resources.get("lblSearchEngine")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "searchEngineSubmit"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: this.searchEngineListToOptions(props.searchEngineUrls),
          value: state.searchEngine,
          onSelect: this.onSearchEngineChange.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          className: "searchEngineSubmitBtn",
          type: "secondary",
          onClick: this.onSearchEngineSubmit.bind(this)
        }, resources.get("Submit")))));
        var columnFour =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "right-column",
          key: "columnFour"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          tooltipMessage: resources.get("lblVerification.Help"),
          label: resources.get("lblVerification")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "createVerification"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          inputStyle: {
            margin: "0"
          },
          withLabel: false,
          error: this.state.error.verificationValidity && this.state.triedToCreate,
          errorMessage: resources.get("VerificationValidity.ErrorMessage"),
          value: state.verification,
          onChange: this.onVerificationChange.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          className: "createVerificationBtn",
          type: "secondary",
          onClick: this.onCreateVerification.bind(this)
        }, resources.get("Create")))));
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: sitemapSettings_style_default.a.sitemapSettings
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "columnTitle"
          }, resources.get("SitemapSettings")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
            numberOfColumns: 2
          }, [columnOne, columnTwo]),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "columnTitle2"
          }, resources.get("SitemapProviders")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-items-grid"
          }, this.renderProvidersHeader(), this.renderedProviders()),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "columnTitle3"
          }, resources.get("SiteSubmission")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
            numberOfColumns: 2
          }, [columnThree, columnFour]),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.clientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, resources.get("Cancel")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !this.props.clientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, resources.get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null)
      );
    }
  }]);

  return SitemapSettingsPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

sitemapSettings_SitemapSettingsPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  sitemapSettings: prop_types_default.a.object,
  searchEngineUrls: prop_types_default.a.array,
  clientModified: prop_types_default.a.bool,
  providers: prop_types_default.a.array
};

function sitemapSettings_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    sitemapSettings: state.seo.sitemapSettings,
    searchEngineUrls: state.seo.searchEngineUrls,
    clientModified: state.seo.clientModified,
    providers: state.seo.sitemapProviders
  };
}

/* harmony default export */ var components_sitemapSettings = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(sitemapSettings_mapStateToProps)(sitemapSettings_SitemapSettingsPanelBody));
// EXTERNAL MODULE: ./src/components/extensionUrlProviders/providerRow/style.less
var extensionUrlProviders_providerRow_style = __webpack_require__(34);

// CONCATENATED MODULE: ./src/components/extensionUrlProviders/providerRow/index.jsx
function extensionUrlProviders_providerRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { extensionUrlProviders_providerRow_typeof = function _typeof(obj) { return typeof obj; }; } else { extensionUrlProviders_providerRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return extensionUrlProviders_providerRow_typeof(obj); }

function extensionUrlProviders_providerRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function extensionUrlProviders_providerRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function extensionUrlProviders_providerRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) extensionUrlProviders_providerRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) extensionUrlProviders_providerRow_defineProperties(Constructor, staticProps); return Constructor; }

function extensionUrlProviders_providerRow_possibleConstructorReturn(self, call) { if (call && (extensionUrlProviders_providerRow_typeof(call) === "object" || typeof call === "function")) { return call; } return extensionUrlProviders_providerRow_assertThisInitialized(self); }

function extensionUrlProviders_providerRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function extensionUrlProviders_providerRow_getPrototypeOf(o) { extensionUrlProviders_providerRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return extensionUrlProviders_providerRow_getPrototypeOf(o); }

function extensionUrlProviders_providerRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) extensionUrlProviders_providerRow_setPrototypeOf(subClass, superClass); }

function extensionUrlProviders_providerRow_setPrototypeOf(o, p) { extensionUrlProviders_providerRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return extensionUrlProviders_providerRow_setPrototypeOf(o, p); }






var extensionUrlProviders_providerRow_ProviderRow =
/*#__PURE__*/
function (_Component) {
  extensionUrlProviders_providerRow_inherits(ProviderRow, _Component);

  function ProviderRow() {
    extensionUrlProviders_providerRow_classCallCheck(this, ProviderRow);

    return extensionUrlProviders_providerRow_possibleConstructorReturn(this, extensionUrlProviders_providerRow_getPrototypeOf(ProviderRow).call(this));
  }

  extensionUrlProviders_providerRow_createClass(ProviderRow, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var opened = this.props.openId !== "" && this.props.name === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      if (this.props.openId !== "" && this.props.name === this.props.openId) {
        this.props.Collapse();
      } else {
        this.props.OpenCollapse(this.props.name);
      }
    }
  }, {
    key: "updateStatus",
    value: function updateStatus(event) {
      var props = this.props;
      this.props.onUpdateStatus(props.providerId, event);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var opened = this.props.openId !== "" && this.props.name === this.props.openId;

      if (props.visible) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "collapsible-component-extention-url-providers" + (opened ? " row-opened" : "")
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "collapsible-providers " + !opened
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "row"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-name"
          }, props.name),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-enabled"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Checkbox"], {
            value: props.enabled,
            onChange: this.updateStatus.bind(this)
          })),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "provider-item item-row-editButton"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: opened ? "edit-icon-active" : "edit-icon",
            dangerouslySetInnerHTML: {
              __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
            },
            onClick: this.toggle.bind(this)
          })))),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
            accordion: true,
            isOpened: opened,
            style: {
              float: "left",
              overflow: "inherit",
              width: "100%"
            },
            fixedHeight: 160
          }, opened && props.children))
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", null)
        );
      }
    }
  }]);

  return ProviderRow;
}(external_window_dnn_nodeModules_React_["Component"]);

extensionUrlProviders_providerRow_ProviderRow.propTypes = {
  providerId: prop_types_default.a.number,
  name: prop_types_default.a.string,
  enabled: prop_types_default.a.bool,
  OpenCollapse: prop_types_default.a.func,
  Collapse: prop_types_default.a.func,
  id: prop_types_default.a.string,
  openId: prop_types_default.a.string,
  visible: prop_types_default.a.bool,
  onUpdateStatus: prop_types_default.a.func
};
extensionUrlProviders_providerRow_ProviderRow.defaultProps = {
  collapsed: true,
  visible: true
};
/* harmony default export */ var extensionUrlProviders_providerRow = (extensionUrlProviders_providerRow_ProviderRow);
// EXTERNAL MODULE: ./src/components/extensionUrlProviders/providerEditor/style.less
var extensionUrlProviders_providerEditor_style = __webpack_require__(13);
var extensionUrlProviders_providerEditor_style_default = /*#__PURE__*/__webpack_require__.n(extensionUrlProviders_providerEditor_style);

// CONCATENATED MODULE: ./src/components/extensionUrlProviders/providerEditor/index.jsx
function extensionUrlProviders_providerEditor_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { extensionUrlProviders_providerEditor_typeof = function _typeof(obj) { return typeof obj; }; } else { extensionUrlProviders_providerEditor_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return extensionUrlProviders_providerEditor_typeof(obj); }

function extensionUrlProviders_providerEditor_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function extensionUrlProviders_providerEditor_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function extensionUrlProviders_providerEditor_createClass(Constructor, protoProps, staticProps) { if (protoProps) extensionUrlProviders_providerEditor_defineProperties(Constructor.prototype, protoProps); if (staticProps) extensionUrlProviders_providerEditor_defineProperties(Constructor, staticProps); return Constructor; }

function extensionUrlProviders_providerEditor_possibleConstructorReturn(self, call) { if (call && (extensionUrlProviders_providerEditor_typeof(call) === "object" || typeof call === "function")) { return call; } return extensionUrlProviders_providerEditor_assertThisInitialized(self); }

function extensionUrlProviders_providerEditor_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function extensionUrlProviders_providerEditor_getPrototypeOf(o) { extensionUrlProviders_providerEditor_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return extensionUrlProviders_providerEditor_getPrototypeOf(o); }

function extensionUrlProviders_providerEditor_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) extensionUrlProviders_providerEditor_setPrototypeOf(subClass, superClass); }

function extensionUrlProviders_providerEditor_setPrototypeOf(o, p) { extensionUrlProviders_providerEditor_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return extensionUrlProviders_providerEditor_setPrototypeOf(o, p); }






var extensionUrlProviders_providerEditor_ProviderEditor =
/*#__PURE__*/
function (_Component) {
  extensionUrlProviders_providerEditor_inherits(ProviderEditor, _Component);

  function ProviderEditor() {
    extensionUrlProviders_providerEditor_classCallCheck(this, ProviderEditor);

    return extensionUrlProviders_providerEditor_possibleConstructorReturn(this, extensionUrlProviders_providerEditor_getPrototypeOf(ProviderEditor).call(this));
  }
  /* eslint-disable react/no-danger */


  extensionUrlProviders_providerEditor_createClass(ProviderEditor, [{
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: extensionUrlProviders_providerEditor_style_default.a.providerSettingEditor
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("iframe", {
          className: "edit-provider",
          seamless: true,
          src: this.props.settingUrl
        }))
      );
    }
  }]);

  return ProviderEditor;
}(external_window_dnn_nodeModules_React_["Component"]);

extensionUrlProviders_providerEditor_ProviderEditor.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  settingUrl: prop_types_default.a.string,
  Collapse: prop_types_default.a.func
};
/* harmony default export */ var extensionUrlProviders_providerEditor = (extensionUrlProviders_providerEditor_ProviderEditor);
// EXTERNAL MODULE: ./src/components/extensionUrlProviders/style.less
var extensionUrlProviders_style = __webpack_require__(14);
var extensionUrlProviders_style_default = /*#__PURE__*/__webpack_require__.n(extensionUrlProviders_style);

// CONCATENATED MODULE: ./src/components/extensionUrlProviders/index.jsx
function extensionUrlProviders_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { extensionUrlProviders_typeof = function _typeof(obj) { return typeof obj; }; } else { extensionUrlProviders_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return extensionUrlProviders_typeof(obj); }

function extensionUrlProviders_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function extensionUrlProviders_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function extensionUrlProviders_createClass(Constructor, protoProps, staticProps) { if (protoProps) extensionUrlProviders_defineProperties(Constructor.prototype, protoProps); if (staticProps) extensionUrlProviders_defineProperties(Constructor, staticProps); return Constructor; }

function extensionUrlProviders_possibleConstructorReturn(self, call) { if (call && (extensionUrlProviders_typeof(call) === "object" || typeof call === "function")) { return call; } return extensionUrlProviders_assertThisInitialized(self); }

function extensionUrlProviders_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function extensionUrlProviders_getPrototypeOf(o) { extensionUrlProviders_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return extensionUrlProviders_getPrototypeOf(o); }

function extensionUrlProviders_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) extensionUrlProviders_setPrototypeOf(subClass, superClass); }

function extensionUrlProviders_setPrototypeOf(o, p) { extensionUrlProviders_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return extensionUrlProviders_setPrototypeOf(o, p); }












var extensionUrlProviders_tableFields = [];

var extensionUrlProviders_ExtensionUrlProvidersPanelBody =
/*#__PURE__*/
function (_Component) {
  extensionUrlProviders_inherits(ExtensionUrlProvidersPanelBody, _Component);

  function ExtensionUrlProvidersPanelBody() {
    var _this;

    extensionUrlProviders_classCallCheck(this, ExtensionUrlProvidersPanelBody);

    _this = extensionUrlProviders_possibleConstructorReturn(this, extensionUrlProviders_getPrototypeOf(ExtensionUrlProvidersPanelBody).call(this));
    _this.state = {
      openId: ""
    };
    return _this;
  }

  extensionUrlProviders_createClass(ExtensionUrlProvidersPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;
      extensionUrlProviders_tableFields = [];
      extensionUrlProviders_tableFields.push({
        "name": resources.get("Name.Header"),
        "id": "Name"
      });
      extensionUrlProviders_tableFields.push({
        "name": resources.get("Enabled.Header"),
        "id": "Enabled"
      });
      props.dispatch(seo.getExtensionUrlProviders());
    }
  }, {
    key: "onUpdateProviderStatus",
    value: function onUpdateProviderStatus(providerId, isActive) {
      var props = this.props;
      props.dispatch(seo.updateExtensionUrlProviderStatus({
        ProviderId: providerId,
        IsActive: isActive
      }, function () {
        utils["a" /* default */].utilities.notify(resources.get("SettingsUpdateSuccess"));
      }, function () {
        utils["a" /* default */].utilities.notifyError(resources.get("SettingsError"));
      }));
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id) {
      var _this2 = this;

      setTimeout(function () {
        _this2.setState({
          openId: id
        });
      }, this.timeout);
    }
  }, {
    key: "collapse",
    value: function collapse() {
      if (this.state.openId !== "") {
        this.setState({
          openId: ""
        });
      }
    }
  }, {
    key: "toggle",
    value: function toggle(openId) {
      if (openId !== "") {
        this.uncollapse(openId);
      } else {
        this.collapse();
      }
    }
  }, {
    key: "renderProvidersHeader",
    value: function renderProvidersHeader() {
      var tableHeaders = extensionUrlProviders_tableFields.map(function (field) {
        var className = "provider-items header-" + field.id;
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: className,
            key: "header-" + field.id
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedProviders",
    value: function renderedProviders() {
      var _this3 = this;

      if (this.props.providers) {
        if (this.props.providers.length > 0) {
          return this.props.providers.map(function (item, index) {
            return (
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(extensionUrlProviders_providerRow, {
                providerId: item.ExtensionUrlProviderId,
                name: item.ProviderName,
                enabled: item.IsActive,
                index: index,
                key: "provider-" + index,
                closeOnClick: true,
                openId: _this3.state.openId,
                onUpdateStatus: _this3.onUpdateProviderStatus.bind(_this3),
                OpenCollapse: _this3.toggle.bind(_this3),
                Collapse: _this3.collapse.bind(_this3)
              },
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(extensionUrlProviders_providerEditor, {
                settingUrl: item.SettingUrl,
                Collapse: _this3.collapse.bind(_this3),
                openId: _this3.state.openId
              }))
            );
          });
        } else {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
              className: "no-extension-url-providers"
            }, resources.get("NoExtensionUrlProviders"))
          );
        }
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: extensionUrlProviders_style_default.a.extensionUrlProviders
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "columnTitle"
        }, resources.get("ExtensionUrlProviders")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "provider-items-grid"
        }, this.renderProvidersHeader(), this.renderedProviders()))
      );
    }
  }]);

  return ExtensionUrlProvidersPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

extensionUrlProviders_ExtensionUrlProvidersPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  providers: prop_types_default.a.array
};

function extensionUrlProviders_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    providers: state.seo.extensionUrlProviders
  };
}

/* harmony default export */ var extensionUrlProviders = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(extensionUrlProviders_mapStateToProps)(extensionUrlProviders_ExtensionUrlProvidersPanelBody));
// EXTERNAL MODULE: ./src/components/testUrl/style.less
var testUrl_style = __webpack_require__(15);
var testUrl_style_default = /*#__PURE__*/__webpack_require__.n(testUrl_style);

// CONCATENATED MODULE: ./src/components/testUrl/index.jsx
function testUrl_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { testUrl_typeof = function _typeof(obj) { return typeof obj; }; } else { testUrl_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return testUrl_typeof(obj); }

function testUrl_extends() { testUrl_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return testUrl_extends.apply(this, arguments); }

function testUrl_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function testUrl_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function testUrl_createClass(Constructor, protoProps, staticProps) { if (protoProps) testUrl_defineProperties(Constructor.prototype, protoProps); if (staticProps) testUrl_defineProperties(Constructor, staticProps); return Constructor; }

function testUrl_possibleConstructorReturn(self, call) { if (call && (testUrl_typeof(call) === "object" || typeof call === "function")) { return call; } return testUrl_assertThisInitialized(self); }

function testUrl_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function testUrl_getPrototypeOf(o) { testUrl_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return testUrl_getPrototypeOf(o); }

function testUrl_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) testUrl_setPrototypeOf(subClass, superClass); }

function testUrl_setPrototypeOf(o, p) { testUrl_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return testUrl_setPrototypeOf(o, p); }











var testUrl_TestUrlPanelBody =
/*#__PURE__*/
function (_Component) {
  testUrl_inherits(TestUrlPanelBody, _Component);

  function TestUrlPanelBody() {
    var _this;

    testUrl_classCallCheck(this, TestUrlPanelBody);

    _this = testUrl_possibleConstructorReturn(this, testUrl_getPrototypeOf(TestUrlPanelBody).call(this));
    _this.state = {
      test: {
        PageToTest: "",
        QueryString: "",
        CustomPageName: "",
        UrlToTest: ""
      }
    };
    return _this;
  }

  testUrl_createClass(TestUrlPanelBody, [{
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;

      var test = testUrl_extends({}, state.test);

      if (key === "PageToTest") {
        test[key] = event;

        if (test[key] === "-1") {
          props.dispatch(seo.clearUrlTestResults());
        }
      } else if (key === "UrlToTest") {
        test[key] = testUrl_typeof(event) === "object" ? event.target.value : event;

        if (test[key] === "") {
          props.dispatch(seo.clearUrlRewritingTestResults());
        }
      } else {
        test[key] = testUrl_typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        test: test
      });
    }
  }, {
    key: "keyValuePairsToOptions",
    value: function keyValuePairsToOptions(keyValuePairs) {
      var options = [];

      if (keyValuePairs !== undefined) {
        options = keyValuePairs.map(function (item) {
          return {
            label: item.Key,
            value: item.Value
          };
        });
      }

      return options;
    }
  }, {
    key: "onTestPage",
    value: function onTestPage() {
      var state = this.state,
          props = this.props;
      props.dispatch(seo.testUrl(state.test.PageToTest, state.test.QueryString, state.test.CustomPageName));
    }
  }, {
    key: "onTestUrl",
    value: function onTestUrl() {
      var state = this.state,
          props = this.props;
      props.dispatch(seo.testUrlRewrite(state.test.UrlToTest));
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var noneSpecifiedText = "<" + resources.get("NoneSpecified") + ">";
      var PageToTestParameters = {
        portalId: -2,
        cultureCode: "",
        isMultiLanguage: false,
        excludeAdminTabs: false,
        disabledNotSelectable: false,
        roles: props.adminRoleId ? props.adminRoleId.toString() : "0",
        sortOrder: 0
      };
      var columnOne =
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "left-column"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("selectPageToTestLabel.Help"),
        label: resources.get("selectPageToTestLabel") + "*"
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PagePicker"], {
        serviceFramework: utils["a" /* default */].utilities.sf,
        style: {
          width: "100%",
          zIndex: 2
        },
        OnSelect: this.onSettingChange.bind(this, "PageToTest"),
        defaultLabel: noneSpecifiedText,
        noneSpecifiedText: noneSpecifiedText,
        CountText: "{0} Results",
        PortalTabsParameters: PageToTestParameters
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("queryStringLabel.Help"),
        label: resources.get("queryStringLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        value: state.test.QueryString,
        onChange: this.onSettingChange.bind(this, "QueryString")
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("pageNameLabel.Help"),
        label: resources.get("pageNameLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        value: state.test.CustomPageName,
        onChange: this.onSettingChange.bind(this, "CustomPageName")
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "buttons-box"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
        disabled: !this.state.test.PageToTest || this.state.test.PageToTest === "-1",
        type: "secondary",
        onClick: this.onTestPage.bind(this)
      }, resources.get("TestUrlButtonCaption"))));
      var columnTwo =
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "right-column"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("resultingUrlsLabel.Help"),
        label: resources.get("resultingUrlsLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInput"], {
        value: props.urls,
        enabled: false
      })));
      var columnThree =
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "left-column"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("testUrlRewritingLabel.Help"),
        label: resources.get("testUrlRewritingLabel") + "*"
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        value: state.test.UrlToTest,
        onChange: this.onSettingChange.bind(this, "UrlToTest")
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "buttons-box"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
        disabled: !this.state.test.UrlToTest,
        type: "secondary",
        onClick: this.onTestUrl.bind(this)
      }, resources.get("testUrlRewritingButton"))));
      var columnFour =
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("div", {
        className: "right-column"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("rewritingResultLabel.Help"),
        label: resources.get("rewritingResultLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.rewritingResult
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("languageLabel.Help"),
        label: resources.get("languageLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.culture
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("identifiedTabLabel.Help"),
        label: resources.get("identifiedTabLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.identifiedPage
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("redirectionResultLabel.Help"),
        label: resources.get("redirectionResultLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.redirectionResult
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("redirectionReasonLabel.Help"),
        label: resources.get("redirectionReasonLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.redirectionReason
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        tooltipMessage: resources.get("operationMessagesLabel.Help"),
        label: resources.get("operationMessagesLabel")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: false,
        enabled: false,
        value: props.operationMessages
      })));
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: testUrl_style_default.a.testUrl
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "columnTitleOne"
        }, resources.get("TestPageUrl")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          numberOfColumns: 2
        }, [columnOne, columnTwo]),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "columnTitleTwo"
        }, resources.get("TestUrlRewriting")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          numberOfColumns: 2
        }, [columnThree, columnFour]))
      );
    }
  }]);

  return TestUrlPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

testUrl_TestUrlPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  urls: prop_types_default.a.string,
  rewritingResult: prop_types_default.a.string,
  culture: prop_types_default.a.string,
  identifiedPage: prop_types_default.a.string,
  redirectionReason: prop_types_default.a.string,
  redirectionResult: prop_types_default.a.string,
  operationMessages: prop_types_default.a.string,
  adminRoleId: prop_types_default.a.number
};

function testUrl_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    urls: state.seo.urls,
    rewritingResult: state.seo.rewritingResult,
    culture: state.seo.culture,
    identifiedPage: state.seo.identifiedPage,
    redirectionReason: state.seo.redirectionReason,
    redirectionResult: state.seo.redirectionResult,
    operationMessages: state.seo.operationMessages,
    adminRoleId: state.seo.adminRoleId
  };
}

/* harmony default export */ var components_testUrl = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(testUrl_mapStateToProps)(testUrl_TestUrlPanelBody));
// EXTERNAL MODULE: ./src/components/body/style.less
var body_style = __webpack_require__(39);

// CONCATENATED MODULE: ./src/components/body/index.jsx
function body_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { body_typeof = function _typeof(obj) { return typeof obj; }; } else { body_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return body_typeof(obj); }

function body_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function body_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function body_createClass(Constructor, protoProps, staticProps) { if (protoProps) body_defineProperties(Constructor.prototype, protoProps); if (staticProps) body_defineProperties(Constructor, staticProps); return Constructor; }

function body_possibleConstructorReturn(self, call) { if (call && (body_typeof(call) === "object" || typeof call === "function")) { return call; } return body_assertThisInitialized(self); }

function body_getPrototypeOf(o) { body_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return body_getPrototypeOf(o); }

function body_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function body_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) body_setPrototypeOf(subClass, superClass); }

function body_setPrototypeOf(o, p) { body_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return body_setPrototypeOf(o, p); }














var isHost = false;
var body_Body =
/*#__PURE__*/
function (_Component) {
  body_inherits(Body, _Component);

  function Body() {
    var _this;

    body_classCallCheck(this, Body);

    _this = body_possibleConstructorReturn(this, body_getPrototypeOf(Body).call(this));
    _this.handleSelect = _this.handleSelect.bind(body_assertThisInitialized(_this));
    isHost = utils["a" /* default */].settings.isHost;
    return _this;
  }

  body_createClass(Body, [{
    key: "handleSelect",
    value: function handleSelect(index) {
      var props = this.props;
      props.dispatch(pagination.loadTab(index)); //index acts as scopeTypeId
    }
  }, {
    key: "renderTabs",
    value: function renderTabs() {
      if (isHost) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
            onSelect: this.handleSelect.bind(this),
            tabHeaders: [resources.get("URLManagementTab"), resources.get("SitemapSettingsTab")],
            type: "primary"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
            onSelect: this.handleSelect.bind(this),
            tabHeaders: [resources.get("GeneralSettingsTab"), resources.get("ExtensionUrlProvidersTab"),
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("div", {
              style: {
                fontSize: "9pt",
                paddingRight: 25
              },
              key: "first"
            }, resources.get("ExpressionsTab"),
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
              messages: [resources.get("GlobalSetting")],
              type: "global",
              style: {
                position: "absolute",
                right: 0,
                top: 15,
                float: "right",
                textTransform: "none"
              }
            })), resources.get("TestURLTab")],
            type: "secondary"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_generalSettings, null),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(extensionUrlProviders, null),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_regexSettings, null),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_testUrl, null)),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_sitemapSettings, null))
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
            onSelect: this.handleSelect.bind(this),
            tabHeaders: [resources.get("URLManagementTab"), resources.get("SitemapSettingsTab")],
            type: "primary"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
            onSelect: this.handleSelect.bind(this),
            tabHeaders: [resources.get("GeneralSettingsTab"), resources.get("ExtensionUrlProvidersTab"), resources.get("TestURLTab")],
            type: "secondary"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_generalSettings, null),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(extensionUrlProviders, null),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_testUrl, null)),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_sitemapSettings, null))
        );
      }
    }
    /*eslint no-mixed-spaces-and-tabs: "error"*/

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], null, this.renderTabs())
      );
    }
  }]);

  return Body;
}(external_window_dnn_nodeModules_React_["Component"]);
body_Body.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number
};

function body_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.index
  };
}

/* harmony default export */ var body = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(body_mapStateToProps)(body_Body));
// CONCATENATED MODULE: ./src/components/App.jsx
function App_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { App_typeof = function _typeof(obj) { return typeof obj; }; } else { App_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return App_typeof(obj); }

function App_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function App_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function App_createClass(Constructor, protoProps, staticProps) { if (protoProps) App_defineProperties(Constructor.prototype, protoProps); if (staticProps) App_defineProperties(Constructor, staticProps); return Constructor; }

function App_possibleConstructorReturn(self, call) { if (call && (App_typeof(call) === "object" || typeof call === "function")) { return call; } return App_assertThisInitialized(self); }

function App_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function App_getPrototypeOf(o) { App_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return App_getPrototypeOf(o); }

function App_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) App_setPrototypeOf(subClass, superClass); }

function App_setPrototypeOf(o, p) { App_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return App_setPrototypeOf(o, p); }








var App_App =
/*#__PURE__*/
function (_Component) {
  App_inherits(App, _Component);

  function App() {
    App_classCallCheck(this, App);

    return App_possibleConstructorReturn(this, App_getPrototypeOf(App).call(this));
  }

  App_createClass(App, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "seo-app"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 0
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: resources.get("nav_Seo")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(body, null)))
      );
    }
  }]);

  return App;
}(external_window_dnn_nodeModules_React_["Component"]);

App_App.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  selectedPage: prop_types_default.a.number
};

function App_mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex
  };
}

/* harmony default export */ var components_App = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(App_mapStateToProps)(App_App));
// CONCATENATED MODULE: ./src/containers/Root.prod.js
function Root_prod_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Root_prod_typeof = function _typeof(obj) { return typeof obj; }; } else { Root_prod_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Root_prod_typeof(obj); }

function Root_prod_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Root_prod_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Root_prod_createClass(Constructor, protoProps, staticProps) { if (protoProps) Root_prod_defineProperties(Constructor.prototype, protoProps); if (staticProps) Root_prod_defineProperties(Constructor, staticProps); return Constructor; }

function Root_prod_possibleConstructorReturn(self, call) { if (call && (Root_prod_typeof(call) === "object" || typeof call === "function")) { return call; } return Root_prod_assertThisInitialized(self); }

function Root_prod_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Root_prod_getPrototypeOf(o) { Root_prod_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Root_prod_getPrototypeOf(o); }

function Root_prod_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Root_prod_setPrototypeOf(subClass, superClass); }

function Root_prod_setPrototypeOf(o, p) { Root_prod_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Root_prod_setPrototypeOf(o, p); }




var Root_prod_Root =
/*#__PURE__*/
function (_Component) {
  Root_prod_inherits(Root, _Component);

  function Root() {
    Root_prod_classCallCheck(this, Root);

    return Root_prod_possibleConstructorReturn(this, Root_prod_getPrototypeOf(Root).call(this));
  }

  Root_prod_createClass(Root, [{
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "siteInfo-Root"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_App, null))
      );
    }
  }]);

  return Root;
}(external_window_dnn_nodeModules_React_["Component"]);

/* harmony default export */ var Root_prod = __webpack_exports__["default"] = (Root_prod_Root);

/***/ }),
/* 42 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactDOM"
var external_window_dnn_nodeModules_ReactDOM_ = __webpack_require__(17);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(5);

// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/globals/application.js

var boilerPlate = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;        
    var options = window.dnn.initSeo();
    utils["a" /* default */].init(options.utility);
    utils["a" /* default */].moduleName = options.moduleName;
    utils["a" /* default */].settings = options.settings; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(23);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
/* harmony default export */ var application = (boilerPlate);
// EXTERNAL MODULE: external "window.dnn.nodeModules.Redux"
var external_window_dnn_nodeModules_Redux_ = __webpack_require__(8);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxThunk"
var external_window_dnn_nodeModules_ReduxThunk_ = __webpack_require__(16);
var external_window_dnn_nodeModules_ReduxThunk_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxThunk_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxImmutableStateInvariant"
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_ = __webpack_require__(18);
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxImmutableStateInvariant_);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 3 modules
var actionTypes = __webpack_require__(3);

// CONCATENATED MODULE: ./src/reducers/paginationReducer.js
function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["a" /* pagination */].LOAD_TAB_DATA:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        tabIndex: action.payload.index
      });

    default:
      return _objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/visiblePanelReducer.js
function visiblePanelReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { visiblePanelReducer_defineProperty(target, key, source[key]); }); } return target; }

function visiblePanelReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0,
    selectedPageVisibleIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["c" /* visiblePanel */].SELECT_PANEL:
      return visiblePanelReducer_objectSpread2(visiblePanelReducer_objectSpread2({}, state), {}, {
        selectedPage: action.payload.selectedPage,
        selectedPageVisibleIndex: action.payload.selectedPageVisibleIndex
      });

    default:
      return visiblePanelReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/seoReducer.js
function seoReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { seoReducer_defineProperty(target, key, source[key]); }); } return target; }

function seoReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function seoSettings() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["b" /* seo */].RETRIEVED_SEO_GENERAL_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        generalSettings: action.data.generalSettings,
        replacementCharacterList: action.data.replacementCharacterList,
        deletedPageHandlingTypes: action.data.deletedPageHandlingTypes,
        clientModified: action.data.clientModified,
        adminRoleId: action.data.adminRoleId
      });

    case actionTypes["b" /* seo */].SEO_GENERAL_SETTINS_CLIENT_MODIFIED:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        generalSettings: action.data.generalSettings,
        clientModified: action.data.clientModified
      });

    case actionTypes["b" /* seo */].UPDATED_SEO_GENERAL_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        clientModified: action.data.clientModified
      });

    case actionTypes["b" /* seo */].RETRIEVED_SEO_REGEX_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        regexSettings: action.data.regexSettings,
        regexClientModified: action.data.regexClientModified
      });

    case actionTypes["b" /* seo */].SEO_REGEX_SETTINS_CLIENT_MODIFIED:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        regexSettings: action.data.regexSettings,
        regexClientModified: action.data.regexClientModified
      });

    case actionTypes["b" /* seo */].UPDATED_SEO_REGEX_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        regexClientModified: action.data.regexClientModified
      });

    case actionTypes["b" /* seo */].TESTED_SEO_PAGE_URL:
    case actionTypes["b" /* seo */].CLEARED_SEO_TEST_PAGE_URL_RESULTS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        urls: action.data.urls
      });

    case actionTypes["b" /* seo */].TESTED_SEO_URL_REWRITING:
    case actionTypes["b" /* seo */].CLEARED_SEO_TEST_URL_REWRITING_RESULTS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        rewritingResult: action.data.rewritingResult,
        culture: action.data.culture,
        identifiedPage: action.data.identifiedPage,
        redirectionReason: action.data.redirectionReason,
        redirectionResult: action.data.redirectionResult,
        operationMessages: action.data.operationMessages
      });

    case actionTypes["b" /* seo */].RETRIEVED_SEO_SITEMAP_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        sitemapSettings: action.data.sitemapSettings,
        searchEngineUrls: action.data.searchEngineUrls,
        clientModified: action.data.clientModified
      });

    case actionTypes["b" /* seo */].UPDATED_SEO_SITEMAP_SETTINGS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        clientModified: action.data.clientModified
      });

    case actionTypes["b" /* seo */].SEO_SITEMAP_SETTINS_CLIENT_MODIFIED:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        sitemapSettings: action.data.sitemapSettings,
        clientModified: action.data.clientModified
      });

    case actionTypes["b" /* seo */].RETRIEVED_SEO_SITEMAP_PROVIDERS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        sitemapProviders: action.data.providers
      });

    case actionTypes["b" /* seo */].RETRIEVED_SEO_EXTENSION_URL_PROVIDERS:
      return seoReducer_objectSpread2(seoReducer_objectSpread2({}, state), {}, {
        extensionUrlProviders: action.data.providers
      });

    default:
      return seoReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/rootReducer.js




var rootReducer = Object(external_window_dnn_nodeModules_Redux_["combineReducers"])({
  pagination: pagination,
  visiblePanel: visiblePanel,
  seo: seoSettings
});
/* harmony default export */ var reducers_rootReducer = (rootReducer);
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevTools"
var external_window_dnn_nodeModules_ReduxDevTools_ = __webpack_require__(19);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsLogMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_ = __webpack_require__(20);
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsDockMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_ = __webpack_require__(21);
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_);

// CONCATENATED MODULE: ./src/containers/DevTools.js




/* harmony default export */ var DevTools = (Object(external_window_dnn_nodeModules_ReduxDevTools_["createDevTools"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_default.a, {
  toggleVisibilityKey: "ctrl-h",
  changePositionKey: "ctrl-q"
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default.a, null))));
// CONCATENATED MODULE: ./src/store/configureStore.js





/* eslint-disable no-undef */

var IS_PRODUCTION = "production" === "production";
function configureStore(initialState) {
  var store = Object(external_window_dnn_nodeModules_Redux_["createStore"])(reducers_rootReducer, initialState, Object(external_window_dnn_nodeModules_Redux_["compose"])(IS_PRODUCTION ? Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a) : Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a, external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default()()), DevTools.instrument()));
  return store;
}
// EXTERNAL MODULE: ./src/containers/Root.js
var Root = __webpack_require__(22);
var Root_default = /*#__PURE__*/__webpack_require__.n(Root);

// CONCATENATED MODULE: ./src/main.jsx






var main_store = configureStore();
application.dispatch = main_store.dispatch;
application.init();
var appContainer = document.getElementById("seo-container");
Object(external_window_dnn_nodeModules_ReactDOM_["render"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactRedux_["Provider"], {
  store: main_store
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(Root_default.a, null)), appContainer);

/***/ })
/******/ ]);
//# sourceMappingURL=seo-bundle.js.map