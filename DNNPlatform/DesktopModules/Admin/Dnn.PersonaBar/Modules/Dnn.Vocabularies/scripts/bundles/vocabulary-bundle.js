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
/******/ 	return __webpack_require__(__webpack_require__.s = 40);
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
  module.exports = __webpack_require__(27)();
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
// CONCATENATED MODULE: ./src/constants/actionTypes/vocabulary.js
var vocabularyActionTypes = {
  RETRIEVED_VOCABULARY_LIST: "RETRIEVED_VOCABULARY_LIST",
  UPDATED_VOCABULARY: "UPDATED_VOCABULARY",
  ADDED_VOCABULARY: "ADDED_VOCABULARY",
  DELETED_VOCABULARY: "DELETED_VOCABULARY",
  FAILED_TO_ADD_VOCABULARY: "FAILED_TO_ADD_VOCABULARY"
};
/* harmony default export */ var vocabulary = (vocabularyActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/vocabularyTermList.js
var vocabularyTermActionTypes = {
  GET_VOCABULARY_TERMS: "GET_VOCABULARY_TERMS",
  RETRIEVED_VOCABULARY_TERMS: "RETRIEVED_VOCABULARY_TERMS",
  UPDATED_VOCABULARY_TERM: "UPDATED_VOCABULARY_TERM",
  ADDED_VOCABULARY_TERM: "ADDED_VOCABULARY_TERM",
  DELETED_VOCABULARY_TERM: "DELETED_VOCABULARY_TERM",
  SET_TERM_SELECTED: "SET_TERM_SELECTED",
  CLEAR_TERMS_SELECTED: "CLEAR_TERMS_SELECTED"
};
/* harmony default export */ var vocabularyTermList = (vocabularyTermActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/visiblePanel.js
var visiblePanel_paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
/* harmony default export */ var visiblePanel = (visiblePanel_paginationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/index.js
/* concated harmony reexport pagination */__webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* concated harmony reexport vocabulary */__webpack_require__.d(__webpack_exports__, "c", function() { return vocabulary; });
/* concated harmony reexport vocabularyTermList */__webpack_require__.d(__webpack_exports__, "d", function() { return vocabularyTermList; });
/* concated harmony reexport visiblePanel */__webpack_require__.d(__webpack_exports__, "b", function() { return visiblePanel; });






/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
var utils = {
  init: function init(options) {
    if (!options || !options.utility) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = options.utility;
    this.moduleName = options.moduleName;
    this.settings = options.settings;
  },
  canEdit: function canEdit() {
    return this.settings.isHost || this.settings.isAdmin || this.settings.permissions && this.settings.permissions.EDIT === true;
  },
  isHost: function isHost() {
    return this.settings.isHost;
  },
  utilities: null,
  moduleName: null,
  settings: null
};
/* harmony default export */ __webpack_exports__["a"] = (utils);

/***/ }),
/* 5 */
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
/* 6 */
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

var	fixUrls = __webpack_require__(26);

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

module.exports = window.dnn.nodeModules.ReactCollapse;

/***/ }),
/* 10 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactCustomScrollBars;

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 12 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 13 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 14 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 15 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 16 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(39);
} else {}

/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(29);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(32);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(33);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(34);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(35);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(38);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(25);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 26 */
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
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(28);

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
/* 28 */
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
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1Q7-DkePapyqxJ1Cmdh1BU {\n  display: table;\n  width: 100%;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU.false {\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3;\n  position: relative;\n  top: -1px;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU:not(:last-child) div.collapsible-header {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header {\n  text-transform: uppercase;\n  color: #b2bdc3;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 22px 22px 16px;\n  box-sizing: border-box;\n  cursor: pointer;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header * {\n  pointer-events: none;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header .delete-button {\n  pointer-events: all;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header.false {\n  border-bottom: none;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header .collapse-icon {\n  position: absolute;\n  right: 20px;\n  top: 21px;\n  font-size: 30px;\n  line-height: 16px;\n  cursor: pointer;\n  -webkit-touch-callout: none;\n  /* iOS Safari */\n  -webkit-user-select: none;\n  /* Chrome/Safari/Opera */\n  -khtml-user-select: none;\n  /* Konqueror */\n  -moz-user-select: none;\n  /* Firefox */\n  -ms-user-select: none;\n  /* IE/Edge */\n  user-select: none;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header .collapse-icon:after {\n  border: 6px solid transparent;\n  border-bottom: 6px solid #4B4E4F;\n  content: \"\";\n  position: absolute;\n  left: -15px;\n  top: 0;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU div.collapsible-header .collapse-icon.collapsed:after {\n  border-top: 6px solid #4B4E4F;\n  border-bottom: 6px solid transparent;\n  top: 5px;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU .term-header-collapsible > div {\n  float: left;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU .term-header-collapsible > div > .dnn-grid-system {\n  padding: 45px 0;\n  transition: none;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU .vocab-footer {\n  text-align: center;\n  padding-bottom: 55px;\n}\n._1Q7-DkePapyqxJ1Cmdh1BU .vocab-footer > .dnn-ui-common-button {\n  margin-left: 11px;\n}\n.term-header .term-icon svg {\n  width: 25px;\n  float: left;\n  height: 25px;\n  margin-right: 15px;\n  margin-top: -5px;\n  fill: #6F7273;\n}\n.term-header .term-label {\n  color: #46292B;\n}\n.term-header .term-label .dnn-text-overflow-wrapper {\n  font-size: 10pt;\n}\n.term-header .term-label * {\n  pointer-events: all !important;\n}\n.term-header .delete-button {\n  display: inline;\n  pointer-events: none !important;\n}\n.term-header .delete-button svg {\n  pointer-events: all !important;\n  margin-left: 10px;\n  margin-bottom: -2px;\n  width: 16px;\n  height: 16px;\n}\n.term-header .delete-button svg g,\n.term-header .delete-button svg polygon {\n  pointer-events: none !important;\n}\n", ""]);

// Exports
exports.locals = {
	"collapsibleComponent": "_1Q7-DkePapyqxJ1Cmdh1BU"
};

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" fill=\"currentColor\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<path d=\"M1788.9,709.7l-448.7-448.7l-1,1v-0.6H261.8v1530.4h1530.4V709.7H1788.9z M1458.2,1174.5c-57,0-104.9-38.7-119-91.2h-190.3\r\n\tc-14.9,51.2-62.2,88.7-118.2,88.7c-56,0-103.3-37.4-118.2-88.7H722.3c-13.8,52.8-61.9,91.8-119.1,91.8c-68,0-123.1-55.1-123.1-123.1\r\n\ts55.1-123.1,123.1-123.1c57.5,0,105.8,39.4,119.3,92.7h188.1c12.4-54.9,61.5-95.8,120.1-95.8c58.6,0,107.7,41,120.1,95.8h187.9\r\n\tc13.4-53.6,61.8-93.2,119.5-93.2c68,0,123.1,55.1,123.1,123.1S1526.2,1174.5,1458.2,1174.5z M1339.1,709.7V359.4l350.3,350.3H1339.1\r\n\tz\"/>\r\n</svg>\r\n");

/***/ }),
/* 31 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" fill=\"currentColor\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<path d=\"M1794.9,709.7l-448.7-448.7l-1,1v-0.6H267.8v1530.4h1530.4V709.7H1794.9z M1458.2,1442.6c-68,0-123.1-55.1-123.1-123.1\r\n\tc0-56.3,37.7-103.7,89.2-118.4v-123.3H628.1v121.1c56.1,11.5,98.2,61.1,98.2,120.6c0,68-55.1,123.1-123.1,123.1\r\n\tS480,1387.5,480,1319.4c0-55.2,36.3-101.8,86.2-117.5v-186h13.4h48.4h371.7v-131c-52-14.4-90.2-62.1-90.2-118.7\r\n\tc0-68,55.1-123.1,123.1-123.1s123.1,55.1,123.1,123.1c0,58.1-40.2,106.7-94.2,119.7v130h362.7h61.8v61.8v121.8\r\n\tc54.6,12.6,95.2,61.5,95.2,120C1581.3,1387.5,1526.2,1442.6,1458.2,1442.6z M1345.1,709.7V359.4l350.3,350.3H1345.1z\"/>\r\n</svg>\r\n");

/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.AjqC8R5fyT52anSSrLnge {\n  padding: 0 30px 30px 40px;\n  display: block;\n  box-sizing: border-box;\n  float: left;\n  width: 100%;\n  height: auto;\n  border-right: 1px solid #ddd;\n  margin-bottom: 15px;\n}\n.AjqC8R5fyT52anSSrLnge .dnn-editable-field:first-child {\n  padding-top: 2px;\n}\n", ""]);

// Exports
exports.locals = {
	"vocabulariesLeftPane": "AjqC8R5fyT52anSSrLnge"
};

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2-JLfinRboEzFzNHAnL31t {\n  background: #FAFAFA;\n  padding: 15px 25px;\n}\n._2-JLfinRboEzFzNHAnL31t .add-term-title {\n  font-size: 13px;\n  text-transform: uppercase;\n  font-weight: bold;\n  margin-bottom: 15px;\n}\n._2-JLfinRboEzFzNHAnL31t .parent-display {\n  padding: 8px;\n  background-color: white;\n  border: 1px solid #bfbfbf;\n  cursor: pointer;\n}\n._2-JLfinRboEzFzNHAnL31t .parent-tree {\n  background: white;\n  padding: 15px 25px;\n}\n._2-JLfinRboEzFzNHAnL31t .parent-tree .term-name:hover {\n  color: #2FA5EB;\n}\n._2-JLfinRboEzFzNHAnL31t .dnn-single-line-input-with-error,\n._2-JLfinRboEzFzNHAnL31t .dnn-multi-line-input-with-error {\n  width: 100%;\n}\n._2-JLfinRboEzFzNHAnL31t .add-term-footer {\n  text-align: right;\n}\n._2-JLfinRboEzFzNHAnL31t .add-term-footer button {\n  margin-right: 11px;\n}\n._2-JLfinRboEzFzNHAnL31t .add-term-footer button:last-child {\n  margin-right: 0;\n}\n._2-JLfinRboEzFzNHAnL31t .dnn-ui-common-input-group > label {\n  font-weight: bold;\n  margin-bottom: 5px;\n  display: block;\n  color: #46292B;\n}\n._2-JLfinRboEzFzNHAnL31t .dnn-ui-common-input-group input,\n._2-JLfinRboEzFzNHAnL31t .dnn-ui-common-input-group textarea,\n._2-JLfinRboEzFzNHAnL31t .dnn-ui-common-input-group select {\n  color: #46292B;\n  border-radius: 0px;\n  width: 100%;\n}\n._2-JLfinRboEzFzNHAnL31t .dnn-ui-common-input-group textarea {\n  height: 60px;\n  padding: 5px;\n  margin-bottom: 0;\n}\n", ""]);

// Exports
exports.locals = {
	"addTermBox": "_2-JLfinRboEzFzNHAnL31t"
};

/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._33e9wKHLVMJiEijO7ey92C {\n  cursor: pointer;\n}\n._33e9wKHLVMJiEijO7ey92C.selected > div > div > span {\n  color: #21A3DA;\n}\n._33e9wKHLVMJiEijO7ey92C.selected > div > div > span * {\n  color: #333;\n}\n._33e9wKHLVMJiEijO7ey92C > div:first-child {\n  display: inline-block;\n  position: relative;\n  padding-right: 25px;\n}\n._33e9wKHLVMJiEijO7ey92C > div:first-child:hover .edit-button {\n  display: block;\n}\n._33e9wKHLVMJiEijO7ey92C .edit-svg {\n  margin-right: 5px;\n  position: absolute;\n  left: -17px;\n  top: 3px;\n}\n._33e9wKHLVMJiEijO7ey92C .edit-svg svg {\n  width: 12px;\n  height: 12px;\n  fill: #6F7273;\n}\n._33e9wKHLVMJiEijO7ey92C span {\n  font-size: 15px;\n  font-weight: bold;\n  display: block;\n  margin-bottom: 5px;\n  color: #6F7273;\n}\n._33e9wKHLVMJiEijO7ey92C .edit-button {\n  width: 16px;\n  height: 16px;\n  position: absolute;\n  right: 0;\n  top: 0;\n  margin-left: 10px;\n  display: none;\n}\n", ""]);

// Exports
exports.locals = {
	"termLi": "_33e9wKHLVMJiEijO7ey92C"
};

/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list-label {\n  font-weight: bold;\n  text-transform: uppercase;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button {\n  float: right;\n  font-weight: bold;\n  cursor: pointer;\n  color: #C8C8C8;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button:hover {\n  color: #6F7273;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button:hover svg {\n  fill: #6F7273;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button:active {\n  color: #1E88C3;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button:active svg {\n  fill: #1E88C3;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .add-term-button svg {\n  width: 16px;\n  height: 16px;\n  float: left;\n  margin-right: 5px;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list {\n  padding: 0 30px 15px;\n  position: relative;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content {\n  transition: 0.5s;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content ul.root-level {\n  list-style: none;\n  padding: 5px 15px;\n  min-height: 285px;\n  margin-top: 5px;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content ul.root-level.Hierarchy {\n  padding: 5px 25px;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content ul.root-level ul {\n  padding: 0 15px;\n  list-style: none;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content.closed {\n  opacity: 0;\n  z-index: -1;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-list-content.open {\n  opacity: 1;\n  z-index: 10;\n  transition-delay: 0.5s;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-ul {\n  padding: 0 15px;\n  list-style: none;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-ul.root-level {\n  padding: 5px 25px;\n}\n._3Gj_kj90tDJ1kTBvV1ggmC .term-list .term-ul.term-list-level li.selected > div > div > span {\n  color: #6F7273;\n}\n", ""]);

// Exports
exports.locals = {
	"vocabulariesRightPane": "_3Gj_kj90tDJ1kTBvV1ggmC"
};

/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(37);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(6)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 37 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, ".vocabulary-list .dnn-persona-bar-page-body .vocabulary-error {\n  font-size: 15px;\n  padding: 25px;\n  color: #C8C8C8;\n  text-transform: uppercase;\n}\n", ""]);



/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(5)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box {\n  padding: 25px 50px;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-single-line-input-with-error,\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-multi-line-input-with-error {\n  width: 100%;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box label {\n  float: left;\n  margin-right: 15px;\n  font-weight: bold;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-radio-buttons {\n  float: left;\n  width: 100%;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-radio-buttons label:first-child {\n  width: 65px;\n  word-wrap: break-word;\n  margin-bottom: 15px;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group {\n  margin-bottom: 15px;\n  float: left;\n  width: 100%;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group > label {\n  font-weight: bold;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group input,\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group textarea,\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group select {\n  border-radius: 0px;\n  width: 100%;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .dnn-ui-common-input-group textarea {\n  height: 100px;\n  padding: 5px;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .action-buttons {\n  float: left;\n  width: 100%;\n  text-align: center;\n  margin: 20px 0;\n  position: relative;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .action-buttons .required-help-text {\n  position: absolute;\n  top: 15px;\n  left: 0;\n  color: #6F7273;\n}\n._3wAZB1r5A5ZIgQKEkLpIKU .create-box .action-buttons button:first-child {\n  margin-right: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"createVocabulary": "_3wAZB1r5A5ZIgQKEkLpIKU"
};

/***/ }),
/* 39 */
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
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(7);

