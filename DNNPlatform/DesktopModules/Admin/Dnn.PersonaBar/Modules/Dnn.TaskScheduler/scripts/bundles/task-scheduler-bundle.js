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
/******/ 	return __webpack_require__(__webpack_require__.s = 53);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
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
  module.exports = __webpack_require__(25)();
}


/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

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
// CONCATENATED MODULE: ./src/constants/actionTypes/task.js
var taskActionTypes = {
  RETRIEVED_TASK_LIST: "RETRIEVED_TASK_LIST",
  RETRIEVED_TASK_STATUS_LIST: "RETRIEVED_TASK_STATUS_LIST",
  STARTED_SCHEDULE: "STARTED_SCHEDULE",
  STOPPED_SCHEDULE: "STOPPED_SCHEDULE",
  RETRIEVED_SCHEDULE_SETTINGS: "RETRIEVED_SCHEDULE_SETTINGS",
  UPDATED_SCHEDULE_SETTINGS: "UPDATED_SCHEDULE_SETTINGS",
  RETRIEVED_SCHEDULE_HISTORY: "RETRIEVED_SCHEDULE_HISTORY",
  RETRIEVED_SCHEDULE_ITEMS: "RETRIEVED_SCHEDULE_ITEMS",
  RETRIEVED_SERVER_LIST: "RETRIEVED_SERVER_LIST",
  RETRIEVED_SCHEDULE_ITEM: "RETRIEVED_SCHEDULE_ITEM",
  DELETED_SCHEDULE_ITEM: "DELETED_SCHEDULE_ITEM",
  CREATED_SCHEDULE_ITEM: "CREATED_SCHEDULE_ITEM",
  UPDATED_SCHEDULE_ITEM: "UPDATED_SCHEDULE_ITEM",
  EXECUTED_SCHEDULE_ITEM: "EXECUTED_SCHEDULE_ITEM",
  SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED: "SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED",
  CANCELED_SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED: "CANCELED_SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED",
  UPDATED_SCHEDULE_SETTINGS_PENDING: "UPDATED_SCHEDULE_SETTINGS_PENDING"
};
/* harmony default export */ var task = (taskActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/index.js
/* concated harmony reexport pagination */__webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* concated harmony reexport visiblePanel */__webpack_require__.d(__webpack_exports__, "c", function() { return visiblePanel; });
/* concated harmony reexport task */__webpack_require__.d(__webpack_exports__, "b", function() { return task; });





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
  formatString: function formatString() {
    var format = arguments[0];
    var methodsArgs = arguments;
    return format.replace(/[{[](\d+)[\]}]/gi, function (_, index) {
      var argsIndex = parseInt(index) + 1;
      return methodsArgs[argsIndex];
    });
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

var	fixUrls = __webpack_require__(24);

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
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<polygon points=\"1110.8,531.7 991.8,531.7 991.8,1015.4 653.9,1015.4 653.9,1134.4 1110.8,1134.4 \t\"/>\r\n\t<path d=\"M1796.5,1021.5v-0.8C1796,594.4,1450.3,249,1024,249c-426.6,0-772.5,345.8-772.5,772.5s345.8,772.5,772.5,772.5V1625\r\n\t\tc-333.8,0-604.4-270.6-604.4-604.4S690.2,416.3,1024,416.3s604.4,270.6,604.4,604.4v0.8h-188.9l271.3,272.4l271.3-272.4H1796.5z\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 12 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 13 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 14 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 15 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(52);
} else {}

/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(27);

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
/* 18 */
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
/* 19 */
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
/* 20 */
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
/* 21 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(46);

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
/* 22 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(23);

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
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 24 */
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
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(26);

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
/* 26 */
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
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3LcpqusY9n2NIHI3DrlWLC {\n  border: solid 1px #C8C8C8;\n  margin: 15px 15px 0px 15px;\n  float: left;\n}\n._3LcpqusY9n2NIHI3DrlWLC:not(:first-child) {\n  margin-top: 15px;\n}\n._3LcpqusY9n2NIHI3DrlWLC:last-child {\n  margin-bottom: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"taskStatusItemRow": "_3LcpqusY9n2NIHI3DrlWLC"
};

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2487L2qUmfPM0eKOpmpJFy {\n  padding: 5px 15px 5px 15px;\n  display: block;\n  box-sizing: border-box;\n  float: left;\n  width: 100%;\n  height: auto;\n  border-right: 1px solid #C8C8C8;\n  margin: 10px 0 10px 0;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskIcon {\n  width: 35px;\n  height: 35px;\n  float: left;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskIcon > svg {\n  fill: #959695;\n}\n@-ms-keyframes spin {\n  from {\n    -ms-transform: rotate(360deg);\n  }\n  to {\n    -ms-transform: rotate(0deg);\n  }\n}\n@-moz-keyframes spin {\n  from {\n    -moz-transform: rotate(360deg);\n  }\n  to {\n    -moz-transform: rotate(0deg);\n  }\n}\n@-webkit-keyframes spin {\n  from {\n    -webkit-transform: rotate(360deg);\n  }\n  to {\n    -webkit-transform: rotate(0deg);\n  }\n}\n@keyframes spin {\n  from {\n    transform: rotate(360deg);\n  }\n  to {\n    transform: rotate(0deg);\n  }\n}\n._2487L2qUmfPM0eKOpmpJFy .taskIconProcessing {\n  width: 35px;\n  height: 35px;\n  float: left;\n  -webkit-animation-name: spin;\n  -webkit-animation-duration: 4000ms;\n  -webkit-animation-iteration-count: infinite;\n  -webkit-animation-timing-function: linear;\n  -moz-animation-name: spin;\n  -moz-animation-duration: 4000ms;\n  -moz-animation-iteration-count: infinite;\n  -moz-animation-timing-function: linear;\n  -ms-animation-name: spin;\n  -ms-animation-duration: 4000ms;\n  -ms-animation-iteration-count: infinite;\n  -ms-animation-timing-function: linear;\n  animation-name: spin;\n  animation-duration: 4000ms;\n  animation-iteration-count: infinite;\n  animation-timing-function: linear;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskIconProcessing > svg {\n  fill: #1E88C3;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail {\n  padding: 10px 0 0 45px;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail > div {\n  padding: 0 0 10px 0;\n  float: left;\n  width: 100%;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail .taskDetail-name {\n  font-weight: bolder;\n  font-size: 15px;\n  white-space: pre-wrap;\n  white-space: -moz-pre-wrap;\n  white-space: -pre-wrap;\n  white-space: -o-pre-wrap;\n  word-wrap: break-word;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail .taskDetail-common {\n  color: #4B4E4F;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail .taskDetail-common .taskDetail-common-title {\n  font-weight: bolder;\n  float: left;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail .taskDetail-common .taskDetail-common-value {\n  float: right;\n}\n._2487L2qUmfPM0eKOpmpJFy .taskDetail .taskDetail-common .taskDetail-common-title-processing {\n  font-weight: bolder;\n  float: left;\n  color: #1E88C3;\n}\n", ""]);

// Exports
exports.locals = {
	"taskStatusItemLeftPane": "_2487L2qUmfPM0eKOpmpJFy"
};

/***/ }),
/* 29 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<path d=\"M1024,254.5c-424.9,0-769.3,344.4-769.3,769.3S599.1,1793,1024,1793s769.3-344.4,769.3-769.3S1448.9,254.5,1024,254.5z\r\n\t\t M1024,1625.5c-332.3,0-601.7-269.4-601.7-601.7S691.7,422,1024,422c332.3,0,601.7,269.4,601.7,601.7S1356.3,1625.5,1024,1625.5z\"\r\n\t\t/>\r\n\t<polygon points=\"1063,584.9 947,584.9 947,1027.7 947,1141.4 947,1143.7 1358.4,1143.7 1358.4,1027.7 1063,1027.7 \t\"/>\r\n\t<rect x=\"511.1\" y=\"59.4\" width=\"1024.9\" height=\"120\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<g>\r\n\t\t<path d=\"M1709.7,748.2l-271.3,272.4h187c-0.1,333.8-269.6,604.3-602,604.3c-227.3,0-425.1-126.5-527.6-313.2l-120.3,120.8\r\n\t\t\tc136.3,216.6,376.8,360.5,650.9,360.5c424.9,0,769.3-345.8,769.3-772.4H1981L1709.7,748.2z\"/>\r\n\t\t<path d=\"M424.6,1021.4c0.1-333.8,269.6-604.3,602-604.3c227.3,0,425.1,126.5,527.6,313.2l120.3-120.8\r\n\t\t\tC1538.1,392.9,1297.6,249,1023.6,249c-424.9,0-769.3,345.8-769.3,772.4H69l271.3,272.4l271.3-272.4H424.6z\"/>\r\n\t</g>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2G3hZRqANZWVNy5sAxwcHr {\n  padding: 5px 15px 15px 15px;\n  display: block;\n  box-sizing: border-box;\n  float: left;\n  width: 100%;\n  height: auto;\n  margin-top: 0px;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight {\n  float: left;\n  width: 100%;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight > div {\n  padding: 0 0 10px 0;\n  float: left;\n  width: 100%;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-idWrapper {\n  float: right;\n  padding-bottom: 5px;\n  color: #4B4E4F;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-idWrapper .taskDetailRight-id {\n  font-weight: bolder;\n  font-size: 15px;\n  float: left;\n  padding-left: 85%;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-idWrapper .taskDetailRight-id-value {\n  float: right;\n  padding-top: 2px;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-common {\n  padding: 0 0 10px 0;\n  color: #4B4E4F;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-common .taskDetailRight-common-title {\n  font-weight: bolder;\n  float: left;\n}\n._2G3hZRqANZWVNy5sAxwcHr .taskDetailRight .taskDetailRight-common .taskDetailRight-common-value {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"taskStatusItemRightPane": "_2G3hZRqANZWVNy5sAxwcHr"
};

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

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#scheduler-container .dnn-persona-bar-page-body .react-tabs [hidden] {\n  display: none;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .noData {\n  display: block;\n  text-align: center;\n  float: left;\n  width: 100%;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .noData .noTasks {\n  font-size: x-large;\n  color: #4B4E4F;\n  margin-left: 40px;\n  margin-right: 40px;\n  margin-top: 60px;\n  left: 0;\n  right: 0;\n  position: absolute;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .noData .noTasksMessage {\n  font-size: small;\n  color: #C8C8C8;\n  margin-left: 40px;\n  margin-right: 40px;\n  margin-top: 110px;\n  left: 0;\n  right: 0;\n  position: absolute;\n}\n", ""]);



/***/ }),
/* 34 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 526.08 398.98\"><defs><style>.a{fill:#ccc;}.b,.c{fill:#b3b3b3;}.c{stroke:#b3b3b3;stroke-miterlimit:10;}</style></defs><title>revised no data states</title><path class=\"a\" d=\"M307,273.35l-10.9,2.28c-1.56.48-3.2.92-4.9,1.28-1.92.41-3.78.7-5.57.91h0a19.32,19.32,0,0,1-10.71,2.94c-5.52,0-10.31-1.9-12.73-4.69-2.42,2.79-7.21,4.69-12.73,4.69-6.74,0-12.38-2.83-13.95-6.65-1.83,3.58-7.29,6.17-13.74,6.17s-11.82-2.55-13.69-6.08c-17.83,1.3-29.1,3.21-29.1,5.33,0,3.92,38.36,7.1,85.68,7.1s85.68-3.18,85.68-7.1C350.29,276.87,332.84,274.57,307,273.35Z\"/><path class=\"b\" d=\"M239.28,240.75c.46,3.49,6.6,5.51,13.72,4.5s12.51-4.65,12.05-8.14-6.6-5.51-13.72-4.5S238.82,237.26,239.28,240.75Z\"/><path class=\"b\" d=\"M217.34,209c5.69.69,10.41,3,12.59,5.72.06-4.65-5.64-9-13.21-9.94-8-1-15,2.23-15.66,7.14l1.77.22C205.5,209.57,211.12,208.23,217.34,209Z\"/><path class=\"b\" d=\"M235,265c13.61,1.53,29-3.89,34.45-12.1,2.65-4,2.4-8-.15-11.12a8.21,8.21,0,0,1-1,3.87c-4.32,8.13-20.68,14.47-36.54,14.16-4.5-.09-15.44,1-18.73,0\"/><path class=\"b\" d=\"M189.39,243.74c1.43.46,8.73-2.65,8.73-2.65l-2.13-28c-3.37.26-3.75-3.73-10.48-3.77s-12.15,4.59-12.15,4.59l3.61,8.89,3.61,8.89,3.41,7.21\"/><path class=\"b\" d=\"M223.78,230.58c-4.61,1.29-9,.8-11.2-1a3,3,0,0,0-.38,2.4c.74,2.36,4.86,3.28,9.21,2.07,3.59-1,6.2-3.16,6.59-5.21A18.8,18.8,0,0,1,223.78,230.58Z\"/><path class=\"b\" d=\"M236.89,228.26c-2.36.66-4.62.41-5.74-.52A1.53,1.53,0,0,0,231,229c.38,1.21,2.49,1.68,4.72,1.06,1.84-.52,3.18-1.62,3.38-2.67A9.63,9.63,0,0,1,236.89,228.26Z\"/><path class=\"b\" d=\"M238.31,206.67c-6.73,0-7.11,4-10.48,3.77l-.19,2.52a29.8,29.8,0,0,1,5.16,2.75,26.48,26.48,0,0,1,10.53,13.13l3.52-8.68,3.61-8.89S245,206.63,238.31,206.67Z\"/><path class=\"b\" d=\"M206,267.91a9.29,9.29,0,0,1,2.41-2.62l.48,4.62a4.86,4.86,0,0,0,.26,2.46v.09h0c.85,2.2,3.21,4.17,6.59,5.2,5.57,1.69,11.81.15,13.94-3.43s-.67-7.84-6.24-9.53a16.14,16.14,0,0,0-4.38-.68l-.19-1.82a22.44,22.44,0,0,1,6.47.94c7.75,2.34,11.63,8.27,8.68,13.25s-11.63,7.11-19.38,4.76S203.07,272.88,206,267.91Z\"/><path class=\"b\" d=\"M205.6,255c-7.79-10.21-8.45-15.37-6.65-7.18-1.79,3.08.12,8.14,4.5,11.61,4.6,3.65,12,3.53,14.27.51l-.57-.44C213.86,260.64,208.62,259,205.6,255Z\"/><path class=\"b\" d=\"M261.43,267.91a9.29,9.29,0,0,0-2.41-2.62l-.48,4.62a4.86,4.86,0,0,1-.26,2.46v.09h0c-.85,2.2-3.21,4.17-6.59,5.2-5.57,1.69-11.81.15-13.94-3.43s.67-7.84,6.24-9.53a16.14,16.14,0,0,1,4.38-.68l.19-1.82a22.44,22.44,0,0,0-6.47.94c-7.75,2.34-11.63,8.27-8.68,13.25s11.63,7.11,19.38,4.76S264.39,272.88,261.43,267.91Z\"/><path class=\"b\" d=\"M207.08,267.13c-1.33-.14-6.33-2.7-6.58-5-.2-1.92-4.43-10.68-3.55-2.29.26,3,2.92,6.88,6.16,9a5.05,5.05,0,0,0,5,.65C207.78,268.67,207.42,267.89,207.08,267.13Z\"/><path class=\"b\" d=\"M289.84,225.28c13.48,1.72,25,4.87,30.58,8.08-.69-4.75-14.83-10.22-32.76-12.51-18.85-2.4-34.74-.37-35.49,4.54l4.19.53C262.14,223.78,275.09,223.41,289.84,225.28Z\"/><path class=\"b\" d=\"M266.82,277.41c-1.83-2.2,2.25-5.76,9.13-8a30.78,30.78,0,0,1,11.54-1.57,11.48,11.48,0,0,1,5.23-6.31c-5.8-3.61-9-8.48-7.92-13.31s6.45-8.75,13.77-10.52l5.62-.92a47.21,47.21,0,0,1,16.16,1.42,30,30,0,0,0-3.69-1.78,42.7,42.7,0,0,0-7.81-1.67A38.89,38.89,0,0,0,287.4,238c-3.51,2.17-6,5.11-6.72,8.57-.8,3.63.4,7.23,3,10.34a13.15,13.15,0,0,0-2.95,5.81,12.29,12.29,0,0,0-.28,3,28.64,28.64,0,0,0-13,1.62c-7.66,2.81-11.25,7.89-8,11.35s19.15,3,26.81.15c2.66-1,19.44-4.8,21-6.16-.94.41-18.07,3.48-19.16,3.83C281.3,278.7,268.65,279.61,266.82,277.41Z\"/><path class=\"b\" d=\"M322.81,234.58l-1-1.21c-.89,1.53-.37,3.5.41,5.35a81.5,81.5,0,0,0,1.62,9.41c-.18,3.43-.59,7.69-1.63,10-1.94,4.34-11.29,14.19-14.9,14.52,4.82,1.87,12.35-5.88,16-10.52a28,28,0,0,0,3-5.89,2.9,2.9,0,0,0,.78,1.18c3,3.06,1.92,6.31-5.2,12.49a41.13,41.13,0,0,1-13,7.67,4.69,4.69,0,0,0,1.36,1c5,2.37,19.38-2,23.48-8.27C343,256,322.75,234.46,322.81,234.58Z\"/><path class=\"b\" d=\"M286.38,222.91l-13.56-1.53-10.4,2.1c.12.21.24.42.38.62,2.11,3.1,6.91,5.88,12.81,7,8.52,1.68,16.11-.63,17-5.17a4.57,4.57,0,0,0,.06-1.07Z\"/><path class=\"c\" d=\"M238.44,170.34a20.36,20.36,0,0,1,5.19,3.09c.09-.81-2.23-2.52-5.38-3.94-3.32-1.49-6.3-2.08-6.66-1.33l.74.33A18.55,18.55,0,0,1,238.44,170.34Z\"/><path class=\"c\" d=\"M235.19,179a20.38,20.38,0,0,1,4.67,2.47c0-.63-2.09-2-4.9-3.13s-5.55-1.69-5.81-1.11l.66.27A18.52,18.52,0,0,1,235.19,179Z\"/><path class=\"c\" d=\"M235.83,175.87c-5.77,1.51-4.92.68-6,1.13.18-.36,3-1.3,6.47-2.15,3.68-.9,6.74-1.37,6.84-1l-.82.2A49.39,49.39,0,0,1,235.83,175.87Z\"/><path class=\"c\" d=\"M266,167.76a23.37,23.37,0,0,1,6,3.55c.11-.93-2.56-2.9-6.18-4.52-3.81-1.71-7.23-2.39-7.65-1.53l.85.38A21.3,21.3,0,0,1,266,167.76Z\"/><path class=\"c\" d=\"M262.29,177.71a23.41,23.41,0,0,1,5.36,2.83c0-.72-2.4-2.28-5.62-3.59-3.39-1.37-6.38-1.94-6.67-1.27l.75.31A21.27,21.27,0,0,1,262.29,177.71Z\"/><path class=\"c\" d=\"M263,174.11c-6.62,1.74-5.65.78-6.94,1.29.2-.42,3.41-1.49,7.43-2.47,4.22-1,7.74-1.57,7.85-1.2l-.94.23A56.71,56.71,0,0,1,263,174.11Z\"/><path class=\"c\" d=\"M294.86,165.62a26.81,26.81,0,0,1,6.83,4.07c.12-1.07-2.93-3.32-7.09-5.19-4.37-2-8.3-2.74-8.78-1.75l1,.44C288.31,163.18,291.44,164.09,294.86,165.62Z\"/><path class=\"c\" d=\"M290.57,177a26.85,26.85,0,0,1,6.15,3.25c0-.83-2.75-2.62-6.45-4.12-3.89-1.57-7.31-2.23-7.65-1.46l.86.35A24.4,24.4,0,0,1,290.57,177Z\"/><path class=\"c\" d=\"M291.42,172.91c-7.59,2-6.48.9-8,1.48.24-.48,3.91-1.71,8.52-2.83,4.84-1.18,8.88-1.8,9-1.38l-1.08.26A65.06,65.06,0,0,1,291.42,172.91Z\"/></svg>");

/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1a-b72OjdN0lwBHDJLU64_ {\n  float: left;\n  width: 100%;\n  color: #4B4E4F;\n  padding: 10px 0 10px 0;\n}\n._1a-b72OjdN0lwBHDJLU64_:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1a-b72OjdN0lwBHDJLU64_ .term-label-logNotes {\n  margin: 0 5px 0 15px;\n  float: left;\n  width: 30%;\n}\n._1a-b72OjdN0lwBHDJLU64_ .term-label-server {\n  margin: 0 5px 0 5px;\n  float: left;\n  width: 15%;\n  word-wrap: break-word;\n}\n._1a-b72OjdN0lwBHDJLU64_ .term-label-elapsedTime {\n  text-align: center;\n  margin: 0 5px 0 5px;\n  width: 15%;\n  float: left;\n}\n._1a-b72OjdN0lwBHDJLU64_ .term-label-succeeded {\n  text-align: center;\n  margin: 0 5px 0 5px;\n  width: 12%;\n  float: left;\n}\n._1a-b72OjdN0lwBHDJLU64_ .term-label-startEnd {\n  margin: 0 0 0 5px;\n  float: left;\n}\n._1a-b72OjdN0lwBHDJLU64_ .checkMarkIcon {\n  width: 16px;\n  height: 16px;\n  margin-left: auto;\n  margin-right: auto;\n}\n._1a-b72OjdN0lwBHDJLU64_ .checkMarkIcon > svg {\n  fill: #4B4E4F;\n}\n", ""]);

// Exports
exports.locals = {
	"taskHistoryItemRow": "_1a-b72OjdN0lwBHDJLU64_"
};

/***/ }),
/* 36 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<polygon points=\"1793.7,585.8 1623.9,416.1 756.6,1283.4 424.4,951.3 254.6,1121 756.6,1623 926.4,1453.2 926.4,1453.2 \"/>\r\n</svg>\r\n");

/***/ }),
/* 37 */
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
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .historyIcon {\n  width: 18px;\n  height: 18px;\n  float: left;\n  margin: 19px 10px 10px 15px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .historyIcon > svg {\n  fill: #4B4E4F;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-title {\n  padding: 19px 0 10px 0;\n  margin: 0 15px 0 15px;\n  border-bottom: solid 1px #C8C8C8;\n  font-weight: bolder;\n  color: #4B4E4F;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-pager {\n  width: 100%;\n  padding: 0 15px;\n  margin-bottom: 15px;\n  box-sizing: border-box;\n  float: left;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-pager * {\n  box-sizing: border-box;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid {\n  border: solid 1px #C8C8C8;\n  margin: 15px;\n  float: left;\n  width: 96%;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-LogNotes {\n  width: 30%;\n  float: left;\n  margin-left: 15px;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-Server {\n  width: 15%;\n  float: left;\n  font-weight: bolder;\n  margin-left: 10px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-ElapsedTime {\n  text-align: center;\n  width: 15%;\n  float: left;\n  font-weight: bolder;\n  margin-left: 10px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-Succeeded {\n  text-align: center;\n  width: 12%;\n  float: left;\n  font-weight: bolder;\n  margin-left: 10px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .taskHistoryList-grid .historyHeader-StartEnd {\n  float: left;\n  font-weight: bolder;\n  margin-left: 10px;\n}\n", ""]);



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
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-component-scheduler {\n  display: block;\n  float: left;\n  width: 100%;\n  cursor: auto;\n}\n.collapsible-component-scheduler:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 10px 0;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row {\n  float: left;\n  width: 100%;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-name {\n  width: 28%;\n  float: left;\n  padding-left: 15px;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-frequency {\n  width: 17%;\n  float: left;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-retryTimeLapse {\n  width: 17%;\n  float: left;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-nextStart {\n  width: 19%;\n  float: left;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-enabled {\n  width: 10%;\n  float: left;\n  text-align: center;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-enabled .checkMarkIcon {\n  width: 16px;\n  height: 16px;\n  margin-left: auto;\n  margin-right: auto;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-enabled .checkMarkIcon > svg {\n  fill: #4B4E4F;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton {\n  width: 3%;\n  float: right;\n  margin-right: 5px;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton .edit-icon {\n  cursor: pointer;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton .edit-icon > svg {\n  width: 22px;\n  float: left;\n  height: 22px;\n  margin-top: -2px;\n  fill: #C8C8C8;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton .edit-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton .edit-icon-active {\n  cursor: pointer;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-editButton .edit-icon-active > svg {\n  width: 22px;\n  float: left;\n  height: 22px;\n  margin-top: -2px;\n  fill: #1E88C3;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton {\n  width: 3%;\n  float: right;\n  margin-right: 10px;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .collapsible-content {\n  -webkit-box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  -moz-box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  z-index: 1000;\n  background-color: #FFFFFF;\n  position: absolute;\n  width: 810px;\n  margin: -114px 0 0 -764px;\n  text-align: left;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .history-icon {\n  cursor: pointer;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .history-icon > svg {\n  width: 22px;\n  float: left;\n  height: 22px;\n  margin-top: -2px;\n  fill: #C8C8C8;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .history-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .history-icon-active {\n  cursor: pointer;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-historyButton .history-icon-active > svg {\n  width: 22px;\n  float: left;\n  height: 22px;\n  margin-top: -2px;\n  fill: #1E88C3;\n}\n.collapsible-component-scheduler div.collapsible-header-scheduler .row .item-row-wrapper {\n  padding: 0 5px 0 5px;\n}\n", ""]);



/***/ }),
/* 41 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<polygon points=\"1793.7,585.8 1623.9,416.1 756.6,1283.4 424.4,951.3 254.6,1121 756.6,1623 926.4,1453.2 926.4,1453.2 \"/>\r\n</svg>\r\n");

/***/ }),
/* 42 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(43);

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
/* 43 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.scheduler-setting-editor {\n  margin: 10px 0;\n}\n.scheduler-setting-editor .scheduler-item-container {\n  display: flex;\n}\n.scheduler-setting-editor .scheduler-item-container .scheduler-item-column {\n  flex: 0 1 50%;\n}\n.scheduler-setting-editor .container {\n  padding: 2% 8%;\n}\n.scheduler-setting-editor .container.right-column {\n  border-left: 1px solid #C8C8C8;\n}\n.scheduler-setting-editor .container .title-row {\n  width: 100%;\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  margin-bottom: 10px;\n}\n.scheduler-setting-editor .container .status-row {\n  width: 100%;\n  float: left;\n  margin-bottom: 10px;\n}\n.scheduler-setting-editor .container .status-row .left {\n  float: left;\n  width: 50%;\n}\n.scheduler-setting-editor .container .status-row .right {\n  float: right;\n  width: 50%;\n  text-align: right;\n}\n.scheduler-setting-editor .container .status-row .right .checkbox {\n  float: right;\n}\n.scheduler-setting-editor .container .editor-row {\n  float: left;\n  width: 100%;\n}\n.scheduler-setting-editor .container .editor-row label {\n  font-weight: bolder;\n  clear: both;\n  float: left;\n  text-align: left;\n  width: 100%;\n  margin-bottom: 5px;\n}\n.scheduler-setting-editor .container .editor-row select {\n  padding: 8px 16px 8px 5px;\n}\n.scheduler-setting-editor .container .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.scheduler-setting-editor .container .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.scheduler-setting-editor .container .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.scheduler-setting-editor .container .editor-row .dnn-dropdown {\n  width: 100%;\n  margin-bottom: 30px;\n}\n.scheduler-setting-editor .container .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.scheduler-setting-editor .container .editor-row .dnn-day-picker {\n  width: 100%;\n  margin-bottom: 32px;\n}\n.scheduler-setting-editor .container .editor-row .dnn-day-picker {\n  width: 100%;\n}\n.scheduler-setting-editor .container .editor-row .dnn-day-picker select {\n  padding: 0;\n}\n.scheduler-setting-editor .container .editor-row .dnn-day-picker .calendar-text {\n  width: calc(100% - 37px);\n}\n.scheduler-setting-editor .container .editor-row .dnn-day-picker .clear-button {\n  line-height: 28px;\n}\n.scheduler-setting-editor .container .editor-row .switch-button label {\n  width: auto;\n}\n.scheduler-setting-editor .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 10px 0;\n}\n.scheduler-setting-editor .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.scheduler-setting-editor .buttons-box .edit-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.scheduler-setting-editor .buttons-box .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n", ""]);



/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(45);

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
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items {\n  width: 100%;\n  display: block;\n  float: left;\n  padding: 0 20px;\n  box-sizing: border-box;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items * {\n  box-sizing: border-box;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .schedule-items-grid {\n  border: solid 1px #C8C8C8;\n  width: 100%;\n  float: left;\n  margin-bottom: 20px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-row {\n  border-bottom: 1px solid #C8C8C8;\n  padding: 10px 0 10px 0;\n  width: 100%;\n  float: left;\n  overflow: hidden;\n  text-transform: uppercase;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-FriendlyName {\n  width: 28%;\n  float: left;\n  font-weight: bolder;\n  padding-left: 15px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-Frequency {\n  width: 17%;\n  float: left;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-RetryTimeLapse {\n  width: 17%;\n  float: left;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-NextStart {\n  width: 19%;\n  float: left;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-Enabled {\n  width: 10%;\n  float: left;\n  font-weight: bolder;\n  margin-left: 12px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .header-LogSettingEditButton {\n  float: left;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .collapsible-component .collapsible-header {\n  text-align: right;\n  text-transform: none;\n  padding-right: 40px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .collapsible-component .collapsible-header .collapse-icon {\n  display: none;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .collapsible-component .collapsible-header .collapse-icon.collapsed {\n  display: none;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .servergroup-filter-container {\n  width: 30%;\n  float: left;\n  margin: 10px 0 15px 0;\n  border-bottom: 1px solid #C8C8C8;\n  height: 37px;\n  font-weight: bolder;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow {\n  text-align: right;\n  width: 70%;\n  float: right;\n  margin: 10px 0 15px 0;\n  font-weight: bolder;\n  border-bottom: 1px solid #C8C8C8;\n  overflow: hidden;\n  height: 37px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox {\n  width: auto;\n  float: right;\n  color: #4B4E4F;\n  cursor: pointer;\n  margin-top: 8px;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n  cursor: pointer;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox .add-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n  fill: #4B4E4F;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox-active {\n  width: auto;\n  float: right;\n  color: #1E88C3;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox-active .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .AddItemRow .AddItemBox-active .add-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n  fill: #1E88C3;\n}\n#scheduler-container .dnn-persona-bar-page-body .react-tabs .schedule-items .schedule-item {\n  color: #4B4E4F;\n  word-break: keep-all;\n  word-wrap: break-word;\n}\n", ""]);



/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane-time {\n  float: right;\n  padding: 0 0 10px 0;\n  color: #4B4E4F;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane-time .topPane-time-name {\n  float: left;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane-time .topPane-time-value {\n  float: right;\n  width: 150px;\n  text-align: right;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane {\n  background-color: #fff;\n  padding: 15px 15px 15px 15px;\n  display: block;\n  box-sizing: border-box;\n  float: left;\n  width: 100%;\n  height: auto;\n  border: 1px solid #C8C8C8;\n  margin-bottom: 15px;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-left {\n  float: left;\n  margin-right: 30px;\n  font-weight: bolder;\n  font-size: 14px;\n  width: 40px;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle {\n  float: left;\n  padding-right: 50px;\n  width: 35%;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-name {\n  font-weight: bolder;\n  font-size: 14px;\n  padding: 0 0 9px 0;\n  color: #1E88C3;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-name-stopped {\n  font-weight: bolder;\n  font-size: 14px;\n  padding: 0 0 9px 0;\n  color: #EA2134;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common {\n  color: #4B4E4F;\n  width: 100%;\n  float: left;\n  padding: 0 0 10px 0;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .topPane-middle-common-title {\n  font-weight: bolder;\n  float: left;\n  max-width: 90%;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .topPane-middle-common-value {\n  float: right;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .editIcon {\n  width: 16px;\n  height: 16px;\n  float: right;\n  padding-left: 3px;\n  margin-right: -20px;\n  cursor: pointer;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .editIcon > svg {\n  fill: #C8C8C8;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .editIcon > svg:hover {\n  fill: #4B4E4F;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .editIconDisabled {\n  width: 16px;\n  height: 16px;\n  float: right;\n  padding-left: 3px;\n  margin-right: -20px;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .editIconDisabled > svg {\n  fill: #C8C8C8;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .collapsible-content {\n  z-index: 1000;\n  background-color: #FFFFFF;\n  position: absolute;\n  width: 420px;\n  margin: -45px 0 0 -92px;\n  text-align: left;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-middle .topPane-middle-common .collapsible-content .collapsible-content-mode > div {\n  border: solid 1px #C8C8C8;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-right {\n  float: left;\n  border-right: solid 1px #C8C8C8;\n  padding-right: 30px;\n  width: 20%;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-right .topPane-right-common {\n  color: #4B4E4F;\n  width: 100%;\n  float: left;\n  padding: 0 0 10px 0;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-right .topPane-right-common .topPane-right-common-title {\n  font-weight: bolder;\n  float: left;\n  max-width: 90%;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-right .topPane-right-common .topPane-right-common-value {\n  float: right;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-button {\n  float: right;\n  padding-top: 15px;\n  margin-right: 10px;\n  max-width: 155px;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-button .dnn-ui-common-button {\n  height: auto;\n  padding-top: 6px;\n  padding-bottom: 6px;\n}\n.YqvYUn6Ry3K-kzlcbB7Z3 .topPane .topPane-button .topPane-button-start {\n  border: 2px solid #EA2134;\n  color: #EA2134;\n}\n", ""]);

// Exports
exports.locals = {
	"topPaneWrapper": "YqvYUn6Ry3K-kzlcbB7Z3"
};

/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(48);

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
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-content-mode-hidden {\n  box-shadow: 0 0 17px 2px #C8C8C8;\n  box-shadow: none;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper {\n  padding: 20px 30px 20px 30px;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .divider {\n  padding-bottom: 15px;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row {\n  float: left;\n  width: 100%;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row label {\n  font-weight: bolder;\n  clear: both;\n  float: left;\n  text-align: left;\n  margin-bottom: 5px;\n  width: 120px;\n  padding: 7px 0 0 0;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row select {\n  padding: 8px 16px 8px 5px;\n  width: 220px;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .editor-row input {\n  float: left;\n  width: 220px;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .action-buttons {\n  text-align: center;\n  margin: 20px 0;\n}\n.collapsible-content-mode-hidden .modepanel-content-wrapper .action-buttons button:first-child {\n  margin-right: 15px;\n}\n.collapsible-content-mode {\n  box-shadow: 0 0 17px 2px #C8C8C8;\n}\n.collapsible-content-mode .modepanel-content-wrapper {\n  padding: 20px 30px 20px 30px;\n}\n.collapsible-content-mode .modepanel-content-wrapper .divider {\n  padding-bottom: 15px;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row {\n  float: left;\n  width: 100%;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row label {\n  font-weight: bolder;\n  clear: both;\n  float: left;\n  text-align: left;\n  margin-bottom: 5px;\n  width: 120px;\n  padding: 7px 0 0 0;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row select {\n  padding: 8px 16px 8px 5px;\n  width: 220px;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.collapsible-content-mode .modepanel-content-wrapper .editor-row input {\n  float: left;\n  width: 220px;\n}\n.collapsible-content-mode .modepanel-content-wrapper .action-buttons {\n  text-align: center;\n  margin: 20px 0;\n}\n.collapsible-content-mode .modepanel-content-wrapper .action-buttons button:first-child {\n  margin-right: 15px;\n}\n", ""]);



/***/ }),
/* 49 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<polygon points=\"306.5,1438.4 259,1789 609.6,1741.5 610.8,1740.3 307.7,1437.2 \t\"/>\r\n\t<rect x=\"294.4\" y=\"752.6\" transform=\"matrix(0.7071 -0.7071 0.7071 0.7071 -367.0447 1047.6558)\" width=\"1573.5\" height=\"428.7\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(51);

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
/* 51 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#scheduler-container .dnn-persona-bar-page-body {\n  padding: 10px 30px 25px 30px;\n}\n#scheduler-container .dnn-persona-bar-page-body .persona-bar-page-body {\n  padding: 0;\n  border: none;\n  background-color: #FBFCFC;\n}\n#scheduler-container .dnn-persona-bar-page-body .persona-bar-page-body .react-tabs {\n  float: left;\n  border: solid 1px #C8C8C8;\n  width: 100%;\n  box-sizing: border-box;\n}\n#scheduler-container .dnn-persona-bar-page-body .persona-bar-page-body .react-tabs .ReactTabs__TabPanel {\n  float: left;\n  background-color: #fff;\n  width: 100%;\n}\n#scheduler-container .dnn-persona-bar-page-body .persona-bar-page-body .row-opened {\n  margin-top: -1px;\n  margin-bottom: -1px;\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3 !important;\n}\n", ""]);



/***/ }),
/* 52 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: D:/a/1/s/node_modules/prop-types/index.js
var prop_types = __webpack_require__(1);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(5);

// EXTERNAL MODULE: external "window.dnn.nodeModules.CommonComponents"
var external_window_dnn_nodeModules_CommonComponents_ = __webpack_require__(2);

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
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



function serializeQueryStringParameters(obj) {
  var s = [];

  for (var p in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, p)) {
      s.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  }

  return s.join("&");
}

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
    key: "getTaskStatusList",
    value: function getTaskStatusList(searchParameters, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      searchParameters = _extends({}, searchParameters, {//logType: "*"
      });
      sf.getsilence("GetScheduleStatus?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "startSchedule",
    value: function startSchedule(callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("StartSchedule", {}, callback);
    }
  }, {
    key: "stopSchedule",
    value: function stopSchedule(callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("StopSchedule", {}, callback);
    }
  }, {
    key: "getSchedulerSettings",
    value: function getSchedulerSettings(callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.get("GetSchedulerSettings", {}, callback);
    }
  }, {
    key: "updateSchedulerSettings",
    value: function updateSchedulerSettings(payload, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("UpdateSchedulerSettings", payload, callback);
    }
  }, {
    key: "getScheduleItemHistory",
    value: function getScheduleItemHistory(searchParameters, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      searchParameters = _extends({}, searchParameters, {//logType: "*"
      });
      sf.get("GetScheduleItemHistory?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "getScheduleItems",
    value: function getScheduleItems(searchParameters, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      searchParameters = _extends({}, searchParameters, {//logType: "*"
      });
      sf.get("GetScheduleItems?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "getServerList",
    value: function getServerList(callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.get("GetServers", {}, callback);
    }
  }, {
    key: "getGetScheduleItem",
    value: function getGetScheduleItem(searchParameters, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      searchParameters = _extends({}, searchParameters, {//logType: "*"
      });
      sf.get("GetScheduleItem?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "deleteScheduleItem",
    value: function deleteScheduleItem(payload, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("DeleteSchedule", payload, callback);
    }
  }, {
    key: "createScheduleItem",
    value: function createScheduleItem(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("CreateScheduleItem", payload, callback, failureCallback);
    }
  }, {
    key: "updateScheduleItem",
    value: function updateScheduleItem(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("UpdateScheduleItem", payload, callback, failureCallback);
    }
  }, {
    key: "runScheduleItem",
    value: function runScheduleItem(payload, callback) {
      var sf = this.getServiceFramework("TaskScheduler");
      sf.post("RunSchedule", payload, callback);
    }
  }]);

  return ApplicationService;
}();

var applicationService = new applicationService_ApplicationService();
/* harmony default export */ var services_applicationService = (applicationService);
// CONCATENATED MODULE: ./src/actions/task.js


var taskActions = {
  getTaskStatusList: function getTaskStatusList(searchParameters, callback) {
    return function (dispatch) {
      services_applicationService.getTaskStatusList(searchParameters, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_TASK_STATUS_LIST,
          data: {
            serverTime: data.Results.ServerTime,
            schedulingEnabled: data.Results.SchedulingEnabled,
            status: data.Results.Status,
            freeThreads: data.Results.FreeThreadCount,
            activeThreads: data.Results.ActiveThreadCount,
            maxThreads: data.Results.MaxThreadCount,
            taskProcessingList: data.Results.ScheduleProcessing,
            taskStatusList: data.Results.ScheduleQueue,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  startSchedule: function startSchedule(callback) {
    return function (dispatch) {
      services_applicationService.startSchedule(function (data) {
        dispatch({
          type: actionTypes["b" /* task */].STARTED_SCHEDULE,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  stopSchedule: function stopSchedule(callback) {
    return function (dispatch) {
      services_applicationService.stopSchedule(function (data) {
        dispatch({
          type: actionTypes["b" /* task */].STOPPED_SCHEDULE,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getSchedulerSettings: function getSchedulerSettings(callback) {
    return function (dispatch) {
      services_applicationService.getSchedulerSettings(function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_SCHEDULE_SETTINGS,
          data: {
            schedulerModeOptions: data.Results.SchedulerModeOptions,
            schedulerMode: data.Results.SchedulerMode,
            schedulerDelay: data.Results.SchedulerdelayAtAppStart
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateSchedulerSettings: function updateSchedulerSettings(payload, callback) {
    return function (dispatch) {
      services_applicationService.updateSchedulerSettings(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].UPDATED_SCHEDULE_SETTINGS,
          data: {
            schedulerMode: payload.SchedulerMode,
            schedulerDelay: payload.SchedulerdelayAtAppStart,
            settingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateSchedulerStatusOnProgress: function updateSchedulerStatusOnProgress(callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* task */].UPDATED_SCHEDULE_SETTINGS_PENDING,
        data: {
          schedulerMode: "Pending"
        }
      });

      if (callback) {
        callback();
      }
    };
  },
  getScheduleItemHistory: function getScheduleItemHistory(searchParameters, callback) {
    return function (dispatch) {
      services_applicationService.getScheduleItemHistory(searchParameters, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_SCHEDULE_HISTORY,
          data: {
            taskHistoryList: data.Results,
            totalHistoryCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getSchedulerItemList: function getSchedulerItemList(searchParameters, callback) {
    return function (dispatch) {
      services_applicationService.getScheduleItems(searchParameters, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_SCHEDULE_ITEMS,
          data: {
            schedulerItemList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getServerList: function getServerList(callback) {
    return function (dispatch) {
      services_applicationService.getServerList(function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_SERVER_LIST,
          data: {
            serverList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getGetScheduleItem: function getGetScheduleItem(searchParameters, callback) {
    return function (dispatch) {
      services_applicationService.getGetScheduleItem(searchParameters, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].RETRIEVED_SCHEDULE_ITEM,
          data: {
            scheduleItemDetail: data.Results,
            totalCount: data.TotalResults,
            settingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  deleteSchedule: function deleteSchedule(payload, schedulerItemList, callback) {
    return function (dispatch) {
      services_applicationService.deleteScheduleItem(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].DELETED_SCHEDULE_ITEM,
          data: {
            schedulerItemList: schedulerItemList,
            totalCount: schedulerItemList.length
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  createScheduleItem: function createScheduleItem(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.createScheduleItem(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].CREATED_SCHEDULE_ITEM,
          data: {
            Success: data.Success,
            settingsClientModified: false
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
  updateScheduleItem: function updateScheduleItem(payload, callback, failureCallback) {
    return function (dispatch) {
      services_applicationService.updateScheduleItem(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].UPDATED_SCHEDULE_ITEM,
          data: {
            Success: data.Success,
            settingsClientModified: false
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
  runScheduleItem: function runScheduleItem(payload, callback) {
    return function (dispatch) {
      services_applicationService.runScheduleItem(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* task */].EXECUTED_SCHEDULE_ITEM,
          data: {
            Success: data.Success
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  settingsClientModified: function settingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* task */].SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED,
        data: {
          scheduleItemDetail: parameter,
          settingsClientModified: true
        }
      });
    };
  },
  cancelSettingsClientModified: function cancelSettingsClientModified() {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* task */].CANCELED_SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED,
        data: {
          settingsClientModified: false
        }
      });
    };
  }
};
/* harmony default export */ var task = (taskActions);
// CONCATENATED MODULE: ./src/actions/index.js




// EXTERNAL MODULE: ./src/components/taskQueue/taskStatusItemRow/style.less
var style = __webpack_require__(17);
var style_default = /*#__PURE__*/__webpack_require__.n(style);

// EXTERNAL MODULE: ./src/components/taskQueue/LeftPane/style.less
var LeftPane_style = __webpack_require__(18);
var LeftPane_style_default = /*#__PURE__*/__webpack_require__.n(LeftPane_style);

// CONCATENATED MODULE: ./src/resources/index.jsx

var resx = {
  get: function get(key) {
    var moduleName = utils["a" /* default */].moduleName;
    return utils["a" /* default */].utilities.getResx(moduleName, key);
  }
};
/* harmony default export */ var resources = (resx);
// CONCATENATED MODULE: ./src/components/taskQueue/LeftPane/LeftPane.jsx
function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function LeftPane_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function LeftPane_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function LeftPane_createClass(Constructor, protoProps, staticProps) { if (protoProps) LeftPane_defineProperties(Constructor.prototype, protoProps); if (staticProps) LeftPane_defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }





/*eslint-disable quotes*/

var svgIcon = __webpack_require__(29).default;

var svgIcon2 = __webpack_require__(30).default;

var LeftPane_LeftPane =
/*#__PURE__*/
function (_Component) {
  _inherits(LeftPane, _Component);

  function LeftPane() {
    LeftPane_classCallCheck(this, LeftPane);

    return _possibleConstructorReturn(this, _getPrototypeOf(LeftPane).call(this));
  }

  LeftPane_createClass(LeftPane, [{
    key: "onEnter",
    value: function onEnter(key, value) {
      var props = this.props;
      props.onEnter(key, value, props.index);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: LeftPane_style_default.a.taskStatusItemLeftPane
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.nextStart,
          className: "taskIcon",
          dangerouslySetInnerHTML: {
            __html: svgIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.startDate,
          className: "taskIconProcessing",
          dangerouslySetInnerHTML: {
            __html: svgIcon2
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-name"
        }, props.friendlyName),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.startDate,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title-processing"
        }, resources.get("processing"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.nextStart,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title"
        }, resources.get("NextStart.Label")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-value"
        }, props.nextStart)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.startDate,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title"
        }, resources.get("Started.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-value"
        }, props.startDate)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.nextStart,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title"
        }, resources.get("Overdue.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-value"
        }, props.overdue)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.startDate,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title"
        }, resources.get("Duration.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-value"
        }, props.elapsedTime)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          hidden: !this.props.nextStart,
          className: "taskDetail-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-title"
        }, resources.get("TimeRemaining.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetail-common-value"
        }, props.remainingTime))))
      );
    }
  }]);

  return LeftPane;
}(external_window_dnn_nodeModules_React_["Component"]);

LeftPane_LeftPane.propTypes = {
  friendlyName: prop_types_default.a.string,
  nextStart: prop_types_default.a.string,
  overdue: prop_types_default.a.bool,
  remainingTime: prop_types_default.a.string,
  elapsedTime: prop_types_default.a.number,
  startDate: prop_types_default.a.string
};
/* harmony default export */ var taskQueue_LeftPane_LeftPane = (LeftPane_LeftPane);
// CONCATENATED MODULE: ./src/components/taskQueue/LeftPane/index.jsx

/* harmony default export */ var taskQueue_LeftPane = (taskQueue_LeftPane_LeftPane);
// EXTERNAL MODULE: ./src/components/taskQueue/RightPane/style.less
var RightPane_style = __webpack_require__(19);
var RightPane_style_default = /*#__PURE__*/__webpack_require__.n(RightPane_style);

// CONCATENATED MODULE: ./src/components/taskQueue/RightPane/RightPane.jsx
function RightPane_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { RightPane_typeof = function _typeof(obj) { return typeof obj; }; } else { RightPane_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return RightPane_typeof(obj); }

function RightPane_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function RightPane_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function RightPane_createClass(Constructor, protoProps, staticProps) { if (protoProps) RightPane_defineProperties(Constructor.prototype, protoProps); if (staticProps) RightPane_defineProperties(Constructor, staticProps); return Constructor; }

function RightPane_possibleConstructorReturn(self, call) { if (call && (RightPane_typeof(call) === "object" || typeof call === "function")) { return call; } return RightPane_assertThisInitialized(self); }

function RightPane_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function RightPane_getPrototypeOf(o) { RightPane_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return RightPane_getPrototypeOf(o); }

function RightPane_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) RightPane_setPrototypeOf(subClass, superClass); }

function RightPane_setPrototypeOf(o, p) { RightPane_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return RightPane_setPrototypeOf(o, p); }






var RightPane_RightPane =
/*#__PURE__*/
function (_Component) {
  RightPane_inherits(RightPane, _Component);

  function RightPane() {
    RightPane_classCallCheck(this, RightPane);

    return RightPane_possibleConstructorReturn(this, RightPane_getPrototypeOf(RightPane).call(this));
  }

  RightPane_createClass(RightPane, [{
    key: "onEnter",
    value: function onEnter(key, value) {
      var props = this.props;
      props.onEnter(key, value, props.index);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: RightPane_style_default.a.taskStatusItemRightPane
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-idWrapper"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-id"
        }, resources.get("ScheduleID.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-id-value"
        }, props.scheduleId)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-title"
        }, resources.get("Servers.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-value"
        }, props.servers)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-title"
        }, resources.get("ObjectDependencies.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-value"
        }, props.objectDependencies)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-title"
        }, resources.get("TriggeredBy.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-value"
        }, props.scheduleSource)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-title"
        }, resources.get("Thread.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskDetailRight-common-value"
        }, props.threadId))))
      );
    }
  }]);

  return RightPane;
}(external_window_dnn_nodeModules_React_["Component"]);

RightPane_RightPane.propTypes = {
  scheduleId: prop_types_default.a.number,
  servers: prop_types_default.a.string,
  objectDependencies: prop_types_default.a.string,
  threadId: prop_types_default.a.number,
  scheduleSource: prop_types_default.a.string
};
/* harmony default export */ var taskQueue_RightPane_RightPane = (RightPane_RightPane);
// CONCATENATED MODULE: ./src/components/taskQueue/RightPane/index.jsx

/* harmony default export */ var taskQueue_RightPane = (taskQueue_RightPane_RightPane);
// CONCATENATED MODULE: ./src/components/taskQueue/taskStatusItemRow/index.jsx
function taskStatusItemRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { taskStatusItemRow_typeof = function _typeof(obj) { return typeof obj; }; } else { taskStatusItemRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return taskStatusItemRow_typeof(obj); }

function taskStatusItemRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function taskStatusItemRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function taskStatusItemRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) taskStatusItemRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) taskStatusItemRow_defineProperties(Constructor, staticProps); return Constructor; }

function taskStatusItemRow_possibleConstructorReturn(self, call) { if (call && (taskStatusItemRow_typeof(call) === "object" || typeof call === "function")) { return call; } return taskStatusItemRow_assertThisInitialized(self); }

function taskStatusItemRow_getPrototypeOf(o) { taskStatusItemRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return taskStatusItemRow_getPrototypeOf(o); }

function taskStatusItemRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function taskStatusItemRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) taskStatusItemRow_setPrototypeOf(subClass, superClass); }

function taskStatusItemRow_setPrototypeOf(o, p) { taskStatusItemRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return taskStatusItemRow_setPrototypeOf(o, p); }









var taskStatusItemRow_TaskStatusItemRow =
/*#__PURE__*/
function (_Component) {
  taskStatusItemRow_inherits(TaskStatusItemRow, _Component);

  function TaskStatusItemRow() {
    var _this;

    taskStatusItemRow_classCallCheck(this, TaskStatusItemRow);

    _this = taskStatusItemRow_possibleConstructorReturn(this, taskStatusItemRow_getPrototypeOf(TaskStatusItemRow).call(this));
    _this.state = {
      collapsed: true,
      collapsedClass: true,
      repainting: false
    };
    _this.handleClick = _this.handleClick.bind(taskStatusItemRow_assertThisInitialized(_this));
    return _this;
  }

  taskStatusItemRow_createClass(TaskStatusItemRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      document.addEventListener("click", this.handleClick);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener("click", this.handleClick);
      this._isMounted = false;
    }
    /*eslint-disable eqeqeq*/

  }, {
    key: "handleClick",
    value: function handleClick(event) {
      // Note: this workaround is needed in IE. The remove event listener in the componentWillUnmount is called
      // before the handleClick handler is called, but in spite of that, the handleClick is executed. To avoid
      // the "findDOMNode was called on an unmounted component." error we need to check if the component is mounted before execute this code
      if (!this._isMounted) {
        return;
      }

      if (!this.node.contains(event.target) && typeof event.target.className == "string" && event.target.className.indexOf("do-not-close") == -1) {
        this.timeout = 475;
      } else {
        this.timeout = 0;
      }
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: style_default.a.taskStatusItemRow,
          ref: function ref(node) {
            return _this2.node = node;
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(taskQueue_LeftPane, {
          friendlyName: props.friendlyName,
          nextStart: props.nextStart,
          overdue: props.overdue,
          remainingTime: props.remainingTime,
          elapsedTime: props.elapsedTime,
          startDate: props.startDate,
          key: "schedule-left-" + props.scheduleId
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(taskQueue_RightPane, {
          scheduleId: props.scheduleId,
          servers: props.servers,
          objectDependencies: props.objectDependencies,
          threadId: props.threadId,
          scheduleSource: props.scheduleSource,
          key: "schedule-right-" + props.scheduleId
        })))
      );
    }
  }]);

  return TaskStatusItemRow;
}(external_window_dnn_nodeModules_React_["Component"]);