// EXTERNAL MODULE: external "window.dnn.nodeModules.CommonComponents"
var external_window_dnn_nodeModules_CommonComponents_ = __webpack_require__(2);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 4 modules
var actionTypes = __webpack_require__(3);

// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/services/vocabularyService.js
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

 //import "fetch-ie8";

function serializeQueryStringParameters(obj) {
  var s = [];

  for (var p in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, p)) {
      s.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  }

  return s.join("&");
}

var vocabularyService_VocabularyService =
/*#__PURE__*/
function () {
  function VocabularyService() {
    _classCallCheck(this, VocabularyService);
  }

  _createClass(VocabularyService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getVocabularyList",
    value: function getVocabularyList(searchParameters, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      searchParameters = _extends({}, searchParameters, {
        scopeTypeId: searchParameters.scopeTypeId > 0 ? searchParameters.scopeTypeId : "*"
      });
      sf.get("GetVocabularies?" + serializeQueryStringParameters(searchParameters), {}, callback, failureCallback);
    }
  }, {
    key: "addVocabulary",
    value: function addVocabulary(vocabulary, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("CreateVocabulary", vocabulary, callback, failureCallback);
    }
  }, {
    key: "updateVocabulary",
    value: function updateVocabulary(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("UpdateVocabulary", payload, callback, failureCallback);
    }
  }, {
    key: "deleteVocabulary",
    value: function deleteVocabulary(vocabularyId, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("DeleteVocabulary?vocabularyId=" + vocabularyId, {}, callback, failureCallback);
    }
  }]);

  return VocabularyService;
}();

var vocabularyService = new vocabularyService_VocabularyService();
/* harmony default export */ var services_vocabularyService = (vocabularyService);
// CONCATENATED MODULE: ./src/actions/pagination.js


var paginationActions = {
  loadMore: function loadMore(searchParameters) {
    return function (dispatch) {
      services_vocabularyService.getVocabularyList(searchParameters, function (data) {
        dispatch({
          type: actionTypes["a" /* pagination */].LOAD_MORE,
          loadMore: true,
          payload: searchParameters,
          data: {
            vocabularyList: data.Results,
            totalCount: data.TotalResults
          }
        });
      });
    };
  },
  loadTab: function loadTab(searchParameters) {
    return function (dispatch) {
      services_vocabularyService.getVocabularyList(searchParameters, function (data) {
        dispatch({
          type: actionTypes["a" /* pagination */].LOAD_TAB_DATA,
          loadMore: true,
          payload: searchParameters,
          tabIndex: searchParameters.scopeTypeId,
          data: {
            vocabularyList: data.Results,
            totalCount: data.TotalResults
          }
        });
      });
    };
  }
};
/* harmony default export */ var pagination = (paginationActions);
// CONCATENATED MODULE: ./src/actions/vocabulary.js