taskStatusItemRow_TaskStatusItemRow.propTypes = {
  scheduleId: prop_types_default.a.number,
  friendlyName: prop_types_default.a.string,
  overdue: prop_types_default.a.bool,
  remainingTime: prop_types_default.a.string,
  nextStart: prop_types_default.a.string,
  objectDependencies: prop_types_default.a.string,
  scheduleSource: prop_types_default.a.string,
  servers: prop_types_default.a.string,
  threadId: prop_types_default.a.number,
  elapsedTime: prop_types_default.a.number,
  startDate: prop_types_default.a.string,
  children: prop_types_default.a.node
};

function mapStateToProps() {
  return {};
}

/* harmony default export */ var taskStatusItemRow = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(mapStateToProps)(taskStatusItemRow_TaskStatusItemRow));
// EXTERNAL MODULE: ./src/components/taskQueue/style.less
var taskQueue_style = __webpack_require__(32);

// CONCATENATED MODULE: ./src/components/taskQueue/index.jsx
function taskQueue_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { taskQueue_typeof = function _typeof(obj) { return typeof obj; }; } else { taskQueue_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return taskQueue_typeof(obj); }

function taskQueue_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function taskQueue_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function taskQueue_createClass(Constructor, protoProps, staticProps) { if (protoProps) taskQueue_defineProperties(Constructor.prototype, protoProps); if (staticProps) taskQueue_defineProperties(Constructor, staticProps); return Constructor; }