var vocabularyActions = {
  getVocabularyList: function getVocabularyList(searchParameters, loadMore, callback) {
    return function (dispatch) {
      services_vocabularyService.getVocabularyList(searchParameters, function (data) {
        dispatch({
          type: actionTypes["c" /* vocabulary */].RETRIEVED_VOCABULARY_LIST,
          loadMore: loadMore,
          data: {
            vocabularyList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  addVocabulary: function addVocabulary(vocabulary, totalCount, callback) {
    return function (dispatch) {
      services_vocabularyService.addVocabulary(vocabulary, function (data) {
        vocabulary.VocabularyId = data.VocabularyId;
        vocabulary.Type = vocabulary.ScopeTypeId === 2 ? "Hierarchy" : "Simple";
        dispatch({
          type: actionTypes["c" /* vocabulary */].ADDED_VOCABULARY,
          payload: {
            addedVocabulary: vocabulary,
            totalCount: totalCount
          }
        });

        if (callback) {
          callback();
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  updateVocabulary: function updateVocabulary(payload, index) {
    return function (dispatch) {
      services_vocabularyService.updateVocabulary(payload, function () {
        dispatch({
          type: actionTypes["c" /* vocabulary */].UPDATED_VOCABULARY,
          data: {
            updatedTerm: payload,
            index: index
          }
        });
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  deleteVocabulary: function deleteVocabulary(vocabularyId, index, totalCount, callback) {
    return function (dispatch) {
      services_vocabularyService.deleteVocabulary(vocabularyId, function () {
        dispatch({
          type: actionTypes["c" /* vocabulary */].DELETED_VOCABULARY,
          payload: {
            index: index,
            totalCount: totalCount
          }
        });

        if (callback) {
          callback();
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  }
};
/* harmony default export */ var actions_vocabulary = (vocabularyActions);
// CONCATENATED MODULE: ./src/services/vocabularyTermService.js
function vocabularyTermService_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function vocabularyTermService_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function vocabularyTermService_createClass(Constructor, protoProps, staticProps) { if (protoProps) vocabularyTermService_defineProperties(Constructor.prototype, protoProps); if (staticProps) vocabularyTermService_defineProperties(Constructor, staticProps); return Constructor; }



var vocabularyTermService_VocabularyTermService =
/*#__PURE__*/
function () {
  function VocabularyTermService() {
    vocabularyTermService_classCallCheck(this, VocabularyTermService);
  }

  vocabularyTermService_createClass(VocabularyTermService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getVocabularyTerms",
    value: function getVocabularyTerms(vocabularyTermId, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.get("GetTermsByVocabularyId?vocabularyId=" + vocabularyTermId + "&pageIndex=0", {}, callback, failureCallback);
    }
  }, {
    key: "addVocabularyTerm",
    value: function addVocabularyTerm(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("CreateTerm", payload, callback, failureCallback);
    }
  }, {
    key: "updateVocabularyTerm",
    value: function updateVocabularyTerm(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("UpdateTerm", payload, callback, failureCallback);
    }
  }, {
    key: "deleteVocabularyTerm",
    value: function deleteVocabularyTerm(termId, callback, failureCallback) {
      var sf = this.getServiceFramework("Vocabularies");
      sf.post("DeleteTerm?termId=" + termId, {}, callback, failureCallback);
    }
  }]);

  return VocabularyTermService;
}();

var vocabularyTermService = new vocabularyTermService_VocabularyTermService();
/* harmony default export */ var services_vocabularyTermService = (vocabularyTermService);
// CONCATENATED MODULE: ./src/actions/vocabularyTermList.js



var vocabularyTermActions = {
  getVocabularyTerms: function getVocabularyTerms(vocabularyId) {
    return function (dispatch) {
      services_vocabularyTermService.getVocabularyTerms(vocabularyId, function (data) {
        dispatch({
          type: actionTypes["d" /* vocabularyTermList */].RETRIEVED_VOCABULARY_TERMS,
          data: {
            vocabularyTerms: data.Results,
            totalCount: data.TotalResults
          }
        });
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  addVocabularyTerm: function addVocabularyTerm(term, totalCount, callback) {
    return function (dispatch) {
      services_vocabularyTermService.addVocabularyTerm(term, function (data) {
        term.TermId = data.TermId;
        dispatch({
          type: actionTypes["d" /* vocabularyTermList */].ADDED_VOCABULARY_TERM,
          payload: {
            addedTerm: term,
            totalCount: totalCount
          }
        });

        if (callback) {
          callback();
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  updateVocabularyTerm: function updateVocabularyTerm(payload, index, callback) {
    return function (dispatch) {
      services_vocabularyTermService.updateVocabularyTerm(payload, function () {
        dispatch({
          type: actionTypes["d" /* vocabularyTermList */].UPDATED_VOCABULARY_TERM,
          payload: {
            updatedTerm: payload,
            index: index
          }
        });

        if (callback) {
          callback();
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  deleteVocabularyTerm: function deleteVocabularyTerm(termId, vocabularyTerms, totalCount) {
    return function (dispatch) {
      services_vocabularyTermService.deleteVocabularyTerm(termId, function () {
        dispatch({
          type: actionTypes["d" /* vocabularyTermList */].DELETED_VOCABULARY_TERM,
          payload: {
            vocabularyTerms: vocabularyTerms,
            totalCount: totalCount
          }
        });
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        utils["a" /* default */].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  setTermSelected: function setTermSelected(updatedTerm, index) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* vocabularyTermList */].SET_TERM_SELECTED,
        payload: {
          updatedTerm: updatedTerm,
          index: index
        }
      });
    };
  },
  clearSelected: function clearSelected(vocabularyTerms) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* vocabularyTermList */].CLEAR_TERMS_SELECTED,
        payload: {
          vocabularyTerms: vocabularyTerms
        }
      });
    };
  }
};
/* harmony default export */ var vocabularyTermList = (vocabularyTermActions);
// CONCATENATED MODULE: ./src/actions/visiblePanel.js

var visiblePanelActions = {
  selectPanel: function selectPanel(panel) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* visiblePanel */].SELECT_PANEL,
        payload: {
          selectedPage: panel
        }
      });
    };
  }
};
/* harmony default export */ var visiblePanel = (visiblePanelActions);
// CONCATENATED MODULE: ./src/actions/index.js





// EXTERNAL MODULE: ./src/components/VocabularyList/TermHeader/style.less
var style = __webpack_require__(18);
var style_default = /*#__PURE__*/__webpack_require__.n(style);

// CONCATENATED MODULE: ./src/resources/index.jsx

var resx = {
  get: function get(key) {
    var moduleName = utils["a" /* default */].moduleName;
    return utils["a" /* default */].utilities.getResx(moduleName, key);
  }
};
/* harmony default export */ var resources = (resx);
// CONCATENATED MODULE: ./src/components/VocabularyList/TermHeader/index.jsx
function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function TermHeader_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function TermHeader_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function TermHeader_createClass(Constructor, protoProps, staticProps) { if (protoProps) TermHeader_defineProperties(Constructor.prototype, protoProps); if (staticProps) TermHeader_defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }







/* eslint-disable quotes */

var SimpleType = __webpack_require__(30).default;

var HierarchyType = __webpack_require__(31).default;

var TermHeader_TermHeader =
/*#__PURE__*/
function (_Component) {
  _inherits(TermHeader, _Component);

  function TermHeader() {
    var _this;

    TermHeader_classCallCheck(this, TermHeader);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(TermHeader).call(this));
    _this.state = {
      collapsed: true,
      repainting: false
    };
    _this.timeout = 0;
    _this.handleClick = _this.handleClick.bind(_assertThisInitialized(_this));
    return _this;
  }

  TermHeader_createClass(TermHeader, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;

      if (!props.closeOnClick) {
        return;
      }

      document.addEventListener("click", this.handleClick);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener("click", this.handleClick);
      this._isMounted = false;
    }
  }, {
    key: "handleClick",
    value: function handleClick(event) {
      // Note: this workaround is needed in IE. The remove event listener in the componentWillUnmount is called
      // before the handleClick handler is called, but in spite of that, the handleClick is executed. To avoid
      // the "findDOMNode was called on an unmounted component." error we need to check if the component is mounted before execute this code
      if (!this._isMounted) {
        return;
      }

      if (!this.node.contains(event.target) && typeof event.target.className === "string" && event.target.className.indexOf("false") > -1) {
        if (event.target.className.indexOf("delete-button") > -1) {
          return;
        }

        this.timeout = 475;
        this.collapse();
      } else {
        this.timeout = 0;
      }
    }
  }, {
    key: "uncollapse",
    value: function uncollapse() {
      this.setState({
        collapsed: false
      });
    }
  }, {
    key: "collapse",
    value: function collapse() {
      this.setState({
        collapsed: true
      });
    }
  }, {
    key: "toggle",
    value: function toggle(event) {
      if (this.state.collapsed) {
        this.uncollapse();
      } else {
        if (event.target.parentNode.className.indexOf("delete-button") > -1) {
          return;
        }

        this.collapse();
      }
    }
  }, {
    key: "onDelete",
    value: function onDelete() {
      var _this2 = this;

      var props = this.props;
      props.onDelete(props.term, props.index, function () {
        _this2.collapse();
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      var props = this.props,
          state = this.state;
      var svgIcon = props.type === 1 ? SimpleType : HierarchyType;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          ref: function ref(node) {
            return _this3.node = node;
          },
          className: "" + style_default.a.collapsibleComponent + " " + state.collapsed + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "collapsible-header " + state.collapsed,
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-header"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-icon",
          dangerouslySetInnerHTML: {
            __html: svgIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "term-label"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
          text: props.header,
          maxWidth: 200
        }))), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          className: "collapse-icon " + (this.state.collapsed ? "collapsed" : "")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          className: "term-header-collapsible",
          isOpened: !this.state.collapsed,
          style: {
            float: "left"
          }
        }, !state.collapsed && props.children, !state.collapsed &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "vocab-footer"
        }, !props.term.IsSystem && utils["a" /* default */].canEdit() &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onDelete.bind(this)
        }, resources.get("DeleteTerm")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.collapse.bind(this)
        }, resources.get("Close")))))
      );
    }
  }]);

  return TermHeader;
}(external_window_dnn_nodeModules_React_["Component"]);

TermHeader_TermHeader.propTypes = {
  header: prop_types_default.a.string,
  type: prop_types_default.a.string,
  label: prop_types_default.a.node,
  children: prop_types_default.a.node,
  disabled: prop_types_default.a.bool,
  className: prop_types_default.a.string,
  closeOnClick: prop_types_default.a.bool,
  term: prop_types_default.a.object,
  onDelete: prop_types_default.a.func,
  index: prop_types_default.a.number
};
/* harmony default export */ var VocabularyList_TermHeader = (TermHeader_TermHeader);
// EXTERNAL MODULE: ./src/components/VocabularyList/LeftPane/style.less
var LeftPane_style = __webpack_require__(19);
var LeftPane_style_default = /*#__PURE__*/__webpack_require__.n(LeftPane_style);

// CONCATENATED MODULE: ./src/components/VocabularyList/LeftPane/LeftPane.jsx
function LeftPane_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { LeftPane_typeof = function _typeof(obj) { return typeof obj; }; } else { LeftPane_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return LeftPane_typeof(obj); }

function LeftPane_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function LeftPane_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function LeftPane_createClass(Constructor, protoProps, staticProps) { if (protoProps) LeftPane_defineProperties(Constructor.prototype, protoProps); if (staticProps) LeftPane_defineProperties(Constructor, staticProps); return Constructor; }

function LeftPane_possibleConstructorReturn(self, call) { if (call && (LeftPane_typeof(call) === "object" || typeof call === "function")) { return call; } return LeftPane_assertThisInitialized(self); }

function LeftPane_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function LeftPane_getPrototypeOf(o) { LeftPane_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return LeftPane_getPrototypeOf(o); }

function LeftPane_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) LeftPane_setPrototypeOf(subClass, superClass); }

function LeftPane_setPrototypeOf(o, p) { LeftPane_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return LeftPane_setPrototypeOf(o, p); }








var LeftPane_LeftPane =
/*#__PURE__*/
function (_Component) {
  LeftPane_inherits(LeftPane, _Component);

  function LeftPane() {
    LeftPane_classCallCheck(this, LeftPane);

    return LeftPane_possibleConstructorReturn(this, LeftPane_getPrototypeOf(LeftPane).call(this));
  }

  LeftPane_createClass(LeftPane, [{
    key: "onEnter",
    value: function onEnter(key, value) {
      var props = this.props;
      props.onEnter(key, value, props.index);
    }
  }, {
    key: "canEdit",
    value: function canEdit() {
      var props = this.props;
      return utils["a" /* default */].isHost() || props.scopeType === "Portal" && utils["a" /* default */].canEdit();
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: LeftPane_style_default.a.vocabulariesLeftPane
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["EditableField"], {
          label: resources.get("Description"),
          value: props.description,
          onEnter: this.onEnter.bind(this, "Description"),
          inputType: "textArea",
          editable: this.canEdit(),
          helpText: resources.get("EditFieldHelper")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["EditableField"], {
          label: resources.get("Type"),
          value: resources.get(props.type),
          onEnter: this.onEnter.bind(this, "Type"),
          editable: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["EditableField"], {
          label: resources.get("Scope"),
          value: resources.get(props.scopeType),
          onEnter: this.onEnter.bind(this, "ScopeType"),
          editable: false
        }))
      );
    }
  }]);

  return LeftPane;
}(external_window_dnn_nodeModules_React_["Component"]);

LeftPane_LeftPane.propTypes = {
  description: prop_types_default.a.string,
  type: prop_types_default.a.string,
  scope: prop_types_default.a.string,
  onEnter: prop_types_default.a.func,
  index: prop_types_default.a.number
};
/* harmony default export */ var VocabularyList_LeftPane_LeftPane = (LeftPane_LeftPane);
// CONCATENATED MODULE: ./src/components/VocabularyList/LeftPane/index.jsx

/* harmony default export */ var VocabularyList_LeftPane = (VocabularyList_LeftPane_LeftPane);
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactCustomScrollBars"
var external_window_dnn_nodeModules_ReactCustomScrollBars_ = __webpack_require__(10);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactCollapse"
var external_window_dnn_nodeModules_ReactCollapse_ = __webpack_require__(9);
var external_window_dnn_nodeModules_ReactCollapse_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReactCollapse_);

// EXTERNAL MODULE: ./src/components/VocabularyList/RightPane/AddTermBox/style.less
var AddTermBox_style = __webpack_require__(20);
var AddTermBox_style_default = /*#__PURE__*/__webpack_require__.n(AddTermBox_style);

// CONCATENATED MODULE: ./src/components/VocabularyList/RightPane/AddTermBox/index.jsx
var _this = undefined;








var parentTermTreeStyle = {
  width: "100%",
  height: "110px",
  border: "1px solid #BFBFBF",
  borderTop: "none",
  background: "white",
  boxSizing: "border-box"
};

var AddTermBox_AddTermBox = function AddTermBox(_ref) {
  var isOpened = _ref.isOpened,
      editMode = _ref.editMode,
      termBeingEdited = _ref.termBeingEdited,
      termTreeVisible = _ref.termTreeVisible,
      parentDisplay = _ref.parentDisplay,
      parentTermTree = _ref.parentTermTree,
      parentTreeOpened = _ref.parentTreeOpened,
      toggleParentTree = _ref.toggleParentTree,
      onTermValueChange = _ref.onTermValueChange,
      deleteTerm = _ref.deleteTerm,
      closeAddTerm = _ref.closeAddTerm,
      onUpdateTerm = _ref.onUpdateTerm,
      error = _ref.error;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCollapse_default.a, {
      isOpened: isOpened
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: AddTermBox_style_default.a.addTermBox
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "add-term-title"
    }, editMode ? resources.get("CurrentTerm") : resources.get("AddTerm")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      inputId: "create-term-name",
      withLabel: true,
      label: resources.get("TermName") + "*",
      value: termBeingEdited.Name,
      onChange: onTermValueChange.bind(_this, "Name"),
      error: error,
      errorMessage: resources.get("TermValidationError.Message")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
      inputId: "create-term-description",
      withLabel: true,
      label: resources.get("Description"),
      value: termBeingEdited.Description,
      onChange: onTermValueChange.bind(_this, "Description")
    })), termTreeVisible &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], {
      style: {
        marginBottom: 32
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("label", null, resources.get("ParentTerm")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "parent-display",
      onClick: toggleParentTree
    }, parentDisplay && parentDisplay.Name || ""),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCollapse_default.a, {
      isOpened: parentTreeOpened,
      fixedHeight: 115,
      keepCollapsedContent: true
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
      style: parentTermTreeStyle
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("ul", {
      className: "term-ul root-level parent-tree"
    }, parentTermTree)))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: "add-term-footer"
    }, editMode &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: deleteTerm
    }, resources.get("DeleteTerm")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: closeAddTerm
    }, resources.get("cancelCreate")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onUpdateTerm
    }, resources.get("SaveTerm")))))
  );
};