function taskQueue_possibleConstructorReturn(self, call) { if (call && (taskQueue_typeof(call) === "object" || typeof call === "function")) { return call; } return taskQueue_assertThisInitialized(self); }

function taskQueue_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function taskQueue_getPrototypeOf(o) { taskQueue_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return taskQueue_getPrototypeOf(o); }

function taskQueue_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) taskQueue_setPrototypeOf(subClass, superClass); }

function taskQueue_setPrototypeOf(o, p) { taskQueue_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return taskQueue_setPrototypeOf(o, p); }








var noDataImage = __webpack_require__(34).default;

var taskQueue_TaskQueuePanelBody =
/*#__PURE__*/
function (_Component) {
  taskQueue_inherits(TaskQueuePanelBody, _Component);

  function TaskQueuePanelBody() {
    taskQueue_classCallCheck(this, TaskQueuePanelBody);

    return taskQueue_possibleConstructorReturn(this, taskQueue_getPrototypeOf(TaskQueuePanelBody).call(this));
  }
  /* eslint-disable react/no-danger */


  taskQueue_createClass(TaskQueuePanelBody, [{
    key: "renderedTaskStatusList",
    value: function renderedTaskStatusList() {
      var props = this.props;
      return props.taskStatusList.map(function (term, index) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(taskStatusItemRow, {
            scheduleId: term.ScheduleID,
            friendlyName: term.FriendlyName,
            overdue: term.Overdue,
            remainingTime: term.RemainingTime,
            nextStart: term.NextStart,
            objectDependencies: term.ObjectDependencies,
            scheduleSource: term.ScheduleSource,
            threadId: term.ThreadID,
            servers: term.Servers,
            key: "taskStatusItem-" + index,
            closeOnClick: true
          })
        );
      });
    }
  }, {
    key: "renderedTaskProcessingList",
    value: function renderedTaskProcessingList() {
      var props = this.props;

      if (props.taskProcessingList) {
        return props.taskProcessingList.map(function (term, index) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(taskStatusItemRow, {
              scheduleId: term.ScheduleID,
              friendlyName: term.TypeFullName,
              elapsedTime: term.ElapsedTime,
              startDate: term.StartDate,
              objectDependencies: term.ObjectDependencies,
              scheduleSource: term.ScheduleSource,
              threadId: term.ThreadID,
              servers: term.Servers,
              key: "taskStatusItem-" + index,
              closeOnClick: true
            })
          );
        });
      }
    }
    /*eslint-disable eqeqeq*/

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null, props.taskStatusList && props.taskStatusList.length == 0 && props.taskProcessingList && props.taskProcessingList.length == 0 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "noData"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "noTasks"
        }, props.schedulingEnabled === "True" ? resources.get("NoTasks") : resources.get("DisabledMessage")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "noTasksMessage"
        }, resources.get("NoTasksMessage")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: noDataImage
          }
        })), this.renderedTaskProcessingList(), this.renderedTaskStatusList())
      );
    }
  }]);

  return TaskQueuePanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