AddTermBox_AddTermBox.propTypes = {
  isOpened: prop_types_default.a.bool,
  editMode: prop_types_default.a.bool,
  termBeingEdited: prop_types_default.a.object,
  termTreeVisible: prop_types_default.a.bool,
  parentDisplay: prop_types_default.a.object,
  parentTermTree: prop_types_default.a.node,
  parentTreeOpened: prop_types_default.a.bool,
  toggleParentTree: prop_types_default.a.func,
  onTermValueChange: prop_types_default.a.func,
  deleteTerm: prop_types_default.a.func,
  closeAddTerm: prop_types_default.a.func,
  onUpdateTerm: prop_types_default.a.func,
  error: prop_types_default.a.bool
};
/* harmony default export */ var RightPane_AddTermBox = (AddTermBox_AddTermBox);
// EXTERNAL MODULE: ./src/components/VocabularyList/RightPane/Term/style.less
var Term_style = __webpack_require__(21);
var Term_style_default = /*#__PURE__*/__webpack_require__.n(Term_style);

// CONCATENATED MODULE: ./src/components/VocabularyList/RightPane/Term/index.jsx
function Term_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Term_typeof = function _typeof(obj) { return typeof obj; }; } else { Term_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Term_typeof(obj); }

function Term_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Term_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Term_createClass(Constructor, protoProps, staticProps) { if (protoProps) Term_defineProperties(Constructor.prototype, protoProps); if (staticProps) Term_defineProperties(Constructor, staticProps); return Constructor; }

function Term_possibleConstructorReturn(self, call) { if (call && (Term_typeof(call) === "object" || typeof call === "function")) { return call; } return Term_assertThisInitialized(self); }

function Term_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Term_getPrototypeOf(o) { Term_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Term_getPrototypeOf(o); }

function Term_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Term_setPrototypeOf(subClass, superClass); }

function Term_setPrototypeOf(o, p) { Term_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Term_setPrototypeOf(o, p); }








var Term_Term =
/*#__PURE__*/
function (_Component) {
  Term_inherits(Term, _Component);

  function Term() {
    var _this;

    Term_classCallCheck(this, Term);

    _this = Term_possibleConstructorReturn(this, Term_getPrototypeOf(Term).call(this));
    _this.state = {
      isOpened: false
    };
    return _this;
  }

  Term_createClass(Term, [{
    key: "toggleTerm",
    value: function toggleTerm(event) {
      if (event) {
        event.preventDefault();
      }

      this.setState({
        isOpened: !this.state.isOpened
      });
    }
  }, {
    key: "onLiClick",
    value: function onLiClick(event) {
      event.preventDefault();
      var props = this.props;

      if (props.isEditable) {
        this.toggleTerm();
      } else {
        this.onClick();
      }
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      if (props.closeAll) {
        this.setState({
          isOpened: false
        });
      }
    }
  }, {
    key: "onClick",
    value: function onClick() {
      var props = this.props;
      props.onClick(props.term);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var className = Term_style_default.a.termLi + (props.children.length > 0 ? " has-children" : "") + (state.isOpened ? " opened" : "") + (props.term.selected ? " selected" : "");
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("li", {
          className: className
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null, props.children.length > 0 && !state.isOpened &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].ArrowRightIcon
          },
          className: "edit-svg",
          onClick: this.toggleTerm.bind(this)
        }), props.children.length > 0 && state.isOpened &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].ArrowDownIcon
          },
          className: "edit-svg",
          onClick: this.toggleTerm.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          onClick: this.onLiClick.bind(this)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          className: "term-name",
          dangerouslySetInnerHTML: {
            __html: props.term.Name
          }
        }), props.isEditable && utils["a" /* default */].canEdit() &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "edit-button",
          onClick: this.onClick.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
          }
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCollapse_default.a, {
          isOpened: state.isOpened
        }, props.children))
      );
    }
  }]);

  return Term;
}(external_window_dnn_nodeModules_React_["Component"]);