taskQueue_TaskQueuePanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  schedulingEnabled: prop_types_default.a.string,
  taskStatusList: prop_types_default.a.array,
  taskProcessingList: prop_types_default.a.array
};

function taskQueue_mapStateToProps(state) {
  return {
    schedulingEnabled: state.task.schedulingEnabled,
    taskStatusList: state.task.taskStatusList,
    taskProcessingList: state.task.taskProcessingList,
    tabIndex: state.pagination.tabIndex
  };
}

/* harmony default export */ var taskQueue = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(taskQueue_mapStateToProps)(taskQueue_TaskQueuePanelBody));
// EXTERNAL MODULE: ./src/components/history/taskHistoryItemRow/style.less
var taskHistoryItemRow_style = __webpack_require__(20);
var taskHistoryItemRow_style_default = /*#__PURE__*/__webpack_require__.n(taskHistoryItemRow_style);

// CONCATENATED MODULE: ./src/components/history/taskHistoryItemRow/index.jsx
function taskHistoryItemRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { taskHistoryItemRow_typeof = function _typeof(obj) { return typeof obj; }; } else { taskHistoryItemRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return taskHistoryItemRow_typeof(obj); }

function taskHistoryItemRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function taskHistoryItemRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function taskHistoryItemRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) taskHistoryItemRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) taskHistoryItemRow_defineProperties(Constructor, staticProps); return Constructor; }

function taskHistoryItemRow_possibleConstructorReturn(self, call) { if (call && (taskHistoryItemRow_typeof(call) === "object" || typeof call === "function")) { return call; } return taskHistoryItemRow_assertThisInitialized(self); }

function taskHistoryItemRow_getPrototypeOf(o) { taskHistoryItemRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return taskHistoryItemRow_getPrototypeOf(o); }

function taskHistoryItemRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function taskHistoryItemRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) taskHistoryItemRow_setPrototypeOf(subClass, superClass); }

function taskHistoryItemRow_setPrototypeOf(o, p) { taskHistoryItemRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return taskHistoryItemRow_setPrototypeOf(o, p); }





/*eslint-disable quotes*/

var taskHistoryItemRow_svgIcon = __webpack_require__(36).default;