Term_Term.propTypes = {
  term: prop_types_default.a.object,
  children: prop_types_default.a.node,
  isEditable: prop_types_default.a.bool,
  onClick: prop_types_default.a.func
};
/* harmony default export */ var RightPane_Term = (Term_Term);
// EXTERNAL MODULE: ./src/components/VocabularyList/RightPane/style.less
var RightPane_style = __webpack_require__(22);
var RightPane_style_default = /*#__PURE__*/__webpack_require__.n(RightPane_style);

// CONCATENATED MODULE: ./src/components/VocabularyList/RightPane/index.jsx
function RightPane_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { RightPane_typeof = function _typeof(obj) { return typeof obj; }; } else { RightPane_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return RightPane_typeof(obj); }

function RightPane_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function RightPane_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function RightPane_createClass(Constructor, protoProps, staticProps) { if (protoProps) RightPane_defineProperties(Constructor.prototype, protoProps); if (staticProps) RightPane_defineProperties(Constructor, staticProps); return Constructor; }

function RightPane_possibleConstructorReturn(self, call) { if (call && (RightPane_typeof(call) === "object" || typeof call === "function")) { return call; } return RightPane_assertThisInitialized(self); }

function RightPane_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function RightPane_getPrototypeOf(o) { RightPane_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return RightPane_getPrototypeOf(o); }

function RightPane_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) RightPane_setPrototypeOf(subClass, superClass); }

function RightPane_setPrototypeOf(o, p) { RightPane_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return RightPane_setPrototypeOf(o, p); }











function findInChildren(list, parentTermId) {
  if (!list) {
    return null;
  }

  var parentTerm = null;

  for (var i = 0; i < list.length && !parentTerm; i++) {
    var term = list[i];
    parentTerm = term.TermId === parentTermId ? term : findInChildren(term.ChildTerms, parentTermId);
  }

  return parentTerm;
}

function getVocabularyTermList(terms, type) {
  if (type === "Simple") {
    return terms;
  }

  var _terms = [];
  terms.forEach(function (term) {
    if (term.ParentTermId < 0) {
      term.ChildTerms = [];

      if (!_terms.find(function (_term) {
        return _term.TermId === term.TermId;
      })) {
        _terms = _terms.concat(term);
      }
    } else {
      var parent = findInChildren(_terms, term.ParentTermId);

      if (parent) {
        if (!parent.ChildTerms) {
          parent.ChildTerms = [];
        }

        if (!parent.ChildTerms.find(function (_term) {
          return _term.TermId === term.TermId;
        })) {
          parent.ChildTerms = parent.ChildTerms.concat(term);
        }
      }
    }
  });
  return _terms;
}

function removeChildTerms(terms) {
  return terms.map(function (term) {
    delete term.ChildTerms;
    return term;
  });
}

var RightPane_RightPane =
/*#__PURE__*/
function (_Component) {
  RightPane_inherits(RightPane, _Component);

  function RightPane() {
    var _this;

    RightPane_classCallCheck(this, RightPane);

    _this = RightPane_possibleConstructorReturn(this, RightPane_getPrototypeOf(RightPane).call(this));
    _this.state = {
      editBoxOpened: false,
      _editBoxOpened: false,
      editMode: false,
      parentTreeOpened: false,
      open: false,
      termBeingEdited: {
        TermId: -1,
        Name: "",
        Description: "",
        VocabularyId: -1,
        ParentTermId: -1
      },
      nameError: true,
      triedToSubmitTerm: false
    };
    return _this;
  }

  RightPane_createClass(RightPane, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var termBeingEdited = this.state.termBeingEdited;
      var props = this.props;
      termBeingEdited.VocabularyId = props.vocabularyId;
      this.setState({
        termBeingEdited: termBeingEdited
      });
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.getVocabularyTerms(props.vocabularyId, props.index);
    }
  }, {
    key: "openAddTerm",
    value: function openAddTerm(editMode, type, event) {
      var _this2 = this;

      if (event) {
        event.preventDefault();
      }

      var termBeingEdited = this.state.termBeingEdited;
      var props = this.props;
      termBeingEdited = {
        TermId: -1,
        Name: "",
        Description: "",
        VocabularyId: props.vocabularyId,
        ParentTermId: type === "Hierarchy" ? props.vocabularyTerms.length > 0 && props.vocabularyTerms[0].TermId || -1 : -1
      };

      if (!editMode) {
        delete termBeingEdited.TermId;
      }

      this.setState({
        editBoxOpened: true,
        termBeingEdited: termBeingEdited,
        editMode: editMode,
        triedToSubmitTerm: false,
        nameError: true
      });
      setTimeout(function () {
        _this2.setState({
          _editBoxOpened: true
        });
      }, 500);
      var parentTerm = JSON.parse(JSON.stringify(props.vocabularyTerms)).find(function (term) {
        return term.TermId === termBeingEdited.ParentTermId;
      });

      if (parentTerm) {
        parentTerm.selected = true;
        props.selectParentTerm(parentTerm);
      }
    }
  }, {
    key: "closeAddTerm",
    value: function closeAddTerm() {
      var _this3 = this;

      var props = this.props;
      setTimeout(function () {
        _this3.setState({
          termBeingEdited: {
            TermId: -1,
            Name: "",
            Description: "",
            VocabularyId: props.vocabularyId,
            ParentTermId: props.type === "Hierarchy" ? props.vocabularyTerms.length > 0 && props.vocabularyTerms[0].TermId || -1 : -1
          },
          editBoxOpened: false
        });
      }, 500);
      this.setState({
        _editBoxOpened: false
      });
    }
  }, {
    key: "toggleParentTree",
    value: function toggleParentTree(event) {
      if (event) {
        event.preventDefault();
      }

      var state = this.state;
      this.setState({
        parentTreeOpened: !state.parentTreeOpened
      });
    }
  }, {
    key: "onTermValueChange",
    value: function onTermValueChange(key, event) {
      var value = event.target.value;
      var state = this.state;
      state.termBeingEdited[key] = value;
      state.triedToSubmitTerm = false;

      if (key === "Name" && value === "") {
        state.nameError = true;
      } else {
        state.nameError = false;
      }

      this.setState({
        state: state
      });
    }
  }, {
    key: "onUpdateTerm",
    value: function onUpdateTerm(event) {
      var _this4 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;

      if (props.type === "Simple") {
        delete state.termBeingEdited.ParentTermId;
      }

      this.setState({
        triedToSubmitTerm: true
      });

      if (state.nameError) {
        return;
      }

      props.onUpdateTerm(state.termBeingEdited, state.editMode, function () {
        _this4.closeAddTerm();
      });
    }
  }, {
    key: "onEditTerm",
    value: function onEditTerm(term) {
      var props = this.props;

      if (!this.canEdit()) {
        return;
      }

      this.openAddTerm(true);
      var termBeingEdited = this.state.termBeingEdited;
      termBeingEdited = term;
      this.setState({
        termBeingEdited: termBeingEdited,
        triedToSubmitTerm: false,
        nameError: false
      });
      var parentTerm = JSON.parse(JSON.stringify(props.vocabularyTerms)).find(function (term) {
        return term.TermId === termBeingEdited.ParentTermId;
      });

      if (parentTerm) {
        parentTerm.selected = true;
        props.selectParentTerm(parentTerm);
      }
    }
  }, {
    key: "getChildTerms",
    value: function getChildTerms(term, clickFunction, isEditable) {
      var _this5 = this;

      var children = [];

      if (term.ChildTerms) {
        children = term.ChildTerms.map(function (child) {
          var _children = _this5.getChildTerms(child, clickFunction, isEditable);

          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("ul", {
              className: "term-ul",
              key: "ul-" + term.TermId
            },
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(RightPane_Term, {
              term: child,
              onClick: clickFunction,
              isEditable: isEditable,
              key: "term-" + term.TermId
            }, _children))
          );
        });
      }

      return children;
    }
  }, {
    key: "onSelectParent",
    value: function onSelectParent(term) {
      var _this6 = this;

      var termBeingEdited = this.state.termBeingEdited;
      var props = this.props;
      termBeingEdited.ParentTermId = term.TermId;
      this.setState({
        termBeingEdited: termBeingEdited
      }, function () {
        _this6.toggleParentTree();

        props.selectParentTerm(term);
      });
    }
  }, {
    key: "deleteTerm",
    value: function deleteTerm() {
      var _this7 = this;

      var props = this.props,
          state = this.state;
      utils["a" /* default */].utilities.confirm(resources.get("ConfirmDeletion_Term").replace("{0}", state.termBeingEdited.Name), //confirm message
      resources.get("DeleteTerm"), //delete button text
      resources.get("cancelCreate"), //cancel text
      function () {
        props.onDeleteTerm(state.termBeingEdited.TermId);

        _this7.closeAddTerm();
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      this.setState({
        open: !this.state.open
      });
    }
  }, {
    key: "canEdit",
    value: function canEdit() {
      var props = this.props;
      return utils["a" /* default */].isHost() || props.scopeType === "Portal" && utils["a" /* default */].canEdit();
    }
  }, {
    key: "render",
    value: function render() {
      var _this8 = this;

      var props = this.props,
          state = this.state;

      if (!props.vocabularyTerms) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("p", null, "Empty")
        );
      }

      var _vocabularyTerms = JSON.parse(JSON.stringify(props.vocabularyTerms));

      var vocabularyTerms = getVocabularyTermList(_vocabularyTerms, props.type);
      var terms = vocabularyTerms.map(function (term) {
        var children = _this8.getChildTerms(term, _this8.onEditTerm.bind(_this8), true);

        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(RightPane_Term, {
            term: term,
            onClick: _this8.onEditTerm.bind(_this8),
            isEditable: _this8.canEdit(),
            key: "term-" + term.TermId
          }, children)
        );
      });
      var parentTermTree = getVocabularyTermList(removeChildTerms(_vocabularyTerms).filter(function (term) {
        return term.TermId !== state.termBeingEdited.TermId;
      }), props.type, state.termBeingEdited.TermId);
      parentTermTree = parentTermTree.map(function (term) {
        var children = _this8.getChildTerms(term, _this8.onSelectParent.bind(_this8), false);

        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(RightPane_Term, {
            term: term,
            onClick: _this8.onSelectParent.bind(_this8),
            isEditable: false,
            key: "term-" + term.TermId
          }, children)
        );
      });
      var parentDisplay = props.vocabularyTerms.find(function (term) {
        return term.TermId === state.termBeingEdited.ParentTermId;
      });
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: RightPane_style_default.a.vocabulariesRightPane
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "term-list"
        }, state.editBoxOpened &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(RightPane_AddTermBox, {
          isOpened: state._editBoxOpened,
          editMode: state.editMode,
          error: state.triedToSubmitTerm && state.nameError,
          termBeingEdited: state.termBeingEdited,
          termTreeVisible: props.type === "Hierarchy" && vocabularyTerms.length > 0 && (!state.editMode || state.termBeingEdited.ParentTermId > 0),
          parentDisplay: parentDisplay,
          parentTermTree: parentTermTree,
          parentTreeOpened: state.parentTreeOpened,
          toggleParentTree: this.toggleParentTree.bind(this),
          onTermValueChange: this.onTermValueChange.bind(this),
          deleteTerm: this.deleteTerm.bind(this),
          closeAddTerm: this.closeAddTerm.bind(this),
          onUpdateTerm: this.onUpdateTerm.bind(this)
        }), !state._editBoxOpened &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "term-list-content " + (!this.state.editBoxOpened ? "open" : "closed")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          className: "term-list-label"
        }, resources.get("Terms") + " (" + props.totalTermCount + ")"), this.canEdit() &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "add-term-button do-not-close",
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].AddIcon + " " + resources.get("AddTerm")
          },
          onClick: this.openAddTerm.bind(this, false, props.type)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: {
            width: "345px",
            height: "300px",
            border: "1px solid #DBDBDB",
            marginTop: 10
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("ul", {
          className: "term-ul root-level term-list-level " + props.type
        }, terms)))))
      );
    }
  }]);

  return RightPane;
}(external_window_dnn_nodeModules_React_["Component"]);