var taskHistoryItemRow_TaskHistoryItemRow =
/*#__PURE__*/
function (_Component) {
  taskHistoryItemRow_inherits(TaskHistoryItemRow, _Component);

  function TaskHistoryItemRow() {
    var _this;

    taskHistoryItemRow_classCallCheck(this, TaskHistoryItemRow);

    _this = taskHistoryItemRow_possibleConstructorReturn(this, taskHistoryItemRow_getPrototypeOf(TaskHistoryItemRow).call(this));
    _this.state = {
      collapsed: true,
      collapsedClass: true,
      repainting: false
    };
    _this.timeout = 0; // setInterval(() => {
    //     console.log("Repainting: ", this.state.repainting);
    // }, 500);

    _this.handleClick = _this.handleClick.bind(taskHistoryItemRow_assertThisInitialized(_this));
    return _this;
  }

  taskHistoryItemRow_createClass(TaskHistoryItemRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      document.addEventListener("click", this.handleClick);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener("click", this.handleClick);
      this._isMounted = false;
    }
    /*eslint-disable eqeqeq*/

  }, {
    key: "handleClick",
    value: function handleClick(event) {
      // Note: this workaround is needed in IE. The remove event listener in the componentWillUnmount is called
      // before the handleClick handler is called, but in spite of that, the handleClick is executed. To avoid
      // the "findDOMNode was called on an unmounted component." error we need to check if the component is mounted before execute this code
      if (!this._isMounted) {
        return;
      }

      if (!this.node.contains(event.target) && typeof event.target.className == "string" && event.target.className.indexOf("do-not-close") == -1) {
        this.timeout = 475;
      } else {
        this.timeout = 0;
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getSucceededDisplay",
    value: function getSucceededDisplay() {
      if (this.props.succeeded) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "checkMarkIcon",
            dangerouslySetInnerHTML: {
              __html: taskHistoryItemRow_svgIcon
            }
          })
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, "\xA0 ")
      );
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getLogNotesDisplay",
    value: function getLogNotesDisplay() {
      if (this.props.friendlyName.length > 0 || this.props.logNotes.length > 0) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", null, this.props.friendlyName),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            dangerouslySetInnerHTML: {
              __html: this.props.logNotes
            }
          }))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, "\xA0 ")
      );
    }
  }, {
    key: "getStartEndDisplay",
    value: function getStartEndDisplay() {
      var display = "<span>&nbsp;</span>";

      if (this.props.startDate) {
        display = "<p>S: " + this.props.startDate + "</p>";
      }

      if (this.props.endDate) {
        display += "<p>E: " + this.props.endDate + "</p>";
      }

      if (this.props.nextStart) {
        display += "<p>N: " + this.props.nextStart + "</p>";
      }

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: display
          }
        })
      );
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: taskHistoryItemRow_style_default.a.taskHistoryItemRow,
          ref: function ref(node) {
            return _this2.node = node;
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-logNotes"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-wrapper"
        }, this.getLogNotesDisplay())),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-server"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, this.props.server, "\xA0 "))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-elapsedTime"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, this.props.elapsedTime, "\xA0 "))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-succeeded"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-wrapper"
        }, this.getSucceededDisplay())),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-startEnd"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label-wrapper"
        }, this.getStartEndDisplay())))
      );
    }
  }]);

  return TaskHistoryItemRow;
}(external_window_dnn_nodeModules_React_["Component"]);

taskHistoryItemRow_TaskHistoryItemRow.propTypes = {
  friendlyName: prop_types_default.a.string,
  logNotes: prop_types_default.a.string,
  server: prop_types_default.a.string,
  elapsedTime: prop_types_default.a.number,
  succeeded: prop_types_default.a.bool,
  startDate: prop_types_default.a.string,
  endDate: prop_types_default.a.string,
  nextStart: prop_types_default.a.string,
  children: prop_types_default.a.node
};

function taskHistoryItemRow_mapStateToProps() {
  return {};
}

/* harmony default export */ var taskHistoryItemRow = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(taskHistoryItemRow_mapStateToProps)(taskHistoryItemRow_TaskHistoryItemRow));
// EXTERNAL MODULE: ./src/components/history/style.less
var history_style = __webpack_require__(37);

// CONCATENATED MODULE: ./src/components/history/index.jsx
function history_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { history_typeof = function _typeof(obj) { return typeof obj; }; } else { history_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return history_typeof(obj); }

function history_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function history_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function history_createClass(Constructor, protoProps, staticProps) { if (protoProps) history_defineProperties(Constructor.prototype, protoProps); if (staticProps) history_defineProperties(Constructor, staticProps); return Constructor; }

function history_possibleConstructorReturn(self, call) { if (call && (history_typeof(call) === "object" || typeof call === "function")) { return call; } return history_assertThisInitialized(self); }

function history_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function history_getPrototypeOf(o) { history_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return history_getPrototypeOf(o); }

function history_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) history_setPrototypeOf(subClass, superClass); }

function history_setPrototypeOf(o, p) { history_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return history_setPrototypeOf(o, p); }









/*eslint-disable quotes*/

var history_svgIcon = __webpack_require__(10).default;

var pageSizeOptions = [];
var tableFields = [];

var history_HistoryPanelBody =
/*#__PURE__*/
function (_Component) {
  history_inherits(HistoryPanelBody, _Component);

  function HistoryPanelBody(props) {
    var _this;

    history_classCallCheck(this, HistoryPanelBody);

    _this = history_possibleConstructorReturn(this, history_getPrototypeOf(HistoryPanelBody).call(this));
    _this.state = {
      taskHistoryList: [],
      scheduleId: -1,
      pageIndex: 0,
      pageSize: props.pageSize ? props.pageSize : 10,
      totalCount: 0
    };
    return _this;
  }

  history_createClass(HistoryPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props,
          state = this.state;
      props.dispatch(task.getScheduleItemHistory({
        scheduleId: props.scheduleId,
        pageIndex: state.pageIndex,
        pageSize: state.pageSize
      }));
      tableFields = [];
      tableFields.push({
        "name": resources.get("DescriptionColumn"),
        "id": "LogNotes"
      });
      tableFields.push({
        "name": resources.get("RanOnServerColumn"),
        "id": "Server"
      });
      tableFields.push({
        "name": resources.get("DurationColumn"),
        "id": "ElapsedTime"
      });
      tableFields.push({
        "name": resources.get("SucceededColumn"),
        "id": "Succeeded"
      });
      tableFields.push({
        "name": resources.get("StartEndColumn"),
        "id": "StartEnd"
      });
      pageSizeOptions = [];
      pageSizeOptions.push({
        "value": "10",
        "label": "10 entries per page"
      });
      pageSizeOptions.push({
        "value": "25",
        "label": "25 entries per page"
      });
      pageSizeOptions.push({
        "value": "50",
        "label": "50 entries per page"
      });
      pageSizeOptions.push({
        "value": "100",
        "label": "100 entries per page"
      });
      pageSizeOptions.push({
        "value": "250",
        "label": "250 entries per page"
      });
    }
  }, {
    key: "onEnter",
    value: function onEnter(key) {
      var state = this.state;
      alert("You pressed enter! My value is: " + state[key]);
    }
  }, {
    key: "onPageChange",
    value: function onPageChange(currentPage, pageSize) {
      var state = this.state,
          props = this.props;

      if (pageSize !== undefined && state.pageSize !== pageSize) {
        state.pageSize = pageSize;
      }

      state.pageIndex = currentPage;
      this.setState({
        state: state
      }, function () {
        props.dispatch(task.getScheduleItemHistory({
          scheduleId: props.scheduleId,
          pageIndex: state.pageIndex,
          pageSize: state.pageSize
        }));
      });
    }
  }, {
    key: "renderedHistoryListHeader",
    value: function renderedHistoryListHeader() {
      var tableHeaders = tableFields.map(function (field, index) {
        var className = "historyHeader historyHeader-" + field.id;
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: className,
            key: "historyHeader-" + index
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "historyHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderPager",
    value: function renderPager() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskHistoryList-pager"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Pager"], {
          showStartEndButtons: true,
          showPageSizeOptions: true,
          showPageInfo: false,
          numericCounters: 4,
          pageSize: state.pageSize,
          totalRecords: props.totalCount || 0,
          onPageChanged: this.onPageChange.bind(this),
          pageSizeDropDownWithoutBorder: true,
          pageSizeOptionText: resources.get("pageSizeOption"),
          summaryText: resources.get("pagerSummary")
        }))
      );
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderedHistoryList",
    value: function renderedHistoryList() {
      var props = this.props;

      if (props.taskHistoryList) {
        return props.taskHistoryList.map(function (term, index) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(taskHistoryItemRow, {
              friendlyName: term.FriendlyName,
              logNotes: term.LogNotes,
              server: term.Server,
              elapsedTime: term.ElapsedTime,
              succeeded: term.Succeeded,
              startDate: term.StartDate,
              endDate: term.EndDate,
              nextStart: term.NextStart,
              key: "taskHistoryItem-" + index,
              closeOnClick: true
            })
          );
        });
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "historyIcon",
          dangerouslySetInnerHTML: {
            __html: history_svgIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskHistoryList-title"
        }, props.title),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taskHistoryList-grid"
        }, this.renderedHistoryListHeader(), this.renderedHistoryList()), this.renderPager())
      );
    }
  }]);

  return HistoryPanelBody;
}(external_window_dnn_nodeModules_React_["Component"]);

history_HistoryPanelBody.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  taskHistoryList: prop_types_default.a.array,
  scheduleId: prop_types_default.a.number,
  title: prop_types_default.a.string,
  totalCount: prop_types_default.a.number,
  pageSize: prop_types_default.a.number
};

function history_mapStateToProps(state) {
  return {
    taskHistoryList: state.task.taskHistoryList,
    tabIndex: state.pagination.tabIndex,
    totalCount: state.task.totalHistoryCount
  };
}

/* harmony default export */ var components_history = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(history_mapStateToProps)(history_HistoryPanelBody));
// EXTERNAL MODULE: ./src/components/scheduler/schedulerRow/style.less
var schedulerRow_style = __webpack_require__(39);

// CONCATENATED MODULE: ./src/components/scheduler/schedulerRow/index.jsx
function schedulerRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { schedulerRow_typeof = function _typeof(obj) { return typeof obj; }; } else { schedulerRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return schedulerRow_typeof(obj); }

function schedulerRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function schedulerRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function schedulerRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) schedulerRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) schedulerRow_defineProperties(Constructor, staticProps); return Constructor; }

function schedulerRow_possibleConstructorReturn(self, call) { if (call && (schedulerRow_typeof(call) === "object" || typeof call === "function")) { return call; } return schedulerRow_assertThisInitialized(self); }

function schedulerRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function schedulerRow_getPrototypeOf(o) { schedulerRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return schedulerRow_getPrototypeOf(o); }

function schedulerRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) schedulerRow_setPrototypeOf(subClass, superClass); }

function schedulerRow_setPrototypeOf(o, p) { schedulerRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return schedulerRow_setPrototypeOf(o, p); }









/*eslint-disable quotes*/

var schedulerRow_svgIcon = __webpack_require__(41).default;

var schedulerRow_svgIcon2 = __webpack_require__(10).default;

var schedulerRow_SchedulerRow =
/*#__PURE__*/
function (_Component) {
  schedulerRow_inherits(SchedulerRow, _Component);

  function SchedulerRow() {
    var _this;

    schedulerRow_classCallCheck(this, SchedulerRow);

    _this = schedulerRow_possibleConstructorReturn(this, schedulerRow_getPrototypeOf(SchedulerRow).call(this));
    _this.state = {
      panelOpened: -1
    };
    return _this;
  }

  schedulerRow_createClass(SchedulerRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "toggle",
    value: function toggle(index) {
      var _this2 = this;

      var props = this.props;

      if (props.settingsClientModified) {
        utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
          props.dispatch(task.cancelSettingsClientModified());

          _this2.toggleInternal(index);
        });
      } else {
        this.toggleInternal(index);
      }
    }
  }, {
    key: "toggleInternal",
    value: function toggleInternal(index) {
      if (this.props.openId !== "" && this.props.id === this.props.openId) {
        if (this.state.panelOpened !== index) {
          this.setState({
            panelOpened: index
          });
          this.props.OpenCollapse(this.props.id, index);
        } else {
          this.props.Collapse();
        }
      } else {
        this.setState({
          panelOpened: index
        });
        this.props.OpenCollapse(this.props.id, index);
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getEnabledDisplay",
    value: function getEnabledDisplay() {
      if (this.props.id !== "add") {
        if (this.props.enabled) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("div", {
              className: "checkMarkIcon",
              dangerouslySetInnerHTML: {
                __html: schedulerRow_svgIcon
              }
            })
          );
        } else return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", null, "\xA0 ")
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, "-")
      );
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "collapsible-component-scheduler" + (opened ? " row-opened" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "collapsible-header-scheduler " + !opened
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "row"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          title: props.name,
          className: "schedule-item item-row-name"
        }, props.name, "\xA0 "),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-frequency"
        }, props.frequency),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-retryTimeLapse"
        }, props.retryTimeLapse),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-nextStart"
        }, props.nextStart, "\xA0 "),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-enabled"
        }, this.getEnabledDisplay()), props.id !== "add" &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-historyButton"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: opened && props.panelIndex === 1 ? "history-icon-active" : "history-icon",
          title: resources.get("ControlTitle_history"),
          dangerouslySetInnerHTML: {
            __html: schedulerRow_svgIcon2
          },
          onClick: this.toggle.bind(this, 1)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-item item-row-editButton"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: opened && props.panelIndex === 0 ? "edit-icon-active" : "edit-icon",
          title: resources.get("ControlTitle_edit"),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
          },
          onClick: this.toggle.bind(this, 0)
        })))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          autoScroll: true,
          isOpened: opened,
          style: {
            float: "left",
            width: "100%"
          }
        }, opened && props.children))
      );
    }
  }]);

  return SchedulerRow;
}(external_window_dnn_nodeModules_React_["Component"]);

schedulerRow_SchedulerRow.propTypes = {
  scheduleId: prop_types_default.a.number,
  name: prop_types_default.a.string,
  frequency: prop_types_default.a.string,
  retryTimeLapse: prop_types_default.a.string,
  enabled: prop_types_default.a.bool,
  nextStart: prop_types_default.a.string,
  disabled: prop_types_default.a.bool,
  OpenCollapse: prop_types_default.a.func,
  Collapse: prop_types_default.a.func,
  id: prop_types_default.a.string,
  openId: prop_types_default.a.string,
  panelIndex: prop_types_default.a.number,
  settingsClientModified: prop_types_default.a.bool
};

function schedulerRow_mapStateToProps(state) {
  return {
    scheduleItemDetail: state.task.scheduleItemDetail,
    settingsClientModified: state.task.settingsClientModified
  };
}

schedulerRow_SchedulerRow.defaultProps = {
  collapsed: true
};
/* harmony default export */ var schedulerRow = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(schedulerRow_mapStateToProps)(schedulerRow_SchedulerRow));
// EXTERNAL MODULE: ./src/components/scheduler/schedulerEditor/style.less
var schedulerEditor_style = __webpack_require__(42);

// CONCATENATED MODULE: ./src/components/scheduler/schedulerEditor/index.jsx
function schedulerEditor_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { schedulerEditor_typeof = function _typeof(obj) { return typeof obj; }; } else { schedulerEditor_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return schedulerEditor_typeof(obj); }

function schedulerEditor_extends() { schedulerEditor_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return schedulerEditor_extends.apply(this, arguments); }

function schedulerEditor_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function schedulerEditor_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function schedulerEditor_createClass(Constructor, protoProps, staticProps) { if (protoProps) schedulerEditor_defineProperties(Constructor.prototype, protoProps); if (staticProps) schedulerEditor_defineProperties(Constructor, staticProps); return Constructor; }

function schedulerEditor_possibleConstructorReturn(self, call) { if (call && (schedulerEditor_typeof(call) === "object" || typeof call === "function")) { return call; } return schedulerEditor_assertThisInitialized(self); }

function schedulerEditor_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function schedulerEditor_getPrototypeOf(o) { schedulerEditor_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return schedulerEditor_getPrototypeOf(o); }

function schedulerEditor_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) schedulerEditor_setPrototypeOf(subClass, superClass); }

function schedulerEditor_setPrototypeOf(o, p) { schedulerEditor_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return schedulerEditor_setPrototypeOf(o, p); }










var re = /^[1-9][0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$/;
var retainHistoryNumOptions = [];
var timeLapseMeasurementOptions = [];
var attachToEventOptions = [];
var catchUpEnabledOptions = [];

var schedulerEditor_SchedulerEditor =
/*#__PURE__*/
function (_Component) {
  schedulerEditor_inherits(SchedulerEditor, _Component);

  function SchedulerEditor() {
    var _this;

    schedulerEditor_classCallCheck(this, SchedulerEditor);

    _this = schedulerEditor_possibleConstructorReturn(this, schedulerEditor_getPrototypeOf(SchedulerEditor).call(this));
    _this.state = {
      error: {
        name: true,
        frequency: true,
        retry: false
      },
      triedToSubmit: false
    };
    return _this;
  }

  schedulerEditor_createClass(SchedulerEditor, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;

      if (props.scheduleId) {
        props.dispatch(task.getGetScheduleItem({
          scheduleId: props.scheduleId
        }));
      }

      retainHistoryNumOptions = [];
      retainHistoryNumOptions.push({
        "value": "0",
        "label": resources.get("None")
      });
      retainHistoryNumOptions.push({
        "value": "1",
        "label": "1"
      });
      retainHistoryNumOptions.push({
        "value": "5",
        "label": "5"
      });
      retainHistoryNumOptions.push({
        "value": "10",
        "label": "10"
      });
      retainHistoryNumOptions.push({
        "value": "25",
        "label": "25"
      });
      retainHistoryNumOptions.push({
        "value": "50",
        "label": "50"
      });
      retainHistoryNumOptions.push({
        "value": "60",
        "label": "60"
      });
      retainHistoryNumOptions.push({
        "value": "100",
        "label": "100"
      });
      retainHistoryNumOptions.push({
        "value": "250",
        "label": "250"
      });
      retainHistoryNumOptions.push({
        "value": "500",
        "label": "500"
      });
      retainHistoryNumOptions.push({
        "value": "-1",
        "label": resources.get("All")
      });

      if (timeLapseMeasurementOptions.length === 0) {
        timeLapseMeasurementOptions.push({
          "value": "s",
          "label": resources.get("Seconds")
        });
        timeLapseMeasurementOptions.push({
          "value": "m",
          "label": resources.get("Minutes")
        });
        timeLapseMeasurementOptions.push({
          "value": "h",
          "label": resources.get("Hours")
        });
        timeLapseMeasurementOptions.push({
          "value": "d",
          "label": resources.get("Days")
        });
        timeLapseMeasurementOptions.push({
          "value": "w",
          "label": resources.get("Weeks")
        });
        timeLapseMeasurementOptions.push({
          "value": "mo",
          "label": resources.get("Months")
        });
        timeLapseMeasurementOptions.push({
          "value": "y",
          "label": resources.get("Years")
        });
      }

      if (attachToEventOptions.length === 0) {
        attachToEventOptions.push({
          "value": "",
          "label": resources.get("None")
        });
        attachToEventOptions.push({
          "value": "APPLICATION_START",
          "label": "APPLICATION_START"
        });
      }

      if (catchUpEnabledOptions.length === 0) {
        catchUpEnabledOptions.push({
          "value": "false",
          "label": resources.get("Disabled")
        });
        catchUpEnabledOptions.push({
          "value": "true",
          "label": resources.get("Enabled.Label")
        });
      }

      this.setState({
        triedToSubmit: false,
        error: {}
      });
    }
  }, {
    key: "runSchedule",
    value: function runSchedule() {
      var props = this.props;
      var scheduleItemDetail = props.scheduleItemDetail;
      props.dispatch(task.runScheduleItem(scheduleItemDetail, function () {
        utils["a" /* default */].utilities.notify(resources.get("RunNow"));
      }, function () {
        utils["a" /* default */].utilities.notify(resources.get("RunNowError"));
      }));
    }
  }, {
    key: "isEmptyDate",
    value: function isEmptyDate(date) {
      return !date || new Date(date).getFullYear() < 1970;
    }
  }, {
    key: "getDefaultIfNull",
    value: function getDefaultIfNull(obj, key, defaultValue) {
      if (obj && key && obj[key]) {
        return obj[key].toString();
      }

      return defaultValue;
    }
  }, {
    key: "getValue",
    value: function getValue(selectKey) {
      var props = this.props;

      switch (selectKey) {
        case "FriendlyName":
          return this.getDefaultIfNull(props.scheduleItemDetail, "FriendlyName", "");

        case "TypeFullName":
          return this.getDefaultIfNull(props.scheduleItemDetail, "TypeFullName", "");

        case "RetainHistoryNum":
          return this.getDefaultIfNull(props.scheduleItemDetail, "RetainHistoryNum", "0");

        case "Servers":
          return this.getDefaultIfNull(props.scheduleItemDetail, "Servers", "");

        case "ObjectDependencies":
          return this.getDefaultIfNull(props.scheduleItemDetail, "ObjectDependencies", "");

        case "ScheduleStartDate":
          if (!this.isEmptyDate(this.getDefaultIfNull(props.scheduleItemDetail, "ScheduleStartDate", null))) {
            return new Date(props.scheduleItemDetail.ScheduleStartDate);
          } else {
            return null;
          }

        case "TimeLapse":
          return this.getDefaultIfNull(props.scheduleItemDetail, "TimeLapse", "");

        case "TimeLapseMeasurement":
          return this.getDefaultIfNull(props.scheduleItemDetail, "TimeLapseMeasurement", "s");

        case "RetryTimeLapse":
          return this.getDefaultIfNull(props.scheduleItemDetail, "RetryTimeLapse", "");

        case "RetryTimeLapseMeasurement":
          return this.getDefaultIfNull(props.scheduleItemDetail, "RetryTimeLapseMeasurement", "s");

        case "AttachToEvent":
          return this.getDefaultIfNull(props.scheduleItemDetail, "AttachToEvent", "");

        case "CatchUpEnabled":
          return this.getDefaultIfNull(props.scheduleItemDetail, "CatchUpEnabled", "false");

        case "Enabled":
          return this.getDefaultIfNull(props.scheduleItemDetail, "Enabled", "false");

        default:
          break;
      }
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;

      var scheduleItemDetail = schedulerEditor_extends({}, props.scheduleItemDetail);

      if (key === "ScheduleStartDate") {
        scheduleItemDetail[key] = event;
      } else if (key === "CatchUpEnabled") {
        scheduleItemDetail[key] = event.value === "true" ? true : false;
      } else if (key === "RetainHistoryNum" || key === "TimeLapseMeasurement" || key === "RetryTimeLapseMeasurement" || key === "AttachToEvent") {
        scheduleItemDetail[key] = event.value;
      } else {
        scheduleItemDetail[key] = schedulerEditor_typeof(event) === "object" ? event.target.value : event;
      }

      if (scheduleItemDetail[key] === "" && key === "TypeFullName") {
        state.error["name"] = true;
      } else if (scheduleItemDetail[key] !== "" && key === "TypeFullName") {
        state.error["name"] = false;
      }

      if ((scheduleItemDetail[key] === "" || !re.test(scheduleItemDetail[key])) && key === "TimeLapse") {
        state.error["frequency"] = true;
      } else if (scheduleItemDetail[key] !== "" && re.test(scheduleItemDetail[key]) && key === "TimeLapse") {
        state.error["frequency"] = false;
      }

      if (scheduleItemDetail[key] === "" && key === "RetryTimeLapse") {
        state.error["retry"] = false;
      } else if (key === "RetryTimeLapse") {
        if (!re.test(scheduleItemDetail[key])) {
          state.error["retry"] = true;
        } else {
          state.error["retry"] = false;
        }
      }

      this.setState({
        scheduleItemDetail: scheduleItemDetail,
        triedToSubmit: false,
        error: state.error
      });
      props.dispatch(task.settingsClientModified(scheduleItemDetail));
    }
  }, {
    key: "onUpdateItem",
    value: function onUpdateItem(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.name || state.error.frequency || state.error.retry) {
        return;
      }

      this.handleRecommendedServers(function () {
        return props.onUpdate(props.scheduleItemDetail);
      });
    }
  }, {
    key: "compareIgnoreCase",
    value: function compareIgnoreCase(prev, next) {
      return prev.toLowerCase().localeCompare(next.toLowerCase());
    }
  }, {
    key: "uniqueIgnoreCase",
    value: function uniqueIgnoreCase(value, index, self) {
      var lower = self.map(function (x) {
        return (x || "").toLowerCase();
      });
      return lower.indexOf(value.toLowerCase()) === index;
    }
  }, {
    key: "normalizeServers",
    value: function normalizeServers(commaSeparatedServers) {
      return (commaSeparatedServers || "").split(",").map(function (x) {
        return x.trim();
      }).filter(function (x) {
        return !!x;
      }).filter(this.uniqueIgnoreCase).sort(this.compareIgnoreCase).join(", ");
    }
  }, {
    key: "onServersBlur",
    value: function onServersBlur() {
      var scheduleItemDetail = this.props.scheduleItemDetail;
      this.onSettingChange("Servers", {
        target: {
          value: this.normalizeServers(scheduleItemDetail.Servers)
        }
      });
    }
  }, {
    key: "handleRecommendedServers",
    value: function handleRecommendedServers(callback) {
      var scheduleItemDetail = this.props.scheduleItemDetail;
      var recommendedServers = scheduleItemDetail.RecommendedServers || [];

      var proceed = function proceed() {
        if (callback && typeof callback === "function") {
          callback();
        }
      };

      if (scheduleItemDetail.Enabled && recommendedServers.length > 0) {
        var servers = this.normalizeServers(scheduleItemDetail.Servers);
        var recommended = this.normalizeServers(recommendedServers.join(", "));

        if (this.compareIgnoreCase(servers, recommended) !== 0) {
          var message = utils["a" /* default */].formatString(resources.get("RecommendServers"), recommended);
          var yes = resources.get("Yes");
          var no = resources.get("No");
          utils["a" /* default */].utilities.confirm(message, yes, no, function () {
            return proceed();
          });
        } else {
          proceed();
        }
      } else {
        proceed();
      }
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var props = this.props;

      if (props.settingsClientModified) {
        utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
          props.dispatch(task.cancelSettingsClientModified());
          props.Collapse();
        });
      } else {
        props.Collapse();
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;

      if (props.panelIndex === 1) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_history, {
            pageSize: 5,
            scheduleId: props.scheduleId,
            title: resources.get("HistoryModalTitle")
          })
        );
      }

      if (props.scheduleItemDetail !== undefined || props.id === "add") {
        var columnOne =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "container",
          key: "columnOne"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            width: 100 + "%",
            float: "left"
          },
          label: resources.get("plFriendlyName"),
          error: false,
          value: this.getValue("FriendlyName" || false),
          onChange: this.onSettingChange.bind(this, "FriendlyName")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            width: 100 + "%",
            float: "left"
          },
          label: resources.get("plType") + " *",
          error: state.error.name && state.triedToSubmit,
          errorMessage: resources.get("TypeRequired"),
          value: this.getValue("TypeFullName") || "",
          onChange: this.onSettingChange.bind(this, "TypeFullName")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: resources.get("plRetainHistoryNum"),
          style: {
            margin: "0 0 5px 0"
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: retainHistoryNumOptions,
          value: this.getValue("RetainHistoryNum"),
          onSelect: this.onSettingChange.bind(this, "RetainHistoryNum")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            width: 100 + "%",
            float: "left"
          },
          label: resources.get("Servers"),
          error: false,
          value: this.getValue("Servers") || "",
          onBlur: this.onServersBlur.bind(this),
          onChange: this.onSettingChange.bind(this, "Servers")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            width: 100 + "%",
            float: "left"
          },
          label: resources.get("plObjectDependencies"),
          error: false,
          value: this.getValue("ObjectDependencies") || "",
          onChange: this.onSettingChange.bind(this, "ObjectDependencies")
        })));
        var columnTwo =
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "container right-column",
          key: "columnTwo"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: resources.get("plScheduleStartDate"),
          style: {
            margin: "0 0 5px 0"
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DatePicker"], {
          date: this.getValue("ScheduleStartDate"),
          updateDate: this.onSettingChange.bind(this, "ScheduleStartDate"),
          isDateRange: false,
          hasTimePicker: true,
          showClearDateButton: true
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            float: "left",
            width: "47.5%",
            whiteSpace: "pre"
          },
          label: resources.get("plTimeLapse") + " *",
          error: state.error.frequency && state.triedToSubmit,
          errorMessage: resources.get("TimeLapseRequired.ErrorMessage"),
          value: this.getValue("TimeLapse") || "",
          onChange: this.onSettingChange.bind(this, "TimeLapse")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "text-section"
        }, "\xA0 "),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          style: {
            width: 46 + "%",
            float: "right",
            margin: "25px 0 0 0"
          },
          options: timeLapseMeasurementOptions,
          value: this.getValue("TimeLapseMeasurement"),
          onSelect: this.onSettingChange.bind(this, "TimeLapseMeasurement")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          withLabel: true,
          style: {
            width: "47.5%",
            float: "left",
            whiteSpace: "pre"
          },
          label: resources.get("plRetryTimeLapse"),
          error: state.error.retry && state.triedToSubmit,
          errorMessage: resources.get("RetryTimeLapseValidator.ErrorMessage"),
          value: this.getValue("RetryTimeLapse") === "-1" ? "" : this.getValue("RetryTimeLapse"),
          onChange: this.onSettingChange.bind(this, "RetryTimeLapse")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "text-section"
        }, "\xA0 "),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          style: {
            width: 46 + "%",
            float: "right",
            margin: "25px 0 0 0"
          },
          options: timeLapseMeasurementOptions,
          value: this.getValue("RetryTimeLapseMeasurement"),
          onSelect: this.onSettingChange.bind(this, "RetryTimeLapseMeasurement")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: resources.get("plAttachToEvent"),
          style: {
            margin: "0 0 5px 0"
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: attachToEventOptions,
          value: this.getValue("AttachToEvent"),
          onSelect: this.onSettingChange.bind(this, "AttachToEvent")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: resources.get("plCatchUpEnabled"),
          style: {
            margin: "0 0 5px 0"
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: catchUpEnabledOptions,
          value: this.getValue("CatchUpEnabled"),
          onSelect: this.onSettingChange.bind(this, "CatchUpEnabled")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: resources.get("plEnabled"),
          style: {
            width: "47.5%"
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "right"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          onText: resources.get("SwitchOn"),
          offText: resources.get("SwitchOff"),
          value: this.getValue("Enabled") === "true" ? true : false,
          onChange: this.onSettingChange.bind(this, "Enabled")
        }))));
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "scheduler-setting-editor"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "scheduler-item-container"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "scheduler-item-column"
          }, columnOne),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "scheduler-item-column"
          }, columnTwo)),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "buttons-box"
          }, props.scheduleId !== undefined &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "secondary",
            onClick: props.onDelete.bind(this, props.scheduleId)
          }, resources.get("cmdDelete")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, resources.get("Cancel")), props.scheduleId !== undefined && props.scheduleItemDetail.Enabled && props.enabled &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "secondary",
            onClick: this.runSchedule.bind(this)
          }, resources.get("cmdRun")),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            disabled: !props.settingsClientModified,
            type: "primary",
            onClick: this.onUpdateItem.bind(this)
          }, props.scheduleId !== undefined ? resources.get("Update") : resources.get("cmdSave"))))
        );
      } else return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null)
      );
    }
  }]);

  return SchedulerEditor;
}(external_window_dnn_nodeModules_React_["Component"]);