RightPane_RightPane.propTypes = {
  vocabularyTerms: prop_types_default.a.array,
  totalTermCount: prop_types_default.a.number,
  type: prop_types_default.a.string,
  scopeType: prop_types_default.a.string,
  index: prop_types_default.a.number,
  selectParentTerm: prop_types_default.a.func
};
/* harmony default export */ var VocabularyList_RightPane = (RightPane_RightPane);
// EXTERNAL MODULE: ./src/components/VocabularyList/style.less
var VocabularyList_style = __webpack_require__(36);

// CONCATENATED MODULE: ./src/components/VocabularyList/index.jsx
function VocabularyList_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { VocabularyList_typeof = function _typeof(obj) { return typeof obj; }; } else { VocabularyList_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return VocabularyList_typeof(obj); }

function VocabularyList_extends() { VocabularyList_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return VocabularyList_extends.apply(this, arguments); }

function VocabularyList_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function VocabularyList_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function VocabularyList_createClass(Constructor, protoProps, staticProps) { if (protoProps) VocabularyList_defineProperties(Constructor.prototype, protoProps); if (staticProps) VocabularyList_defineProperties(Constructor, staticProps); return Constructor; }

function VocabularyList_possibleConstructorReturn(self, call) { if (call && (VocabularyList_typeof(call) === "object" || typeof call === "function")) { return call; } return VocabularyList_assertThisInitialized(self); }

function VocabularyList_getPrototypeOf(o) { VocabularyList_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return VocabularyList_getPrototypeOf(o); }

function VocabularyList_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function VocabularyList_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) VocabularyList_setPrototypeOf(subClass, superClass); }

function VocabularyList_setPrototypeOf(o, p) { VocabularyList_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return VocabularyList_setPrototypeOf(o, p); }












var loadMoreButtonStyle = {
  display: "block",
  margin: "20px auto"
};

function getTermUpdates(term) {
  return {
    TermId: term.TermId,
    Name: term.Name,
    Description: term.Description,
    VocabularyId: term.VocabularyId,
    ParentTermId: term.ParentTermId
  };
}

function getRootParentTerm(termId, term, termList) {
  while (term.TermId !== termId && term.ParentTermId > -1) {
    term = termList.find(function (_term) {
      return _term.TermId === term.ParentTermId;
    });
  }

  return term;
}

function removeTermFromList(termId, vocabularyTermList) {
  return vocabularyTermList.filter(function (term) {
    var rootParentTerm = getRootParentTerm(termId, term, vocabularyTermList);
    return rootParentTerm.TermId !== termId;
  });
}

var VocabularyList_VocabularyListComponent =
/*#__PURE__*/
function (_Component) {
  VocabularyList_inherits(VocabularyListComponent, _Component);

  function VocabularyListComponent() {
    var _this;

    VocabularyList_classCallCheck(this, VocabularyListComponent);

    _this = VocabularyList_possibleConstructorReturn(this, VocabularyList_getPrototypeOf(VocabularyListComponent).call(this));
    _this.onDescriptionUpdate = _this.onDescriptionUpdate.bind(VocabularyList_assertThisInitialized(_this));
    _this.state = {
      vocabularyList: [],
      isOpened: false
    };
    return _this;
  }

  VocabularyList_createClass(VocabularyListComponent, [{
    key: "getNextPage",
    value: function getNextPage(pageIndex, pageSize, scopeTypeId) {
      return {
        pageIndex: pageIndex || 0,
        pageSize: pageSize,
        scopeTypeId: scopeTypeId || 0
      };
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.dispatch(actions_vocabulary.getVocabularyList(this.getNextPage(props.pagination.pageIndex, props.pagination.pageSize, props.pagination.scopeTypeId)));
    }
  }, {
    key: "getVocabularyTerms",
    value: function getVocabularyTerms(vocabularyId, index) {
      var props = this.props;
      props.dispatch(vocabularyTermList.getVocabularyTerms(vocabularyId, index));
    }
  }, {
    key: "updateVocabulary",
    value: function updateVocabulary(vocabularyList, key, index, value) {
      var newValue = VocabularyList_extends({}, vocabularyList[index]);

      newValue[key] = value;
      return newValue;
    }
  }, {
    key: "onUpdateTerm",
    value: function onUpdateTerm(termBeingEdited, editMode, callback) {
      var props = this.props;

      if (editMode) {
        var index = props.vocabularyTerms.findIndex(function (term) {
          return term.TermId === termBeingEdited.TermId;
        });
        props.dispatch(vocabularyTermList.updateVocabularyTerm(getTermUpdates(termBeingEdited), index, callback));
      } else {
        var totalTermCount = props.totalTermCount;
        props.dispatch(vocabularyTermList.addVocabularyTerm(getTermUpdates(termBeingEdited), ++totalTermCount, callback));
      }
    }
  }, {
    key: "onDescriptionUpdate",
    value: function onDescriptionUpdate(key, value, index) {
      var props = this.props;
      var vocabularyList = props.vocabularyList;
      var newValue = this.updateVocabulary(vocabularyList, key, index, value);
      props.dispatch(actions_vocabulary.updateVocabulary(newValue, index));
    }
  }, {
    key: "onLoadMore",
    value: function onLoadMore(event) {
      event.preventDefault();
      var props = this.props;
      var pageIndex = props.pagination.pageIndex; //copy page index

      var nextPage = this.getNextPage(++pageIndex, props.pagination.pageSize, props.pagination.scopeTypeId);
      props.dispatch(pagination.loadMore(nextPage));
    }
  }, {
    key: "handleSelect",
    value: function handleSelect(index
    /*,last*/
    ) {
      var props = this.props;
      props.dispatch(pagination.loadTab(this.getNextPage(0, props.pagination.pageSize, index))); //index acts as scopeTypeId
    }
  }, {
    key: "onDeleteVocabulary",
    value: function onDeleteVocabulary(vocabulary, index, callback) {
      var _this2 = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(resources.get("ConfirmDeletion_Vocabulary").replace("{0}", vocabulary.Name), //confirm delete message
      resources.get("DeleteVocabulary"), //confirm delete text
      resources.get("cancelCreate"), //cancel text
      function () {
        var totalCount = props.totalCount;

        if (callback) {
          callback();
        }

        props.dispatch(actions_vocabulary.deleteVocabulary(vocabulary.VocabularyId, index, --totalCount, function () {
          if (props.vocabularyList.length < props.totalCount) {
            var pageIndex = props.pagination.pageIndex; //copy page index

            var currentPage = _this2.getNextPage(pageIndex, props.pagination.pageSize, props.pagination.scopeTypeId);

            props.dispatch(pagination.loadMore(currentPage));
          }
        }));
      });
    }
  }, {
    key: "onDeleteTerm",
    value: function onDeleteTerm(termId) {
      var props = this.props;
      var vocabularyTerms = removeTermFromList(termId, props.vocabularyTerms);
      props.dispatch(vocabularyTermList.deleteVocabularyTerm(termId, vocabularyTerms, vocabularyTerms.length));
    }
  }, {
    key: "selectParentTerm",
    value: function selectParentTerm(term) {
      var props = this.props;
      var vocabularyTerms = JSON.parse(JSON.stringify(props.vocabularyTerms));
      var previousTerms = vocabularyTerms.map(function (_term) {
        delete _term.selected;
        return _term;
      });
      var index = props.vocabularyTerms.findIndex(function (_term) {
        return _term.TermId === term.TermId;
      });
      term.selected = true;
      props.dispatch(vocabularyTermList.clearSelected(previousTerms));
      props.dispatch(vocabularyTermList.setTermSelected(term, index));
    }
  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      var vocabularyList = this.props.vocabularyList;
      var props = this.props;
      var renderedVocabularyList = vocabularyList.map(function (term, index) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(VocabularyList_TermHeader, {
            header: term.Name,
            type: term.TypeId,
            term: term,
            index: index,
            key: "vocabularyTerm-" + index,
            closeOnClick: true,
            onDelete: _this3.onDeleteVocabulary.bind(_this3)
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], null,
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(VocabularyList_LeftPane, {
            description: term.Description,
            type: term.Type,
            onEnter: _this3.onDescriptionUpdate,
            index: index,
            scopeType: term.ScopeType
          }),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(VocabularyList_RightPane, {
            vocabularyId: term.VocabularyId,
            getVocabularyTerms: _this3.getVocabularyTerms.bind(_this3),
            onUpdateTerm: _this3.onUpdateTerm.bind(_this3),
            vocabularyTerms: props.vocabularyTerms,
            selectParentTerm: _this3.selectParentTerm.bind(_this3),
            totalTermCount: props.totalTermCount,
            index: index,
            type: term.Type,
            scopeType: term.ScopeType,
            onDeleteTerm: _this3.onDeleteTerm.bind(_this3),
            parentTerms: vocabularyList
          })))
        );
      });
      var loadMoreEnabled = props.vocabularyList.length < props.totalCount;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "vocabulary-list"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
          onSelect: this.handleSelect.bind(this),
          selectedIndex: props.tabIndex,
          tabHeaders: [resources.get("All"), resources.get("Application"), resources.get("Portal")],
          type: "secondary"
        }, renderedVocabularyList.length > 0 && renderedVocabularyList ||
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "vocabulary-error"
        }, resources.get("NoVocabularyTerms.Error")), renderedVocabularyList.length > 0 && renderedVocabularyList ||
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "vocabulary-error"
        }, resources.get("NoVocabularyTerms.Error")), renderedVocabularyList.length > 0 && renderedVocabularyList ||
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "vocabulary-error"
        }, resources.get("NoVocabularyTerms.Error")))), loadMoreEnabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          style: loadMoreButtonStyle,
          onClick: this.onLoadMore.bind(this)
        }, resources.get("LoadMore")))
      );
    }
  }]);

  return VocabularyListComponent;
}(external_window_dnn_nodeModules_React_["Component"]);