schedulerEditor_SchedulerEditor.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  scheduleItemDetail: prop_types_default.a.object,
  enabled: prop_types_default.a.bool,
  serverList: prop_types_default.a.array,
  scheduleId: prop_types_default.a.number,
  Collapse: prop_types_default.a.func,
  onDelete: prop_types_default.a.func,
  onUpdate: prop_types_default.a.func,
  id: prop_types_default.a.string,
  settingsClientModified: prop_types_default.a.bool,
  panelIndex: prop_types_default.a.number
};

function schedulerEditor_mapStateToProps(state) {
  return {
    scheduleItemDetail: state.task.scheduleItemDetail,
    settingsClientModified: state.task.settingsClientModified
  };
}

/* harmony default export */ var schedulerEditor = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(schedulerEditor_mapStateToProps)(schedulerEditor_SchedulerEditor));
// EXTERNAL MODULE: ./src/components/scheduler/style.less
var scheduler_style = __webpack_require__(44);

// CONCATENATED MODULE: ./src/components/scheduler/index.jsx
function scheduler_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { scheduler_typeof = function _typeof(obj) { return typeof obj; }; } else { scheduler_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return scheduler_typeof(obj); }

function scheduler_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function scheduler_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function scheduler_createClass(Constructor, protoProps, staticProps) { if (protoProps) scheduler_defineProperties(Constructor.prototype, protoProps); if (staticProps) scheduler_defineProperties(Constructor, staticProps); return Constructor; }

function scheduler_possibleConstructorReturn(self, call) { if (call && (scheduler_typeof(call) === "object" || typeof call === "function")) { return call; } return scheduler_assertThisInitialized(self); }

function scheduler_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function scheduler_getPrototypeOf(o) { scheduler_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return scheduler_getPrototypeOf(o); }

function scheduler_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) scheduler_setPrototypeOf(subClass, superClass); }

function scheduler_setPrototypeOf(o, p) { scheduler_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return scheduler_setPrototypeOf(o, p); }











var scheduler_tableFields = [];

var scheduler_SchedulerPanel =
/*#__PURE__*/
function (_Component) {
  scheduler_inherits(SchedulerPanel, _Component);

  function SchedulerPanel() {
    var _this;

    scheduler_classCallCheck(this, SchedulerPanel);

    _this = scheduler_possibleConstructorReturn(this, scheduler_getPrototypeOf(SchedulerPanel).call(this));
    _this.state = {
      currentServerId: "*",
      schedulerItemList: [],
      openId: "",
      serverList: [],
      historyPanelOpen: false
    };
    return _this;
  }

  scheduler_createClass(SchedulerPanel, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.dispatch(task.getSchedulerItemList());

      if (props.serverList === null || props.serverList === [] || props.serverList === undefined) {
        props.dispatch(task.getServerList());
      }

      scheduler_tableFields = [];
      scheduler_tableFields.push({
        "name": resources.get("Name.Header"),
        "id": "FriendlyName"
      });
      scheduler_tableFields.push({
        "name": resources.get("Frequency.Header"),
        "id": "Frequency"
      });
      scheduler_tableFields.push({
        "name": resources.get("RetryTimeLapse.Header"),
        "id": "RetryTimeLapse"
      });
      scheduler_tableFields.push({
        "name": resources.get("NextStart.Header"),
        "id": "NextStart"
      });
      scheduler_tableFields.push({
        "name": resources.get("Enabled.Header"),
        "id": "Enabled"
      });
    }
  }, {
    key: "createServerOptions",
    value: function createServerOptions(serverList) {
      var serverOptions = [];

      if (serverList !== undefined) {
        serverOptions = serverList.map(function (item) {
          return {
            label: item.ServerName,
            value: item.ServerID
          };
        });
      }

      return serverOptions;
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableHeaders = scheduler_tableFields.map(function (field) {
        var className = "header-" + field.id;
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: className,
            key: "header-" + field.id
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", null, field.name, "\xA0 "))
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
    key: "uncollapse",
    value: function uncollapse(id, index) {
      this.setState({
        openId: id,
        historyPanelOpen: index === 1
      });
    }
  }, {
    key: "collapse",
    value: function collapse(index) {
      if (this.state.openId !== "") {
        this.setState({
          openId: "",
          historyPanelOpen: index !== 1
        });
      }
    }
  }, {
    key: "toggle",
    value: function toggle(openId, index) {
      if (openId !== "") {
        this.uncollapse(openId, index);
      }
    }
  }, {
    key: "onSelectServer",
    value: function onSelectServer(event) {
      var props = this.props;
      var state = this.state;

      var _ref = event || {},
          value = _ref.value,
          label = _ref.label;

      var server = label;
      var serverId = value;

      if (server && serverId !== state.currentServerId) {
        state.currentServerId = serverId;
        this.setState({
          currentServerId: state.currentServerId
        }, function () {
          props.dispatch(task.getSchedulerItemList({
            serverName: server
          }));
        });
      }
    }
  }, {
    key: "onUpdateSchedulerItem",
    value: function onUpdateSchedulerItem(scheduleItemDetail) {
      var _this2 = this;

      var props = this.props;

      if (scheduleItemDetail.ScheduleID) {
        props.dispatch(task.updateScheduleItem(scheduleItemDetail, function () {
          utils["a" /* default */].utilities.notify(resources.get("ScheduleItemUpdateSuccess"));

          _this2.collapse();

          props.dispatch(task.getSchedulerItemList());
        }, function (error) {
          var errorMessage = JSON.parse(error.responseText);
          utils["a" /* default */].utilities.notifyError(errorMessage.Message);
        }));
      } else {
        props.dispatch(task.createScheduleItem(scheduleItemDetail, function () {
          utils["a" /* default */].utilities.notify(resources.get("ScheduleItemCreateSuccess"));

          _this2.collapse();

          props.dispatch(task.getSchedulerItemList());
        }, function (error) {
          utils["a" /* default */].utilities.notify(resources.get("ScheduleItemCreateError"));
          var errorMessage = JSON.parse(error.responseText);
          utils["a" /* default */].utilities.notifyError(errorMessage.Message);
        }));
      }
    }
  }, {
    key: "onDeleteSchedulerItem",
    value: function onDeleteSchedulerItem(scheduleId) {
      var _this3 = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(resources.get("ScheduleItemDeletedWarning"), resources.get("Yes"), resources.get("No"), function () {
        var itemList = props.schedulerItemList.filter(function (item) {
          return item.ScheduleID !== scheduleId;
        });
        props.dispatch(task.deleteSchedule({
          "ScheduleId": scheduleId
        }, itemList, function () {
          utils["a" /* default */].utilities.notify(resources.get("DeleteSuccess"));

          _this3.collapse();
        }, function () {
          utils["a" /* default */].utilities.notify(resources.get("DeleteError"));
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderedScedulerItemList",
    value: function renderedScedulerItemList() {
      var _this4 = this;

      var i = 0;

      if (this.props.schedulerItemList) {
        return this.props.schedulerItemList.map(function (item, index) {
          var id = "row-" + i++;
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(schedulerRow, {
              scheduleId: item.ScheduleID,
              name: item.FriendlyName,
              frequency: item.Frequency,
              retryTimeLapse: item.RetryTimeLapse,
              nextStart: item.NextStart,
              enabled: item.Enabled,
              index: index,
              key: "scheduleItem-" + index,
              closeOnClick: true,
              openId: _this4.state.openId,
              OpenCollapse: _this4.toggle.bind(_this4),
              Collapse: _this4.collapse.bind(_this4),
              id: id,
              panelIndex: _this4.state.historyPanelOpen ? 1 : 0
            },
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(schedulerEditor, {
              serverList: _this4.props.serverList,
              enabled: item.Enabled,
              scheduleId: item.ScheduleID,
              Collapse: _this4.collapse.bind(_this4),
              onDelete: _this4.onDeleteSchedulerItem.bind(_this4),
              onUpdate: _this4.onUpdateSchedulerItem.bind(_this4),
              id: id,
              openId: _this4.state.openId,
              panelIndex: _this4.state.historyPanelOpen ? 1 : 0
            }))
          );
        });
      }
    }
  }, {
    key: "render",
    value: function render() {
      var opened = this.state.openId === "add";
      var serverOptions = this.createServerOptions(this.props.serverList);
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-items"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "servergroup-filter-container"
        }, serverOptions.length > 2 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: serverOptions,
          style: {
            width: "100%"
          },
          withBorder: false,
          value: this.state.currentServerId,
          onSelect: this.onSelectServer.bind(this)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "AddItemRow"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: opened ? "AddItemBox-active" : "AddItemBox",
          onClick: this.toggle.bind(this, opened ? "" : "add")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "add-icon",
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].AddIcon
          }
        }), " ", resources.get("cmdAddTask"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "schedule-items-grid"
        }, this.renderHeader(),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          isOpened: opened,
          style: {
            float: "left",
            width: "100%"
          },
          fixedHeight: 650
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(schedulerRow, {
          name: "-",
          frequency: "-",
          retryTimeLapse: "-",
          nextStart: "-",
          index: "add",
          key: "scheduleItem-add",
          closeOnClick: true,
          openId: this.state.openId,
          OpenCollapse: this.toggle.bind(this),
          Collapse: this.collapse.bind(this),
          id: "add"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(schedulerEditor, {
          serverList: this.props.serverList,
          Collapse: this.collapse.bind(this),
          onDelete: this.onDeleteSchedulerItem.bind(this),
          onUpdate: this.onUpdateSchedulerItem.bind(this),
          id: "add",
          openId: this.state.openId
        }))), this.renderedScedulerItemList())))
      );
    }
  }]);

  return SchedulerPanel;
}(external_window_dnn_nodeModules_React_["Component"]);

scheduler_SchedulerPanel.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  schedulerItemList: prop_types_default.a.array,
  serverList: prop_types_default.a.array
};

function scheduler_mapStateToProps(state) {
  return {
    schedulerItemList: state.task.schedulerItemList,
    serverList: state.task.serverList,
    tabIndex: state.pagination.tabIndex
  };
}

/* harmony default export */ var scheduler = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(scheduler_mapStateToProps)(scheduler_SchedulerPanel));
// EXTERNAL MODULE: ./src/components/topPane/style.less
var topPane_style = __webpack_require__(21);
var topPane_style_default = /*#__PURE__*/__webpack_require__.n(topPane_style);

// EXTERNAL MODULE: ./src/components/topPane/modePanel/style.less
var modePanel_style = __webpack_require__(47);

// CONCATENATED MODULE: ./src/components/topPane/modePanel/index.jsx
function modePanel_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { modePanel_typeof = function _typeof(obj) { return typeof obj; }; } else { modePanel_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return modePanel_typeof(obj); }

function modePanel_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function modePanel_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function modePanel_createClass(Constructor, protoProps, staticProps) { if (protoProps) modePanel_defineProperties(Constructor.prototype, protoProps); if (staticProps) modePanel_defineProperties(Constructor, staticProps); return Constructor; }

function modePanel_possibleConstructorReturn(self, call) { if (call && (modePanel_typeof(call) === "object" || typeof call === "function")) { return call; } return modePanel_assertThisInitialized(self); }

function modePanel_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function modePanel_getPrototypeOf(o) { modePanel_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return modePanel_getPrototypeOf(o); }

function modePanel_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) modePanel_setPrototypeOf(subClass, superClass); }

function modePanel_setPrototypeOf(o, p) { modePanel_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return modePanel_setPrototypeOf(o, p); }









var modePanel_re = /^([0-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-4][0-4][0])$/;

var modePanel_ModePanel =
/*#__PURE__*/
function (_Component) {
  modePanel_inherits(ModePanel, _Component);

  function ModePanel() {
    var _this;

    modePanel_classCallCheck(this, ModePanel);

    _this = modePanel_possibleConstructorReturn(this, modePanel_getPrototypeOf(ModePanel).call(this));
    _this.state = {
      updateRequest: {
        SchedulerMode: "",
        SchedulerdelayAtAppStart: ""
      },
      error: {
        schedulerDelay: false
      },
      triedToSubmit: false,
      clientModified: false
    };
    return _this;
  }

  modePanel_createClass(ModePanel, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props,
          state = this.state;

      if (props.schedulerDelay === "" || !modePanel_re.test(props.schedulerDelay)) {
        state.error["schedulerDelay"] = true;
      }

      this.setState({
        triedToSubmit: false,
        error: state.error,
        updateRequest: {
          SchedulerMode: props.schedulerMode,
          SchedulerdelayAtAppStart: props.schedulerDelay
        }
      });
    }
  }, {
    key: "onSave",
    value: function onSave(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.schedulerDelay) {
        return;
      }

      props.dispatch(task.updateSchedulerSettings(state.updateRequest, function () {
        utils["a" /* default */].utilities.notify(resources.get("SchedulerUpdateSuccess"));
      }, function () {
        utils["a" /* default */].utilities.notify(resources.get("SchedulerUpdateError"));
      }));
      this.setState({
        clientModified: false
      });
      props.onClose();
    }
  }, {
    key: "onValueChange",
    value: function onValueChange(key, event) {
      var state = this.state;
      var value = event.target.value;
      var updateRequest = this.state.updateRequest;
      updateRequest[key] = value;

      if (!modePanel_re.test(updateRequest[key]) && key === "SchedulerdelayAtAppStart") {
        state.error["schedulerDelay"] = true;
      } else if (modePanel_re.test(updateRequest[key]) && key === "SchedulerdelayAtAppStart") {
        state.error["schedulerDelay"] = false;
      }

      this.setState({
        triedToSubmit: false,
        updateRequest: updateRequest,
        error: state.error,
        clientModified: true
      });
    }
  }, {
    key: "onClose",
    value: function onClose() {
      var props = this.props,
          state = this.state;
      state.error["schedulerDelay"] = false;
      this.setState({
        error: state.error,
        clientModified: false
      });
      props.onClose();
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: props.isOpened ? "collapsible-content-mode" : "collapsible-content-mode-hidden"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          fixedHeight: props.fixedHeight,
          keepCollapsedContent: props.keepCollapsedContent,
          isOpened: props.isOpened
        }, props.fixedHeight &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "modepanel-content-wrapper",
          style: {
            height: "calc(100% - 100px)"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: ""
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("label", null, resources.get("plSchedulerMode")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Select"], {
          onChange: this.onValueChange.bind(this, "SchedulerMode"),
          options: props.schedulerModeOptions,
          value: state.updateRequest.SchedulerMode
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("label", null, resources.get("plScheduleAppStartDelay")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          inputStyle: {
            margin: "0"
          },
          withLabel: false,
          error: this.state.error.schedulerDelay && this.state.triedToSubmit,
          errorMessage: resources.get("ScheduleAppStartDelayValidation"),
          value: state.updateRequest.SchedulerdelayAtAppStart,
          onChange: this.onValueChange.bind(this, "SchedulerdelayAtAppStart")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "action-buttons"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onClose.bind(this)
        }, resources.get("Cancel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          disabled: !state.clientModified,
          onClick: this.onSave.bind(this)
        }, resources.get("Update")))))), !props.fixedHeight && props.children))
      );
    }
  }]);

  return ModePanel;
}(external_window_dnn_nodeModules_React_["Component"]);