VocabularyList_VocabularyListComponent.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  vocabularyList: prop_types_default.a.array,
  totalCount: prop_types_default.a.number,
  totalTermCount: prop_types_default.a.number,
  vocabularyTerms: prop_types_default.a.array,
  pagination: prop_types_default.a.object,
  tabIndex: prop_types_default.a.number,
  scopeTypeId: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.number])
};

function mapStateToProps(state) {
  return {
    vocabularyList: state.vocabulary.vocabularyList,
    totalCount: state.vocabulary.totalCount,
    totalTermCount: state.vocabularyTermList.totalCount,
    vocabularyTerms: state.vocabularyTermList.vocabularyTerms,
    pagination: state.pagination,
    tabIndex: state.pagination.tabIndex,
    scopeTypeId: state.pagination.scopeTypeId
  };
}

/* harmony default export */ var VocabularyList = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(mapStateToProps)(VocabularyList_VocabularyListComponent));
// EXTERNAL MODULE: ./src/components/CreateVocabulary/style.less
var CreateVocabulary_style = __webpack_require__(23);
var CreateVocabulary_style_default = /*#__PURE__*/__webpack_require__.n(CreateVocabulary_style);

// CONCATENATED MODULE: ./src/components/CreateVocabulary/index.jsx
function CreateVocabulary_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { CreateVocabulary_typeof = function _typeof(obj) { return typeof obj; }; } else { CreateVocabulary_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return CreateVocabulary_typeof(obj); }

function CreateVocabulary_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function CreateVocabulary_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function CreateVocabulary_createClass(Constructor, protoProps, staticProps) { if (protoProps) CreateVocabulary_defineProperties(Constructor.prototype, protoProps); if (staticProps) CreateVocabulary_defineProperties(Constructor, staticProps); return Constructor; }

function CreateVocabulary_possibleConstructorReturn(self, call) { if (call && (CreateVocabulary_typeof(call) === "object" || typeof call === "function")) { return call; } return CreateVocabulary_assertThisInitialized(self); }

function CreateVocabulary_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function CreateVocabulary_getPrototypeOf(o) { CreateVocabulary_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return CreateVocabulary_getPrototypeOf(o); }

function CreateVocabulary_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) CreateVocabulary_setPrototypeOf(subClass, superClass); }

function CreateVocabulary_setPrototypeOf(o, p) { CreateVocabulary_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return CreateVocabulary_setPrototypeOf(o, p); }










var CreateVocabulary_CreateVocabulary =
/*#__PURE__*/
function (_Component) {
  CreateVocabulary_inherits(CreateVocabulary, _Component);

  function CreateVocabulary() {
    var _this;

    CreateVocabulary_classCallCheck(this, CreateVocabulary);

    _this = CreateVocabulary_possibleConstructorReturn(this, CreateVocabulary_getPrototypeOf(CreateVocabulary).call(this));
    _this.state = {
      term: {
        Name: "",
        Description: "",
        TypeId: 1,
        ScopeTypeId: 2
      },
      error: {
        name: true
      },
      triedToSubmit: false
    };
    return _this;
  }

  CreateVocabulary_createClass(CreateVocabulary, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.setState({
        term: {
          Name: "",
          Description: "",
          TypeId: 1,
          ScopeTypeId: 2
        }
      });
    }
  }, {
    key: "onCloseVocabulary",
    value: function onCloseVocabulary() {
      var props = this.props;
      props.onCloseVocabulary();
    }
  }, {
    key: "onTermValueChange",
    value: function onTermValueChange(key, event) {
      var value = CreateVocabulary_typeof(event) === "object" ? event.target.value : parseInt(event); //event is value

      var state = this.state;
      var term = this.state.term;
      term[key] = value;
      state.triedToSubmit = false;

      if (value === "" && key === "Name") {
        state.error["name"] = true;
      } else if (value !== "" && key === "Name") {
        state.error["name"] = false;
      }

      this.setState({
        state: state,
        term: term
      });
    }
  }, {
    key: "getNextPage",
    value: function getNextPage(pageIndex, pageSize, scopeTypeId) {
      return {
        pageIndex: pageIndex || 0,
        pageSize: pageSize,
        scopeTypeId: scopeTypeId || 0
      };
    }
  }, {
    key: "onAddNewVocabulary",
    value: function onAddNewVocabulary(event) {
      var _this2 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.name) {
        return;
      }

      var totalCount = props.totalCount;
      props.dispatch(actions_vocabulary.addVocabulary(state.term, ++totalCount, function () {
        _this2.onCloseVocabulary();

        props.dispatch(pagination.loadTab(_this2.getNextPage(0, 10000, state.term.ScopeTypeId))); //index acts as scopeTypeId 
      }));
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var typeOptions = [{
        label: resources.get("Simple"),
        value: 1
      }, {
        label: resources.get("Hierarchy"),
        value: 2
      }];
      var scopeOptions = [{
        label: resources.get("Portal"),
        value: 2
      }, {
        label: resources.get("Application"),
        value: 1
      }];
      return (
        /*#__PURE__*/
        //inline style for height to allow proper calculating
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: resources.get("BackToVocabularies"),
            onClick: this.onCloseVocabulary.bind(this)
          },
          className: CreateVocabulary_style_default.a.createVocabulary,
          style: {
            height: "calc(100% - 100px)"
          }
        }, props.isOpen &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "create-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          inputId: "create-vocabulary-name",
          withLabel: true,
          label: resources.get("TermName") + "*",
          error: state.error.name && state.triedToSubmit,
          errorMessage: resources.get("TermValidationError.Message"),
          value: state.term.Name,
          onChange: this.onTermValueChange.bind(this, "Name")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["InputGroup"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          inputId: "create-vocabulary-description",
          withLabel: true,
          label: resources.get("Description"),
          value: state.term.Description,
          onChange: this.onTermValueChange.bind(this, "Description")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["RadioButtons"], {
          onChange: this.onTermValueChange.bind(this, "TypeId"),
          options: typeOptions,
          label: resources.get("Type.Header") + ":",
          buttonGroup: "vocabularyType",
          buttonWidth: 130,
          value: state.term.TypeId
        }), utils["a" /* default */].isHost() &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["RadioButtons"], {
          onChange: this.onTermValueChange.bind(this, "ScopeTypeId"),
          options: scopeOptions,
          label: resources.get("Scope.Header") + ":",
          buttonGroup: "scopeType",
          buttonWidth: 130,
          value: state.term.ScopeTypeId
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "action-buttons"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onCloseVocabulary.bind(this)
        }, resources.get("cancelCreate")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onAddNewVocabulary.bind(this)
        }, resources.get("CreateVocabulary")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          className: "required-help-text"
        }, "* ", resources.get("RequiredField")))))
      );
    }
  }]);

  return CreateVocabulary;
}(external_window_dnn_nodeModules_React_["Component"]);

CreateVocabulary_CreateVocabulary.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  totalCount: prop_types_default.a.number,
  onCloseVocabulary: prop_types_default.a.func,
  isOpen: prop_types_default.a.bool
};

function CreateVocabulary_mapStateToProps(state) {
  return {
    totalCount: state.vocabulary.totalCount,
    vocabularyAddedIsValid: state.vocabulary.vocabularyAddedIsValid,
    pagination: state.pagination
  };
}

/* harmony default export */ var components_CreateVocabulary = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(CreateVocabulary_mapStateToProps)(CreateVocabulary_CreateVocabulary));
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
    key: "openCreateVocabulary",
    value: function openCreateVocabulary() {
      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(1));
    }
  }, {
    key: "closeCreateVocabulary",
    value: function closeCreateVocabulary() {
      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(0));
    }
  }, {
    key: "navigateMap",
    value: function navigateMap(page, index, event) {
      event.preventDefault();
      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(page, index));
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "taxonomy-app"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 0
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: resources.get("ControlTitle_")
        }, utils["a" /* default */].canEdit() &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          size: "large",
          onClick: this.openCreateVocabulary.bind(this)
        }, resources.get("Create"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(VocabularyList, null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 1
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: resources.get("Create"),
          onCreateVocabulary: this.openCreateVocabulary.bind(this)
        }), props.selectedPage === 1 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_CreateVocabulary, {
          onCloseVocabulary: this.closeCreateVocabulary.bind(this),
          isOpen: props.selectedPage === 1
        })))
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
          className: "taxonomy-Root"
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
/* 40 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactDOM"
var external_window_dnn_nodeModules_ReactDOM_ = __webpack_require__(12);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(7);

// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/globals/taxonomy.js

var vocabularies = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;        
    var options = window.dnn.initVocabularies();
    utils["a" /* default */].init(options); // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(24);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
/* harmony default export */ var taxonomy = (vocabularies);
// EXTERNAL MODULE: external "window.dnn.nodeModules.Redux"
var external_window_dnn_nodeModules_Redux_ = __webpack_require__(8);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxThunk"
var external_window_dnn_nodeModules_ReduxThunk_ = __webpack_require__(11);
var external_window_dnn_nodeModules_ReduxThunk_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxThunk_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxImmutableStateInvariant"
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_ = __webpack_require__(13);
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxImmutableStateInvariant_);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 4 modules
var actionTypes = __webpack_require__(3);

// CONCATENATED MODULE: ./src/reducers/vocabularyReducer.js
function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }



function updateVocabularyList(vocabularyList, index, value) {
  return [].concat(_toConsumableArray(vocabularyList.slice(0, index)), [value], _toConsumableArray(vocabularyList.slice(index + 1)));
}

function insertRecords(vocabularyList, addedList) {
  var newVocabularyList = vocabularyList;
  addedList.forEach(function (vocabulary) {
    var alreadyThere = vocabularyList.find(function (_vocabulary) {
      return _vocabulary.VocabularyId === vocabulary.VocabularyId;
    });

    if (!alreadyThere) {
      newVocabularyList.push(vocabulary);
    }
  });
  return newVocabularyList;
}

function vocabularyList() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    vocabularyList: [],
    vocabularyTerms: [],
    totalTermCount: 0,
    totalCount: 0,
    vocabularyAddedIsValid: true
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["a" /* pagination */].LOAD_MORE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyList: insertRecords(JSON.parse(JSON.stringify(state.vocabularyList)), action.data.vocabularyList),
        totalCount: action.data.totalCount
      });

    case actionTypes["a" /* pagination */].LOAD_TAB_DATA:
    case actionTypes["c" /* vocabulary */].RETRIEVED_VOCABULARY_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyList: action.data.vocabularyList,
        totalCount: action.data.totalCount
      });

    case actionTypes["c" /* vocabulary */].UPDATED_VOCABULARY:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyList: updateVocabularyList(state.vocabularyList, action.data.index, action.data.updatedTerm)
      });

    case actionTypes["c" /* vocabulary */].UPDATED_VOCABULARY_TERM:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyTerms: [].concat(_toConsumableArray(state.vocabularyTerms.slice(0, action.payload.index)), [action.payload.updatedTerm], _toConsumableArray(state.vocabularyTerms.slice(action.payload.index + 1)))
      });

    case actionTypes["c" /* vocabulary */].ADDED_VOCABULARY_TERM:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyTerms: [].concat(_toConsumableArray(state.vocabularyTerms), [action.payload.addedTerm]),
        totalTermCount: action.payload.totalCount
      });

    case actionTypes["c" /* vocabulary */].ADDED_VOCABULARY:
      return {
        vocabularyAddedIsValid: true,
        vocabularyList: state.vocabularyList.concat(action.payload.addedVocabulary),
        totalCount: action.payload.totalCount
      };

    case actionTypes["c" /* vocabulary */].RETRIEVED_VOCABULARY_TERMS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyTerms: action.data.vocabularyTerms,
        totalTermCount: action.data.totalCount
      });

    case actionTypes["c" /* vocabulary */].DELETED_VOCABULARY:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyList: [].concat(_toConsumableArray(state.vocabularyList.slice(0, action.payload.index)), _toConsumableArray(state.vocabularyList.slice(action.payload.index + 1))),
        totalCount: action.payload.totalCount
      });

    case actionTypes["c" /* vocabulary */].FAILED_TO_ADD_VOCABULARY:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        vocabularyAddedIsValid: false
      });

    default:
      return _objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/vocabularyTermListReducer.js
function vocabularyTermListReducer_toConsumableArray(arr) { return vocabularyTermListReducer_arrayWithoutHoles(arr) || vocabularyTermListReducer_iterableToArray(arr) || vocabularyTermListReducer_nonIterableSpread(); }

function vocabularyTermListReducer_nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function vocabularyTermListReducer_iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function vocabularyTermListReducer_arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

function vocabularyTermListReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { vocabularyTermListReducer_defineProperty(target, key, source[key]); }); } return target; }

function vocabularyTermListReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function vocabularyTermList() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    vocabularyTerms: [],
    totalCount: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["d" /* vocabularyTermList */].UPDATED_VOCABULARY_TERM:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: [].concat(vocabularyTermListReducer_toConsumableArray(state.vocabularyTerms.slice(0, action.payload.index)), [action.payload.updatedTerm], vocabularyTermListReducer_toConsumableArray(state.vocabularyTerms.slice(action.payload.index + 1)))
      });

    case actionTypes["d" /* vocabularyTermList */].ADDED_VOCABULARY_TERM:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: [].concat(vocabularyTermListReducer_toConsumableArray(state.vocabularyTerms), [action.payload.addedTerm]),
        totalCount: action.payload.totalCount
      });

    case actionTypes["d" /* vocabularyTermList */].RETRIEVED_VOCABULARY_TERMS:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: action.data.vocabularyTerms,
        totalCount: action.data.totalCount
      });

    case actionTypes["d" /* vocabularyTermList */].DELETED_VOCABULARY_TERM:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: action.payload.vocabularyTerms,
        totalCount: action.payload.totalCount
      });

    case actionTypes["d" /* vocabularyTermList */].SET_TERM_SELECTED:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: [].concat(vocabularyTermListReducer_toConsumableArray(state.vocabularyTerms.slice(0, action.payload.index)), [action.payload.updatedTerm], vocabularyTermListReducer_toConsumableArray(state.vocabularyTerms.slice(action.payload.index + 1)))
      });

    case actionTypes["d" /* vocabularyTermList */].CLEAR_TERMS_SELECTED:
      return vocabularyTermListReducer_objectSpread2(vocabularyTermListReducer_objectSpread2({}, state), {}, {
        vocabularyTerms: action.payload.vocabularyTerms
      });

    default:
      return vocabularyTermListReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/paginationReducer.js
function paginationReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { paginationReducer_defineProperty(target, key, source[key]); }); } return target; }

function paginationReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0,
    pageIndex: 0,
    scopeTypeId: "*",
    pageSize: 10
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["a" /* pagination */].LOAD_MORE:
      return paginationReducer_objectSpread2(paginationReducer_objectSpread2({}, state), {}, {
        pageIndex: action.payload.pageIndex,
        scopeTypeId: action.payload.scopeTypeId
      });

    case actionTypes["a" /* pagination */].LOAD_TAB_DATA:
      return paginationReducer_objectSpread2(paginationReducer_objectSpread2({}, state), {}, {
        pageIndex: 0,
        tabIndex: action.tabIndex,
        scopeTypeId: action.payload.scopeTypeId
      });

    default:
      return paginationReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/visiblePanelReducer.js
function visiblePanelReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { visiblePanelReducer_defineProperty(target, key, source[key]); }); } return target; }

function visiblePanelReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["b" /* visiblePanel */].SELECT_PANEL:
      return visiblePanelReducer_objectSpread2(visiblePanelReducer_objectSpread2({}, state), {}, {
        selectedPage: action.payload.selectedPage
      });

    default:
      return visiblePanelReducer_objectSpread2({}, state);
  }
}
// CONCATENATED MODULE: ./src/reducers/rootReducer.js





var rootReducer = Object(external_window_dnn_nodeModules_Redux_["combineReducers"])({
  vocabulary: vocabularyList,
  pagination: pagination,
  vocabularyTermList: vocabularyTermList,
  visiblePanel: visiblePanel
});
/* harmony default export */ var reducers_rootReducer = (rootReducer);
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevTools"
var external_window_dnn_nodeModules_ReduxDevTools_ = __webpack_require__(14);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsLogMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_ = __webpack_require__(15);
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsDockMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_ = __webpack_require__(16);
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
var Root = __webpack_require__(17);
var Root_default = /*#__PURE__*/__webpack_require__.n(Root);

// CONCATENATED MODULE: ./src/main.jsx






var main_store = configureStore();
taxonomy.dispatch = main_store.dispatch;
taxonomy.init();
var appContainer = document.getElementById("vocabularies-panel");
Object(external_window_dnn_nodeModules_ReactDOM_["render"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactRedux_["Provider"], {
  store: main_store
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(Root_default.a, null)), appContainer);

/***/ })
/******/ ]);
//# sourceMappingURL=vocabulary-bundle.js.map