modePanel_ModePanel.propTypes = {
  label: prop_types_default.a.string,
  fixedHeight: prop_types_default.a.number,
  collapsibleWidth: prop_types_default.a.number,
  collapsibleHeight: prop_types_default.a.number,
  keepCollapsedContent: prop_types_default.a.bool,
  scrollAreaStyle: prop_types_default.a.object,
  children: prop_types_default.a.node,
  isOpened: prop_types_default.a.bool,
  onClose: prop_types_default.a.func.isRequired,
  schedulerMode: prop_types_default.a.string.isRequired,
  schedulerDelay: prop_types_default.a.number.isRequired,
  schedulerModeOptions: prop_types_default.a.array.isRequired,
  clientModified: prop_types_default.a.bool
};

function modePanel_mapStateToProps(state) {
  return {
    schedulerDelay: state.task.schedulerDelay,
    schedulerMode: state.task.schedulerMode,
    clientModified: state.task.settingsClientModified
  };
}

/* harmony default export */ var modePanel = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(modePanel_mapStateToProps)(modePanel_ModePanel));
// CONCATENATED MODULE: ./src/components/topPane/index.jsx
function topPane_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { topPane_typeof = function _typeof(obj) { return typeof obj; }; } else { topPane_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return topPane_typeof(obj); }

function topPane_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function topPane_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function topPane_createClass(Constructor, protoProps, staticProps) { if (protoProps) topPane_defineProperties(Constructor.prototype, protoProps); if (staticProps) topPane_defineProperties(Constructor, staticProps); return Constructor; }

function topPane_possibleConstructorReturn(self, call) { if (call && (topPane_typeof(call) === "object" || typeof call === "function")) { return call; } return topPane_assertThisInitialized(self); }

function topPane_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function topPane_getPrototypeOf(o) { topPane_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return topPane_getPrototypeOf(o); }

function topPane_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) topPane_setPrototypeOf(subClass, superClass); }

function topPane_setPrototypeOf(o, p) { topPane_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return topPane_setPrototypeOf(o, p); }










/*eslint-disable quotes*/

var topPane_svgIcon = __webpack_require__(49).default;

var topPane_TopPane =
/*#__PURE__*/
function (_Component) {
  topPane_inherits(TopPane, _Component);

  function TopPane() {
    var _this;

    topPane_classCallCheck(this, TopPane);

    _this = topPane_possibleConstructorReturn(this, topPane_getPrototypeOf(TopPane).call(this));
    _this.state = {
      modePanelOpen: false
    };
    return _this;
  }

  topPane_createClass(TopPane, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      var persistedSettings = utils["a" /* default */].utilities.persistent.load();
      props.dispatch(task.getSchedulerSettings());
      props.dispatch(task.getTaskStatusList());
      this.taskListTimeout = setInterval(function () {
        if (persistedSettings.expandPersonaBar && persistedSettings.activeIdentifier === "Dnn.TaskScheduler") {
          props.dispatch(task.getTaskStatusList());
        }
      }, 5000);
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      clearInterval(this.taskListTimeout);
    }
  }, {
    key: "onClick",
    value: function onClick() {
      var props = this.props;

      if (this.isStopped()) {
        props.dispatch(task.startSchedule(function () {
          utils["a" /* default */].utilities.notify(resources.get("SchedulerStartSuccess"));
          props.dispatch(task.getTaskStatusList());
        }, function () {
          utils["a" /* default */].utilities.notify(resources.get("SchedulerStartError"));
        }));
      } else {
        props.dispatch(task.stopSchedule(function () {
          utils["a" /* default */].utilities.notify(resources.get("SchedulerStopSuccess"));
          props.dispatch(task.getTaskStatusList());
        }, function () {
          utils["a" /* default */].utilities.notify(resources.get("SchedulerStopError"));
        }));
      }
    }
  }, {
    key: "getButtonDisplay",
    value: function getButtonDisplay() {
      if (this.isStopped() || this.props.schedulerMode === "2") {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
            text: resources.get("StartSchedule"),
            maxWidth: 100
          })
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
            text: resources.get("StopSchedule"),
            maxWidth: 100
          })
        );
      }
    }
  }, {
    key: "isStopped",
    value: function isStopped() {
      var props = this.props;

      if (props.status === "STOPPED") {
        return true;
      } else {
        return false;
      }
    }
  }, {
    key: "getschedulerModeDisplay",
    value: function getschedulerModeDisplay() {
      var props = this.props;

      if (props.schedulerMode) {
        return props.schedulerModeOptions[parseInt(props.schedulerMode)].Key;
      }

      return "";
    }
  }, {
    key: "toggleModePanel",
    value: function toggleModePanel() {
      if (this.props.status !== "SHUTTING_DOWN") {
        this.setState({
          modePanelOpen: !this.state.modePanelOpen
        });
      } else {
        if (this.state.modePanelOpen) {
          this.setState({
            modePanelOpen: !this.state.modePanelOpen
          });
        }
      }
    }
  }, {
    key: "getModeOptions",
    value: function getModeOptions() {
      var props = this.props;

      if (props.schedulerModeOptions) {
        var options = props.schedulerModeOptions.map(function (mode) {
          return {
            value: mode.Value,
            label: mode.Key
          };
        });
        return options;
      } else return [{
        value: "",
        label: ""
      }];
    }
  }, {
    key: "renderButton",
    value: function renderButton() {
      var props = this.props;

      if (props.status !== "SHUTTING_DOWN") {
        if (props.schedulingEnabled === "True") {
          if (props.schedulerMode === "2") {
            return (
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
                type: "primary",
                disabled: true
              }, this.getButtonDisplay())
            );
          } else {
            return (
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
                className: props.status !== "STOPPED" ? "topPane-button-start" : "",
                type: props.status !== "STOPPED" ? "secondary" : "primary",
                onClick: this.onClick.bind(this)
              }, this.getButtonDisplay())
            );
          }
        } else {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
              type: "primary",
              disabled: true
            }, this.getButtonDisplay())
          );
        }
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
            type: "primary",
            disabled: true
          }, this.getButtonDisplay())
        );
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: topPane_style_default.a.topPaneWrapper
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-time"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-time-name"
        }, resources.get("ServerTime")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-time-value"
        }, props.serverTime)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-left"
        }, resources.get("lblStatusLabel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: props.status === "STOPPED" ? "topPane-middle-name-stopped" : "topPane-middle-name"
        }, resources.get(props.status)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common-title",
          style: {
            maxWidth: "60%"
          }
        }, resources.get("plSchedulerMode")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: props.status === "SHUTTING_DOWN" ? "editIconDisabled" : "editIcon",
          dangerouslySetInnerHTML: {
            __html: topPane_svgIcon
          },
          onClick: this.toggleModePanel.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "collapsible-content"
        }, props.schedulerDelay &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(modePanel, {
          fixedHeight: 200,
          isOpened: state.modePanelOpen,
          onClose: this.toggleModePanel.bind(this),
          schedulerDelay: props.schedulerDelay,
          schedulerModeOptions: this.getModeOptions(),
          schedulerMode: props.schedulerMode
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common-value"
        }, this.getschedulerModeDisplay())),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common-title"
        }, resources.get("lblStartDelay")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-middle-common-value"
        }, props.schedulerDelay))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-title"
        }, resources.get("lblMaxThreadsLabel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-value"
        }, props.maxThreads)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-title"
        }, resources.get("lblActiveThreadsLabel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-value"
        }, props.activeThreads)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-title"
        }, resources.get("lblFreeThreadsLabel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-right-common-value"
        }, props.freeThreads))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "topPane-button"
        }, this.renderButton())))
      );
    }
  }]);

  return TopPane;
}(external_window_dnn_nodeModules_React_["Component"]);

topPane_TopPane.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  status: prop_types_default.a.string,
  maxThreads: prop_types_default.a.string,
  activeThreads: prop_types_default.a.string,
  freeThreads: prop_types_default.a.string,
  schedulerMode: prop_types_default.a.string,
  schedulerDelay: prop_types_default.a.number,
  schedulerModeOptions: prop_types_default.a.array,
  schedulingEnabled: prop_types_default.a.string,
  serverTime: prop_types_default.a.string
};

function topPane_mapStateToProps(state) {
  return {
    schedulingEnabled: state.task.schedulingEnabled,
    status: state.task.status,
    freeThreads: state.task.freeThreads,
    activeThreads: state.task.activeThreads,
    maxThreads: state.task.maxThreads,
    schedulerMode: state.task.schedulerMode,
    schedulerDelay: state.task.schedulerDelay,
    schedulerModeOptions: state.task.schedulerModeOptions,
    serverTime: state.task.serverTime
  };
}

/* harmony default export */ var topPane = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(topPane_mapStateToProps)(topPane_TopPane));
// EXTERNAL MODULE: ./src/components/body/style.less
var body_style = __webpack_require__(50);

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













var body_Body =
/*#__PURE__*/
function (_Component) {
  body_inherits(Body, _Component);

  function Body() {
    var _this;

    body_classCallCheck(this, Body);

    _this = body_possibleConstructorReturn(this, body_getPrototypeOf(Body).call(this));
    _this.handleSelect = _this.handleSelect.bind(body_assertThisInitialized(_this));
    return _this;
  }

  body_createClass(Body, [{
    key: "handleSelect",
    value: function handleSelect(index) {
      var props = this.props;

      if (props.settingsClientModified) {
        utils["a" /* default */].utilities.confirm(resources.get("SettingsRestoreWarning"), resources.get("Yes"), resources.get("No"), function () {
          props.dispatch(task.cancelSettingsClientModified());
          props.dispatch(pagination.loadTab(index));
        });
      } else {
        props.dispatch(pagination.loadTab(index));
      }
    }
    /*eslint no-mixed-spaces-and-tabs: "error"*/

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(topPane, null),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
          onSelect: this.handleSelect.bind(this),
          selectedIndex: this.props.tabIndex,
          tabHeaders: [resources.get("TabTaskQueue"), resources.get("TabScheduler"), resources.get("TabHistory")],
          type: "primary"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(taskQueue, null),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(scheduler, null),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_history, {
          title: resources.get("TabHistoryTitle")
        })))
      );
    }
  }]);

  return Body;
}(external_window_dnn_nodeModules_React_["Component"]);
body_Body.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  tabIndex: prop_types_default.a.number,
  status: prop_types_default.a.string,
  settingsClientModified: prop_types_default.a.bool
};

function body_mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    status: state.task.status,
    settingsClientModified: state.task.settingsClientModified
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
          className: "taskScheduler-app"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 0
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: resources.get("nav_TaskScheduler")
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
          className: "taskScheduler-Root"
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
/* 53 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactDOM"
var external_window_dnn_nodeModules_ReactDOM_ = __webpack_require__(11);

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
    var options = window.dnn.initScheduler();
    utils["a" /* default */].init(options.utility);
    utils["a" /* default */].moduleName = options.moduleName; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(22);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
/* harmony default export */ var application = (boilerPlate);
// EXTERNAL MODULE: external "window.dnn.nodeModules.Redux"
var external_window_dnn_nodeModules_Redux_ = __webpack_require__(8);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxThunk"
var external_window_dnn_nodeModules_ReduxThunk_ = __webpack_require__(9);
var external_window_dnn_nodeModules_ReduxThunk_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxThunk_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxImmutableStateInvariant"
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_ = __webpack_require__(12);
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
// CONCATENATED MODULE: ./src/reducers/taskReducer.js
function taskReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { taskReducer_defineProperty(target, key, source[key]); }); } return target; }

function taskReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function taskList() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    taskStatusList: [],
    totalCount: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["b" /* task */].RETRIEVED_TASK_STATUS_LIST:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        serverTime: action.data.serverTime,
        schedulingEnabled: action.data.schedulingEnabled,
        status: action.data.status,
        freeThreads: action.data.freeThreads,
        activeThreads: action.data.activeThreads,
        maxThreads: action.data.maxThreads,
        taskProcessingList: action.data.taskProcessingList,
        taskStatusList: action.data.taskStatusList,
        totalCount: action.data.totalCount
      });

    case actionTypes["b" /* task */].RETRIEVED_SCHEDULE_SETTINGS:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        schedulerModeOptions: action.data.schedulerModeOptions,
        schedulerMode: action.data.schedulerMode,
        schedulerDelay: action.data.schedulerDelay
      });

    case actionTypes["b" /* task */].UPDATED_SCHEDULE_SETTINGS:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        schedulerMode: action.data.schedulerMode,
        schedulerDelay: action.data.schedulerDelay,
        settingsClientModified: action.data.settingsClientModified
      });

    case actionTypes["b" /* task */].RETRIEVED_SCHEDULE_HISTORY:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        taskHistoryList: action.data.taskHistoryList,
        totalHistoryCount: action.data.totalHistoryCount
      });

    case actionTypes["b" /* task */].RETRIEVED_SCHEDULE_ITEMS:
    case actionTypes["b" /* task */].DELETED_SCHEDULE_ITEM:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        schedulerItemList: action.data.schedulerItemList,
        totalCount: action.data.totalCount
      });

    case actionTypes["b" /* task */].RETRIEVED_SERVER_LIST:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        serverList: action.data.serverList
      });

    case actionTypes["b" /* task */].RETRIEVED_SCHEDULE_ITEM:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        scheduleItemDetail: action.data.scheduleItemDetail,
        settingsClientModified: action.data.settingsClientModified
      });

    case actionTypes["b" /* task */].CREATED_SCHEDULE_ITEM:
    case actionTypes["b" /* task */].UPDATED_SCHEDULE_ITEM:
    case actionTypes["b" /* task */].CANCELED_SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        settingsClientModified: action.data.settingsClientModified
      });

    case actionTypes["b" /* task */].SCHEDULE_ITEM_SETTINS_CLIENT_MODIFIED:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        scheduleItemDetail: action.data.scheduleItemDetail,
        settingsClientModified: action.data.settingsClientModified
      });

    case actionTypes["b" /* task */].UPDATED_SCHEDULE_SETTINGS_PENDING:
      return taskReducer_objectSpread2(taskReducer_objectSpread2({}, state), {}, {
        schedulerMode: action.data.schedulerMode
      });

    default:
      return taskReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/rootReducer.js




var rootReducer = Object(external_window_dnn_nodeModules_Redux_["combineReducers"])({
  pagination: pagination,
  visiblePanel: visiblePanel,
  task: taskList
});
/* harmony default export */ var reducers_rootReducer = (rootReducer);
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevTools"
var external_window_dnn_nodeModules_ReduxDevTools_ = __webpack_require__(13);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsLogMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_ = __webpack_require__(14);
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsDockMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_ = __webpack_require__(15);
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
var Root = __webpack_require__(16);
var Root_default = /*#__PURE__*/__webpack_require__.n(Root);

// CONCATENATED MODULE: ./src/main.jsx






var main_store = configureStore();
application.dispatch = main_store.dispatch;
application.init();
var appContainer = document.getElementById("scheduler-container");
Object(external_window_dnn_nodeModules_ReactDOM_["render"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactRedux_["Provider"], {
  store: main_store
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(Root_default.a, null)), appContainer);

/***/ })
/******/ ]);
//# sourceMappingURL=task-scheduler-bundle.js.map